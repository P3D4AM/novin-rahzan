using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;
using Microsoft.Identity.Client.PlatformsCommon.Factories;

namespace Microsoft.Identity.Client;

public sealed class PublicClientApplication : ClientApplicationBase, IPublicClientApplication, IClientApplicationBase, IByRefreshToken
{
	public bool IsSystemWebViewAvailable => base.ServiceBundle.PlatformProxy.IsSystemWebViewAvailable;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use overload with SecureString instead (See https://aka.ms/msal-net-3-breaking-changes and https://aka.ms/msal-net-up)", true)]
	public async Task<AuthenticationResult> AcquireTokenByUsernamePasswordAsync(IEnumerable<string> scopes, string username, string password)
	{
		throw new NotImplementedException();
	}

	[Obsolete("Use PublicClientApplicationBuilder instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public PublicClientApplication(string clientId)
		: this(clientId, "https://login.microsoftonline.com/common/")
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use PublicClientApplicationBuilder instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public PublicClientApplication(string clientId, string authority)
		: base(PublicClientApplicationBuilder.Create(clientId).WithRedirectUri(PlatformProxyFactory.CreatePlatformProxy(null).GetDefaultRedirectUri(clientId)).WithAuthority(new Uri(authority))
			.BuildConfiguration())
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes")]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, string loginHint, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenInteractive instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scopes, IAccount account, Prompt prompt, string extraQueryParameters, IEnumerable<string> extraScopesToConsent, string authority, UIParent parent)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenByUsernamePassword instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenByUsernamePasswordAsync(IEnumerable<string> scopes, string username, SecureString securePassword)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, string extraQueryParameters, Func<DeviceCodeResult, Task> deviceCodeResultCallback)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback, CancellationToken cancellationToken)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenWithDeviceCode instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenWithDeviceCodeAsync(IEnumerable<string> scopes, string extraQueryParameters, Func<DeviceCodeResult, Task> deviceCodeResultCallback, CancellationToken cancellationToken)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenByRefreshToken instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	Task<AuthenticationResult> IByRefreshToken.AcquireTokenByRefreshTokenAsync(IEnumerable<string> scopes, string refreshToken)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenByIntegratedWindowsAuth instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenByIntegratedWindowsAuthAsync(IEnumerable<string> scopes)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use AcquireTokenByIntegratedWindowsAuth instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public Task<AuthenticationResult> AcquireTokenByIntegratedWindowsAuthAsync(IEnumerable<string> scopes, string username)
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	[Obsolete("Use PublicClientApplicationBuilder instead. See https://aka.ms/msal-net-3-breaking-changes", true)]
	public PublicClientApplication(string clientId, string authority, TokenCache userTokenCache)
		: this(PublicClientApplicationBuilder.Create(clientId).WithAuthority(new Uri(authority)).BuildConfiguration())
	{
		throw MigrationHelper.CreateMsalNet3BreakingChangesException();
	}

	internal PublicClientApplication(ApplicationConfiguration configuration)
		: base(configuration)
	{
	}

	[CLSCompliant(false)]
	public AcquireTokenInteractiveParameterBuilder AcquireTokenInteractive(IEnumerable<string> scopes)
	{
		return AcquireTokenInteractiveParameterBuilder.Create(ClientExecutorFactory.CreatePublicClientExecutor(this), scopes).WithParentActivityOrWindowFunc(base.ServiceBundle.Config.ParentActivityOrWindowFunc);
	}

	public AcquireTokenWithDeviceCodeParameterBuilder AcquireTokenWithDeviceCode(IEnumerable<string> scopes, Func<DeviceCodeResult, Task> deviceCodeResultCallback)
	{
		return AcquireTokenWithDeviceCodeParameterBuilder.Create(ClientExecutorFactory.CreatePublicClientExecutor(this), scopes, deviceCodeResultCallback);
	}

	AcquireTokenByRefreshTokenParameterBuilder IByRefreshToken.AcquireTokenByRefreshToken(IEnumerable<string> scopes, string refreshToken)
	{
		return AcquireTokenByRefreshTokenParameterBuilder.Create(ClientExecutorFactory.CreateClientApplicationBaseExecutor(this), scopes, refreshToken);
	}

	public AcquireTokenByIntegratedWindowsAuthParameterBuilder AcquireTokenByIntegratedWindowsAuth(IEnumerable<string> scopes)
	{
		return AcquireTokenByIntegratedWindowsAuthParameterBuilder.Create(ClientExecutorFactory.CreatePublicClientExecutor(this), scopes);
	}

	public AcquireTokenByUsernamePasswordParameterBuilder AcquireTokenByUsernamePassword(IEnumerable<string> scopes, string username, SecureString password)
	{
		return AcquireTokenByUsernamePasswordParameterBuilder.Create(ClientExecutorFactory.CreatePublicClientExecutor(this), scopes, username, password);
	}
}
