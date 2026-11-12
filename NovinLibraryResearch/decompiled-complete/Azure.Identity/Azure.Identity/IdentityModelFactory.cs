using System;
using System.Collections.Generic;

namespace Azure.Identity;

public static class IdentityModelFactory
{
	internal static AuthenticationRecord AuthenticationRecord(string username, string authority, string homeAccountId, string tenantId, string clientId)
	{
		return new AuthenticationRecord(username, authority, homeAccountId, tenantId, clientId);
	}

	public static DeviceCodeInfo DeviceCodeInfo(string userCode, string deviceCode, Uri verificationUri, DateTimeOffset expiresOn, string message, string clientId, IReadOnlyCollection<string> scopes)
	{
		return new DeviceCodeInfo(userCode, deviceCode, verificationUri, expiresOn, message, clientId, scopes);
	}
}
