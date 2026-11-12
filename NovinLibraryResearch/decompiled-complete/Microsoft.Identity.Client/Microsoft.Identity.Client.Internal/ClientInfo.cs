using System;
using System.Globalization;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client.Internal;

[JsonObject]
[Preserve(AllMembers = true)]
internal class ClientInfo
{
	[JsonProperty(PropertyName = "uid")]
	public string UniqueObjectIdentifier { get; set; }

	[JsonProperty(PropertyName = "utid")]
	public string UniqueTenantIdentifier { get; set; }

	public static ClientInfo CreateFromJson(string clientInfo)
	{
		if (string.IsNullOrEmpty(clientInfo))
		{
			throw new MsalClientException("json_parse_failed", "client info is null");
		}
		try
		{
			return JsonHelper.DeserializeFromJson<ClientInfo>(Base64UrlHelpers.DecodeToBytes(clientInfo));
		}
		catch (Exception innerException)
		{
			throw new MsalClientException("json_parse_failed", "Failed to parse the returned client info.", innerException);
		}
	}

	public string ToAccountIdentifier()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", UniqueObjectIdentifier, UniqueTenantIdentifier);
	}
}
