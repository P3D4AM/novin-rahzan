using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Internal.Broker;

internal interface ITokenRequestComponent
{
	Task<MsalTokenResponse> FetchTokensAsync(CancellationToken cancellationToken);
}
