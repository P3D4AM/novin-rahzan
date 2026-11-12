using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionModelBuilder : IConventionAnnotatableBuilder
{
	new IConventionModel Metadata { get; }

	IConventionEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] string name, bool? shouldBeOwned = false, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] Type type, bool? shouldBeOwned = false, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder Entity([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IConventionEntityType definingEntityType, bool fromDataAnnotation = false);

	IConventionOwnedEntityTypeBuilder Owned([JetBrains.Annotations.NotNull] Type type, bool fromDataAnnotation = false);

	bool IsIgnored([JetBrains.Annotations.NotNull] Type type, bool fromDataAnnotation = false);

	bool IsIgnored([JetBrains.Annotations.NotNull] string typeName, bool fromDataAnnotation = false);

	IConventionModelBuilder Ignore([JetBrains.Annotations.NotNull] Type type, bool fromDataAnnotation = false);

	IConventionModelBuilder Ignore([JetBrains.Annotations.NotNull] string typeName, bool fromDataAnnotation = false);

	IConventionModelBuilder HasNoEntityType([JetBrains.Annotations.NotNull] IConventionEntityType entityType, bool fromDataAnnotation = false);

	bool CanIgnore([JetBrains.Annotations.NotNull] Type type, bool fromDataAnnotation = false);

	bool CanIgnore([JetBrains.Annotations.NotNull] string typeName, bool fromDataAnnotation = false);

	IConventionModelBuilder HasChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false);

	bool CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false);

	IConventionModelBuilder UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false);

	bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false);
}
