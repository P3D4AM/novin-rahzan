using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Json;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client.Internal;

internal static class ClaimsHelper
{
	private const string AccessTokenClaim = "access_token";

	private const string XmsClientCapability = "xms_cc";

	internal static string GetMergedClaimsAndClientCapabilities(string claims, IEnumerable<string> clientCapabilities)
	{
		if (clientCapabilities != null && clientCapabilities.Any())
		{
			JObject capabilitiesJson = CreateClientCapabilitiesRequestJson(clientCapabilities);
			return MergeClaimsIntoCapabilityJson(claims, capabilitiesJson).ToString(Formatting.None);
		}
		return claims;
	}

	private static JObject MergeClaimsIntoCapabilityJson(string claims, JObject capabilitiesJson)
	{
		if (!string.IsNullOrEmpty(claims))
		{
			JObject content;
			try
			{
				content = JObject.Parse(claims);
			}
			catch (JsonReaderException innerException)
			{
				throw new MsalClientException("invalid_json_claims_format", MsalErrorMessage.InvalidJsonClaimsFormat(claims), innerException);
			}
			capabilitiesJson.Merge(content, new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Union
			});
		}
		return capabilitiesJson;
	}

	private static JObject CreateClientCapabilitiesRequestJson(IEnumerable<string> clientCapabilities)
	{
		return new JObject
		{
			new JProperty("access_token", new JObject(new JObject(new JProperty("xms_cc", new JObject(new JProperty("values", new JArray(clientCapabilities)))))))
		};
	}
}
