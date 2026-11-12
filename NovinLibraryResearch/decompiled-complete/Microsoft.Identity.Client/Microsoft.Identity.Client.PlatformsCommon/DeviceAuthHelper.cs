using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.PlatformsCommon;

internal class DeviceAuthHelper
{
	public static IDictionary<string, string> ParseChallengeData(HttpResponseHeaders responseHeaders)
	{
		IDictionary<string, string> dictionary = new Dictionary<string, string>();
		string text = responseHeaders.GetValues("WWW-Authenticate").SingleOrDefault();
		text = text.Substring("PKeyAuth".Length + 1);
		if (string.IsNullOrEmpty(text))
		{
			return dictionary;
		}
		foreach (string item in CoreHelpers.SplitWithQuotes(text, ','))
		{
			List<string> list = CoreHelpers.SplitWithQuotes(item, '=');
			if (list.Count == 2)
			{
				dictionary.Add(list[0].Trim(), list[1].Trim().Replace("\"", ""));
			}
		}
		return dictionary;
	}

	public static bool IsDeviceAuthChallenge(HttpResponseHeaders responseHeaders)
	{
		if (responseHeaders != null && responseHeaders != null && responseHeaders.Contains("WWW-Authenticate"))
		{
			return responseHeaders.GetValues("WWW-Authenticate").First().StartsWith("PKeyAuth", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public static string GetBypassChallengeResponse(HttpResponseHeaders responseHeaders)
	{
		IDictionary<string, string> dictionary = ParseChallengeData(responseHeaders);
		return string.Format(CultureInfo.InvariantCulture, "PKeyAuth Context=\"{0}\",Version=\"{1}\"", dictionary["Context"], dictionary["Version"]);
	}

	public static string GetBypassChallengeResponse(Dictionary<string, string> response)
	{
		return string.Format(CultureInfo.InvariantCulture, "PKeyAuth Context=\"{0}\",Version=\"{1}\"", response["Context"], response["Version"]);
	}

	public static bool CanOSPerformPKeyAuth()
	{
		if (Environment.OSVersion.Version.Major == 6)
		{
			return true;
		}
		return false;
	}
}
