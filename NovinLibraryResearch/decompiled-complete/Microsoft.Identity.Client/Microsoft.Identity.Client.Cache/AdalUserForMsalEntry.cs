using System;

namespace Microsoft.Identity.Client.Cache;

internal class AdalUserForMsalEntry
{
	public string ClientId { get; }

	public string Authority { get; }

	public string ClientInfo { get; }

	public AdalUserInfo UserInfo { get; }

	public AdalUserForMsalEntry(string clientId, string authority, string clientInfo, AdalUserInfo userInfo)
	{
		ClientId = clientId ?? throw new ArgumentNullException("clientId");
		Authority = authority;
		ClientInfo = clientInfo;
		UserInfo = userInfo ?? throw new ArgumentNullException("userInfo");
	}
}
