using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
{
	private readonly QuerySqlGeneratorDependencies _dependencies;

	public SqlServerQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
	{
		_dependencies = dependencies;
	}

	public virtual QuerySqlGenerator Create()
	{
		return new SqlServerQuerySqlGenerator(_dependencies);
	}
}
