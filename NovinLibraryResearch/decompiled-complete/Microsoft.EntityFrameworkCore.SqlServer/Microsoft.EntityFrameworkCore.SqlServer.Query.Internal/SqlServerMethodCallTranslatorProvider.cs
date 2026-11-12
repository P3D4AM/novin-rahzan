using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider
{
	public SqlServerMethodCallTranslatorProvider([JetBrains.Annotations.NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies)
		: base(dependencies)
	{
		ISqlExpressionFactory sqlExpressionFactory = dependencies.SqlExpressionFactory;
		AddTranslators(new IMethodCallTranslator[9]
		{
			new SqlServerConvertTranslator(sqlExpressionFactory),
			new SqlServerDateTimeMethodTranslator(sqlExpressionFactory),
			new SqlServerDateDiffFunctionsTranslator(sqlExpressionFactory),
			new SqlServerFullTextSearchFunctionsTranslator(sqlExpressionFactory),
			new SqlServerIsDateFunctionTranslator(sqlExpressionFactory),
			new SqlServerMathTranslator(sqlExpressionFactory),
			new SqlServerNewGuidTranslator(sqlExpressionFactory),
			new SqlServerObjectToStringTranslator(sqlExpressionFactory),
			new SqlServerStringMethodTranslator(sqlExpressionFactory)
		});
	}
}
