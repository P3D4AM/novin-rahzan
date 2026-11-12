using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azure.Core;

internal class HttpMessageSanitizer
{
	private const string LogAllValue = "*";

	private readonly bool _logAllHeaders;

	private readonly bool _logFullQueries;

	private readonly string[] _allowedQueryParameters;

	private readonly string _redactedPlaceholder;

	private readonly HashSet<string> _allowedHeaders;

	public HttpMessageSanitizer(string[] allowedQueryParameters, string[] allowedHeaders, string redactedPlaceholder = "REDACTED")
	{
		_logAllHeaders = allowedHeaders.Contains("*");
		_logFullQueries = allowedQueryParameters.Contains("*");
		_allowedQueryParameters = allowedQueryParameters;
		_redactedPlaceholder = redactedPlaceholder;
		_allowedHeaders = new HashSet<string>(allowedHeaders, StringComparer.InvariantCultureIgnoreCase);
	}

	public string SanitizeHeader(string name, string value)
	{
		if (_logAllHeaders || _allowedHeaders.Contains(name))
		{
			return value;
		}
		return _redactedPlaceholder;
	}

	public string SanitizeUrl(string url)
	{
		if (_logFullQueries)
		{
			return url;
		}
		int num = url.IndexOf('?');
		if (num == -1)
		{
			return url;
		}
		StringBuilder stringBuilder = new StringBuilder(url.Length);
		stringBuilder.Append(url, 0, num);
		string text = url.Substring(num);
		int num2 = 1;
		stringBuilder.Append('?');
		do
		{
			int num3 = text.IndexOf('&', num2);
			int num4 = text.IndexOf('=', num2);
			bool flag = false;
			if ((num3 == -1 && num4 == -1) || (num3 != -1 && (num4 == -1 || num4 > num3)))
			{
				num4 = num3;
				flag = true;
			}
			if (num4 == -1)
			{
				num4 = text.Length;
			}
			num3 = ((num3 != -1) ? (num3 + 1) : text.Length);
			ReadOnlySpan<char> span = MemoryExtensions.AsSpan(text, num2, num4 - num2);
			bool flag2 = false;
			string[] allowedQueryParameters = _allowedQueryParameters;
			foreach (string text2 in allowedQueryParameters)
			{
				if (MemoryExtensions.Equals(span, MemoryExtensions.AsSpan(text2), StringComparison.OrdinalIgnoreCase))
				{
					flag2 = true;
					break;
				}
			}
			int num5 = num3 - num2;
			int count = num4 - num2;
			if (flag2)
			{
				stringBuilder.Append(text, num2, num5);
			}
			else if (flag)
			{
				stringBuilder.Append(text, num2, num5);
			}
			else
			{
				stringBuilder.Append(text, num2, count);
				stringBuilder.Append("=");
				stringBuilder.Append(_redactedPlaceholder);
				if (text[num3 - 1] == '&')
				{
					stringBuilder.Append("&");
				}
			}
			num2 += num5;
		}
		while (num2 < text.Length);
		return stringBuilder.ToString();
	}
}
