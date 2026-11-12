using System.Collections.Generic;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.OAuth2.Throttling;

internal interface IThrottlingProvider
{
	void TryThrottle(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams);

	void RecordException(AuthenticationRequestParameters requestParams, IReadOnlyDictionary<string, string> bodyParams, MsalServiceException ex);

	void ResetCache();
}
