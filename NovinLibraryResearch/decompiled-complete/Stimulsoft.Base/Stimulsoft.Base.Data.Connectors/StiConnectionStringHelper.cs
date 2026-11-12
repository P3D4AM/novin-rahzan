using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Base.Data.Connectors;

internal static class StiConnectionStringHelper
{
	internal static string GetConnectionStringKey(string connectionString, string key, char[] separator = null)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			return null;
		}
		separator = separator ?? new char[2] { ';', ',' };
		string[] source = connectionString.Split(separator);
		string text = source.FirstOrDefault((string s) => s.ToLowerInvariant().StartsWith(key.ToLowerInvariant()));
		if (text == null)
		{
			return null;
		}
		int num = text.IndexOf('=');
		string[] array = text.Split(new char[1] { '=' });
		if (array.Length < 2)
		{
			return null;
		}
		string text2 = text.Substring(num + 1, text.Length - num - 1);
		if (text2.StartsWith("\"") && text2.EndsWith("\""))
		{
			text2 = text2.Substring(0, text2.Length - 2);
		}
		return text2;
	}

	internal static string GetConnectionStringKey(string connectionString)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			return null;
		}
		string[] source = connectionString.Split(';', ',');
		return source.FirstOrDefault((string s) => !s.Contains("="));
	}

	internal static string SetConnectionStringKey(string connectionString, string key, string value)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			return key + "=" + value;
		}
		IEnumerable<string> values = from s in connectionString.Split(';', ',')
			where !s.ToLowerInvariant().StartsWith(key.ToLowerInvariant())
			select s;
		return string.Join(";", values) + ";" + key + "=" + value;
	}

	internal static string RemoveConnectionStringKey(string connectionString, string key)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			return connectionString;
		}
		IEnumerable<string> values = from s in connectionString.Split(';', ',')
			where !s.ToLowerInvariant().StartsWith(key.ToLowerInvariant())
			select s;
		return string.Join(";", values);
	}
}
