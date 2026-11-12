namespace SharpCompress.Compressors.PPMd.I1;

internal struct PpmState
{
	public uint _address;

	public byte[] _memory;

	public static readonly PpmState ZERO = new PpmState(0u, null);

	public const int SIZE = 6;

	public byte Symbol
	{
		get
		{
			return _memory[_address];
		}
		set
		{
			_memory[_address] = value;
		}
	}

	public byte Frequency
	{
		get
		{
			return _memory[_address + 1];
		}
		set
		{
			_memory[_address + 1] = value;
		}
	}

	public Model.PpmContext Successor
	{
		get
		{
			return new Model.PpmContext((uint)(_memory[_address + 2] | (_memory[_address + 3] << 8) | (_memory[_address + 4] << 16) | (_memory[_address + 5] << 24)), _memory);
		}
		set
		{
			_memory[_address + 2] = (byte)value._address;
			_memory[_address + 3] = (byte)(value._address >> 8);
			_memory[_address + 4] = (byte)(value._address >> 16);
			_memory[_address + 5] = (byte)(value._address >> 24);
		}
	}

	public PpmState this[int offset] => new PpmState((uint)(_address + offset * 6), _memory);

	public PpmState(uint address, byte[] memory)
	{
		_address = address;
		_memory = memory;
	}

	public static implicit operator PpmState(Pointer pointer)
	{
		return new PpmState(pointer._address, pointer._memory);
	}

	public static PpmState operator +(PpmState state, int offset)
	{
		state._address = (uint)(state._address + offset * 6);
		return state;
	}

	public static PpmState operator ++(PpmState state)
	{
		state._address += 6u;
		return state;
	}

	public static PpmState operator -(PpmState state, int offset)
	{
		state._address = (uint)(state._address - offset * 6);
		return state;
	}

	public static PpmState operator --(PpmState state)
	{
		state._address -= 6u;
		return state;
	}

	public static bool operator <=(PpmState state1, PpmState state2)
	{
		return state1._address <= state2._address;
	}

	public static bool operator >=(PpmState state1, PpmState state2)
	{
		return state1._address >= state2._address;
	}

	public static bool operator ==(PpmState state1, PpmState state2)
	{
		return state1._address == state2._address;
	}

	public static bool operator !=(PpmState state1, PpmState state2)
	{
		return state1._address != state2._address;
	}

	public override bool Equals(object obj)
	{
		if (obj is PpmState)
		{
			return ((PpmState)obj)._address == _address;
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return _address.GetHashCode();
	}
}
