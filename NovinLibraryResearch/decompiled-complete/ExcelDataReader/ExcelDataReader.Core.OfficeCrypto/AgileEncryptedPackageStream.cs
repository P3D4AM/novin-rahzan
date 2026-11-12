using System;
using System.IO;
using System.Security.Cryptography;

namespace ExcelDataReader.Core.OfficeCrypto;

internal class AgileEncryptedPackageStream : Stream
{
	private const int SegmentLength = 4096;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => DecryptedLength;

	public override long Position
	{
		get
		{
			return Offset - 4096 + SegmentOffset;
		}
		set
		{
			Seek(value, SeekOrigin.Begin);
		}
	}

	private Stream Stream { get; set; }

	private byte[] Key { get; }

	private byte[] IV { get; }

	private HashIdentifier HashAlgorithm { get; }

	private EncryptionInfo Encryption { get; }

	private int Offset { get; set; }

	private byte[] SegmentBytes { get; set; } = new byte[4096];


	private int SegmentOffset { get; set; }

	private int SegmentIndex { get; set; }

	private int DecryptedLength { get; set; }

	public AgileEncryptedPackageStream(Stream stream, byte[] key, byte[] iv, EncryptionInfo encryption)
	{
		Stream = stream;
		Key = key;
		IV = iv;
		Encryption = encryption;
		Stream.Read(SegmentBytes, 0, 8);
		DecryptedLength = BitConverter.ToInt32(SegmentBytes, 0);
		ReadSegment();
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		if (Position >= Length)
		{
			throw new InvalidOperationException("Tried to read past the end of the encrypted stream");
		}
		int num = 0;
		while (num < count)
		{
			if (SegmentOffset == SegmentBytes.Length)
			{
				ReadSegment();
				SegmentOffset = 0;
			}
			int num2 = Math.Min(count - num, SegmentBytes.Length - SegmentOffset);
			Array.Copy(SegmentBytes, SegmentOffset, buffer, offset + num, num2);
			num += num2;
			SegmentOffset += num2;
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		switch (origin)
		{
		case SeekOrigin.Begin:
			SegmentIndex = (int)(offset / 4096);
			Offset = SegmentIndex * 4096;
			SegmentOffset = (int)(offset % 4096);
			if (Offset < Length)
			{
				ReadSegment();
			}
			return Position;
		case SeekOrigin.Current:
			return Seek(Position + offset, SeekOrigin.Begin);
		case SeekOrigin.End:
			return Seek(Length + offset, SeekOrigin.Begin);
		default:
			return Offset;
		}
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Stream?.Dispose();
			Stream = null;
		}
		base.Dispose(disposing);
	}

	private void ReadSegment()
	{
		byte[] iv = Encryption.GenerateBlockKey(SegmentIndex, IV);
		Stream.Seek(8 + Offset, SeekOrigin.Begin);
		Stream.Read(SegmentBytes, 0, 4096);
		using (SymmetricAlgorithm algo = Encryption.CreateCipher())
		{
			SegmentBytes = CryptoHelpers.DecryptBytes(algo, SegmentBytes, Key, iv);
		}
		SegmentIndex++;
		Offset += 4096;
	}
}
