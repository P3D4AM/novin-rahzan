using System;
using System.ComponentModel;
using System.Net;

namespace Stimulsoft.Base;

[ToolboxItem(false)]
public class StiWebClientEx : WebClient
{
	public CookieContainer CookieContainer { get; set; }

	protected override WebRequest GetWebRequest(Uri address)
	{
		WebRequest webRequest = base.GetWebRequest(address);
		if (webRequest is HttpWebRequest httpWebRequest)
		{
			httpWebRequest.UserAgent = "Foo";
			httpWebRequest.Accept = "*/*";
			if (CookieContainer != null)
			{
				httpWebRequest.CookieContainer = CookieContainer;
			}
		}
		return webRequest;
	}

	public StiWebClientEx(CookieContainer container = null)
	{
		CookieContainer = container;
		base.Encoding = StiBaseOptions.WebClientEncoding;
		if (!StiBaseOptions.WebClientCheckTlsProtocols)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		foreach (SecurityProtocolType value in Enum.GetValues(typeof(SecurityProtocolType)))
		{
			if (value.GetHashCode() == 768)
			{
				flag = true;
			}
			if (value.GetHashCode() == 3072)
			{
				flag2 = true;
			}
		}
		if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls11) && flag)
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11;
		}
		if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) && flag2)
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
		}
	}
}
