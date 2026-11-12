using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Extensibility;

public interface ICustomWebUi
{
	Task<Uri> AcquireAuthorizationCodeAsync(Uri authorizationUri, Uri redirectUri, CancellationToken cancellationToken);
}
