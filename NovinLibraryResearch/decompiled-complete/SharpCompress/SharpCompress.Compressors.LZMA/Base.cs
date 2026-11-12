namespace SharpCompress.Compressors.LZMA;

internal abstract class Base
{
	public struct State
	{
		public uint _index;

		public void Init()
		{
			_index = 0u;
		}

		public void UpdateChar()
		{
			if (_index < 4)
			{
				_index = 0u;
			}
			else if (_index < 10)
			{
				_index -= 3u;
			}
			else
			{
				_index -= 6u;
			}
		}

		public void UpdateMatch()
		{
			_index = ((_index < 7) ? 7u : 10u);
		}

		public void UpdateRep()
		{
			_index = ((_index < 7) ? 8u : 11u);
		}

		public void UpdateShortRep()
		{
			_index = ((_index < 7) ? 9u : 11u);
		}

		public bool IsCharState()
		{
			return _index < 7;
		}
	}

	public const uint K_NUM_REP_DISTANCES = 4u;

	public const uint K_NUM_STATES = 12u;

	public const int K_NUM_POS_SLOT_BITS = 6;

	public const int K_DIC_LOG_SIZE_MIN = 0;

	public const int K_NUM_LEN_TO_POS_STATES_BITS = 2;

	public const uint K_NUM_LEN_TO_POS_STATES = 4u;

	public const uint K_MATCH_MIN_LEN = 2u;

	public const int K_NUM_ALIGN_BITS = 4;

	public const uint K_ALIGN_TABLE_SIZE = 16u;

	public const uint K_ALIGN_MASK = 15u;

	public const uint K_START_POS_MODEL_INDEX = 4u;

	public const uint K_END_POS_MODEL_INDEX = 14u;

	public const uint K_NUM_POS_MODELS = 10u;

	public const uint K_NUM_FULL_DISTANCES = 128u;

	public const uint K_NUM_LIT_POS_STATES_BITS_ENCODING_MAX = 4u;

	public const uint K_NUM_LIT_CONTEXT_BITS_MAX = 8u;

	public const int K_NUM_POS_STATES_BITS_MAX = 4;

	public const uint K_NUM_POS_STATES_MAX = 16u;

	public const int K_NUM_POS_STATES_BITS_ENCODING_MAX = 4;

	public const uint K_NUM_POS_STATES_ENCODING_MAX = 16u;

	public const int K_NUM_LOW_LEN_BITS = 3;

	public const int K_NUM_MID_LEN_BITS = 3;

	public const int K_NUM_HIGH_LEN_BITS = 8;

	public const uint K_NUM_LOW_LEN_SYMBOLS = 8u;

	public const uint K_NUM_MID_LEN_SYMBOLS = 8u;

	public const uint K_NUM_LEN_SYMBOLS = 272u;

	public const uint K_MATCH_MAX_LEN = 273u;

	public static uint GetLenToPosState(uint len)
	{
		len -= 2;
		if (len < 4)
		{
			return len;
		}
		return 3u;
	}
}
