using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionModel : IModel, IAnnotatable, IConventionAnnotatable
{
	IConventionModelBuilder Builder { get; }

	IConventionEntityType AddEntityType([JetBrains.Annotations.NotNull] string name, bool fromDataAnnotation = false);

	IConventionEntityType AddEntityType([JetBrains.Annotations.NotNull] Type clrType, bool fromDataAnnotation = false);

	IConventionEntityType AddEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType, bool fromDataAnnotation = false);

	IConventionEntityType AddEntityType([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType, bool fromDataAnnotation = false);

	new IConventionEntityType FindEntityType([JetBrains.Annotations.NotNull] string name);

	IConventionEntityType FindEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType);

	void RemoveEntityType([JetBrains.Annotations.NotNull] IConventionEntityType entityType);

	new IEnumerable<IConventionEntityType> GetEntityTypes();

	void AddIgnored([JetBrains.Annotations.NotNull] string typeName, bool fromDataAnnotation = false);

	void RemoveIgnored([JetBrains.Annotations.NotNull] string typeName);

	ConfigurationSource? FindIgnoredConfigurationSource([JetBrains.Annotations.NotNull] string typeName);
}
