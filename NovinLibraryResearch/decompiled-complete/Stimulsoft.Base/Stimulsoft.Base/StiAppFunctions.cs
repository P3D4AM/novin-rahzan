using System.Collections;
using System.Collections.Generic;

namespace Stimulsoft.Base;

internal class StiAppFunctions
{
	internal static Hashtable FunctionsToCompile { get; set; }

	internal static Hashtable FunctionsToCompileLower { get; set; }

	internal static Hashtable Functions { get; set; }

	internal static Hashtable FunctionsLower { get; set; }

	public static IStiAppFunction[] GetFunctions(bool isCompile, bool isCaseSensitive)
	{
		List<IStiAppFunction> list = new List<IStiAppFunction>();
		Hashtable hashtable = (isCompile ? FunctionsToCompile : Functions);
		foreach (string key in hashtable.Keys)
		{
			IStiAppFunction[] functions = GetFunctions(key, isCompile, isCaseSensitive);
			list.AddRange(functions);
		}
		return list.ToArray();
	}

	public static IStiAppFunction[] GetFunctions(string functionName, bool isCompile, bool isCaseSensitive)
	{
		Hashtable hashtable = (isCompile ? FunctionsToCompile : Functions);
		Hashtable hashtable2 = (isCompile ? FunctionsToCompileLower : FunctionsLower);
		if (hashtable[functionName] is List<IStiAppFunction> list)
		{
			return list.ToArray();
		}
		return ((!isCaseSensitive) ? (hashtable2[functionName.ToLowerInvariant()] as List<IStiAppFunction>) : (hashtable[functionName] as List<IStiAppFunction>))?.ToArray();
	}

	static StiAppFunctions()
	{
		FunctionsToCompile = new Hashtable();
		FunctionsToCompileLower = new Hashtable();
		Functions = new Hashtable();
		FunctionsLower = new Hashtable();
	}
}
