namespace SharpCompress.Compressors.LZMA.RangeCoder;

internal struct BitTreeEncoder
{
	private readonly BitEncoder[] _models;

	private readonly int _numBitLevels;

	public BitTreeEncoder(int numBitLevels)
	{
		_numBitLevels = numBitLevels;
		_models = new BitEncoder[1 << numBitLevels];
	}

	public void Init()
	{
		for (uint num = 1u; num < 1 << _numBitLevels; num++)
		{
			_models[num].Init();
		}
	}

	public void Encode(Encoder rangeEncoder, uint symbol)
	{
		uint num = 1u;
		int num2 = _numBitLevels;
		while (num2 > 0)
		{
			num2--;
			uint num3 = (symbol >> num2) & 1u;
			_models[num].Encode(rangeEncoder, num3);
			num = (num << 1) | num3;
		}
	}

	public void ReverseEncode(Encoder rangeEncoder, uint symbol)
	{
		uint num = 1u;
		for (uint num2 = 0u; num2 < _numBitLevels; num2++)
		{
			uint num3 = symbol & 1u;
			_models[num].Encode(rangeEncoder, num3);
			num = (num << 1) | num3;
			symbol >>= 1;
		}
	}

	public uint GetPrice(uint symbol)
	{
		uint num = 0u;
		uint num2 = 1u;
		int num3 = _numBitLevels;
		while (num3 > 0)
		{
			num3--;
			uint num4 = (symbol >> num3) & 1u;
			num += _models[num2].GetPrice(num4);
			num2 = (num2 << 1) + num4;
		}
		return num;
	}

	public uint ReverseGetPrice(uint symbol)
	{
		uint num = 0u;
		uint num2 = 1u;
		for (int num3 = _numBitLevels; num3 > 0; num3--)
		{
			uint num4 = symbol & 1u;
			symbol >>= 1;
			num += _models[num2].GetPrice(num4);
			num2 = (num2 << 1) | num4;
		}
		return num;
	}

	public static uint ReverseGetPrice(BitEncoder[] models, uint startIndex, int numBitLevels, uint symbol)
	{
		uint num = 0u;
		uint num2 = 1u;
		for (int num3 = numBitLevels; num3 > 0; num3--)
		{
			uint num4 = symbol & 1u;
			symbol >>= 1;
			num += models[startIndex + num2].GetPrice(num4);
			num2 = (num2 << 1) | num4;
		}
		return num;
	}

	public static void ReverseEncode(BitEncoder[] models, uint startIndex, Encoder rangeEncoder, int numBitLevels, uint symbol)
	{
		uint num = 1u;
		for (int i = 0; i < numBitLevels; i++)
		{
			uint num2 = symbol & 1u;
			models[startIndex + num].Encode(rangeEncoder, num2);
			num = (num << 1) | num2;
			symbol >>= 1;
		}
	}
}
