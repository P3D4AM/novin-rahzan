using System.Collections;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Blockly.StiBlocks.Functions;

public static class StiBlocklyFunctionBlockKeyCache
{
	private static Hashtable cache;

	public static string CreateKey(StiFunction function)
	{
		string text = function.FunctionName + ".";
		if (function.ArgumentNames != null)
		{
			for (int i = 0; i < function.ArgumentNames.Length; i++)
			{
				text = text + function.ArgumentNames[i] + "." + function.ArgumentTypes[i].Name;
			}
		}
		return text;
	}

	public static StiFunction GetFunction(string key)
	{
		if (cache != null)
		{
			FillCache();
		}
		return cache[key] as StiFunction;
	}

	private static void FillCache()
	{
		cache = new Hashtable();
		StiFunction[] functions = StiFunctions.GetFunctions(isCompile: false);
		StiFunction[] array = functions;
		foreach (StiFunction stiFunction in array)
		{
			cache.Add(CreateKey(stiFunction), stiFunction);
		}
	}

	public static Hashtable GetBlockKeyTable()
	{
		if (cache == null)
		{
			FillCache();
		}
		return cache;
	}
}
