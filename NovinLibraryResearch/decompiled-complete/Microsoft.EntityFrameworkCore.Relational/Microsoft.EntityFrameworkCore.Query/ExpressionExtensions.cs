using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public static class ExpressionExtensions
{
	public static RelationalTypeMapping InferTypeMapping(params SqlExpression[] expressions)
	{
		foreach (SqlExpression sqlExpression in expressions)
		{
			if (sqlExpression.TypeMapping != null)
			{
				return sqlExpression.TypeMapping;
			}
		}
		return null;
	}
}
