using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Parameters;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.TelemetryCore;

namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal class TelemetryClientApplicationBaseExecutor : AbstractMatsExecutor, IClientApplicationBaseExecutor
{
	private readonly IClientApplicationBaseExecutor _executor;

	public IServiceBundle ServiceBundle => _executor.ServiceBundle;

	public TelemetryClientApplicationBaseExecutor(IClientApplicationBaseExecutor executor, ITelemetryClient mats)
		: base(mats)
	{
		_executor = executor;
	}

	public Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenSilentParameters silentParameters, CancellationToken cancellationToken)
	{
		return ExecuteMatsAsync(commonParameters, async () => await _executor.ExecuteAsync(commonParameters, silentParameters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public Task<AuthenticationResult> ExecuteAsync(AcquireTokenCommonParameters commonParameters, AcquireTokenByRefreshTokenParameters byRefreshTokenParameters, CancellationToken cancellationToken)
	{
		return ExecuteMatsAsync(commonParameters, async () => await _executor.ExecuteAsync(commonParameters, byRefreshTokenParameters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}
}
