using Microsoft.Identity.Client;

namespace Azure.Identity;

internal class AuthenticationAccount : IAccount
{
	private AuthenticationRecord _profile;

	string IAccount.Username => _profile.Username;

	string IAccount.Environment => _profile.Authority;

	AccountId IAccount.HomeAccountId => _profile.AccountId;

	internal AuthenticationAccount(AuthenticationRecord profile)
	{
		_profile = profile;
	}

	public static explicit operator AuthenticationAccount(AuthenticationRecord profile)
	{
		return new AuthenticationAccount(profile);
	}

	public static explicit operator AuthenticationRecord(AuthenticationAccount account)
	{
		return account._profile;
	}
}
