using System.Text;

namespace Stimulsoft.Report.Export;

public sealed class StiEncodingHelper
{
	public static string[] GetEncodingNames()
	{
		return GetEncodingNames(GetEncodings());
	}

	public static string[] GetEncodingNames(Encoding[] encodings)
	{
		string[] array = new string[encodings.Length];
		for (int i = 0; i < encodings.Length; i++)
		{
			array[i] = encodings[i].EncodingName;
		}
		return array;
	}

	public static Encoding[] GetEncodings()
	{
		try
		{
			return new Encoding[13]
			{
				Encoding.Default,
				Encoding.ASCII,
				Encoding.BigEndianUnicode,
				Encoding.Unicode,
				Encoding.UTF7,
				Encoding.UTF8,
				Encoding.GetEncoding(1250),
				Encoding.GetEncoding(1251),
				Encoding.GetEncoding(1252),
				Encoding.GetEncoding(1253),
				Encoding.GetEncoding(1254),
				Encoding.GetEncoding(1255),
				Encoding.GetEncoding(1256)
			};
		}
		catch
		{
			return new Encoding[6]
			{
				Encoding.Default,
				Encoding.ASCII,
				Encoding.BigEndianUnicode,
				Encoding.Unicode,
				Encoding.UTF7,
				Encoding.UTF8
			};
		}
	}

	public static Encoding GetEncodingFromName(string encodingName)
	{
		return GetEncodingFromName(encodingName, GetEncodings());
	}

	public static Encoding GetEncodingFromName(string encodingName, Encoding[] encodings)
	{
		foreach (Encoding encoding in encodings)
		{
			if (encoding.EncodingName == encodingName)
			{
				return encoding;
			}
		}
		return Encoding.UTF8;
	}
}
