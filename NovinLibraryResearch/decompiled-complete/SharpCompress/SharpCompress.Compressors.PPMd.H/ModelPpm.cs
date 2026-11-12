using System;
using System.IO;
using System.Text;
using SharpCompress.Compressors.LZMA.RangeCoder;
using SharpCompress.Compressors.Rar;

namespace SharpCompress.Compressors.PPMd.H;

internal class ModelPpm
{
	public const int MAX_O = 64;

	public const int INT_BITS = 7;

	public const int PERIOD_BITS = 7;

	public static readonly int TOT_BITS = 14;

	public static readonly int INTERVAL = 128;

	public static readonly int BIN_SCALE = 1 << TOT_BITS;

	public const int MAX_FREQ = 124;

	private readonly See2Context[][] _see2Cont = new See2Context[25][];

	private See2Context _dummySee2Cont;

	private PpmContext _minContext;

	private PpmContext _maxContext;

	private int _numMasked;

	private int _initEsc;

	private int _orderFall;

	private int _maxOrder;

	private int _runLength;

	private int _initRl;

	private readonly int[] _charMask = new int[256];

	private readonly int[] _ns2Indx = new int[256];

	private readonly int[] _ns2BsIndx = new int[256];

	private readonly int[] _hb2Flag = new int[256];

	private int _escCount;

	private int _prevSuccess;

	private int _hiBitsFlag;

	private readonly int[][] _binSumm = new int[128][];

	private static readonly int[] INIT_BIN_ESC = new int[8] { 15581, 7999, 22975, 18675, 25761, 23228, 26162, 24657 };

	private readonly State _tempState1 = new State(null);

	private readonly State _tempState2 = new State(null);

	private readonly State _tempState3 = new State(null);

	private readonly State _tempState4 = new State(null);

	private readonly StateRef _tempStateRef1 = new StateRef();

	private readonly StateRef _tempStateRef2 = new StateRef();

	private readonly PpmContext _tempPpmContext1 = new PpmContext(null);

	private readonly PpmContext _tempPpmContext2 = new PpmContext(null);

	private readonly PpmContext _tempPpmContext3 = new PpmContext(null);

	private readonly PpmContext _tempPpmContext4 = new PpmContext(null);

	private readonly int[] _ps = new int[64];

	public SubAllocator SubAlloc { get; } = new SubAllocator();


	public virtual See2Context DummySee2Cont => _dummySee2Cont;

	public virtual int InitRl => _initRl;

	public virtual int EscCount
	{
		get
		{
			return _escCount;
		}
		set
		{
			_escCount = value & 0xFF;
		}
	}

	public virtual int[] CharMask => _charMask;

	public virtual int NumMasked
	{
		get
		{
			return _numMasked;
		}
		set
		{
			_numMasked = value;
		}
	}

	public virtual int PrevSuccess
	{
		get
		{
			return _prevSuccess;
		}
		set
		{
			_prevSuccess = value & 0xFF;
		}
	}

	public virtual int InitEsc
	{
		get
		{
			return _initEsc;
		}
		set
		{
			_initEsc = value;
		}
	}

	public virtual int RunLength
	{
		get
		{
			return _runLength;
		}
		set
		{
			_runLength = value;
		}
	}

	public virtual int HiBitsFlag
	{
		get
		{
			return _hiBitsFlag;
		}
		set
		{
			_hiBitsFlag = value & 0xFF;
		}
	}

	public virtual int[][] BinSumm => _binSumm;

	internal RangeCoder Coder { get; private set; }

	internal State FoundState { get; private set; }

	public virtual byte[] Heap => SubAlloc.Heap;

	public virtual int OrderFall => _orderFall;

	private void InitBlock()
	{
		for (int i = 0; i < 25; i++)
		{
			_see2Cont[i] = new See2Context[16];
		}
		for (int j = 0; j < 128; j++)
		{
			_binSumm[j] = new int[64];
		}
	}

	public ModelPpm()
	{
		InitBlock();
		_minContext = null;
		_maxContext = null;
	}

	private void RestartModelRare()
	{
		new Span<int>(_charMask).Clear();
		SubAlloc.InitSubAllocator();
		_initRl = -((_maxOrder < 12) ? _maxOrder : 12) - 1;
		int address = SubAlloc.AllocContext();
		_minContext.Address = address;
		_maxContext.Address = address;
		_minContext.SetSuffix(0);
		_orderFall = _maxOrder;
		_minContext.NumStats = 256;
		_minContext.FreqData.SummFreq = _minContext.NumStats + 1;
		address = SubAlloc.AllocUnits(128);
		FoundState.Address = address;
		_minContext.FreqData.SetStats(address);
		State state = new State(SubAlloc.Heap);
		address = _minContext.FreqData.GetStats();
		_runLength = _initRl;
		_prevSuccess = 0;
		for (int i = 0; i < 256; i++)
		{
			state.Address = address + i * 6;
			state.Symbol = i;
			state.Freq = 1;
			state.SetSuccessor(0);
		}
		for (int j = 0; j < 128; j++)
		{
			for (int k = 0; k < 8; k++)
			{
				for (int l = 0; l < 64; l += 8)
				{
					_binSumm[j][k + l] = BIN_SCALE - INIT_BIN_ESC[k] / (j + 2);
				}
			}
		}
		for (int m = 0; m < 25; m++)
		{
			for (int n = 0; n < 16; n++)
			{
				_see2Cont[m][n].Initialize(5 * m + 10);
			}
		}
	}

	private void StartModelRare(int maxOrder)
	{
		_escCount = 1;
		_maxOrder = maxOrder;
		RestartModelRare();
		_ns2BsIndx[0] = 0;
		_ns2BsIndx[1] = 2;
		for (int i = 0; i < 9; i++)
		{
			_ns2BsIndx[2 + i] = 4;
		}
		for (int j = 0; j < 245; j++)
		{
			_ns2BsIndx[11 + j] = 6;
		}
		int k;
		for (k = 0; k < 3; k++)
		{
			_ns2Indx[k] = k;
		}
		int num = k;
		int num2 = 1;
		int num3 = 1;
		for (; k < 256; k++)
		{
			_ns2Indx[k] = num;
			if (--num2 == 0)
			{
				num2 = ++num3;
				num++;
			}
		}
		for (int l = 0; l < 64; l++)
		{
			_hb2Flag[l] = 0;
		}
		for (int m = 0; m < 192; m++)
		{
			_hb2Flag[64 + m] = 8;
		}
		_dummySee2Cont.Shift = 7;
	}

	private void ClearMask()
	{
		_escCount = 1;
		new Span<int>(_charMask).Clear();
	}

	internal bool DecodeInit(IRarUnpack unpackRead, int escChar)
	{
		int num = unpackRead.Char & 0xFF;
		bool flag = (num & 0x20) != 0;
		int num2 = 0;
		if (flag)
		{
			num2 = unpackRead.Char;
		}
		else if (SubAlloc.GetAllocatedMemory() == 0)
		{
			return false;
		}
		if (((uint)num & 0x40u) != 0)
		{
			escChar = unpackRead.Char;
			unpackRead.PpmEscChar = escChar;
		}
		Coder = new RangeCoder(unpackRead);
		if (flag)
		{
			num = (num & 0x1F) + 1;
			if (num > 16)
			{
				num = 16 + (num - 16) * 3;
			}
			if (num == 1)
			{
				SubAlloc.StopSubAllocator();
				return false;
			}
			SubAlloc.StartSubAllocator(num2 + 1 << 20);
			_minContext = new PpmContext(Heap);
			_maxContext = new PpmContext(Heap);
			FoundState = new State(Heap);
			_dummySee2Cont = new See2Context();
			for (int i = 0; i < 25; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					_see2Cont[i][j] = new See2Context();
				}
			}
			StartModelRare(num);
		}
		return _minContext.Address != 0;
	}

	public virtual int DecodeChar()
	{
		if (_minContext.Address <= SubAlloc.PText || _minContext.Address > SubAlloc.HeapEnd)
		{
			return -1;
		}
		if (_minContext.NumStats != 1)
		{
			if (_minContext.FreqData.GetStats() <= SubAlloc.PText || _minContext.FreqData.GetStats() > SubAlloc.HeapEnd)
			{
				return -1;
			}
			if (!_minContext.DecodeSymbol1(this))
			{
				return -1;
			}
		}
		else
		{
			_minContext.DecodeBinSymbol(this);
		}
		Coder.Decode();
		while (FoundState.Address == 0)
		{
			Coder.AriDecNormalize();
			do
			{
				_orderFall++;
				_minContext.Address = _minContext.GetSuffix();
				if (_minContext.Address <= SubAlloc.PText || _minContext.Address > SubAlloc.HeapEnd)
				{
					return -1;
				}
			}
			while (_minContext.NumStats == _numMasked);
			if (!_minContext.DecodeSymbol2(this))
			{
				return -1;
			}
			Coder.Decode();
		}
		int symbol = FoundState.Symbol;
		if (_orderFall == 0 && FoundState.GetSuccessor() > SubAlloc.PText)
		{
			int successor = FoundState.GetSuccessor();
			_minContext.Address = successor;
			_maxContext.Address = successor;
		}
		else
		{
			UpdateModel();
			if (_escCount == 0)
			{
				ClearMask();
			}
		}
		Coder.AriDecNormalize();
		return symbol;
	}

	public virtual See2Context[][] GetSee2Cont()
	{
		return _see2Cont;
	}

	public virtual void IncEscCount(int dEscCount)
	{
		EscCount += dEscCount;
	}

	public virtual void IncRunLength(int dRunLength)
	{
		RunLength += dRunLength;
	}

	public virtual int[] GetHb2Flag()
	{
		return _hb2Flag;
	}

	public virtual int[] GetNs2BsIndx()
	{
		return _ns2BsIndx;
	}

	public virtual int[] GetNs2Indx()
	{
		return _ns2Indx;
	}

	private int CreateSuccessors(bool skip, State p1)
	{
		StateRef tempStateRef = _tempStateRef2;
		State state = _tempState1.Initialize(Heap);
		PpmContext ppmContext = _tempPpmContext1.Initialize(Heap);
		ppmContext.Address = _minContext.Address;
		PpmContext ppmContext2 = _tempPpmContext2.Initialize(Heap);
		ppmContext2.Address = FoundState.GetSuccessor();
		State state2 = _tempState2.Initialize(Heap);
		int num = 0;
		bool flag = false;
		if (!skip)
		{
			_ps[num++] = FoundState.Address;
			if (ppmContext.GetSuffix() == 0)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			bool flag2 = false;
			if (p1.Address != 0)
			{
				state2.Address = p1.Address;
				ppmContext.Address = ppmContext.GetSuffix();
				flag2 = true;
			}
			do
			{
				if (!flag2)
				{
					ppmContext.Address = ppmContext.GetSuffix();
					if (ppmContext.NumStats != 1)
					{
						state2.Address = ppmContext.FreqData.GetStats();
						if (state2.Symbol != FoundState.Symbol)
						{
							do
							{
								state2.IncrementAddress();
							}
							while (state2.Symbol != FoundState.Symbol);
						}
					}
					else
					{
						state2.Address = ppmContext.GetOneState().Address;
					}
				}
				flag2 = false;
				if (state2.GetSuccessor() != ppmContext2.Address)
				{
					ppmContext.Address = state2.GetSuccessor();
					break;
				}
				_ps[num++] = state2.Address;
			}
			while (ppmContext.GetSuffix() != 0);
		}
		if (num == 0)
		{
			return ppmContext.Address;
		}
		tempStateRef.Symbol = Heap[ppmContext2.Address];
		tempStateRef.SetSuccessor(ppmContext2.Address + 1);
		if (ppmContext.NumStats != 1)
		{
			if (ppmContext.Address <= SubAlloc.PText)
			{
				return 0;
			}
			state2.Address = ppmContext.FreqData.GetStats();
			if (state2.Symbol != tempStateRef.Symbol)
			{
				do
				{
					state2.IncrementAddress();
				}
				while (state2.Symbol != tempStateRef.Symbol);
			}
			int num2 = state2.Freq - 1;
			int num3 = ppmContext.FreqData.SummFreq - ppmContext.NumStats - num2;
			tempStateRef.Freq = 1 + ((2 * num2 > num3) ? ((2 * num2 + 3 * num3 - 1) / (2 * num3)) : ((5 * num2 > num3) ? 1 : 0));
		}
		else
		{
			tempStateRef.Freq = ppmContext.GetOneState().Freq;
		}
		do
		{
			state.Address = _ps[--num];
			ppmContext.Address = ppmContext.CreateChild(this, state, tempStateRef);
			if (ppmContext.Address == 0)
			{
				return 0;
			}
		}
		while (num != 0);
		return ppmContext.Address;
	}

	private void UpdateModelRestart()
	{
		RestartModelRare();
		_escCount = 0;
	}

	private void UpdateModel()
	{
		StateRef tempStateRef = _tempStateRef1;
		tempStateRef.Values = FoundState;
		State state = _tempState3.Initialize(Heap);
		State state2 = _tempState4.Initialize(Heap);
		PpmContext ppmContext = _tempPpmContext3.Initialize(Heap);
		PpmContext ppmContext2 = _tempPpmContext4.Initialize(Heap);
		ppmContext.Address = _minContext.GetSuffix();
		if (tempStateRef.Freq < 31 && ppmContext.Address != 0)
		{
			if (ppmContext.NumStats != 1)
			{
				state.Address = ppmContext.FreqData.GetStats();
				if (state.Symbol != tempStateRef.Symbol)
				{
					do
					{
						state.IncrementAddress();
					}
					while (state.Symbol != tempStateRef.Symbol);
					state2.Address = state.Address - 6;
					if (state.Freq >= state2.Freq)
					{
						State.PpmdSwap(state, state2);
						state.DecrementAddress();
					}
				}
				if (state.Freq < 115)
				{
					state.IncrementFreq(2);
					ppmContext.FreqData.IncrementSummFreq(2);
				}
			}
			else
			{
				state.Address = ppmContext.GetOneState().Address;
				if (state.Freq < 32)
				{
					state.IncrementFreq(1);
				}
			}
		}
		if (_orderFall == 0)
		{
			FoundState.SetSuccessor(CreateSuccessors(skip: true, state));
			_minContext.Address = FoundState.GetSuccessor();
			_maxContext.Address = FoundState.GetSuccessor();
			if (_minContext.Address == 0)
			{
				UpdateModelRestart();
			}
			return;
		}
		SubAlloc.Heap[SubAlloc.PText] = (byte)tempStateRef.Symbol;
		SubAlloc.IncPText();
		ppmContext2.Address = SubAlloc.PText;
		if (SubAlloc.PText >= SubAlloc.FakeUnitsStart)
		{
			UpdateModelRestart();
			return;
		}
		if (tempStateRef.GetSuccessor() != 0)
		{
			if (tempStateRef.GetSuccessor() <= SubAlloc.PText)
			{
				tempStateRef.SetSuccessor(CreateSuccessors(skip: false, state));
				if (tempStateRef.GetSuccessor() == 0)
				{
					UpdateModelRestart();
					return;
				}
			}
			if (--_orderFall == 0)
			{
				ppmContext2.Address = tempStateRef.GetSuccessor();
				if (_maxContext.Address != _minContext.Address)
				{
					SubAlloc.DecPText(1);
				}
			}
		}
		else
		{
			FoundState.SetSuccessor(ppmContext2.Address);
			tempStateRef.SetSuccessor(_minContext);
		}
		int numStats = _minContext.NumStats;
		int num = _minContext.FreqData.SummFreq - numStats - (tempStateRef.Freq - 1);
		ppmContext.Address = _maxContext.Address;
		while (ppmContext.Address != _minContext.Address)
		{
			int numStats2;
			if ((numStats2 = ppmContext.NumStats) != 1)
			{
				if ((numStats2 & 1) == 0)
				{
					ppmContext.FreqData.SetStats(SubAlloc.ExpandUnits(ppmContext.FreqData.GetStats(), Utility.URShift(numStats2, 1)));
					if (ppmContext.FreqData.GetStats() == 0)
					{
						UpdateModelRestart();
						return;
					}
				}
				int dSummFreq = ((2 * numStats2 < numStats) ? 1 : 0) + 2 * (((4 * numStats2 <= numStats) ? 1 : 0) & ((ppmContext.FreqData.SummFreq <= 8 * numStats2) ? 1 : 0));
				ppmContext.FreqData.IncrementSummFreq(dSummFreq);
			}
			else
			{
				state.Address = SubAlloc.AllocUnits(1);
				if (state.Address == 0)
				{
					UpdateModelRestart();
					return;
				}
				state.SetValues(ppmContext.GetOneState());
				ppmContext.FreqData.SetStats(state);
				if (state.Freq < 30)
				{
					state.IncrementFreq(state.Freq);
				}
				else
				{
					state.Freq = 120;
				}
				ppmContext.FreqData.SummFreq = state.Freq + _initEsc + ((numStats > 3) ? 1 : 0);
			}
			int num2 = 2 * tempStateRef.Freq * (ppmContext.FreqData.SummFreq + 6);
			int num3 = num + ppmContext.FreqData.SummFreq;
			if (num2 < 6 * num3)
			{
				num2 = 1 + ((num2 > num3) ? 1 : 0) + ((num2 >= 4 * num3) ? 1 : 0);
				ppmContext.FreqData.IncrementSummFreq(3);
			}
			else
			{
				num2 = 4 + ((num2 >= 9 * num3) ? 1 : 0) + ((num2 >= 12 * num3) ? 1 : 0) + ((num2 >= 15 * num3) ? 1 : 0);
				ppmContext.FreqData.IncrementSummFreq(num2);
			}
			state.Address = ppmContext.FreqData.GetStats() + numStats2 * 6;
			state.SetSuccessor(ppmContext2);
			state.Symbol = tempStateRef.Symbol;
			state.Freq = num2;
			numStats2 = (ppmContext.NumStats = numStats2 + 1);
			ppmContext.Address = ppmContext.GetSuffix();
		}
		int successor = tempStateRef.GetSuccessor();
		_maxContext.Address = successor;
		_minContext.Address = successor;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("ModelPPM[");
		stringBuilder.Append("\n  numMasked=");
		stringBuilder.Append(_numMasked);
		stringBuilder.Append("\n  initEsc=");
		stringBuilder.Append(_initEsc);
		stringBuilder.Append("\n  orderFall=");
		stringBuilder.Append(_orderFall);
		stringBuilder.Append("\n  maxOrder=");
		stringBuilder.Append(_maxOrder);
		stringBuilder.Append("\n  runLength=");
		stringBuilder.Append(_runLength);
		stringBuilder.Append("\n  initRL=");
		stringBuilder.Append(_initRl);
		stringBuilder.Append("\n  escCount=");
		stringBuilder.Append(_escCount);
		stringBuilder.Append("\n  prevSuccess=");
		stringBuilder.Append(_prevSuccess);
		stringBuilder.Append("\n  foundState=");
		stringBuilder.Append(FoundState);
		stringBuilder.Append("\n  coder=");
		stringBuilder.Append(Coder);
		stringBuilder.Append("\n  subAlloc=");
		stringBuilder.Append(SubAlloc);
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}

	internal bool DecodeInit(Stream stream, int maxOrder, int maxMemory)
	{
		if (stream != null)
		{
			Coder = new RangeCoder(stream);
		}
		if (maxOrder == 1)
		{
			SubAlloc.StopSubAllocator();
			return false;
		}
		SubAlloc.StartSubAllocator(maxMemory);
		_minContext = new PpmContext(Heap);
		_maxContext = new PpmContext(Heap);
		FoundState = new State(Heap);
		_dummySee2Cont = new See2Context();
		for (int i = 0; i < 25; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				_see2Cont[i][j] = new See2Context();
			}
		}
		StartModelRare(maxOrder);
		return _minContext.Address != 0;
	}

	internal void NextContext()
	{
		int successor = FoundState.GetSuccessor();
		if (_orderFall == 0 && successor > SubAlloc.PText)
		{
			_minContext.Address = successor;
			_maxContext.Address = successor;
		}
		else
		{
			UpdateModel();
		}
	}

	public int DecodeChar(SharpCompress.Compressors.LZMA.RangeCoder.Decoder decoder)
	{
		if (_minContext.NumStats != 1)
		{
			State state = _tempState1.Initialize(Heap);
			state.Address = _minContext.FreqData.GetStats();
			int threshold;
			int num;
			if ((threshold = (int)decoder.GetThreshold((uint)_minContext.FreqData.SummFreq)) < (num = state.Freq))
			{
				decoder.Decode(0u, (uint)state.Freq);
				byte result = (byte)state.Symbol;
				_minContext.update1_0(this, state.Address);
				NextContext();
				return result;
			}
			_prevSuccess = 0;
			int num2 = _minContext.NumStats - 1;
			do
			{
				state.IncrementAddress();
				if ((num += state.Freq) > threshold)
				{
					decoder.Decode((uint)(num - state.Freq), (uint)state.Freq);
					byte result2 = (byte)state.Symbol;
					_minContext.Update1(this, state.Address);
					NextContext();
					return result2;
				}
			}
			while (--num2 > 0);
			if (threshold >= _minContext.FreqData.SummFreq)
			{
				return -2;
			}
			_hiBitsFlag = _hb2Flag[FoundState.Symbol];
			decoder.Decode((uint)num, (uint)(_minContext.FreqData.SummFreq - num));
			for (num2 = 0; num2 < 256; num2++)
			{
				_charMask[num2] = -1;
			}
			_charMask[state.Symbol] = 0;
			num2 = _minContext.NumStats - 1;
			do
			{
				state.DecrementAddress();
				_charMask[state.Symbol] = 0;
			}
			while (--num2 > 0);
		}
		else
		{
			State state2 = _tempState1.Initialize(Heap);
			state2.Address = _minContext.GetOneState().Address;
			_hiBitsFlag = GetHb2Flag()[FoundState.Symbol];
			int num3 = state2.Freq - 1;
			int arrayIndex = _minContext.GetArrayIndex(this, state2);
			int num4 = _binSumm[num3][arrayIndex];
			if (decoder.DecodeBit((uint)num4, 14) == 0)
			{
				_binSumm[num3][arrayIndex] = (num4 + INTERVAL - _minContext.GetMean(num4, 7, 2)) & 0xFFFF;
				FoundState.Address = state2.Address;
				byte result3 = (byte)state2.Symbol;
				state2.IncrementFreq((state2.Freq < 128) ? 1 : 0);
				_prevSuccess = 1;
				IncRunLength(1);
				NextContext();
				return result3;
			}
			num4 = (num4 - _minContext.GetMean(num4, 7, 2)) & 0xFFFF;
			_binSumm[num3][arrayIndex] = num4;
			_initEsc = PpmContext.EXP_ESCAPE[Utility.URShift(num4, 10)];
			for (int i = 0; i < 256; i++)
			{
				_charMask[i] = -1;
			}
			_charMask[state2.Symbol] = 0;
			_prevSuccess = 0;
		}
		while (true)
		{
			State state3 = _tempState1.Initialize(Heap);
			int numStats = _minContext.NumStats;
			do
			{
				_orderFall++;
				_minContext.Address = _minContext.GetSuffix();
				if (_minContext.Address <= SubAlloc.PText || _minContext.Address > SubAlloc.HeapEnd)
				{
					return -1;
				}
			}
			while (_minContext.NumStats == numStats);
			int num5 = 0;
			state3.Address = _minContext.FreqData.GetStats();
			int num6 = 0;
			int num7 = _minContext.NumStats - numStats;
			do
			{
				int num8 = _charMask[state3.Symbol];
				num5 += state3.Freq & num8;
				_minContext._ps[num6] = state3.Address;
				state3.IncrementAddress();
				num6 -= num8;
			}
			while (num6 != num7);
			int escFreq;
			See2Context see2Context = _minContext.MakeEscFreq(this, numStats, out escFreq);
			escFreq += num5;
			int threshold2 = (int)decoder.GetThreshold((uint)escFreq);
			if (threshold2 < num5)
			{
				State state4 = _tempState2.Initialize(Heap);
				num5 = 0;
				num6 = 0;
				state4.Address = _minContext._ps[num6];
				while ((num5 += state4.Freq) <= threshold2)
				{
					num6++;
					state4.Address = _minContext._ps[num6];
				}
				state3.Address = state4.Address;
				decoder.Decode((uint)(num5 - state3.Freq), (uint)state3.Freq);
				see2Context.Update();
				byte result4 = (byte)state3.Symbol;
				_minContext.Update2(this, state3.Address);
				UpdateModel();
				return result4;
			}
			if (threshold2 >= escFreq)
			{
				break;
			}
			decoder.Decode((uint)num5, (uint)(escFreq - num5));
			see2Context.Summ += escFreq;
			do
			{
				state3.Address = _minContext._ps[--num6];
				_charMask[state3.Symbol] = 0;
			}
			while (num6 != 0);
		}
		return -2;
	}
}
