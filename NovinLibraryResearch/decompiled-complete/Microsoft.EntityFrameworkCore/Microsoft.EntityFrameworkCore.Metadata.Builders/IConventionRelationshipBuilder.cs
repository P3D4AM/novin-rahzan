using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionRelationshipBuilder : IConventionAnnotatableBuilder
{
	new IConventionForeignKey Metadata { get; }

	IConventionRelationshipBuilder HasEntityTypes([JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, [JetBrains.Annotations.NotNull] IConventionEntityType dependentEntityType, bool fromDataAnnotation = false);

	bool CanInvert([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> newForeignKeyProperties, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	bool CanSetForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false);

	bool CanSetForeignKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	bool CanSetPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false);

	bool CanSetPrincipalKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasNavigation([JetBrains.Annotations.CanBeNull] string name, bool pointsToPrincipal, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasNavigation([JetBrains.Annotations.CanBeNull] MemberInfo property, bool pointsToPrincipal, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasNavigations([JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, [JetBrains.Annotations.CanBeNull] string navigationToDependentName, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasNavigations([JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, bool fromDataAnnotation = false);

	bool CanSetNavigation([JetBrains.Annotations.CanBeNull] string name, bool pointsToPrincipal, bool fromDataAnnotation = false);

	bool CanSetNavigation([JetBrains.Annotations.CanBeNull] MemberInfo property, bool pointsToPrincipal, bool fromDataAnnotation = false);

	bool CanSetNavigations([JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, [JetBrains.Annotations.CanBeNull] string navigationToDependentName, bool fromDataAnnotation = false);

	bool CanSetNavigations([JetBrains.Annotations.CanBeNull] MemberInfo navigationToPrincipal, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToDependent, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasField([JetBrains.Annotations.CanBeNull] string fieldName, bool pointsToPrincipal, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool pointsToPrincipal, bool fromDataAnnotation = false);

	bool CanSetField([JetBrains.Annotations.CanBeNull] string fieldName, bool pointsToPrincipal, bool fromDataAnnotation = false);

	bool CanSetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool pointsToPrincipal, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder IsEagerLoaded(bool? eagerLoaded, bool pointsToPrincipal, bool fromDataAnnotation = false);

	bool CanSetIsEagerLoaded(bool? eagerLoaded, bool pointsToPrincipal, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder IsRequired(bool? required, bool fromDataAnnotation = false);

	bool CanSetIsRequired(bool? required, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder IsOwnership(bool? ownership, bool fromDataAnnotation = false);

	bool CanSetIsOwnership(bool? ownership, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder IsUnique(bool? unique, bool fromDataAnnotation = false);

	bool CanSetIsUnique(bool? unique, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder OnDelete(DeleteBehavior? deleteBehavior, bool fromDataAnnotation = false);

	bool CanSetOnDelete(DeleteBehavior? deleteBehavior, bool fromDataAnnotation = false);
}
