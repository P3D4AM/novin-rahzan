using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RequiredNavigationAttributeConvention : NavigationAttributeConventionBase<RequiredAttribute>
{
	public RequiredNavigationAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	public override void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, RequiredAttribute attribute, IConventionContext<IConventionNavigation> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationshipBuilder, "relationshipBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigation, "navigation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull<RequiredAttribute>(attribute, "attribute");
		if (navigation.IsCollection())
		{
			Dependencies.Logger.RequiredAttributeOnCollection(navigation.ForeignKey.DependentToPrincipal);
			return;
		}
		if (!navigation.IsDependentToPrincipal())
		{
			IConventionNavigation conventionNavigation = navigation.FindInverse();
			if (conventionNavigation != null && NavigationAttributeConventionBase<RequiredAttribute>.GetAttributes<RequiredAttribute>(conventionNavigation.DeclaringEntityType, conventionNavigation).Any())
			{
				Dependencies.Logger.RequiredAttributeOnBothNavigations(navigation, conventionNavigation);
				return;
			}
			if (relationshipBuilder.Metadata.GetPrincipalEndConfigurationSource().HasValue)
			{
				Dependencies.Logger.RequiredAttributeOnDependent(navigation.ForeignKey.PrincipalToDependent);
				return;
			}
			IConventionRelationshipBuilder conventionRelationshipBuilder = relationshipBuilder.HasEntityTypes(relationshipBuilder.Metadata.DeclaringEntityType, relationshipBuilder.Metadata.PrincipalEntityType);
			if (conventionRelationshipBuilder == null)
			{
				return;
			}
			Dependencies.Logger.RequiredAttributeInverted(conventionRelationshipBuilder.Metadata.DependentToPrincipal);
			relationshipBuilder = conventionRelationshipBuilder;
		}
		relationshipBuilder.IsRequired(true, fromDataAnnotation: true);
		context.StopProcessingIfChanged(relationshipBuilder.Metadata.DependentToPrincipal);
	}
}
