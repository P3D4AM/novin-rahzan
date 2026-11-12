using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

public class ProviderConventionSetBuilder : IProviderConventionSetBuilder
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ProviderConventionSetBuilder([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual ConventionSet CreateConventionSet()
	{
		ConventionSet conventionSet = new ConventionSet();
		PropertyDiscoveryConvention item = new PropertyDiscoveryConvention(Dependencies);
		KeyDiscoveryConvention item2 = new KeyDiscoveryConvention(Dependencies);
		InversePropertyAttributeConvention item3 = new InversePropertyAttributeConvention(Dependencies);
		RelationshipDiscoveryConvention item4 = new RelationshipDiscoveryConvention(Dependencies);
		ServicePropertyDiscoveryConvention item5 = new ServicePropertyDiscoveryConvention(Dependencies);
		conventionSet.EntityTypeAddedConventions.Add(new NotMappedEntityTypeAttributeConvention(Dependencies));
		conventionSet.EntityTypeAddedConventions.Add(new OwnedEntityTypeAttributeConvention(Dependencies));
		conventionSet.EntityTypeAddedConventions.Add(new NotMappedMemberAttributeConvention(Dependencies));
		conventionSet.EntityTypeAddedConventions.Add(new BaseTypeDiscoveryConvention(Dependencies));
		conventionSet.EntityTypeAddedConventions.Add(item);
		conventionSet.EntityTypeAddedConventions.Add(item5);
		conventionSet.EntityTypeAddedConventions.Add(item2);
		conventionSet.EntityTypeAddedConventions.Add(item3);
		conventionSet.EntityTypeAddedConventions.Add(item4);
		conventionSet.EntityTypeAddedConventions.Add(new DerivedTypeDiscoveryConvention(Dependencies));
		conventionSet.EntityTypeIgnoredConventions.Add(item3);
		conventionSet.EntityTypeIgnoredConventions.Add(item4);
		DiscriminatorConvention item6 = new DiscriminatorConvention(Dependencies);
		conventionSet.EntityTypeRemovedConventions.Add(new OwnedTypesConvention(Dependencies));
		conventionSet.EntityTypeRemovedConventions.Add(item6);
		ForeignKeyIndexConvention item7 = new ForeignKeyIndexConvention(Dependencies);
		ValueGenerationConvention item8 = new ValueGenerationConvention(Dependencies);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item5);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item2);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item3);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item4);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item7);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item8);
		conventionSet.EntityTypeBaseTypeChangedConventions.Add(item6);
		ForeignKeyPropertyDiscoveryConvention item9 = new ForeignKeyPropertyDiscoveryConvention(Dependencies);
		conventionSet.EntityTypeMemberIgnoredConventions.Add(item3);
		conventionSet.EntityTypeMemberIgnoredConventions.Add(item4);
		conventionSet.EntityTypeMemberIgnoredConventions.Add(item9);
		conventionSet.EntityTypeMemberIgnoredConventions.Add(item5);
		KeyAttributeConvention item10 = new KeyAttributeConvention(Dependencies);
		BackingFieldConvention item11 = new BackingFieldConvention(Dependencies);
		ConcurrencyCheckAttributeConvention item12 = new ConcurrencyCheckAttributeConvention(Dependencies);
		DatabaseGeneratedAttributeConvention item13 = new DatabaseGeneratedAttributeConvention(Dependencies);
		RequiredPropertyAttributeConvention item14 = new RequiredPropertyAttributeConvention(Dependencies);
		NonNullableReferencePropertyConvention item15 = new NonNullableReferencePropertyConvention(Dependencies);
		NonNullableNavigationConvention item16 = new NonNullableNavigationConvention(Dependencies);
		MaxLengthAttributeConvention item17 = new MaxLengthAttributeConvention(Dependencies);
		StringLengthAttributeConvention item18 = new StringLengthAttributeConvention(Dependencies);
		TimestampAttributeConvention item19 = new TimestampAttributeConvention(Dependencies);
		conventionSet.PropertyAddedConventions.Add(item11);
		conventionSet.PropertyAddedConventions.Add(item12);
		conventionSet.PropertyAddedConventions.Add(item13);
		conventionSet.PropertyAddedConventions.Add(item14);
		conventionSet.PropertyAddedConventions.Add(item15);
		conventionSet.PropertyAddedConventions.Add(item17);
		conventionSet.PropertyAddedConventions.Add(item18);
		conventionSet.PropertyAddedConventions.Add(item19);
		conventionSet.PropertyAddedConventions.Add(item10);
		conventionSet.PropertyAddedConventions.Add(item2);
		conventionSet.PropertyAddedConventions.Add(item9);
		conventionSet.EntityTypePrimaryKeyChangedConventions.Add(item9);
		conventionSet.EntityTypePrimaryKeyChangedConventions.Add(item8);
		conventionSet.KeyAddedConventions.Add(item9);
		conventionSet.KeyAddedConventions.Add(item7);
		conventionSet.KeyRemovedConventions.Add(item9);
		conventionSet.KeyRemovedConventions.Add(item7);
		conventionSet.KeyRemovedConventions.Add(item2);
		CascadeDeleteConvention item20 = new CascadeDeleteConvention(Dependencies);
		ForeignKeyAttributeConvention item21 = new ForeignKeyAttributeConvention(Dependencies);
		conventionSet.ForeignKeyAddedConventions.Add(item21);
		conventionSet.ForeignKeyAddedConventions.Add(item9);
		conventionSet.ForeignKeyAddedConventions.Add(item2);
		conventionSet.ForeignKeyAddedConventions.Add(item8);
		conventionSet.ForeignKeyAddedConventions.Add(item20);
		conventionSet.ForeignKeyAddedConventions.Add(item7);
		conventionSet.ForeignKeyRemovedConventions.Add(item2);
		conventionSet.ForeignKeyRemovedConventions.Add(item8);
		conventionSet.ForeignKeyRemovedConventions.Add(item7);
		conventionSet.ForeignKeyPropertiesChangedConventions.Add(item9);
		conventionSet.ForeignKeyPropertiesChangedConventions.Add(item2);
		conventionSet.ForeignKeyPropertiesChangedConventions.Add(item8);
		conventionSet.ForeignKeyPropertiesChangedConventions.Add(item7);
		conventionSet.ForeignKeyUniquenessChangedConventions.Add(item9);
		conventionSet.ForeignKeyUniquenessChangedConventions.Add(item2);
		conventionSet.ForeignKeyUniquenessChangedConventions.Add(item7);
		conventionSet.ForeignKeyRequirednessChangedConventions.Add(item20);
		conventionSet.ForeignKeyRequirednessChangedConventions.Add(item9);
		conventionSet.ForeignKeyOwnershipChangedConventions.Add(new NavigationEagerLoadingConvention(Dependencies));
		conventionSet.ForeignKeyOwnershipChangedConventions.Add(item2);
		conventionSet.ForeignKeyOwnershipChangedConventions.Add(item4);
		conventionSet.ModelInitializedConventions.Add(new DbSetFindingConvention(Dependencies));
		conventionSet.ModelFinalizedConventions.Add(new ModelCleanupConvention(Dependencies));
		conventionSet.ModelFinalizedConventions.Add(item10);
		conventionSet.ModelFinalizedConventions.Add(item21);
		conventionSet.ModelFinalizedConventions.Add(new ChangeTrackingStrategyConvention(Dependencies));
		conventionSet.ModelFinalizedConventions.Add(new ConstructorBindingConvention(Dependencies));
		conventionSet.ModelFinalizedConventions.Add(new TypeMappingConvention(Dependencies));
		conventionSet.ModelFinalizedConventions.Add(item7);
		conventionSet.ModelFinalizedConventions.Add(item9);
		conventionSet.ModelFinalizedConventions.Add(item5);
		conventionSet.ModelFinalizedConventions.Add(item15);
		conventionSet.ModelFinalizedConventions.Add(item16);
		conventionSet.ModelFinalizedConventions.Add(new QueryFilterDefiningQueryRewritingConvention(Dependencies));
		conventionSet.ModelFinalizedConventions.Add(item3);
		conventionSet.ModelFinalizedConventions.Add(item11);
		conventionSet.ModelFinalizedConventions.Add(new ValidatingConvention(Dependencies));
		conventionSet.NavigationAddedConventions.Add(item11);
		conventionSet.NavigationAddedConventions.Add(new RequiredNavigationAttributeConvention(Dependencies));
		conventionSet.NavigationAddedConventions.Add(item16);
		conventionSet.NavigationAddedConventions.Add(item3);
		conventionSet.NavigationAddedConventions.Add(item9);
		conventionSet.NavigationAddedConventions.Add(item4);
		conventionSet.NavigationRemovedConventions.Add(item4);
		conventionSet.IndexAddedConventions.Add(item7);
		conventionSet.IndexRemovedConventions.Add(item7);
		conventionSet.IndexUniquenessChangedConventions.Add(item7);
		conventionSet.ForeignKeyPrincipalEndChangedConventions.Add(item9);
		conventionSet.PropertyNullabilityChangedConventions.Add(item9);
		conventionSet.PropertyFieldChangedConventions.Add(item2);
		conventionSet.PropertyFieldChangedConventions.Add(item9);
		conventionSet.PropertyFieldChangedConventions.Add(item10);
		conventionSet.PropertyFieldChangedConventions.Add(item12);
		conventionSet.PropertyFieldChangedConventions.Add(item13);
		conventionSet.PropertyFieldChangedConventions.Add(item14);
		conventionSet.PropertyFieldChangedConventions.Add(item15);
		conventionSet.PropertyFieldChangedConventions.Add(item17);
		conventionSet.PropertyFieldChangedConventions.Add(item18);
		conventionSet.PropertyFieldChangedConventions.Add(item19);
		return conventionSet;
	}

	protected virtual bool ReplaceConvention<TConvention, TImplementation>([JetBrains.Annotations.NotNull] IList<TConvention> conventionsList, [JetBrains.Annotations.NotNull] TImplementation newConvention) where TImplementation : TConvention
	{
		return ConventionSet.Replace(conventionsList, newConvention);
	}
}
