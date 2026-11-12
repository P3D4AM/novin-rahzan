using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionProperty : IProperty, IPropertyBase, IAnnotatable, IConventionPropertyBase, IConventionAnnotatable
{
	IConventionPropertyBuilder Builder { get; }

	new IConventionEntityType DeclaringEntityType { get; }

	ConfigurationSource GetConfigurationSource();

	ConfigurationSource? GetTypeConfigurationSource();

	void SetIsNullable(bool? nullable, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsNullableConfigurationSource();

	void SetValueGenerated(ValueGenerated? valueGenerated, bool fromDataAnnotation = false);

	ConfigurationSource? GetValueGeneratedConfigurationSource();

	void SetIsConcurrencyToken(bool? concurrencyToken, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsConcurrencyTokenConfigurationSource();
}
