using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Internal;

[JsonObject]
[Preserve(AllMembers = true)]
internal class IdToken
{
	[JsonProperty(PropertyName = "iss")]
	public string Issuer { get; set; }

	[JsonProperty(PropertyName = "oid")]
	public string ObjectId { get; set; }

	[JsonProperty(PropertyName = "sub")]
	public string Subject { get; set; }

	[JsonProperty(PropertyName = "tid")]
	public string TenantId { get; set; }

	[JsonProperty(PropertyName = "ver")]
	public string Version { get; set; }

	[JsonProperty(PropertyName = "preferred_username")]
	public string PreferredUsername { get; set; }

	[JsonProperty(PropertyName = "name")]
	public string Name { get; set; }

	[JsonProperty(PropertyName = "home_oid")]
	public string HomeObjectId { get; set; }

	[JsonProperty(PropertyName = "upn")]
	public string Upn { get; set; }

	[JsonProperty(PropertyName = "given_name")]
	public string GivenName { get; set; }

	[JsonProperty(PropertyName = "family_name")]
	public string FamilyName { get; set; }

	public static IdToken Parse(string idToken)
	{
		if (string.IsNullOrEmpty(idToken))
		{
			return null;
		}
		IdToken idToken2 = null;
		string[] array = idToken.Split(new char[1] { '.' });
		if (array.Length < 2)
		{
			throw new MsalClientException("invalid_jwt", "ID Token must contain at least 2 parts.");
		}
		try
		{
			return JsonHelper.DeserializeFromJson<IdToken>(Base64UrlHelpers.DecodeToBytes(array[1]));
		}
		catch (JsonException innerException)
		{
			throw new MsalClientException("json_parse_failed", "Failed to parse the returned id token.", innerException);
		}
	}

	public string GetUniqueId()
	{
		return ObjectId ?? Subject;
	}
}
