using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class ComparisonTranslator : IMethodCallTranslator
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public ComparisonTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (method.ReturnType == typeof(int))
		{
			SqlExpression sqlExpression = null;
			SqlExpression sqlExpression2 = null;
			if (method.Name == "Compare" && arguments.Count == 2 && arguments[0].Type.UnwrapNullableType() == arguments[1].Type.UnwrapNullableType())
			{
				sqlExpression = arguments[0];
				sqlExpression2 = arguments[1];
			}
			else if (method.Name == "CompareTo" && arguments.Count == 1 && instance != null && instance.Type.UnwrapNullableType() == arguments[0].Type.UnwrapNullableType())
			{
				sqlExpression = instance;
				sqlExpression2 = arguments[0];
			}
			if (sqlExpression != null && sqlExpression2 != null)
			{
				return _sqlExpressionFactory.Case(new CaseWhenClause[3]
				{
					new CaseWhenClause(_sqlExpressionFactory.Equal(sqlExpression, sqlExpression2), _sqlExpressionFactory.Constant(0)),
					new CaseWhenClause(_sqlExpressionFactory.GreaterThan(sqlExpression, sqlExpression2), _sqlExpressionFactory.Constant(1)),
					new CaseWhenClause(_sqlExpressionFactory.LessThan(sqlExpression, sqlExpression2), _sqlExpressionFactory.Constant(-1))
				}, null);
			}
		}
		return null;
	}
}
