namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
{
	private readonly QuerySqlGeneratorDependencies _dependencies;

	public QuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual QuerySqlGenerator Create()
	{
		return new QuerySqlGenerator(_dependencies);
	}
}
