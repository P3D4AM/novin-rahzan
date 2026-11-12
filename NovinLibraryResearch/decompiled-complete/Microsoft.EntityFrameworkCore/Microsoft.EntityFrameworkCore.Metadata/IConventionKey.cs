using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionKey : IConventionAnnotatable, IAnnotatable, IKey
{
	IConventionKeyBuilder Builder { get; }

	new IReadOnlyList<IConventionProperty> Properties { get; }

	new IConventionEntityType DeclaringEntityType { get; }

	ConfigurationSource GetConfigurationSource();
}
