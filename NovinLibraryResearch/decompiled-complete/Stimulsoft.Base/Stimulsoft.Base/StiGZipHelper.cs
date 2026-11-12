using System;
using System.IO;
using System.IO.Compression;

namespace Stimulsoft.Base;

public sealed class StiGZipHelper
{
	public static byte[] ConvertStringToByteArray(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using StreamWriter streamWriter = new StreamWriter(memoryStream);
		streamWriter.Write(str);
		streamWriter.Flush();
		memoryStream.Flush();
		memoryStream.Close();
		return memoryStream.ToArray();
	}

	public static string ConvertByteArrayToString(byte[] bytes)
	{
		if (bytes == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream(bytes);
		using StreamReader streamReader = new StreamReader(memoryStream);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		memoryStream.Flush();
		memoryStream.Close();
		return result;
	}

	public static string Pack(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		return Convert.ToBase64String(Pack(ConvertStringToByteArray(str)));
	}

	public static string Unpack(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		return ConvertByteArrayToString(Unpack(Convert.FromBase64String(str)));
	}

	public static byte[] Pack(byte[] bytes)
	{
		if (bytes == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true);
		gZipStream.Write(bytes, 0, bytes.Length);
		gZipStream.Close();
		return memoryStream.ToArray();
	}

	public static Stream Pack(Stream stream)
	{
		MemoryStream memoryStream = new MemoryStream();
		using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
		byte[] array = new byte[2048];
		while (true)
		{
			int num = gZipStream.Read(array, 0, array.Length);
			if (num <= 0)
			{
				break;
			}
			memoryStream.Write(array, 0, num);
		}
		return memoryStream;
	}

	public static byte[] Unpack(byte[] bytes)
	{
		using MemoryStream stream = new MemoryStream(bytes);
		using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
		using MemoryStream memoryStream = new MemoryStream();
		byte[] array = new byte[2048];
		while (true)
		{
			int num = gZipStream.Read(array, 0, array.Length);
			if (num <= 0)
			{
				break;
			}
			memoryStream.Write(array, 0, num);
		}
		return memoryStream.ToArray();
	}

	public static Stream Unpack(Stream stream)
	{
		MemoryStream memoryStream = new MemoryStream();
		using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
		byte[] array = new byte[2048];
		while (true)
		{
			int num = gZipStream.Read(array, 0, array.Length);
			if (num > 0)
			{
				memoryStream.Write(array, 0, num);
				continue;
			}
			break;
		}
		return memoryStream;
	}

	public static void Unpack(Stream stream, Stream resultStream)
	{
		using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
		{
			byte[] array = new byte[2048];
			while (true)
			{
				int num = gZipStream.Read(array, 0, array.Length);
				if (num > 0)
				{
					resultStream.Write(array, 0, num);
					continue;
				}
				break;
			}
		}
		resultStream.Seek(0L, SeekOrigin.Begin);
	}
}
