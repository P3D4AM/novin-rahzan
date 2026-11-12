using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

internal class AuthenticationRecord
{
	private const string UsernamePropertyName = "username";

	private const string AuthorityPropertyName = "authority";

	private const string HomeAccountIdPropertyName = "homeAccountId";

	private const string TenantIdPropertyName = "tenantId";

	private const string ClientIdPropertyName = "clientId";

	private static readonly JsonEncodedText s_usernamePropertyNameBytes = JsonEncodedText.Encode("username");

	private static readonly JsonEncodedText s_authorityPropertyNameBytes = JsonEncodedText.Encode("authority");

	private static readonly JsonEncodedText s_homeAccountIdPropertyNameBytes = JsonEncodedText.Encode("homeAccountId");

	private static readonly JsonEncodedText s_tenantIdPropertyNameBytes = JsonEncodedText.Encode("tenantId");

	private static readonly JsonEncodedText s_clientIdPropertyNameBytes = JsonEncodedText.Encode("clientId");

	public string Username { get; private set; }

	public string Authority { get; private set; }

	public string HomeAccountId => AccountId.Identifier;

	public string TenantId { get; private set; }

	public string ClientId { get; private set; }

	internal AccountId AccountId { get; private set; }

	internal AuthenticationRecord()
	{
	}

	internal AuthenticationRecord(AuthenticationResult authResult, string clientId)
	{
		Username = authResult.Account.Username;
		Authority = authResult.Account.Environment;
		AccountId = authResult.Account.HomeAccountId;
		TenantId = authResult.TenantId;
		ClientId = clientId;
	}

	internal AuthenticationRecord(string username, string authority, string homeAccountId, string tenantId, string clientId)
	{
		Username = username;
		Authority = authority;
		AccountId = BuildAccountIdFromString(homeAccountId);
		TenantId = tenantId;
		ClientId = clientId;
	}

	public void Serialize(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		SerializeAsync(stream, async: false, cancellationToken).EnsureCompleted();
	}

	public async Task SerializeAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		await SerializeAsync(stream, async: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static AuthenticationRecord Deserialize(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		return DeserializeAsync(stream, async: false, cancellationToken).EnsureCompleted();
	}

	public static async Task<AuthenticationRecord> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		return await DeserializeAsync(stream, async: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task SerializeAsync(Stream stream, bool async, CancellationToken cancellationToken)
	{
		using Utf8JsonWriter json = new Utf8JsonWriter(stream);
		json.WriteStartObject();
		json.WriteString(s_usernamePropertyNameBytes, Username);
		json.WriteString(s_authorityPropertyNameBytes, Authority);
		json.WriteString(s_homeAccountIdPropertyNameBytes, HomeAccountId);
		json.WriteString(s_tenantIdPropertyNameBytes, TenantId);
		json.WriteString(s_clientIdPropertyNameBytes, ClientId);
		json.WriteEndObject();
		if (async)
		{
			await json.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			json.Flush();
		}
	}

	private static async Task<AuthenticationRecord> DeserializeAsync(Stream stream, bool async, CancellationToken cancellationToken)
	{
		AuthenticationRecord authProfile = new AuthenticationRecord();
		JsonDocument jsonDocument = ((!async) ? JsonDocument.Parse(stream) : (await JsonDocument.ParseAsync(stream, default(JsonDocumentOptions), cancellationToken).ConfigureAwait(continueOnCapturedContext: false)));
		using JsonDocument jsonDocument2 = jsonDocument;
		foreach (JsonProperty item in jsonDocument2.RootElement.EnumerateObject())
		{
			switch (item.Name)
			{
			case "username":
				authProfile.Username = item.Value.GetString();
				break;
			case "authority":
				authProfile.Authority = item.Value.GetString();
				break;
			case "homeAccountId":
				authProfile.AccountId = BuildAccountIdFromString(item.Value.GetString());
				break;
			case "tenantId":
				authProfile.TenantId = item.Value.GetString();
				break;
			case "clientId":
				authProfile.ClientId = item.Value.GetString();
				break;
			}
		}
		return authProfile;
	}

	private static AccountId BuildAccountIdFromString(string homeAccountId)
	{
		string[] array = homeAccountId.Split(new char[1] { '.' });
		if (array.Length == 2)
		{
			return new AccountId(homeAccountId, array[0], array[1]);
		}
		return new AccountId(homeAccountId);
	}
}
