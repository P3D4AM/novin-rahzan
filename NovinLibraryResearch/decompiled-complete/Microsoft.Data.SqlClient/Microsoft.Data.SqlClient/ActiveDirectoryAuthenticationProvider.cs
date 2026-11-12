using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.Common;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensibility;

namespace Microsoft.Data.SqlClient;

public sealed class ActiveDirectoryAuthenticationProvider : SqlAuthenticationProvider
{
	private class CustomWebUi : ICustomWebUi
	{
		private readonly Func<Uri, Uri, CancellationToken, Task<Uri>> _acquireAuthorizationCodeAsyncCallback;

		internal CustomWebUi(Func<Uri, Uri, CancellationToken, Task<Uri>> acquireAuthorizationCodeAsyncCallback)
		{
			_acquireAuthorizationCodeAsyncCallback = acquireAuthorizationCodeAsyncCallback;
		}

		public Task<Uri> AcquireAuthorizationCodeAsync(Uri authorizationUri, Uri redirectUri, CancellationToken cancellationToken)
		{
			return _acquireAuthorizationCodeAsyncCallback(authorizationUri, redirectUri, cancellationToken);
		}
	}

	internal class PublicClientAppKey
	{
		public readonly string _authority;

		public readonly string _redirectUri;

		public readonly string _applicationClientId;

		public readonly Func<IWin32Window> _iWin32WindowFunc;

		public PublicClientAppKey(string authority, string redirectUri, string applicationClientId, Func<IWin32Window> iWin32WindowFunc)
		{
			_authority = authority;
			_redirectUri = redirectUri;
			_applicationClientId = applicationClientId;
			_iWin32WindowFunc = iWin32WindowFunc;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is PublicClientAppKey publicClientAppKey)
			{
				if (string.CompareOrdinal(_authority, publicClientAppKey._authority) == 0 && string.CompareOrdinal(_redirectUri, publicClientAppKey._redirectUri) == 0 && string.CompareOrdinal(_applicationClientId, publicClientAppKey._applicationClientId) == 0)
				{
					return publicClientAppKey._iWin32WindowFunc == _iWin32WindowFunc;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Tuple.Create(_authority, _redirectUri, _applicationClientId, _iWin32WindowFunc).GetHashCode();
		}
	}

	private static ConcurrentDictionary<PublicClientAppKey, IPublicClientApplication> s_pcaMap = new ConcurrentDictionary<PublicClientAppKey, IPublicClientApplication>();

	private static readonly string s_nativeClientRedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

	private static readonly string s_defaultScopeSuffix = "/.default";

	private readonly string _type = typeof(ActiveDirectoryAuthenticationProvider).Name;

	private readonly SqlClientLogger _logger = new SqlClientLogger();

	private Func<DeviceCodeResult, Task> _deviceCodeFlowCallback;

	private ICustomWebUi _customWebUI;

	private readonly string _applicationClientId = "2fd908ad-0664-4344-b9be-cd3e8b574c38";

	private Func<IWin32Window> _iWin32WindowFunc;

	public ActiveDirectoryAuthenticationProvider()
	{
		new ActiveDirectoryAuthenticationProvider(DefaultDeviceFlowCallback);
	}

	public ActiveDirectoryAuthenticationProvider(string applicationClientId)
	{
		new ActiveDirectoryAuthenticationProvider(DefaultDeviceFlowCallback, applicationClientId);
	}

	public ActiveDirectoryAuthenticationProvider(Func<DeviceCodeResult, Task> deviceCodeFlowCallbackMethod, string applicationClientId = null)
	{
		if (applicationClientId != null)
		{
			_applicationClientId = applicationClientId;
		}
		SetDeviceCodeFlowCallback(deviceCodeFlowCallbackMethod);
	}

	public static void ClearUserTokenCache()
	{
		if (!s_pcaMap.IsEmpty)
		{
			s_pcaMap.Clear();
		}
	}

	public void SetDeviceCodeFlowCallback(Func<DeviceCodeResult, Task> deviceCodeFlowCallbackMethod)
	{
		_deviceCodeFlowCallback = deviceCodeFlowCallbackMethod;
	}

	public void SetAcquireAuthorizationCodeAsyncCallback(Func<Uri, Uri, CancellationToken, Task<Uri>> acquireAuthorizationCodeAsyncCallback)
	{
		_customWebUI = new CustomWebUi(acquireAuthorizationCodeAsyncCallback);
	}

	public override bool IsSupported(SqlAuthenticationMethod authentication)
	{
		if (authentication != SqlAuthenticationMethod.ActiveDirectoryIntegrated && authentication != SqlAuthenticationMethod.ActiveDirectoryPassword && authentication != SqlAuthenticationMethod.ActiveDirectoryInteractive && authentication != SqlAuthenticationMethod.ActiveDirectoryServicePrincipal && authentication != SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow && authentication != SqlAuthenticationMethod.ActiveDirectoryManagedIdentity && authentication != SqlAuthenticationMethod.ActiveDirectoryMSI)
		{
			return authentication == SqlAuthenticationMethod.ActiveDirectoryDefault;
		}
		return true;
	}

	public override void BeforeLoad(SqlAuthenticationMethod authentication)
	{
		_logger.LogInfo(_type, "BeforeLoad", $"being loaded into SqlAuthProviders for {authentication}.");
	}

	public override void BeforeUnload(SqlAuthenticationMethod authentication)
	{
		_logger.LogInfo(_type, "BeforeUnload", $"being unloaded from SqlAuthProviders for {authentication}.");
	}

	public void SetIWin32WindowFunc(Func<IWin32Window> iWin32WindowFunc)
	{
		_iWin32WindowFunc = iWin32WindowFunc;
	}

	public override async Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters)
	{
		CancellationTokenSource cts = new CancellationTokenSource();
		cts.CancelAfter(parameters.ConnectionTimeout * 1000);
		string text = (parameters.Resource.EndsWith(s_defaultScopeSuffix) ? parameters.Resource : (parameters.Resource + s_defaultScopeSuffix));
		string[] scopes = new string[1] { text };
		int num = parameters.Authority.LastIndexOf('/');
		string text2 = parameters.Authority.Substring(num + 1);
		string uriString = parameters.Authority.Remove(num + 1);
		TokenRequestContext requestContext = new TokenRequestContext(scopes);
		string text3 = (string.IsNullOrWhiteSpace(parameters.UserId) ? null : parameters.UserId);
		if (parameters.AuthenticationMethod == SqlAuthenticationMethod.ActiveDirectoryDefault)
		{
			DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions
			{
				AuthorityHost = new Uri(uriString),
				ManagedIdentityClientId = text3,
				InteractiveBrowserTenantId = text2,
				SharedTokenCacheTenantId = text2,
				SharedTokenCacheUsername = text3,
				VisualStudioCodeTenantId = text2,
				VisualStudioTenantId = text2,
				ExcludeInteractiveBrowserCredential = true
			};
			AccessToken accessToken = await new DefaultAzureCredential(options).GetTokenAsync(requestContext, cts.Token);
			SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token for Default auth mode. Expiry Time: {0}", accessToken.ExpiresOn);
			return new SqlAuthenticationToken(accessToken.Token, accessToken.ExpiresOn);
		}
		TokenCredentialOptions options2 = new TokenCredentialOptions
		{
			AuthorityHost = new Uri(uriString)
		};
		if (parameters.AuthenticationMethod == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity || parameters.AuthenticationMethod == SqlAuthenticationMethod.ActiveDirectoryMSI)
		{
			AccessToken accessToken2 = await new ManagedIdentityCredential(text3, options2).GetTokenAsync(requestContext, cts.Token);
			SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token for Managed Identity auth mode. Expiry Time: {0}", accessToken2.ExpiresOn);
			return new SqlAuthenticationToken(accessToken2.Token, accessToken2.ExpiresOn);
		}
		if (parameters.AuthenticationMethod == SqlAuthenticationMethod.ActiveDirectoryServicePrincipal)
		{
			AccessToken accessToken3 = await new ClientSecretCredential(text2, parameters.UserId, parameters.Password, options2).GetTokenAsync(requestContext, cts.Token);
			SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token for Active Directory Service Principal auth mode. Expiry Time: {0}", accessToken3.ExpiresOn);
			return new SqlAuthenticationToken(accessToken3.Token, accessToken3.ExpiresOn);
		}
		string redirectUri = s_nativeClientRedirectUri;
		PublicClientAppKey publicClientAppKey = new PublicClientAppKey(parameters.Authority, redirectUri, _applicationClientId, _iWin32WindowFunc);
		IPublicClientApplication app = GetPublicClientAppInstance(publicClientAppKey);
		AuthenticationResult result;
		if (parameters.AuthenticationMethod == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
		{
			result = (string.IsNullOrEmpty(parameters.UserId) ? (await app.AcquireTokenByIntegratedWindowsAuth(scopes).WithCorrelationId(parameters.ConnectionId).ExecuteAsync(cts.Token)) : (await app.AcquireTokenByIntegratedWindowsAuth(scopes).WithCorrelationId(parameters.ConnectionId).WithUsername(parameters.UserId)
				.ExecuteAsync(cts.Token)));
			SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token for Active Directory Integrated auth mode. Expiry Time: {0}", result?.ExpiresOn);
		}
		else if (parameters.AuthenticationMethod == SqlAuthenticationMethod.ActiveDirectoryPassword)
		{
			SecureString secureString = new SecureString();
			string password = parameters.Password;
			foreach (char c in password)
			{
				secureString.AppendChar(c);
			}
			secureString.MakeReadOnly();
			result = await app.AcquireTokenByUsernamePassword(scopes, parameters.UserId, secureString).WithCorrelationId(parameters.ConnectionId).ExecuteAsync(cts.Token);
			SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token for Active Directory Password auth mode. Expiry Time: {0}", result?.ExpiresOn);
		}
		else
		{
			if (parameters.AuthenticationMethod != SqlAuthenticationMethod.ActiveDirectoryInteractive && parameters.AuthenticationMethod != SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow)
			{
				SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | {0} authentication mode not supported by ActiveDirectoryAuthenticationProvider class.", parameters.AuthenticationMethod);
				throw SQL.UnsupportedAuthenticationSpecified(parameters.AuthenticationMethod);
			}
			IEnumerator<IAccount> enumerator = (await app.GetAccountsAsync()).GetEnumerator();
			IAccount account = null;
			if (enumerator.MoveNext())
			{
				if (!string.IsNullOrEmpty(parameters.UserId))
				{
					do
					{
						IAccount current = enumerator.Current;
						if (string.Compare(parameters.UserId, current.Username, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							account = current;
							break;
						}
					}
					while (enumerator.MoveNext());
				}
				else
				{
					account = enumerator.Current;
				}
			}
			if (account != null)
			{
				try
				{
					result = await app.AcquireTokenSilent(scopes, account).ExecuteAsync(cts.Token);
					SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token (silent) for {0} auth mode. Expiry Time: {1}", parameters.AuthenticationMethod, result?.ExpiresOn);
				}
				catch (MsalUiRequiredException)
				{
					result = await AcquireTokenInteractiveDeviceFlowAsync(app, scopes, parameters.ConnectionId, parameters.UserId, parameters.AuthenticationMethod, cts);
					SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token (interactive) for {0} auth mode. Expiry Time: {1}", parameters.AuthenticationMethod, result?.ExpiresOn);
				}
			}
			else
			{
				result = await AcquireTokenInteractiveDeviceFlowAsync(app, scopes, parameters.ConnectionId, parameters.UserId, parameters.AuthenticationMethod, cts);
				SqlClientEventSource.Log.TryTraceEvent("AcquireTokenAsync | Acquired access token (interactive) for {0} auth mode. Expiry Time: {1}", parameters.AuthenticationMethod, result?.ExpiresOn);
			}
		}
		return new SqlAuthenticationToken(result.AccessToken, result.ExpiresOn);
	}

	private async Task<AuthenticationResult> AcquireTokenInteractiveDeviceFlowAsync(IPublicClientApplication app, string[] scopes, Guid connectionId, string userId, SqlAuthenticationMethod authenticationMethod, CancellationTokenSource cts)
	{
		_ = 2;
		try
		{
			if (authenticationMethod == SqlAuthenticationMethod.ActiveDirectoryInteractive)
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				if (_customWebUI != null)
				{
					return await app.AcquireTokenInteractive(scopes).WithCorrelationId(connectionId).WithCustomWebUi(_customWebUI)
						.WithLoginHint(userId)
						.ExecuteAsync(cancellationTokenSource.Token);
				}
				return await app.AcquireTokenInteractive(scopes).WithCorrelationId(connectionId).WithLoginHint(userId)
					.ExecuteAsync(cancellationTokenSource.Token);
			}
			return await app.AcquireTokenWithDeviceCode(scopes, (DeviceCodeResult deviceCodeResult) => _deviceCodeFlowCallback(deviceCodeResult)).WithCorrelationId(connectionId).ExecuteAsync(cts.Token);
		}
		catch (OperationCanceledException)
		{
			SqlClientEventSource.Log.TryTraceEvent("AcquireTokenInteractiveDeviceFlowAsync | Operation timed out while acquiring access token.");
			throw (authenticationMethod == SqlAuthenticationMethod.ActiveDirectoryInteractive) ? SQL.ActiveDirectoryInteractiveTimeout() : SQL.ActiveDirectoryDeviceFlowTimeout();
		}
	}

	private Task DefaultDeviceFlowCallback(DeviceCodeResult result)
	{
		SqlClientEventSource.Log.TryTraceEvent("AcquireTokenInteractiveDeviceFlowAsync | Callback triggered with Device Code Result: {0}", result.Message);
		Console.WriteLine(result.Message);
		return Task.FromResult(0);
	}

	private IPublicClientApplication GetPublicClientAppInstance(PublicClientAppKey publicClientAppKey)
	{
		if (!s_pcaMap.TryGetValue(publicClientAppKey, out var value))
		{
			value = CreateClientAppInstance(publicClientAppKey);
			s_pcaMap.TryAdd(publicClientAppKey, value);
		}
		return value;
	}

	private IPublicClientApplication CreateClientAppInstance(PublicClientAppKey publicClientAppKey)
	{
		if (_iWin32WindowFunc != null)
		{
			return PublicClientApplicationBuilder.Create(publicClientAppKey._applicationClientId).WithAuthority(publicClientAppKey._authority).WithClientName("Framework Microsoft SqlClient Data Provider")
				.WithClientVersion(ADP.GetAssemblyVersion().ToString())
				.WithRedirectUri(publicClientAppKey._redirectUri)
				.WithParentActivityOrWindow(_iWin32WindowFunc)
				.Build();
		}
		return PublicClientApplicationBuilder.Create(publicClientAppKey._applicationClientId).WithAuthority(publicClientAppKey._authority).WithClientName("Framework Microsoft SqlClient Data Provider")
			.WithClientVersion(ADP.GetAssemblyVersion().ToString())
			.WithRedirectUri(publicClientAppKey._redirectUri)
			.Build();
	}
}
