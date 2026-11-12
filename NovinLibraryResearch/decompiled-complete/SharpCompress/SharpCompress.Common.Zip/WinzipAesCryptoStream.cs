using System;
using System.Buffers.Binary;
using System.IO;
using System.Security.Cryptography;

namespace SharpCompress.Common.Zip;

internal class WinzipAesCryptoStream : Stream
{
	private const int BLOCK_SIZE_IN_BYTES = 16;

	private readonly SymmetricAlgorithm _cipher;

	private readonly byte[] _counter = new byte[16];

	private readonly Stream _stream;

	private readonly ICryptoTransform _transform;

	private int _nonce = 1;

	private byte[] _counterOut = new byte[16];

	private bool _isFinalBlock;

	private long _totalBytesLeftToRead;

	private bool _isDisposed;

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

	internal WinzipAesCryptoStream(Stream stream, WinzipAesEncryptionData winzipAesEncryptionData, long length)
	{
		_stream = stream;
		_totalBytesLeftToRead = length;
		_cipher = CreateCipher(winzipAesEncryptionData);
		byte[] rgbIV = new byte[16];
		_transform = _cipher.CreateEncryptor(winzipAesEncryptionData.KeyBytes, rgbIV);
	}

	private SymmetricAlgorithm CreateCipher(WinzipAesEncryptionData winzipAesEncryptionData)
	{
		Aes aes = Aes.Create();
		aes.BlockSize = 128;
		aes.KeySize = winzipAesEncryptionData.KeyBytes.Length * 8;
		aes.Mode = CipherMode.ECB;
		aes.Padding = PaddingMode.None;
		return aes;
	}

	protected override void Dispose(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			if (disposing)
			{
				Span<byte> buffer = stackalloc byte[10];
				_stream.ReadFully(buffer);
				_stream.Dispose();
			}
		}
	}

	public override void Flush()
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (_totalBytesLeftToRead == 0L)
		{
			return 0;
		}
		int count2 = count;
		if (count > _totalBytesLeftToRead)
		{
			count2 = (int)_totalBytesLeftToRead;
		}
		int num = _stream.Read(buffer, offset, count2);
		_totalBytesLeftToRead -= num;
		ReadTransformBlocks(buffer, offset, num);
		return num;
	}

	private int ReadTransformOneBlock(byte[] buffer, int offset, int last)
	{
		if (_isFinalBlock)
		{
			throw new InvalidOperationException();
		}
		int num = last - offset;
		int num2 = ((num > 16) ? 16 : num);
		BinaryPrimitives.WriteInt32LittleEndian(_counter, _nonce++);
		if (num2 == num && _totalBytesLeftToRead == 0L)
		{
			_counterOut = _transform.TransformFinalBlock(_counter, 0, 16);
			_isFinalBlock = true;
		}
		else
		{
			_transform.TransformBlock(_counter, 0, 16, _counterOut, 0);
		}
		XorInPlace(buffer, offset, num2);
		return num2;
	}

	private void XorInPlace(byte[] buffer, int offset, int count)
	{
		for (int i = 0; i < count; i++)
		{
			buffer[offset + i] = (byte)(_counterOut[i] ^ buffer[offset + i]);
		}
	}

	private void ReadTransformBlocks(byte[] buffer, int offset, int count)
	{
		int i = offset;
		int num2;
		for (int num = count + offset; i < buffer.Length && i < num; i += num2)
		{
			num2 = ReadTransformOneBlock(buffer, i, num);
		}
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
