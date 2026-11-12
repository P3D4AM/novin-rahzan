using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalSqlTranslatingExpressionVisitorDependencies
{
	public ISqlExpressionFactory SqlExpressionFactory { get; }

	public IMemberTranslatorProvider MemberTranslatorProvider { get; }

	public IMethodCallTranslatorProvider MethodCallTranslatorProvider { get; }

	[EntityFrameworkInternal]
	public RelationalSqlTranslatingExpressionVisitorDependencies([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory, [JetBrains.Annotations.NotNull] IMemberTranslatorProvider memberTranslatorProvider, [JetBrains.Annotations.NotNull] IMethodCallTranslatorProvider methodCallTranslatorProvider)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlExpressionFactory, "sqlExpressionFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberTranslatorProvider, "memberTranslatorProvider");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodCallTranslatorProvider, "methodCallTranslatorProvider");
		SqlExpressionFactory = sqlExpressionFactory;
		MemberTranslatorProvider = memberTranslatorProvider;
		MethodCallTranslatorProvider = methodCallTranslatorProvider;
	}

	public RelationalSqlTranslatingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory)
	{
		return new RelationalSqlTranslatingExpressionVisitorDependencies(sqlExpressionFactory, MemberTranslatorProvider, MethodCallTranslatorProvider);
	}

	public RelationalSqlTranslatingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] IMemberTranslatorProvider memberTranslatorProvider)
	{
		return new RelationalSqlTranslatingExpressionVisitorDependencies(SqlExpressionFactory, memberTranslatorProvider, MethodCallTranslatorProvider);
	}

	public RelationalSqlTranslatingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] IMethodCallTranslatorProvider methodCallTranslatorProvider)
	{
		return new RelationalSqlTranslatingExpressionVisitorDependencies(SqlExpressionFactory, MemberTranslatorProvider, methodCallTranslatorProvider);
	}
}
