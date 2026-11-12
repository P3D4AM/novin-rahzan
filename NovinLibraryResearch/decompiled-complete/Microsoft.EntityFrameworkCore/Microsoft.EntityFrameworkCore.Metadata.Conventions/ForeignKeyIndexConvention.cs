using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ForeignKeyIndexConvention : IForeignKeyAddedConvention, IConvention, IForeignKeyRemovedConvention, IForeignKeyPropertiesChangedConvention, IForeignKeyUniquenessChangedConvention, IKeyAddedConvention, IKeyRemovedConvention, IEntityTypeBaseTypeChangedConvention, IIndexAddedConvention, IIndexRemovedConvention, IIndexUniquenessChangedConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ForeignKeyIndexConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionForeignKey metadata = relationshipBuilder.Metadata;
		CreateIndex(metadata.Properties, metadata.IsUnique, metadata.DeclaringEntityType.Builder);
	}

	public virtual void ProcessForeignKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionForeignKey foreignKey, IConventionContext<IConventionForeignKey> context)
	{
		OnForeignKeyRemoved(foreignKey.DeclaringEntityType, foreignKey.Properties);
	}

	public virtual void ProcessForeignKeyPropertiesChanged(IConventionRelationshipBuilder relationshipBuilder, IReadOnlyList<IConventionProperty> oldDependentProperties, IConventionKey oldPrincipalKey, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionForeignKey metadata = relationshipBuilder.Metadata;
		if (!metadata.Properties.SequenceEqual(oldDependentProperties))
		{
			OnForeignKeyRemoved(metadata.DeclaringEntityType, oldDependentProperties);
			if (relationshipBuilder.Metadata.Builder != null)
			{
				CreateIndex(metadata.Properties, metadata.IsUnique, metadata.DeclaringEntityType.Builder);
			}
		}
	}

	private static void OnForeignKeyRemoved(IConventionEntityType declaringType, IReadOnlyList<IConventionProperty> foreignKeyProperties)
	{
		IConventionIndex conventionIndex = declaringType.FindIndex(foreignKeyProperties);
		if (conventionIndex == null)
		{
			return;
		}
		List<IConventionForeignKey> list = declaringType.FindForeignKeys(foreignKeyProperties).ToList();
		if (list.Count != 0)
		{
			if (conventionIndex.IsUnique && list.All((IConventionForeignKey fk) => !fk.IsUnique))
			{
				conventionIndex.Builder.IsUnique(false);
			}
		}
		else
		{
			conventionIndex.DeclaringEntityType.Builder.HasNoIndex(conventionIndex);
		}
	}

	public virtual void ProcessKeyAdded(IConventionKeyBuilder keyBuilder, IConventionContext<IConventionKeyBuilder> context)
	{
		IConventionKey key = keyBuilder.Metadata;
		foreach (IConventionIndex item in (from i in key.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredIndexes())
			where AreIndexedBy(i.Properties, i.IsUnique, key.Properties, coveringIndexUnique: true)
			select i).ToList())
		{
			RemoveIndex(item);
		}
	}

	public virtual void ProcessKeyRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionKey key, IConventionContext<IConventionKey> context)
	{
		foreach (IConventionForeignKey item in from fk in key.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredForeignKeys())
			where AreIndexedBy(fk.Properties, fk.IsUnique, key.Properties, coveringIndexUnique: true)
			select fk)
		{
			CreateIndex(item.Properties, item.IsUnique, item.DeclaringEntityType.Builder);
		}
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (entityTypeBuilder.Metadata.BaseType != newBaseType)
		{
			return;
		}
		List<IConventionKey> source = newBaseType?.GetKeys().ToList();
		List<IConventionIndex> source2 = newBaseType?.GetIndexes().ToList();
		foreach (IConventionForeignKey foreignKey in entityTypeBuilder.Metadata.GetDeclaredForeignKeys().Concat(entityTypeBuilder.Metadata.GetDerivedForeignKeys()))
		{
			IConventionIndex conventionIndex = foreignKey.DeclaringEntityType.FindIndex(foreignKey.Properties);
			if (conventionIndex == null)
			{
				CreateIndex(foreignKey.Properties, foreignKey.IsUnique, foreignKey.DeclaringEntityType.Builder);
			}
			else if (newBaseType != null)
			{
				if (source.FirstOrDefault((IConventionKey k) => AreIndexedBy(foreignKey.Properties, foreignKey.IsUnique, k.Properties, coveringIndexUnique: true)) != null)
				{
					RemoveIndex(conventionIndex);
				}
				else if (source2.FirstOrDefault((IConventionIndex i) => AreIndexedBy(foreignKey.Properties, foreignKey.IsUnique, i.Properties, i.IsUnique)) != null)
				{
					RemoveIndex(conventionIndex);
				}
			}
		}
	}

	public virtual void ProcessIndexAdded(IConventionIndexBuilder indexBuilder, IConventionContext<IConventionIndexBuilder> context)
	{
		IConventionIndex index = indexBuilder.Metadata;
		foreach (IConventionIndex item in (from i in index.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredIndexes())
			where i != index && AreIndexedBy(i.Properties, i.IsUnique, index.Properties, index.IsUnique)
			select i).ToList())
		{
			RemoveIndex(item);
		}
	}

	public virtual void ProcessIndexRemoved(IConventionEntityTypeBuilder entityTypeBuilder, IConventionIndex index, IConventionContext<IConventionIndex> context)
	{
		foreach (IConventionForeignKey item in from fk in index.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredForeignKeys())
			where AreIndexedBy(fk.Properties, fk.IsUnique, index.Properties, index.IsUnique)
			select fk)
		{
			CreateIndex(item.Properties, item.IsUnique, item.DeclaringEntityType.Builder);
		}
	}

	public virtual void ProcessForeignKeyUniquenessChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionForeignKey foreignKey = relationshipBuilder.Metadata;
		IConventionIndex conventionIndex = foreignKey.DeclaringEntityType.FindIndex(foreignKey.Properties);
		if (conventionIndex == null)
		{
			if (foreignKey.IsUnique)
			{
				CreateIndex(foreignKey.Properties, foreignKey.IsUnique, foreignKey.DeclaringEntityType.Builder);
			}
			return;
		}
		if (!foreignKey.IsUnique)
		{
			if (foreignKey.DeclaringEntityType.GetKeys().FirstOrDefault((IConventionKey k) => AreIndexedBy(foreignKey.Properties, unique: false, k.Properties, coveringIndexUnique: true)) != null)
			{
				RemoveIndex(conventionIndex);
				return;
			}
			if (foreignKey.DeclaringEntityType.GetIndexes().FirstOrDefault((IConventionIndex i) => AreIndexedBy(foreignKey.Properties, unique: false, i.Properties, i.IsUnique)) != null)
			{
				RemoveIndex(conventionIndex);
				return;
			}
		}
		conventionIndex.Builder.IsUnique(foreignKey.IsUnique);
	}

	public virtual void ProcessIndexUniquenessChanged(IConventionIndexBuilder indexBuilder, IConventionContext<IConventionIndexBuilder> context)
	{
		IConventionIndex index = indexBuilder.Metadata;
		if (index.IsUnique)
		{
			foreach (IConventionIndex item in (from i in index.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredIndexes())
				where i != index && AreIndexedBy(i.Properties, i.IsUnique, index.Properties, coveringIndexUnique: true)
				select i).ToList())
			{
				RemoveIndex(item);
			}
			return;
		}
		foreach (IConventionForeignKey item2 in from fk in index.DeclaringEntityType.GetDerivedTypesInclusive().SelectMany((IConventionEntityType t) => t.GetDeclaredForeignKeys())
			where fk.IsUnique && AreIndexedBy(fk.Properties, fk.IsUnique, index.Properties, coveringIndexUnique: true)
			select fk)
		{
			CreateIndex(item2.Properties, item2.IsUnique, item2.DeclaringEntityType.Builder);
		}
	}

	protected virtual IConventionIndex CreateIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool unique, [JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder)
	{
		foreach (IConventionKey key in entityTypeBuilder.Metadata.GetKeys())
		{
			if (AreIndexedBy(properties, unique, key.Properties, coveringIndexUnique: true))
			{
				return null;
			}
		}
		foreach (IConventionIndex index in entityTypeBuilder.Metadata.GetIndexes())
		{
			if (AreIndexedBy(properties, unique, index.Properties, index.IsUnique))
			{
				return null;
			}
		}
		IConventionIndexBuilder conventionIndexBuilder = entityTypeBuilder.HasIndex(properties);
		if (unique)
		{
			conventionIndexBuilder?.IsUnique(true);
		}
		return conventionIndexBuilder?.Metadata;
	}

	protected virtual bool AreIndexedBy([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool unique, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> coveringIndexProperties, bool coveringIndexUnique)
	{
		if (unique || !coveringIndexProperties.Select((IConventionProperty p) => p.Name).StartsWith(properties.Select((IConventionProperty p) => p.Name)))
		{
			if (unique && coveringIndexUnique)
			{
				return coveringIndexProperties.SequenceEqual(properties);
			}
			return false;
		}
		return true;
	}

	private static void RemoveIndex(IConventionIndex index)
	{
		index.DeclaringEntityType.Builder.HasNoIndex(index);
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		EventDefinition<string, string, string> eventDefinition = CoreResources.LogRedundantIndexRemoved(Dependencies.Logger);
		if (eventDefinition.GetLogBehavior(Dependencies.Logger) == WarningBehavior.Ignore && !Dependencies.Logger.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			return;
		}
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			foreach (IConventionForeignKey declaredForeignKey in entityType.GetDeclaredForeignKeys())
			{
				foreach (IConventionKey key in entityType.GetKeys())
				{
					if (AreIndexedBy(declaredForeignKey.Properties, declaredForeignKey.IsUnique, key.Properties, coveringIndexUnique: true) && declaredForeignKey.Properties.Count != key.Properties.Count)
					{
						Dependencies.Logger.RedundantIndexRemoved(declaredForeignKey.Properties, key.Properties);
					}
				}
				foreach (IConventionIndex index in entityType.GetIndexes())
				{
					if (AreIndexedBy(declaredForeignKey.Properties, declaredForeignKey.IsUnique, index.Properties, index.IsUnique) && declaredForeignKey.Properties.Count != index.Properties.Count)
					{
						Dependencies.Logger.RedundantIndexRemoved(declaredForeignKey.Properties, index.Properties);
					}
				}
			}
		}
	}
}
