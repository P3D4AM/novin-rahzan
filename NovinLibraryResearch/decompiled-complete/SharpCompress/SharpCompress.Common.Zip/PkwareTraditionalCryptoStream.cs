using System;
using System.IO;

namespace SharpCompress.Common.Zip;

internal class PkwareTraditionalCryptoStream : Stream
{
	private readonly PkwareTraditionalEncryptionData _encryptor;

	private readonly CryptoMode _mode;

	private readonly Stream _stream;

	private bool _isDisposed;

	public override bool CanRead => _mode == CryptoMode.Decrypt;

	public override bool CanSeek => false;

	public override bool CanWrite => _mode == CryptoMode.Encrypt;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public PkwareTraditionalCryptoStream(Stream stream, PkwareTraditionalEncryptionData encryptor, CryptoMode mode)
	{
		_encryptor = encryptor;
		_stream = stream;
		_mode = mode;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_mode == CryptoMode.Encrypt)
		{
			throw new NotSupportedException("This stream does not encrypt via Read()");
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		byte[] array = new byte[count];
		int num = _stream.Read(array, 0, count);
		Buffer.BlockCopy(_encryptor.Decrypt(array, num), 0, buffer, offset, num);
		return num;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (_mode == CryptoMode.Decrypt)
		{
			throw new NotSupportedException("This stream does not Decrypt via Write()");
		}
		if (count != 0)
		{
			byte[] array;
			if (offset != 0)
			{
				array = new byte[count];
				Buffer.BlockCopy(buffer, offset, array, 0, count);
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = _encryptor.Encrypt(array, count);
			_stream.Write(array2, 0, array2.Length);
		}
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			base.Dispose(disposing);
			_stream.Dispose();
		}
	}
}
