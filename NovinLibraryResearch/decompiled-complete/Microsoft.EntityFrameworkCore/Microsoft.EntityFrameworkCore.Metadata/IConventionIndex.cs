using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionIndex : IIndex, IAnnotatable, IConventionAnnotatable
{
	IConventionIndexBuilder Builder { get; }

	new IReadOnlyList<IConventionProperty> Properties { get; }

	new IConventionEntityType DeclaringEntityType { get; }

	ConfigurationSource GetConfigurationSource();

	void SetIsUnique(bool? unique, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsUniqueConfigurationSource();
}
