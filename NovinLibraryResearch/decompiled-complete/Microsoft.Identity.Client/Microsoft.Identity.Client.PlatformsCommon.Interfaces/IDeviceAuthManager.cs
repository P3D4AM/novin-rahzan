using System;
using System.Net.Http.Headers;

namespace Microsoft.Identity.Client.PlatformsCommon.Interfaces;

internal interface IDeviceAuthManager
{
	bool TryCreateDeviceAuthChallengeResponseAsync(HttpResponseHeaders headers, Uri endpointUri, out string responseHeader);
}
