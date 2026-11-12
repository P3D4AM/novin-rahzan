using System.Globalization;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.OAuth2;

internal class DeviceAuthJWTResponse
{
	private readonly DeviceAuthHeader header;

	private readonly DeviceAuthPayload payload;

	public DeviceAuthJWTResponse(string audience, string nonce, string base64EncodedCertificate)
	{
		header = new DeviceAuthHeader(base64EncodedCertificate);
		payload = new DeviceAuthPayload(audience, nonce);
	}

	public string GetResponseToSign()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", Base64UrlHelpers.Encode(JsonHelper.SerializeToJson(header).ToByteArray()), Base64UrlHelpers.Encode(JsonHelper.SerializeToJson(payload).ToByteArray()));
	}
}
