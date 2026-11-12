using System;
using System.IO;
using System.Security.Cryptography;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class StandardEncryptedPackageStream : Stream
{
	public override bool CanRead => BaseStream.CanRead;

	public override bool CanSeek => BaseStream.CanSeek;

	public override bool CanWrite => BaseStream.CanWrite;

	public override long Length => DecryptedLength;

	public override long Position
	{
		get
		{
			return BaseStream.Position;
		}
		set
		{
			BaseStream.Position = value;
		}
	}

	private Stream BaseStream { get; set; }

	private SymmetricAlgorithm Cipher { get; set; }

	private ICryptoTransform Decryptor { get; set; }

	private long DecryptedLength { get; }

	public StandardEncryptedPackageStream(Stream underlyingStream, byte[] secretKey, StandardEncryption encryption)
	{
		Cipher = CryptoHelpers.CreateCipher(encryption.CipherAlgorithm, encryption.KeySize, encryption.BlockSize, CipherMode.ECB);
		Decryptor = Cipher.CreateDecryptor(secretKey, encryption.SaltValue);
		byte[] array = new byte[8];
		underlyingStream.Read(array, 0, 8);
		DecryptedLength = BitConverter.ToInt32(array, 0);
		BaseStream = new CryptoStream(underlyingStream, Decryptor, CryptoStreamMode.Read);
	}

	public override void Flush()
	{
		BaseStream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return BaseStream.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return BaseStream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		BaseStream.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		BaseStream.Write(buffer, offset, count);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Decryptor?.Dispose();
			Decryptor = null;
			((IDisposable)Cipher)?.Dispose();
			Cipher = null;
			BaseStream?.Dispose();
			BaseStream = null;
		}
		base.Dispose(disposing);
	}
}
