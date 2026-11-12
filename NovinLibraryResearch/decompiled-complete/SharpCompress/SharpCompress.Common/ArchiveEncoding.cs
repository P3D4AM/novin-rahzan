using System;
using System.Text;

namespace SharpCompress.Common;

public class ArchiveEncoding
{
	public Encoding Default { get; set; }

	public Encoding Password { get; set; }

	public Encoding? Forced { get; set; }

	public Func<byte[], int, int, string>? CustomDecoder { get; set; }

	public ArchiveEncoding()
		: this(Encoding.Default, Encoding.Default)
	{
	}

	public ArchiveEncoding(Encoding def, Encoding password)
	{
		Default = def;
		Password = password;
	}

	public string Decode(byte[] bytes)
	{
		return Decode(bytes, 0, bytes.Length);
	}

	public string Decode(byte[] bytes, int start, int length)
	{
		return GetDecoder()(bytes, start, length);
	}

	public string DecodeUTF8(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
	}

	public byte[] Encode(string str)
	{
		return GetEncoding().GetBytes(str);
	}

	public Encoding GetEncoding()
	{
		return Forced ?? Default ?? Encoding.UTF8;
	}

	public Func<byte[], int, int, string> GetDecoder()
	{
		return CustomDecoder ?? ((Func<byte[], int, int, string>)((byte[] bytes, int index, int count) => GetEncoding().GetString(bytes, index, count)));
	}
}
