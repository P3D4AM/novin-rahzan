using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReferenceReferenceBuilder : InvertibleRelationshipBuilderBase
{
	[EntityFrameworkInternal]
	public ReferenceReferenceBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(declaringEntityType, relatedEntityType, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	protected ReferenceReferenceBuilder(InternalRelationshipBuilder builder, ReferenceReferenceBuilder oldBuilder, bool inverted = false, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
		: base(builder, oldBuilder, inverted, foreignKeySet, principalKeySet, requiredSet)
	{
	}

	public virtual ReferenceReferenceBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual ReferenceReferenceBuilder HasForeignKey([JetBrains.Annotations.NotNull] string dependentEntityTypeName, [JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		return new ReferenceReferenceBuilder(HasForeignKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityTypeName, "dependentEntityTypeName")), dependentEntityTypeName, Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyPropertyNames, "foreignKeyPropertyNames")), this, Builder.Metadata.DeclaringEntityType.Name != ResolveEntityType(dependentEntityTypeName).Name, foreignKeyPropertyNames.Length != 0);
	}

	public virtual ReferenceReferenceBuilder HasForeignKey([JetBrains.Annotations.NotNull] Type dependentEntityType, [JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		return new ReferenceReferenceBuilder(HasForeignKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityType, "dependentEntityType")), dependentEntityType.ShortDisplayName(), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyPropertyNames, "foreignKeyPropertyNames")), this, Builder.Metadata.DeclaringEntityType.ClrType != dependentEntityType, foreignKeyPropertyNames.Length != 0);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasForeignKeyBuilder([JetBrains.Annotations.CanBeNull] EntityType dependentEntityType, [JetBrains.Annotations.NotNull] string dependentEntityTypeName, [JetBrains.Annotations.NotNull] IReadOnlyList<string> foreignKeyPropertyNames)
	{
		return HasForeignKeyBuilder(dependentEntityType, dependentEntityTypeName, (InternalRelationshipBuilder b, EntityType d) => b.HasForeignKey(foreignKeyPropertyNames, d, ConfigurationSource.Explicit));
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasForeignKeyBuilder([JetBrains.Annotations.NotNull] EntityType dependentEntityType, [JetBrains.Annotations.NotNull] string dependentEntityTypeName, [JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> foreignKeyProperties)
	{
		return HasForeignKeyBuilder(dependentEntityType, dependentEntityTypeName, (InternalRelationshipBuilder b, EntityType d) => b.HasForeignKey(foreignKeyProperties, d, ConfigurationSource.Explicit));
	}

	private InternalRelationshipBuilder HasForeignKeyBuilder(EntityType dependentEntityType, string dependentEntityTypeName, Func<InternalRelationshipBuilder, EntityType, InternalRelationshipBuilder> hasForeignKey)
	{
		if (dependentEntityType == null)
		{
			throw new InvalidOperationException(CoreStrings.DependentEntityTypeNotInRelationship(DeclaringEntityType.DisplayName(), RelatedEntityType.DisplayName(), dependentEntityTypeName));
		}
		using IConventionBatch batch = dependentEntityType.Model.ConventionDispatcher.DelayConventions();
		InternalRelationshipBuilder arg = Builder.HasEntityTypes(GetOtherEntityType(dependentEntityType), dependentEntityType, ConfigurationSource.Explicit);
		arg = hasForeignKey(arg, dependentEntityType);
		return batch.Run(arg);
	}

	public virtual ReferenceReferenceBuilder HasPrincipalKey([JetBrains.Annotations.NotNull] string principalEntityTypeName, [JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		return new ReferenceReferenceBuilder(HasPrincipalKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(principalEntityTypeName, "principalEntityTypeName")), principalEntityTypeName, Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyPropertyNames, "keyPropertyNames")), this, Builder.Metadata.PrincipalEntityType.Name != ResolveEntityType(principalEntityTypeName).Name, foreignKeySet: false, keyPropertyNames.Length != 0);
	}

	public virtual ReferenceReferenceBuilder HasPrincipalKey([JetBrains.Annotations.NotNull] Type principalEntityType, [JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		return new ReferenceReferenceBuilder(HasPrincipalKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType")), principalEntityType.ShortDisplayName(), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyPropertyNames, "keyPropertyNames")), this, Builder.Metadata.PrincipalEntityType.ClrType != principalEntityType, foreignKeySet: false, keyPropertyNames.Length != 0);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasPrincipalKeyBuilder([JetBrains.Annotations.CanBeNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] string principalEntityTypeName, [JetBrains.Annotations.NotNull] IReadOnlyList<string> foreignKeyPropertyNames)
	{
		return HasPrincipalKeyBuilder(principalEntityType, principalEntityTypeName, (InternalRelationshipBuilder b) => b.HasPrincipalKey(foreignKeyPropertyNames, ConfigurationSource.Explicit));
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder HasPrincipalKeyBuilder([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] string principalEntityTypeName, [JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> foreignKeyProperties)
	{
		return HasPrincipalKeyBuilder(principalEntityType, principalEntityTypeName, (InternalRelationshipBuilder b) => b.HasPrincipalKey(foreignKeyProperties, ConfigurationSource.Explicit));
	}

	private InternalRelationshipBuilder HasPrincipalKeyBuilder(EntityType principalEntityType, string principalEntityTypeName, Func<InternalRelationshipBuilder, InternalRelationshipBuilder> hasPrincipalKey)
	{
		if (principalEntityType == null)
		{
			throw new InvalidOperationException(CoreStrings.PrincipalEntityTypeNotInRelationship(DeclaringEntityType.DisplayName(), RelatedEntityType.DisplayName(), principalEntityTypeName));
		}
		using IConventionBatch batch = principalEntityType.Model.ConventionDispatcher.DelayConventions();
		InternalRelationshipBuilder arg = Builder.HasEntityTypes(principalEntityType, GetOtherEntityType(principalEntityType), ConfigurationSource.Explicit);
		arg = hasPrincipalKey(arg);
		return batch.Run(arg);
	}

	[EntityFrameworkInternal]
	protected virtual EntityType ResolveEntityType([JetBrains.Annotations.NotNull] string entityTypeName)
	{
		if (DeclaringEntityType.Name == entityTypeName)
		{
			return (EntityType)DeclaringEntityType;
		}
		if (RelatedEntityType.Name == entityTypeName)
		{
			return (EntityType)RelatedEntityType;
		}
		if (DeclaringEntityType.DisplayName() == entityTypeName)
		{
			return (EntityType)DeclaringEntityType;
		}
		if (!(RelatedEntityType.DisplayName() == entityTypeName))
		{
			return null;
		}
		return (EntityType)RelatedEntityType;
	}

	[EntityFrameworkInternal]
	protected virtual EntityType ResolveEntityType([JetBrains.Annotations.NotNull] Type entityType)
	{
		if (DeclaringEntityType.ClrType == entityType)
		{
			return (EntityType)DeclaringEntityType;
		}
		if (!(RelatedEntityType.ClrType == entityType))
		{
			return null;
		}
		return (EntityType)RelatedEntityType;
	}

	private EntityType GetOtherEntityType(EntityType entityType)
	{
		if (DeclaringEntityType != entityType)
		{
			return (EntityType)DeclaringEntityType;
		}
		return (EntityType)RelatedEntityType;
	}

	public virtual ReferenceReferenceBuilder IsRequired(bool required = true)
	{
		return new ReferenceReferenceBuilder(Builder.IsRequired(required, ConfigurationSource.Explicit), this, inverted: false, foreignKeySet: false, principalKeySet: false, requiredSet: true);
	}

	public virtual ReferenceReferenceBuilder OnDelete(DeleteBehavior deleteBehavior)
	{
		return new ReferenceReferenceBuilder(Builder.OnDelete(deleteBehavior, ConfigurationSource.Explicit), this);
	}
}
public class ReferenceReferenceBuilder<TEntity, TRelatedEntity> : ReferenceReferenceBuilder where TEntity : class where TRelatedEntity : class
{
	[EntityFrameworkInternal]
	public ReferenceReferenceBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(declaringEntityType, relatedEntityType, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	protected ReferenceReferenceBuilder(InternalRelationshipBuilder builder, ReferenceReferenceBuilder oldBuilder, bool inverted = false, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
		: base(builder, oldBuilder, inverted, foreignKeySet, principalKeySet, requiredSet)
	{
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (ReferenceReferenceBuilder<TEntity, TRelatedEntity>)base.HasAnnotation(annotation, value);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasForeignKey([JetBrains.Annotations.NotNull] string dependentEntityTypeName, [JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(HasForeignKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityTypeName, "dependentEntityTypeName")), dependentEntityTypeName, Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyPropertyNames, "foreignKeyPropertyNames")), this, Builder.Metadata.DeclaringEntityType.Name != ResolveEntityType(dependentEntityTypeName).Name, foreignKeyPropertyNames.Length != 0);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasForeignKey([JetBrains.Annotations.NotNull] Type dependentEntityType, [JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(HasForeignKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependentEntityType, "dependentEntityType")), dependentEntityType.ShortDisplayName(), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyPropertyNames, "foreignKeyPropertyNames")), this, Builder.Metadata.DeclaringEntityType.ClrType != dependentEntityType, foreignKeyPropertyNames.Length != 0);
	}

	public virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasForeignKey<TDependentEntity>([JetBrains.Annotations.NotNull] params string[] foreignKeyPropertyNames) where TDependentEntity : class
	{
		return HasForeignKey(typeof(TDependentEntity), foreignKeyPropertyNames);
	}

	public virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasForeignKey<TDependentEntity>([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, object>> foreignKeyExpression) where TDependentEntity : class
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(HasForeignKeyBuilder(ResolveEntityType(typeof(TDependentEntity)), typeof(TDependentEntity).ShortDisplayName(), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeyExpression, "foreignKeyExpression").GetPropertyAccessList()), this, Builder.Metadata.DeclaringEntityType.ClrType != typeof(TDependentEntity), foreignKeySet: true);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasPrincipalKey([JetBrains.Annotations.NotNull] string principalEntityTypeName, [JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(HasPrincipalKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(principalEntityTypeName, "principalEntityTypeName")), principalEntityTypeName, Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyPropertyNames, "keyPropertyNames")), this, Builder.Metadata.PrincipalEntityType.Name != principalEntityTypeName, foreignKeySet: false, keyPropertyNames.Length != 0);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasPrincipalKey([JetBrains.Annotations.NotNull] Type principalEntityType, [JetBrains.Annotations.NotNull] params string[] keyPropertyNames)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(HasPrincipalKeyBuilder(ResolveEntityType(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType")), principalEntityType.ShortDisplayName(), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyPropertyNames, "keyPropertyNames")), this, Builder.Metadata.PrincipalEntityType.ClrType != principalEntityType, foreignKeySet: false, keyPropertyNames.Length != 0);
	}

	public virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasPrincipalKey<TPrincipalEntity>([JetBrains.Annotations.NotNull] params string[] keyPropertyNames) where TPrincipalEntity : class
	{
		return HasPrincipalKey(typeof(TPrincipalEntity), keyPropertyNames);
	}

	public virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasPrincipalKey<TPrincipalEntity>([JetBrains.Annotations.NotNull] Expression<Func<TPrincipalEntity, object>> keyExpression) where TPrincipalEntity : class
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(HasPrincipalKeyBuilder(ResolveEntityType(typeof(TPrincipalEntity)), typeof(TPrincipalEntity).ShortDisplayName(), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyExpression, "keyExpression").GetPropertyAccessList()), this, Builder.Metadata.PrincipalEntityType.ClrType != typeof(TPrincipalEntity), foreignKeySet: false, principalKeySet: true);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> IsRequired(bool required = true)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(Builder.IsRequired(required, ConfigurationSource.Explicit), this, inverted: false, foreignKeySet: false, principalKeySet: false, requiredSet: true);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> OnDelete(DeleteBehavior deleteBehavior)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(Builder.OnDelete(deleteBehavior, ConfigurationSource.Explicit), this);
	}
}
