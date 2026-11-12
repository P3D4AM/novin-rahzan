namespace Microsoft.Identity.Client.Internal;

internal enum ConfidentialClientAuthenticationType
{
	None,
	ClientCertificate,
	ClientCertificateWithClaims,
	ClientSecret,
	SignedClientAssertion
}
