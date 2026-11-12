namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal class BitInput
{
	public const int MAX_SIZE = 32768;

	public int InAddr;

	public int InBit;

	public bool ExternalBuffer;

	public byte[] InBuf;

	public BitInput(bool AllocBuffer)
	{
		ExternalBuffer = false;
		if (AllocBuffer)
		{
			uint num = 32771u;
			InBuf = new byte[num];
		}
		else
		{
			InBuf = null;
		}
	}

	public void faddbits(uint Bits)
	{
		addbits(Bits);
	}

	public uint fgetbits()
	{
		return getbits();
	}

	private void SetExternalBuffer(byte[] Buf)
	{
		InBuf = Buf;
		ExternalBuffer = true;
	}

	public void InitBitInput()
	{
		InAddr = (InBit = 0);
	}

	public void addbits(uint _Bits)
	{
		int num = checked((int)_Bits);
		num += InBit;
		InAddr += num >> 3;
		InBit = num & 7;
	}

	public uint getbits()
	{
		return ((uint)((InBuf[InAddr] << 16) | (InBuf[InAddr + 1] << 8) | InBuf[InAddr + 2]) >> 8 - InBit) & 0xFFFFu;
	}

	public uint getbits32()
	{
		return ((uint)(((InBuf[InAddr] << 24) | (InBuf[InAddr + 1] << 16) | (InBuf[InAddr + 2] << 8) | InBuf[InAddr + 3]) << InBit) | ((uint)InBuf[InAddr + 4] >> 8 - InBit)) & 0xFFFFFFFFu;
	}

	private bool Overflow(uint IncPtr)
	{
		return InAddr + IncPtr >= 32768;
	}
}
