using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Internal.Requests.Silent;

internal interface ISilentAuthRequestStrategy
{
	Task<AuthenticationResult> ExecuteAsync(CancellationToken cancellationToken);
}
