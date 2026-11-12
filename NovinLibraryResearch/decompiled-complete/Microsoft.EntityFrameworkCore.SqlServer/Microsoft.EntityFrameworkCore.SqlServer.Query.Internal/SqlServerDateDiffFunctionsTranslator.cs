using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerDateDiffFunctionsTranslator : IMethodCallTranslator
{
	private readonly Dictionary<MethodInfo, string> _methodInfoDateDiffMapping = new Dictionary<MethodInfo, string>
	{
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"YEAR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"YEAR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"YEAR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffYear", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"YEAR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"MONTH"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"MONTH"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"MONTH"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMonth", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"MONTH"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"DAY"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"DAY"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"DAY"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffDay", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"DAY"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"HOUR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"HOUR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"HOUR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"HOUR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan),
				typeof(TimeSpan)
			}),
			"HOUR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffHour", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan?),
				typeof(TimeSpan?)
			}),
			"HOUR"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"MINUTE"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"MINUTE"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"MINUTE"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"MINUTE"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan),
				typeof(TimeSpan)
			}),
			"MINUTE"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMinute", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan?),
				typeof(TimeSpan?)
			}),
			"MINUTE"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"SECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"SECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"SECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"SECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan),
				typeof(TimeSpan)
			}),
			"SECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffSecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan?),
				typeof(TimeSpan?)
			}),
			"SECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"MILLISECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"MILLISECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"MILLISECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"MILLISECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan),
				typeof(TimeSpan)
			}),
			"MILLISECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMillisecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan?),
				typeof(TimeSpan?)
			}),
			"MILLISECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"MICROSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"MICROSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"MICROSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"MICROSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan),
				typeof(TimeSpan)
			}),
			"MICROSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffMicrosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan?),
				typeof(TimeSpan?)
			}),
			"MICROSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime),
				typeof(DateTime)
			}),
			"NANOSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTime?),
				typeof(DateTime?)
			}),
			"NANOSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset),
				typeof(DateTimeOffset)
			}),
			"NANOSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(DateTimeOffset?),
				typeof(DateTimeOffset?)
			}),
			"NANOSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan),
				typeof(TimeSpan)
			}),
			"NANOSECOND"
		},
		{
			typeof(SqlServerDbFunctionsExtensions).GetRuntimeMethod("DateDiffNanosecond", new Type[3]
			{
				typeof(DbFunctions),
				typeof(TimeSpan?),
				typeof(TimeSpan?)
			}),
			"NANOSECOND"
		}
	};

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerDateDiffFunctionsTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		if (_methodInfoDateDiffMapping.TryGetValue(method, out var value))
		{
			SqlExpression sqlExpression = arguments[1];
			SqlExpression sqlExpression2 = arguments[2];
			RelationalTypeMapping typeMapping = ExpressionExtensions.InferTypeMapping(sqlExpression, sqlExpression2);
			sqlExpression = _sqlExpressionFactory.ApplyTypeMapping(sqlExpression, typeMapping);
			sqlExpression2 = _sqlExpressionFactory.ApplyTypeMapping(sqlExpression2, typeMapping);
			return _sqlExpressionFactory.Function("DATEDIFF", new SqlExpression[3]
			{
				_sqlExpressionFactory.Fragment(value),
				sqlExpression,
				sqlExpression2
			}, typeof(int));
		}
		return null;
	}
}
