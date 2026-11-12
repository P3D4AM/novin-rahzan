using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class RelationalQueryContextFactory : IQueryContextFactory
{
	private readonly QueryContextDependencies _dependencies;

	private readonly RelationalQueryContextDependencies _relationalDependencies;

	public RelationalQueryContextFactory([JetBrains.Annotations.NotNull] QueryContextDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalQueryContextDependencies relationalDependencies)
	{
		_dependencies = dependencies;
		_relationalDependencies = relationalDependencies;
	}

	public virtual QueryContext Create()
	{
		return new RelationalQueryContext(_dependencies, _relationalDependencies);
	}
}
