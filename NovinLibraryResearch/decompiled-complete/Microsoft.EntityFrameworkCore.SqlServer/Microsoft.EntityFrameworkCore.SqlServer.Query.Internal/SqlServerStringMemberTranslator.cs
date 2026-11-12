using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerStringMemberTranslator : IMemberTranslator
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public SqlServerStringMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType)
	{
		if (member.Name == "Length" && instance?.Type == typeof(string))
		{
			return _sqlExpressionFactory.Convert(_sqlExpressionFactory.Function("LEN", new SqlExpression[1] { instance }, typeof(long)), returnType);
		}
		return null;
	}
}
