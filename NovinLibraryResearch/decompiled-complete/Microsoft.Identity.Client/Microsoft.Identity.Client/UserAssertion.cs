using System;
using Microsoft.Identity.Client.PlatformsCommon.Factories;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client;

public sealed class UserAssertion
{
	public string Assertion { get; private set; }

	public string AssertionType { get; private set; }

	internal string AssertionHash { get; set; }

	public UserAssertion(string jwtBearerToken)
		: this(jwtBearerToken, "urn:ietf:params:oauth:grant-type:jwt-bearer")
	{
	}

	public UserAssertion(string assertion, string assertionType)
	{
		if (string.IsNullOrWhiteSpace(assertion))
		{
			throw new ArgumentNullException("assertion");
		}
		if (string.IsNullOrWhiteSpace(assertionType))
		{
			throw new ArgumentNullException("assertionType");
		}
		ICryptographyManager cryptographyManager = PlatformProxyFactory.CreatePlatformProxy(null).CryptographyManager;
		AssertionType = assertionType;
		Assertion = assertion;
		AssertionHash = cryptographyManager.CreateBase64UrlEncodedSha256Hash(Assertion);
	}
}
