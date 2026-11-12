using System.IO;

namespace Stimulsoft.Base.Helpers;

public static class StiBytesToStringConverter
{
	public static string ConvertBytesToString(byte[] bytes)
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

	public static byte[] ConvertStringToBytes(string str)
	{
		if (str == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using StreamWriter streamWriter = new StreamWriter(memoryStream);
		streamWriter.Write(str);
		streamWriter.Close();
		memoryStream.Flush();
		memoryStream.Close();
		return memoryStream.ToArray();
	}
}
