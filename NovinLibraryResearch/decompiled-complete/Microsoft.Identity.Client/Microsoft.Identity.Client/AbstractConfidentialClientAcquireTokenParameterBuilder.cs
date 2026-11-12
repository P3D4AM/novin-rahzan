using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.ApiConfig.Executors;

namespace Microsoft.Identity.Client;

public abstract class AbstractConfidentialClientAcquireTokenParameterBuilder<T> : AbstractAcquireTokenParameterBuilder<T> where T : AbstractAcquireTokenParameterBuilder<T>
{
	internal IConfidentialClientApplicationExecutor ConfidentialClientApplicationExecutor { get; }

	internal AbstractConfidentialClientAcquireTokenParameterBuilder(IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor)
		: base(confidentialClientApplicationExecutor.ServiceBundle)
	{
		ConfidentialClientApplicationExecutor = confidentialClientApplicationExecutor;
	}

	internal abstract Task<AuthenticationResult> ExecuteInternalAsync(CancellationToken cancellationToken);

	public override Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken)
	{
		ValidateAndCalculateApiId();
		return ExecuteInternalAsync(cancellationToken);
	}
}
