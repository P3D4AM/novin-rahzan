using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Identity.Client.Instance;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client.Cache;

internal class AdalUsersForMsal
{
	private readonly IEnumerable<AdalUserForMsalEntry> _userEntries;

	public AdalUsersForMsal(IEnumerable<AdalUserForMsalEntry> userEntries)
	{
		_userEntries = userEntries ?? throw new ArgumentNullException("userEntries");
	}

	public IDictionary<string, AdalUserInfo> GetUsersWithClientInfo(IEnumerable<string> envAliases)
	{
		return _userEntries.Where((AdalUserForMsalEntry u) => !string.IsNullOrEmpty(u.Authority) && !string.IsNullOrEmpty(u.ClientInfo) && (envAliases?.ContainsOrdinalIgnoreCase(Authority.GetEnviroment(u.Authority)) ?? true)).ToLookup((AdalUserForMsalEntry u) => u.ClientInfo, (AdalUserForMsalEntry u) => u.UserInfo).ToDictionary((IGrouping<string, AdalUserInfo> group) => group.Key, (IGrouping<string, AdalUserInfo> group) => group.First());
	}

	public IEnumerable<AdalUserInfo> GetUsersWithoutClientInfo(IEnumerable<string> envAliases)
	{
		return from u in _userEntries
			where !string.IsNullOrEmpty(u.Authority) && string.IsNullOrEmpty(u.ClientInfo) && (envAliases?.ContainsOrdinalIgnoreCase(Authority.GetEnviroment(u.Authority)) ?? true)
			select u.UserInfo;
	}

	public ISet<string> GetAdalUserEnviroments()
	{
		return new HashSet<string>(from u in _userEntries
			where !string.IsNullOrEmpty(u.Authority)
			select Authority.GetEnviroment(u.Authority), StringComparer.OrdinalIgnoreCase);
	}
}
