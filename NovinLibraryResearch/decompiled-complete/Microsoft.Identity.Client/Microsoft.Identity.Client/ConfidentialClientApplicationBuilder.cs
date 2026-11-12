using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client;

public class ConfidentialClientApplicationBuilder : AbstractApplicationBuilder<ConfidentialClientApplicationBuilder>
{
	internal ConfidentialClientApplicationBuilder(ApplicationConfiguration configuration)
		: base(configuration)
	{
	}

	public static ConfidentialClientApplicationBuilder CreateWithApplicationOptions(ConfidentialClientApplicationOptions options)
	{
		ConfidentialClientApplicationBuilder confidentialClientApplicationBuilder = new ConfidentialClientApplicationBuilder(new ApplicationConfiguration()).WithOptions(options);
		if (!string.IsNullOrWhiteSpace(options.ClientSecret))
		{
			confidentialClientApplicationBuilder = confidentialClientApplicationBuilder.WithClientSecret(options.ClientSecret);
		}
		return confidentialClientApplicationBuilder;
	}

	public static ConfidentialClientApplicationBuilder Create(string clientId)
	{
		return new ConfidentialClientApplicationBuilder(new ApplicationConfiguration()).WithClientId(clientId);
	}

	public ConfidentialClientApplicationBuilder WithCertificate(X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		if (!certificate.HasPrivateKey)
		{
			throw new MsalClientException("cert_without_private_key", MsalErrorMessage.CertMustHavePrivateKey("certificate"));
		}
		base.Config.ClientCredentialCertificate = certificate;
		base.Config.ConfidentialClientCredentialCount++;
		return this;
	}

	public ConfidentialClientApplicationBuilder WithClientClaims(X509Certificate2 certificate, IDictionary<string, string> claimsToSign, bool mergeWithDefaultClaims = true)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		if (claimsToSign == null || !claimsToSign.Any())
		{
			throw new ArgumentNullException("claimsToSign");
		}
		base.Config.ClientCredentialCertificate = certificate;
		base.Config.ClaimsToSign = claimsToSign;
		base.Config.MergeWithDefaultClaims = mergeWithDefaultClaims;
		base.Config.ConfidentialClientCredentialCount++;
		return this;
	}

	public ConfidentialClientApplicationBuilder WithClientSecret(string clientSecret)
	{
		if (string.IsNullOrWhiteSpace(clientSecret))
		{
			throw new ArgumentNullException("clientSecret");
		}
		base.Config.ClientSecret = clientSecret;
		base.Config.ConfidentialClientCredentialCount++;
		return this;
	}

	public ConfidentialClientApplicationBuilder WithClientAssertion(string signedClientAssertion)
	{
		if (string.IsNullOrWhiteSpace(signedClientAssertion))
		{
			throw new ArgumentNullException("signedClientAssertion");
		}
		base.Config.SignedClientAssertion = signedClientAssertion;
		base.Config.ConfidentialClientCredentialCount++;
		return this;
	}

	internal override void Validate()
	{
		base.Validate();
		base.Config.ClientCredential = new ClientCredentialWrapper(base.Config);
		if (string.IsNullOrWhiteSpace(base.Config.RedirectUri))
		{
			base.Config.RedirectUri = "https://replyUrlNotSet";
		}
		if (!Uri.TryCreate(base.Config.RedirectUri, UriKind.Absolute, out Uri _))
		{
			throw new InvalidOperationException(MsalErrorMessage.InvalidRedirectUriReceived(base.Config.RedirectUri));
		}
	}

	public IConfidentialClientApplication Build()
	{
		return BuildConcrete();
	}

	internal ConfidentialClientApplication BuildConcrete()
	{
		return new ConfidentialClientApplication(BuildConfiguration());
	}
}
