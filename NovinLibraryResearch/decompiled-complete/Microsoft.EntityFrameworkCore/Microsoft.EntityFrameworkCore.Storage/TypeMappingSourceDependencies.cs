using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class TypeMappingSourceDependencies
{
	public IValueConverterSelector ValueConverterSelector { get; }

	public IEnumerable<ITypeMappingSourcePlugin> Plugins { get; }

	[EntityFrameworkInternal]
	public TypeMappingSourceDependencies([JetBrains.Annotations.NotNull] IValueConverterSelector valueConverterSelector, [JetBrains.Annotations.NotNull] IEnumerable<ITypeMappingSourcePlugin> plugins)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(valueConverterSelector, "valueConverterSelector");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(plugins, "plugins");
		ValueConverterSelector = valueConverterSelector;
		Plugins = plugins;
	}

	public TypeMappingSourceDependencies With([JetBrains.Annotations.NotNull] IValueConverterSelector valueConverterSelector)
	{
		return new TypeMappingSourceDependencies(valueConverterSelector, Plugins);
	}

	public TypeMappingSourceDependencies With([JetBrains.Annotations.NotNull] IEnumerable<ITypeMappingSourcePlugin> plugins)
	{
		return new TypeMappingSourceDependencies(ValueConverterSelector, plugins);
	}
}
