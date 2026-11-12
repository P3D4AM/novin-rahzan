using System;
using System.IO;
using SharpCompress.Compressors.LZMA.LZ;
using SharpCompress.Compressors.LZMA.RangeCoder;

namespace SharpCompress.Compressors.LZMA;

internal class Encoder : ICoder, ISetCoderProperties, IWriteCoderProperties
{
	private enum EMatchFinderType
	{
		Bt2,
		Bt4
	}

	private class LiteralEncoder
	{
		public struct Encoder2
		{
			private BitEncoder[] _encoders;

			public void Create()
			{
				_encoders = new BitEncoder[768];
			}

			public void Init()
			{
				for (int i = 0; i < 768; i++)
				{
					_encoders[i].Init();
				}
			}

			public void Encode(SharpCompress.Compressors.LZMA.RangeCoder.Encoder rangeEncoder, byte symbol)
			{
				uint num = 1u;
				for (int num2 = 7; num2 >= 0; num2--)
				{
					uint num3 = (uint)(symbol >> num2) & 1u;
					_encoders[num].Encode(rangeEncoder, num3);
					num = (num << 1) | num3;
				}
			}

			public void EncodeMatched(SharpCompress.Compressors.LZMA.RangeCoder.Encoder rangeEncoder, byte matchByte, byte symbol)
			{
				uint num = 1u;
				bool flag = true;
				for (int num2 = 7; num2 >= 0; num2--)
				{
					uint num3 = (uint)(symbol >> num2) & 1u;
					uint num4 = num;
					if (flag)
					{
						uint num5 = (uint)(matchByte >> num2) & 1u;
						num4 += 1 + num5 << 8;
						flag = num5 == num3;
					}
					_encoders[num4].Encode(rangeEncoder, num3);
					num = (num << 1) | num3;
				}
			}

			public uint GetPrice(bool matchMode, byte matchByte, byte symbol)
			{
				uint num = 0u;
				uint num2 = 1u;
				int num3 = 7;
				if (matchMode)
				{
					while (num3 >= 0)
					{
						uint num4 = (uint)(matchByte >> num3) & 1u;
						uint num5 = (uint)(symbol >> num3) & 1u;
						num += _encoders[(1 + num4 << 8) + num2].GetPrice(num5);
						num2 = (num2 << 1) | num5;
						if (num4 != num5)
						{
							num3--;
							break;
						}
						num3--;
					}
				}
				while (num3 >= 0)
				{
					uint num6 = (uint)(symbol >> num3) & 1u;
					num += _encoders[num2].GetPrice(num6);
					num2 = (num2 << 1) | num6;
					num3--;
				}
				return num;
			}
		}

		private Encoder2[] _coders;

		private int _numPrevBits;

		private int _numPosBits;

		private uint _posMask;

		public void Create(int numPosBits, int numPrevBits)
		{
			if (_coders == null || _numPrevBits != numPrevBits || _numPosBits != numPosBits)
			{
				_numPosBits = numPosBits;
				_posMask = (uint)((1 << numPosBits) - 1);
				_numPrevBits = numPrevBits;
				uint num = (uint)(1 << _numPrevBits + _numPosBits);
				_coders = new Encoder2[num];
				for (uint num2 = 0u; num2 < num; num2++)
				{
					_coders[num2].Create();
				}
			}
		}

		public void Init()
		{
			uint num = (uint)(1 << _numPrevBits + _numPosBits);
			for (uint num2 = 0u; num2 < num; num2++)
			{
				_coders[num2].Init();
			}
		}

		public Encoder2 GetSubCoder(uint pos, byte prevByte)
		{
			return _coders[(int)((pos & _posMask) << _numPrevBits) + (prevByte >> 8 - _numPrevBits)];
		}
	}

	private class LenEncoder
	{
		private BitEncoder _choice;

		private BitEncoder _choice2;

		private readonly BitTreeEncoder[] _lowCoder = new BitTreeEncoder[16];

		private readonly BitTreeEncoder[] _midCoder = new BitTreeEncoder[16];

		private BitTreeEncoder _highCoder = new BitTreeEncoder(8);

		public LenEncoder()
		{
			for (uint num = 0u; num < 16; num++)
			{
				_lowCoder[num] = new BitTreeEncoder(3);
				_midCoder[num] = new BitTreeEncoder(3);
			}
		}

		public void Init(uint numPosStates)
		{
			_choice.Init();
			_choice2.Init();
			for (uint num = 0u; num < numPosStates; num++)
			{
				_lowCoder[num].Init();
				_midCoder[num].Init();
			}
			_highCoder.Init();
		}

		public void Encode(SharpCompress.Compressors.LZMA.RangeCoder.Encoder rangeEncoder, uint symbol, uint posState)
		{
			if (symbol < 8)
			{
				_choice.Encode(rangeEncoder, 0u);
				_lowCoder[posState].Encode(rangeEncoder, symbol);
				return;
			}
			symbol -= 8;
			_choice.Encode(rangeEncoder, 1u);
			if (symbol < 8)
			{
				_choice2.Encode(rangeEncoder, 0u);
				_midCoder[posState].Encode(rangeEncoder, symbol);
			}
			else
			{
				_choice2.Encode(rangeEncoder, 1u);
				_highCoder.Encode(rangeEncoder, symbol - 8);
			}
		}

		public void SetPrices(uint posState, uint numSymbols, uint[] prices, uint st)
		{
			uint price = _choice.GetPrice0();
			uint price2 = _choice.GetPrice1();
			uint num = price2 + _choice2.GetPrice0();
			uint num2 = price2 + _choice2.GetPrice1();
			uint num3 = 0u;
			for (num3 = 0u; num3 < 8; num3++)
			{
				if (num3 >= numSymbols)
				{
					return;
				}
				prices[st + num3] = price + _lowCoder[posState].GetPrice(num3);
			}
			for (; num3 < 16; num3++)
			{
				if (num3 >= numSymbols)
				{
					return;
				}
				prices[st + num3] = num + _midCoder[posState].GetPrice(num3 - 8);
			}
			for (; num3 < numSymbols; num3++)
			{
				prices[st + num3] = num2 + _highCoder.GetPrice(num3 - 8 - 8);
			}
		}
	}

	private class LenPriceTableEncoder : LenEncoder
	{
		private readonly uint[] _prices = new uint[4352];

		private uint _tableSize;

		private readonly uint[] _counters = new uint[16];

		public void SetTableSize(uint tableSize)
		{
			_tableSize = tableSize;
		}

		public uint GetPrice(uint symbol, uint posState)
		{
			return _prices[posState * 272 + symbol];
		}

		private void UpdateTable(uint posState)
		{
			SetPrices(posState, _tableSize, _prices, posState * 272);
			_counters[posState] = _tableSize;
		}

		public void UpdateTables(uint numPosStates)
		{
			for (uint num = 0u; num < numPosStates; num++)
			{
				UpdateTable(num);
			}
		}

		public new void Encode(SharpCompress.Compressors.LZMA.RangeCoder.Encoder rangeEncoder, uint symbol, uint posState)
		{
			base.Encode(rangeEncoder, symbol, posState);
			if (--_counters[posState] == 0)
			{
				UpdateTable(posState);
			}
		}
	}

	private class Optimal
	{
		public Base.State _state;

		public bool _prev1IsChar;

		public bool _prev2;

		public uint _posPrev2;

		public uint _backPrev2;

		public uint _price;

		public uint _posPrev;

		public uint _backPrev;

		public uint _backs0;

		public uint _backs1;

		public uint _backs2;

		public uint _backs3;

		public void MakeAsChar()
		{
			_backPrev = uint.MaxValue;
			_prev1IsChar = false;
		}

		public void MakeAsShortRep()
		{
			_backPrev = 0u;
			_prev1IsChar = false;
		}

		public bool IsShortRep()
		{
			return _backPrev == 0;
		}
	}

	private const uint K_IFINITY_PRICE = 268435455u;

	private static readonly byte[] G_FAST_POS;

	private Base.State _state;

	private byte _previousByte;

	private readonly uint[] _repDistances = new uint[4];

	private const int K_DEFAULT_DICTIONARY_LOG_SIZE = 22;

	private const uint K_NUM_FAST_BYTES_DEFAULT = 32u;

	private const uint K_NUM_LEN_SPEC_SYMBOLS = 16u;

	private const uint K_NUM_OPTS = 4096u;

	private readonly Optimal[] _optimum = new Optimal[4096];

	private BinTree _matchFinder;

	private readonly SharpCompress.Compressors.LZMA.RangeCoder.Encoder _rangeEncoder = new SharpCompress.Compressors.LZMA.RangeCoder.Encoder();

	private readonly BitEncoder[] _isMatch = new BitEncoder[192];

	private readonly BitEncoder[] _isRep = new BitEncoder[12];

	private readonly BitEncoder[] _isRepG0 = new BitEncoder[12];

	private readonly BitEncoder[] _isRepG1 = new BitEncoder[12];

	private readonly BitEncoder[] _isRepG2 = new BitEncoder[12];

	private readonly BitEncoder[] _isRep0Long = new BitEncoder[192];

	private readonly BitTreeEncoder[] _posSlotEncoder = new BitTreeEncoder[4];

	private readonly BitEncoder[] _posEncoders = new BitEncoder[114];

	private BitTreeEncoder _posAlignEncoder = new BitTreeEncoder(4);

	private readonly LenPriceTableEncoder _lenEncoder = new LenPriceTableEncoder();

	private readonly LenPriceTableEncoder _repMatchLenEncoder = new LenPriceTableEncoder();

	private readonly LiteralEncoder _literalEncoder = new LiteralEncoder();

	private readonly uint[] _matchDistances = new uint[548];

	private uint _numFastBytes = 32u;

	private uint _longestMatchLength;

	private uint _numDistancePairs;

	private uint _additionalOffset;

	private uint _optimumEndIndex;

	private uint _optimumCurrentIndex;

	private bool _longestMatchWasFound;

	private readonly uint[] _posSlotPrices = new uint[256];

	private readonly uint[] _distancesPrices = new uint[512];

	private readonly uint[] _alignPrices = new uint[16];

	private uint _alignPriceCount;

	private uint _distTableSize = 44u;

	private int _posStateBits = 2;

	private uint _posStateMask = 3u;

	private int _numLiteralPosStateBits;

	private int _numLiteralContextBits = 3;

	private uint _dictionarySize = 4194304u;

	private uint _dictionarySizePrev = uint.MaxValue;

	private uint _numFastBytesPrev = uint.MaxValue;

	private long _nowPos64;

	private bool _finished;

	private Stream _inStream;

	private EMatchFinderType _matchFinderType = EMatchFinderType.Bt4;

	private bool _writeEndMark;

	private bool _needReleaseMfStream;

	private bool _processingMode;

	private readonly uint[] _reps = new uint[4];

	private readonly uint[] _repLens = new uint[4];

	private const int K_PROP_SIZE = 5;

	private readonly byte[] _properties = new byte[5];

	private readonly uint[] _tempPrices = new uint[128];

	private uint _matchPriceCount;

	private static readonly string[] K_MATCH_FINDER_I_DS;

	private uint _trainSize;

	static Encoder()
	{
		G_FAST_POS = new byte[2048];
		K_MATCH_FINDER_I_DS = new string[2] { "BT2", "BT4" };
		int num = 2;
		G_FAST_POS[0] = 0;
		G_FAST_POS[1] = 1;
		for (byte b = 2; b < 22; b++)
		{
			uint num2 = (uint)(1 << (b >> 1) - 1);
			uint num3 = 0u;
			while (num3 < num2)
			{
				G_FAST_POS[num] = b;
				num3++;
				num++;
			}
		}
	}

	private static uint GetPosSlot(uint pos)
	{
		if (pos < 2048)
		{
			return G_FAST_POS[pos];
		}
		if (pos < 2097152)
		{
			return (uint)(G_FAST_POS[pos >> 10] + 20);
		}
		return (uint)(G_FAST_POS[pos >> 20] + 40);
	}

	private static uint GetPosSlot2(uint pos)
	{
		if (pos < 131072)
		{
			return (uint)(G_FAST_POS[pos >> 6] + 12);
		}
		if (pos < 134217728)
		{
			return (uint)(G_FAST_POS[pos >> 16] + 32);
		}
		return (uint)(G_FAST_POS[pos >> 26] + 52);
	}

	private void BaseInit()
	{
		_state.Init();
		_previousByte = 0;
		for (uint num = 0u; num < 4; num++)
		{
			_repDistances[num] = 0u;
		}
	}

	private void Create()
	{
		if (_matchFinder == null)
		{
			BinTree binTree = new BinTree();
			int type = 4;
			if (_matchFinderType == EMatchFinderType.Bt2)
			{
				type = 2;
			}
			binTree.SetType(type);
			_matchFinder = binTree;
		}
		_literalEncoder.Create(_numLiteralPosStateBits, _numLiteralContextBits);
		if (_dictionarySize != _dictionarySizePrev || _numFastBytesPrev != _numFastBytes)
		{
			_matchFinder.Create(_dictionarySize, 4096u, _numFastBytes, 4370u);
			_dictionarySizePrev = _dictionarySize;
			_numFastBytesPrev = _numFastBytes;
		}
	}

	public Encoder()
	{
		for (int i = 0; (long)i < 4096L; i++)
		{
			_optimum[i] = new Optimal();
		}
		for (int j = 0; (long)j < 4L; j++)
		{
			_posSlotEncoder[j] = new BitTreeEncoder(6);
		}
	}

	private void SetWriteEndMarkerMode(bool writeEndMarker)
	{
		_writeEndMark = writeEndMarker;
	}

	private void Init()
	{
		BaseInit();
		_rangeEncoder.Init();
		for (uint num = 0u; num < 12; num++)
		{
			for (uint num2 = 0u; num2 <= _posStateMask; num2++)
			{
				uint num3 = (num << 4) + num2;
				_isMatch[num3].Init();
				_isRep0Long[num3].Init();
			}
			_isRep[num].Init();
			_isRepG0[num].Init();
			_isRepG1[num].Init();
			_isRepG2[num].Init();
		}
		_literalEncoder.Init();
		for (uint num = 0u; num < 4; num++)
		{
			_posSlotEncoder[num].Init();
		}
		for (uint num = 0u; num < 114; num++)
		{
			_posEncoders[num].Init();
		}
		_lenEncoder.Init((uint)(1 << _posStateBits));
		_repMatchLenEncoder.Init((uint)(1 << _posStateBits));
		_posAlignEncoder.Init();
		_longestMatchWasFound = false;
		_optimumEndIndex = 0u;
		_optimumCurrentIndex = 0u;
		_additionalOffset = 0u;
	}

	private void ReadMatchDistances(out uint lenRes, out uint numDistancePairs)
	{
		lenRes = 0u;
		numDistancePairs = _matchFinder.GetMatches(_matchDistances);
		if (numDistancePairs != 0)
		{
			lenRes = _matchDistances[numDistancePairs - 2];
			if (lenRes == _numFastBytes)
			{
				lenRes += _matchFinder.GetMatchLen((int)(lenRes - 1), _matchDistances[numDistancePairs - 1], 273 - lenRes);
			}
		}
		_additionalOffset++;
	}

	private void MovePos(uint num)
	{
		if (num != 0)
		{
			_matchFinder.Skip(num);
			_additionalOffset += num;
		}
	}

	private uint GetRepLen1Price(Base.State state, uint posState)
	{
		return _isRepG0[state._index].GetPrice0() + _isRep0Long[(state._index << 4) + posState].GetPrice0();
	}

	private uint GetPureRepPrice(uint repIndex, Base.State state, uint posState)
	{
		uint price;
		if (repIndex == 0)
		{
			price = _isRepG0[state._index].GetPrice0();
			return price + _isRep0Long[(state._index << 4) + posState].GetPrice1();
		}
		price = _isRepG0[state._index].GetPrice1();
		if (repIndex == 1)
		{
			return price + _isRepG1[state._index].GetPrice0();
		}
		price += _isRepG1[state._index].GetPrice1();
		return price + _isRepG2[state._index].GetPrice(repIndex - 2);
	}

	private uint GetRepPrice(uint repIndex, uint len, Base.State state, uint posState)
	{
		return _repMatchLenEncoder.GetPrice(len - 2, posState) + GetPureRepPrice(repIndex, state, posState);
	}

	private uint GetPosLenPrice(uint pos, uint len, uint posState)
	{
		uint lenToPosState = Base.GetLenToPosState(len);
		uint num = ((pos >= 128) ? (_posSlotPrices[(lenToPosState << 6) + GetPosSlot2(pos)] + _alignPrices[pos & 0xF]) : _distancesPrices[lenToPosState * 128 + pos]);
		return num + _lenEncoder.GetPrice(len - 2, posState);
	}

	private uint Backward(out uint backRes, uint cur)
	{
		_optimumEndIndex = cur;
		uint posPrev = _optimum[cur]._posPrev;
		uint backPrev = _optimum[cur]._backPrev;
		do
		{
			if (_optimum[cur]._prev1IsChar)
			{
				_optimum[posPrev].MakeAsChar();
				_optimum[posPrev]._posPrev = posPrev - 1;
				if (_optimum[cur]._prev2)
				{
					_optimum[posPrev - 1]._prev1IsChar = false;
					_optimum[posPrev - 1]._posPrev = _optimum[cur]._posPrev2;
					_optimum[posPrev - 1]._backPrev = _optimum[cur]._backPrev2;
				}
			}
			uint num = posPrev;
			uint backPrev2 = backPrev;
			backPrev = _optimum[num]._backPrev;
			posPrev = _optimum[num]._posPrev;
			_optimum[num]._backPrev = backPrev2;
			_optimum[num]._posPrev = cur;
			cur = num;
		}
		while (cur != 0);
		backRes = _optimum[0]._backPrev;
		_optimumCurrentIndex = _optimum[0]._posPrev;
		return _optimumCurrentIndex;
	}

	private uint GetOptimum(uint position, out uint backRes)
	{
		if (_optimumEndIndex != _optimumCurrentIndex)
		{
			uint result = _optimum[_optimumCurrentIndex]._posPrev - _optimumCurrentIndex;
			backRes = _optimum[_optimumCurrentIndex]._backPrev;
			_optimumCurrentIndex = _optimum[_optimumCurrentIndex]._posPrev;
			return result;
		}
		_optimumCurrentIndex = (_optimumEndIndex = 0u);
		uint lenRes;
		uint numDistancePairs;
		if (!_longestMatchWasFound)
		{
			ReadMatchDistances(out lenRes, out numDistancePairs);
		}
		else
		{
			lenRes = _longestMatchLength;
			numDistancePairs = _numDistancePairs;
			_longestMatchWasFound = false;
		}
		uint num = _matchFinder.GetNumAvailableBytes() + 1;
		if (num < 2)
		{
			backRes = uint.MaxValue;
			return 1u;
		}
		if (num > 273)
		{
			num = 273u;
		}
		uint num2 = 0u;
		for (uint num3 = 0u; num3 < 4; num3++)
		{
			_reps[num3] = _repDistances[num3];
			_repLens[num3] = _matchFinder.GetMatchLen(-1, _reps[num3], 273u);
			if (_repLens[num3] > _repLens[num2])
			{
				num2 = num3;
			}
		}
		if (_repLens[num2] >= _numFastBytes)
		{
			backRes = num2;
			uint num4 = _repLens[num2];
			MovePos(num4 - 1);
			return num4;
		}
		if (lenRes >= _numFastBytes)
		{
			backRes = _matchDistances[numDistancePairs - 1] + 4;
			MovePos(lenRes - 1);
			return lenRes;
		}
		byte indexByte = _matchFinder.GetIndexByte(-1);
		byte indexByte2 = _matchFinder.GetIndexByte((int)(0 - _repDistances[0] - 1 - 1));
		if (lenRes < 2 && indexByte != indexByte2 && _repLens[num2] < 2)
		{
			backRes = uint.MaxValue;
			return 1u;
		}
		_optimum[0]._state = _state;
		uint num5 = position & _posStateMask;
		_optimum[1]._price = _isMatch[(_state._index << 4) + num5].GetPrice0() + _literalEncoder.GetSubCoder(position, _previousByte).GetPrice(!_state.IsCharState(), indexByte2, indexByte);
		_optimum[1].MakeAsChar();
		uint price = _isMatch[(_state._index << 4) + num5].GetPrice1();
		uint num6 = price + _isRep[_state._index].GetPrice1();
		if (indexByte2 == indexByte)
		{
			uint num7 = num6 + GetRepLen1Price(_state, num5);
			if (num7 < _optimum[1]._price)
			{
				_optimum[1]._price = num7;
				_optimum[1].MakeAsShortRep();
			}
		}
		uint num8 = ((lenRes >= _repLens[num2]) ? lenRes : _repLens[num2]);
		if (num8 < 2)
		{
			backRes = _optimum[1]._backPrev;
			return 1u;
		}
		_optimum[1]._posPrev = 0u;
		_optimum[0]._backs0 = _reps[0];
		_optimum[0]._backs1 = _reps[1];
		_optimum[0]._backs2 = _reps[2];
		_optimum[0]._backs3 = _reps[3];
		uint num9 = num8;
		do
		{
			_optimum[num9--]._price = 268435455u;
		}
		while (num9 >= 2);
		for (uint num3 = 0u; num3 < 4; num3++)
		{
			uint num10 = _repLens[num3];
			if (num10 < 2)
			{
				continue;
			}
			uint num11 = num6 + GetPureRepPrice(num3, _state, num5);
			do
			{
				uint num12 = num11 + _repMatchLenEncoder.GetPrice(num10 - 2, num5);
				Optimal optimal = _optimum[num10];
				if (num12 < optimal._price)
				{
					optimal._price = num12;
					optimal._posPrev = 0u;
					optimal._backPrev = num3;
					optimal._prev1IsChar = false;
				}
			}
			while (--num10 >= 2);
		}
		uint num13 = price + _isRep[_state._index].GetPrice0();
		num9 = ((_repLens[0] >= 2) ? (_repLens[0] + 1) : 2u);
		if (num9 <= lenRes)
		{
			uint num14;
			for (num14 = 0u; num9 > _matchDistances[num14]; num14 += 2)
			{
			}
			while (true)
			{
				uint num15 = _matchDistances[num14 + 1];
				uint num16 = num13 + GetPosLenPrice(num15, num9, num5);
				Optimal optimal2 = _optimum[num9];
				if (num16 < optimal2._price)
				{
					optimal2._price = num16;
					optimal2._posPrev = 0u;
					optimal2._backPrev = num15 + 4;
					optimal2._prev1IsChar = false;
				}
				if (num9 == _matchDistances[num14])
				{
					num14 += 2;
					if (num14 == numDistancePairs)
					{
						break;
					}
				}
				num9++;
			}
		}
		uint num17 = 0u;
		uint lenRes2;
		while (true)
		{
			num17++;
			if (num17 == num8)
			{
				return Backward(out backRes, num17);
			}
			ReadMatchDistances(out lenRes2, out numDistancePairs);
			if (lenRes2 >= _numFastBytes)
			{
				break;
			}
			position++;
			uint num18 = _optimum[num17]._posPrev;
			Base.State state;
			if (_optimum[num17]._prev1IsChar)
			{
				num18--;
				if (_optimum[num17]._prev2)
				{
					state = _optimum[_optimum[num17]._posPrev2]._state;
					if (_optimum[num17]._backPrev2 < 4)
					{
						state.UpdateRep();
					}
					else
					{
						state.UpdateMatch();
					}
				}
				else
				{
					state = _optimum[num18]._state;
				}
				state.UpdateChar();
			}
			else
			{
				state = _optimum[num18]._state;
			}
			if (num18 == num17 - 1)
			{
				if (_optimum[num17].IsShortRep())
				{
					state.UpdateShortRep();
				}
				else
				{
					state.UpdateChar();
				}
			}
			else
			{
				uint num19;
				if (_optimum[num17]._prev1IsChar && _optimum[num17]._prev2)
				{
					num18 = _optimum[num17]._posPrev2;
					num19 = _optimum[num17]._backPrev2;
					state.UpdateRep();
				}
				else
				{
					num19 = _optimum[num17]._backPrev;
					if (num19 < 4)
					{
						state.UpdateRep();
					}
					else
					{
						state.UpdateMatch();
					}
				}
				Optimal optimal3 = _optimum[num18];
				switch (num19)
				{
				case 0u:
					_reps[0] = optimal3._backs0;
					_reps[1] = optimal3._backs1;
					_reps[2] = optimal3._backs2;
					_reps[3] = optimal3._backs3;
					break;
				case 1u:
					_reps[0] = optimal3._backs1;
					_reps[1] = optimal3._backs0;
					_reps[2] = optimal3._backs2;
					_reps[3] = optimal3._backs3;
					break;
				case 2u:
					_reps[0] = optimal3._backs2;
					_reps[1] = optimal3._backs0;
					_reps[2] = optimal3._backs1;
					_reps[3] = optimal3._backs3;
					break;
				case 3u:
					_reps[0] = optimal3._backs3;
					_reps[1] = optimal3._backs0;
					_reps[2] = optimal3._backs1;
					_reps[3] = optimal3._backs2;
					break;
				default:
					_reps[0] = num19 - 4;
					_reps[1] = optimal3._backs0;
					_reps[2] = optimal3._backs1;
					_reps[3] = optimal3._backs2;
					break;
				}
			}
			_optimum[num17]._state = state;
			_optimum[num17]._backs0 = _reps[0];
			_optimum[num17]._backs1 = _reps[1];
			_optimum[num17]._backs2 = _reps[2];
			_optimum[num17]._backs3 = _reps[3];
			uint price2 = _optimum[num17]._price;
			indexByte = _matchFinder.GetIndexByte(-1);
			indexByte2 = _matchFinder.GetIndexByte((int)(0 - _reps[0] - 1 - 1));
			num5 = position & _posStateMask;
			uint num20 = price2 + _isMatch[(state._index << 4) + num5].GetPrice0() + _literalEncoder.GetSubCoder(position, _matchFinder.GetIndexByte(-2)).GetPrice(!state.IsCharState(), indexByte2, indexByte);
			Optimal optimal4 = _optimum[num17 + 1];
			bool flag = false;
			if (num20 < optimal4._price)
			{
				optimal4._price = num20;
				optimal4._posPrev = num17;
				optimal4.MakeAsChar();
				flag = true;
			}
			price = price2 + _isMatch[(state._index << 4) + num5].GetPrice1();
			num6 = price + _isRep[state._index].GetPrice1();
			if (indexByte2 == indexByte && (optimal4._posPrev >= num17 || optimal4._backPrev != 0))
			{
				uint num21 = num6 + GetRepLen1Price(state, num5);
				if (num21 <= optimal4._price)
				{
					optimal4._price = num21;
					optimal4._posPrev = num17;
					optimal4.MakeAsShortRep();
					flag = true;
				}
			}
			uint val = _matchFinder.GetNumAvailableBytes() + 1;
			val = Math.Min(4095 - num17, val);
			num = val;
			if (num < 2)
			{
				continue;
			}
			if (num > _numFastBytes)
			{
				num = _numFastBytes;
			}
			if (!flag && indexByte2 != indexByte)
			{
				uint limit = Math.Min(val - 1, _numFastBytes);
				uint matchLen = _matchFinder.GetMatchLen(0, _reps[0], limit);
				if (matchLen >= 2)
				{
					Base.State state2 = state;
					state2.UpdateChar();
					uint num22 = (position + 1) & _posStateMask;
					uint num23 = num20 + _isMatch[(state2._index << 4) + num22].GetPrice1() + _isRep[state2._index].GetPrice1();
					uint num24 = num17 + 1 + matchLen;
					while (num8 < num24)
					{
						_optimum[++num8]._price = 268435455u;
					}
					uint num25 = num23 + GetRepPrice(0u, matchLen, state2, num22);
					Optimal optimal5 = _optimum[num24];
					if (num25 < optimal5._price)
					{
						optimal5._price = num25;
						optimal5._posPrev = num17 + 1;
						optimal5._backPrev = 0u;
						optimal5._prev1IsChar = true;
						optimal5._prev2 = false;
					}
				}
			}
			uint num26 = 2u;
			for (uint num27 = 0u; num27 < 4; num27++)
			{
				uint num28 = _matchFinder.GetMatchLen(-1, _reps[num27], num);
				if (num28 < 2)
				{
					continue;
				}
				uint num29 = num28;
				while (true)
				{
					if (num8 < num17 + num28)
					{
						_optimum[++num8]._price = 268435455u;
						continue;
					}
					uint num30 = num6 + GetRepPrice(num27, num28, state, num5);
					Optimal optimal6 = _optimum[num17 + num28];
					if (num30 < optimal6._price)
					{
						optimal6._price = num30;
						optimal6._posPrev = num17;
						optimal6._backPrev = num27;
						optimal6._prev1IsChar = false;
					}
					if (--num28 < 2)
					{
						break;
					}
				}
				num28 = num29;
				if (num27 == 0)
				{
					num26 = num28 + 1;
				}
				if (num28 >= val)
				{
					continue;
				}
				uint limit2 = Math.Min(val - 1 - num28, _numFastBytes);
				uint matchLen2 = _matchFinder.GetMatchLen((int)num28, _reps[num27], limit2);
				if (matchLen2 >= 2)
				{
					Base.State state3 = state;
					state3.UpdateRep();
					uint num31 = (position + num28) & _posStateMask;
					uint num32 = num6 + GetRepPrice(num27, num28, state, num5) + _isMatch[(state3._index << 4) + num31].GetPrice0() + _literalEncoder.GetSubCoder(position + num28, _matchFinder.GetIndexByte((int)(num28 - 1 - 1))).GetPrice(matchMode: true, _matchFinder.GetIndexByte((int)(num28 - 1 - (_reps[num27] + 1))), _matchFinder.GetIndexByte((int)(num28 - 1)));
					state3.UpdateChar();
					num31 = (position + num28 + 1) & _posStateMask;
					uint num33 = num32 + _isMatch[(state3._index << 4) + num31].GetPrice1() + _isRep[state3._index].GetPrice1();
					uint num34 = num28 + 1 + matchLen2;
					while (num8 < num17 + num34)
					{
						_optimum[++num8]._price = 268435455u;
					}
					uint num35 = num33 + GetRepPrice(0u, matchLen2, state3, num31);
					Optimal optimal7 = _optimum[num17 + num34];
					if (num35 < optimal7._price)
					{
						optimal7._price = num35;
						optimal7._posPrev = num17 + num28 + 1;
						optimal7._backPrev = 0u;
						optimal7._prev1IsChar = true;
						optimal7._prev2 = true;
						optimal7._posPrev2 = num17;
						optimal7._backPrev2 = num27;
					}
				}
			}
			if (lenRes2 > num)
			{
				lenRes2 = num;
				for (numDistancePairs = 0u; lenRes2 > _matchDistances[numDistancePairs]; numDistancePairs += 2)
				{
				}
				_matchDistances[numDistancePairs] = lenRes2;
				numDistancePairs += 2;
			}
			if (lenRes2 < num26)
			{
				continue;
			}
			num13 = price + _isRep[state._index].GetPrice0();
			while (num8 < num17 + lenRes2)
			{
				_optimum[++num8]._price = 268435455u;
			}
			uint num36;
			for (num36 = 0u; num26 > _matchDistances[num36]; num36 += 2)
			{
			}
			uint num37 = num26;
			while (true)
			{
				uint num38 = _matchDistances[num36 + 1];
				uint num39 = num13 + GetPosLenPrice(num38, num37, num5);
				Optimal optimal8 = _optimum[num17 + num37];
				if (num39 < optimal8._price)
				{
					optimal8._price = num39;
					optimal8._posPrev = num17;
					optimal8._backPrev = num38 + 4;
					optimal8._prev1IsChar = false;
				}
				if (num37 == _matchDistances[num36])
				{
					if (num37 < val)
					{
						uint limit3 = Math.Min(val - 1 - num37, _numFastBytes);
						uint matchLen3 = _matchFinder.GetMatchLen((int)num37, num38, limit3);
						if (matchLen3 >= 2)
						{
							Base.State state4 = state;
							state4.UpdateMatch();
							uint num40 = (position + num37) & _posStateMask;
							uint num41 = num39 + _isMatch[(state4._index << 4) + num40].GetPrice0() + _literalEncoder.GetSubCoder(position + num37, _matchFinder.GetIndexByte((int)(num37 - 1 - 1))).GetPrice(matchMode: true, _matchFinder.GetIndexByte((int)(num37 - (num38 + 1) - 1)), _matchFinder.GetIndexByte((int)(num37 - 1)));
							state4.UpdateChar();
							num40 = (position + num37 + 1) & _posStateMask;
							uint num42 = num41 + _isMatch[(state4._index << 4) + num40].GetPrice1() + _isRep[state4._index].GetPrice1();
							uint num43 = num37 + 1 + matchLen3;
							while (num8 < num17 + num43)
							{
								_optimum[++num8]._price = 268435455u;
							}
							num39 = num42 + GetRepPrice(0u, matchLen3, state4, num40);
							optimal8 = _optimum[num17 + num43];
							if (num39 < optimal8._price)
							{
								optimal8._price = num39;
								optimal8._posPrev = num17 + num37 + 1;
								optimal8._backPrev = 0u;
								optimal8._prev1IsChar = true;
								optimal8._prev2 = true;
								optimal8._posPrev2 = num17;
								optimal8._backPrev2 = num38 + 4;
							}
						}
					}
					num36 += 2;
					if (num36 == numDistancePairs)
					{
						break;
					}
				}
				num37++;
			}
		}
		_numDistancePairs = numDistancePairs;
		_longestMatchLength = lenRes2;
		_longestMatchWasFound = true;
		return Backward(out backRes, num17);
	}

	private bool ChangePair(uint smallDist, uint bigDist)
	{
		if (smallDist < 33554432)
		{
			return bigDist >= smallDist << 7;
		}
		return false;
	}

	private void WriteEndMarker(uint posState)
	{
		if (_writeEndMark)
		{
			_isMatch[(_state._index << 4) + posState].Encode(_rangeEncoder, 1u);
			_isRep[_state._index].Encode(_rangeEncoder, 0u);
			_state.UpdateMatch();
			uint num = 2u;
			_lenEncoder.Encode(_rangeEncoder, num - 2, posState);
			uint symbol = 63u;
			uint lenToPosState = Base.GetLenToPosState(num);
			_posSlotEncoder[lenToPosState].Encode(_rangeEncoder, symbol);
			int num2 = 30;
			uint num3 = (uint)((1 << num2) - 1);
			_rangeEncoder.EncodeDirectBits(num3 >> 4, num2 - 4);
			_posAlignEncoder.ReverseEncode(_rangeEncoder, num3 & 0xFu);
		}
	}

	private void Flush(uint nowPos)
	{
		ReleaseMfStream();
		WriteEndMarker(nowPos & _posStateMask);
		_rangeEncoder.FlushData();
		_rangeEncoder.FlushStream();
	}

	public void CodeOneBlock(out long inSize, out long outSize, out bool finished)
	{
		inSize = 0L;
		outSize = 0L;
		finished = true;
		if (_inStream != null)
		{
			_matchFinder.SetStream(_inStream);
			_needReleaseMfStream = true;
			_inStream = null;
		}
		if (_finished)
		{
			return;
		}
		_finished = true;
		long nowPos = _nowPos64;
		if (_nowPos64 == 0L)
		{
			if (_trainSize != 0)
			{
				while (_trainSize != 0 && (!_processingMode || !_matchFinder.IsDataStarved))
				{
					_matchFinder.Skip(1u);
					_trainSize--;
				}
				if (_trainSize == 0)
				{
					_previousByte = _matchFinder.GetIndexByte(-1);
				}
			}
			if (_processingMode && _matchFinder.IsDataStarved)
			{
				_finished = false;
				return;
			}
			if (_matchFinder.GetNumAvailableBytes() == 0)
			{
				Flush((uint)_nowPos64);
				return;
			}
			ReadMatchDistances(out var _, out var _);
			uint num = (uint)(int)_nowPos64 & _posStateMask;
			_isMatch[(_state._index << 4) + num].Encode(_rangeEncoder, 0u);
			_state.UpdateChar();
			byte indexByte = _matchFinder.GetIndexByte((int)(0 - _additionalOffset));
			_literalEncoder.GetSubCoder((uint)_nowPos64, _previousByte).Encode(_rangeEncoder, indexByte);
			_previousByte = indexByte;
			_additionalOffset--;
			_nowPos64++;
		}
		if (_processingMode && _matchFinder.IsDataStarved)
		{
			_finished = false;
			return;
		}
		if (_matchFinder.GetNumAvailableBytes() == 0)
		{
			Flush((uint)_nowPos64);
			return;
		}
		while (true)
		{
			if (_processingMode && _matchFinder.IsDataStarved)
			{
				_finished = false;
				return;
			}
			uint backRes;
			uint optimum = GetOptimum((uint)_nowPos64, out backRes);
			uint num2 = (uint)(int)_nowPos64 & _posStateMask;
			uint num3 = (_state._index << 4) + num2;
			if (optimum == 1 && backRes == uint.MaxValue)
			{
				_isMatch[num3].Encode(_rangeEncoder, 0u);
				byte indexByte2 = _matchFinder.GetIndexByte((int)(0 - _additionalOffset));
				LiteralEncoder.Encoder2 subCoder = _literalEncoder.GetSubCoder((uint)_nowPos64, _previousByte);
				if (!_state.IsCharState())
				{
					byte indexByte3 = _matchFinder.GetIndexByte((int)(0 - _repDistances[0] - 1 - _additionalOffset));
					subCoder.EncodeMatched(_rangeEncoder, indexByte3, indexByte2);
				}
				else
				{
					subCoder.Encode(_rangeEncoder, indexByte2);
				}
				_previousByte = indexByte2;
				_state.UpdateChar();
			}
			else
			{
				_isMatch[num3].Encode(_rangeEncoder, 1u);
				if (backRes < 4)
				{
					_isRep[_state._index].Encode(_rangeEncoder, 1u);
					if (backRes == 0)
					{
						_isRepG0[_state._index].Encode(_rangeEncoder, 0u);
						if (optimum == 1)
						{
							_isRep0Long[num3].Encode(_rangeEncoder, 0u);
						}
						else
						{
							_isRep0Long[num3].Encode(_rangeEncoder, 1u);
						}
					}
					else
					{
						_isRepG0[_state._index].Encode(_rangeEncoder, 1u);
						if (backRes == 1)
						{
							_isRepG1[_state._index].Encode(_rangeEncoder, 0u);
						}
						else
						{
							_isRepG1[_state._index].Encode(_rangeEncoder, 1u);
							_isRepG2[_state._index].Encode(_rangeEncoder, backRes - 2);
						}
					}
					if (optimum == 1)
					{
						_state.UpdateShortRep();
					}
					else
					{
						_repMatchLenEncoder.Encode(_rangeEncoder, optimum - 2, num2);
						_state.UpdateRep();
					}
					uint num4 = _repDistances[backRes];
					if (backRes != 0)
					{
						for (uint num5 = backRes; num5 >= 1; num5--)
						{
							_repDistances[num5] = _repDistances[num5 - 1];
						}
						_repDistances[0] = num4;
					}
				}
				else
				{
					_isRep[_state._index].Encode(_rangeEncoder, 0u);
					_state.UpdateMatch();
					_lenEncoder.Encode(_rangeEncoder, optimum - 2, num2);
					backRes -= 4;
					uint posSlot = GetPosSlot(backRes);
					uint lenToPosState = Base.GetLenToPosState(optimum);
					_posSlotEncoder[lenToPosState].Encode(_rangeEncoder, posSlot);
					if (posSlot >= 4)
					{
						int num6 = (int)((posSlot >> 1) - 1);
						uint num7 = (2 | (posSlot & 1)) << num6;
						uint num8 = backRes - num7;
						if (posSlot < 14)
						{
							BitTreeEncoder.ReverseEncode(_posEncoders, num7 - posSlot - 1, _rangeEncoder, num6, num8);
						}
						else
						{
							_rangeEncoder.EncodeDirectBits(num8 >> 4, num6 - 4);
							_posAlignEncoder.ReverseEncode(_rangeEncoder, num8 & 0xFu);
							_alignPriceCount++;
						}
					}
					uint num9 = backRes;
					for (uint num10 = 3u; num10 >= 1; num10--)
					{
						_repDistances[num10] = _repDistances[num10 - 1];
					}
					_repDistances[0] = num9;
					_matchPriceCount++;
				}
				_previousByte = _matchFinder.GetIndexByte((int)(optimum - 1 - _additionalOffset));
			}
			_additionalOffset -= optimum;
			_nowPos64 += optimum;
			if (_additionalOffset == 0)
			{
				if (_matchPriceCount >= 128)
				{
					FillDistancesPrices();
				}
				if (_alignPriceCount >= 16)
				{
					FillAlignPrices();
				}
				inSize = _nowPos64;
				outSize = _rangeEncoder.GetProcessedSizeAdd();
				if (_processingMode && _matchFinder.IsDataStarved)
				{
					_finished = false;
					return;
				}
				if (_matchFinder.GetNumAvailableBytes() == 0)
				{
					Flush((uint)_nowPos64);
					return;
				}
				if (_nowPos64 - nowPos >= 4096)
				{
					break;
				}
			}
		}
		_finished = false;
		finished = false;
	}

	private void ReleaseMfStream()
	{
		if (_matchFinder != null && _needReleaseMfStream)
		{
			_matchFinder.ReleaseStream();
			_needReleaseMfStream = false;
		}
	}

	private void SetOutStream(Stream outStream)
	{
		_rangeEncoder.SetStream(outStream);
	}

	private void ReleaseOutStream()
	{
		_rangeEncoder.ReleaseStream();
	}

	private void ReleaseStreams()
	{
		ReleaseMfStream();
		ReleaseOutStream();
	}

	public void SetStreams(Stream inStream, Stream outStream, long inSize, long outSize)
	{
		_inStream = inStream;
		_finished = false;
		Create();
		SetOutStream(outStream);
		Init();
		_matchFinder.Init();
		FillDistancesPrices();
		FillAlignPrices();
		_lenEncoder.SetTableSize(_numFastBytes + 1 - 2);
		_lenEncoder.UpdateTables((uint)(1 << _posStateBits));
		_repMatchLenEncoder.SetTableSize(_numFastBytes + 1 - 2);
		_repMatchLenEncoder.UpdateTables((uint)(1 << _posStateBits));
		_nowPos64 = 0L;
	}

	public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress)
	{
		_needReleaseMfStream = false;
		_processingMode = false;
		try
		{
			SetStreams(inStream, outStream, inSize, outSize);
			while (true)
			{
				CodeOneBlock(out var inSize2, out var outSize2, out var finished);
				if (finished)
				{
					break;
				}
				progress?.SetProgress(inSize2, outSize2);
			}
		}
		finally
		{
			ReleaseStreams();
		}
	}

	public long Code(Stream inStream, bool final)
	{
		_matchFinder.SetStream(inStream);
		_processingMode = !final;
		try
		{
			long inSize;
			bool finished;
			do
			{
				CodeOneBlock(out inSize, out var _, out finished);
			}
			while (!finished);
			return inSize;
		}
		finally
		{
			_matchFinder.ReleaseStream();
			if (final)
			{
				ReleaseStreams();
			}
		}
	}

	public void Train(Stream trainStream)
	{
		if (_nowPos64 > 0)
		{
			throw new InvalidOperationException();
		}
		_trainSize = (uint)trainStream.Length;
		if (_trainSize != 0)
		{
			_matchFinder.SetStream(trainStream);
			while (_trainSize != 0 && !_matchFinder.IsDataStarved)
			{
				_matchFinder.Skip(1u);
				_trainSize--;
			}
			if (_trainSize == 0)
			{
				_previousByte = _matchFinder.GetIndexByte(-1);
			}
			_matchFinder.ReleaseStream();
		}
	}

	public void WriteCoderProperties(Stream outStream)
	{
		WriteCoderProperties(_properties);
		outStream.Write(_properties, 0, 5);
	}

	public void WriteCoderProperties(Span<byte> span)
	{
		span[0] = (byte)((_posStateBits * 5 + _numLiteralPosStateBits) * 9 + _numLiteralContextBits);
		for (int i = 0; i < 4; i++)
		{
			span[1 + i] = (byte)((_dictionarySize >> 8 * i) & 0xFFu);
		}
	}

	private void FillDistancesPrices()
	{
		for (uint num = 4u; num < 128; num++)
		{
			uint posSlot = GetPosSlot(num);
			int num2 = (int)((posSlot >> 1) - 1);
			uint num3 = (2 | (posSlot & 1)) << num2;
			_tempPrices[num] = BitTreeEncoder.ReverseGetPrice(_posEncoders, num3 - posSlot - 1, num2, num - num3);
		}
		for (uint num4 = 0u; num4 < 4; num4++)
		{
			BitTreeEncoder bitTreeEncoder = _posSlotEncoder[num4];
			uint num5 = num4 << 6;
			for (uint num6 = 0u; num6 < _distTableSize; num6++)
			{
				_posSlotPrices[num5 + num6] = bitTreeEncoder.GetPrice(num6);
			}
			for (uint num6 = 14u; num6 < _distTableSize; num6++)
			{
				_posSlotPrices[num5 + num6] += (num6 >> 1) - 1 - 4 << 6;
			}
			uint num7 = num4 * 128;
			uint num8;
			for (num8 = 0u; num8 < 4; num8++)
			{
				_distancesPrices[num7 + num8] = _posSlotPrices[num5 + num8];
			}
			for (; num8 < 128; num8++)
			{
				_distancesPrices[num7 + num8] = _posSlotPrices[num5 + GetPosSlot(num8)] + _tempPrices[num8];
			}
		}
		_matchPriceCount = 0u;
	}

	private void FillAlignPrices()
	{
		for (uint num = 0u; num < 16; num++)
		{
			_alignPrices[num] = _posAlignEncoder.ReverseGetPrice(num);
		}
		_alignPriceCount = 0u;
	}

	private static int FindMatchFinder(string s)
	{
		for (int i = 0; i < K_MATCH_FINDER_I_DS.Length; i++)
		{
			if (s == K_MATCH_FINDER_I_DS[i])
			{
				return i;
			}
		}
		return -1;
	}

	public void SetCoderProperties(CoderPropId[] propIDs, object[] properties)
	{
		for (uint num = 0u; num < properties.Length; num++)
		{
			object obj = properties[num];
			switch (propIDs[num])
			{
			case CoderPropId.NumFastBytes:
				if (!(obj is int num2))
				{
					throw new InvalidParamException();
				}
				if (num2 < 5 || (long)num2 > 273L)
				{
					throw new InvalidParamException();
				}
				_numFastBytes = (uint)num2;
				break;
			case CoderPropId.MatchFinder:
			{
				if (!(obj is string))
				{
					throw new InvalidParamException();
				}
				EMatchFinderType matchFinderType = _matchFinderType;
				int num6 = FindMatchFinder(((string)obj).ToUpper());
				if (num6 < 0)
				{
					throw new InvalidParamException();
				}
				_matchFinderType = (EMatchFinderType)num6;
				if (_matchFinder != null && matchFinderType != _matchFinderType)
				{
					_dictionarySizePrev = uint.MaxValue;
					_matchFinder = null;
				}
				break;
			}
			case CoderPropId.DictionarySize:
			{
				if (!(obj is int num7))
				{
					throw new InvalidParamException();
				}
				if ((long)num7 < 1L || (long)num7 > 1073741824L)
				{
					throw new InvalidParamException();
				}
				_dictionarySize = (uint)num7;
				int i;
				for (i = 0; (long)i < 30L && num7 > (uint)(1 << i); i++)
				{
				}
				_distTableSize = (uint)(i * 2);
				break;
			}
			case CoderPropId.PosStateBits:
				if (!(obj is int num3))
				{
					throw new InvalidParamException();
				}
				if (num3 < 0 || (long)num3 > 4L)
				{
					throw new InvalidParamException();
				}
				_posStateBits = num3;
				_posStateMask = (uint)((1 << _posStateBits) - 1);
				break;
			case CoderPropId.LitPosBits:
				if (!(obj is int num5))
				{
					throw new InvalidParamException();
				}
				if (num5 < 0 || (long)num5 > 4L)
				{
					throw new InvalidParamException();
				}
				_numLiteralPosStateBits = num5;
				break;
			case CoderPropId.LitContextBits:
				if (!(obj is int num4))
				{
					throw new InvalidParamException();
				}
				if (num4 < 0 || (long)num4 > 8L)
				{
					throw new InvalidParamException();
				}
				_numLiteralContextBits = num4;
				break;
			case CoderPropId.EndMarker:
				if (!(obj is bool))
				{
					throw new InvalidParamException();
				}
				SetWriteEndMarkerMode((bool)obj);
				break;
			default:
				throw new InvalidParamException();
			case CoderPropId.Algorithm:
				break;
			}
		}
	}

	public void SetTrainSize(uint trainSize)
	{
		_trainSize = trainSize;
	}
}
