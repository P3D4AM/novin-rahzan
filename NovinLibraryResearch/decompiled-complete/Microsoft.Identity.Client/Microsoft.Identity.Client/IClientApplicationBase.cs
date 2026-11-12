using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client;

public interface IClientApplicationBase
{
	IAppConfig AppConfig { get; }

	ITokenCache UserTokenCache { get; }

	string Authority { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[Obsolete("Use GetAccountsAsync instead (See https://aka.ms/msal-net-2-released)", true)]
	IEnumerable<IUser> Users { get; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use WithComponent on AbstractApplicationBuilder<T> to configure this instead.  See https://aka.ms/msal-net-3-breaking-changes or https://aka.ms/msal-net-application-configuration", true)]
	string Component { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ExtraQueryParameters on each call instead.  See https://aka.ms/msal-net-3-breaking-changes or https://aka.ms/msal-net-application-configuration", true)]
	string SliceParameters { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Can be set on AbstractApplicationBuilder<T>.WithAuthority as needed.  See https://aka.ms/msal-net-3-breaking-changes or https://aka.ms/msal-net-application-configuration", true)]
	bool ValidateAuthority { get; }

	[Obsolete("Should be set using AbstractApplicationBuilder<T>.WithRedirectUri and can be viewed with ClientApplicationBase.AppConfig.RedirectUri. See https://aka.ms/msal-net-3-breaking-changes or https://aka.ms/msal-net-application-configuration", true)]
	string RedirectUri { get; set; }

	[Obsolete("Use AppConfig.ClientId instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	string ClientId { get; }

	Task<IEnumerable<IAccount>> GetAccountsAsync();

	Task<IAccount> GetAccountAsync(string identifier);

	Task<IEnumerable<IAccount>> GetAccountsAsync(string userFlow);

	AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, IAccount account);

	AcquireTokenSilentParameterBuilder AcquireTokenSilent(IEnumerable<string> scopes, string loginHint);

	Task RemoveAsync(IAccount account);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use GetAccountAsync instead and pass IAccount.HomeAccountId.Identifier (See https://aka.ms/msal-net-2-released)", true)]
	IUser GetUser(string identifier);

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use RemoveAccountAsync instead (See https://aka.ms/msal-net-2-released)", true)]
	void Remove(IUser user);

	[Obsolete("Use AcquireTokenSilent instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes, IAccount account);

	[Obsolete("Use AcquireTokenSilent instead.See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenSilentAsync(IEnumerable<string> scopes, IAccount account, string authority, bool forceRefresh);
}
