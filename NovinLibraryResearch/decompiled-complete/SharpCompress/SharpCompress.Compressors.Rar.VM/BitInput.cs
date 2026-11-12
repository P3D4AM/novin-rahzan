namespace SharpCompress.Compressors.Rar.VM;

internal class BitInput
{
	internal const int MAX_SIZE = 32768;

	public int inAddr;

	public int inBit;

	public bool ExternalBuffer;

	public int InAddr
	{
		get
		{
			return inAddr;
		}
		set
		{
			inAddr = value;
		}
	}

	public int InBit
	{
		get
		{
			return inBit;
		}
		set
		{
			inBit = value;
		}
	}

	internal byte[] InBuf { get; }

	internal BitInput()
	{
		InBuf = new byte[32768];
	}

	internal void InitBitInput()
	{
		inAddr = 0;
		inBit = 0;
	}

	internal void faddbits(uint bits)
	{
		AddBits((int)bits);
	}

	internal void AddBits(int bits)
	{
		bits += inBit;
		inAddr += bits >> 3;
		inBit = bits & 7;
	}

	internal uint fgetbits()
	{
		return (uint)GetBits();
	}

	internal uint getbits()
	{
		return (uint)GetBits();
	}

	internal int GetBits()
	{
		return Utility.URShift(((InBuf[inAddr] & 0xFF) << 16) + ((InBuf[inAddr + 1] & 0xFF) << 8) + (InBuf[inAddr + 2] & 0xFF), 8 - inBit) & 0xFFFF;
	}

	internal bool Overflow(int IncPtr)
	{
		return inAddr + IncPtr >= 32768;
	}
}
