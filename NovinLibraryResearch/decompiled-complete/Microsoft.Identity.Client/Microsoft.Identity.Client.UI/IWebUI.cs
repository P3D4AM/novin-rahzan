using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.UI;

internal interface IWebUI
{
	Task<AuthorizationResult> AcquireAuthorizationAsync(Uri authorizationUri, Uri redirectUri, RequestContext requestContext, CancellationToken cancellationToken);

	Uri UpdateRedirectUri(Uri redirectUri);
}
