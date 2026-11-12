using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.AspNetCore.WebSockets.Internal;

internal static class HandshakeHelpers
{
	public static readonly IEnumerable<string> NeededHeaders = new string[4] { "Upgrade", "Connection", "Sec-WebSocket-Key", "Sec-WebSocket-Version" };

	public static bool CheckSupportedWebSocketRequest(string method, IEnumerable<KeyValuePair<string, string>> headers)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (!string.Equals("GET", method, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		foreach (KeyValuePair<string, string> header in headers)
		{
			if (string.Equals("Connection", header.Key, StringComparison.OrdinalIgnoreCase))
			{
				if (string.Equals("Upgrade", header.Value, StringComparison.OrdinalIgnoreCase))
				{
					flag2 = true;
				}
			}
			else if (string.Equals("Upgrade", header.Key, StringComparison.OrdinalIgnoreCase))
			{
				if (string.Equals("websocket", header.Value, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
			}
			else if (string.Equals("Sec-WebSocket-Version", header.Key, StringComparison.OrdinalIgnoreCase))
			{
				if (string.Equals("13", header.Value, StringComparison.OrdinalIgnoreCase))
				{
					flag4 = true;
				}
			}
			else if (string.Equals("Sec-WebSocket-Key", header.Key, StringComparison.OrdinalIgnoreCase))
			{
				flag3 = IsRequestKeyValid(header.Value);
			}
		}
		return flag2 && flag && flag4 && flag3;
	}

	public static IEnumerable<KeyValuePair<string, string>> GenerateResponseHeaders(string key, string subProtocol)
	{
		yield return new KeyValuePair<string, string>("Connection", "Upgrade");
		yield return new KeyValuePair<string, string>("Upgrade", "websocket");
		yield return new KeyValuePair<string, string>("Sec-WebSocket-Accept", CreateResponseKey(key));
		if (!string.IsNullOrWhiteSpace(subProtocol))
		{
			yield return new KeyValuePair<string, string>("Sec-WebSocket-Protocol", subProtocol);
		}
	}

	public static bool IsRequestKeyValid(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		try
		{
			return Convert.FromBase64String(value).Length == 16;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static string CreateResponseKey(string requestKey)
	{
		if (requestKey == null)
		{
			throw new ArgumentNullException("requestKey");
		}
		using SHA1 sHA = SHA1.Create();
		string s = requestKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		return Convert.ToBase64String(sHA.ComputeHash(bytes));
	}
}
