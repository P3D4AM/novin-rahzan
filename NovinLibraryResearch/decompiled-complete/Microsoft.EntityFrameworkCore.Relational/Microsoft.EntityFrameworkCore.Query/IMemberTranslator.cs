using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IMemberTranslator
{
	SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType);
}
