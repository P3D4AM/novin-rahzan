using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Microsoft.Identity.Client;

namespace Azure.Identity;

public class VisualStudioCodeCredential : TokenCredential
{
	private const string CredentialsSection = "VS Code Azure";

	private const string ClientId = "aebc6443-996d-45c2-90f0-388ff96faa56";

	private readonly IVisualStudioCodeAdapter _vscAdapter;

	private readonly IFileSystemService _fileSystem;

	private readonly CredentialPipeline _pipeline;

	private readonly string _tenantId;

	private readonly MsalPublicClient _client;

	public VisualStudioCodeCredential()
		: this(null, null, null, null, null)
	{
	}

	public VisualStudioCodeCredential(VisualStudioCodeCredentialOptions options)
		: this(options, null, null, null, null)
	{
	}

	internal VisualStudioCodeCredential(VisualStudioCodeCredentialOptions options, CredentialPipeline pipeline, MsalPublicClient client, IFileSystemService fileSystem, IVisualStudioCodeAdapter vscAdapter)
	{
		_tenantId = options?.TenantId ?? "common";
		_pipeline = pipeline ?? CredentialPipeline.GetInstance(options);
		_client = client ?? new MsalPublicClient(_pipeline, options?.TenantId, "aebc6443-996d-45c2-90f0-388ff96faa56", null, null);
		_fileSystem = fileSystem ?? FileSystemService.Default;
		_vscAdapter = vscAdapter ?? GetVscAdapter();
	}

	public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		return await GetTokenImplAsync(requestContext, async: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
	{
		return GetTokenImplAsync(requestContext, async: false, cancellationToken).EnsureCompleted();
	}

	private async ValueTask<AccessToken> GetTokenImplAsync(TokenRequestContext requestContext, bool async, CancellationToken cancellationToken)
	{
		using CredentialDiagnosticScope scope = _pipeline.StartGetTokenScope("VisualStudioCodeCredential.GetToken", requestContext);
		try
		{
			GetUserSettings(out var tenant, out var environmentName);
			if (string.Equals(tenant, "adfs", StringComparison.Ordinal))
			{
				throw new CredentialUnavailableException("VisualStudioCodeCredential authentication unavailable. ADFS tenant / authorities are not supported.");
			}
			AzureCloudInstance azureCloudInstance = GetAzureCloudInstance(environmentName);
			string storedCredentials = GetStoredCredentials(environmentName);
			AuthenticationResult authenticationResult = await _client.AcquireTokenByRefreshToken(requestContext.Scopes, storedCredentials, azureCloudInstance, tenant, async, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return scope.Succeeded(new AccessToken(authenticationResult.AccessToken, authenticationResult.ExpiresOn));
		}
		catch (MsalUiRequiredException innerException)
		{
			throw scope.FailWrapAndThrow(new CredentialUnavailableException("VisualStudioCodeCredential authentication unavailable. Token acquisition failed. Ensure that you have authenticated in VSCode Azure Account.", innerException));
		}
		catch (Exception ex)
		{
			throw scope.FailWrapAndThrow(ex);
		}
	}

	private string GetStoredCredentials(string environmentName)
	{
		try
		{
			string credentials = _vscAdapter.GetCredentials("VS Code Azure", environmentName);
			if (!IsRefreshTokenString(credentials))
			{
				throw new CredentialUnavailableException("Need to re-authenticate user in VSCode Azure Account.");
			}
			return credentials;
		}
		catch (Exception ex) when (!(ex is OperationCanceledException) && !(ex is CredentialUnavailableException))
		{
			throw new CredentialUnavailableException("Stored credentials not found. Need to authenticate user in VSCode Azure Account.", ex);
		}
	}

	private static bool IsRefreshTokenString(string str)
	{
		foreach (uint num in str)
		{
			if ((num < 48 || num > 57) && (num < 65 || num > 90) && (num < 97 || num > 122) && num != 95 && num != 45 && num != 46)
			{
				return false;
			}
		}
		return true;
	}

	private void GetUserSettings(out string tenant, out string environmentName)
	{
		string userSettingsPath = _vscAdapter.GetUserSettingsPath();
		tenant = _tenantId;
		environmentName = "AzureCloud";
		try
		{
			JsonElement rootElement = JsonDocument.Parse(_fileSystem.ReadAllText(userSettingsPath)).RootElement;
			if (rootElement.TryGetProperty("azure.tenant", out var value))
			{
				tenant = value.GetString();
			}
			if (rootElement.TryGetProperty("azure.cloud", out var value2))
			{
				environmentName = value2.GetString();
			}
		}
		catch (IOException)
		{
		}
		catch (JsonException)
		{
		}
	}

	private static IVisualStudioCodeAdapter GetVscAdapter()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return new WindowsVisualStudioCodeAdapter();
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			return new MacosVisualStudioCodeAdapter();
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			return new LinuxVisualStudioCodeAdapter();
		}
		throw new PlatformNotSupportedException();
	}

	private static AzureCloudInstance GetAzureCloudInstance(string name)
	{
		return name switch
		{
			"AzureCloud" => AzureCloudInstance.AzurePublic, 
			"AzureChina" => AzureCloudInstance.AzureChina, 
			"AzureGermanCloud" => AzureCloudInstance.AzureGermany, 
			"AzureUSGovernment" => AzureCloudInstance.AzureUsGovernment, 
			_ => AzureCloudInstance.AzurePublic, 
		};
	}
}
