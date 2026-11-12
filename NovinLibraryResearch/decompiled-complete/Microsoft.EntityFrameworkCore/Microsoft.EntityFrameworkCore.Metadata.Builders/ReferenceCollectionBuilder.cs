using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReferenceCollectionBuilder : RelationshipBuilderBase
{
	[EntityFrameworkInternal]
	public ReferenceCollectionBuilder([JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType dependentEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(principalEntityType, dependentEntityType, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	protected ReferenceCollectionBuilder(InternalRelationshipBuilder builder, ReferenceCollectionBuilder oldBuilder, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
		: base(builder, oldBuilder, foreignKeySet, principalKeySet, requiredSet)
	{
	}

	public virtual ReferenceCollectionBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual ReferenceCollectionBuilder HasForeignKey([JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		return new ReferenceCollectionBuilder(HasForeignKeyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(foreignKeyPropertyNames, "foreignKeyPropertyNames")), this, foreignKeySet: true);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasForeignKeyBuilder([JetBrains.Annotations.NotNull] IReadOnlyList<string> foreignKeyPropertyNames)
	{
		return Builder.HasForeignKey(foreignKeyPropertyNames, (EntityType)DependentEntityType, ConfigurationSource.Explicit);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasForeignKeyBuilder([JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> foreignKeyProperties)
	{
		return Builder.HasForeignKey(foreignKeyProperties, (EntityType)DependentEntityType, ConfigurationSource.Explicit);
	}

	public virtual ReferenceCollectionBuilder HasPrincipalKey([JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		return new ReferenceCollectionBuilder(HasPrincipalKeyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(keyPropertyNames, "keyPropertyNames")), this, foreignKeySet: false, principalKeySet: true);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasPrincipalKeyBuilder([JetBrains.Annotations.NotNull] IReadOnlyList<string> keyPropertyNames)
	{
		return Builder.HasPrincipalKey(keyPropertyNames, ConfigurationSource.Explicit);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasPrincipalKeyBuilder([JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> keyProperties)
	{
		return Builder.HasPrincipalKey(keyProperties, ConfigurationSource.Explicit);
	}

	public virtual ReferenceCollectionBuilder IsRequired(bool required = true)
	{
		return new ReferenceCollectionBuilder(Builder.IsRequired(required, ConfigurationSource.Explicit), this, foreignKeySet: false, principalKeySet: false, requiredSet: true);
	}

	public virtual ReferenceCollectionBuilder OnDelete(DeleteBehavior deleteBehavior)
	{
		return new ReferenceCollectionBuilder(Builder.OnDelete(deleteBehavior, ConfigurationSource.Explicit), this);
	}
}
public class ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> : ReferenceCollectionBuilder where TPrincipalEntity : class where TDependentEntity : class
{
	[EntityFrameworkInternal]
	public ReferenceCollectionBuilder([JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType dependentEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(principalEntityType, dependentEntityType, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	protected ReferenceCollectionBuilder(InternalRelationshipBuilder builder, ReferenceCollectionBuilder oldBuilder, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
		: base(builder, oldBuilder, foreignKeySet, principalKeySet, requiredSet)
	{
	}

	public new virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> HasForeignKey([JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		return new ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>(HasForeignKeyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(foreignKeyPropertyNames, "foreignKeyPropertyNames")), this, foreignKeySet: true);
	}

	public virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> HasForeignKey([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, object>> foreignKeyExpression)
	{
		return new ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>(HasForeignKeyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyExpression, "foreignKeyExpression").GetPropertyAccessList()), this, foreignKeySet: true);
	}

	public new virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> HasPrincipalKey([JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		return new ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>(HasPrincipalKeyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(keyPropertyNames, "keyPropertyNames")), this, foreignKeySet: false, principalKeySet: true);
	}

	public virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> HasPrincipalKey([JetBrains.Annotations.NotNull] Expression<Func<TPrincipalEntity, object>> keyExpression)
	{
		return new ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>(HasPrincipalKeyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyExpression, "keyExpression").GetPropertyAccessList()), this, foreignKeySet: false, principalKeySet: true);
	}

	public new virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>)base.HasAnnotation(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value"));
	}

	public new virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> IsRequired(bool required = true)
	{
		return new ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>(Builder.IsRequired(required, ConfigurationSource.Explicit), this, foreignKeySet: false, principalKeySet: false, requiredSet: true);
	}

	public new virtual ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity> OnDelete(DeleteBehavior deleteBehavior)
	{
		return new ReferenceCollectionBuilder<TPrincipalEntity, TDependentEntity>(Builder.OnDelete(deleteBehavior, ConfigurationSource.Explicit), this);
	}
}
