using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IDatabaseCreator
{
	bool EnsureDeleted();

	Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default(CancellationToken));

	bool EnsureCreated();

	Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default(CancellationToken));

	bool CanConnect();

	Task<bool> CanConnectAsync(CancellationToken cancellationToken = default(CancellationToken));
}
