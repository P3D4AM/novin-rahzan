using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectConfigurationRetriever : IConfigurationRetriever<OpenIdConnectConfiguration>
{
	public static Task<OpenIdConnectConfiguration> GetAsync(string address, CancellationToken cancel)
	{
		return GetAsync(address, new HttpDocumentRetriever(), cancel);
	}

	public static Task<OpenIdConnectConfiguration> GetAsync(string address, HttpClient httpClient, CancellationToken cancel)
	{
		return GetAsync(address, new HttpDocumentRetriever(httpClient), cancel);
	}

	Task<OpenIdConnectConfiguration> IConfigurationRetriever<OpenIdConnectConfiguration>.GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
	{
		return GetAsync(address, retriever, cancel);
	}

	public static async Task<OpenIdConnectConfiguration> GetAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
	{
		if (string.IsNullOrWhiteSpace(address))
		{
			throw LogHelper.LogArgumentNullException("address");
		}
		if (retriever == null)
		{
			throw LogHelper.LogArgumentNullException("retriever");
		}
		string text = await retriever.GetDocumentAsync(address, cancel).ConfigureAwait(continueOnCapturedContext: false);
		LogHelper.LogVerbose("IDX21811: Deserializing the string: '{0}' obtained from metadata endpoint into openIdConnectConfiguration object.", text);
		OpenIdConnectConfiguration openIdConnectConfiguration = JsonConvert.DeserializeObject<OpenIdConnectConfiguration>(text);
		if (!string.IsNullOrEmpty(openIdConnectConfiguration.JwksUri))
		{
			LogHelper.LogVerbose("IDX21812: Retrieving json web keys from: '{0}'.", openIdConnectConfiguration.JwksUri);
			string value = await retriever.GetDocumentAsync(openIdConnectConfiguration.JwksUri, cancel).ConfigureAwait(continueOnCapturedContext: false);
			LogHelper.LogVerbose("IDX21813: Deserializing json web keys: '{0}'.", openIdConnectConfiguration.JwksUri);
			openIdConnectConfiguration.JsonWebKeySet = JsonConvert.DeserializeObject<JsonWebKeySet>(value);
			foreach (SecurityKey signingKey in openIdConnectConfiguration.JsonWebKeySet.GetSigningKeys())
			{
				openIdConnectConfiguration.SigningKeys.Add(signingKey);
			}
		}
		return openIdConnectConfiguration;
	}
}
