using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class ContainsTranslator : IMethodCallTranslator
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public ContainsTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (method.IsGenericMethod && method.GetGenericMethodDefinition().Equals(Microsoft.EntityFrameworkCore.EnumerableMethods.Contains))
		{
			return _sqlExpressionFactory.In(arguments[1], arguments[0], negated: false);
		}
		if (method.Name == "Contains" && arguments.Count == 1 && method.DeclaringType.GetInterfaces().Append<Type>(method.DeclaringType).Any((Type t) => t == typeof(IList) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ICollection<>))))
		{
			return _sqlExpressionFactory.In(arguments[0], instance, negated: false);
		}
		return null;
	}
}
