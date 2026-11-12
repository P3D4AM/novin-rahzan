using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class LikeTranslator : IMethodCallTranslator
{
	private static readonly MethodInfo _methodInfo = typeof(DbFunctionsExtensions).GetRuntimeMethod("Like", new Type[3]
	{
		typeof(DbFunctions),
		typeof(string),
		typeof(string)
	});

	private static readonly MethodInfo _methodInfoWithEscape = typeof(DbFunctionsExtensions).GetRuntimeMethod("Like", new Type[4]
	{
		typeof(DbFunctions),
		typeof(string),
		typeof(string),
		typeof(string)
	});

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public LikeTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (object.Equals(method, _methodInfo))
		{
			return _sqlExpressionFactory.Like(arguments[1], arguments[2]);
		}
		if (object.Equals(method, _methodInfoWithEscape))
		{
			return _sqlExpressionFactory.Like(arguments[1], arguments[2], arguments[3]);
		}
		return null;
	}
}
