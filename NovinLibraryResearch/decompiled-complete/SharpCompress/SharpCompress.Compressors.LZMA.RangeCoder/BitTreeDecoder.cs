namespace SharpCompress.Compressors.LZMA.RangeCoder;

internal struct BitTreeDecoder
{
	private readonly BitDecoder[] _models;

	private readonly int _numBitLevels;

	public BitTreeDecoder(int numBitLevels)
	{
		_numBitLevels = numBitLevels;
		_models = new BitDecoder[1 << numBitLevels];
	}

	public void Init()
	{
		for (uint num = 1u; num < 1 << _numBitLevels; num++)
		{
			_models[num].Init();
		}
	}

	public uint Decode(Decoder rangeDecoder)
	{
		uint num = 1u;
		for (int num2 = _numBitLevels; num2 > 0; num2--)
		{
			num = (num << 1) + _models[num].Decode(rangeDecoder);
		}
		return num - (uint)(1 << _numBitLevels);
	}

	public uint ReverseDecode(Decoder rangeDecoder)
	{
		uint num = 1u;
		uint num2 = 0u;
		for (int i = 0; i < _numBitLevels; i++)
		{
			uint num3 = _models[num].Decode(rangeDecoder);
			num <<= 1;
			num += num3;
			num2 |= num3 << i;
		}
		return num2;
	}

	public static uint ReverseDecode(BitDecoder[] models, uint startIndex, Decoder rangeDecoder, int numBitLevels)
	{
		uint num = 1u;
		uint num2 = 0u;
		for (int i = 0; i < numBitLevels; i++)
		{
			uint num3 = models[startIndex + num].Decode(rangeDecoder);
			num <<= 1;
			num += num3;
			num2 |= num3 << i;
		}
		return num2;
	}
}
