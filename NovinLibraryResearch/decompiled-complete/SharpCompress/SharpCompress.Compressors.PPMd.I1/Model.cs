using System;
using System.IO;

namespace SharpCompress.Compressors.PPMd.I1;

internal class Model
{
	internal struct PpmContext
	{
		public uint _address;

		public byte[] _memory;

		public static readonly PpmContext ZERO = new PpmContext(0u, null);

		public const int SIZE = 12;

		public byte NumberStatistics
		{
			get
			{
				return _memory[_address];
			}
			set
			{
				_memory[_address] = value;
			}
		}

		public byte Flags
		{
			get
			{
				return _memory[_address + 1];
			}
			set
			{
				_memory[_address + 1] = value;
			}
		}

		public ushort SummaryFrequency
		{
			get
			{
				return (ushort)(_memory[_address + 2] | (_memory[_address + 3] << 8));
			}
			set
			{
				_memory[_address + 2] = (byte)value;
				_memory[_address + 3] = (byte)(value >> 8);
			}
		}

		public PpmState Statistics
		{
			get
			{
				return new PpmState((uint)(_memory[_address + 4] | (_memory[_address + 5] << 8) | (_memory[_address + 6] << 16) | (_memory[_address + 7] << 24)), _memory);
			}
			set
			{
				_memory[_address + 4] = (byte)value._address;
				_memory[_address + 5] = (byte)(value._address >> 8);
				_memory[_address + 6] = (byte)(value._address >> 16);
				_memory[_address + 7] = (byte)(value._address >> 24);
			}
		}

		public PpmContext Suffix
		{
			get
			{
				return new PpmContext((uint)(_memory[_address + 8] | (_memory[_address + 9] << 8) | (_memory[_address + 10] << 16) | (_memory[_address + 11] << 24)), _memory);
			}
			set
			{
				_memory[_address + 8] = (byte)value._address;
				_memory[_address + 9] = (byte)(value._address >> 8);
				_memory[_address + 10] = (byte)(value._address >> 16);
				_memory[_address + 11] = (byte)(value._address >> 24);
			}
		}

		public PpmState FirstState => new PpmState(_address + 2, _memory);

		public byte FirstStateSymbol
		{
			get
			{
				return _memory[_address + 2];
			}
			set
			{
				_memory[_address + 2] = value;
			}
		}

		public byte FirstStateFrequency
		{
			get
			{
				return _memory[_address + 3];
			}
			set
			{
				_memory[_address + 3] = value;
			}
		}

		public PpmContext FirstStateSuccessor
		{
			get
			{
				return new PpmContext((uint)(_memory[_address + 4] | (_memory[_address + 5] << 8) | (_memory[_address + 6] << 16) | (_memory[_address + 7] << 24)), _memory);
			}
			set
			{
				_memory[_address + 4] = (byte)value._address;
				_memory[_address + 5] = (byte)(value._address >> 8);
				_memory[_address + 6] = (byte)(value._address >> 16);
				_memory[_address + 7] = (byte)(value._address >> 24);
			}
		}

		public PpmContext(uint address, byte[] memory)
		{
			_address = address;
			_memory = memory;
		}

		public static implicit operator PpmContext(Pointer pointer)
		{
			return new PpmContext(pointer._address, pointer._memory);
		}

		public static PpmContext operator +(PpmContext context, int offset)
		{
			context._address = (uint)(context._address + offset * 12);
			return context;
		}

		public static PpmContext operator -(PpmContext context, int offset)
		{
			context._address = (uint)(context._address - offset * 12);
			return context;
		}

		public static bool operator <=(PpmContext context1, PpmContext context2)
		{
			return context1._address <= context2._address;
		}

		public static bool operator >=(PpmContext context1, PpmContext context2)
		{
			return context1._address >= context2._address;
		}

		public static bool operator ==(PpmContext context1, PpmContext context2)
		{
			return context1._address == context2._address;
		}

		public static bool operator !=(PpmContext context1, PpmContext context2)
		{
			return context1._address != context2._address;
		}

		public override bool Equals(object obj)
		{
			if (obj is PpmContext)
			{
				return ((PpmContext)obj)._address == _address;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return _address.GetHashCode();
		}
	}

	public const uint SIGNATURE = 2225909647u;

	public const char VARIANT = 'I';

	public const int MAXIMUM_ORDER = 16;

	private const byte UPPER_FREQUENCY = 5;

	private const byte INTERVAL_BIT_COUNT = 7;

	private const byte PERIOD_BIT_COUNT = 7;

	private const byte TOTAL_BIT_COUNT = 14;

	private const uint INTERVAL = 128u;

	private const uint BINARY_SCALE = 16384u;

	private const uint MAXIMUM_FREQUENCY = 124u;

	private const uint ORDER_BOUND = 9u;

	private readonly See2Context[,] _see2Contexts;

	private readonly See2Context _emptySee2Context;

	private PpmContext _maximumContext;

	private readonly ushort[,] _binarySummary = new ushort[25, 64];

	private readonly byte[] _numberStatisticsToBinarySummaryIndex = new byte[256];

	private readonly byte[] _probabilities = new byte[260];

	private readonly byte[] _characterMask = new byte[256];

	private byte _escapeCount;

	private int _modelOrder;

	private int _orderFall;

	private int _initialEscape;

	private int _initialRunLength;

	private int _runLength;

	private byte _previousSuccess;

	private byte _numberMasked;

	private ModelRestorationMethod _method;

	private PpmState _foundState;

	private Allocator _allocator;

	private Coder _coder;

	private PpmContext _minimumContext;

	private byte _numberStatistics;

	private readonly PpmState[] _decodeStates = new PpmState[256];

	private static readonly ushort[] INITIAL_BINARY_ESCAPES = new ushort[8] { 15581, 7999, 22975, 18675, 25761, 23228, 26162, 24657 };

	private static ReadOnlySpan<byte> EXPONENTIAL_ESCAPES => new byte[16]
	{
		25, 14, 9, 7, 5, 5, 4, 4, 4, 3,
		3, 3, 2, 2, 2, 2
	};

	public Model()
	{
		_numberStatisticsToBinarySummaryIndex[0] = 0;
		_numberStatisticsToBinarySummaryIndex[1] = 2;
		for (int i = 2; i < 11; i++)
		{
			_numberStatisticsToBinarySummaryIndex[i] = 4;
		}
		for (int j = 11; j < 256; j++)
		{
			_numberStatisticsToBinarySummaryIndex[j] = 6;
		}
		uint num = 1u;
		uint num2 = 1u;
		uint num3 = 5u;
		for (int k = 0; k < 5; k++)
		{
			_probabilities[k] = (byte)k;
		}
		for (int l = 5; l < 260; l++)
		{
			_probabilities[l] = (byte)num3;
			num--;
			if (num == 0)
			{
				num2++;
				num = num2;
				num3++;
			}
		}
		_see2Contexts = new See2Context[24, 32];
		for (int m = 0; m < 24; m++)
		{
			for (int n = 0; n < 32; n++)
			{
				_see2Contexts[m, n] = new See2Context();
			}
		}
		_emptySee2Context = new See2Context();
		_emptySee2Context._summary = 44943;
		_emptySee2Context._shift = 172;
		_emptySee2Context._count = 132;
	}

	public void Encode(Stream target, Stream source, PpmdProperties properties)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		EncodeStart(properties);
		EncodeBlock(target, source, final: true);
	}

	internal Coder EncodeStart(PpmdProperties properties)
	{
		_allocator = properties._allocator;
		_coder = new Coder();
		_coder.RangeEncoderInitialize();
		StartModel(properties.ModelOrder, properties.RestorationMethod);
		return _coder;
	}

	internal void EncodeBlock(Stream target, Stream source, bool final)
	{
		while (true)
		{
			_minimumContext = _maximumContext;
			_numberStatistics = _minimumContext.NumberStatistics;
			int num = source.ReadByte();
			if (num < 0 && !final)
			{
				break;
			}
			if (_numberStatistics != 0)
			{
				EncodeSymbol1(num, _minimumContext);
				_coder.RangeEncodeSymbol();
			}
			else
			{
				EncodeBinarySymbol(num, _minimumContext);
				_coder.RangeShiftEncodeSymbol(14);
			}
			while (_foundState == PpmState.ZERO)
			{
				_coder.RangeEncoderNormalize(target);
				do
				{
					_orderFall++;
					_minimumContext = _minimumContext.Suffix;
					if (_minimumContext == PpmContext.ZERO)
					{
						_coder.RangeEncoderFlush(target);
						return;
					}
				}
				while (_minimumContext.NumberStatistics == _numberMasked);
				EncodeSymbol2(num, _minimumContext);
				_coder.RangeEncodeSymbol();
			}
			if (_orderFall == 0 && (Pointer)_foundState.Successor >= _allocator._baseUnit)
			{
				_maximumContext = _foundState.Successor;
			}
			else
			{
				UpdateModel(_minimumContext);
				if (_escapeCount == 0)
				{
					ClearMask();
				}
			}
			_coder.RangeEncoderNormalize(target);
		}
	}

	public void Decode(Stream target, Stream source, PpmdProperties properties)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		DecodeStart(source, properties);
		byte[] array = new byte[65536];
		int count;
		while ((count = DecodeBlock(source, array, 0, array.Length)) != 0)
		{
			target.Write(array, 0, count);
		}
	}

	internal Coder DecodeStart(Stream source, PpmdProperties properties)
	{
		_allocator = properties._allocator;
		_coder = new Coder();
		_coder.RangeDecoderInitialize(source);
		StartModel(properties.ModelOrder, properties.RestorationMethod);
		_minimumContext = _maximumContext;
		_numberStatistics = _minimumContext.NumberStatistics;
		return _coder;
	}

	internal int DecodeBlock(Stream source, byte[] buffer, int offset, int count)
	{
		if (_minimumContext == PpmContext.ZERO)
		{
			return 0;
		}
		int num = 0;
		while (num < count)
		{
			if (_numberStatistics != 0)
			{
				DecodeSymbol1(_minimumContext);
			}
			else
			{
				DecodeBinarySymbol(_minimumContext);
			}
			_coder.RangeRemoveSubrange();
			for (; _foundState == PpmState.ZERO; DecodeSymbol2(_minimumContext), _coder.RangeRemoveSubrange())
			{
				_coder.RangeDecoderNormalize(source);
				while (true)
				{
					_orderFall++;
					_minimumContext = _minimumContext.Suffix;
					if (_minimumContext == PpmContext.ZERO)
					{
						break;
					}
					if (_minimumContext.NumberStatistics == _numberMasked)
					{
						continue;
					}
					goto IL_009d;
				}
				goto end_IL_015d;
				IL_009d:;
			}
			buffer[offset] = _foundState.Symbol;
			offset++;
			num++;
			if (_orderFall == 0 && (Pointer)_foundState.Successor >= _allocator._baseUnit)
			{
				_maximumContext = _foundState.Successor;
			}
			else
			{
				UpdateModel(_minimumContext);
				if (_escapeCount == 0)
				{
					ClearMask();
				}
			}
			_minimumContext = _maximumContext;
			_numberStatistics = _minimumContext.NumberStatistics;
			_coder.RangeDecoderNormalize(source);
			continue;
			end_IL_015d:
			break;
		}
		return num;
	}

	private void StartModel(int modelOrder, ModelRestorationMethod modelRestorationMethod)
	{
		Array.Clear(_characterMask, 0, _characterMask.Length);
		_escapeCount = 1;
		if (modelOrder < 2)
		{
			_orderFall = _modelOrder;
			PpmContext ppmContext = _maximumContext;
			while (ppmContext.Suffix != PpmContext.ZERO)
			{
				_orderFall--;
				ppmContext = ppmContext.Suffix;
			}
			return;
		}
		_modelOrder = modelOrder;
		_orderFall = modelOrder;
		_method = modelRestorationMethod;
		_allocator.Initialize();
		_initialRunLength = -((modelOrder < 12) ? modelOrder : 12) - 1;
		_runLength = _initialRunLength;
		_maximumContext = _allocator.AllocateContext();
		_maximumContext.Suffix = PpmContext.ZERO;
		_maximumContext.NumberStatistics = byte.MaxValue;
		_maximumContext.SummaryFrequency = (ushort)(_maximumContext.NumberStatistics + 2);
		_maximumContext.Statistics = _allocator.AllocateUnits(128u);
		_previousSuccess = 0;
		for (int i = 0; i < 256; i++)
		{
			PpmState ppmState = _maximumContext.Statistics[i];
			ppmState.Symbol = (byte)i;
			ppmState.Frequency = 1;
			ppmState.Successor = PpmContext.ZERO;
		}
		uint num = 0u;
		int j = 0;
		for (; num < 25; num++)
		{
			for (; _probabilities[j] == num; j++)
			{
			}
			for (int k = 0; k < 8; k++)
			{
				_binarySummary[num, k] = (ushort)(16384uL - (ulong)(INITIAL_BINARY_ESCAPES[k] / (j + 1)));
			}
			for (int l = 8; l < 64; l += 8)
			{
				for (int m = 0; m < 8; m++)
				{
					_binarySummary[num, l + m] = _binarySummary[num, m];
				}
			}
		}
		num = 0u;
		uint num2 = 0u;
		for (; num < 24; num++)
		{
			for (; _probabilities[num2 + 3] == num + 3; num2++)
			{
			}
			for (int n = 0; n < 32; n++)
			{
				_see2Contexts[num, n].Initialize(2 * num2 + 5);
			}
		}
	}

	private void UpdateModel(PpmContext minimumContext)
	{
		PpmState state = PpmState.ZERO;
		PpmContext ppmContext = _maximumContext;
		uint frequency = _foundState.Frequency;
		byte symbol = _foundState.Symbol;
		PpmContext ppmContext2 = _foundState.Successor;
		PpmContext suffix = minimumContext.Suffix;
		if (frequency < 31 && suffix != PpmContext.ZERO)
		{
			if (suffix.NumberStatistics != 0)
			{
				state = suffix.Statistics;
				if (state.Symbol != symbol)
				{
					byte symbol2;
					do
					{
						symbol2 = state[1].Symbol;
						++state;
					}
					while (symbol2 != symbol);
					if (state[0].Frequency >= state[-1].Frequency)
					{
						Swap(state[0], state[-1]);
						--state;
					}
				}
				uint num = (((uint)state.Frequency < 115u) ? 2u : 0u);
				state.Frequency += (byte)num;
				suffix.SummaryFrequency += (byte)num;
			}
			else
			{
				state = suffix.FirstState;
				state.Frequency += ((state.Frequency < 32) ? ((byte)1) : ((byte)0));
			}
		}
		if (_orderFall == 0 && ppmContext2 != PpmContext.ZERO)
		{
			_foundState.Successor = CreateSuccessors(skip: true, state, minimumContext);
			if (!(_foundState.Successor == PpmContext.ZERO))
			{
				_maximumContext = _foundState.Successor;
				return;
			}
		}
		else
		{
			_allocator._text[0] = symbol;
			++_allocator._text;
			PpmContext successor = _allocator._text;
			if (!(_allocator._text >= _allocator._baseUnit))
			{
				if (ppmContext2 != PpmContext.ZERO)
				{
					if (ppmContext2 < _allocator._baseUnit)
					{
						ppmContext2 = CreateSuccessors(skip: false, state, minimumContext);
					}
				}
				else
				{
					ppmContext2 = ReduceOrder(state, minimumContext);
				}
				if (!(ppmContext2 == PpmContext.ZERO))
				{
					if (--_orderFall == 0)
					{
						successor = ppmContext2;
						_allocator._text -= ((_maximumContext != minimumContext) ? 1 : 0);
					}
					else if (_method > ModelRestorationMethod.Freeze)
					{
						successor = ppmContext2;
						_allocator._text = _allocator._heap;
						_orderFall = 0;
					}
					uint numberStatistics = minimumContext.NumberStatistics;
					uint num2 = minimumContext.SummaryFrequency - numberStatistics - frequency;
					byte b = (byte)((symbol >= 64) ? 8u : 0u);
					while (true)
					{
						if (ppmContext != minimumContext)
						{
							uint numberStatistics2 = ppmContext.NumberStatistics;
							if (numberStatistics2 != 0)
							{
								if ((numberStatistics2 & (true ? 1u : 0u)) != 0)
								{
									state = _allocator.ExpandUnits(ppmContext.Statistics, numberStatistics2 + 1 >> 1);
									if (state == PpmState.ZERO)
									{
										break;
									}
									ppmContext.Statistics = state;
								}
								ppmContext.SummaryFrequency += (ushort)((3 * numberStatistics2 + 1 < numberStatistics) ? 1 : 0);
							}
							else
							{
								state = _allocator.AllocateUnits(1u);
								if (state == PpmState.ZERO)
								{
									break;
								}
								Copy(state, ppmContext.FirstState);
								ppmContext.Statistics = state;
								if ((uint)state.Frequency < 30u)
								{
									state.Frequency += state.Frequency;
								}
								else
								{
									state.Frequency = 120;
								}
								ppmContext.SummaryFrequency = (ushort)((uint)(state.Frequency + _initialEscape) + ((numberStatistics > 2) ? 1u : 0u));
							}
							uint num = (uint)(2 * frequency * (ppmContext.SummaryFrequency + 6));
							uint num3 = num2 + ppmContext.SummaryFrequency;
							if (num < 6 * num3)
							{
								num = (uint)(1 + ((num > num3) ? 1 : 0) + ((num >= 4 * num3) ? 1 : 0));
								ppmContext.SummaryFrequency += 4;
							}
							else
							{
								num = (uint)(4 + ((num > 9 * num3) ? 1 : 0) + ((num > 12 * num3) ? 1 : 0) + ((num > 15 * num3) ? 1 : 0));
								ppmContext.SummaryFrequency += (ushort)num;
							}
							state = ppmContext.Statistics + ++ppmContext.NumberStatistics;
							state.Successor = successor;
							state.Symbol = symbol;
							state.Frequency = (byte)num;
							ppmContext.Flags |= b;
							ppmContext = ppmContext.Suffix;
							continue;
						}
						_maximumContext = ppmContext2;
						return;
					}
				}
			}
		}
		RestoreModel(ppmContext, minimumContext, ppmContext2);
	}

	private PpmContext CreateSuccessors(bool skip, PpmState state, PpmContext context)
	{
		PpmContext successor = _foundState.Successor;
		PpmState[] array = new PpmState[16];
		uint num = 0u;
		byte symbol = _foundState.Symbol;
		if (!skip)
		{
			array[num++] = _foundState;
			if (context.Suffix == PpmContext.ZERO)
			{
				goto IL_016a;
			}
		}
		bool flag = false;
		if (state != PpmState.ZERO)
		{
			context = context.Suffix;
			flag = true;
		}
		do
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				context = context.Suffix;
				if (context.NumberStatistics != 0)
				{
					state = context.Statistics;
					byte symbol2;
					if (state.Symbol != symbol)
					{
						do
						{
							symbol2 = state[1].Symbol;
							++state;
						}
						while (symbol2 != symbol);
					}
					symbol2 = (((uint)state.Frequency < 115u) ? ((byte)1) : ((byte)0));
					state.Frequency += symbol2;
					context.SummaryFrequency += symbol2;
				}
				else
				{
					state = context.FirstState;
					state.Frequency += (((context.Suffix.NumberStatistics == 0) & (state.Frequency < 24)) ? ((byte)1) : ((byte)0));
				}
			}
			if (state.Successor != successor)
			{
				context = state.Successor;
				break;
			}
			array[num++] = state;
		}
		while (context.Suffix != PpmContext.ZERO);
		goto IL_016a;
		IL_016a:
		if (num == 0)
		{
			return context;
		}
		byte numberStatistics = 0;
		byte b = (byte)((symbol >= 64) ? 16u : 0u);
		symbol = successor.NumberStatistics;
		byte firstStateSymbol = symbol;
		PpmContext firstStateSuccessor = (Pointer)successor + 1;
		b |= (byte)((symbol >= 64) ? 8u : 0u);
		byte firstStateFrequency;
		if (context.NumberStatistics != 0)
		{
			state = context.Statistics;
			if (state.Symbol != symbol)
			{
				byte symbol3;
				do
				{
					symbol3 = state[1].Symbol;
					++state;
				}
				while (symbol3 != symbol);
			}
			uint num2 = (uint)(state.Frequency - 1);
			uint num3 = (uint)(context.SummaryFrequency - context.NumberStatistics - num2);
			firstStateFrequency = (byte)(1 + ((2 * num2 > num3) ? ((int)((num2 + 2 * num3 - 3) / num3)) : ((5 * num2 > num3) ? 1 : 0)));
		}
		else
		{
			firstStateFrequency = context.FirstStateFrequency;
		}
		do
		{
			PpmContext ppmContext = _allocator.AllocateContext();
			if (ppmContext == PpmContext.ZERO)
			{
				return PpmContext.ZERO;
			}
			ppmContext.NumberStatistics = numberStatistics;
			ppmContext.Flags = b;
			ppmContext.FirstStateSymbol = firstStateSymbol;
			ppmContext.FirstStateFrequency = firstStateFrequency;
			ppmContext.FirstStateSuccessor = firstStateSuccessor;
			ppmContext.Suffix = context;
			context = ppmContext;
			array[--num].Successor = context;
		}
		while (num != 0);
		return context;
	}

	private PpmContext ReduceOrder(PpmState state, PpmContext context)
	{
		PpmState[] array = new PpmState[16];
		uint num = 0u;
		PpmContext ppmContext = context;
		PpmContext ppmContext2 = _allocator._text;
		byte symbol = _foundState.Symbol;
		array[num++] = _foundState;
		_foundState.Successor = ppmContext2;
		_orderFall++;
		bool flag = false;
		if (state != PpmState.ZERO)
		{
			context = context.Suffix;
			flag = true;
		}
		while (true)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				if (context.Suffix == PpmContext.ZERO)
				{
					if (_method > ModelRestorationMethod.Freeze)
					{
						do
						{
							array[--num].Successor = context;
						}
						while (num != 0);
						_allocator._text = _allocator._heap + 1;
						_orderFall = 1;
					}
					return context;
				}
				context = context.Suffix;
				if (context.NumberStatistics != 0)
				{
					state = context.Statistics;
					byte symbol2;
					if (state.Symbol != symbol)
					{
						do
						{
							symbol2 = state[1].Symbol;
							++state;
						}
						while (symbol2 != symbol);
					}
					symbol2 = (byte)(((uint)state.Frequency < 115u) ? 2u : 0u);
					state.Frequency += symbol2;
					context.SummaryFrequency += symbol2;
				}
				else
				{
					state = context.FirstState;
					state.Frequency += ((state.Frequency < 32) ? ((byte)1) : ((byte)0));
				}
			}
			if (state.Successor != PpmContext.ZERO)
			{
				break;
			}
			array[num++] = state;
			state.Successor = ppmContext2;
			_orderFall++;
		}
		if (_method > ModelRestorationMethod.Freeze)
		{
			context = state.Successor;
			do
			{
				array[--num].Successor = context;
			}
			while (num != 0);
			_allocator._text = _allocator._heap + 1;
			_orderFall = 1;
			return context;
		}
		if (state.Successor <= ppmContext2)
		{
			PpmState foundState = _foundState;
			_foundState = state;
			state.Successor = CreateSuccessors(skip: false, PpmState.ZERO, context);
			_foundState = foundState;
		}
		if (_orderFall == 1 && ppmContext == _maximumContext)
		{
			_foundState.Successor = state.Successor;
			--_allocator._text;
		}
		return state.Successor;
	}

	private void RestoreModel(PpmContext context, PpmContext minimumContext, PpmContext foundStateSuccessor)
	{
		_allocator._text = _allocator._heap;
		PpmContext ppmContext = _maximumContext;
		while (ppmContext != context)
		{
			if (--ppmContext.NumberStatistics == 0)
			{
				ppmContext.Flags = (byte)((ppmContext.Flags & 0x10) + ((ppmContext.Statistics.Symbol >= 64) ? 8 : 0));
				PpmState statistics = ppmContext.Statistics;
				Copy(ppmContext.FirstState, statistics);
				_allocator.SpecialFreeUnits(statistics);
				ppmContext.FirstStateFrequency = (byte)(ppmContext.FirstStateFrequency + 11 >> 3);
			}
			else
			{
				Refresh((uint)(ppmContext.NumberStatistics + 3 >> 1), scale: false, ppmContext);
			}
			ppmContext = ppmContext.Suffix;
		}
		while (ppmContext != minimumContext)
		{
			if (ppmContext.NumberStatistics == 0)
			{
				ppmContext.FirstStateFrequency -= (byte)(ppmContext.FirstStateFrequency >> 1);
			}
			else if ((ppmContext.SummaryFrequency += 4) > 128 + 4 * ppmContext.NumberStatistics)
			{
				Refresh((uint)(ppmContext.NumberStatistics + 2 >> 1), scale: true, ppmContext);
			}
			ppmContext = ppmContext.Suffix;
		}
		if (_method > ModelRestorationMethod.Freeze)
		{
			_maximumContext = foundStateSuccessor;
			_allocator._glueCount += (((_allocator._memoryNodes[1].Stamp & 1) == 0) ? 1u : 0u);
			return;
		}
		if (_method == ModelRestorationMethod.Freeze)
		{
			while (_maximumContext.Suffix != PpmContext.ZERO)
			{
				_maximumContext = _maximumContext.Suffix;
			}
			RemoveBinaryContexts(0, _maximumContext);
			_method++;
			_allocator._glueCount = 0u;
			_orderFall = _modelOrder;
			return;
		}
		if (_method == ModelRestorationMethod.Restart || _allocator.GetMemoryUsed() < _allocator._allocatorSize >> 1)
		{
			StartModel(_modelOrder, _method);
			_escapeCount = 0;
			return;
		}
		while (_maximumContext.Suffix != PpmContext.ZERO)
		{
			_maximumContext = _maximumContext.Suffix;
		}
		do
		{
			CutOff(0, _maximumContext);
			_allocator.ExpandText();
		}
		while (_allocator.GetMemoryUsed() > 3 * (_allocator._allocatorSize >> 2));
		_allocator._glueCount = 0u;
		_orderFall = _modelOrder;
	}

	private static void Swap(PpmState state1, PpmState state2)
	{
		byte symbol = state1.Symbol;
		byte frequency = state1.Frequency;
		PpmContext successor = state1.Successor;
		state1.Symbol = state2.Symbol;
		state1.Frequency = state2.Frequency;
		state1.Successor = state2.Successor;
		state2.Symbol = symbol;
		state2.Frequency = frequency;
		state2.Successor = successor;
	}

	private static void Copy(PpmState state1, PpmState state2)
	{
		state1.Symbol = state2.Symbol;
		state1.Frequency = state2.Frequency;
		state1.Successor = state2.Successor;
	}

	private static int Mean(int sum, int shift, int round)
	{
		return sum + (1 << shift - round) >> shift;
	}

	private void ClearMask()
	{
		_escapeCount = 1;
		Array.Clear(_characterMask, 0, _characterMask.Length);
	}

	private void EncodeBinarySymbol(int symbol, PpmContext context)
	{
		PpmState firstState = context.FirstState;
		int num = _probabilities[firstState.Frequency - 1];
		int num2 = _numberStatisticsToBinarySummaryIndex[context.Suffix.NumberStatistics] + _previousSuccess + context.Flags + ((_runLength >> 26) & 0x20);
		if (firstState.Symbol == symbol)
		{
			_foundState = firstState;
			firstState.Frequency += ((firstState.Frequency < 196) ? ((byte)1) : ((byte)0));
			_coder._lowCount = 0u;
			_coder._highCount = _binarySummary[num, num2];
			_binarySummary[num, num2] += (ushort)(128L - (long)Mean(_binarySummary[num, num2], 7, 2));
			_previousSuccess = 1;
			_runLength++;
		}
		else
		{
			_coder._lowCount = _binarySummary[num, num2];
			_binarySummary[num, num2] -= (ushort)Mean(_binarySummary[num, num2], 7, 2);
			_coder._highCount = 16384u;
			_initialEscape = EXPONENTIAL_ESCAPES[_binarySummary[num, num2] >> 10];
			_characterMask[firstState.Symbol] = _escapeCount;
			_previousSuccess = 0;
			_numberMasked = 0;
			_foundState = PpmState.ZERO;
		}
	}

	private void EncodeSymbol1(int symbol, PpmContext context)
	{
		uint symbol2 = context.Statistics.Symbol;
		PpmState statistics = context.Statistics;
		_coder._scale = context.SummaryFrequency;
		if (symbol2 == symbol)
		{
			_coder._highCount = statistics.Frequency;
			_previousSuccess = (byte)((2 * _coder._highCount >= _coder._scale) ? 1u : 0u);
			_foundState = statistics;
			_foundState.Frequency += 4;
			context.SummaryFrequency += 4;
			_runLength += _previousSuccess;
			if ((uint)statistics.Frequency > 124u)
			{
				Rescale(context);
			}
			_coder._lowCount = 0u;
			return;
		}
		uint num = statistics.Frequency;
		symbol2 = context.NumberStatistics;
		_previousSuccess = 0;
		while (true)
		{
			PpmState ppmState = ++statistics;
			if (ppmState.Symbol == symbol)
			{
				break;
			}
			num += statistics.Frequency;
			if (--symbol2 == 0)
			{
				_coder._lowCount = num;
				_characterMask[statistics.Symbol] = _escapeCount;
				_numberMasked = context.NumberStatistics;
				symbol2 = context.NumberStatistics;
				_foundState = PpmState.ZERO;
				do
				{
					byte[] characterMask = _characterMask;
					ppmState = --statistics;
					characterMask[ppmState.Symbol] = _escapeCount;
				}
				while (--symbol2 != 0);
				_coder._highCount = _coder._scale;
				return;
			}
		}
		_coder._highCount = (_coder._lowCount = num) + statistics.Frequency;
		Update1(statistics, context);
	}

	private void EncodeSymbol2(int symbol, PpmContext context)
	{
		See2Context see2Context = MakeEscapeFrequency(context);
		uint num = 0u;
		uint num2 = (uint)(context.NumberStatistics - _numberMasked);
		PpmState ppmState = context.Statistics - 1;
		while (true)
		{
			uint symbol2 = ppmState[1].Symbol;
			++ppmState;
			if (_characterMask[symbol2] != _escapeCount)
			{
				_characterMask[symbol2] = _escapeCount;
				if (symbol2 == symbol)
				{
					break;
				}
				num += ppmState.Frequency;
				if (--num2 == 0)
				{
					_coder._lowCount = num;
					_coder._scale += _coder._lowCount;
					_coder._highCount = _coder._scale;
					see2Context._summary += (ushort)_coder._scale;
					_numberMasked = context.NumberStatistics;
					return;
				}
			}
		}
		_coder._lowCount = num;
		num += ppmState.Frequency;
		_coder._highCount = num;
		PpmState ppmState2 = ppmState;
		while (--num2 != 0)
		{
			uint symbol2;
			do
			{
				symbol2 = ppmState2[1].Symbol;
				++ppmState2;
			}
			while (_characterMask[symbol2] == _escapeCount);
			num += ppmState2.Frequency;
		}
		_coder._scale += num;
		see2Context.Update();
		Update2(ppmState, context);
	}

	private void DecodeBinarySymbol(PpmContext context)
	{
		PpmState firstState = context.FirstState;
		int num = _probabilities[firstState.Frequency - 1];
		int num2 = _numberStatisticsToBinarySummaryIndex[context.Suffix.NumberStatistics] + _previousSuccess + context.Flags + ((_runLength >> 26) & 0x20);
		if (_coder.RangeGetCurrentShiftCount(14) < _binarySummary[num, num2])
		{
			_foundState = firstState;
			firstState.Frequency += ((firstState.Frequency < 196) ? ((byte)1) : ((byte)0));
			_coder._lowCount = 0u;
			_coder._highCount = _binarySummary[num, num2];
			_binarySummary[num, num2] += (ushort)(128L - (long)Mean(_binarySummary[num, num2], 7, 2));
			_previousSuccess = 1;
			_runLength++;
		}
		else
		{
			_coder._lowCount = _binarySummary[num, num2];
			_binarySummary[num, num2] -= (ushort)Mean(_binarySummary[num, num2], 7, 2);
			_coder._highCount = 16384u;
			_initialEscape = EXPONENTIAL_ESCAPES[_binarySummary[num, num2] >> 10];
			_characterMask[firstState.Symbol] = _escapeCount;
			_previousSuccess = 0;
			_numberMasked = 0;
			_foundState = PpmState.ZERO;
		}
	}

	private void DecodeSymbol1(PpmContext context)
	{
		uint num = context.Statistics.Frequency;
		PpmState statistics = context.Statistics;
		_coder._scale = context.SummaryFrequency;
		uint num2 = _coder.RangeGetCurrentCount();
		if (num2 < num)
		{
			_coder._highCount = num;
			_previousSuccess = (byte)((2 * _coder._highCount >= _coder._scale) ? 1u : 0u);
			_foundState = statistics;
			num += 4;
			_foundState.Frequency = (byte)num;
			context.SummaryFrequency += 4;
			_runLength += _previousSuccess;
			if (num > 124)
			{
				Rescale(context);
			}
			_coder._lowCount = 0u;
			return;
		}
		uint num3 = context.NumberStatistics;
		_previousSuccess = 0;
		while (true)
		{
			uint num4 = num;
			PpmState ppmState = ++statistics;
			if ((num = num4 + ppmState.Frequency) > num2)
			{
				break;
			}
			if (--num3 == 0)
			{
				_coder._lowCount = num;
				_characterMask[statistics.Symbol] = _escapeCount;
				_numberMasked = context.NumberStatistics;
				num3 = context.NumberStatistics;
				_foundState = PpmState.ZERO;
				do
				{
					byte[] characterMask = _characterMask;
					ppmState = --statistics;
					characterMask[ppmState.Symbol] = _escapeCount;
				}
				while (--num3 != 0);
				_coder._highCount = _coder._scale;
				return;
			}
		}
		_coder._highCount = num;
		_coder._lowCount = _coder._highCount - statistics.Frequency;
		Update1(statistics, context);
	}

	private void DecodeSymbol2(PpmContext context)
	{
		See2Context see2Context = MakeEscapeFrequency(context);
		uint num = 0u;
		uint num2 = (uint)(context.NumberStatistics - _numberMasked);
		uint num3 = 0u;
		PpmState ppmState = context.Statistics - 1;
		while (true)
		{
			uint symbol = ppmState[1].Symbol;
			++ppmState;
			if (_characterMask[symbol] != _escapeCount)
			{
				num += ppmState.Frequency;
				_decodeStates[num3++] = ppmState;
				if (--num2 == 0)
				{
					break;
				}
			}
		}
		_coder._scale += num;
		uint num4 = _coder.RangeGetCurrentCount();
		num3 = 0u;
		ppmState = _decodeStates[num3];
		if (num4 < num)
		{
			num = 0u;
			while ((num += ppmState.Frequency) <= num4)
			{
				ppmState = _decodeStates[++num3];
			}
			_coder._highCount = num;
			_coder._lowCount = _coder._highCount - ppmState.Frequency;
			see2Context.Update();
			Update2(ppmState, context);
			return;
		}
		_coder._lowCount = num;
		_coder._highCount = _coder._scale;
		num2 = (uint)(context.NumberStatistics - _numberMasked);
		_numberMasked = context.NumberStatistics;
		do
		{
			_characterMask[_decodeStates[num3].Symbol] = _escapeCount;
			num3++;
		}
		while (--num2 != 0);
		see2Context._summary += (ushort)_coder._scale;
	}

	private void Update1(PpmState state, PpmContext context)
	{
		_foundState = state;
		_foundState.Frequency += 4;
		context.SummaryFrequency += 4;
		if (state[0].Frequency > state[-1].Frequency)
		{
			Swap(state[0], state[-1]);
			_foundState = --state;
			if ((uint)state.Frequency > 124u)
			{
				Rescale(context);
			}
		}
	}

	private void Update2(PpmState state, PpmContext context)
	{
		_foundState = state;
		_foundState.Frequency += 4;
		context.SummaryFrequency += 4;
		if ((uint)state.Frequency > 124u)
		{
			Rescale(context);
		}
		_escapeCount++;
		_runLength = _initialRunLength;
	}

	private See2Context MakeEscapeFrequency(PpmContext context)
	{
		uint num = (uint)(2 * context.NumberStatistics);
		See2Context see2Context;
		if (context.NumberStatistics != byte.MaxValue)
		{
			num = context.Suffix.NumberStatistics;
			int num2 = _probabilities[context.NumberStatistics + 2] - 3;
			int num3 = ((context.SummaryFrequency > 11 * (context.NumberStatistics + 1)) ? 1 : 0) + ((2 * context.NumberStatistics < num + _numberMasked) ? 2 : 0) + context.Flags;
			see2Context = _see2Contexts[num2, num3];
			_coder._scale = see2Context.Mean();
		}
		else
		{
			see2Context = _emptySee2Context;
			_coder._scale = 1u;
		}
		return see2Context;
	}

	private void Rescale(PpmContext context)
	{
		uint num = context.NumberStatistics;
		PpmState foundState;
		for (foundState = _foundState; foundState != context.Statistics; --foundState)
		{
			Swap(foundState[0], foundState[-1]);
		}
		foundState.Frequency += 4;
		context.SummaryFrequency += 4;
		uint num2 = (uint)(context.SummaryFrequency - foundState.Frequency);
		int num3 = ((_orderFall != 0 || _method > ModelRestorationMethod.Freeze) ? 1 : 0);
		foundState.Frequency = (byte)(foundState.Frequency + num3 >> 1);
		context.SummaryFrequency = foundState.Frequency;
		do
		{
			uint num4 = num2;
			PpmState ppmState = ++foundState;
			num2 = num4 - ppmState.Frequency;
			foundState.Frequency = (byte)(foundState.Frequency + num3 >> 1);
			context.SummaryFrequency += foundState.Frequency;
			if (foundState[0].Frequency > foundState[-1].Frequency)
			{
				PpmState ppmState2 = foundState;
				byte symbol = ppmState2.Symbol;
				byte frequency = ppmState2.Frequency;
				PpmContext successor = ppmState2.Successor;
				byte num5;
				do
				{
					Copy(ppmState2[0], ppmState2[-1]);
					num5 = frequency;
					ppmState = --ppmState2;
				}
				while (num5 > ppmState[-1].Frequency);
				ppmState2.Symbol = symbol;
				ppmState2.Frequency = frequency;
				ppmState2.Successor = successor;
			}
		}
		while (--num != 0);
		if (foundState.Frequency == 0)
		{
			PpmState ppmState;
			do
			{
				num++;
				ppmState = --foundState;
			}
			while (ppmState.Frequency == 0);
			num2 += num;
			uint num6 = (uint)(context.NumberStatistics + 2 >> 1);
			context.NumberStatistics -= (byte)num;
			if (context.NumberStatistics == 0)
			{
				byte symbol = context.Statistics.Symbol;
				byte frequency = context.Statistics.Frequency;
				PpmContext successor = context.Statistics.Successor;
				frequency = (byte)((2 * frequency + num2 - 1) / num2);
				if ((uint)frequency > 41u)
				{
					frequency = 41;
				}
				_allocator.FreeUnits(context.Statistics, num6);
				context.FirstStateSymbol = symbol;
				context.FirstStateFrequency = frequency;
				context.FirstStateSuccessor = successor;
				context.Flags = (byte)((context.Flags & 0x10) + ((symbol >= 64) ? 8 : 0));
				_foundState = context.FirstState;
				return;
			}
			context.Statistics = _allocator.ShrinkUnits(context.Statistics, num6, (uint)(context.NumberStatistics + 2 >> 1));
			context.Flags &= 247;
			num = context.NumberStatistics;
			foundState = context.Statistics;
			context.Flags |= (byte)((foundState.Symbol >= 64) ? 8 : 0);
			do
			{
				byte flags = context.Flags;
				ppmState = ++foundState;
				context.Flags = (byte)(flags | (byte)((ppmState.Symbol >= 64) ? 8u : 0u));
			}
			while (--num != 0);
		}
		num2 -= num2 >> 1;
		context.SummaryFrequency += (ushort)num2;
		context.Flags |= 4;
		_foundState = context.Statistics;
	}

	private void Refresh(uint oldUnitCount, bool scale, PpmContext context)
	{
		int num = context.NumberStatistics;
		int num2 = (scale ? 1 : 0);
		context.Statistics = _allocator.ShrinkUnits(context.Statistics, oldUnitCount, (uint)(num + 2 >> 1));
		PpmState statistics = context.Statistics;
		context.Flags = (byte)((context.Flags & (16 + (scale ? 4 : 0))) + ((statistics.Symbol >= 64) ? 8 : 0));
		int num3 = context.SummaryFrequency - statistics.Frequency;
		statistics.Frequency = (byte)(statistics.Frequency + num2 >> num2);
		context.SummaryFrequency = statistics.Frequency;
		do
		{
			int num4 = num3;
			PpmState ppmState = ++statistics;
			num3 = num4 - ppmState.Frequency;
			statistics.Frequency = (byte)(statistics.Frequency + num2 >> num2);
			context.SummaryFrequency += statistics.Frequency;
			context.Flags |= (byte)((statistics.Symbol >= 64) ? 8 : 0);
		}
		while (--num != 0);
		num3 = num3 + num2 >> num2;
		context.SummaryFrequency += (ushort)num3;
	}

	private PpmContext CutOff(int order, PpmContext context)
	{
		if (context.NumberStatistics == 0)
		{
			PpmState firstState = context.FirstState;
			if ((Pointer)firstState.Successor >= _allocator._baseUnit)
			{
				if (order < _modelOrder)
				{
					firstState.Successor = CutOff(order + 1, firstState.Successor);
				}
				else
				{
					firstState.Successor = PpmContext.ZERO;
				}
				if (firstState.Successor == PpmContext.ZERO && (long)order > 9L)
				{
					_allocator.SpecialFreeUnits(context);
					return PpmContext.ZERO;
				}
				return context;
			}
			_allocator.SpecialFreeUnits(context);
			return PpmContext.ZERO;
		}
		uint num = (uint)(context.NumberStatistics + 2 >> 1);
		context.Statistics = _allocator.MoveUnitsUp(context.Statistics, num);
		int numberStatistics = context.NumberStatistics;
		for (PpmState firstState = context.Statistics + numberStatistics; firstState >= context.Statistics; --firstState)
		{
			if (firstState.Successor < _allocator._baseUnit)
			{
				firstState.Successor = PpmContext.ZERO;
				Swap(firstState, context.Statistics[numberStatistics--]);
			}
			else if (order < _modelOrder)
			{
				firstState.Successor = CutOff(order + 1, firstState.Successor);
			}
			else
			{
				firstState.Successor = PpmContext.ZERO;
			}
		}
		if (numberStatistics != context.NumberStatistics && order != 0)
		{
			context.NumberStatistics = (byte)numberStatistics;
			PpmState firstState = context.Statistics;
			if (numberStatistics < 0)
			{
				_allocator.FreeUnits(firstState, num);
				_allocator.SpecialFreeUnits(context);
				return PpmContext.ZERO;
			}
			if (numberStatistics == 0)
			{
				context.Flags = (byte)((context.Flags & 0x10) + ((firstState.Symbol >= 64) ? 8 : 0));
				Copy(context.FirstState, firstState);
				_allocator.FreeUnits(firstState, num);
				context.FirstStateFrequency = (byte)(context.FirstStateFrequency + 11 >> 3);
			}
			else
			{
				Refresh(num, context.SummaryFrequency > 16 * numberStatistics, context);
			}
		}
		return context;
	}

	private PpmContext RemoveBinaryContexts(int order, PpmContext context)
	{
		if (context.NumberStatistics == 0)
		{
			PpmState firstState = context.FirstState;
			if ((Pointer)firstState.Successor >= _allocator._baseUnit && order < _modelOrder)
			{
				firstState.Successor = RemoveBinaryContexts(order + 1, firstState.Successor);
			}
			else
			{
				firstState.Successor = PpmContext.ZERO;
			}
			if (firstState.Successor == PpmContext.ZERO && (context.Suffix.NumberStatistics == 0 || context.Suffix.Flags == byte.MaxValue))
			{
				_allocator.FreeUnits(context, 1u);
				return PpmContext.ZERO;
			}
			return context;
		}
		for (PpmState ppmState = context.Statistics + context.NumberStatistics; ppmState >= context.Statistics; --ppmState)
		{
			if ((Pointer)ppmState.Successor >= _allocator._baseUnit && order < _modelOrder)
			{
				ppmState.Successor = RemoveBinaryContexts(order + 1, ppmState.Successor);
			}
			else
			{
				ppmState.Successor = PpmContext.ZERO;
			}
		}
		return context;
	}
}
