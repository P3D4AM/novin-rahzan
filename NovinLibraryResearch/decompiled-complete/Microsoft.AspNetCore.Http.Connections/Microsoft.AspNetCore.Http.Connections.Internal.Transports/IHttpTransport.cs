using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http.Connections.Internal.Transports;

public interface IHttpTransport
{
	Task ProcessRequestAsync(HttpContext context, CancellationToken token);
}
