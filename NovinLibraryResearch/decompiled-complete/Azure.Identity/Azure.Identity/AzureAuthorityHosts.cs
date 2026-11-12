using System;

namespace Azure.Identity;

public static class AzureAuthorityHosts
{
	private const string AzurePublicCloudHostUrl = "https://login.microsoftonline.com/";

	private const string AzureChinaHostUrl = "https://login.chinacloudapi.cn/";

	private const string AzureGermanyHostUrl = "https://login.microsoftonline.de/";

	private const string AzureGovernmentHostUrl = "https://login.microsoftonline.us/";

	public static Uri AzurePublicCloud { get; } = new Uri("https://login.microsoftonline.com/");


	public static Uri AzureChina { get; } = new Uri("https://login.chinacloudapi.cn/");


	public static Uri AzureGermany { get; } = new Uri("https://login.microsoftonline.de/");


	public static Uri AzureGovernment { get; } = new Uri("https://login.microsoftonline.us/");


	internal static Uri GetDefault()
	{
		if (EnvironmentVariables.AuthorityHost != null)
		{
			return new Uri(EnvironmentVariables.AuthorityHost);
		}
		return AzurePublicCloud;
	}

	internal static string GetDefaultScope(Uri authorityHost)
	{
		return authorityHost.ToString() switch
		{
			"https://login.microsoftonline.com/" => "https://management.core.windows.net//.default", 
			"https://login.chinacloudapi.cn/" => "https://management.core.chinacloudapi.cn//.default", 
			"https://login.microsoftonline.de/" => "https://management.core.cloudapi.de//.default", 
			"https://login.microsoftonline.us/" => "https://management.core.usgovcloudapi.net//.default", 
			_ => null, 
		};
	}

	internal static Uri GetDeviceCodeRedirectUri(Uri authorityHost)
	{
		return new Uri(authorityHost, "/common/oauth2/nativeclient");
	}
}
