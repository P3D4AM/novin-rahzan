using System;
using System.Buffers.Binary;
using SharpCompress.Compressors.PPMd.I1;

namespace SharpCompress.Compressors.PPMd;

public class PpmdProperties
{
	private int _allocatorSize;

	internal Allocator? _allocator;

	public int ModelOrder { get; }

	public PpmdVersion Version { get; } = PpmdVersion.I1;


	internal ModelRestorationMethod RestorationMethod { get; }

	public int AllocatorSize
	{
		get
		{
			return _allocatorSize;
		}
		set
		{
			_allocatorSize = value;
			if (Version == PpmdVersion.I1)
			{
				if (_allocator == null)
				{
					_allocator = new Allocator();
				}
				_allocator.Start(_allocatorSize);
			}
		}
	}

	public byte[] Properties
	{
		get
		{
			byte[] array = new byte[2];
			BinaryPrimitives.WriteUInt16LittleEndian(array, (ushort)(ModelOrder - 1 + ((AllocatorSize >> 20) - 1 << 4) + ((ushort)RestorationMethod << 12)));
			return array;
		}
	}

	public PpmdProperties()
		: this(16777216, 6)
	{
	}

	public PpmdProperties(int allocatorSize, int modelOrder)
		: this(allocatorSize, modelOrder, ModelRestorationMethod.Restart)
	{
	}

	internal PpmdProperties(int allocatorSize, int modelOrder, ModelRestorationMethod modelRestorationMethod)
	{
		AllocatorSize = allocatorSize;
		ModelOrder = modelOrder;
		RestorationMethod = modelRestorationMethod;
	}

	public PpmdProperties(byte[] properties)
		: this(MemoryExtensions.AsSpan(properties))
	{
	}

	public PpmdProperties(ReadOnlySpan<byte> properties)
	{
		if (properties.Length == 2)
		{
			ushort num = BinaryPrimitives.ReadUInt16LittleEndian(properties);
			AllocatorSize = ((num >> 4) & 0xFF) + 1 << 20;
			ModelOrder = (num & 0xF) + 1;
			RestorationMethod = (ModelRestorationMethod)(num >> 12);
		}
		else if (properties.Length == 5)
		{
			Version = PpmdVersion.H7Z;
			AllocatorSize = BinaryPrimitives.ReadInt32LittleEndian(properties.Slice(1));
			ModelOrder = properties[0];
		}
	}
}
