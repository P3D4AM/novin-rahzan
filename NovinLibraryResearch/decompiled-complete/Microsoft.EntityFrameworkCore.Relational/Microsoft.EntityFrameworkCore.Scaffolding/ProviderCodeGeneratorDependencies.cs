using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public sealed class ProviderCodeGeneratorDependencies
{
	public IEnumerable<IProviderCodeGeneratorPlugin> Plugins { get; }

	[EntityFrameworkInternal]
	public ProviderCodeGeneratorDependencies([JetBrains.Annotations.NotNull] IEnumerable<IProviderCodeGeneratorPlugin> plugins)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(plugins, "plugins");
		Plugins = plugins;
	}

	public ProviderCodeGeneratorDependencies With([JetBrains.Annotations.NotNull] IEnumerable<IProviderCodeGeneratorPlugin> plugins)
	{
		return new ProviderCodeGeneratorDependencies(plugins);
	}
}
