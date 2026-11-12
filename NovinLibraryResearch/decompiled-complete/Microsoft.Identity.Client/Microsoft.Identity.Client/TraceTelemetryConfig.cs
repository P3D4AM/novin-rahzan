#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Identity.Client.TelemetryCore.Internal;
using Microsoft.Identity.Json;
using Microsoft.Identity.Json.Linq;

namespace Microsoft.Identity.Client;

public class TraceTelemetryConfig : ITelemetryConfig
{
	public TelemetryAudienceType AudienceType => TelemetryAudienceType.PreProduction;

	public string SessionId { get; }

	public Action<ITelemetryEventPayload> DispatchAction => delegate(ITelemetryEventPayload payload)
	{
		JObject jObject = new JObject();
		foreach (KeyValuePair<string, bool> boolValue in payload.BoolValues)
		{
			jObject[boolValue.Key] = boolValue.Value;
		}
		foreach (KeyValuePair<string, int> intValue in payload.IntValues)
		{
			jObject[intValue.Key] = intValue.Value;
		}
		foreach (KeyValuePair<string, long> int64Value in payload.Int64Values)
		{
			jObject[int64Value.Key] = int64Value.Value;
		}
		foreach (KeyValuePair<string, string> stringValue in payload.StringValues)
		{
			jObject[stringValue.Key] = stringValue.Value;
		}
		Trace.TraceInformation(JsonConvert.SerializeObject(jObject, Formatting.None));
		Trace.Flush();
	};

	public IEnumerable<string> AllowedScopes => new List<string>();

	public TraceTelemetryConfig()
	{
		SessionId = Guid.NewGuid().AsMatsCorrelationId();
	}
}
