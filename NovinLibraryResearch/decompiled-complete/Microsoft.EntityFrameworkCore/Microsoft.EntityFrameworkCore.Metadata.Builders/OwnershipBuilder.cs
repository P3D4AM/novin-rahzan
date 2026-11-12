using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OwnershipBuilder : RelationshipBuilderBase
{
	[EntityFrameworkInternal]
	public OwnershipBuilder([JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType dependentEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(principalEntityType, dependentEntityType, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	protected OwnershipBuilder(InternalRelationshipBuilder builder, OwnershipBuilder oldBuilder, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
		: base(builder, oldBuilder, foreignKeySet, principalKeySet, requiredSet)
	{
	}

	public virtual OwnershipBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual OwnershipBuilder HasForeignKey([JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		Builder = Builder.HasForeignKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyPropertyNames, "foreignKeyPropertyNames"), (EntityType)DependentEntityType, ConfigurationSource.Explicit);
		return new OwnershipBuilder(Builder, this, foreignKeyPropertyNames.Length != 0);
	}

	public virtual OwnershipBuilder HasPrincipalKey([JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		Builder = Builder.HasPrincipalKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyPropertyNames, "keyPropertyNames"), ConfigurationSource.Explicit);
		return new OwnershipBuilder(Builder, this, foreignKeySet: false, keyPropertyNames.Length != 0);
	}
}
public class OwnershipBuilder<TEntity, TDependentEntity> : OwnershipBuilder where TEntity : class where TDependentEntity : class
{
	[EntityFrameworkInternal]
	public OwnershipBuilder([JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType dependentEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(principalEntityType, dependentEntityType, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	protected OwnershipBuilder(InternalRelationshipBuilder builder, OwnershipBuilder oldBuilder, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
		: base(builder, oldBuilder, foreignKeySet, principalKeySet, requiredSet)
	{
	}

	public new virtual OwnershipBuilder<TEntity, TDependentEntity> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (OwnershipBuilder<TEntity, TDependentEntity>)base.HasAnnotation(annotation, value);
	}

	public new virtual OwnershipBuilder<TEntity, TDependentEntity> HasForeignKey([JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		Builder = Builder.HasForeignKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyPropertyNames, "foreignKeyPropertyNames"), (EntityType)DependentEntityType, ConfigurationSource.Explicit);
		return new OwnershipBuilder<TEntity, TDependentEntity>(Builder, this, foreignKeyPropertyNames.Length != 0);
	}

	public virtual OwnershipBuilder<TEntity, TDependentEntity> HasForeignKey([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, object>> foreignKeyExpression)
	{
		Builder = Builder.HasForeignKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyExpression, "foreignKeyExpression").GetPropertyAccessList(), (EntityType)DependentEntityType, ConfigurationSource.Explicit);
		return new OwnershipBuilder<TEntity, TDependentEntity>(Builder, this, foreignKeySet: true);
	}

	public new virtual OwnershipBuilder<TEntity, TDependentEntity> HasPrincipalKey([JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		Builder = Builder.HasPrincipalKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyPropertyNames, "keyPropertyNames"), ConfigurationSource.Explicit);
		return new OwnershipBuilder<TEntity, TDependentEntity>(Builder, this, foreignKeySet: false, keyPropertyNames.Length != 0);
	}

	public virtual OwnershipBuilder<TEntity, TDependentEntity> HasPrincipalKey([JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> keyExpression)
	{
		Builder = Builder.HasPrincipalKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyExpression, "keyExpression").GetPropertyAccessList(), ConfigurationSource.Explicit);
		return new OwnershipBuilder<TEntity, TDependentEntity>(Builder, this, foreignKeySet: false, principalKeySet: true);
	}
}
