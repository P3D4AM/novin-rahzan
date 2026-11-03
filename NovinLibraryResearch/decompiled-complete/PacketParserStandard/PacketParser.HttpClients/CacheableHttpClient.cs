using System;
using System.Net.Http;

namespace PacketParser.HttpClients;

public static class CacheableHttpClient
{
	public static Func<HttpClient> Factory { get; set; }

	public static HttpClient Default
	{
		get
		{
			if (Factory != null)
			{
				return Factory();
			}
			return null;
		}
	}
}
