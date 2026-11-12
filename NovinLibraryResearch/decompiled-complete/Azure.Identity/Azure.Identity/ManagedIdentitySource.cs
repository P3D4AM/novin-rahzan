using System;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.Identity;

internal abstract class ManagedIdentitySource
{
	internal const string AuthenticationResponseInvalidFormatError = "Invalid response, the authentication response was not in the expected format.";

	protected internal CredentialPipeline Pipeline { get; }

	protected internal string ClientId { get; }

	protected ManagedIdentitySource(CredentialPipeline pipeline)
	{
		Pipeline = pipeline;
	}

	public virtual async ValueTask<AccessToken> AuthenticateAsync(bool async, TokenRequestContext context, CancellationToken cancellationToken)
	{
		using Request request = CreateRequest(context.Scopes);
		Response response = ((!async) ? Pipeline.HttpPipeline.SendRequest(request, cancellationToken) : (await Pipeline.HttpPipeline.SendRequestAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
		Response response2 = response;
		return await HandleResponseAsync(async, context, response2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected virtual async ValueTask<AccessToken> HandleResponseAsync(bool async, TokenRequestContext context, Response response, CancellationToken cancellationToken)
	{
		if (response.Status == 200)
		{
			JsonDocument jsonDocument = ((!async) ? JsonDocument.Parse(response.ContentStream) : (await JsonDocument.ParseAsync(response.ContentStream, default(JsonDocumentOptions), cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
			using JsonDocument jsonDocument2 = jsonDocument;
			JsonElement root = jsonDocument2.RootElement;
			return GetTokenFromResponse(in root);
		}
		RequestFailedException ex = ((!async) ? Pipeline.Diagnostics.CreateRequestFailedException(response) : (await Pipeline.Diagnostics.CreateRequestFailedExceptionAsync(response).ConfigureAwait(continueOnCapturedContext: false)));
		throw ex;
	}

	protected abstract Request CreateRequest(string[] scopes);

	private static AccessToken GetTokenFromResponse(in JsonElement root)
	{
		string text = null;
		DateTimeOffset? dateTimeOffset = null;
		foreach (JsonProperty item in root.EnumerateObject())
		{
			switch (item.Name)
			{
			case "access_token":
				text = item.Value.GetString();
				break;
			case "expires_on":
				dateTimeOffset = TryParseExpiresOn(item.Value);
				break;
			}
		}
		if (text == null || !dateTimeOffset.HasValue)
		{
			throw new AuthenticationFailedException("Invalid response, the authentication response was not in the expected format.");
		}
		return new AccessToken(text, dateTimeOffset.Value);
	}

	private static DateTimeOffset? TryParseExpiresOn(JsonElement jsonExpiresOn)
	{
		if ((jsonExpiresOn.ValueKind == JsonValueKind.Number && jsonExpiresOn.TryGetInt64(out var value)) || (jsonExpiresOn.ValueKind == JsonValueKind.String && long.TryParse(jsonExpiresOn.GetString(), out value)))
		{
			return DateTimeOffset.FromUnixTimeSeconds(value);
		}
		if (jsonExpiresOn.ValueKind == JsonValueKind.String && DateTimeOffset.TryParse(jsonExpiresOn.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
		{
			return result;
		}
		return null;
	}
}
