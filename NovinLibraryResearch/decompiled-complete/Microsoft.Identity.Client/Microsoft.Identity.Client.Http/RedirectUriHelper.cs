using System;
using System.Globalization;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.Http;

internal class RedirectUriHelper
{
	public static void Validate(Uri redirectUri, bool usesSystemBrowser = false)
	{
		if (redirectUri == null)
		{
			throw new MsalClientException("no_redirect_uri", "No redirectUri was configured. MSAL does not provide any defaults.");
		}
		if (!string.IsNullOrWhiteSpace(redirectUri.Fragment))
		{
			throw new ArgumentException("'redirectUri' must NOT include a fragment component", "redirectUri");
		}
		if (usesSystemBrowser && "urn:ietf:wg:oauth:2.0:oob".Equals(redirectUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
		{
			throw new MsalClientException("redirect_uri_validation_failed", string.Format(CultureInfo.InvariantCulture, "The OAuth2 redirect uri {0} should not be used with the system browser, because the operating system cannot go back to the app. Consider using the default redirect uri for this platform. See https://aka.ms/msal-client-apps for more details.", "urn:ietf:wg:oauth:2.0:oob"));
		}
	}

	public static void ValidateIosBrokerRedirectUri(Uri redirectUri, string bundleId, ICoreLogger logger)
	{
		string text = "msauth." + bundleId + "://auth";
		string originalString = redirectUri.OriginalString;
		if (string.Equals(text, originalString.TrimEnd(new char[1] { '/' }), StringComparison.Ordinal))
		{
			logger.Verbose("Valid MSAL style redirect Uri detected.");
			return;
		}
		if (redirectUri.Authority.Equals(bundleId, StringComparison.OrdinalIgnoreCase))
		{
			logger.Verbose("Valid ADAL style redirect Uri detected.");
			return;
		}
		throw new MsalClientException("cannot_invoke_broker", "The broker redirect URI is incorrect, it should be " + text + " or app_scheme ://" + bundleId + " - please visit https://aka.ms/msal-net-xamarin for details about redirect URIs.");
	}
}
