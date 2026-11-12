using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ForeignKeyPropertyDiscoveryConvention : IForeignKeyAddedConvention, IConvention, INavigationAddedConvention, IPropertyAddedConvention, IEntityTypeMemberIgnoredConvention, IPropertyNullabilityChangedConvention, IPropertyFieldChangedConvention, IForeignKeyPropertiesChangedConvention, IForeignKeyPrincipalEndChangedConvention, IForeignKeyUniquenessChangedConvention, IForeignKeyRequirednessChangedConvention, IKeyAddedConvention, IKeyRemovedConvention, IEntityTypePrimaryKeyChangedConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ForeignKeyPropertyDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		bool flag = true;
		foreach (IConventionProperty property in relationshipBuilder.Metadata.Properties)
		{
			if (property.IsNullable)
			{
				flag = false;
				relationshipBuilder.IsRequired(false);
				break;
			}
		}
		if (flag)
		{
			relationshipBuilder.IsRequired(true);
		}
		IConventionRelationshipBuilder newRelationshipBuilder = DiscoverProperties(relationshipBuilder, context);
		foreach (IConventionForeignKey item in (from fk in newRelationshipBuilder.Metadata.DeclaringEntityType.GetForeignKeys()
			where fk != newRelationshipBuilder.Metadata
			select fk).Concat(newRelationshipBuilder.Metadata.DeclaringEntityType.GetDerivedTypes().SelectMany((IConventionEntityType et) => et.GetDeclaredForeignKeys())).ToList())
		{
			if (item.Builder != null)
			{
				DiscoverProperties(item.Builder, context);
			}
		}
		context.StopProcessingIfChanged(newRelationshipBuilder);
	}

	private IConventionRelationshipBuilder DiscoverProperties(IConventionRelationshipBuilder relationshipBuilder, IConventionContext context)
	{
		IConventionForeignKey foreignKey = relationshipBuilder.Metadata;
		if (!ConfigurationSource.Convention.Overrides(foreignKey.GetPropertiesConfigurationSource()))
		{
			IConventionBatch conventionBatch = context.DelayConventions();
			using IMetadataReference<IConventionForeignKey> metadataReference = conventionBatch.Track(foreignKey);
			foreach (IConventionProperty property in foreignKey.Properties)
			{
				if (ConfigurationSource.Convention.Overrides(property.GetTypeConfigurationSource()) && property.IsShadowProperty() && property.ClrType.IsNullableType() == foreignKey.IsRequired && property.GetContainingForeignKeys().All((IConventionForeignKey otherFk) => otherFk.IsRequired == foreignKey.IsRequired))
				{
					Type type = property.ClrType.MakeNullable(!foreignKey.IsRequired);
					if (property.ClrType != type)
					{
						property.DeclaringEntityType.Builder.Property(type, property.Name, property.GetConfigurationSource() == ConfigurationSource.DataAnnotation);
					}
				}
			}
			conventionBatch.Dispose();
			return metadataReference.Object?.Builder;
		}
		bool flag = true;
		if (foreignKey.DeclaringEntityType.DefiningEntityType != foreignKey.PrincipalEntityType && !foreignKey.IsOwnership && !foreignKey.DeclaringEntityType.IsKeyless && (foreignKey.IsUnique || ConfigurationSource.Convention.Overrides(foreignKey.GetIsUniqueConfigurationSource())))
		{
			IConventionNavigation principalToDependent = foreignKey.PrincipalToDependent;
			if ((principalToDependent == null || !principalToDependent.IsCollection()) && foreignKey.DeclaringEntityType.FindOwnership() == null)
			{
				if (ConfigurationSource.Convention.Overrides(foreignKey.GetPrincipalEndConfigurationSource()) && (foreignKey.PrincipalEntityType.DefiningEntityType == foreignKey.DeclaringEntityType || (foreignKey.PrincipalEntityType.FindOwnership() != null && foreignKey.PrincipalToDependent != null && foreignKey.DependentToPrincipal == null)))
				{
					IConventionRelationshipBuilder conventionRelationshipBuilder = relationshipBuilder.HasEntityTypes(foreignKey.DeclaringEntityType, foreignKey.PrincipalEntityType);
					if (conventionRelationshipBuilder != null)
					{
						return conventionRelationshipBuilder;
					}
				}
				goto IL_0292;
			}
		}
		relationshipBuilder = relationshipBuilder.HasEntityTypes(foreignKey.PrincipalEntityType, foreignKey.DeclaringEntityType);
		flag = false;
		goto IL_0292;
		IL_0292:
		IReadOnlyList<IConventionProperty> foreignKeyProperties = FindCandidateForeignKeyProperties(relationshipBuilder.Metadata, onDependent: true);
		if (foreignKeyProperties == null)
		{
			if (flag && ConfigurationSource.Convention.Overrides(foreignKey.GetPrincipalEndConfigurationSource()) && FindCandidateForeignKeyProperties(foreignKey, onDependent: false) != null)
			{
				IConventionRelationshipBuilder conventionRelationshipBuilder2 = relationshipBuilder.HasEntityTypes(foreignKey.DeclaringEntityType, foreignKey.PrincipalEntityType);
				IConventionForeignKey conventionForeignKey = conventionRelationshipBuilder2?.Metadata;
				if (conventionForeignKey != null && conventionForeignKey.IsSelfReferencing())
				{
					conventionRelationshipBuilder2 = conventionRelationshipBuilder2.HasNavigations(conventionForeignKey.PrincipalToDependent?.Name, conventionForeignKey.DependentToPrincipal?.Name);
				}
				object obj = conventionRelationshipBuilder2;
				if (obj == null)
				{
					if (foreignKey.Builder != null)
					{
						return relationshipBuilder;
					}
					obj = null;
				}
				return (IConventionRelationshipBuilder)obj;
			}
			if (foreignKey.DeclaringEntityType.BaseType == null && !foreignKey.IsSelfReferencing())
			{
				if (foreignKey.IsUnique)
				{
					if (!foreignKey.IsOwnership && (!ConfigurationSource.Convention.Overrides(foreignKey.GetPrincipalEndConfigurationSource()) || foreignKey.DeclaringEntityType.DefiningEntityType == foreignKey.PrincipalEntityType))
					{
						foreignKeyProperties = GetCompatiblePrimaryKeyProperties(foreignKey.DeclaringEntityType, foreignKey.PrincipalEntityType, foreignKey.PrincipalKey.Properties);
					}
					else if (flag)
					{
						foreignKeyProperties = FindCandidateForeignKeyProperties(foreignKey, onDependent: true, matchPk: true);
						IReadOnlyList<IConventionProperty> readOnlyList = FindCandidateForeignKeyProperties(foreignKey, onDependent: false, matchPk: true);
						if (readOnlyList != null)
						{
							if (foreignKeyProperties == null)
							{
								using IConventionBatch conventionBatch2 = context.DelayConventions();
								IConventionRelationshipBuilder conventionRelationshipBuilder3 = relationshipBuilder.HasEntityTypes(foreignKey.DeclaringEntityType, foreignKey.PrincipalEntityType);
								return conventionBatch2.Run(conventionRelationshipBuilder3.HasForeignKey(readOnlyList).Metadata)?.Builder;
							}
							foreignKeyProperties = null;
							((ForeignKey)relationshipBuilder.Metadata).SetPrincipalEndConfigurationSource(null);
						}
					}
				}
				else
				{
					IConventionKey dependentPk = foreignKey.DeclaringEntityType.FindPrimaryKey();
					if (dependentPk != null && dependentPk.Properties.Count > foreignKey.PrincipalKey.Properties.Count && TryFindMatchingProperties(foreignKey, "", onDependent: true, matchPk: false, out foreignKeyProperties) && foreignKeyProperties != null && foreignKeyProperties.Any((IConventionProperty p) => !dependentPk.Properties.Contains(p) || p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)))
					{
						foreignKeyProperties = null;
					}
				}
			}
			if (foreignKeyProperties == null && flag && ConfigurationSource.Convention.Overrides(foreignKey.GetPrincipalEndConfigurationSource()))
			{
				((ForeignKey)relationshipBuilder.Metadata).SetPrincipalEndConfigurationSource(null);
			}
		}
		else if (flag && ConfigurationSource.Convention.Overrides(foreignKey.GetPrincipalEndConfigurationSource()) && FindCandidateForeignKeyProperties(foreignKey, onDependent: false) != null)
		{
			foreignKeyProperties = null;
			((ForeignKey)relationshipBuilder.Metadata).SetPrincipalEndConfigurationSource(null);
		}
		if (foreignKeyProperties == null)
		{
			return ((ForeignKey)foreignKey).Builder.ReuniquifyTemporaryProperties(force: false);
		}
		int num = foreignKey.DeclaringEntityType.FindForeignKeys(foreignKeyProperties).Concat(foreignKey.DeclaringEntityType.GetDerivedTypes().SelectMany((IConventionEntityType et) => et.FindDeclaredForeignKeys(foreignKeyProperties))).Count();
		if (foreignKey.Properties.SequenceEqual(foreignKeyProperties))
		{
			if (num <= 1)
			{
				return relationshipBuilder;
			}
			return ((ForeignKey)foreignKey).Builder.ReuniquifyTemporaryProperties(force: true);
		}
		if (num > 0)
		{
			return ((ForeignKey)foreignKey).Builder.ReuniquifyTemporaryProperties(force: false);
		}
		IConventionRelationshipBuilder conventionRelationshipBuilder4 = relationshipBuilder.HasForeignKey(foreignKeyProperties);
		if (conventionRelationshipBuilder4 != null)
		{
			return conventionRelationshipBuilder4;
		}
		if (relationshipBuilder.Metadata.Builder != null)
		{
			return relationshipBuilder;
		}
		return null;
	}

	private IReadOnlyList<IConventionProperty> FindCandidateForeignKeyProperties(IConventionForeignKey foreignKey, bool onDependent, bool matchPk = false)
	{
		IConventionNavigation conventionNavigation = (onDependent ? foreignKey.DependentToPrincipal : foreignKey.PrincipalToDependent);
		if (conventionNavigation != null && TryFindMatchingProperties(foreignKey, conventionNavigation.Name, onDependent, matchPk, out var match))
		{
			return match;
		}
		IConventionEntityType type = (onDependent ? foreignKey.PrincipalEntityType : foreignKey.DeclaringEntityType);
		TryFindMatchingProperties(foreignKey, type.ShortName(), onDependent, matchPk, out match);
		return match;
	}

	private static IReadOnlyList<IConventionProperty> GetCompatiblePrimaryKeyProperties(IConventionEntityType dependentEntityType, IConventionEntityType principalEntityType, IReadOnlyList<IConventionProperty> propertiesToReference)
	{
		IReadOnlyList<IConventionProperty> readOnlyList = dependentEntityType.FindPrimaryKey()?.Properties;
		if (readOnlyList == null || !ForeignKey.AreCompatible(propertiesToReference, readOnlyList, principalEntityType, dependentEntityType, shouldThrow: false))
		{
			return null;
		}
		return readOnlyList;
	}

	private bool TryFindMatchingProperties(IConventionForeignKey foreignKey, string baseName, bool onDependent, bool matchPk, out IReadOnlyList<IConventionProperty> match)
	{
		match = null;
		IConventionEntityType conventionEntityType = (onDependent ? foreignKey.DeclaringEntityType : foreignKey.PrincipalEntityType);
		IConventionEntityType principalEntityType = (onDependent ? foreignKey.PrincipalEntityType : foreignKey.DeclaringEntityType);
		IReadOnlyList<IConventionProperty> readOnlyList = (onDependent ? foreignKey.PrincipalKey.Properties : foreignKey.DeclaringEntityType.FindPrimaryKey()?.Properties);
		if (readOnlyList == null)
		{
			return false;
		}
		IConventionProperty[] array = new IConventionProperty[readOnlyList.Count];
		bool flag = true;
		for (int i = 0; i < readOnlyList.Count; i++)
		{
			IConventionProperty conventionProperty = readOnlyList[i];
			IConventionProperty conventionProperty2 = TryGetProperty(conventionEntityType, baseName, conventionProperty.Name);
			if (conventionProperty2 == null)
			{
				flag = false;
			}
			else
			{
				array[i] = conventionProperty2;
			}
		}
		if (!flag && readOnlyList.Count == 1 && baseName.Length > 0)
		{
			IConventionProperty conventionProperty3 = TryGetProperty(conventionEntityType, baseName, "Id");
			if (conventionProperty3 != null)
			{
				array[0] = conventionProperty3;
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (!ForeignKey.AreCompatible(readOnlyList, array, principalEntityType, conventionEntityType, shouldThrow: false))
		{
			if (readOnlyList.All((IConventionProperty p) => !p.IsShadowProperty() || p.GetConfigurationSource().Overrides(ConfigurationSource.DataAnnotation)))
			{
				Dependencies.Logger.IncompatibleMatchingForeignKeyProperties(array, readOnlyList);
			}
			return true;
		}
		foreach (IConventionKey key in conventionEntityType.GetKeys())
		{
			bool flag2 = true;
			for (int j = 0; j < key.Properties.Count; j++)
			{
				if (!array.Contains(key.Properties[j]))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2 && (!foreignKey.IsUnique || (key.IsPrimaryKey() && !matchPk)))
			{
				return true;
			}
		}
		match = array;
		return true;
	}

	private static IConventionProperty TryGetProperty(IConventionEntityType entityType, string prefix, string suffix)
	{
		foreach (IConventionProperty property in entityType.GetProperties())
		{
			if ((!property.IsShadowProperty() || !ConfigurationSource.Convention.Overrides(property.GetConfigurationSource())) && property.Name.Length == prefix.Length + suffix.Length && property.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) && property.Name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
			{
				return property;
			}
		}
		return null;
	}

	public virtual void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, IConventionContext<IConventionNavigation> context)
	{
		context.StopProcessingIfChanged(DiscoverProperties(relationshipBuilder, context)?.Metadata.GetNavigation(navigation.IsDependentToPrincipal()));
	}

	public virtual void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		Process(propertyBuilder, context);
		if (propertyBuilder.Metadata.Builder == null)
		{
			context.StopProcessing();
		}
	}

	private void Process(IConventionPropertyBuilder propertyBuilder, IConventionContext context)
	{
		IConventionProperty metadata = propertyBuilder.Metadata;
		if (!metadata.IsShadowProperty() || !ConfigurationSource.Convention.Overrides(metadata.GetConfigurationSource()))
		{
			IConventionEntityType declaringEntityType = propertyBuilder.Metadata.DeclaringEntityType;
			Process(declaringEntityType, context);
		}
	}

	public virtual void ProcessEntityTypeMemberIgnored(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionContext<string> context)
	{
		Process(entityTypeBuilder.Metadata, context);
	}

	private void Process(IConventionEntityType entityType, IConventionContext context)
	{
		foreach (IConventionForeignKey item in entityType.GetDeclaredForeignKeys().Concat(entityType.GetDerivedForeignKeys()).ToList())
		{
			if (item.Builder != null)
			{
				DiscoverProperties(item.Builder, context);
			}
		}
		foreach (IConventionForeignKey item2 in entityType.GetReferencingForeignKeys().ToList())
		{
			if (item2.Builder != null && ConfigurationSource.Convention.Overrides(item2.GetPrincipalEndConfigurationSource()))
			{
				DiscoverProperties(item2.Builder, context);
			}
		}
	}

	public virtual void ProcessPropertyFieldChanged(IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, FieldInfo oldFieldInfo, IConventionContext<FieldInfo> context)
	{
		Process(propertyBuilder, context);
	}

	public virtual void ProcessPropertyNullabilityChanged(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		bool isNullable = propertyBuilder.Metadata.IsNullable;
		foreach (IConventionForeignKey containingForeignKey in propertyBuilder.Metadata.GetContainingForeignKeys())
		{
			if (containingForeignKey.IsRequired == isNullable && (isNullable || !containingForeignKey.Properties.Any((IConventionProperty p) => p.IsNullable)))
			{
				containingForeignKey.Builder.IsRequired(!isNullable);
			}
		}
	}

	public virtual void ProcessForeignKeyUniquenessChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionRelationshipBuilder result = DiscoverProperties(relationshipBuilder, context);
		context.StopProcessingIfChanged(result);
	}

	public virtual void ProcessForeignKeyRequirednessChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		bool isRequired = relationshipBuilder.Metadata.IsRequired;
		using IConventionBatch conventionBatch = context.DelayConventions();
		using (IEnumerator<IConventionProperty> enumerator = relationshipBuilder.Metadata.Properties.Where((IConventionProperty p) => p.ClrType.IsNullableType()).GetEnumerator())
		{
			while (enumerator.MoveNext() && (enumerator.Current.Builder.IsRequired(isRequired) == null || isRequired))
			{
			}
		}
		context.StopProcessingIfChanged(conventionBatch.Run(DiscoverProperties(relationshipBuilder, context).Metadata)?.Builder);
	}

	public virtual void ProcessForeignKeyPropertiesChanged(IConventionRelationshipBuilder relationshipBuilder, IReadOnlyList<IConventionProperty> oldDependentProperties, IConventionKey oldPrincipalKey, IConventionContext<IConventionRelationshipBuilder> context)
	{
		if (relationshipBuilder.Metadata.Builder != null && relationshipBuilder.Metadata.Properties != oldDependentProperties)
		{
			ProcessForeignKeyAdded(relationshipBuilder, context);
		}
	}

	public virtual void ProcessForeignKeyPrincipalEndChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		ProcessForeignKeyAdded(relationshipBuilder, context);
	}

	public virtual void ProcessKeyAdded(IConventionKeyBuilder keyBuilder, IConventionContext<IConventionKeyBuilder> context)
	{
		IConventionKey metadata = keyBuilder.Metadata;
		foreach (IConventionForeignKey foreignKey in metadata.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredForeignKeys()).ToList())
		{
			if (metadata.Properties.All((IConventionProperty p) => foreignKey.Properties.Contains(p)) && (!foreignKey.IsUnique || foreignKey.DeclaringEntityType.BaseType != null))
			{
				foreignKey.Builder.HasForeignKey((IReadOnlyList<IConventionProperty>)null, fromDataAnnotation: false);
			}
		}
	}

	public virtual void ProcessKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey key, IConventionContext<IConventionKey> context)
	{
		foreach (IConventionForeignKey item in key.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredForeignKeys()).ToList())
		{
			if (!item.IsUnique || item.DeclaringEntityType.BaseType != null)
			{
				DiscoverProperties(item.Builder, context);
			}
		}
	}

	public virtual void ProcessEntityTypePrimaryKeyChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey newPrimaryKey, IConventionKey previousPrimaryKey, IConventionContext<IConventionKey> context)
	{
		if (newPrimaryKey != null && newPrimaryKey.Builder == null)
		{
			return;
		}
		foreach (IConventionForeignKey item in entityTypeBuilder.Metadata.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredForeignKeys()).ToList())
		{
			if (item.IsUnique)
			{
				DiscoverProperties(item.Builder, context);
			}
		}
		foreach (IConventionForeignKey item2 in entityTypeBuilder.Metadata.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredReferencingForeignKeys()).ToList())
		{
			DiscoverProperties(item2.Builder, context);
		}
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			bool flag = false;
			foreach (IConventionForeignKey foreignKey in from fk in entityType.GetDeclaredForeignKeys()
				where !fk.GetPropertiesConfigurationSource().HasValue
				select fk)
			{
				IReadOnlyList<IConventionProperty> foreignKeyProperties = FindCandidateForeignKeyProperties(foreignKey, onDependent: true);
				if (foreignKeyProperties != null)
				{
					IConventionForeignKey conventionForeignKey = foreignKey.DeclaringEntityType.FindForeignKeys(foreignKeyProperties).Concat(foreignKey.DeclaringEntityType.GetDerivedTypes().SelectMany((IConventionEntityType et) => et.FindDeclaredForeignKeys(foreignKeyProperties))).FirstOrDefault((IConventionForeignKey fk) => fk != foreignKey && ConfigurationSource.Convention.Overrides(fk.GetPropertiesConfigurationSource()));
					if (conventionForeignKey != null)
					{
						throw new InvalidOperationException(CoreStrings.AmbiguousForeignKeyPropertyCandidates(conventionForeignKey.DeclaringEntityType.DisplayName() + ((conventionForeignKey.DependentToPrincipal == null) ? "" : ("." + conventionForeignKey.DependentToPrincipal.Name)), conventionForeignKey.PrincipalEntityType.DisplayName() + ((conventionForeignKey.PrincipalToDependent == null) ? "" : ("." + conventionForeignKey.PrincipalToDependent.Name)), foreignKey.DeclaringEntityType.DisplayName() + ((foreignKey.DependentToPrincipal == null) ? "" : ("." + foreignKey.DependentToPrincipal.Name)), foreignKey.PrincipalEntityType.DisplayName() + ((foreignKey.PrincipalToDependent == null) ? "" : ("." + foreignKey.PrincipalToDependent.Name)), foreignKeyProperties.Format()));
					}
				}
				if (!flag && HasUniquifiedProperties(foreignKey))
				{
					IConventionForeignKey conventionForeignKey2 = entityType.GetDeclaredForeignKeys().FirstOrDefault((IConventionForeignKey otherForeignKey) => otherForeignKey != foreignKey && otherForeignKey.PrincipalEntityType == foreignKey.PrincipalEntityType && !otherForeignKey.GetPropertiesConfigurationSource().HasValue);
					if (conventionForeignKey2 != null)
					{
						flag = true;
						Dependencies.Logger.ConflictingShadowForeignKeysWarning(conventionForeignKey2);
					}
				}
			}
		}
	}

	private static string GetPropertyBaseName(IConventionForeignKey foreignKey)
	{
		return foreignKey.DependentToPrincipal?.Name ?? foreignKey.PrincipalEntityType.ShortName();
	}

	private static bool HasUniquifiedProperties(IConventionForeignKey foreignKey)
	{
		string propertyBaseName = GetPropertyBaseName(foreignKey);
		for (int i = 0; i < foreignKey.Properties.Count; i++)
		{
			string name = foreignKey.Properties[i].Name;
			string name2 = foreignKey.PrincipalKey.Properties[i].Name;
			if (name.Length != propertyBaseName.Length + name2.Length || !name.StartsWith(propertyBaseName, StringComparison.Ordinal) || !name.EndsWith(name2, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}
}
