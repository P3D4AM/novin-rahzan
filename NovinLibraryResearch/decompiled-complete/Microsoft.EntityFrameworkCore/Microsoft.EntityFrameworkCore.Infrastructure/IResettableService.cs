using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IResettableService
{
	void ResetState();

	Task ResetStateAsync(CancellationToken cancellationToken = default(CancellationToken));
}
