using System;
using System.IO;
using SharpCompress.Compressors.LZMA.LZ;
using SharpCompress.Compressors.LZMA.RangeCoder;

namespace SharpCompress.Compressors.LZMA;

internal class Decoder : ICoder, ISetDecoderProperties
{
	private class LenDecoder
	{
		private BitDecoder _choice;

		private BitDecoder _choice2;

		private readonly BitTreeDecoder[] _lowCoder = new BitTreeDecoder[16];

		private readonly BitTreeDecoder[] _midCoder = new BitTreeDecoder[16];

		private BitTreeDecoder _highCoder = new BitTreeDecoder(8);

		private uint _numPosStates;

		public void Create(uint numPosStates)
		{
			for (uint num = _numPosStates; num < numPosStates; num++)
			{
				_lowCoder[num] = new BitTreeDecoder(3);
				_midCoder[num] = new BitTreeDecoder(3);
			}
			_numPosStates = numPosStates;
		}

		public void Init()
		{
			_choice.Init();
			for (uint num = 0u; num < _numPosStates; num++)
			{
				_lowCoder[num].Init();
				_midCoder[num].Init();
			}
			_choice2.Init();
			_highCoder.Init();
		}

		public uint Decode(SharpCompress.Compressors.LZMA.RangeCoder.Decoder rangeDecoder, uint posState)
		{
			if (_choice.Decode(rangeDecoder) == 0)
			{
				return _lowCoder[posState].Decode(rangeDecoder);
			}
			uint num = 8u;
			if (_choice2.Decode(rangeDecoder) == 0)
			{
				return num + _midCoder[posState].Decode(rangeDecoder);
			}
			num += 8;
			return num + _highCoder.Decode(rangeDecoder);
		}
	}

	private class LiteralDecoder
	{
		private struct Decoder2
		{
			private BitDecoder[] _decoders;

			public void Create()
			{
				_decoders = new BitDecoder[768];
			}

			public void Init()
			{
				for (int i = 0; i < 768; i++)
				{
					_decoders[i].Init();
				}
			}

			public byte DecodeNormal(SharpCompress.Compressors.LZMA.RangeCoder.Decoder rangeDecoder)
			{
				uint num = 1u;
				do
				{
					num = (num << 1) | _decoders[num].Decode(rangeDecoder);
				}
				while (num < 256);
				return (byte)num;
			}

			public byte DecodeWithMatchByte(SharpCompress.Compressors.LZMA.RangeCoder.Decoder rangeDecoder, byte matchByte)
			{
				uint num = 1u;
				do
				{
					uint num2 = (uint)(matchByte >> 7) & 1u;
					matchByte <<= 1;
					uint num3 = _decoders[(1 + num2 << 8) + num].Decode(rangeDecoder);
					num = (num << 1) | num3;
					if (num2 != num3)
					{
						while (num < 256)
						{
							num = (num << 1) | _decoders[num].Decode(rangeDecoder);
						}
						break;
					}
				}
				while (num < 256);
				return (byte)num;
			}
		}

		private Decoder2[] _coders;

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
				_coders = new Decoder2[num];
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

		private uint GetState(uint pos, byte prevByte)
		{
			return ((pos & _posMask) << _numPrevBits) + (uint)(prevByte >> 8 - _numPrevBits);
		}

		public byte DecodeNormal(SharpCompress.Compressors.LZMA.RangeCoder.Decoder rangeDecoder, uint pos, byte prevByte)
		{
			return _coders[GetState(pos, prevByte)].DecodeNormal(rangeDecoder);
		}

		public byte DecodeWithMatchByte(SharpCompress.Compressors.LZMA.RangeCoder.Decoder rangeDecoder, uint pos, byte prevByte, byte matchByte)
		{
			return _coders[GetState(pos, prevByte)].DecodeWithMatchByte(rangeDecoder, matchByte);
		}
	}

	private OutWindow _outWindow;

	private readonly BitDecoder[] _isMatchDecoders = new BitDecoder[192];

	private readonly BitDecoder[] _isRepDecoders = new BitDecoder[12];

	private readonly BitDecoder[] _isRepG0Decoders = new BitDecoder[12];

	private readonly BitDecoder[] _isRepG1Decoders = new BitDecoder[12];

	private readonly BitDecoder[] _isRepG2Decoders = new BitDecoder[12];

	private readonly BitDecoder[] _isRep0LongDecoders = new BitDecoder[192];

	private readonly BitTreeDecoder[] _posSlotDecoder = new BitTreeDecoder[4];

	private readonly BitDecoder[] _posDecoders = new BitDecoder[114];

	private BitTreeDecoder _posAlignDecoder = new BitTreeDecoder(4);

	private readonly LenDecoder _lenDecoder = new LenDecoder();

	private readonly LenDecoder _repLenDecoder = new LenDecoder();

	private readonly LiteralDecoder _literalDecoder = new LiteralDecoder();

	private int _dictionarySize;

	private uint _posStateMask;

	private Base.State _state;

	private uint _rep0;

	private uint _rep1;

	private uint _rep2;

	private uint _rep3;

	public Decoder()
	{
		_dictionarySize = -1;
		for (int i = 0; (long)i < 4L; i++)
		{
			_posSlotDecoder[i] = new BitTreeDecoder(6);
		}
	}

	private void CreateDictionary()
	{
		if (_dictionarySize < 0)
		{
			throw new InvalidParamException();
		}
		_outWindow = new OutWindow();
		int windowSize = Math.Max(_dictionarySize, 4096);
		_outWindow.Create(windowSize);
	}

	private void SetLiteralProperties(int lp, int lc)
	{
		if (lp > 8)
		{
			throw new InvalidParamException();
		}
		if (lc > 8)
		{
			throw new InvalidParamException();
		}
		_literalDecoder.Create(lp, lc);
	}

	private void SetPosBitsProperties(int pb)
	{
		if (pb > 4)
		{
			throw new InvalidParamException();
		}
		uint num = (uint)(1 << pb);
		_lenDecoder.Create(num);
		_repLenDecoder.Create(num);
		_posStateMask = num - 1;
	}

	private void Init()
	{
		for (uint num = 0u; num < 12; num++)
		{
			for (uint num2 = 0u; num2 <= _posStateMask; num2++)
			{
				uint num3 = (num << 4) + num2;
				_isMatchDecoders[num3].Init();
				_isRep0LongDecoders[num3].Init();
			}
			_isRepDecoders[num].Init();
			_isRepG0Decoders[num].Init();
			_isRepG1Decoders[num].Init();
			_isRepG2Decoders[num].Init();
		}
		_literalDecoder.Init();
		for (uint num = 0u; num < 4; num++)
		{
			_posSlotDecoder[num].Init();
		}
		for (uint num = 0u; num < 114; num++)
		{
			_posDecoders[num].Init();
		}
		_lenDecoder.Init();
		_repLenDecoder.Init();
		_posAlignDecoder.Init();
		_state.Init();
		_rep0 = 0u;
		_rep1 = 0u;
		_rep2 = 0u;
		_rep3 = 0u;
	}

	public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress)
	{
		if (_outWindow == null)
		{
			CreateDictionary();
		}
		_outWindow.Init(outStream);
		if (outSize > 0)
		{
			_outWindow.SetLimit(outSize);
		}
		else
		{
			_outWindow.SetLimit(long.MaxValue - _outWindow._total);
		}
		SharpCompress.Compressors.LZMA.RangeCoder.Decoder decoder = new SharpCompress.Compressors.LZMA.RangeCoder.Decoder();
		decoder.Init(inStream);
		Code(_dictionarySize, _outWindow, decoder);
		_outWindow.ReleaseStream();
		decoder.ReleaseStream();
		if (!decoder.IsFinished || (inSize > 0 && decoder._total != inSize))
		{
			throw new DataErrorException();
		}
		if (_outWindow.HasPending)
		{
			throw new DataErrorException();
		}
		_outWindow = null;
	}

	internal bool Code(int dictionarySize, OutWindow outWindow, SharpCompress.Compressors.LZMA.RangeCoder.Decoder rangeDecoder)
	{
		int num = Math.Max(dictionarySize, 1);
		outWindow.CopyPending();
		while (outWindow.HasSpace)
		{
			uint num2 = (uint)(int)outWindow._total & _posStateMask;
			if (_isMatchDecoders[(_state._index << 4) + num2].Decode(rangeDecoder) == 0)
			{
				byte @byte = outWindow.GetByte(0);
				byte b = (_state.IsCharState() ? _literalDecoder.DecodeNormal(rangeDecoder, (uint)outWindow._total, @byte) : _literalDecoder.DecodeWithMatchByte(rangeDecoder, (uint)outWindow._total, @byte, outWindow.GetByte((int)_rep0)));
				outWindow.PutByte(b);
				_state.UpdateChar();
				continue;
			}
			uint len;
			if (_isRepDecoders[_state._index].Decode(rangeDecoder) == 1)
			{
				if (_isRepG0Decoders[_state._index].Decode(rangeDecoder) == 0)
				{
					if (_isRep0LongDecoders[(_state._index << 4) + num2].Decode(rangeDecoder) == 0)
					{
						_state.UpdateShortRep();
						outWindow.PutByte(outWindow.GetByte((int)_rep0));
						continue;
					}
				}
				else
				{
					uint rep;
					if (_isRepG1Decoders[_state._index].Decode(rangeDecoder) == 0)
					{
						rep = _rep1;
					}
					else
					{
						if (_isRepG2Decoders[_state._index].Decode(rangeDecoder) == 0)
						{
							rep = _rep2;
						}
						else
						{
							rep = _rep3;
							_rep3 = _rep2;
						}
						_rep2 = _rep1;
					}
					_rep1 = _rep0;
					_rep0 = rep;
				}
				len = _repLenDecoder.Decode(rangeDecoder, num2) + 2;
				_state.UpdateRep();
			}
			else
			{
				_rep3 = _rep2;
				_rep2 = _rep1;
				_rep1 = _rep0;
				len = 2 + _lenDecoder.Decode(rangeDecoder, num2);
				_state.UpdateMatch();
				uint num3 = _posSlotDecoder[Base.GetLenToPosState(len)].Decode(rangeDecoder);
				if (num3 >= 4)
				{
					int num4 = (int)((num3 >> 1) - 1);
					_rep0 = (2 | (num3 & 1)) << num4;
					if (num3 < 14)
					{
						_rep0 += BitTreeDecoder.ReverseDecode(_posDecoders, _rep0 - num3 - 1, rangeDecoder, num4);
					}
					else
					{
						_rep0 += rangeDecoder.DecodeDirectBits(num4 - 4) << 4;
						_rep0 += _posAlignDecoder.ReverseDecode(rangeDecoder);
					}
				}
				else
				{
					_rep0 = num3;
				}
			}
			if (_rep0 >= outWindow._total || _rep0 >= num)
			{
				if (_rep0 == uint.MaxValue)
				{
					return true;
				}
				throw new DataErrorException();
			}
			outWindow.CopyBlock((int)_rep0, (int)len);
		}
		return false;
	}

	public void SetDecoderProperties(byte[] properties)
	{
		if (properties.Length < 1)
		{
			throw new InvalidParamException();
		}
		int lc = properties[0] % 9;
		int num = properties[0] / 9;
		int lp = num % 5;
		int num2 = num / 5;
		if (num2 > 4)
		{
			throw new InvalidParamException();
		}
		SetLiteralProperties(lp, lc);
		SetPosBitsProperties(num2);
		Init();
		if (properties.Length >= 5)
		{
			_dictionarySize = 0;
			for (int i = 0; i < 4; i++)
			{
				_dictionarySize += properties[1 + i] << i * 8;
			}
		}
	}

	public void Train(Stream stream)
	{
		if (_outWindow == null)
		{
			CreateDictionary();
		}
		_outWindow.Train(stream);
	}
}
