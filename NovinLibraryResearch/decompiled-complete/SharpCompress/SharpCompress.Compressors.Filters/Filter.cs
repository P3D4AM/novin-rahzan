using System;
using System.IO;

namespace SharpCompress.Compressors.Filters;

internal abstract class Filter : Stream
{
	protected bool _isEncoder;

	protected Stream _baseStream;

	private readonly byte[] _tail;

	private readonly byte[] _window;

	private int _transformed;

	private int _read;

	private bool _endReached;

	private bool _isDisposed;

	public override bool CanRead => !_isEncoder;

	public override bool CanSeek => false;

	public override bool CanWrite => _isEncoder;

	public override long Length => _baseStream.Length;

	public override long Position
	{
		get
		{
			return _baseStream.Position;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	protected Filter(bool isEncoder, Stream baseStream, int lookahead)
	{
		_isEncoder = isEncoder;
		_baseStream = baseStream;
		_tail = new byte[lookahead - 1];
		_window = new byte[_tail.Length * 2];
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
		if (_transformed > 0)
		{
			int num2 = _transformed;
			if (num2 > count)
			{
				num2 = count;
			}
			Buffer.BlockCopy(_tail, 0, buffer, offset, num2);
			_transformed -= num2;
			_read -= num2;
			offset += num2;
			count -= num2;
			num += num2;
			Buffer.BlockCopy(_tail, num2, _tail, 0, _read);
		}
		if (count == 0)
		{
			return num;
		}
		int num3 = _read;
		if (num3 > count)
		{
			num3 = count;
		}
		Buffer.BlockCopy(_tail, 0, buffer, offset, num3);
		_read -= num3;
		Buffer.BlockCopy(_tail, num3, _tail, 0, _read);
		while (!_endReached && num3 < count)
		{
			int num4 = _baseStream.Read(buffer, offset + num3, count - num3);
			num3 += num4;
			if (num4 == 0)
			{
				_endReached = true;
			}
		}
		while (!_endReached && _read < _tail.Length)
		{
			int num5 = _baseStream.Read(_tail, _read, _tail.Length - _read);
			_read += num5;
			if (num5 == 0)
			{
				_endReached = true;
			}
		}
		if (num3 > _tail.Length)
		{
			_transformed = Transform(buffer, offset, num3);
			offset += _transformed;
			count -= _transformed;
			num += _transformed;
			num3 -= _transformed;
			_transformed = 0;
		}
		if (count == 0)
		{
			return num;
		}
		Buffer.BlockCopy(buffer, offset, _window, 0, num3);
		Buffer.BlockCopy(_tail, 0, _window, num3, _read);
		if (num3 + _read > _tail.Length)
		{
			_transformed = Transform(_window, 0, num3 + _read);
		}
		else
		{
			_transformed = num3 + _read;
		}
		Buffer.BlockCopy(_window, 0, buffer, offset, num3);
		Buffer.BlockCopy(_window, num3, _tail, 0, _read);
		num += num3;
		_transformed -= num3;
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
		Transform(buffer, offset, count);
		_baseStream.Write(buffer, offset, count);
	}

	protected abstract int Transform(byte[] buffer, int offset, int count);
}
