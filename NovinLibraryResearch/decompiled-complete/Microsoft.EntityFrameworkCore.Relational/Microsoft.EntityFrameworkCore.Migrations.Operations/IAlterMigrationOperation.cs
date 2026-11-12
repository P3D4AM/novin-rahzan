using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public interface IAlterMigrationOperation
{
	IMutableAnnotatable OldAnnotations { get; }
}
