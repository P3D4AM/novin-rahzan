using System;
using System.IO;

namespace SharpCompress.Compressors.LZMA.LZ;

internal class OutWindow
{
	private byte[] _buffer;

	private int _windowSize;

	private int _pos;

	private int _streamPos;

	private int _pendingLen;

	private int _pendingDist;

	private Stream _stream;

	public long _total;

	public long _limit;

	public bool HasSpace
	{
		get
		{
			if (_pos < _windowSize)
			{
				return _total < _limit;
			}
			return false;
		}
	}

	public bool HasPending => _pendingLen > 0;

	public int AvailableBytes => _pos - _streamPos;

	public void Create(int windowSize)
	{
		if (_windowSize != windowSize)
		{
			_buffer = new byte[windowSize];
		}
		else
		{
			_buffer[windowSize - 1] = 0;
		}
		_windowSize = windowSize;
		_pos = 0;
		_streamPos = 0;
		_pendingLen = 0;
		_total = 0L;
		_limit = 0L;
	}

	public void Reset()
	{
		Create(_windowSize);
	}

	public void Init(Stream stream)
	{
		ReleaseStream();
		_stream = stream;
	}

	public void Train(Stream stream)
	{
		long length = stream.Length;
		int num = (int)((length < _windowSize) ? length : _windowSize);
		stream.Position = length - num;
		_total = 0L;
		_limit = num;
		_pos = _windowSize - num;
		CopyStream(stream, num);
		if (_pos == _windowSize)
		{
			_pos = 0;
		}
		_streamPos = _pos;
	}

	public void ReleaseStream()
	{
		Flush();
		_stream = null;
	}

	public void Flush()
	{
		if (_stream == null)
		{
			return;
		}
		int num = _pos - _streamPos;
		if (num != 0)
		{
			_stream.Write(_buffer, _streamPos, num);
			if (_pos >= _windowSize)
			{
				_pos = 0;
			}
			_streamPos = _pos;
		}
	}

	public void CopyBlock(int distance, int len)
	{
		int num = len;
		int num2 = _pos - distance - 1;
		if (num2 < 0)
		{
			num2 += _windowSize;
		}
		while (num > 0 && _pos < _windowSize && _total < _limit)
		{
			if (num2 >= _windowSize)
			{
				num2 = 0;
			}
			_buffer[_pos++] = _buffer[num2++];
			_total++;
			if (_pos >= _windowSize)
			{
				Flush();
			}
			num--;
		}
		_pendingLen = num;
		_pendingDist = distance;
	}

	public void PutByte(byte b)
	{
		_buffer[_pos++] = b;
		_total++;
		if (_pos >= _windowSize)
		{
			Flush();
		}
	}

	public byte GetByte(int distance)
	{
		int num = _pos - distance - 1;
		if (num < 0)
		{
			num += _windowSize;
		}
		return _buffer[num];
	}

	public int CopyStream(Stream stream, int len)
	{
		int num = len;
		while (num > 0 && _pos < _windowSize && _total < _limit)
		{
			int num2 = _windowSize - _pos;
			if (num2 > _limit - _total)
			{
				num2 = (int)(_limit - _total);
			}
			if (num2 > num)
			{
				num2 = num;
			}
			int num3 = stream.Read(_buffer, _pos, num2);
			if (num3 == 0)
			{
				throw new DataErrorException();
			}
			num -= num3;
			_pos += num3;
			_total += num3;
			if (_pos >= _windowSize)
			{
				Flush();
			}
		}
		return len - num;
	}

	public void SetLimit(long size)
	{
		_limit = _total + size;
	}

	public int Read(byte[] buffer, int offset, int count)
	{
		if (_streamPos >= _pos)
		{
			return 0;
		}
		int num = _pos - _streamPos;
		if (num > count)
		{
			num = count;
		}
		Buffer.BlockCopy(_buffer, _streamPos, buffer, offset, num);
		_streamPos += num;
		if (_streamPos >= _windowSize)
		{
			_pos = 0;
			_streamPos = 0;
		}
		return num;
	}

	public void CopyPending()
	{
		if (_pendingLen > 0)
		{
			CopyBlock(_pendingDist, _pendingLen);
		}
	}
}
