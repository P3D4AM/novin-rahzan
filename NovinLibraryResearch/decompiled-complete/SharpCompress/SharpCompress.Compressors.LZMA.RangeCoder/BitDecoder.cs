namespace SharpCompress.Compressors.LZMA.RangeCoder;

internal struct BitDecoder
{
	public const int K_NUM_BIT_MODEL_TOTAL_BITS = 11;

	public const uint K_BIT_MODEL_TOTAL = 2048u;

	private const int K_NUM_MOVE_BITS = 5;

	private uint _prob;

	public void UpdateModel(int numMoveBits, uint symbol)
	{
		if (symbol == 0)
		{
			_prob += 2048 - _prob >> numMoveBits;
		}
		else
		{
			_prob -= _prob >> numMoveBits;
		}
	}

	public void Init()
	{
		_prob = 1024u;
	}

	public uint Decode(Decoder rangeDecoder)
	{
		uint num = (rangeDecoder._range >> 11) * _prob;
		if (rangeDecoder._code < num)
		{
			rangeDecoder._range = num;
			_prob += 2048 - _prob >> 5;
			if (rangeDecoder._range < 16777216)
			{
				rangeDecoder._code = (rangeDecoder._code << 8) | (byte)rangeDecoder._stream.ReadByte();
				rangeDecoder._range <<= 8;
				rangeDecoder._total++;
			}
			return 0u;
		}
		rangeDecoder._range -= num;
		rangeDecoder._code -= num;
		_prob -= _prob >> 5;
		if (rangeDecoder._range < 16777216)
		{
			rangeDecoder._code = (rangeDecoder._code << 8) | (byte)rangeDecoder._stream.ReadByte();
			rangeDecoder._range <<= 8;
			rangeDecoder._total++;
		}
		return 1u;
	}
}
