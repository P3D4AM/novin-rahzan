using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.Utils;

internal static class CoreHelpers
{
	internal static string ByteArrayToString(byte[] input)
	{
		if (input == null || input.Length == 0)
		{
			return null;
		}
		return Encoding.UTF8.GetString(input, 0, input.Length);
	}

	public static DateTime UnixTimestampToDateTime(double unixTimestamp)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp).ToUniversalTime();
	}

	public static DateTime UnixTimestampStringToDateTime(string str)
	{
		return UnixTimestampToDateTime(Convert.ToInt64(str, CultureInfo.InvariantCulture));
	}

	public static string DateTimeToUnixTimestamp(DateTimeOffset dateTimeOffset)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return ((long)dateTimeOffset.Subtract(dateTime).TotalSeconds).ToString(CultureInfo.InvariantCulture);
	}

	public static long CurrDateTimeInUnixTimestamp()
	{
		DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return (long)DateTime.UtcNow.Subtract(value).TotalSeconds;
	}

	public static long DateTimeToUnixTimestampMilliseconds(DateTimeOffset dateTimeOffset)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return (long)dateTimeOffset.Subtract(dateTime).TotalMilliseconds;
	}

	public static long GetDurationFromWindowsTimestamp(string windowsTimestampInFuture, ICoreLogger logger)
	{
		if (string.IsNullOrEmpty(windowsTimestampInFuture))
		{
			return 0L;
		}
		if (!ulong.TryParse(windowsTimestampInFuture, out var result) || result <= 11644473600L || result == ulong.MaxValue)
		{
			logger.Warning("Invalid Universal time " + windowsTimestampInFuture);
			return 0L;
		}
		return (long)(result - 11644473600L) - CurrDateTimeInUnixTimestamp();
	}

	public static long GetDurationFromNowInSeconds(string unixTimestampInFuture)
	{
		if (string.IsNullOrEmpty(unixTimestampInFuture))
		{
			return 0L;
		}
		return long.Parse(unixTimestampInFuture, CultureInfo.InvariantCulture) - CurrDateTimeInUnixTimestamp();
	}

	public static string CreateString(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
	}

	public static string UrlEncode(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return message;
		}
		message = Uri.EscapeDataString(message);
		message = message.Replace("%20", "+");
		return message;
	}

	public static string UrlDecode(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return message;
		}
		message = message.Replace("+", "%20");
		message = Uri.UnescapeDataString(message);
		return message;
	}

	public static void AddKeyValueString(StringBuilder messageBuilder, string key, string value)
	{
		AddKeyValueString(messageBuilder, key, value.ToCharArray());
	}

	public static string ToQueryParameter(this IDictionary<string, string> input)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (input.Count > 0)
		{
			foreach (string key in input.Keys)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}={1}&", key, UrlEncode(input[key]));
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
		}
		return stringBuilder.ToString();
	}

	public static Dictionary<string, string> ParseKeyValueList(string input, char delimiter, bool urlDecode, bool lowercaseKeys, RequestContext requestContext)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (string item in SplitWithQuotes(input, delimiter))
		{
			List<string> list = SplitWithQuotes(item, '=');
			if (list.Count == 2 && !string.IsNullOrWhiteSpace(list[0]) && !string.IsNullOrWhiteSpace(list[1]))
			{
				string text = list[0];
				string text2 = list[1];
				if (urlDecode)
				{
					text = UrlDecode(text);
					text2 = UrlDecode(text2);
				}
				if (lowercaseKeys)
				{
					text = text.Trim().ToLowerInvariant();
				}
				text2 = text2.Trim().Trim(new char[1] { '"' }).Trim();
				if (dictionary.ContainsKey(text))
				{
					requestContext?.Logger.Warning(string.Format(CultureInfo.InvariantCulture, "Key/value pair list contains redundant key '{0}'.", text));
				}
				dictionary[text] = text2;
			}
		}
		return dictionary;
	}

	public static Dictionary<string, string> ParseKeyValueList(string input, char delimiter, bool urlDecode, RequestContext requestContext)
	{
		return ParseKeyValueList(input, delimiter, urlDecode, lowercaseKeys: true, requestContext);
	}

	internal static List<string> SplitWithQuotes(string input, char delimiter)
	{
		List<string> list = new List<string>();
		if (string.IsNullOrWhiteSpace(input))
		{
			return list;
		}
		int num = 0;
		bool flag = false;
		string text;
		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] == delimiter && !flag)
			{
				text = input.Substring(num, i - num);
				if (!string.IsNullOrWhiteSpace(text.Trim()))
				{
					list.Add(text);
				}
				num = i + 1;
			}
			else if (input[i] == '"')
			{
				flag = !flag;
			}
		}
		text = input.Substring(num);
		if (!string.IsNullOrWhiteSpace(text.Trim()))
		{
			list.Add(text);
		}
		return list;
	}

	private static void AddKeyValueString(StringBuilder messageBuilder, string key, char[] value)
	{
		string arg = ((messageBuilder.Length == 0) ? string.Empty : "&");
		messageBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}=", arg, key);
		messageBuilder.Append(value);
	}
}
