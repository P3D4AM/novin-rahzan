using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class HistoryRow
{
	public virtual string MigrationId { get; }

	public virtual string ProductVersion { get; }

	public HistoryRow([JetBrains.Annotations.NotNull] string migrationId, [JetBrains.Annotations.NotNull] string productVersion)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(productVersion, "productVersion");
		MigrationId = migrationId;
		ProductVersion = productVersion;
	}
}
