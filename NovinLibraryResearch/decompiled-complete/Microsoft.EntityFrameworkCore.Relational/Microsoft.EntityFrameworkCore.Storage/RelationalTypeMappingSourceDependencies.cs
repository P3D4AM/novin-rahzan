using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class RelationalTypeMappingSourceDependencies
{
	public IEnumerable<IRelationalTypeMappingSourcePlugin> Plugins { get; }

	[EntityFrameworkInternal]
	public RelationalTypeMappingSourceDependencies([JetBrains.Annotations.NotNull] IEnumerable<IRelationalTypeMappingSourcePlugin> plugins)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(plugins, "plugins");
		Plugins = plugins;
	}

	public RelationalTypeMappingSourceDependencies With([JetBrains.Annotations.NotNull] IEnumerable<IRelationalTypeMappingSourcePlugin> plugins)
	{
		return new RelationalTypeMappingSourceDependencies(plugins);
	}
}
