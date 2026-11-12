using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IHistoryRepository
{
	bool Exists();

	Task<bool> ExistsAsync(CancellationToken cancellationToken = default(CancellationToken));

	IReadOnlyList<HistoryRow> GetAppliedMigrations();

	Task<IReadOnlyList<HistoryRow>> GetAppliedMigrationsAsync(CancellationToken cancellationToken = default(CancellationToken));

	string GetCreateScript();

	string GetCreateIfNotExistsScript();

	string GetInsertScript([JetBrains.Annotations.NotNull] HistoryRow row);

	string GetDeleteScript([JetBrains.Annotations.NotNull] string migrationId);

	string GetBeginIfNotExistsScript([JetBrains.Annotations.NotNull] string migrationId);

	string GetBeginIfExistsScript([JetBrains.Annotations.NotNull] string migrationId);

	string GetEndIfScript();
}
