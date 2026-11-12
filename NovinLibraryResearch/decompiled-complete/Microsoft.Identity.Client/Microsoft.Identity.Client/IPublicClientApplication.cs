using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client;

public interface IPublicClientApplication : IClientApplicationBase
{
	bool IsSystemWebViewAvailable { get; }

	AcquireTokenInteractiveParameterBuilder AcquireTokenInteractive(IEnumerable<string> scopes);

	AcquireTokenWithDeviceCodeParameterBuilder AcquireTokenWithDeviceCode(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback);

	AcquireTokenByIntegratedWindowsAuthParameterBuilder AcquireTokenByIntegratedWindowsAuth(IEnumerable<string> scopes);

	AcquireTokenByUsernamePasswordParameterBuilder AcquireTokenByUsernamePassword(IEnumerable<string> scopes, string username, SecureString password);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, UIParent parent);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, UIParent parent);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, UIParent parent);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters, UIParent parent);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters, UIParent parent);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority, UIParent parent);

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority, UIParent parent);

	[Obsolete("Use AcquireTokenByUsernamePassword instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenByUsernamePasswordAsync(IEnumerable<string> scopes, string username, SecureString securePassword);

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback);

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, string extraQueryParameters, Func<DeviceCodeResult, Task> deviceCodeResultCallback);

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback, CancellationToken cancellationToken);

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, string extraQueryParameters, Func<DeviceCodeResult, Task> deviceCodeResultCallback, CancellationToken cancellationToken);

	[Obsolete("Use AcquireTokenByIntegratedWindowsAuth instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenByIntegratedWindowsAuthAsync(IEnumerable<string> scopes);

	[Obsolete("Use AcquireTokenByIntegratedWindowsAuth instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> AcquireTokenByIntegratedWindowsAuthAsync(IEnumerable<string> scopes, string username);
}
