using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCompress.Common.Rar;

internal sealed class RarCryptoWrapper : Stream
{
	private readonly Stream _actualStream;

	private readonly byte[] _salt;

	private RarRijndael _rijndael;

	private readonly Queue<byte> _data = new Queue<byte>();

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position { get; set; }

	public RarCryptoWrapper(Stream actualStream, string password, byte[] salt)
	{
		_actualStream = actualStream;
		_salt = salt;
		_rijndael = RarRijndael.InitializeFrom(password, salt);
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_salt == null)
		{
			return _actualStream.Read(buffer, offset, count);
		}
		return ReadAndDecrypt(buffer, offset, count);
	}

	public int ReadAndDecrypt(byte[] buffer, int offset, int count)
	{
		int count2 = _data.Count;
		int num = count - count2;
		if (num > 0)
		{
			int num2 = num + ((~num + 1) & 0xF);
			Span<byte> span = stackalloc byte[16];
			for (int i = 0; i < num2 / 16; i++)
			{
				_actualStream.Read(span);
				byte[] array = _rijndael.ProcessBlock(span);
				foreach (byte item in array)
				{
					_data.Enqueue(item);
				}
			}
			for (int k = 0; k < count; k++)
			{
				buffer[offset + k] = _data.Dequeue();
			}
		}
		return count;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (_rijndael != null)
		{
			_rijndael.Dispose();
			_rijndael = null;
		}
		base.Dispose(disposing);
	}
}
