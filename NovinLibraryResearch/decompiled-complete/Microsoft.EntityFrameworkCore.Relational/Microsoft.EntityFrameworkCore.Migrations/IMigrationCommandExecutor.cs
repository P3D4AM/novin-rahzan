using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrationCommandExecutor
{
	void ExecuteNonQuery([JetBrains.Annotations.NotNull] IEnumerable<MigrationCommand> migrationCommands, [JetBrains.Annotations.NotNull] IRelationalConnection connection);

	Task ExecuteNonQueryAsync([JetBrains.Annotations.NotNull] IEnumerable<MigrationCommand> migrationCommands, [JetBrains.Annotations.NotNull] IRelationalConnection connection, CancellationToken cancellationToken = default(CancellationToken));
}
