using System;
using System.IO;

namespace SharpCompress.Compressors.Filters;

internal class BCJ2Filter : Stream
{
	private readonly Stream _baseStream;

	private readonly byte[] _input = new byte[4096];

	private int _inputOffset;

	private int _inputCount;

	private bool _endReached;

	private long _position;

	private readonly byte[] _output = new byte[4];

	private int _outputOffset;

	private int _outputCount;

	private readonly byte[] _control;

	private readonly byte[] _data1;

	private readonly byte[] _data2;

	private int _controlPos;

	private int _data1Pos;

	private int _data2Pos;

	private readonly ushort[] _p = new ushort[258];

	private uint _range;

	private uint _code;

	private byte _prevByte;

	private bool _isDisposed;

	private const int K_NUM_TOP_BITS = 24;

	private const int K_TOP_VALUE = 16777216;

	private const int K_NUM_BIT_MODEL_TOTAL_BITS = 11;

	private const int K_BIT_MODEL_TOTAL = 2048;

	private const int K_NUM_MOVE_BITS = 5;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length => _baseStream.Length + _data1.Length + _data2.Length;

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	private static bool IsJ(byte b0, byte b1)
	{
		if ((b1 & 0xFE) != 232)
		{
			return IsJcc(b0, b1);
		}
		return true;
	}

	private static bool IsJcc(byte b0, byte b1)
	{
		if (b0 == 15)
		{
			return (b1 & 0xF0) == 128;
		}
		return false;
	}

	public BCJ2Filter(byte[] control, byte[] data1, byte[] data2, Stream baseStream)
	{
		_control = control;
		_data1 = data1;
		_data2 = data2;
		_baseStream = baseStream;
		for (int i = 0; i < _p.Length; i++)
		{
			_p[i] = 1024;
		}
		_code = 0u;
		_range = uint.MaxValue;
		for (int i = 0; i < 5; i++)
		{
			_code = (_code << 8) | control[_controlPos++];
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			base.Dispose(disposing);
			_baseStream.Dispose();
		}
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = 0;
		byte b = 0;
		while (!_endReached && num < count)
		{
			while (_outputOffset < _outputCount)
			{
				b = _output[_outputOffset++];
				buffer[offset++] = b;
				num++;
				_position++;
				_prevByte = b;
				if (num == count)
				{
					return num;
				}
			}
			if (_inputOffset == _inputCount)
			{
				_inputOffset = 0;
				_inputCount = _baseStream.Read(_input, 0, _input.Length);
				if (_inputCount == 0)
				{
					_endReached = true;
					break;
				}
			}
			b = _input[_inputOffset++];
			buffer[offset++] = b;
			num++;
			_position++;
			if (!IsJ(_prevByte, b))
			{
				_prevByte = b;
				continue;
			}
			int num2 = b switch
			{
				232 => _prevByte, 
				233 => 256, 
				_ => 257, 
			};
			uint num3 = (_range >> 11) * _p[num2];
			if (_code < num3)
			{
				_range = num3;
				_p[num2] += (ushort)(2048 - _p[num2] >> 5);
				if (_range < 16777216)
				{
					_range <<= 8;
					_code = (_code << 8) | _control[_controlPos++];
				}
				_prevByte = b;
				continue;
			}
			_range -= num3;
			_code -= num3;
			_p[num2] -= (ushort)(_p[num2] >> 5);
			if (_range < 16777216)
			{
				_range <<= 8;
				_code = (_code << 8) | _control[_controlPos++];
			}
			uint num4 = (uint)((b != 232) ? ((_data2[_data2Pos++] << 24) | (_data2[_data2Pos++] << 16) | (_data2[_data2Pos++] << 8) | _data2[_data2Pos++]) : ((_data1[_data1Pos++] << 24) | (_data1[_data1Pos++] << 16) | (_data1[_data1Pos++] << 8) | _data1[_data1Pos++]));
			num4 -= (uint)(int)(_position + 4);
			_output[0] = (byte)num4;
			_output[1] = (byte)(num4 >> 8);
			_output[2] = (byte)(num4 >> 16);
			_output[3] = (byte)(num4 >> 24);
			_outputOffset = 0;
			_outputCount = 4;
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}
}
