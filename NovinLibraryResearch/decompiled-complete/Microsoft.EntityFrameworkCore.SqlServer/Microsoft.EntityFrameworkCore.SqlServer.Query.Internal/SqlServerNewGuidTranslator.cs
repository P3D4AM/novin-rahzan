using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerNewGuidTranslator : IMethodCallTranslator
{
	private static readonly MethodInfo _methodInfo = typeof(Guid).GetRuntimeMethod("NewGuid", Array.Empty<Type>());

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerNewGuidTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (!_methodInfo.Equals(method))
		{
			return null;
		}
		return _sqlExpressionFactory.Function("NEWID", Array.Empty<SqlExpression>(), method.ReturnType);
	}
}
