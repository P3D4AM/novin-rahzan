using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class EqualsTranslator : IMethodCallTranslator
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public EqualsTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		SqlExpression sqlExpression = null;
		SqlExpression sqlExpression2 = null;
		if (method.Name == "Equals" && instance != null && arguments.Count == 1)
		{
			sqlExpression = instance;
			sqlExpression2 = RemoveObjectConvert(arguments[0]);
		}
		else if (instance == null && method.Name == "Equals" && arguments.Count == 2)
		{
			sqlExpression = RemoveObjectConvert(arguments[0]);
			sqlExpression2 = RemoveObjectConvert(arguments[1]);
		}
		if (sqlExpression != null && sqlExpression2 != null)
		{
			if (sqlExpression.Type.UnwrapNullableType() == sqlExpression2.Type.UnwrapNullableType())
			{
				return _sqlExpressionFactory.Equal(sqlExpression, sqlExpression2);
			}
			return _sqlExpressionFactory.Constant(false);
		}
		return null;
	}

	private SqlExpression RemoveObjectConvert(SqlExpression expression)
	{
		if (expression is SqlUnaryExpression { OperatorType: ExpressionType.Convert } sqlUnaryExpression && sqlUnaryExpression.Type == typeof(object))
		{
			return sqlUnaryExpression.Operand;
		}
		return expression;
	}
}
