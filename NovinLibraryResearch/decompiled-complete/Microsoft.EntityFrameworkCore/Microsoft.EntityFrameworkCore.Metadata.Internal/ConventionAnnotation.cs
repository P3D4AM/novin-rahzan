using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ConventionAnnotation : Annotation, IConventionAnnotation, IAnnotation
{
	private ConfigurationSource _configurationSource;

	public ConventionAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
		: base(name, value)
	{
		_configurationSource = configurationSource;
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual ConfigurationSource UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		return _configurationSource = configurationSource.Max(_configurationSource);
	}
}
