using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class EnumHasFlagTranslator : IMethodCallTranslator
{
	private static readonly MethodInfo _methodInfo = typeof(Enum).GetRuntimeMethod("HasFlag", new Type[1] { typeof(Enum) });

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public EnumHasFlagTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (object.Equals(method, _methodInfo))
		{
			SqlExpression sqlExpression = arguments[0];
			if (instance.Type.UnwrapNullableType() != sqlExpression.Type.UnwrapNullableType())
			{
				return null;
			}
			return _sqlExpressionFactory.Equal(_sqlExpressionFactory.And(instance, sqlExpression), sqlExpression);
		}
		return null;
	}
}
