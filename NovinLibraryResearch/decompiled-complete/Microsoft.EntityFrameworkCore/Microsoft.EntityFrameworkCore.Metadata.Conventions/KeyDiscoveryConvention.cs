using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class KeyDiscoveryConvention : IEntityTypeAddedConvention, IConvention, IPropertyAddedConvention, IKeyRemovedConvention, IEntityTypeBaseTypeChangedConvention, IPropertyFieldChangedConvention, IForeignKeyAddedConvention, IForeignKeyRemovedConvention, IForeignKeyPropertiesChangedConvention, IForeignKeyUniquenessChangedConvention, IForeignKeyOwnershipChangedConvention
{
	private const string KeySuffix = "Id";

	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public KeyDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	private void TryConfigurePrimaryKey(IConventionEntityTypeBuilder entityTypeBuilder)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (metadata.BaseType != null || metadata.IsKeyless || !entityTypeBuilder.CanSetPrimaryKey(null))
		{
			return;
		}
		List<IConventionProperty> list = null;
		IConventionForeignKey conventionForeignKey = metadata.FindDefiningNavigation()?.ForeignKey ?? metadata.FindOwnership();
		if (conventionForeignKey != null && conventionForeignKey.DeclaringEntityType != metadata)
		{
			conventionForeignKey = null;
		}
		if (conventionForeignKey != null && conventionForeignKey.IsUnique)
		{
			list = conventionForeignKey.Properties.ToList();
		}
		if (list == null)
		{
			IEnumerable<IConventionProperty> candidateProperties = from p in metadata.GetProperties()
				where !p.IsShadowProperty() || !ConfigurationSource.Convention.Overrides(p.GetConfigurationSource())
				select p;
			list = DiscoverKeyProperties(metadata, candidateProperties).ToList();
			if (list.Count > 1)
			{
				Dependencies.Logger.MultiplePrimaryKeyCandidates(list[0], list[1]);
				return;
			}
		}
		if (conventionForeignKey != null && !conventionForeignKey.IsUnique)
		{
			if (list.Count == 0 || conventionForeignKey.Properties.Contains(list.First()))
			{
				IConventionProperty conventionProperty = metadata.FindPrimaryKey()?.Properties.Last();
				if (conventionProperty == null || metadata.FindPrimaryKey().Properties.Count == 1 || conventionForeignKey.Properties.Contains(conventionProperty))
				{
					conventionProperty = ((InternalEntityTypeBuilder)entityTypeBuilder).CreateUniqueProperty("Id", typeof(int), isRequired: true);
				}
				list.Clear();
				list.Add(conventionProperty);
			}
			IConventionProperty item = list[0];
			list.RemoveAt(0);
			list.AddRange(conventionForeignKey.Properties);
			list.Add(item);
		}
		if (list.Count > 0)
		{
			entityTypeBuilder.PrimaryKey(list);
		}
	}

	public static IEnumerable<IConventionProperty> DiscoverKeyProperties([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IEnumerable<IConventionProperty> candidateProperties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		IEnumerable<IConventionProperty> enumerable = candidateProperties.Where((IConventionProperty p) => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));
		if (!enumerable.Any())
		{
			string entityTypeName = entityType.ShortName();
			enumerable = candidateProperties.Where((IConventionProperty p) => p.Name.Length == entityTypeName.Length + "Id".Length && p.Name.StartsWith(entityTypeName, StringComparison.OrdinalIgnoreCase) && p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase));
		}
		return enumerable;
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		TryConfigurePrimaryKey(entityTypeBuilder);
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (entityTypeBuilder.Metadata.BaseType == newBaseType)
		{
			TryConfigurePrimaryKey(entityTypeBuilder);
		}
	}

	public virtual void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		TryConfigurePrimaryKey(propertyBuilder.Metadata.DeclaringEntityType.Builder);
	}

	public virtual void ProcessPropertyFieldChanged(IConventionPropertyBuilder propertyBuilder, FieldInfo newFieldInfo, FieldInfo oldFieldInfo, IConventionContext<FieldInfo> context)
	{
		TryConfigurePrimaryKey(propertyBuilder.Metadata.DeclaringEntityType.Builder);
	}

	public virtual void ProcessKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey key, IConventionContext<IConventionKey> context)
	{
		if (entityTypeBuilder.Metadata.FindPrimaryKey() == null)
		{
			TryConfigurePrimaryKey(entityTypeBuilder);
		}
	}

	public virtual void ProcessForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		if (relationshipBuilder.Metadata.IsOwnership)
		{
			TryConfigurePrimaryKey(relationshipBuilder.Metadata.DeclaringEntityType.Builder);
		}
	}

	public virtual void ProcessForeignKeyPropertiesChanged(IConventionRelationshipBuilder relationshipBuilder, IReadOnlyList<IConventionProperty> oldDependentProperties, IConventionKey oldPrincipalKey, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionForeignKey metadata = relationshipBuilder.Metadata;
		if (metadata.IsOwnership && !metadata.Properties.SequenceEqual(oldDependentProperties) && relationshipBuilder.Metadata.Builder != null)
		{
			TryConfigurePrimaryKey(metadata.DeclaringEntityType.Builder);
		}
	}

	public virtual void ProcessForeignKeyOwnershipChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		TryConfigurePrimaryKey(relationshipBuilder.Metadata.DeclaringEntityType.Builder);
	}

	public virtual void ProcessForeignKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionForeignKey foreignKey, IConventionContext<IConventionForeignKey> context)
	{
		if (foreignKey.IsOwnership)
		{
			TryConfigurePrimaryKey(entityTypeBuilder);
		}
	}

	public virtual void ProcessForeignKeyUniquenessChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		if (relationshipBuilder.Metadata.IsOwnership)
		{
			TryConfigurePrimaryKey(relationshipBuilder.Metadata.DeclaringEntityType.Builder);
		}
	}
}
