using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class PropertiesSnapshot
{
	private List<InternalPropertyBuilder> Properties
	{
		[DebuggerStepThrough]
		get;
	}

	private List<RelationshipSnapshot> Relationships
	{
		[DebuggerStepThrough]
		get;
		set; }

	private List<InternalIndexBuilder> Indexes
	{
		[DebuggerStepThrough]
		get;
		set; }

	private List<(InternalKeyBuilder, ConfigurationSource?)> Keys
	{
		[DebuggerStepThrough]
		get;
		set; }

	public PropertiesSnapshot([JetBrains.Annotations.CanBeNull] List<InternalPropertyBuilder> properties, [JetBrains.Annotations.CanBeNull] List<InternalIndexBuilder> indexes, [JetBrains.Annotations.CanBeNull] List<(InternalKeyBuilder, ConfigurationSource?)> keys, [JetBrains.Annotations.CanBeNull] List<RelationshipSnapshot> relationships)
	{
		Properties = properties;
		Indexes = indexes;
		Keys = keys;
		Relationships = relationships;
	}

	public virtual void Add([JetBrains.Annotations.NotNull] List<RelationshipSnapshot> relationships)
	{
		if (Relationships == null)
		{
			Relationships = relationships;
		}
		else
		{
			Relationships.AddRange(relationships);
		}
	}

	public virtual void Add([JetBrains.Annotations.NotNull] List<InternalIndexBuilder> indexes)
	{
		if (Indexes == null)
		{
			Indexes = indexes;
		}
		else
		{
			Indexes.AddRange(indexes);
		}
	}

	public virtual void Add([JetBrains.Annotations.NotNull] List<(InternalKeyBuilder, ConfigurationSource?)> keys)
	{
		if (Keys == null)
		{
			Keys = keys;
		}
		else
		{
			Keys.AddRange(keys);
		}
	}

	public virtual void Attach([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder)
	{
		if (Properties != null)
		{
			foreach (InternalPropertyBuilder property in Properties)
			{
				property.Attach(entityTypeBuilder);
			}
		}
		if (Keys != null)
		{
			foreach (var key in Keys)
			{
				key.Item1.Attach(entityTypeBuilder.Metadata.RootType().Builder, key.Item2);
			}
		}
		if (Indexes != null)
		{
			foreach (InternalIndexBuilder index in Indexes)
			{
				EntityType declaringEntityType = index.Metadata.DeclaringEntityType;
				InternalEntityTypeBuilder entityTypeBuilder2 = ((declaringEntityType.Name == entityTypeBuilder.Metadata.Name) ? entityTypeBuilder : declaringEntityType.Builder);
				index.Attach(entityTypeBuilder2);
			}
		}
		if (Relationships == null)
		{
			return;
		}
		foreach (RelationshipSnapshot relationship in Relationships)
		{
			relationship.Attach(entityTypeBuilder);
		}
	}
}
