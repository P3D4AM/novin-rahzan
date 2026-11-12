namespace SharpCompress.Compressors.LZMA.RangeCoder;

internal struct BitEncoder
{
	public const int K_NUM_BIT_MODEL_TOTAL_BITS = 11;

	public const uint K_BIT_MODEL_TOTAL = 2048u;

	private const int K_NUM_MOVE_BITS = 5;

	private const int K_NUM_MOVE_REDUCING_BITS = 2;

	public const int K_NUM_BIT_PRICE_SHIFT_BITS = 6;

	private uint _prob;

	private static readonly uint[] PROB_PRICES;

	public void Init()
	{
		_prob = 1024u;
	}

	public void UpdateModel(uint symbol)
	{
		if (symbol == 0)
		{
			_prob += 2048 - _prob >> 5;
		}
		else
		{
			_prob -= _prob >> 5;
		}
	}

	public void Encode(Encoder encoder, uint symbol)
	{
		uint num = (encoder._range >> 11) * _prob;
		if (symbol == 0)
		{
			encoder._range = num;
			_prob += 2048 - _prob >> 5;
		}
		else
		{
			encoder._low += num;
			encoder._range -= num;
			_prob -= _prob >> 5;
		}
		if (encoder._range < 16777216)
		{
			encoder._range <<= 8;
			encoder.ShiftLow();
		}
	}

	static BitEncoder()
	{
		PROB_PRICES = new uint[512];
		for (int num = 8; num >= 0; num--)
		{
			int num2 = 1 << 9 - num - 1;
			uint num3 = (uint)(1 << 9 - num);
			for (uint num4 = (uint)num2; num4 < num3; num4++)
			{
				PROB_PRICES[num4] = (uint)(num << 6) + (num3 - num4 << 6 >> 9 - num - 1);
			}
		}
	}

	public uint GetPrice(uint symbol)
	{
		return PROB_PRICES[(((_prob - symbol) ^ (int)(0 - symbol)) & 0x7FF) >> 2];
	}

	public uint GetPrice0()
	{
		return PROB_PRICES[_prob >> 2];
	}

	public uint GetPrice1()
	{
		return PROB_PRICES[2048 - _prob >> 2];
	}
}
