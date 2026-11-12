using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.TelemetryCore.Internal;

namespace Microsoft.Identity.Client.TelemetryCore;

internal interface ITelemetryClient : IDisposable
{
	IMatsTelemetryManager TelemetryManager { get; }

	MatsScenario CreateScenario();

	MatsAction StartAction(MatsScenario scenario, string correlationId);

	MatsAction StartActionWithScopes(MatsScenario scenario, string correlationId, IEnumerable<string> scopes);

	void EndAction(MatsAction action, AuthenticationResult authenticationResult);

	void EndAction(MatsAction action, Exception ex);

	void EndAction(MatsAction action, AuthOutcome outcome, ErrorSource errorSource, string error, string errorDescription);
}
