using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("{Sql}")]
public class SqlOperation : MigrationOperation
{
	public virtual string Sql
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual bool SuppressTransaction { get; set; }
}
