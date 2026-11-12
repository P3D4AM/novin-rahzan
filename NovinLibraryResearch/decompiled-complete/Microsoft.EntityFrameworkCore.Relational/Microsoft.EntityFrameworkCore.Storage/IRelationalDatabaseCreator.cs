using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalDatabaseCreator : IDatabaseCreator
{
	bool Exists();

	Task<bool> ExistsAsync(CancellationToken cancellationToken = default(CancellationToken));

	bool HasTables();

	Task<bool> HasTablesAsync(CancellationToken cancellationToken = default(CancellationToken));

	void Create();

	Task CreateAsync(CancellationToken cancellationToken = default(CancellationToken));

	void Delete();

	Task DeleteAsync(CancellationToken cancellationToken = default(CancellationToken));

	void CreateTables();

	Task CreateTablesAsync(CancellationToken cancellationToken = default(CancellationToken));

	string GenerateCreateScript();
}
