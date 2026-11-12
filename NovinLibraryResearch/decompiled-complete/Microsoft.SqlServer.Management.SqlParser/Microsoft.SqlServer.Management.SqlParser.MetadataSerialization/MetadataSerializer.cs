using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class MetadataSerializer : ISerializationContext, ISerializationHandler<int>
{
	private class ValueTable<T> : ISequentialValueList<T>, IEnumerable<T>, IEnumerable
	{
		private const int InitialSize = 256;

		private readonly IEqualityComparer<T> comparer;

		private readonly Dictionary<T, int> valueIdMap;

		private readonly List<T> values;

		private int currentId;

		public int Count => values.Count;

		public ValueTable(IEqualityComparer<T> comparer)
		{
			this.comparer = comparer;
			valueIdMap = new Dictionary<T, int>(256, comparer);
			values = new List<T>(256);
			values.Add(default(T));
		}

		public int RegisterValue(T value)
		{
			int value2;
			if (comparer.Equals(value, default(T)))
			{
				value2 = 0;
			}
			else if (!valueIdMap.TryGetValue(value, out value2))
			{
				value2 = ++currentId;
				valueIdMap.Add(value, value2);
				values.Add(value);
			}
			return value2;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private readonly ISerializationFilter filter;

	private readonly SerializationVisitor serializationVisitor;

	private readonly Stack<IMetadataObject> workList;

	private readonly Dictionary<IMetadataObject, int> metadataIdMap;

	private readonly Dictionary<int, IMetadataObject> metadataExcIdMap;

	private readonly SortedDictionary<int, TypeDataTable> dataTables;

	private readonly ValueTable<byte[]> byteArrayTable;

	private readonly ValueTable<int[]> collectionIdTable;

	private readonly ValueTable<CollationInfo> collationInfoTable;

	private readonly ValueTable<ComputedColumnInfo> computedColumnInfoTable;

	private readonly ValueTable<DataTypeSpec> dataTypeSpecTable;

	private readonly ValueTable<double> doubleTable;

	private readonly ValueTable<GridDensity> gridDensityTable;

	private readonly ValueTable<IdentityColumnInfo> identityColumnInfoTable;

	private readonly ValueTable<IndexType> indexTypeTable;

	private readonly ValueTable<int> intTable;

	private readonly ValueTable<IPassword> passwordTable;

	private readonly ValueTable<string> stringTable;

	private readonly ValueTable<StatisticsType> statisticsTypeTable;

	private int currentId;

	private int rootId;

	int ISerializationContext.RootObjectId => rootId;

	int ISerializationContext.ObjectCount => currentId + 1;

	IEnumerable<TypeDataTable> ISerializationContext.DataTables => dataTables.Values;

	ISequentialValueList<byte[]> ISerializationContext.ByteArrayTable => byteArrayTable;

	ISequentialValueList<int[]> ISerializationContext.CollectionIdTable => collectionIdTable;

	ISequentialValueList<CollationInfo> ISerializationContext.CollationInfoTable => collationInfoTable;

	ISequentialValueList<ComputedColumnInfo> ISerializationContext.ComputedColumnInfoTable => computedColumnInfoTable;

	ISequentialValueList<DataTypeSpec> ISerializationContext.DataTypeSpecTable => dataTypeSpecTable;

	ISequentialValueList<double> ISerializationContext.DoubleTable => doubleTable;

	ISequentialValueList<GridDensity> ISerializationContext.GridDensityTable => gridDensityTable;

	ISequentialValueList<IdentityColumnInfo> ISerializationContext.IdentityColumnInfoTable => identityColumnInfoTable;

	ISequentialValueList<IndexType> ISerializationContext.IndexTypeTable => indexTypeTable;

	ISequentialValueList<int> ISerializationContext.IntTable => intTable;

	ISequentialValueList<IPassword> ISerializationContext.PasswordTable => passwordTable;

	ISequentialValueList<string> ISerializationContext.StringTable => stringTable;

	ISequentialValueList<StatisticsType> ISerializationContext.StatisticsTypeTable => statisticsTypeTable;

	private MetadataSerializer(ISerializationFilter filter)
	{
		this.filter = filter;
		serializationVisitor = new SerializationVisitor(SerializationOptions.ExcludeServerName | SerializationOptions.ExcludeText);
		workList = new Stack<IMetadataObject>(1024);
		metadataIdMap = new Dictionary<IMetadataObject, int>(4096);
		metadataExcIdMap = new Dictionary<int, IMetadataObject>(8);
		dataTables = new SortedDictionary<int, TypeDataTable>();
		byteArrayTable = new ValueTable<byte[]>(EqualityComparer.ByteArray);
		collectionIdTable = new ValueTable<int[]>(EqualityComparer.IntArray);
		collationInfoTable = new ValueTable<CollationInfo>(EqualityComparer.CollationInfo);
		computedColumnInfoTable = new ValueTable<ComputedColumnInfo>(EqualityComparer.ComputedColumnInfo);
		dataTypeSpecTable = new ValueTable<DataTypeSpec>(EqualityComparer.DataTypeSpec);
		doubleTable = new ValueTable<double>(EqualityComparer.Double);
		gridDensityTable = new ValueTable<GridDensity>(EqualityComparer.GridDensity);
		identityColumnInfoTable = new ValueTable<IdentityColumnInfo>(EqualityComparer.IdentityColumnInfo);
		indexTypeTable = new ValueTable<IndexType>(EqualityComparer.IndexType);
		intTable = new ValueTable<int>(EqualityComparer.Int);
		passwordTable = new ValueTable<IPassword>(EqualityComparer.Password);
		stringTable = new ValueTable<string>(EqualityComparer.String);
		statisticsTypeTable = new ValueTable<StatisticsType>(EqualityComparer.StatisticsType);
	}

	public static ISerializationContext Serialize(IServer server, ISerializationFilter filter)
	{
		MetadataSerializer metadataSerializer = new MetadataSerializer(filter);
		metadataSerializer.rootId = metadataSerializer.SerializeMetadataObject(server);
		return metadataSerializer;
	}

	private int SerializeMetadataObject(IMetadataObject metadataObject)
	{
		int metadataObjectId = GetMetadataObjectId(metadataObject);
		while (workList.Count > 0)
		{
			IMetadataObject metadataObject2 = workList.Pop();
			ProcessMetadataObjectDefinition(metadataObject2);
		}
		return metadataObjectId;
	}

	private int GetMetadataObjectId(IMetadataObject metadataObject)
	{
		int value;
		if (metadataObject == null)
		{
			value = 0;
		}
		else if (!metadataIdMap.TryGetValue(metadataObject, out value))
		{
			metadataObject.Accept(serializationVisitor);
			if (filter.IsIncluded(metadataObject))
			{
				value = ++currentId;
				metadataIdMap.Add(metadataObject, value);
				workList.Push(metadataObject);
			}
			else
			{
				value = ++currentId;
				metadataIdMap.Add(metadataObject, value);
				metadataExcIdMap.Add(value, metadataObject);
			}
		}
		return value;
	}

	private void ProcessMetadataObjectDefinition(IMetadataObject metadataObject)
	{
		int id = metadataIdMap[metadataObject];
		SerializationInfo serializationInfo = metadataObject.Accept(serializationVisitor);
		TypeDataTable dataTable = GetDataTable(serializationInfo.TypeInfo);
		int[] objectData = serializationInfo.GetObjectData(this, metadataObject);
		dataTable.AddRow(id, objectData);
	}

	private TypeDataTable GetDataTable(MetadataTypeInfo typeInfo)
	{
		if (!dataTables.TryGetValue(typeInfo.Id, out var value))
		{
			value = new TypeDataTable(typeInfo);
			dataTables.Add(typeInfo.Id, value);
		}
		return value;
	}

	public int RegisterValue(bool value)
	{
		if (!value)
		{
			return 0;
		}
		return 1;
	}

	public int RegisterValue(byte value)
	{
		return intTable.RegisterValue(value);
	}

	public int RegisterValue(byte[] value)
	{
		return 0;
	}

	public int RegisterValue(CollationInfo value)
	{
		return collationInfoTable.RegisterValue(value);
	}

	public int RegisterValue(ComputedColumnInfo value)
	{
		return computedColumnInfoTable.RegisterValue(value);
	}

	public int RegisterValue(double value)
	{
		return doubleTable.RegisterValue(value);
	}

	public int RegisterValue(DatabaseCompatibilityLevel value)
	{
		return (int)value;
	}

	public int RegisterValue(ActivationOrder value)
	{
		return (int)value;
	}

	public int RegisterValue(ExecutionContextType value)
	{
		return (int)value;
	}

	public int RegisterValue(ForeignKeyAction value)
	{
		return (int)value;
	}

	public int RegisterValue(GridDensity value)
	{
		return gridDensityTable.RegisterValue(value);
	}

	public int RegisterValue(IdentityColumnInfo value)
	{
		return identityColumnInfoTable.RegisterValue(value);
	}

	public int RegisterValue(DataTypeSpec value)
	{
		return dataTypeSpecTable.RegisterValue(value);
	}

	public int RegisterValue(int value)
	{
		return intTable.RegisterValue(value);
	}

	public int RegisterValue(DatabasePermissionType value)
	{
		return (int)value;
	}

	public int RegisterValue(PermissionState value)
	{
		return (int)value;
	}

	public int RegisterValue(SynonymBaseType value)
	{
		return (int)value;
	}

	public int RegisterValue(LoginType value)
	{
		return (int)value;
	}

	public int RegisterValue(IndexType value)
	{
		return indexTypeTable.RegisterValue(value);
	}

	public int RegisterValue(IPassword value)
	{
		return 0;
	}

	public int RegisterValue(SortOrder value)
	{
		return (int)value;
	}

	public int RegisterValue(StatisticsType value)
	{
		return statisticsTypeTable.RegisterValue(value);
	}

	public int RegisterValue(string value)
	{
		return stringTable.RegisterValue(value);
	}

	public int RegisterValue(TabularType value)
	{
		return (int)value;
	}

	public int RegisterValue(ITriggerEventTypeSet value)
	{
		return 0;
	}

	public int RegisterValue(UserType value)
	{
		return (int)value;
	}

	public int RegisterValue(IMetadataObject metadataObject, IMetadataObject parentObject, bool isChild)
	{
		return GetMetadataObjectId(metadataObject);
	}

	public int RegisterValue<T>(IMetadataCollection<T> collection, IMetadataObject parentObject, bool isChild) where T : IMetadataObject
	{
		if (typeof(IUdtMember).GetIsAssignableFrom(typeof(T)) || typeof(IUserDefinedAggregate).GetIsAssignableFrom(typeof(T)) || typeof(IUserDefinedClrType).GetIsAssignableFrom(typeof(T)))
		{
			return 0;
		}
		int[] array = null;
		if (collection.Count > 0)
		{
			array = new int[collection.Count];
			int num = 0;
			foreach (T item in collection)
			{
				if (filter.IsIncluded(item))
				{
					array[num++] = GetMetadataObjectId(item);
				}
			}
			if (num < array.Length)
			{
				if (num > 0)
				{
					int[] array2 = new int[num];
					Array.Copy(array, array2, num);
					array = array2;
				}
				else
				{
					array = null;
				}
			}
		}
		if (array == null)
		{
			return 0;
		}
		return collectionIdTable.RegisterValue(array);
	}
}
