using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IMemberTranslatorProvider
{
	SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType);
}
