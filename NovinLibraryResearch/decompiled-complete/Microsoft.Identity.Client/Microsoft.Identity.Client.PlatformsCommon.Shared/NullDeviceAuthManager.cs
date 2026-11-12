using System;
using System.Net.Http.Headers;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.PlatformsCommon.Shared;

internal class NullDeviceAuthManager : IDeviceAuthManager
{
	public bool TryCreateDeviceAuthChallengeResponseAsync(HttpResponseHeaders headers, Uri endpointUri, out string responseHeader)
	{
		if (!DeviceAuthHelper.IsDeviceAuthChallenge(headers))
		{
			responseHeader = string.Empty;
			return false;
		}
		responseHeader = DeviceAuthHelper.GetBypassChallengeResponse(headers);
		return true;
	}
}
