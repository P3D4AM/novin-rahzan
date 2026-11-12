using System;
using System.IO;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public static class StiJsonChecker
{
	public static bool IsValidJsonFile(string file)
	{
		string strInput = File.ReadAllText(file);
		return IsValidJson(strInput);
	}

	public static bool IsValidJson(string strInput)
	{
		strInput = strInput.Trim();
		if ((!strInput.StartsWith("{") || !strInput.EndsWith("}")) && (!strInput.StartsWith("[") || !strInput.EndsWith("]")))
		{
			return false;
		}
		try
		{
			JToken.Parse(strInput);
			return true;
		}
		catch (JsonReaderException)
		{
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
