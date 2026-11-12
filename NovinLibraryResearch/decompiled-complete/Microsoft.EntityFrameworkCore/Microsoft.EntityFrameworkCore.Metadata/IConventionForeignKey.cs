using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionForeignKey : IForeignKey, IAnnotatable, IConventionAnnotatable
{
	IConventionRelationshipBuilder Builder { get; }

	new IReadOnlyList<IConventionProperty> Properties { get; }

	new IConventionKey PrincipalKey { get; }

	new IConventionEntityType DeclaringEntityType { get; }

	new IConventionEntityType PrincipalEntityType { get; }

	new IConventionNavigation DependentToPrincipal { get; }

	new IConventionNavigation PrincipalToDependent { get; }

	ConfigurationSource GetConfigurationSource();

	void SetProperties([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, [JetBrains.Annotations.NotNull] IConventionKey principalKey, bool fromDataAnnotation = false);

	ConfigurationSource? GetPropertiesConfigurationSource();

	ConfigurationSource? GetPrincipalKeyConfigurationSource();

	ConfigurationSource? GetPrincipalEndConfigurationSource();

	void SetIsUnique(bool? unique, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsUniqueConfigurationSource();

	void SetIsRequired(bool? required, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsRequiredConfigurationSource();

	void SetIsOwnership(bool? ownership, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsOwnershipConfigurationSource();

	void SetDeleteBehavior(DeleteBehavior? deleteBehavior, bool fromDataAnnotation = false);

	ConfigurationSource? GetDeleteBehaviorConfigurationSource();

	IConventionNavigation HasDependentToPrincipal([JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false);

	IConventionNavigation HasDependentToPrincipal([JetBrains.Annotations.CanBeNull] MemberInfo property, bool fromDataAnnotation = false);

	ConfigurationSource? GetDependentToPrincipalConfigurationSource();

	IConventionNavigation HasPrincipalToDependent([JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false);

	IConventionNavigation HasPrincipalToDependent([JetBrains.Annotations.CanBeNull] MemberInfo property, bool fromDataAnnotation = false);

	ConfigurationSource? GetPrincipalToDependentConfigurationSource();
}
