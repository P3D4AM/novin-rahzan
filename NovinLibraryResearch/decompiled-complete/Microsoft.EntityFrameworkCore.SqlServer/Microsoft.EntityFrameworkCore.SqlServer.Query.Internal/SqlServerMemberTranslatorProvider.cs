using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerMemberTranslatorProvider : RelationalMemberTranslatorProvider
{
	public SqlServerMemberTranslatorProvider([JetBrains.Annotations.NotNull] RelationalMemberTranslatorProviderDependencies dependencies)
		: base(dependencies)
	{
		ISqlExpressionFactory sqlExpressionFactory = dependencies.SqlExpressionFactory;
		AddTranslators(new IMemberTranslator[2]
		{
			new SqlServerDateTimeMemberTranslator(sqlExpressionFactory),
			new SqlServerStringMemberTranslator(sqlExpressionFactory)
		});
	}
}
