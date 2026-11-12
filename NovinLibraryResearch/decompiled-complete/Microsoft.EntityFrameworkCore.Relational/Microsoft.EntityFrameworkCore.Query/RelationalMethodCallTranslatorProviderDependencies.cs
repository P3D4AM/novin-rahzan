using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalMethodCallTranslatorProviderDependencies
{
	public ISqlExpressionFactory SqlExpressionFactory { get; }

	public IEnumerable<IMethodCallTranslatorPlugin> Plugins { get; }

	[EntityFrameworkInternal]
	public RelationalMethodCallTranslatorProviderDependencies([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory, [JetBrains.Annotations.NotNull] IEnumerable<IMethodCallTranslatorPlugin> plugins)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlExpressionFactory, "sqlExpressionFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(plugins, "plugins");
		SqlExpressionFactory = sqlExpressionFactory;
		Plugins = plugins;
	}

	public RelationalMethodCallTranslatorProviderDependencies With([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory)
	{
		return new RelationalMethodCallTranslatorProviderDependencies(sqlExpressionFactory, Plugins);
	}

	public RelationalMethodCallTranslatorProviderDependencies With([JetBrains.Annotations.NotNull] IEnumerable<IMethodCallTranslatorPlugin> plugins)
	{
		return new RelationalMethodCallTranslatorProviderDependencies(SqlExpressionFactory, plugins);
	}
}
