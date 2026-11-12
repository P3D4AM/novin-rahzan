using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerDateTimeMemberTranslator : IMemberTranslator
{
	private static readonly Dictionary<string, string> _datePartMapping = new Dictionary<string, string>
	{
		{ "Year", "year" },
		{ "Month", "month" },
		{ "DayOfYear", "dayofyear" },
		{ "Day", "day" },
		{ "Hour", "hour" },
		{ "Minute", "minute" },
		{ "Second", "second" },
		{ "Millisecond", "millisecond" }
	};

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerDateTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType)
	{
		Type declaringType = member.DeclaringType;
		if (declaringType == typeof(DateTime) || declaringType == typeof(DateTimeOffset))
		{
			string name = member.Name;
			if (_datePartMapping.TryGetValue(name, out var value))
			{
				return _sqlExpressionFactory.Function("DATEPART", new SqlExpression[2]
				{
					_sqlExpressionFactory.Fragment(value),
					instance
				}, returnType);
			}
			switch (name)
			{
			case "Date":
			{
				if (AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19052", out var isEnabled) && isEnabled)
				{
					return _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
					{
						_sqlExpressionFactory.Fragment("date"),
						instance
					}, returnType, instance.TypeMapping);
				}
				return _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
				{
					_sqlExpressionFactory.Fragment("date"),
					instance
				}, returnType, (declaringType == typeof(DateTime)) ? instance.TypeMapping : _sqlExpressionFactory.FindMapping(typeof(DateTime)));
			}
			case "TimeOfDay":
				return _sqlExpressionFactory.Convert(instance, returnType);
			case "Now":
				return _sqlExpressionFactory.Function((declaringType == typeof(DateTime)) ? "GETDATE" : "SYSDATETIMEOFFSET", Array.Empty<SqlExpression>(), returnType);
			case "UtcNow":
			{
				SqlFunctionExpression sqlFunctionExpression = _sqlExpressionFactory.Function((declaringType == typeof(DateTime)) ? "GETUTCDATE" : "SYSUTCDATETIME", Array.Empty<SqlExpression>(), returnType);
				if (!(declaringType == typeof(DateTime)))
				{
					return _sqlExpressionFactory.Convert(sqlFunctionExpression, returnType);
				}
				return sqlFunctionExpression;
			}
			case "Today":
				return _sqlExpressionFactory.Function("CONVERT", new SqlExpression[2]
				{
					_sqlExpressionFactory.Fragment("date"),
					_sqlExpressionFactory.Function("GETDATE", Array.Empty<SqlExpression>(), typeof(DateTime))
				}, returnType);
			}
		}
		return null;
	}
}
