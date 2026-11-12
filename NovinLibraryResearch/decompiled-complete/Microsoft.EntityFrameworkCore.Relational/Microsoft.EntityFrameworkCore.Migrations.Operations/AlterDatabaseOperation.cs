using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public class AlterDatabaseOperation : MigrationOperation, IAlterMigrationOperation
{
	public virtual Annotatable OldDatabase { get; } = new Annotatable();


	IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldDatabase;
}
