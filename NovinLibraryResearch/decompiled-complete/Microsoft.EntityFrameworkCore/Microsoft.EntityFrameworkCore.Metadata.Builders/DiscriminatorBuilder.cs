using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DiscriminatorBuilder : IConventionDiscriminatorBuilder
{
	[EntityFrameworkInternal]
	protected virtual InternalEntityTypeBuilder EntityTypeBuilder { get; }

	[EntityFrameworkInternal]
	public DiscriminatorBuilder([JetBrains.Annotations.NotNull] IMutableEntityType entityType)
	{
		EntityTypeBuilder = ((EntityType)entityType).Builder;
	}

	public virtual DiscriminatorBuilder HasValue([JetBrains.Annotations.CanBeNull] object value)
	{
		return HasValue(EntityTypeBuilder, value, ConfigurationSource.Explicit);
	}

	public virtual DiscriminatorBuilder HasValue<TEntity>([JetBrains.Annotations.CanBeNull] object value)
	{
		return HasValue(typeof(TEntity), value);
	}

	public virtual DiscriminatorBuilder HasValue([JetBrains.Annotations.NotNull] Type entityType, [JetBrains.Annotations.CanBeNull] object value)
	{
		InternalEntityTypeBuilder entityTypeBuilder = EntityTypeBuilder.ModelBuilder.Entity(entityType, ConfigurationSource.Explicit, null);
		return HasValue(entityTypeBuilder, value, ConfigurationSource.Explicit);
	}

	public virtual DiscriminatorBuilder HasValue([JetBrains.Annotations.NotNull] string entityTypeName, [JetBrains.Annotations.CanBeNull] object value)
	{
		InternalEntityTypeBuilder entityTypeBuilder = EntityTypeBuilder.ModelBuilder.Entity(entityTypeName, ConfigurationSource.Explicit, null);
		return HasValue(entityTypeBuilder, value, ConfigurationSource.Explicit);
	}

	private DiscriminatorBuilder HasValue(InternalEntityTypeBuilder entityTypeBuilder, object value, ConfigurationSource configurationSource)
	{
		if (entityTypeBuilder == null)
		{
			return null;
		}
		InternalEntityTypeBuilder entityTypeBuilder2 = EntityTypeBuilder;
		if (!entityTypeBuilder2.Metadata.IsAssignableFrom(entityTypeBuilder.Metadata) && entityTypeBuilder.HasBaseType(entityTypeBuilder2.Metadata, configurationSource) == null)
		{
			throw new InvalidOperationException(CoreStrings.DiscriminatorEntityTypeNotDerived(entityTypeBuilder.Metadata.DisplayName(), entityTypeBuilder2.Metadata.DisplayName()));
		}
		if (configurationSource == ConfigurationSource.Explicit)
		{
			MutableEntityTypeExtensions.SetDiscriminatorValue(entityTypeBuilder.Metadata, value);
		}
		else
		{
			if (!entityTypeBuilder.CanSetAnnotation("DiscriminatorValue", value, configurationSource))
			{
				return null;
			}
			entityTypeBuilder.Metadata.SetDiscriminatorValue(value, configurationSource == ConfigurationSource.DataAnnotation);
		}
		return this;
	}

	IConventionDiscriminatorBuilder IConventionDiscriminatorBuilder.HasValue(object value, bool fromDataAnnotation)
	{
		return HasValue(EntityTypeBuilder, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionDiscriminatorBuilder IConventionDiscriminatorBuilder.HasValue(IConventionEntityType entityType, object value, bool fromDataAnnotation)
	{
		return HasValue((InternalEntityTypeBuilder)entityType.Builder, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionDiscriminatorBuilder.CanSetValue(object value, bool fromDataAnnotation)
	{
		return ((IConventionDiscriminatorBuilder)this).CanSetValue((IConventionEntityType)EntityTypeBuilder.Metadata, value, fromDataAnnotation);
	}

	bool IConventionDiscriminatorBuilder.CanSetValue(IConventionEntityType entityType, object value, bool fromDataAnnotation)
	{
		InternalEntityTypeBuilder entityTypeBuilder = EntityTypeBuilder;
		if (!entityTypeBuilder.Metadata.IsAssignableFrom(entityType) && !entityType.Builder.CanSetBaseType(entityTypeBuilder.Metadata, fromDataAnnotation))
		{
			return false;
		}
		return entityType.Builder.CanSetAnnotation("DiscriminatorValue", value, fromDataAnnotation);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
public class DiscriminatorBuilder<TDiscriminator>
{
	private DiscriminatorBuilder Builder { get; }

	[EntityFrameworkInternal]
	public DiscriminatorBuilder([JetBrains.Annotations.NotNull] DiscriminatorBuilder builder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Builder = builder;
	}

	public virtual DiscriminatorBuilder<TDiscriminator> HasValue([JetBrains.Annotations.CanBeNull] TDiscriminator value)
	{
		DiscriminatorBuilder discriminatorBuilder = Builder.HasValue(value);
		if (discriminatorBuilder != null)
		{
			return new DiscriminatorBuilder<TDiscriminator>(discriminatorBuilder);
		}
		return null;
	}

	public virtual DiscriminatorBuilder<TDiscriminator> HasValue<TEntity>([JetBrains.Annotations.CanBeNull] TDiscriminator value)
	{
		return HasValue(typeof(TEntity), value);
	}

	public virtual DiscriminatorBuilder<TDiscriminator> HasValue([JetBrains.Annotations.NotNull] Type entityType, [JetBrains.Annotations.CanBeNull] TDiscriminator value)
	{
		DiscriminatorBuilder discriminatorBuilder = Builder.HasValue(entityType, value);
		if (discriminatorBuilder != null)
		{
			return new DiscriminatorBuilder<TDiscriminator>(discriminatorBuilder);
		}
		return null;
	}

	public virtual DiscriminatorBuilder<TDiscriminator> HasValue([JetBrains.Annotations.NotNull] string entityTypeName, [JetBrains.Annotations.CanBeNull] TDiscriminator value)
	{
		DiscriminatorBuilder discriminatorBuilder = Builder.HasValue(entityTypeName, value);
		if (discriminatorBuilder != null)
		{
			return new DiscriminatorBuilder<TDiscriminator>(discriminatorBuilder);
		}
		return null;
	}
}
