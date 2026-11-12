using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Internal;

internal sealed class ClientCredentialWrapper
{
	public static int MinKeySizeInBits { get; } = 2048;


	internal string Thumbprint => Base64UrlHelpers.Encode(Certificate.GetCertHash());

	internal X509Certificate2 Certificate { get; private set; }

	internal string CachedAssertion { get; set; }

	internal long ValidTo { get; set; }

	internal bool ContainsX5C { get; set; }

	internal string Audience { get; set; }

	internal string Secret { get; private set; }

	internal string SignedAssertion { get; private set; }

	internal bool AppendDefaultClaims { get; private set; }

	internal ConfidentialClientAuthenticationType AuthenticationType { get; private set; }

	internal IDictionary<string, string> ClaimsToSign { get; private set; }

	public ClientCredentialWrapper(ApplicationConfiguration config)
	{
		ConfidentialClientApplication.GuardMobileFrameworks();
		ValidateCredentialParameters(config);
		switch (AuthenticationType)
		{
		case ConfidentialClientAuthenticationType.ClientCertificate:
			Certificate = config.ClientCredentialCertificate;
			break;
		case ConfidentialClientAuthenticationType.ClientCertificateWithClaims:
			Certificate = config.ClientCredentialCertificate;
			ClaimsToSign = config.ClaimsToSign;
			break;
		case ConfidentialClientAuthenticationType.ClientSecret:
			Secret = config.ClientSecret;
			break;
		case ConfidentialClientAuthenticationType.SignedClientAssertion:
			SignedAssertion = config.SignedClientAssertion;
			break;
		}
	}

	public static ClientCredentialWrapper CreateWithCertificate(X509Certificate2 certificate, IDictionary<string, string> claimsToSign = null)
	{
		return new ClientCredentialWrapper(certificate, claimsToSign);
	}

	public static ClientCredentialWrapper CreateWithSecret(string secret)
	{
		return new ClientCredentialWrapper(secret, ConfidentialClientAuthenticationType.ClientSecret)
		{
			AuthenticationType = ConfidentialClientAuthenticationType.ClientSecret
		};
	}

	public static ClientCredentialWrapper CreateWithSignedClientAssertion(string signedClientAssertion)
	{
		return new ClientCredentialWrapper(signedClientAssertion, ConfidentialClientAuthenticationType.SignedClientAssertion)
		{
			AuthenticationType = ConfidentialClientAuthenticationType.SignedClientAssertion
		};
	}

	private ClientCredentialWrapper(X509Certificate2 certificate, IDictionary<string, string> claimsToSign = null)
	{
		ConfidentialClientApplication.GuardMobileFrameworks();
		Certificate = certificate;
		if (claimsToSign != null && claimsToSign.Any())
		{
			ClaimsToSign = claimsToSign;
			AuthenticationType = ConfidentialClientAuthenticationType.ClientCertificateWithClaims;
		}
		else
		{
			AuthenticationType = ConfidentialClientAuthenticationType.ClientCertificate;
		}
	}

	private ClientCredentialWrapper(string secretOrAssertion, ConfidentialClientAuthenticationType authType)
	{
		ConfidentialClientApplication.GuardMobileFrameworks();
		if (authType == ConfidentialClientAuthenticationType.SignedClientAssertion)
		{
			SignedAssertion = secretOrAssertion;
		}
		else
		{
			Secret = secretOrAssertion;
		}
	}

	private void CheckCertificateKeySize(X509Certificate2 cert)
	{
		if (cert.PublicKey.Key.KeySize < MinKeySizeInBits)
		{
			throw new ArgumentOutOfRangeException("cert", string.Format(CultureInfo.InvariantCulture, "The certificate used must have a key size of at least {0} bits", MinKeySizeInBits));
		}
	}

	private void ValidateCredentialParameters(ApplicationConfiguration config)
	{
		if (config.ConfidentialClientCredentialCount > 1)
		{
			throw new MsalClientException("Client_Credential_Authentication_Types_Are_Mutually_Exclusive", "ClientSecret, Certificate and ClientAssertion are mutually exclusive properties. Only specify one. See https://aka.ms/msal-net-client-credentials");
		}
		if (!string.IsNullOrWhiteSpace(config.ClientSecret))
		{
			AuthenticationType = ConfidentialClientAuthenticationType.ClientSecret;
		}
		if (config.ClientCredentialCertificate != null)
		{
			if (config.ClaimsToSign != null && config.ClaimsToSign.Any())
			{
				AuthenticationType = ConfidentialClientAuthenticationType.ClientCertificateWithClaims;
				AppendDefaultClaims = config.MergeWithDefaultClaims;
			}
			else
			{
				AuthenticationType = ConfidentialClientAuthenticationType.ClientCertificate;
			}
		}
		if (!string.IsNullOrWhiteSpace(config.SignedClientAssertion))
		{
			AuthenticationType = ConfidentialClientAuthenticationType.SignedClientAssertion;
		}
		if (AuthenticationType == ConfidentialClientAuthenticationType.None)
		{
			throw new MsalClientException("Client_Credentials_Required_In_Confidential_Client_Application", "One client credential type required either: ClientSecret, Certificate OR ClientAssertion must be defined when creating a Confidential Client. Only specify one. See https://aka.ms/msal-net-client-credentials");
		}
	}

	internal byte[] Sign(ICryptographyManager cryptographyManager, string message)
	{
		return cryptographyManager.SignWithCertificate(message, Certificate);
	}
}
