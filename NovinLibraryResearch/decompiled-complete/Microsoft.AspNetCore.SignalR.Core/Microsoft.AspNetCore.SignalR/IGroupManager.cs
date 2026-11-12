using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.SignalR;

public interface IGroupManager
{
	Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default(CancellationToken));

	Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default(CancellationToken));
}
