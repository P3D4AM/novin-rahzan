using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Identity.Client;

internal sealed class Account : IAccountInternal, IAccount
{
	private readonly IDictionary<string, string> _wamAccountIds;

	public string Username { get; }

	public string Environment { get; }

	public AccountId HomeAccountId { get; }

	IDictionary<string, string> IAccountInternal.WamAccountIds => _wamAccountIds;

	public Account(string homeAccountId, string username, string environment, IDictionary<string, string> wamAccountIds = null)
	{
		Username = username;
		Environment = environment;
		HomeAccountId = AccountId.ParseFromString(homeAccountId);
		_wamAccountIds = wamAccountIds;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.CurrentCulture, "Account username: {0} environment {1} home account id: {2}", Username, Environment, HomeAccountId);
	}
}
