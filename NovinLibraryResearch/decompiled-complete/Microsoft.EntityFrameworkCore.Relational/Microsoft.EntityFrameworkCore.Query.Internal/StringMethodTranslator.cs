using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class StringMethodTranslator : IMethodCallTranslator
{
	private static readonly MethodInfo _isNullOrEmptyMethodInfo = typeof(string).GetRuntimeMethod("IsNullOrEmpty", new Type[1] { typeof(string) });

	private static readonly MethodInfo _concatMethodInfo = typeof(string).GetRuntimeMethod("Concat", new Type[2]
	{
		typeof(string),
		typeof(string)
	});

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public StringMethodTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (object.Equals(method, _isNullOrEmptyMethodInfo))
		{
			SqlExpression sqlExpression = arguments[0];
			return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.IsNull(sqlExpression), _sqlExpressionFactory.Equal(sqlExpression, _sqlExpressionFactory.Constant(string.Empty)));
		}
		if (object.Equals(method, _concatMethodInfo))
		{
			return _sqlExpressionFactory.Add(arguments[0], arguments[1]);
		}
		return null;
	}
}
