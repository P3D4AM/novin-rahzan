using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerStringMethodTranslator : IMethodCallTranslator
{
	private static readonly MethodInfo _indexOfMethodInfo = typeof(string).GetRuntimeMethod("IndexOf", new Type[1] { typeof(string) });

	private static readonly MethodInfo _replaceMethodInfo = typeof(string).GetRuntimeMethod("Replace", new Type[2]
	{
		typeof(string),
		typeof(string)
	});

	private static readonly MethodInfo _toLowerMethodInfo = typeof(string).GetRuntimeMethod("ToLower", Array.Empty<Type>());

	private static readonly MethodInfo _toUpperMethodInfo = typeof(string).GetRuntimeMethod("ToUpper", Array.Empty<Type>());

	private static readonly MethodInfo _substringMethodInfo = typeof(string).GetRuntimeMethod("Substring", new Type[2]
	{
		typeof(int),
		typeof(int)
	});

	private static readonly MethodInfo _isNullOrWhiteSpaceMethodInfo = typeof(string).GetRuntimeMethod("IsNullOrWhiteSpace", new Type[1] { typeof(string) });

	private static readonly MethodInfo _trimStartMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod("TrimStart", Array.Empty<Type>());

	private static readonly MethodInfo _trimEndMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod("TrimEnd", Array.Empty<Type>());

	private static readonly MethodInfo _trimMethodInfoWithoutArgs = typeof(string).GetRuntimeMethod("Trim", Array.Empty<Type>());

	private static readonly MethodInfo _trimStartMethodInfoWithCharArrayArg = typeof(string).GetRuntimeMethod("TrimStart", new Type[1] { typeof(char[]) });

	private static readonly MethodInfo _trimEndMethodInfoWithCharArrayArg = typeof(string).GetRuntimeMethod("TrimEnd", new Type[1] { typeof(char[]) });

	private static readonly MethodInfo _trimMethodInfoWithCharArrayArg = typeof(string).GetRuntimeMethod("Trim", new Type[1] { typeof(char[]) });

	private static readonly MethodInfo _startsWithMethodInfo = typeof(string).GetRuntimeMethod("StartsWith", new Type[1] { typeof(string) });

	private static readonly MethodInfo _containsMethodInfo = typeof(string).GetRuntimeMethod("Contains", new Type[1] { typeof(string) });

	private static readonly MethodInfo _endsWithMethodInfo = typeof(string).GetRuntimeMethod("EndsWith", new Type[1] { typeof(string) });

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	private const char LikeEscapeChar = '\\';

	public SqlServerStringMethodTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (_indexOfMethodInfo.Equals(method))
		{
			SqlExpression sqlExpression = arguments[0];
			RelationalTypeMapping typeMapping = ExpressionExtensions.InferTypeMapping(instance, sqlExpression);
			sqlExpression = _sqlExpressionFactory.ApplyTypeMapping(sqlExpression, typeMapping);
			SqlBinaryExpression elseResult = _sqlExpressionFactory.Subtract(_sqlExpressionFactory.Function("CHARINDEX", new SqlExpression[2]
			{
				sqlExpression,
				_sqlExpressionFactory.ApplyTypeMapping(instance, typeMapping)
			}, method.ReturnType), _sqlExpressionFactory.Constant(1));
			return _sqlExpressionFactory.Case(new CaseWhenClause[1]
			{
				new CaseWhenClause(_sqlExpressionFactory.Equal(sqlExpression, _sqlExpressionFactory.Constant(string.Empty, typeMapping)), _sqlExpressionFactory.Constant(0))
			}, elseResult);
		}
		if (_replaceMethodInfo.Equals(method))
		{
			SqlExpression sqlExpression2 = arguments[0];
			SqlExpression sqlExpression3 = arguments[1];
			RelationalTypeMapping typeMapping2 = ExpressionExtensions.InferTypeMapping(instance, sqlExpression2, sqlExpression3);
			instance = _sqlExpressionFactory.ApplyTypeMapping(instance, typeMapping2);
			sqlExpression2 = _sqlExpressionFactory.ApplyTypeMapping(sqlExpression2, typeMapping2);
			sqlExpression3 = _sqlExpressionFactory.ApplyTypeMapping(sqlExpression3, typeMapping2);
			return _sqlExpressionFactory.Function("REPLACE", new SqlExpression[3] { instance, sqlExpression2, sqlExpression3 }, method.ReturnType, typeMapping2);
		}
		if (_toLowerMethodInfo.Equals(method) || _toUpperMethodInfo.Equals(method))
		{
			return _sqlExpressionFactory.Function(_toLowerMethodInfo.Equals(method) ? "LOWER" : "UPPER", new SqlExpression[1] { instance }, method.ReturnType, instance.TypeMapping);
		}
		if (_substringMethodInfo.Equals(method))
		{
			return _sqlExpressionFactory.Function("SUBSTRING", new SqlExpression[3]
			{
				instance,
				_sqlExpressionFactory.Add(arguments[0], _sqlExpressionFactory.Constant(1)),
				arguments[1]
			}, method.ReturnType, instance.TypeMapping);
		}
		if (_isNullOrWhiteSpaceMethodInfo.Equals(method))
		{
			SqlExpression sqlExpression4 = arguments[0];
			return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.IsNull(sqlExpression4), _sqlExpressionFactory.Equal(_sqlExpressionFactory.Function("LTRIM", new SqlFunctionExpression[1] { _sqlExpressionFactory.Function("RTRIM", new SqlExpression[1] { sqlExpression4 }, sqlExpression4.Type, sqlExpression4.TypeMapping) }, sqlExpression4.Type, sqlExpression4.TypeMapping), _sqlExpressionFactory.Constant(string.Empty, sqlExpression4.TypeMapping)));
		}
		MethodInfo trimStartMethodInfoWithoutArgs = _trimStartMethodInfoWithoutArgs;
		if ((object)trimStartMethodInfoWithoutArgs != null && trimStartMethodInfoWithoutArgs.Equals(method))
		{
			goto IL_0319;
		}
		if (_trimStartMethodInfoWithCharArrayArg.Equals(method))
		{
			Array obj = (arguments[0] as SqlConstantExpression)?.Value as Array;
			if (obj != null && obj.Length == 0)
			{
				goto IL_0319;
			}
		}
		MethodInfo trimEndMethodInfoWithoutArgs = _trimEndMethodInfoWithoutArgs;
		if ((object)trimEndMethodInfoWithoutArgs != null && trimEndMethodInfoWithoutArgs.Equals(method))
		{
			goto IL_038f;
		}
		if (_trimEndMethodInfoWithCharArrayArg.Equals(method))
		{
			Array obj2 = (arguments[0] as SqlConstantExpression)?.Value as Array;
			if (obj2 != null && obj2.Length == 0)
			{
				goto IL_038f;
			}
		}
		MethodInfo trimMethodInfoWithoutArgs = _trimMethodInfoWithoutArgs;
		if ((object)trimMethodInfoWithoutArgs == null || !trimMethodInfoWithoutArgs.Equals(method))
		{
			if (_trimMethodInfoWithCharArrayArg.Equals(method))
			{
				Array obj3 = (arguments[0] as SqlConstantExpression)?.Value as Array;
				if (obj3 != null && obj3.Length == 0)
				{
					goto IL_0405;
				}
			}
			if (_containsMethodInfo.Equals(method))
			{
				SqlExpression sqlExpression5 = arguments[0];
				RelationalTypeMapping typeMapping3 = ExpressionExtensions.InferTypeMapping(instance, sqlExpression5);
				instance = _sqlExpressionFactory.ApplyTypeMapping(instance, typeMapping3);
				sqlExpression5 = _sqlExpressionFactory.ApplyTypeMapping(sqlExpression5, typeMapping3);
				if (sqlExpression5 is SqlConstantExpression sqlConstantExpression)
				{
					if ((string)sqlConstantExpression.Value == string.Empty)
					{
						return _sqlExpressionFactory.Constant(true);
					}
					return _sqlExpressionFactory.GreaterThan(_sqlExpressionFactory.Function("CHARINDEX", new SqlExpression[2] { sqlExpression5, instance }, typeof(int)), _sqlExpressionFactory.Constant(0));
				}
				return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(sqlExpression5, _sqlExpressionFactory.Constant(string.Empty, typeMapping3)), _sqlExpressionFactory.GreaterThan(_sqlExpressionFactory.Function("CHARINDEX", new SqlExpression[2] { sqlExpression5, instance }, typeof(int)), _sqlExpressionFactory.Constant(0)));
			}
			if (_startsWithMethodInfo.Equals(method))
			{
				return TranslateStartsEndsWith(instance, arguments[0], startsWith: true);
			}
			if (_endsWithMethodInfo.Equals(method))
			{
				return TranslateStartsEndsWith(instance, arguments[0], startsWith: false);
			}
			return null;
		}
		goto IL_0405;
		IL_0319:
		return _sqlExpressionFactory.Function("LTRIM", new SqlExpression[1] { instance }, instance.Type, instance.TypeMapping);
		IL_0405:
		return _sqlExpressionFactory.Function("LTRIM", new SqlFunctionExpression[1] { _sqlExpressionFactory.Function("RTRIM", new SqlExpression[1] { instance }, instance.Type, instance.TypeMapping) }, instance.Type, instance.TypeMapping);
		IL_038f:
		return _sqlExpressionFactory.Function("RTRIM", new SqlExpression[1] { instance }, instance.Type, instance.TypeMapping);
	}

	private SqlExpression TranslateStartsEndsWith(SqlExpression instance, SqlExpression pattern, bool startsWith)
	{
		RelationalTypeMapping typeMapping = ExpressionExtensions.InferTypeMapping(instance, pattern);
		instance = _sqlExpressionFactory.ApplyTypeMapping(instance, typeMapping);
		pattern = _sqlExpressionFactory.ApplyTypeMapping(pattern, typeMapping);
		if (pattern is SqlConstantExpression sqlConstantExpression)
		{
			if (!(sqlConstantExpression.Value is string text))
			{
				return _sqlExpressionFactory.Like(instance, _sqlExpressionFactory.Constant(null, typeMapping));
			}
			if (!text.Any((char c) => IsLikeWildChar(c)))
			{
				return _sqlExpressionFactory.Like(instance, _sqlExpressionFactory.Constant(startsWith ? (text + "%") : ("%" + text)));
			}
			return _sqlExpressionFactory.Like(instance, _sqlExpressionFactory.Constant(startsWith ? (EscapeLikePattern(text) + "%") : ("%" + EscapeLikePattern(text))), _sqlExpressionFactory.Constant('\\'.ToString()));
		}
		if (startsWith)
		{
			return _sqlExpressionFactory.Equal(_sqlExpressionFactory.Function("LEFT", new SqlExpression[2]
			{
				instance,
				_sqlExpressionFactory.Function("LEN", new SqlExpression[1] { pattern }, typeof(int))
			}, typeof(string), typeMapping), pattern);
		}
		return _sqlExpressionFactory.Equal(_sqlExpressionFactory.Function("RIGHT", new SqlExpression[2]
		{
			instance,
			_sqlExpressionFactory.Function("LEN", new SqlExpression[1] { pattern }, typeof(int))
		}, typeof(string), typeMapping), pattern);
	}

	private bool IsLikeWildChar(char c)
	{
		if (c != '%' && c != '_')
		{
			return c == '[';
		}
		return true;
	}

	private string EscapeLikePattern(string pattern)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in pattern)
		{
			if (IsLikeWildChar(c) || c == '\\')
			{
				stringBuilder.Append('\\');
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}
}
