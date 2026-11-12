using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;

namespace Microsoft.Identity.Client;

public abstract class AbstractClientAppBaseAcquireTokenParameterBuilder<T> : AbstractAcquireTokenParameterBuilder<T> where T : AbstractAcquireTokenParameterBuilder<T>
{
	internal IClientApplicationBaseExecutor ClientApplicationBaseExecutor { get; }

	internal AbstractClientAppBaseAcquireTokenParameterBuilder(IClientApplicationBaseExecutor clientApplicationBaseExecutor)
		: base(clientApplicationBaseExecutor.ServiceBundle)
	{
		ClientApplicationBaseExecutor = clientApplicationBaseExecutor;
	}

	internal abstract Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken);

	public override Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		ValidateAndCalculateApiId();
		return ExecuteInternalAsync(cancellationToken);
	}
}
