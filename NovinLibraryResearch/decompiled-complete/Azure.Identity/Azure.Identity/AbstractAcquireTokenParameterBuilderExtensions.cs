using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Azure.Identity;

internal static class AbstractAcquireTokenParameterBuilderExtensions
{
	public static async ValueTask<AuthenticationResult> ExecuteAsync<T>(this AbstractAcquireTokenParameterBuilder<T> builder, bool async, CancellationToken cancellationToken) where T : AbstractAcquireTokenParameterBuilder<T>
	{
		return (!async) ? builder.ExecuteAsync(cancellationToken).GetAwaiter().GetResult() : (await builder.ExecuteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}
}
