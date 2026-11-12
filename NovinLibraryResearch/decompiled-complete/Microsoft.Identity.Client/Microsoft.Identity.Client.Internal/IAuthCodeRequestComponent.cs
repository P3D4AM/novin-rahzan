using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Internal;

internal interface IAuthCodeRequestComponent
{
	Task<Tuple<string, string>> FetchAuthCodeAndPkceVerifierAsync(CancellationToken cancellationToken);

	Uri GetAuthorizationUriWithoutPkce();
}
