using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.Utils;
using Microsoft.Identity.Json;

namespace Microsoft.Identity.Client;

public abstract class AbstractApplicationBuilder<T> where T : AbstractApplicationBuilder<T>
{
	internal ApplicationConfiguration Config { get; }

	internal AbstractApplicationBuilder(ApplicationConfiguration configuration)
	{
		Config = configuration;
	}

	public T WithHttpClientFactory(IMsalHttpClientFactory httpClientFactory)
	{
		Config.HttpClientFactory = httpClientFactory;
		return (T)this;
	}

	[Obsolete("This method name has a typo, please use WithInstanceDiscoveryMetadata instead", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public T WithInstanceDicoveryMetadata(string instanceDiscoveryJson)
	{
		if (string.IsNullOrEmpty(instanceDiscoveryJson))
		{
			throw new ArgumentNullException(instanceDiscoveryJson);
		}
		try
		{
			InstanceDiscoveryResponse customInstanceDiscoveryMetadata = JsonHelper.DeserializeFromJson<InstanceDiscoveryResponse>(instanceDiscoveryJson);
			Config.CustomInstanceDiscoveryMetadata = customInstanceDiscoveryMetadata;
			return (T)this;
		}
		catch (JsonException innerException)
		{
			throw new MsalClientException("invalid-custom-instance-metadata", "The json containing instance metadata could not be parsed. See https://aka.ms/msal-net-custom-instance-metadata for details.", innerException);
		}
	}

	public T WithInstanceDiscoveryMetadata(string instanceDiscoveryJson)
	{
		if (string.IsNullOrEmpty(instanceDiscoveryJson))
		{
			throw new ArgumentNullException(instanceDiscoveryJson);
		}
		try
		{
			InstanceDiscoveryResponse customInstanceDiscoveryMetadata = JsonHelper.DeserializeFromJson<InstanceDiscoveryResponse>(instanceDiscoveryJson);
			Config.CustomInstanceDiscoveryMetadata = customInstanceDiscoveryMetadata;
			return (T)this;
		}
		catch (JsonException innerException)
		{
			throw new MsalClientException("invalid-custom-instance-metadata", "The json containing instance metadata could not be parsed. See https://aka.ms/msal-net-custom-instance-metadata for details.", innerException);
		}
	}

	[Obsolete("This method name has a typo, please use WithInstanceDiscoveryMetadata instead", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public T WithInstanceDicoveryMetadata(Uri instanceDiscoveryUri)
	{
		Config.CustomInstanceDiscoveryMetadataUri = instanceDiscoveryUri ?? throw new ArgumentNullException("instanceDiscoveryUri");
		return (T)this;
	}

	public T WithInstanceDiscoveryMetadata(Uri instanceDiscoveryUri)
	{
		Config.CustomInstanceDiscoveryMetadataUri = instanceDiscoveryUri ?? throw new ArgumentNullException("instanceDiscoveryUri");
		return (T)this;
	}

	internal T WithHttpManager(IHttpManager httpManager)
	{
		Config.HttpManager = httpManager;
		return (T)this;
	}

	internal T WithPlatformProxy(IPlatformProxy platformProxy)
	{
		Config.PlatformProxy = platformProxy;
		return (T)this;
	}

	public T WithLogging(LogCallback loggingCallback, LogLevel? logLevel = null, bool? enablePiiLogging = null, bool? enableDefaultPlatformLogging = null)
	{
		if (Config.LoggingCallback != null)
		{
			throw new InvalidOperationException("LoggingCallback has already been set");
		}
		Config.LoggingCallback = loggingCallback;
		Config.LogLevel = logLevel ?? Config.LogLevel;
		Config.EnablePiiLogging = enablePiiLogging ?? Config.EnablePiiLogging;
		Config.IsDefaultPlatformLoggingEnabled = enableDefaultPlatformLogging ?? Config.IsDefaultPlatformLoggingEnabled;
		return (T)this;
	}

	public T WithDebugLoggingCallback(LogLevel logLevel = LogLevel.Info, bool enablePiiLogging = false, bool withDefaultPlatformLoggingEnabled = false)
	{
		WithLogging(delegate
		{
		}, logLevel, enablePiiLogging, withDefaultPlatformLoggingEnabled);
		return (T)this;
	}

	internal T WithTelemetry(TelemetryCallback telemetryCallback)
	{
		if (Config.TelemetryCallback != null)
		{
			throw new InvalidOperationException("TelemetryCallback has already been set");
		}
		Config.TelemetryCallback = telemetryCallback;
		return (T)this;
	}

	public T WithClientId(string clientId)
	{
		Config.ClientId = clientId;
		return (T)this;
	}

	public T WithRedirectUri(string redirectUri)
	{
		Config.RedirectUri = GetValueIfNotEmpty(Config.RedirectUri, redirectUri);
		return (T)this;
	}

	public T WithTenantId(string tenantId)
	{
		Config.TenantId = GetValueIfNotEmpty(Config.TenantId, tenantId);
		return (T)this;
	}

	public T WithClientName(string clientName)
	{
		Config.ClientName = GetValueIfNotEmpty(Config.ClientName, clientName);
		return (T)this;
	}

	public T WithClientVersion(string clientVersion)
	{
		Config.ClientVersion = GetValueIfNotEmpty(Config.ClientVersion, clientVersion);
		return (T)this;
	}

	protected T WithOptions(ApplicationOptions applicationOptions)
	{
		WithClientId(applicationOptions.ClientId);
		WithRedirectUri(applicationOptions.RedirectUri);
		WithTenantId(applicationOptions.TenantId);
		WithClientName(applicationOptions.ClientName);
		WithClientVersion(applicationOptions.ClientVersion);
		WithClientCapabilities(applicationOptions.ClientCapabilities);
		WithLogging(null, applicationOptions.LogLevel, applicationOptions.EnablePiiLogging, applicationOptions.IsDefaultPlatformLoggingEnabled);
		Config.Instance = applicationOptions.Instance;
		Config.AadAuthorityAudience = applicationOptions.AadAuthorityAudience;
		Config.AzureCloudInstance = applicationOptions.AzureCloudInstance;
		return (T)this;
	}

	public T WithExtraQueryParameters(IDictionary<string, string> extraQueryParameters)
	{
		Config.ExtraQueryParameters = extraQueryParameters ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		return (T)this;
	}

	public T WithExtraQueryParameters(string extraQueryParameters)
	{
		if (!string.IsNullOrWhiteSpace(extraQueryParameters))
		{
			return WithExtraQueryParameters(CoreHelpers.ParseKeyValueList(extraQueryParameters, '&', urlDecode: true, null));
		}
		return (T)this;
	}

	public T WithExperimentalFeatures(bool enableExperimentalFeatures = true)
	{
		Config.ExperimentalFeaturesEnabled = enableExperimentalFeatures;
		return (T)this;
	}

	public T WithClientCapabilities(IEnumerable<string> clientCapabilities)
	{
		if (clientCapabilities != null && clientCapabilities.Any())
		{
			Config.ClientCapabilities = clientCapabilities;
		}
		return (T)this;
	}

	public T WithTelemetry(ITelemetryConfig telemetryConfig)
	{
		Config.TelemetryConfig = telemetryConfig;
		return (T)this;
	}

	internal virtual void Validate()
	{
		if (string.IsNullOrWhiteSpace(Config.ClientId))
		{
			throw new MsalClientException("no_client_id", "No ClientId was specified.");
		}
		CreateAuthorityInfoFromEnums();
		if (Config.CustomInstanceDiscoveryMetadata != null && Config.CustomInstanceDiscoveryMetadataUri != null)
		{
			throw new MsalClientException("custom_metadata_instance_or_uri", "You have configured your own instance metadata using both an Uri and a string. Only one is supported. See https://aka.ms/msal-net-custom-instance-metadata for more details.");
		}
		if (Config.AuthorityInfo.ValidateAuthority && (Config.CustomInstanceDiscoveryMetadata != null || Config.CustomInstanceDiscoveryMetadataUri != null))
		{
			throw new MsalClientException("validate_authority_or_custom_instance_metadata", "You have configured custom instance metadata, but the validateAuthority flag is set to true. These are mutually exclusive. Set the validateAuthority flag to false. See https://aka.ms/msal-net-custom-instance-metadata for more details.");
		}
		if (Config.TelemetryCallback != null && Config.TelemetryConfig != null)
		{
			throw new MsalClientException("telemetry_config_or_telemetry_callback", "MATS cannot be configured at the same time as a TelemetryCallback is provided. These are mututally exclusive.");
		}
	}

	internal ApplicationConfiguration BuildConfiguration()
	{
		Validate();
		return Config;
	}

	private void CreateAuthorityInfoFromEnums()
	{
		if (Config.AuthorityInfo == null)
		{
			string authorityInstance = GetAuthorityInstance();
			string authorityAudience = GetAuthorityAudience();
			Config.AuthorityInfo = new AuthorityInfo(AuthorityType.Aad, new Uri(authorityInstance + "/" + authorityAudience).ToString(), Config.ValidateAuthority);
		}
	}

	private string GetAuthorityAudience()
	{
		if (!string.IsNullOrWhiteSpace(Config.TenantId) && Config.AadAuthorityAudience != 0 && Config.AadAuthorityAudience != AadAuthorityAudience.AzureAdMyOrg)
		{
			throw new InvalidOperationException("TenantId and AadAuthorityAudience are both set, but they're mutually exclusive.");
		}
		if (Config.AadAuthorityAudience != 0)
		{
			return AuthorityInfo.GetAadAuthorityAudienceValue(Config.AadAuthorityAudience, Config.TenantId);
		}
		if (!string.IsNullOrWhiteSpace(Config.TenantId))
		{
			return Config.TenantId;
		}
		return AuthorityInfo.GetAadAuthorityAudienceValue(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount, string.Empty);
	}

	private string GetAuthorityInstance()
	{
		if (!string.IsNullOrWhiteSpace(Config.Instance) && Config.AzureCloudInstance != 0)
		{
			throw new InvalidOperationException("Instance and AzureCloudInstance are both set but they're mutually exclusive.");
		}
		if (!string.IsNullOrWhiteSpace(Config.Instance))
		{
			Config.Instance = Config.Instance.TrimEnd(' ', '/');
			return Config.Instance;
		}
		if (Config.AzureCloudInstance != 0)
		{
			return AuthorityInfo.GetCloudUrl(Config.AzureCloudInstance);
		}
		return AuthorityInfo.GetCloudUrl(AzureCloudInstance.AzurePublic);
	}

	public T WithAuthority(Uri authorityUri, bool validateAuthority = true)
	{
		if (authorityUri == null)
		{
			throw new ArgumentNullException("authorityUri");
		}
		Config.AuthorityInfo = AuthorityInfo.FromAuthorityUri(authorityUri.ToString(), validateAuthority);
		return (T)this;
	}

	public T WithAuthority(string cloudInstanceUri, Guid tenantId, bool validateAuthority = true)
	{
		WithAuthority(cloudInstanceUri, tenantId.ToString("D"), validateAuthority);
		return (T)this;
	}

	public T WithAuthority(string cloudInstanceUri, string tenant, bool validateAuthority = true)
	{
		if (string.IsNullOrWhiteSpace(cloudInstanceUri))
		{
			throw new ArgumentNullException("cloudInstanceUri");
		}
		if (string.IsNullOrWhiteSpace(tenant))
		{
			throw new ArgumentNullException("tenant");
		}
		Config.AuthorityInfo = AuthorityInfo.FromAadAuthority(new Uri(cloudInstanceUri), tenant, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(AzureCloudInstance azureCloudInstance, Guid tenantId, bool validateAuthority = true)
	{
		WithAuthority(azureCloudInstance, tenantId.ToString("D"), validateAuthority);
		return (T)this;
	}

	public T WithAuthority(AzureCloudInstance azureCloudInstance, string tenant, bool validateAuthority = true)
	{
		if (string.IsNullOrWhiteSpace(tenant))
		{
			throw new ArgumentNullException("tenant");
		}
		Config.AzureCloudInstance = azureCloudInstance;
		Config.TenantId = tenant;
		Config.ValidateAuthority = validateAuthority;
		return (T)this;
	}

	public T WithAuthority(AzureCloudInstance azureCloudInstance, AadAuthorityAudience authorityAudience, bool validateAuthority = true)
	{
		Config.AzureCloudInstance = azureCloudInstance;
		Config.AadAuthorityAudience = authorityAudience;
		Config.ValidateAuthority = validateAuthority;
		return (T)this;
	}

	public T WithAuthority(AadAuthorityAudience authorityAudience, bool validateAuthority = true)
	{
		Config.AadAuthorityAudience = authorityAudience;
		Config.ValidateAuthority = validateAuthority;
		return (T)this;
	}

	public T WithAuthority(string authorityUri, bool validateAuthority = true)
	{
		if (string.IsNullOrWhiteSpace(authorityUri))
		{
			throw new ArgumentNullException(authorityUri);
		}
		Config.AuthorityInfo = AuthorityInfo.FromAadAuthority(authorityUri, validateAuthority);
		return (T)this;
	}

	public T WithAdfsAuthority(string authorityUri, bool validateAuthority = true)
	{
		Config.AuthorityInfo = AuthorityInfo.FromAdfsAuthority(authorityUri, validateAuthority);
		return (T)this;
	}

	public T WithB2CAuthority(string authorityUri)
	{
		Config.AuthorityInfo = AuthorityInfo.FromB2CAuthority(authorityUri);
		return (T)this;
	}

	private static string GetValueIfNotEmpty(string original, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return original;
	}
}
