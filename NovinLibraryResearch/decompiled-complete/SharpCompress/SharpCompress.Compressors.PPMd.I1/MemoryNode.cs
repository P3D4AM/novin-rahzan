namespace SharpCompress.Compressors.PPMd.I1;

internal struct MemoryNode
{
	public uint _address;

	public byte[] _memory;

	public static readonly MemoryNode ZERO = new MemoryNode(0u, null);

	public const int SIZE = 12;

	public uint Stamp
	{
		get
		{
			return (uint)(_memory[_address] | (_memory[_address + 1] << 8) | (_memory[_address + 2] << 16) | (_memory[_address + 3] << 24));
		}
		set
		{
			_memory[_address] = (byte)value;
			_memory[_address + 1] = (byte)(value >> 8);
			_memory[_address + 2] = (byte)(value >> 16);
			_memory[_address + 3] = (byte)(value >> 24);
		}
	}

	public MemoryNode Next
	{
		get
		{
			return new MemoryNode((uint)(_memory[_address + 4] | (_memory[_address + 5] << 8) | (_memory[_address + 6] << 16) | (_memory[_address + 7] << 24)), _memory);
		}
		set
		{
			_memory[_address + 4] = (byte)value._address;
			_memory[_address + 5] = (byte)(value._address >> 8);
			_memory[_address + 6] = (byte)(value._address >> 16);
			_memory[_address + 7] = (byte)(value._address >> 24);
		}
	}

	public uint UnitCount
	{
		get
		{
			return (uint)(_memory[_address + 8] | (_memory[_address + 9] << 8) | (_memory[_address + 10] << 16) | (_memory[_address + 11] << 24));
		}
		set
		{
			_memory[_address + 8] = (byte)value;
			_memory[_address + 9] = (byte)(value >> 8);
			_memory[_address + 10] = (byte)(value >> 16);
			_memory[_address + 11] = (byte)(value >> 24);
		}
	}

	public bool Available => Next._address != 0;

	public MemoryNode(uint address, byte[] memory)
	{
		_address = address;
		_memory = memory;
	}

	public void Link(MemoryNode memoryNode)
	{
		memoryNode.Next = Next;
		Next = memoryNode;
	}

	public void Unlink()
	{
		Next = Next.Next;
	}

	public void Insert(MemoryNode memoryNode, uint unitCount)
	{
		Link(memoryNode);
		memoryNode.Stamp = uint.MaxValue;
		memoryNode.UnitCount = unitCount;
		Stamp++;
	}

	public MemoryNode Remove()
	{
		MemoryNode next = Next;
		Unlink();
		Stamp--;
		return next;
	}

	public static implicit operator MemoryNode(Pointer pointer)
	{
		return new MemoryNode(pointer._address, pointer._memory);
	}

	public static MemoryNode operator +(MemoryNode memoryNode, int offset)
	{
		memoryNode._address = (uint)(memoryNode._address + offset * 12);
		return memoryNode;
	}

	public static MemoryNode operator +(MemoryNode memoryNode, uint offset)
	{
		memoryNode._address += offset * 12;
		return memoryNode;
	}

	public static MemoryNode operator -(MemoryNode memoryNode, int offset)
	{
		memoryNode._address = (uint)(memoryNode._address - offset * 12);
		return memoryNode;
	}

	public static MemoryNode operator -(MemoryNode memoryNode, uint offset)
	{
		memoryNode._address -= offset * 12;
		return memoryNode;
	}

	public static bool operator ==(MemoryNode memoryNode1, MemoryNode memoryNode2)
	{
		return memoryNode1._address == memoryNode2._address;
	}

	public static bool operator !=(MemoryNode memoryNode1, MemoryNode memoryNode2)
	{
		return memoryNode1._address != memoryNode2._address;
	}

	public override bool Equals(object obj)
	{
		if (obj is MemoryNode)
		{
			return ((MemoryNode)obj)._address == _address;
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return _address.GetHashCode();
	}
}
