using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerQuerySqlGenerator : QuerySqlGenerator
{
	public SqlServerQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void GenerateTop(SelectExpression selectExpression)
	{
		if (selectExpression.Limit != null && selectExpression.Offset == null)
		{
			Sql.Append("TOP(");
			Visit(selectExpression.Limit);
			Sql.Append(") ");
		}
	}

	protected override void GenerateLimitOffset(SelectExpression selectExpression)
	{
		if (selectExpression.Offset != null)
		{
			Sql.AppendLine().Append("OFFSET ");
			Visit(selectExpression.Offset);
			Sql.Append(" ROWS");
			if (selectExpression.Limit != null)
			{
				Sql.Append(" FETCH NEXT ");
				Visit(selectExpression.Limit);
				Sql.Append(" ROWS ONLY");
			}
		}
	}

	protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
	{
		if (!sqlFunctionExpression.IsBuiltIn && string.IsNullOrEmpty(sqlFunctionExpression.Schema))
		{
			sqlFunctionExpression = SqlFunctionExpression.Create("dbo", sqlFunctionExpression.Name, sqlFunctionExpression.Arguments, sqlFunctionExpression.Type, sqlFunctionExpression.TypeMapping);
		}
		return base.VisitSqlFunction(sqlFunctionExpression);
	}
}
