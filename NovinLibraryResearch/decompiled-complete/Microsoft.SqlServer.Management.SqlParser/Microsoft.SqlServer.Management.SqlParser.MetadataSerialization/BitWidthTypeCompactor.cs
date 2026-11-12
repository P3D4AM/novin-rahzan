using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class BitWidthTypeCompactor : TypeCompactor
{
	private class Node
	{
		private struct NodeValueArray
		{
			private readonly ReadOnlyArray<int> values;

			public int this[int index]
			{
				get
				{
					int num = values[index];
					int bitLength = SerializationUtils.GetBitLength(num);
					if (bitLength != 1)
					{
						return bitLength;
					}
					return num;
				}
			}

			public int Length => values.Length;

			public NodeValueArray(ReadOnlyArray<int> values)
			{
				this.values = values;
			}
		}

		private readonly int value;

		private Node children;

		private Node next;

		private PackedTypeBucket bucket;

		public static Node CreateRoot()
		{
			return new Node(0);
		}

		private Node(int value)
		{
			this.value = value;
		}

		public PackedTypeBucket GetOrCreateBucket(ReadOnlyArray<int> values, TypeDataTable dataTable, BucketIdGenerator idGenerator)
		{
			NodeValueArray nodeValues = new NodeValueArray(values);
			Node node = this;
			for (int i = 0; i < nodeValues.Length; i++)
			{
				int num = nodeValues[i];
				Node node2 = null;
				for (Node node3 = node.children; node3 != null; node3 = node3.next)
				{
					if (node3.value == num)
					{
						node2 = node3;
						break;
					}
				}
				if (node2 == null)
				{
					node = node.AddNewBranch(nodeValues, i);
					node.bucket = new PackedTypeBucket(dataTable, idGenerator.GetNextId());
					break;
				}
				node = node2;
			}
			return node.bucket;
		}

		private Node AddNewBranch(NodeValueArray nodeValues, int startIndex)
		{
			Node node = new Node(nodeValues[startIndex]);
			if (children == null)
			{
				children = node;
			}
			else if (children.value > node.value)
			{
				node.next = children;
				children = node;
			}
			else
			{
				Node node2 = children;
				while (node2.next != null && node2.next.value < node.value)
				{
					node2 = node2.next;
				}
				if (node2.next == null)
				{
					node2.next = node;
				}
				else
				{
					node.next = node2.next;
					node2.next = node;
				}
			}
			for (int i = startIndex + 1; i < nodeValues.Length; i++)
			{
				node.children = new Node(nodeValues[i]);
				node = node.children;
			}
			return node;
		}
	}

	private class PackedTypeBucket : TypeBucket
	{
		private readonly TypeDataTable dataTable;

		public PackedTypeBucket(TypeDataTable dataTable, int id)
			: base(dataTable.Name, id)
		{
			this.dataTable = dataTable;
		}

		public override void GenerateType(TypeGenerator typeGenerator, int rowId)
		{
			ReadOnlyArray<int> values = dataTable.GetRowById(rowId).Values;
			PackedField packedField = new PackedField();
			for (int i = 0; i < values.Length; i++)
			{
				MetadataPropertyInfo propertyInfo = dataTable.TypeInfo.InstanceProperties[i];
				int num = values[i];
				if (num == 0 || num == 1)
				{
					if (num == 1)
					{
						typeGenerator.AddFinalProperty(propertyInfo, num);
					}
				}
				else
				{
					PackedValue packedValue = packedField.AddValue(num);
					typeGenerator.AddIndirectProperty(propertyInfo, packedValue.FieldNumber, packedValue.StartOffset, packedValue.Length);
				}
			}
		}

		public override IEnumerable<uint> GetPackedValues(int rowId)
		{
			ReadOnlyArray<int> values = dataTable.GetRowById(rowId).Values;
			PackedField packedField = new PackedField();
			for (int i = 0; i < values.Length; i++)
			{
				int num = values[i];
				if (num > 1)
				{
					packedField.AddValue(num);
				}
			}
			return packedField.PackedValues;
		}
	}

	private struct PackedValue
	{
		private readonly byte fieldNumber;

		private readonly byte startOffset;

		private readonly byte length;

		private readonly bool hasValue;

		public bool IsNull => !hasValue;

		public int FieldNumber => fieldNumber;

		public int StartOffset => startOffset;

		public int Length => length;

		public PackedValue(int fieldNumber, uint startOffset, uint length)
		{
			this.fieldNumber = (byte)fieldNumber;
			this.startOffset = (byte)startOffset;
			this.length = (byte)length;
			hasValue = true;
		}
	}

	private class PackedField
	{
		private const uint MaxLength = 32u;

		private uint packedValues;

		private uint currentOffset;

		private PackedField next;

		private readonly int order;

		public int Count
		{
			get
			{
				int num = 1;
				for (PackedField packedField = next; packedField != null; packedField = packedField.next)
				{
					num++;
				}
				return num;
			}
		}

		public IEnumerable<uint> PackedValues
		{
			get
			{
				for (PackedField field = this; field != null; field = field.next)
				{
					yield return field.packedValues;
				}
			}
		}

		public PackedField()
			: this(0)
		{
		}

		private PackedField(int order)
		{
			this.order = order;
		}

		public PackedValue AddValue(int value)
		{
			uint bitLength = SerializationUtils.GetBitLength(value);
			if (currentOffset + bitLength <= 32)
			{
				PackedValue result = new PackedValue(order, currentOffset, bitLength);
				packedValues |= (uint)(value << (int)currentOffset);
				currentOffset += bitLength;
				return result;
			}
			if (next == null)
			{
				next = new PackedField(order + 1);
			}
			return next.AddValue(value);
		}
	}

	private const ulong InvalidKey = ulong.MaxValue;

	private readonly TypeDataTable dataTable;

	private readonly BucketIdGenerator idGenerator;

	private readonly Node rootNode;

	private int objectCount;

	public BitWidthTypeCompactor(TypeDataTable dataTable, BucketIdGenerator idGenerator)
	{
		this.dataTable = dataTable;
		this.idGenerator = idGenerator;
		rootNode = Node.CreateRoot();
	}

	public override TypeBucket RegisterDataRow(int rowId)
	{
		ReadOnlyArray<int> values = dataTable.GetRowById(rowId).Values;
		PackedTypeBucket orCreateBucket = rootNode.GetOrCreateBucket(values, dataTable, idGenerator);
		objectCount++;
		return orCreateBucket;
	}
}
