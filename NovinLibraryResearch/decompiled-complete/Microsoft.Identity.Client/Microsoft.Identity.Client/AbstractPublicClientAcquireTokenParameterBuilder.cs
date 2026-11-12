using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;

namespace Microsoft.Identity.Client;

public abstract class AbstractPublicClientAcquireTokenParameterBuilder<T> : AbstractAcquireTokenParameterBuilder<T> where T : AbstractAcquireTokenParameterBuilder<T>
{
	internal IPublicClientApplicationExecutor PublicClientApplicationExecutor { get; }

	internal AbstractPublicClientAcquireTokenParameterBuilder(IPublicClientApplicationExecutor publicClientApplicationExecutor)
		: base(publicClientApplicationExecutor.ServiceBundle)
	{
		PublicClientApplicationExecutor = publicClientApplicationExecutor;
	}

	internal abstract Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken);

	public override Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		ValidateAndCalculateApiId();
		return ExecuteInternalAsync(cancellationToken);
	}
}
