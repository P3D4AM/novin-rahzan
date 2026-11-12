using System;

namespace Microsoft.Identity.Client;

public interface ITokenCacheSerializer
{
	byte[] SerializeMsalV3();

	void DeserializeMsalV3(byte[] msalV3State, bool shouldClearExistingCache = false);

	byte[] SerializeAdalV3();

	void DeserializeAdalV3(byte[] adalV3State);

	[Obsolete("Support for the MSAL v2 token cache format will be dropped in the next major version", false)]
	byte[] SerializeMsalV2();

	[Obsolete("Support for the MSAL v2 token cache format will be dropped in the next major version", false)]
	void DeserializeMsalV2(byte[] msalV2State);
}
