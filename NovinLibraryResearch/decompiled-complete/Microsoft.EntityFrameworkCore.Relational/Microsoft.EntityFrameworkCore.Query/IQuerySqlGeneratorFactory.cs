namespace Microsoft.EntityFrameworkCore.Query;

public interface IQuerySqlGeneratorFactory
{
	QuerySqlGenerator Create();
}
