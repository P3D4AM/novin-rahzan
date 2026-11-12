using System.Collections;
using System.Collections.Generic;

namespace Serilog.Exceptions;

internal static class DictionaryExtensions
{
	public static Dictionary<string, object?> ToStringObjectDictionary(this IDictionary dictionary)
	{
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>(dictionary.Count);
		foreach (object key in dictionary.Keys)
		{
			if (key != null)
			{
				string text = key.ToString();
				object value = dictionary[key];
				if (text != null)
				{
					dictionary2.Add(text, value);
				}
			}
		}
		return dictionary2;
	}
}
