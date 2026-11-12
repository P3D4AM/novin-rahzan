using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.AuthScheme;
using Microsoft.Identity.Client.AuthScheme.PoP;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.Utils;

namespace Microsoft.Identity.Client;

public abstract class AbstractAcquireTokenParameterBuilder<T> where T : AbstractAcquireTokenParameterBuilder<T>
{
	internal IServiceBundle ServiceBundle { get; }

	internal AcquireTokenCommonParameters CommonParameters { get; } = new AcquireTokenCommonParameters();


	internal abstract ApiTelemetryId ApiTelemetryId { get; }

	protected AbstractAcquireTokenParameterBuilder()
	{
	}

	internal AbstractAcquireTokenParameterBuilder(IServiceBundle serviceBundle)
	{
		ServiceBundle = serviceBundle;
	}

	public abstract Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken);

	internal abstract ApiEvent.ApiIds CalculateApiEventId();

	public Task<AuthenticationResult> ExecuteAsync()
	{
		return ExecuteAsync(CancellationToken.None);
	}

	protected T WithScopes(IEnumerable<string> scopes)
	{
		CommonParameters.Scopes = scopes;
		return (T)this;
	}

	public T WithExtraQueryParameters(Dictionary<string, string> extraQueryParameters)
	{
		CommonParameters.ExtraQueryParameters = extraQueryParameters ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		return (T)this;
	}

	public T WithClaims(string claims)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithClaims);
		CommonParameters.Claims = claims;
		return (T)this;
	}

	public T WithExtraQueryParameters(string extraQueryParameters)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithExtraQueryParameters);
		if (!string.IsNullOrWhiteSpace(extraQueryParameters))
		{
			return WithExtraQueryParameters(CoreHelpers.ParseKeyValueList(extraQueryParameters, '&', urlDecode: true, null));
		}
		return (T)this;
	}

	public T WithAuthority(string authorityUri, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		if (string.IsNullOrWhiteSpace(authorityUri))
		{
			throw new ArgumentNullException("authorityUri");
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAuthorityUri(authorityUri, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(string cloudInstanceUri, Guid tenantId, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		if (string.IsNullOrWhiteSpace(cloudInstanceUri))
		{
			throw new ArgumentNullException("cloudInstanceUri");
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAadAuthority(new Uri(cloudInstanceUri), tenantId, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(string cloudInstanceUri, string tenant, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		if (string.IsNullOrWhiteSpace(cloudInstanceUri))
		{
			throw new ArgumentNullException("cloudInstanceUri");
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAadAuthority(new Uri(cloudInstanceUri), tenant, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(AzureCloudInstance azureCloudInstance, Guid tenantId, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAadAuthority(azureCloudInstance, tenantId, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(AzureCloudInstance azureCloudInstance, string tenant, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAadAuthority(azureCloudInstance, tenant, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(AzureCloudInstance azureCloudInstance, AadAuthorityAudience authorityAudience, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAadAuthority(azureCloudInstance, authorityAudience, validateAuthority);
		return (T)this;
	}

	public T WithAuthority(AadAuthorityAudience authorityAudience, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		CommonParameters.AuthorityOverride = AuthorityInfo.FromAadAuthority(authorityAudience, validateAuthority);
		return (T)this;
	}

	public T WithAdfsAuthority(string authorityUri, bool validateAuthority = true)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithAdfsAuthority);
		if (validateAuthority)
		{
			CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithValidateAuthority);
		}
		if (string.IsNullOrWhiteSpace(authorityUri))
		{
			throw new ArgumentNullException("authorityUri");
		}
		CommonParameters.AuthorityOverride = new AuthorityInfo(AuthorityType.Adfs, authorityUri, validateAuthority);
		return (T)this;
	}

	public T WithB2CAuthority(string authorityUri)
	{
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithB2CAuthority);
		if (string.IsNullOrWhiteSpace(authorityUri))
		{
			throw new ArgumentNullException("authorityUri");
		}
		CommonParameters.AuthorityOverride = new AuthorityInfo(AuthorityType.B2C, authorityUri, validateAuthority: false);
		return (T)this;
	}

	public T WithCorrelationId(Guid correlationId)
	{
		CommonParameters.UserProvidedCorrelationId = correlationId;
		CommonParameters.UseCorrelationIdFromUser = true;
		return (T)this;
	}

	internal T WithAuthenticationScheme(IAuthenticationScheme scheme)
	{
		CommonParameters.AuthenticationScheme = scheme ?? throw new ArgumentNullException("scheme");
		return (T)this;
	}

	public T WithProofOfPosession(HttpRequestMessage httpRequestMessage)
	{
		IPoPCryptoProvider defaultPoPCryptoProvider = ServiceBundle.PlatformProxy.GetDefaultPoPCryptoProvider();
		return WithProofOfPosession(httpRequestMessage, defaultPoPCryptoProvider);
	}

	internal T WithProofOfPosession(HttpRequestMessage httpRequestMessage, IPoPCryptoProvider popCryptoProvider)
	{
		if (!ServiceBundle.Config.ExperimentalFeaturesEnabled)
		{
			throw new MsalClientException("experimental_feature", MsalErrorMessage.ExperimentalFeature("WithProofOfPosession"));
		}
		if (httpRequestMessage == null)
		{
			throw new ArgumentNullException("httpRequestMessage");
		}
		if (popCryptoProvider == null)
		{
			throw new ArgumentNullException("popCryptoProvider");
		}
		CommonParameters.AddApiTelemetryFeature(ApiTelemetryFeature.WithPoPScheme);
		CommonParameters.AuthenticationScheme = new PoPAuthenticationScheme(httpRequestMessage, popCryptoProvider);
		return this as T;
	}

	protected virtual void Validate()
	{
	}

	internal void ValidateAndCalculateApiId()
	{
		Validate();
		CommonParameters.ApiId = CalculateApiEventId();
		CommonParameters.ApiTelemId = ApiTelemetryId;
		CommonParameters.CorrelationId = (CommonParameters.UseCorrelationIdFromUser ? CommonParameters.UserProvidedCorrelationId : Guid.NewGuid());
	}
}
