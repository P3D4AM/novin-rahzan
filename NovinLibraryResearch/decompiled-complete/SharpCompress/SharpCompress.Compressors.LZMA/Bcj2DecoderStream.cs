using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCompress.Compressors.LZMA;

internal class Bcj2DecoderStream : DecoderStream2
{
	private class RangeDecoder
	{
		internal readonly Stream _mStream;

		internal uint _range;

		internal uint _code;

		public RangeDecoder(Stream stream)
		{
			_mStream = stream;
			_range = uint.MaxValue;
			for (int i = 0; i < 5; i++)
			{
				_code = (_code << 8) | ReadByte();
			}
		}

		public byte ReadByte()
		{
			int num = _mStream.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return (byte)num;
		}

		public void Dispose()
		{
			_mStream.Dispose();
		}
	}

	private class StatusDecoder
	{
		private const int NUM_MOVE_BITS = 5;

		private const int K_NUM_BIT_MODEL_TOTAL_BITS = 11;

		private const uint K_BIT_MODEL_TOTAL = 2048u;

		private uint _prob;

		public StatusDecoder()
		{
			_prob = 1024u;
		}

		private void UpdateModel(uint symbol)
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

		public uint Decode(RangeDecoder decoder)
		{
			uint num = (decoder._range >> 11) * _prob;
			if (decoder._code < num)
			{
				decoder._range = num;
				_prob += 2048 - _prob >> 5;
				if (decoder._range < 16777216)
				{
					decoder._code = (decoder._code << 8) | decoder.ReadByte();
					decoder._range <<= 8;
				}
				return 0u;
			}
			decoder._range -= num;
			decoder._code -= num;
			_prob -= _prob >> 5;
			if (decoder._range < 16777216)
			{
				decoder._code = (decoder._code << 8) | decoder.ReadByte();
				decoder._range <<= 8;
			}
			return 1u;
		}
	}

	private const int K_NUM_TOP_BITS = 24;

	private const uint K_TOP_VALUE = 16777216u;

	private readonly Stream _mMainStream;

	private readonly Stream _mCallStream;

	private readonly Stream _mJumpStream;

	private readonly RangeDecoder _mRangeDecoder;

	private readonly StatusDecoder[] _mStatusDecoder;

	private long _mWritten;

	private readonly IEnumerator<byte> _mIter;

	private bool _mFinished;

	private bool _isDisposed;

	public Bcj2DecoderStream(Stream[] streams, byte[] info, long limit)
	{
		if (info != null && info.Length != 0)
		{
			throw new NotSupportedException();
		}
		if (streams.Length != 4)
		{
			throw new NotSupportedException();
		}
		_mMainStream = streams[0];
		_mCallStream = streams[1];
		_mJumpStream = streams[2];
		_mRangeDecoder = new RangeDecoder(streams[3]);
		_mStatusDecoder = new StatusDecoder[258];
		for (int i = 0; i < _mStatusDecoder.Length; i++)
		{
			_mStatusDecoder[i] = new StatusDecoder();
		}
		_mIter = Run().GetEnumerator();
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			base.Dispose(disposing);
			_mMainStream.Dispose();
			_mCallStream.Dispose();
			_mJumpStream.Dispose();
		}
	}

	private static bool IsJcc(byte b0, byte b1)
	{
		if (b0 == 15)
		{
			return (b1 & 0xF0) == 128;
		}
		return false;
	}

	private static bool IsJ(byte b0, byte b1)
	{
		if ((b1 & 0xFE) != 232)
		{
			return IsJcc(b0, b1);
		}
		return true;
	}

	private static int GetIndex(byte b0, byte b1)
	{
		return b1 switch
		{
			232 => b0, 
			233 => 256, 
			_ => 257, 
		};
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (count == 0 || _mFinished)
		{
			return 0;
		}
		for (int i = 0; i < count; i++)
		{
			if (!_mIter.MoveNext())
			{
				_mFinished = true;
				return i;
			}
			buffer[offset + i] = _mIter.Current;
		}
		return count;
	}

	public override int ReadByte()
	{
		if (_mFinished)
		{
			return -1;
		}
		if (!_mIter.MoveNext())
		{
			_mFinished = true;
			return -1;
		}
		return _mIter.Current;
	}

	public IEnumerable<byte> Run()
	{
		byte prevByte = 0;
		uint processedBytes = 0u;
		while (true)
		{
			byte b = 0;
			uint j;
			for (j = 0u; j < 262144; j++)
			{
				int num = _mMainStream.ReadByte();
				if (num < 0)
				{
					yield break;
				}
				b = (byte)num;
				_mWritten++;
				yield return b;
				if (IsJ(prevByte, b))
				{
					break;
				}
				prevByte = b;
			}
			processedBytes += j;
			if (j == 262144)
			{
				continue;
			}
			if (_mStatusDecoder[GetIndex(prevByte, b)].Decode(_mRangeDecoder) == 1)
			{
				Stream stream = ((b == 232) ? _mCallStream : _mJumpStream);
				uint num2 = 0u;
				for (j = 0u; j < 4; j++)
				{
					int num3 = stream.ReadByte();
					if (num3 < 0)
					{
						throw new EndOfStreamException();
					}
					num2 <<= 8;
					num2 |= (uint)num3;
				}
				uint dest = num2 - (uint)(int)(_mWritten + 4);
				_mWritten++;
				yield return (byte)dest;
				_mWritten++;
				yield return (byte)(dest >> 8);
				_mWritten++;
				yield return (byte)(dest >> 16);
				_mWritten++;
				yield return (byte)(dest >> 24);
				prevByte = (byte)(dest >> 24);
				processedBytes += 4;
			}
			else
			{
				prevByte = b;
			}
		}
	}
}
