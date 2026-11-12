using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerFullTextSearchFunctionsTranslator : IMethodCallTranslator
{
	private const string FreeTextFunctionName = "FREETEXT";

	private const string ContainsFunctionName = "CONTAINS";

	private static readonly MethodInfo _freeTextMethodInfo = typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("FreeText", new Type[3]
	{
		typeof(DbFunctions),
		typeof(string),
		typeof(string)
	});

	private static readonly MethodInfo _freeTextMethodInfoWithLanguage = typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("FreeText", new Type[4]
	{
		typeof(DbFunctions),
		typeof(string),
		typeof(string),
		typeof(int)
	});

	private static readonly MethodInfo _containsMethodInfo = typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("Contains", new Type[3]
	{
		typeof(DbFunctions),
		typeof(string),
		typeof(string)
	});

	private static readonly MethodInfo _containsMethodInfoWithLanguage = typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("Contains", new Type[4]
	{
		typeof(DbFunctions),
		typeof(string),
		typeof(string),
		typeof(int)
	});

	private static readonly IDictionary<MethodInfo, string> _functionMapping = new Dictionary<MethodInfo, string>
	{
		{ _freeTextMethodInfo, "FREETEXT" },
		{ _freeTextMethodInfoWithLanguage, "FREETEXT" },
		{ _containsMethodInfo, "CONTAINS" },
		{ _containsMethodInfoWithLanguage, "CONTAINS" }
	};

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerFullTextSearchFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (_functionMapping.TryGetValue(method, out var value))
		{
			SqlExpression sqlExpression = arguments[1];
			if (!(sqlExpression is ColumnExpression))
			{
				throw new InvalidOperationException(SqlServerStrings.InvalidColumnNameForFreeText);
			}
			RelationalTypeMapping typeMapping = sqlExpression.TypeMapping;
			SqlExpression item = _sqlExpressionFactory.ApplyTypeMapping(arguments[2], typeMapping);
			List<SqlExpression> list = new List<SqlExpression> { sqlExpression, item };
			if (arguments.Count == 4)
			{
				list.Add(_sqlExpressionFactory.Fragment($"LANGUAGE {((SqlConstantExpression)arguments[3]).Value}"));
			}
			return _sqlExpressionFactory.Function(value, list, typeof(bool));
		}
		return null;
	}
}
