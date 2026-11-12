using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core;

public abstract class TokenCredential
{
	public abstract ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken);

	public abstract AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken);
}
