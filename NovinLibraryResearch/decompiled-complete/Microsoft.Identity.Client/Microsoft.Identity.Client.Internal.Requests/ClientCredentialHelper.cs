using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.Internal.Requests;

internal static class ClientCredentialHelper
{
	public static bool ValidateClientAssertion(ClientCredentialWrapper clientCredential, string audience, bool sendX5C)
	{
		if (clientCredential == null)
		{
			throw new ArgumentNullException("clientCredential");
		}
		if (string.IsNullOrWhiteSpace(clientCredential.CachedAssertion))
		{
			return false;
		}
		bool num = clientCredential.ValidTo <= JsonWebToken.ConvertToTimeT(DateTime.UtcNow + TimeSpan.FromMinutes(5.0));
		bool flag = string.Equals(clientCredential.Audience, audience, StringComparison.OrdinalIgnoreCase) && clientCredential.ContainsX5C == sendX5C;
		return !num && flag;
	}

	public static Dictionary<string, string> CreateClientCredentialBodyParameters(ICoreLogger logger, ICryptographyManager cryptographyManager, ClientCredentialWrapper clientCredential, string clientId, AuthorityEndpoints endpoints, bool sendX5C)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (clientCredential != null)
		{
			if (clientCredential.AuthenticationType == ConfidentialClientAuthenticationType.ClientSecret)
			{
				dictionary["client_secret"] = clientCredential.Secret;
			}
			else
			{
				if ((clientCredential.CachedAssertion == null || clientCredential.ValidTo != 0L) && clientCredential.AuthenticationType != ConfidentialClientAuthenticationType.SignedClientAssertion)
				{
					if (!ValidateClientAssertion(clientCredential, endpoints.SelfSignedJwtAudience, sendX5C))
					{
						logger.Info("Client Assertion does not exist or near expiry. ");
						JsonWebToken jsonWebToken = ((clientCredential.AuthenticationType != ConfidentialClientAuthenticationType.ClientCertificateWithClaims) ? new JsonWebToken(cryptographyManager, clientId, endpoints.SelfSignedJwtAudience) : new JsonWebToken(cryptographyManager, clientId, endpoints.SelfSignedJwtAudience, clientCredential.ClaimsToSign, clientCredential.AppendDefaultClaims));
						clientCredential.CachedAssertion = jsonWebToken.Sign(clientCredential, sendX5C);
						clientCredential.ValidTo = jsonWebToken.ValidTo;
						clientCredential.ContainsX5C = sendX5C;
						clientCredential.Audience = endpoints.SelfSignedJwtAudience;
					}
					else
					{
						logger.Info("Reusing the unexpired Client Assertion...");
					}
				}
				dictionary["client_assertion_type"] = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
				if (clientCredential.AuthenticationType == ConfidentialClientAuthenticationType.SignedClientAssertion)
				{
					dictionary["client_assertion"] = clientCredential.SignedAssertion;
				}
				else
				{
					dictionary["client_assertion"] = clientCredential.CachedAssertion;
				}
			}
		}
		return dictionary;
	}
}
