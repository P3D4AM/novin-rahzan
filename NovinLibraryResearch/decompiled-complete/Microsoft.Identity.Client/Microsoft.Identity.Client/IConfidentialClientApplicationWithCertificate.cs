using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client;

public interface IConfidentialClientApplicationWithCertificate
{
	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenForClientWithCertificateAsync(IEnumerable<string> scopes);

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenForClientWithCertificateAsync(IEnumerable<string> scopes, bool forceRefresh);

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenOnBehalfOfWithCertificateAsync(IEnumerable<string> scopes, UserAssertion userAssertion);

	[Obsolete("Use AcquireTokenForClient instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenOnBehalfOfWithCertificateAsync(IEnumerable<string> scopes, UserAssertion userAssertion, string authority);
}
