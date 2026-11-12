namespace SharpCompress.Compressors.PPMd.I1;

internal struct Pointer
{
	public uint _address;

	public byte[] _memory;

	public static readonly Pointer ZERO = new Pointer(0u, null);

	public const int SIZE = 1;

	public byte this[int offset]
	{
		get
		{
			return _memory[_address + offset];
		}
		set
		{
			_memory[_address + offset] = value;
		}
	}

	public Pointer(uint address, byte[] memory)
	{
		_address = address;
		_memory = memory;
	}

	public static implicit operator Pointer(MemoryNode memoryNode)
	{
		return new Pointer(memoryNode._address, memoryNode._memory);
	}

	public static implicit operator Pointer(Model.PpmContext context)
	{
		return new Pointer(context._address, context._memory);
	}

	public static implicit operator Pointer(PpmState state)
	{
		return new Pointer(state._address, state._memory);
	}

	public static Pointer operator +(Pointer pointer, int offset)
	{
		pointer._address = (uint)(pointer._address + offset);
		return pointer;
	}

	public static Pointer operator +(Pointer pointer, uint offset)
	{
		pointer._address += offset;
		return pointer;
	}

	public static Pointer operator ++(Pointer pointer)
	{
		pointer._address++;
		return pointer;
	}

	public static Pointer operator -(Pointer pointer, int offset)
	{
		pointer._address = (uint)(pointer._address - offset);
		return pointer;
	}

	public static Pointer operator -(Pointer pointer, uint offset)
	{
		pointer._address -= offset;
		return pointer;
	}

	public static Pointer operator --(Pointer pointer)
	{
		pointer._address--;
		return pointer;
	}

	public static uint operator -(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address - pointer2._address;
	}

	public static bool operator <(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address < pointer2._address;
	}

	public static bool operator <=(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address <= pointer2._address;
	}

	public static bool operator >(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address > pointer2._address;
	}

	public static bool operator >=(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address >= pointer2._address;
	}

	public static bool operator ==(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address == pointer2._address;
	}

	public static bool operator !=(Pointer pointer1, Pointer pointer2)
	{
		return pointer1._address != pointer2._address;
	}

	public override bool Equals(object obj)
	{
		if (obj is Pointer)
		{
			return ((Pointer)obj)._address == _address;
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return _address.GetHashCode();
	}
}
