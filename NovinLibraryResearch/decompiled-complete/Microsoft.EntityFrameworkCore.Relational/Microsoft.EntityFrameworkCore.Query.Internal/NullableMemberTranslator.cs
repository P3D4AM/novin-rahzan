using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class NullableMemberTranslator : IMemberTranslator
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public NullableMemberTranslator(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType)
	{
		if (member.DeclaringType.IsNullableValueType())
		{
			switch (member.Name)
			{
			case "Value":
				return instance;
			case "HasValue":
				return _sqlExpressionFactory.IsNotNull(instance);
			}
		}
		return null;
	}
}
