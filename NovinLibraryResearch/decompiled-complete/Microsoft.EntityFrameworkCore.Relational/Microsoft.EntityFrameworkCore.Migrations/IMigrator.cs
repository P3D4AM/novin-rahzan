using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrator
{
	void Migrate([JetBrains.Annotations.CanBeNull] string targetMigration = null);

	Task MigrateAsync([JetBrains.Annotations.CanBeNull] string targetMigration = null, CancellationToken cancellationToken = default(CancellationToken));

	string GenerateScript([JetBrains.Annotations.CanBeNull] string fromMigration = null, [JetBrains.Annotations.CanBeNull] string toMigration = null, bool idempotent = false);
}
