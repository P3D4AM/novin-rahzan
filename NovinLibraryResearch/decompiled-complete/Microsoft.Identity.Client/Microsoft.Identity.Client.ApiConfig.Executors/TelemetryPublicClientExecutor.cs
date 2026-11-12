using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.TelemetryCore;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal class TelemetryPublicClientExecutor : AbstractMatsExecutor, IPublicClientApplicationExecutor
{
	private readonly IPublicClientApplicationExecutor _executor;

	public IServiceBundle ServiceBundle => _executor.ServiceBundle;

	public TelemetryPublicClientExecutor(IPublicClientApplicationExecutor executor, ITelemetryClient mats)
		: base(mats)
	{
		_executor = executor;
	}

	public Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenInteractiveParameters interactiveParameters, CancellationToken cancellationToken)
	{
		return ExecuteMatsAsync(commonParameters, async () => await _executor.ExecuteAsync(commonParameters, interactiveParameters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenWithDeviceCodeParameters withDeviceCodeParameters, CancellationToken cancellationToken)
	{
		return ExecuteMatsAsync(commonParameters, async () => await _executor.ExecuteAsync(commonParameters, withDeviceCodeParameters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByIntegratedWindowsAuthParameters integratedWindowsAuthParameters, CancellationToken cancellationToken)
	{
		return ExecuteMatsAsync(commonParameters, async () => await _executor.ExecuteAsync(commonParameters, integratedWindowsAuthParameters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByUsernamePasswordParameters usernamePasswordParameters, CancellationToken cancellationToken)
	{
		return ExecuteMatsAsync(commonParameters, async () => await _executor.ExecuteAsync(commonParameters, usernamePasswordParameters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}
}
