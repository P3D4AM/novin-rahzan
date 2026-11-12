using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

public class RuntimeConventionSetBuilder : IConventionSetBuilder
{
	private readonly IProviderConventionSetBuilder _conventionSetBuilder;

	private readonly IList<IConventionSetPlugin> _plugins;

	public RuntimeConventionSetBuilder([JetBrains.Annotations.NotNull] IProviderConventionSetBuilder providerConventionSetBuilder, [JetBrains.Annotations.NotNull] IEnumerable<IConventionSetPlugin> plugins)
	{
		_conventionSetBuilder = providerConventionSetBuilder;
		_plugins = plugins.ToList();
	}

	public virtual ConventionSet CreateConventionSet()
	{
		ConventionSet conventionSet = _conventionSetBuilder.CreateConventionSet();
		foreach (IConventionSetPlugin plugin in _plugins)
		{
			conventionSet = plugin.ModifyConventions(conventionSet);
		}
		return conventionSet;
	}
}
