using System;
using SharpCompress.Common.Zip.Headers;
using SharpCompress.Compressors.Deflate;

namespace SharpCompress.Common.Zip;

internal class PkwareTraditionalEncryptionData
{
	private static readonly CRC32 CRC32 = new CRC32();

	private readonly uint[] _keys = new uint[3] { 305419896u, 591751049u, 878082192u };

	private readonly ArchiveEncoding _archiveEncoding;

	private byte MagicByte
	{
		get
		{
			ushort num = (ushort)((ushort)(_keys[2] & 0xFFFFu) | 2u);
			return (byte)(num * (num ^ 1) >> 8);
		}
	}

	private PkwareTraditionalEncryptionData(string password, ArchiveEncoding archiveEncoding)
	{
		_archiveEncoding = archiveEncoding;
		Initialize(password);
	}

	public static PkwareTraditionalEncryptionData ForRead(string password, ZipFileEntry header, byte[] encryptionHeader)
	{
		PkwareTraditionalEncryptionData pkwareTraditionalEncryptionData = new PkwareTraditionalEncryptionData(password, header.ArchiveEncoding);
		byte[] array = pkwareTraditionalEncryptionData.Decrypt(encryptionHeader, encryptionHeader.Length);
		if (array[11] != (byte)((header.Crc >> 24) & 0xFF))
		{
			if (!FlagUtility.HasFlag(header.Flags, HeaderFlags.UsePostDataDescriptor))
			{
				throw new CryptographicException("The password did not match.");
			}
			if (array[11] != (byte)((header.LastModifiedTime >> 8) & 0xFF))
			{
				throw new CryptographicException("The password did not match.");
			}
		}
		return pkwareTraditionalEncryptionData;
	}

	public byte[] Decrypt(byte[] cipherText, int length)
	{
		if (length > cipherText.Length)
		{
			throw new ArgumentOutOfRangeException("length", "Bad length during Decryption: the length parameter must be smaller than or equal to the size of the destination array.");
		}
		byte[] array = new byte[length];
		for (int i = 0; i < length; i++)
		{
			byte b = (byte)(cipherText[i] ^ MagicByte);
			UpdateKeys(b);
			array[i] = b;
		}
		return array;
	}

	public byte[] Encrypt(byte[] plainText, int length)
	{
		if (plainText == null)
		{
			throw new ArgumentNullException("plainText");
		}
		if (length > plainText.Length)
		{
			throw new ArgumentOutOfRangeException("length", "Bad length during Encryption: The length parameter must be smaller than or equal to the size of the destination array.");
		}
		byte[] array = new byte[length];
		for (int i = 0; i < length; i++)
		{
			byte byteValue = plainText[i];
			array[i] = (byte)(plainText[i] ^ MagicByte);
			UpdateKeys(byteValue);
		}
		return array;
	}

	private void Initialize(string password)
	{
		byte[] array = StringToByteArray(password);
		for (int i = 0; i < password.Length; i++)
		{
			UpdateKeys(array[i]);
		}
	}

	internal byte[] StringToByteArray(string value)
	{
		return _archiveEncoding.Password.GetBytes(value);
	}

	private void UpdateKeys(byte byteValue)
	{
		_keys[0] = (uint)CRC32.ComputeCrc32((int)_keys[0], byteValue);
		_keys[1] = _keys[1] + (byte)_keys[0];
		_keys[1] = _keys[1] * 134775813 + 1;
		_keys[2] = (uint)CRC32.ComputeCrc32((int)_keys[2], (byte)(_keys[1] >> 24));
	}
}
