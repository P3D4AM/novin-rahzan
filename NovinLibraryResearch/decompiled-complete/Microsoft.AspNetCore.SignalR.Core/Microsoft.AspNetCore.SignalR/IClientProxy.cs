using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR;

public interface IClientProxy
{
	Task SendCoreAsync(string method, object[] args, CancellationToken cancellationToken = default(CancellationToken));
}
