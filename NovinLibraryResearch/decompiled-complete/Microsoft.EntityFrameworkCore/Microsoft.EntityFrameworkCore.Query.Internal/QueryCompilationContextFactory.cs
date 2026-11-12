namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QueryCompilationContextFactory : IQueryCompilationContextFactory
{
	private readonly QueryCompilationContextDependencies _dependencies;

	public QueryCompilationContextFactory(QueryCompilationContextDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual QueryCompilationContext Create(bool async)
	{
		return new QueryCompilationContext(_dependencies, async);
	}
}
