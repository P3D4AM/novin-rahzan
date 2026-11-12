using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.TelemetryCore.Internal;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal abstract class AbstractMatsExecutor
{
	private readonly ITelemetryClient _mats;

	protected AbstractMatsExecutor(ITelemetryClient mats)
	{
		_mats = mats;
	}

	protected async Task<AuthenticationResult> ExecuteMatsAsync(AcquireTokenCommonParameters commonParameters, Func<Task<AuthenticationResult>> executorAction)
	{
		MatsAction action = _mats.StartAction(_mats.CreateScenario(), commonParameters.CorrelationId.AsMatsCorrelationId());
		try
		{
			AuthenticationResult authenticationResult = await executorAction().ConfigureAwait(continueOnCapturedContext: false);
			_mats.EndAction(action, authenticationResult);
			return authenticationResult;
		}
		catch (Exception ex)
		{
			_mats.EndAction(action, ex);
			throw;
		}
	}

	protected async Task<Uri> ExecuteMatsToUriAsync(AcquireTokenCommonParameters commonParameters, Func<Task<Uri>> executorAction)
	{
		MatsAction action = _mats.StartAction(null, commonParameters.CorrelationId.AsMatsCorrelationId());
		try
		{
			Uri result = await executorAction().ConfigureAwait(continueOnCapturedContext: false);
			_mats.EndAction(action, AuthOutcome.Succeeded, ErrorSource.None, string.Empty, string.Empty);
			return result;
		}
		catch (Exception ex)
		{
			_mats.EndAction(action, ex);
			throw;
		}
	}
}
