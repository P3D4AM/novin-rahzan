using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Identity.Client.Utils;

internal static class UriBuilderExtensions
{
	private const int DefaultHttpsPort = 443;

	public static void AppendQueryParameters(this UriBuilder builder, string queryParams)
	{
		if (builder != null && !string.IsNullOrEmpty(queryParams))
		{
			if (builder.Query.Length > 1)
			{
				builder.Query = builder.Query.Substring(1) + "&" + queryParams;
			}
			else
			{
				builder.Query = queryParams;
			}
		}
	}

	public static void AppendQueryParameters(this UriBuilder builder, IDictionary<string, string> queryParams)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> queryParam in queryParams)
		{
			list.Add(queryParam.Key + "=" + queryParam.Value);
		}
		builder.AppendQueryParameters(string.Join("&", list));
	}

	public static string GetHttpsUriWithOptionalPort(string host, string tenant, string path, int port)
	{
		UriBuilder uriBuilder = new UriBuilder("https", host);
		uriBuilder.Path = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", tenant, path);
		if (port != 443)
		{
			uriBuilder.Port = port;
		}
		return uriBuilder.Uri.AbsoluteUri;
	}

	public static string GetHttpsUriWithOptionalPort(string uri, int port)
	{
		if (port != 443)
		{
			return new UriBuilder(uri)
			{
				Port = port
			}.Uri.AbsoluteUri;
		}
		return uri;
	}
}
