using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalQueryContext : QueryContext
{
	protected virtual RelationalQueryContextDependencies RelationalDependencies { get; }

	public virtual IRelationalConnection Connection => RelationalDependencies.RelationalConnection;

	public RelationalQueryContext([JetBrains.Annotations.NotNull] QueryContextDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalQueryContextDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}
}
