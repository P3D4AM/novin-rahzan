using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionServiceProperty : IServiceProperty, IPropertyBase, IAnnotatable, IConventionPropertyBase, IConventionAnnotatable
{
	IConventionServicePropertyBuilder Builder { get; }

	new IConventionEntityType DeclaringEntityType { get; }

	ConfigurationSource GetConfigurationSource();

	void SetParameterBinding([JetBrains.Annotations.CanBeNull] ServiceParameterBinding parameterBinding, bool fromDataAnnotation = false);

	ConfigurationSource? GetParameterBindingConfigurationSource();
}
