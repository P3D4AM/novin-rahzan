using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.AuthScheme;
using Microsoft.Identity.Client.AuthScheme.Bearer;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenCommonParameters
{
	private readonly Dictionary<string, string> _apiTelemetry = new Dictionary<string, string>();

	public ApiEvent.ApiIds ApiId { get; set; }

	public Guid CorrelationId { get; set; }

	public Guid UserProvidedCorrelationId { get; set; }

	public bool UseCorrelationIdFromUser { get; set; }

	public IEnumerable<string> Scopes { get; set; }

	public IDictionary<string, string> ExtraQueryParameters { get; set; }

	public string Claims { get; set; }

	public AuthorityInfo AuthorityOverride { get; set; }

	public ApiTelemetryId ApiTelemId { get; set; }

	public IAuthenticationScheme AuthenticationScheme { get; set; } = new BearerAuthenticationScheme();


	public void AddApiTelemetryFeature(ApiTelemetryFeature feature)
	{
		_apiTelemetry[MatsConverter.AsString(feature)] = "true";
	}

	public void AddApiTelemetryFeature(ApiTelemetryFeature feature, bool isTrue)
	{
		string value = "false";
		if (isTrue)
		{
			value = "true";
		}
		_apiTelemetry[MatsConverter.AsString(feature)] = value;
	}

	public IEnumerable<KeyValuePair<string, string>> GetApiTelemetryFeatures()
	{
		return _apiTelemetry;
	}
}
