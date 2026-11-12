using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class DeviceCodeCredential : TokenCredential
{
	private const string AuthenticationRequiredMessage = "Interactive authentication is needed to acquire token. Call Authenticate to initiate the device code authentication.";

	private const string NoDefaultScopeMessage = "Authenticating in this environment requires specifying a TokenRequestContext.";

	internal MsalPublicClient Client { get; }

	internal string ClientId { get; }

	internal bool DisableAutomaticAuthentication { get; }

	internal AuthenticationRecord Record { get; private set; }

	internal Func<DeviceCodeInfo, CancellationToken, Task> DeviceCodeCallback { get; }

	internal CredentialPipeline Pipeline { get; }

	public DeviceCodeCredential()
		: this(DefaultDeviceCodeHandler, null, "04b07795-8ddb-461a-bbee-02f9e1bf7b46", null, null)
	{
	}

	public DeviceCodeCredential(DeviceCodeCredentialOptions options)
		: this(options?.DeviceCodeCallback ?? new Func<DeviceCodeInfo, CancellationToken, Task>(DefaultDeviceCodeHandler), options?.TenantId, options?.ClientId ?? "04b07795-8ddb-461a-bbee-02f9e1bf7b46", options, null)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public DeviceCodeCredential(Func<DeviceCodeInfo, CancellationToken, Task> deviceCodeCallback, string clientId, TokenCredentialOptions options = null)
		: this(deviceCodeCallback, null, clientId, options, null)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public DeviceCodeCredential(Func<DeviceCodeInfo, CancellationToken, Task> deviceCodeCallback, string tenantId, string clientId, TokenCredentialOptions options = null)
		: this(deviceCodeCallback, Validations.ValidateTenantId(tenantId, "tenantId", allowNull: true), clientId, options, null)
	{
	}

	internal DeviceCodeCredential(Func<DeviceCodeInfo, CancellationToken, Task> deviceCodeCallback, string tenantId, string clientId, TokenCredentialOptions options, CredentialPipeline pipeline)
		: this(deviceCodeCallback, tenantId, clientId, options, pipeline, null)
	{
	}

	internal DeviceCodeCredential(Func<DeviceCodeInfo, CancellationToken, Task> deviceCodeCallback, string tenantId, string clientId, TokenCredentialOptions options, CredentialPipeline pipeline, MsalPublicClient client)
	{
		ClientId = clientId ?? throw new ArgumentNullException("clientId");
		DeviceCodeCallback = deviceCodeCallback ?? throw new ArgumentNullException("deviceCodeCallback");
		DisableAutomaticAuthentication = (options as DeviceCodeCredentialOptions)?.DisableAutomaticAuthentication ?? false;
		Record = (options as DeviceCodeCredentialOptions)?.AuthenticationRecord;
		Pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		Client = client ?? new MsalPublicClient(Pipeline, tenantId, ClientId, AzureAuthorityHosts.GetDeviceCodeRedirectUri(Pipeline.AuthorityHost).ToString(), options as ITokenCacheOptions);
	}

	internal virtual AuthenticationRecord Authenticate(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = AzureAuthorityHosts.GetDefaultScope(Pipeline.AuthorityHost) ?? throw new CredentialUnavailableException("Authenticating in this environment requires specifying a TokenRequestContext.");
		return Authenticate(new TokenRequestContext(new string[1] { text }), cancellationToken);
	}

	internal virtual async Task<AuthenticationRecord> AuthenticateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = AzureAuthorityHosts.GetDefaultScope(Pipeline.AuthorityHost) ?? throw new CredentialUnavailableException("Authenticating in this environment requires specifying a TokenRequestContext.");
		return await AuthenticateAsync(new TokenRequestContext(new string[1] { text }), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal virtual AuthenticationRecord Authenticate(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AuthenticateImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	internal virtual async Task<AuthenticationRecord> AuthenticateAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await AuthenticateImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetTokenImplAsync(async: false, requestContext, cancellationToken).EnsureCompleted();
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await GetTokenImplAsync(async: true, requestContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	internal static Task DefaultDeviceCodeHandler(DeviceCodeInfo deviceCodeInfo, CancellationToken cancellationToken)
	{
		Console.WriteLine(deviceCodeInfo.Message);
		return Task.CompletedTask;
	}

	private async Task<AuthenticationRecord> AuthenticateImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = Pipeline.StartGetTokenScope("DeviceCodeCredential.Authenticate", requestContext);
		try
		{
			scope.Succeeded(await GetTokenViaDeviceCodeAsync(requestContext.Scopes, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			return Record;
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(bool async, TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = Pipeline.StartGetTokenScope("DeviceCodeCredential.GetToken", requestContext);
		_ = 1;
		try
		{
			Exception innerException = null;
			if (Record != null)
			{
				try
				{
					AuthenticationResult authenticationResult = await Client.AcquireTokenSilentAsync(requestContext.Scopes, Record, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
				}
				catch (MsalUiRequiredException ex)
				{
					innerException = ex;
				}
			}
			if (DisableAutomaticAuthentication)
			{
				throw new AuthenticationRequiredException("Interactive authentication is needed to acquire token. Call Authenticate to initiate the device code authentication.", requestContext, innerException);
			}
			return scope.Succeeded(await GetTokenViaDeviceCodeAsync(requestContext.Scopes, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
		}
		catch (Exception ex2)
		{
			throw scope.FailWrapAndThrow(ex2);
		}
	}

	private async Task<AccessToken> GetTokenViaDeviceCodeAsync(string[] scopes, bool async, CancellationToken cancellationToken)
	{
		AuthenticationResult authenticationResult = await Client.AcquireTokenWithDeviceCodeAsync(scopes, (DeviceCodeResult code) => DeviceCodeCallbackImpl(code, cancellationToken), async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		Record = new AuthenticationRecord(authenticationResult, ClientId);
		return new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn);
	}

	private Task DeviceCodeCallbackImpl(DeviceCodeResult deviceCode, CancellationToken cancellationToken)
	{
		return DeviceCodeCallback(new DeviceCodeInfo(deviceCode), cancellationToken);
	}
}
