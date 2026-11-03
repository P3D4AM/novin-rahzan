using System;
using System.Net;
using System.Web.Services.Protocols;
using SmartAssembly.Attributes;

namespace NovinP.ActivationWebService;

[Serializable]
[DoNotPruneType]
public class ActivationWebService : SoapHttpClientProtocol
{
	protected override WebRequest GetWebRequest(Uri uri)
	{
		HttpWebRequest obj = (HttpWebRequest)((SoapHttpClientProtocol)this).GetWebRequest(uri);
		obj.KeepAlive = false;
		return obj;
	}
}
