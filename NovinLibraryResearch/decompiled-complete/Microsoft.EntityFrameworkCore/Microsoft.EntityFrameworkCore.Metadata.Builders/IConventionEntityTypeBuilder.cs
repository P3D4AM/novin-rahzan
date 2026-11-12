using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionEntityTypeBuilder : IConventionAnnotatableBuilder
{
	new IConventionEntityType Metadata { get; }

	IConventionEntityTypeBuilder HasBaseType([JetBrains.Annotations.CanBeNull] IConventionEntityType baseEntityType, bool fromDataAnnotation = false);

	bool CanSetBaseType([JetBrains.Annotations.CanBeNull] IConventionEntityType baseEntityType, bool fromDataAnnotation = false);

	IConventionPropertyBuilder Property([JetBrains.Annotations.NotNull] Type propertyType, [JetBrains.Annotations.NotNull] string propertyName, bool setTypeConfigurationSource = true, bool fromDataAnnotation = false);

	IConventionPropertyBuilder Property([JetBrains.Annotations.NotNull] MemberInfo memberInfo, bool fromDataAnnotation = false);

	IReadOnlyList<IConventionProperty> GetOrCreateProperties([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, bool fromDataAnnotation = false);

	IReadOnlyList<IConventionProperty> GetOrCreateProperties([JetBrains.Annotations.CanBeNull] IEnumerable<MemberInfo> memberInfos, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder RemoveUnusedShadowProperties([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionServicePropertyBuilder ServiceProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo, bool fromDataAnnotation = false);

	bool IsIgnored([JetBrains.Annotations.NotNull] string memberName, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder Ignore([JetBrains.Annotations.NotNull] string memberName, bool fromDataAnnotation = false);

	bool CanIgnore([JetBrains.Annotations.NotNull] string memberName, bool fromDataAnnotation = false);

	IConventionKeyBuilder PrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	bool CanSetPrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionKeyBuilder HasKey([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoKey([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoKey([JetBrains.Annotations.NotNull] IConventionKey key, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoKey(bool fromDataAnnotation = false);

	IConventionIndexBuilder HasIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoIndex([JetBrains.Annotations.NotNull] IConventionIndex index, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType targetEntityType, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> dependentProperties, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, [JetBrains.Annotations.NotNull] IConventionKey principalKey, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> dependentProperties, [JetBrains.Annotations.NotNull] IConventionKey principalKey, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType targetEntityType, [JetBrains.Annotations.NotNull] string navigationToTargetName, bool setTargetAsPrincipal = false, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType targetEntityType, [JetBrains.Annotations.NotNull] MemberInfo navigationToTarget, bool setTargetAsPrincipal = false, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType targetEntityType, [JetBrains.Annotations.NotNull] string navigationToTargetName, [JetBrains.Annotations.CanBeNull] string inverseNavigationName, bool setTargetAsPrincipal = false, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] IConventionEntityType targetEntityType, [JetBrains.Annotations.NotNull] MemberInfo navigationToTarget, [JetBrains.Annotations.CanBeNull] MemberInfo inverseNavigation, bool setTargetAsPrincipal = false, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] string navigationToTargetName, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] MemberInfo navigationToTarget, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] string navigationToTargetName, [JetBrains.Annotations.CanBeNull] string inverseNavigationName, bool fromDataAnnotation = false);

	IConventionRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] MemberInfo navigationToTarget, [JetBrains.Annotations.CanBeNull] MemberInfo inverseNavigation, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoRelationship([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, [JetBrains.Annotations.NotNull] IConventionKey principalKey, [JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoRelationship([JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey, bool fromDataAnnotation = false);

	bool CanAddNavigation([JetBrains.Annotations.NotNull] string navigationName, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression filter, bool fromDataAnnotation = false);

	bool CanSetQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression filter, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasDefiningQuery([JetBrains.Annotations.CanBeNull] LambdaExpression query, bool fromDataAnnotation = false);

	bool CanSetDefiningQuery([JetBrains.Annotations.CanBeNull] LambdaExpression query, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false);

	bool CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false);

	bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation = false);

	IConventionDiscriminatorBuilder HasDiscriminator(bool fromDataAnnotation = false);

	IConventionDiscriminatorBuilder HasDiscriminator([JetBrains.Annotations.NotNull] Type type, bool fromDataAnnotation = false);

	IConventionDiscriminatorBuilder HasDiscriminator([JetBrains.Annotations.NotNull] string name, bool fromDataAnnotation = false);

	IConventionDiscriminatorBuilder HasDiscriminator([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type type, bool fromDataAnnotation = false);

	IConventionDiscriminatorBuilder HasDiscriminator([JetBrains.Annotations.NotNull] MemberInfo memberInfo, bool fromDataAnnotation = false);

	IConventionEntityTypeBuilder HasNoDeclaredDiscriminator(bool fromDataAnnotation = false);

	bool CanSetDiscriminator([JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false);

	bool CanSetDiscriminator([JetBrains.Annotations.CanBeNull] Type type, bool fromDataAnnotation = false);

	bool CanSetDiscriminator([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string name, bool fromDataAnnotation = false);
}
