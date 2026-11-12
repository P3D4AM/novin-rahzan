using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerObjectToStringTranslator : IMethodCallTranslator
{
	private const int DefaultLength = 100;

	private static readonly Dictionary<Type, string> _typeMapping = new Dictionary<Type, string>
	{
		{
			typeof(int),
			"VARCHAR(11)"
		},
		{
			typeof(long),
			"VARCHAR(20)"
		},
		{
			typeof(DateTime),
			$"VARCHAR({100})"
		},
		{
			typeof(Guid),
			"VARCHAR(36)"
		},
		{
			typeof(byte),
			"VARCHAR(3)"
		},
		{
			typeof(byte[]),
			$"VARCHAR({100})"
		},
		{
			typeof(double),
			$"VARCHAR({100})"
		},
		{
			typeof(DateTimeOffset),
			$"VARCHAR({100})"
		},
		{
			typeof(char),
			"VARCHAR(1)"
		},
		{
			typeof(short),
			"VARCHAR(6)"
		},
		{
			typeof(float),
			$"VARCHAR({100})"
		},
		{
			typeof(decimal),
			$"VARCHAR({100})"
		},
		{
			typeof(TimeSpan),
			$"VARCHAR({100})"
		},
		{
			typeof(uint),
			"VARCHAR(10)"
		},
		{
			typeof(ushort),
			"VARCHAR(5)"
		},
		{
			typeof(ulong),
			"VARCHAR(19)"
		},
		{
			typeof(sbyte),
			"VARCHAR(4)"
		}
	};

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerObjectToStringTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (!(method.Name == "ToString") || arguments.Count != 0 || instance == null || !_typeMapping.TryGetValue(instance.Type.UnwrapNullableType(), out var value))
		{
			return null;
		}
		return _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
		{
			_sqlExpressionFactory.Fragment(value),
			instance
		}, typeof(string));
	}
}
