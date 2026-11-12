using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class NonNullableNavigationConvention : NonNullableConventionBase, INavigationAddedConvention, IConvention
{
	public NonNullableNavigationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	public virtual void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, IConventionContext<IConventionNavigation> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationshipBuilder, "relationshipBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		IConventionModelBuilder modelBuilder = relationshipBuilder.ModelBuilder;
		if (!IsNonNullable(modelBuilder, navigation) || navigation.IsCollection())
		{
			return;
		}
		if (!navigation.IsDependentToPrincipal())
		{
			IConventionNavigation conventionNavigation = navigation.FindInverse();
			if (conventionNavigation != null && IsNonNullable(modelBuilder, conventionNavigation))
			{
				Dependencies.Logger.NonNullableReferenceOnBothNavigations(navigation, conventionNavigation);
				return;
			}
			if (!navigation.ForeignKey.IsUnique || relationshipBuilder.Metadata.GetPrincipalEndConfigurationSource().HasValue)
			{
				Dependencies.Logger.NonNullableReferenceOnDependent(navigation.ForeignKey.PrincipalToDependent);
				return;
			}
			IConventionRelationshipBuilder conventionRelationshipBuilder = relationshipBuilder.HasEntityTypes(relationshipBuilder.Metadata.DeclaringEntityType, relationshipBuilder.Metadata.PrincipalEntityType);
			if (conventionRelationshipBuilder == null)
			{
				return;
			}
			Dependencies.Logger.NonNullableInverted(conventionRelationshipBuilder.Metadata.DependentToPrincipal);
			relationshipBuilder = conventionRelationshipBuilder;
		}
		relationshipBuilder.IsRequired(true);
		context.StopProcessingIfChanged(relationshipBuilder.Metadata.DependentToPrincipal);
	}

	private bool IsNonNullable(IConventionModelBuilder modelBuilder, IConventionNavigation navigation)
	{
		if (navigation.DeclaringEntityType.HasClrType())
		{
			PropertyInfo propertyInfo = navigation.DeclaringEntityType.GetRuntimeProperties().Find(navigation.Name);
			if ((object)propertyInfo != null)
			{
				return IsNonNullableReferenceType(modelBuilder, propertyInfo);
			}
		}
		return false;
	}
}
