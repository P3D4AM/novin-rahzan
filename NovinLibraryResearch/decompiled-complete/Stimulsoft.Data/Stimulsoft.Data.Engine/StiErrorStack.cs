using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public class StiErrorStack
{
	private static Dictionary<string, string> keyToError = new Dictionary<string, string>();

	public static void SetOk(string key)
	{
		lock (keyToError)
		{
			if (keyToError.ContainsKey(key))
			{
				keyToError.Remove(key);
			}
		}
	}

	public static void SetError(string key, string error)
	{
		lock (keyToError)
		{
			keyToError[key] = error;
		}
	}

	public static string GetError(string key)
	{
		lock (keyToError)
		{
			return keyToError.ContainsKey(key) ? keyToError[key] : null;
		}
	}

	public static bool IsFail(string key)
	{
		lock (keyToError)
		{
			return keyToError.ContainsKey(key);
		}
	}
}
