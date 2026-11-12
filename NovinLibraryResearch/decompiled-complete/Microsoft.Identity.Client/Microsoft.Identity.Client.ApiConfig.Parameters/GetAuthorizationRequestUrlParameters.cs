using System.Collections.Generic;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class GetAuthorizationRequestUrlParameters : IAcquireTokenParameters
{
	public string RedirectUri { get; set; }

	public IAccount Account { get; set; }

	public IEnumerable<string> ExtraScopesToConsent { get; set; }

	public string LoginHint { get; set; }

	public AcquireTokenInteractiveParameters ToInteractiveParameters()
	{
		return new AcquireTokenInteractiveParameters
		{
			Account = Account,
			ExtraScopesToConsent = ExtraScopesToConsent,
			LoginHint = LoginHint,
			Prompt = Prompt.SelectAccount,
			UseEmbeddedWebView = WebViewPreference.NotSpecified
		};
	}

	public void LogParameters(ICoreLogger logger)
	{
	}
}
