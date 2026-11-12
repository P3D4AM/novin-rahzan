using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalMemberTranslatorProviderDependencies
{
	public ISqlExpressionFactory SqlExpressionFactory { get; }

	public IEnumerable<IMemberTranslatorPlugin> Plugins { get; }

	[EntityFrameworkInternal]
	public RelationalMemberTranslatorProviderDependencies([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory, [JetBrains.Annotations.NotNull] IEnumerable<IMemberTranslatorPlugin> plugins)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlExpressionFactory, "sqlExpressionFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(plugins, "plugins");
		SqlExpressionFactory = sqlExpressionFactory;
		Plugins = plugins;
	}

	public RelationalMemberTranslatorProviderDependencies With([JetBrains.Annotations.NotNull] ISqlExpressionFactory sqlExpressionFactory)
	{
		return new RelationalMemberTranslatorProviderDependencies(sqlExpressionFactory, Plugins);
	}

	public RelationalMemberTranslatorProviderDependencies With([JetBrains.Annotations.NotNull] IEnumerable<IMemberTranslatorPlugin> plugins)
	{
		return new RelationalMemberTranslatorProviderDependencies(SqlExpressionFactory, plugins);
	}
}
