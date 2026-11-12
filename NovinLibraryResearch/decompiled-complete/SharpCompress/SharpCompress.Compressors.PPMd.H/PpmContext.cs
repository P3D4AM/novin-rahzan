using System;
using System.Buffers.Binary;
using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class PpmContext : Pointer
{
	private static readonly int UNION_SIZE;

	public static readonly int SIZE;

	private int _numStats;

	private readonly FreqData _freqData;

	private readonly State _oneState;

	private int _suffix;

	public static readonly int[] EXP_ESCAPE;

	private readonly State _tempState1 = new State(null);

	private readonly State _tempState2 = new State(null);

	private readonly State _tempState3 = new State(null);

	private readonly State _tempState4 = new State(null);

	private readonly State _tempState5 = new State(null);

	private PpmContext _tempPpmContext;

	internal int[] _ps = new int[256];

	internal FreqData FreqData
	{
		get
		{
			return _freqData;
		}
		set
		{
			_freqData.SummFreq = value.SummFreq;
			_freqData.SetStats(value.GetStats());
		}
	}

	public virtual int NumStats
	{
		get
		{
			if (base.Memory != null)
			{
				_numStats = BinaryPrimitives.ReadInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address)) & 0xFFFF;
			}
			return _numStats;
		}
		set
		{
			_numStats = value & 0xFFFF;
			if (base.Memory != null)
			{
				BinaryPrimitives.WriteInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address), (short)value);
			}
		}
	}

	internal override int Address
	{
		get
		{
			return base.Address;
		}
		set
		{
			base.Address = value;
			_oneState.Address = value + 2;
			_freqData.Address = value + 2;
		}
	}

	public PpmContext(byte[] memory)
		: base(memory)
	{
		_oneState = new State(memory);
		_freqData = new FreqData(memory);
	}

	internal PpmContext Initialize(byte[] mem)
	{
		_oneState.Initialize(mem);
		_freqData.Initialize(mem);
		return Initialize<PpmContext>(mem);
	}

	internal State GetOneState()
	{
		return _oneState;
	}

	internal void SetOneState(StateRef oneState)
	{
		_oneState.SetValues(oneState);
	}

	internal int GetSuffix()
	{
		if (base.Memory != null)
		{
			_suffix = BinaryPrimitives.ReadInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 8));
		}
		return _suffix;
	}

	internal void SetSuffix(PpmContext suffix)
	{
		SetSuffix(suffix.Address);
	}

	internal void SetSuffix(int suffix)
	{
		_suffix = suffix;
		if (base.Memory != null)
		{
			BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 8), suffix);
		}
	}

	private PpmContext GetTempPpmContext(byte[] memory)
	{
		if (_tempPpmContext == null)
		{
			_tempPpmContext = new PpmContext(null);
		}
		return _tempPpmContext.Initialize(memory);
	}

	internal int CreateChild(ModelPpm model, State pStats, StateRef firstState)
	{
		PpmContext tempPpmContext = GetTempPpmContext(model.SubAlloc.Heap);
		tempPpmContext.Address = model.SubAlloc.AllocContext();
		if (tempPpmContext != null)
		{
			tempPpmContext.NumStats = 1;
			tempPpmContext.SetOneState(firstState);
			tempPpmContext.SetSuffix(this);
			pStats.SetSuccessor(tempPpmContext);
		}
		return tempPpmContext.Address;
	}

	internal void Rescale(ModelPpm model)
	{
		int numStats = NumStats;
		int num = NumStats - 1;
		State state = new State(model.Heap);
		State state2 = new State(model.Heap);
		State state3 = new State(model.Heap);
		state2.Address = model.FoundState.Address;
		while (state2.Address != _freqData.GetStats())
		{
			state3.Address = state2.Address - 6;
			State.PpmdSwap(state2, state3);
			state2.DecrementAddress();
		}
		state3.Address = _freqData.GetStats();
		state3.IncrementFreq(4);
		_freqData.IncrementSummFreq(4);
		int num2 = _freqData.SummFreq - state2.Freq;
		int num3 = ((model.OrderFall != 0) ? 1 : 0);
		state2.Freq = Utility.URShift(state2.Freq + num3, 1);
		_freqData.SummFreq = state2.Freq;
		do
		{
			state2.IncrementAddress();
			num2 -= state2.Freq;
			state2.Freq = Utility.URShift(state2.Freq + num3, 1);
			_freqData.IncrementSummFreq(state2.Freq);
			state3.Address = state2.Address - 6;
			if (state2.Freq > state3.Freq)
			{
				state.Address = state2.Address;
				StateRef stateRef = new StateRef();
				stateRef.Values = state;
				State state4 = new State(model.Heap);
				State state5 = new State(model.Heap);
				do
				{
					state4.Address = state.Address - 6;
					state.SetValues(state4);
					state.DecrementAddress();
					state5.Address = state.Address - 6;
				}
				while (state.Address != _freqData.GetStats() && stateRef.Freq > state5.Freq);
				state.SetValues(stateRef);
			}
		}
		while (--num != 0);
		if (state2.Freq == 0)
		{
			do
			{
				num++;
				state2.DecrementAddress();
			}
			while (state2.Freq == 0);
			num2 += num;
			NumStats -= num;
			if (NumStats == 1)
			{
				StateRef stateRef2 = new StateRef();
				state3.Address = _freqData.GetStats();
				stateRef2.Values = state3;
				do
				{
					stateRef2.DecrementFreq(Utility.URShift(stateRef2.Freq, 1));
					num2 = Utility.URShift(num2, 1);
				}
				while (num2 > 1);
				model.SubAlloc.FreeUnits(_freqData.GetStats(), Utility.URShift(numStats + 1, 1));
				_oneState.SetValues(stateRef2);
				model.FoundState.Address = _oneState.Address;
				return;
			}
		}
		num2 -= Utility.URShift(num2, 1);
		_freqData.IncrementSummFreq(num2);
		int num4 = Utility.URShift(numStats + 1, 1);
		int num5 = Utility.URShift(NumStats + 1, 1);
		if (num4 != num5)
		{
			_freqData.SetStats(model.SubAlloc.ShrinkUnits(_freqData.GetStats(), num4, num5));
		}
		model.FoundState.Address = _freqData.GetStats();
	}

	internal int GetArrayIndex(ModelPpm model, State rs)
	{
		PpmContext tempPpmContext = GetTempPpmContext(model.SubAlloc.Heap);
		tempPpmContext.Address = GetSuffix();
		return 0 + model.PrevSuccess + model.GetNs2BsIndx()[tempPpmContext.NumStats - 1] + (model.HiBitsFlag + 2 * model.GetHb2Flag()[rs.Symbol]) + (Utility.URShift(model.RunLength, 26) & 0x20);
	}

	internal int GetMean(int summ, int shift, int round)
	{
		return Utility.URShift(summ + (1 << shift - round), shift);
	}

	internal void DecodeBinSymbol(ModelPpm model)
	{
		State state = _tempState1.Initialize(model.Heap);
		state.Address = _oneState.Address;
		model.HiBitsFlag = model.GetHb2Flag()[model.FoundState.Symbol];
		int num = state.Freq - 1;
		int arrayIndex = GetArrayIndex(model, state);
		int num2 = model.BinSumm[num][arrayIndex];
		if (model.Coder.GetCurrentShiftCount(ModelPpm.TOT_BITS) < num2)
		{
			model.FoundState.Address = state.Address;
			state.IncrementFreq((state.Freq < 128) ? 1 : 0);
			model.Coder.SubRange.LowCount = 0L;
			model.Coder.SubRange.HighCount = num2;
			num2 = (num2 + ModelPpm.INTERVAL - GetMean(num2, 7, 2)) & 0xFFFF;
			model.BinSumm[num][arrayIndex] = num2;
			model.PrevSuccess = 1;
			model.IncRunLength(1);
		}
		else
		{
			model.Coder.SubRange.LowCount = num2;
			num2 = (num2 - GetMean(num2, 7, 2)) & 0xFFFF;
			model.BinSumm[num][arrayIndex] = num2;
			model.Coder.SubRange.HighCount = ModelPpm.BIN_SCALE;
			model.InitEsc = EXP_ESCAPE[Utility.URShift(num2, 10)];
			model.NumMasked = 1;
			model.CharMask[state.Symbol] = model.EscCount;
			model.PrevSuccess = 0;
			model.FoundState.Address = 0;
		}
	}

	internal void Update1(ModelPpm model, int p)
	{
		model.FoundState.Address = p;
		model.FoundState.IncrementFreq(4);
		_freqData.IncrementSummFreq(4);
		State state = _tempState3.Initialize(model.Heap);
		State state2 = _tempState4.Initialize(model.Heap);
		state.Address = p;
		state2.Address = p - 6;
		if (state.Freq > state2.Freq)
		{
			State.PpmdSwap(state, state2);
			model.FoundState.Address = state2.Address;
			if (state2.Freq > 124)
			{
				Rescale(model);
			}
		}
	}

	internal void update1_0(ModelPpm model, int p)
	{
		model.FoundState.Address = p;
		model.PrevSuccess = ((2 * model.FoundState.Freq > _freqData.SummFreq) ? 1 : 0);
		model.IncRunLength(model.PrevSuccess);
		_freqData.IncrementSummFreq(4);
		model.FoundState.IncrementFreq(4);
		if (model.FoundState.Freq > 124)
		{
			Rescale(model);
		}
	}

	internal bool DecodeSymbol2(ModelPpm model)
	{
		int num = NumStats - model.NumMasked;
		See2Context see2Context = MakeEscFreq2(model, num);
		RangeCoder coder = model.Coder;
		State state = _tempState1.Initialize(model.Heap);
		State state2 = _tempState2.Initialize(model.Heap);
		state.Address = _freqData.GetStats() - 6;
		int num2 = 0;
		int num3 = 0;
		while (true)
		{
			state.IncrementAddress();
			if (model.CharMask[state.Symbol] != model.EscCount)
			{
				num3 += state.Freq;
				_ps[num2++] = state.Address;
				if (--num == 0)
				{
					break;
				}
			}
		}
		coder.SubRange.IncScale(num3);
		long num4 = coder.CurrentCount;
		if (num4 >= coder.SubRange.Scale)
		{
			return false;
		}
		num2 = 0;
		state.Address = _ps[num2];
		if (num4 < num3)
		{
			num3 = 0;
			while ((num3 += state.Freq) <= num4)
			{
				state.Address = _ps[++num2];
			}
			coder.SubRange.HighCount = num3;
			coder.SubRange.LowCount = num3 - state.Freq;
			see2Context.Update();
			Update2(model, state.Address);
		}
		else
		{
			coder.SubRange.LowCount = num3;
			coder.SubRange.HighCount = coder.SubRange.Scale;
			num = NumStats - model.NumMasked;
			num2--;
			do
			{
				state2.Address = _ps[++num2];
				model.CharMask[state2.Symbol] = model.EscCount;
			}
			while (--num != 0);
			see2Context.IncSumm((int)coder.SubRange.Scale);
			model.NumMasked = NumStats;
		}
		return true;
	}

	internal void Update2(ModelPpm model, int p)
	{
		State state = _tempState5.Initialize(model.Heap);
		state.Address = p;
		model.FoundState.Address = p;
		model.FoundState.IncrementFreq(4);
		_freqData.IncrementSummFreq(4);
		if (state.Freq > 124)
		{
			Rescale(model);
		}
		model.IncEscCount(1);
		model.RunLength = model.InitRl;
	}

	private See2Context MakeEscFreq2(ModelPpm model, int diff)
	{
		int numStats = NumStats;
		See2Context see2Context;
		if (numStats != 256)
		{
			PpmContext tempPpmContext = GetTempPpmContext(model.Heap);
			tempPpmContext.Address = GetSuffix();
			int num = model.GetNs2Indx()[diff - 1];
			int num2 = 0;
			num2 += ((diff < tempPpmContext.NumStats - numStats) ? 1 : 0);
			num2 += 2 * ((_freqData.SummFreq < 11 * numStats) ? 1 : 0);
			num2 += 4 * ((model.NumMasked > diff) ? 1 : 0);
			num2 += model.HiBitsFlag;
			see2Context = model.GetSee2Cont()[num][num2];
			model.Coder.SubRange.Scale = see2Context.Mean;
		}
		else
		{
			see2Context = model.DummySee2Cont;
			model.Coder.SubRange.Scale = 1L;
		}
		return see2Context;
	}

	internal See2Context MakeEscFreq(ModelPpm model, int numMasked, out int escFreq)
	{
		int numStats = NumStats;
		int num = numStats - numMasked;
		See2Context see2Context;
		if (numStats != 256)
		{
			PpmContext tempPpmContext = GetTempPpmContext(model.Heap);
			tempPpmContext.Address = GetSuffix();
			int num2 = model.GetNs2Indx()[num - 1];
			int num3 = 0;
			num3 += ((num < tempPpmContext.NumStats - numStats) ? 1 : 0);
			num3 += 2 * ((_freqData.SummFreq < 11 * numStats) ? 1 : 0);
			num3 += 4 * ((numMasked > num) ? 1 : 0);
			num3 += model.HiBitsFlag;
			see2Context = model.GetSee2Cont()[num2][num3];
			escFreq = see2Context.Mean;
		}
		else
		{
			see2Context = model.DummySee2Cont;
			escFreq = 1;
		}
		return see2Context;
	}

	internal bool DecodeSymbol1(ModelPpm model)
	{
		RangeCoder coder = model.Coder;
		coder.SubRange.Scale = _freqData.SummFreq;
		State state = new State(model.Heap);
		state.Address = _freqData.GetStats();
		long num = coder.CurrentCount;
		if (num >= coder.SubRange.Scale)
		{
			return false;
		}
		int num2;
		if (num < (num2 = state.Freq))
		{
			coder.SubRange.HighCount = num2;
			model.PrevSuccess = ((2 * num2 > coder.SubRange.Scale) ? 1 : 0);
			model.IncRunLength(model.PrevSuccess);
			num2 += 4;
			model.FoundState.Address = state.Address;
			model.FoundState.Freq = num2;
			_freqData.IncrementSummFreq(4);
			if (num2 > 124)
			{
				Rescale(model);
			}
			coder.SubRange.LowCount = 0L;
			return true;
		}
		if (model.FoundState.Address == 0)
		{
			return false;
		}
		model.PrevSuccess = 0;
		int numStats = NumStats;
		int num3 = numStats - 1;
		while ((num2 += state.IncrementAddress().Freq) <= num)
		{
			if (--num3 == 0)
			{
				model.HiBitsFlag = model.GetHb2Flag()[model.FoundState.Symbol];
				coder.SubRange.LowCount = num2;
				model.CharMask[state.Symbol] = model.EscCount;
				model.NumMasked = numStats;
				num3 = numStats - 1;
				model.FoundState.Address = 0;
				do
				{
					model.CharMask[state.DecrementAddress().Symbol] = model.EscCount;
				}
				while (--num3 != 0);
				coder.SubRange.HighCount = coder.SubRange.Scale;
				return true;
			}
		}
		coder.SubRange.LowCount = num2 - state.Freq;
		coder.SubRange.HighCount = num2;
		Update1(model, state.Address);
		return true;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("PPMContext[");
		stringBuilder.Append("\n  Address=");
		stringBuilder.Append(Address);
		stringBuilder.Append("\n  size=");
		stringBuilder.Append(SIZE);
		stringBuilder.Append("\n  numStats=");
		stringBuilder.Append(NumStats);
		stringBuilder.Append("\n  Suffix=");
		stringBuilder.Append(GetSuffix());
		stringBuilder.Append("\n  freqData=");
		stringBuilder.Append(_freqData);
		stringBuilder.Append("\n  oneState=");
		stringBuilder.Append(_oneState);
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}

	static PpmContext()
	{
		SIZE = 2 + UNION_SIZE + 4;
		EXP_ESCAPE = new int[16]
		{
			25, 14, 9, 7, 5, 5, 4, 4, 4, 3,
			3, 3, 2, 2, 2, 2
		};
		UNION_SIZE = Math.Max(6, 6);
	}
}
