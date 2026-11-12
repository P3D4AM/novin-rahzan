using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public abstract class MigrationOperation : Annotatable
{
	public virtual bool IsDestructiveChange { get; set; }
}
