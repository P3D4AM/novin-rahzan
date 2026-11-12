using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;

namespace Stimulsoft.Base.Helpers;

public static class StiPacker
{
	private static string EncryptedId;

	[DefaultValue(true)]
	public static bool AllowPacking { get; set; }

	public static byte[] Pack(byte[] bytes)
	{
		if (bytes == null || bytes.Length == 0 || !AllowPacking)
		{
			return bytes;
		}
		return AddZipSignature(StiGZipHelper.Pack(bytes));
	}

	public static byte[] Unpack(byte[] bytes)
	{
		if (bytes == null || bytes.Length == 0 || !IsPacked(bytes))
		{
			return bytes;
		}
		try
		{
			Array.Resize(ref bytes, bytes.Length - 3);
			return StiGZipHelper.Unpack(bytes);
		}
		catch (InvalidDataException)
		{
			return bytes;
		}
	}

	public static byte[] PackAndEncrypt(byte[] bytes, string encryptedId)
	{
		return StiEncryption.Encrypt(Pack(bytes), encryptedId);
	}

	public static byte[] UnpackAndDecrypt(byte[] bytes, string encryptedId)
	{
		return Unpack(StiEncryption.Decrypt(bytes, encryptedId));
	}

	public static string PackAndEncryptToString(byte[] bytes)
	{
		bytes = Pack(bytes);
		bytes = StiEncryption.Encrypt(bytes, EncryptedId);
		if (bytes == null)
		{
			return null;
		}
		return Convert.ToBase64String(bytes);
	}

	public static byte[] UnpackAndDecrypt(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		byte[] src = Convert.FromBase64String(str);
		src = StiEncryption.Decrypt(src, EncryptedId);
		return Unpack(src);
	}

	public static Stream Pack(Stream stream)
	{
		if (stream == null || stream.Length == 0L || !AllowPacking)
		{
			return stream;
		}
		return StiGZipHelper.Pack(stream);
	}

	public static Stream Unpack(Stream stream)
	{
		if (stream == null || stream.Length == 0L || !IsPacked(stream))
		{
			return stream;
		}
		try
		{
			return StiGZipHelper.Unpack(stream);
		}
		catch (InvalidDataException)
		{
			return stream;
		}
	}

	public static string PackToString(byte[] bytes)
	{
		bytes = Pack(bytes);
		if (bytes == null)
		{
			return null;
		}
		return Convert.ToBase64String(bytes);
	}

	public static byte[] UnpackFromString(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		byte[] bytes = Convert.FromBase64String(str);
		return Unpack(bytes);
	}

	public static byte[] PackToBytes(string str, bool allowPacking = true)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		byte[] array = StiGZipHelper.ConvertStringToByteArray(str);
		if (array != null && array.Length != 0 && AllowPacking && allowPacking)
		{
			array = AddZipSignature(StiGZipHelper.Pack(array));
		}
		return array;
	}

	public static string UnpackToString(byte[] bytes)
	{
		if (bytes == null || bytes.Length == 0)
		{
			return null;
		}
		bytes = Unpack(bytes);
		return StiGZipHelper.ConvertByteArrayToString(bytes);
	}

	public static byte[] PackToBytes(XElement element)
	{
		if (element == null)
		{
			return null;
		}
		return PackToBytes(((object)element).ToString());
	}

	public static XElement UnpackToXElement(byte[] bytes)
	{
		if (bytes == null || bytes.Length == 0)
		{
			return null;
		}
		string text = UnpackToString(bytes);
		if (text == null)
		{
			return null;
		}
		return XElement.Parse(text);
	}

	private static byte[] AddZipSignature(byte[] bytes)
	{
		Array.Resize(ref bytes, bytes.Length + 3);
		bytes[^3] = 90;
		bytes[^2] = 73;
		bytes[^1] = 80;
		return bytes;
	}

	public static bool IsPacked(Stream stream)
	{
		if (stream.Length < 4)
		{
			return false;
		}
		stream.Seek(stream.Length - 3, SeekOrigin.Current);
		int first = stream.ReadByte();
		int second = stream.ReadByte();
		int third = stream.ReadByte();
		stream.Seek(-stream.Length - 3, SeekOrigin.Current);
		return IsPacked(first, second, third);
	}

	public static bool IsPacked(byte[] bytes)
	{
		if (bytes.Length > 3)
		{
			return IsPacked(bytes[^3], bytes[^2], bytes[^1]);
		}
		return false;
	}

	private static bool IsPacked(int first, int second, int third)
	{
		if (first == 90 && second == 73)
		{
			return third == 80;
		}
		return false;
	}

	static StiPacker()
	{
		EncryptedId = "8pTP&%#5uK889910";
		AllowPacking = true;
	}
}
