using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerIsDateFunctionTranslator : IMethodCallTranslator
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	private static readonly MethodInfo _methodInfo = typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("IsDate", new Type[2]
	{
		typeof(DbFunctions),
		typeof(string)
	});

	public SqlServerIsDateFunctionTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (!_methodInfo.Equals(method))
		{
			return null;
		}
		return _sqlExpressionFactory.Convert(_sqlExpressionFactory.Function("ISDATE", new SqlExpression[1] { arguments[1] }, _methodInfo.ReturnType), _methodInfo.ReturnType);
	}
}
