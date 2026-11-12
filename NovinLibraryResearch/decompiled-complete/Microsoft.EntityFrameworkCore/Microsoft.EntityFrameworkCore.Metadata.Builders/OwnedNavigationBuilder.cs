using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OwnedNavigationBuilder : IInfrastructure<InternalEntityTypeBuilder>
{
	private InternalRelationshipBuilder _builder;

	protected virtual EntityType PrincipalEntityType { get; }

	protected virtual EntityType DependentEntityType { get; }

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder Builder
	{
		get
		{
			if (_builder.Metadata.Builder == null)
			{
				_builder = PrincipalEntityType.FindNavigation(_builder.Metadata.PrincipalToDependent.Name)?.ForeignKey.Builder;
			}
			return _builder;
		}
		private set
		{
			_builder = value;
		}
	}

	InternalEntityTypeBuilder IInfrastructure<InternalEntityTypeBuilder>.Instance => DependentEntityType.Builder;

	public virtual IMutableForeignKey Metadata => Builder.Metadata;

	public virtual IMutableEntityType OwnedEntityType => DependentEntityType;

	[EntityFrameworkInternal]
	public OwnedNavigationBuilder([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, [JetBrains.Annotations.NotNull] InternalRelationshipBuilder builder)
	{
		PrincipalEntityType = principalEntityType;
		DependentEntityType = dependentEntityType;
		_builder = builder;
	}

	[EntityFrameworkInternal]
	protected virtual T UpdateBuilder<T>([JetBrains.Annotations.NotNull] Func<T> configure)
	{
		ForeignKey foreignKey = _builder.Metadata;
		T result = DependentEntityType.Model.ConventionDispatcher.Run(configure, ref foreignKey);
		if (foreignKey != null)
		{
			_builder = foreignKey.Builder;
		}
		return result;
	}

	public virtual OwnedNavigationBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		DependentEntityType.Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual KeyBuilder HasKey([JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		return new KeyBuilder(DependentEntityType.Builder.PrimaryKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual PropertyBuilder Property([JetBrains.Annotations.NotNull] string propertyName)
	{
		return UpdateBuilder(() => new PropertyBuilder(DependentEntityType.Builder.Property(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName"), ConfigurationSource.Explicit).Metadata));
	}

	public virtual PropertyBuilder<TProperty> Property<TProperty>([JetBrains.Annotations.NotNull] string propertyName)
	{
		return UpdateBuilder(() => new PropertyBuilder<TProperty>(DependentEntityType.Builder.Property(typeof(TProperty), Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName"), ConfigurationSource.Explicit).Metadata));
	}

	public virtual PropertyBuilder Property([JetBrains.Annotations.NotNull] Type propertyType, [JetBrains.Annotations.NotNull] string propertyName)
	{
		return new PropertyBuilder(DependentEntityType.Builder.Property(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyType, "propertyType"), Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnedNavigationBuilder Ignore([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		DependentEntityType.Builder.Ignore(propertyName, ConfigurationSource.Explicit);
		return this;
	}

	public virtual IndexBuilder HasIndex([JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		return new IndexBuilder(DependentEntityType.Builder.HasIndex(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnershipBuilder WithOwner([JetBrains.Annotations.CanBeNull] string ownerReference = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(ownerReference, "ownerReference");
		return new OwnershipBuilder(PrincipalEntityType, DependentEntityType, Builder.HasNavigation(ownerReference, pointsToPrincipal: true, ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnedNavigationBuilder OwnsOne([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName"));
		return OwnsOneBuilder(in ownedType, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual OwnedNavigationBuilder OwnsOne([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType2 = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType"), (Model)OwnedEntityType.Model);
		return OwnsOneBuilder(in ownedType2, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual OwnedNavigationBuilder OwnsOne([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			TypeIdentity ownedType = new TypeIdentity(ownedTypeName);
			buildAction(OwnsOneBuilder(in ownedType, navigationName));
			return this;
		}
	}

	public virtual OwnedNavigationBuilder OwnsOne([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			TypeIdentity ownedType2 = new TypeIdentity(ownedType, (Model)OwnedEntityType.Model);
			buildAction(OwnsOneBuilder(in ownedType2, navigationName));
			return this;
		}
	}

	private OwnedNavigationBuilder OwnsOneBuilder(in TypeIdentity ownedType, string navigationName)
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((ownedType.Type == null) ? DependentEntityType.Builder.HasOwnership(ownedType.Name, navigationName, ConfigurationSource.Explicit) : DependentEntityType.Builder.HasOwnership(ownedType.Type, navigationName, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(true, ConfigurationSource.Explicit);
		}
		return new OwnedNavigationBuilder(DependentEntityType, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual OwnedNavigationBuilder OwnsMany([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName"));
		return OwnsManyBuilder(in ownedType, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual OwnedNavigationBuilder OwnsMany([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType2 = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType"), DependentEntityType.Model);
		return OwnsManyBuilder(in ownedType2, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual OwnedNavigationBuilder OwnsMany([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			TypeIdentity ownedType = new TypeIdentity(ownedTypeName);
			buildAction(OwnsManyBuilder(in ownedType, navigationName));
			return this;
		}
	}

	public virtual OwnedNavigationBuilder OwnsMany([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			TypeIdentity ownedType2 = new TypeIdentity(ownedType, DependentEntityType.Model);
			buildAction(OwnsManyBuilder(in ownedType2, navigationName));
			return this;
		}
	}

	private OwnedNavigationBuilder OwnsManyBuilder(in TypeIdentity ownedType, string navigationName)
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((ownedType.Type == null) ? DependentEntityType.Builder.HasOwnership(ownedType.Name, navigationName, ConfigurationSource.Explicit) : DependentEntityType.Builder.HasOwnership(ownedType.Type, navigationName, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(false, ConfigurationSource.Explicit);
		}
		return new OwnedNavigationBuilder(DependentEntityType, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual ReferenceNavigationBuilder HasOne([JetBrains.Annotations.NotNull] string relatedTypeName, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(relatedTypeName, "relatedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(relatedTypeName, navigationName);
		return new ReferenceNavigationBuilder(DependentEntityType, entityType, navigationName, DependentEntityType.Builder.HasRelationship(entityType, navigationName, ConfigurationSource.Explicit, DependentEntityType == entityType).Metadata);
	}

	public virtual ReferenceNavigationBuilder HasOne([JetBrains.Annotations.NotNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		if (!(OwnedEntityType.ClrType == null))
		{
			return HasOne(OwnedEntityType.GetNavigationMemberInfo(navigationName).GetMemberType(), navigationName);
		}
		return HasOne(navigationName, null);
	}

	public virtual ReferenceNavigationBuilder HasOne([JetBrains.Annotations.NotNull] Type relatedType, [JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relatedType, "relatedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(relatedType, navigationName);
		return new ReferenceNavigationBuilder(DependentEntityType, entityType, navigationName, DependentEntityType.Builder.HasRelationship(entityType, navigationName, ConfigurationSource.Explicit, DependentEntityType == entityType).Metadata);
	}

	[EntityFrameworkInternal]
	protected virtual EntityType FindRelatedEntityType([JetBrains.Annotations.NotNull] string relatedTypeName, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		EntityType entityType = DependentEntityType.FindInDefinitionPath(relatedTypeName);
		if (entityType != null)
		{
			return entityType;
		}
		Model model = DependentEntityType.Model;
		if (navigationName != null)
		{
			entityType = model.FindEntityType(relatedTypeName, navigationName, DependentEntityType);
		}
		if (entityType == null)
		{
			string productVersion = model.GetProductVersion();
			if (productVersion != null && productVersion.StartsWith("2.", StringComparison.Ordinal))
			{
				EntityType principalEntityType = DependentEntityType.FindOwnership().PrincipalEntityType;
				if (principalEntityType.Name == relatedTypeName)
				{
					entityType = principalEntityType;
				}
			}
		}
		return entityType ?? Builder.ModelBuilder.Entity(relatedTypeName, ConfigurationSource.Explicit, false).Metadata;
	}

	[EntityFrameworkInternal]
	protected virtual EntityType FindRelatedEntityType([JetBrains.Annotations.NotNull] Type relatedType, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		EntityType entityType = DependentEntityType.FindInDefinitionPath(relatedType);
		if (entityType != null)
		{
			return entityType;
		}
		if (navigationName != null)
		{
			entityType = Builder.ModelBuilder.Metadata.FindEntityType(relatedType, navigationName, DependentEntityType);
		}
		return entityType ?? DependentEntityType.Builder.ModelBuilder.Entity(relatedType, ConfigurationSource.Explicit, false).Metadata;
	}

	public virtual OwnedNavigationBuilder HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy)
	{
		DependentEntityType.Builder.HasChangeTrackingStrategy(changeTrackingStrategy, ConfigurationSource.Explicit);
		return this;
	}

	public virtual OwnedNavigationBuilder UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		DependentEntityType.Builder.UsePropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
		return this;
	}

	public virtual DataBuilder HasData([JetBrains.Annotations.NotNull] params object[] data)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(data, "data");
		OwnedEntityType.AddData(data);
		return new DataBuilder();
	}
}
public class OwnedNavigationBuilder<TEntity, TDependentEntity> : OwnedNavigationBuilder where TEntity : class where TDependentEntity : class
{
	[EntityFrameworkInternal]
	public OwnedNavigationBuilder([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] EntityType dependentEntityType, [JetBrains.Annotations.NotNull] InternalRelationshipBuilder builder)
		: base(principalEntityType, dependentEntityType, builder)
	{
	}

	public new virtual OwnedNavigationBuilder<TEntity, TDependentEntity> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (OwnedNavigationBuilder<TEntity, TDependentEntity>)base.HasAnnotation(annotation, value);
	}

	public virtual KeyBuilder HasKey([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, object>> keyExpression)
	{
		return new KeyBuilder(DependentEntityType.Builder.PrimaryKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyExpression, "keyExpression").GetPropertyAccessList(), ConfigurationSource.Explicit).Metadata);
	}

	public virtual PropertyBuilder<TProperty> Property<TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, TProperty>> propertyExpression)
	{
		return UpdateBuilder(() => new PropertyBuilder<TProperty>(DependentEntityType.Builder.Property(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression").GetPropertyAccess(), ConfigurationSource.Explicit).Metadata));
	}

	public new virtual OwnedNavigationBuilder<TEntity, TDependentEntity> Ignore([JetBrains.Annotations.NotNull] string propertyName)
	{
		return (OwnedNavigationBuilder<TEntity, TDependentEntity>)base.Ignore(propertyName);
	}

	public virtual OwnedNavigationBuilder<TEntity, TDependentEntity> Ignore([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, object>> propertyExpression)
	{
		return (OwnedNavigationBuilder<TEntity, TDependentEntity>)base.Ignore(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression").GetPropertyAccess().GetSimpleMemberName());
	}

	public virtual IndexBuilder HasIndex([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, object>> indexExpression)
	{
		return new IndexBuilder(DependentEntityType.Builder.HasIndex(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexExpression, "indexExpression").GetPropertyAccessList(), ConfigurationSource.Explicit).Metadata);
	}

	public new virtual OwnershipBuilder<TEntity, TDependentEntity> WithOwner([JetBrains.Annotations.CanBeNull] string ownerReference = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(ownerReference, "ownerReference");
		return new OwnershipBuilder<TEntity, TDependentEntity>(PrincipalEntityType, DependentEntityType, Builder.HasNavigation(ownerReference, pointsToPrincipal: true, ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnershipBuilder<TEntity, TDependentEntity> WithOwner([JetBrains.Annotations.CanBeNull] Expression<Func<TDependentEntity, TEntity>> referenceExpression)
	{
		return new OwnershipBuilder<TEntity, TDependentEntity>(PrincipalEntityType, DependentEntityType, Builder.HasNavigation(referenceExpression?.GetPropertyAccess(), pointsToPrincipal: true, ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity> OwnsOne<TNewDependentEntity>([JetBrains.Annotations.NotNull] string navigationName) where TNewDependentEntity : class
	{
		return OwnsOneBuilder<TNewDependentEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName")));
	}

	public virtual OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity> OwnsOne<TNewDependentEntity>([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, TNewDependentEntity>> navigationExpression) where TNewDependentEntity : class
	{
		return OwnsOneBuilder<TNewDependentEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression").GetPropertyAccess()));
	}

	public virtual OwnedNavigationBuilder<TEntity, TDependentEntity> OwnsOne<TNewDependentEntity>([JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity>> buildAction) where TNewDependentEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(OwnsOneBuilder<TNewDependentEntity>(new MemberIdentity(navigationName)));
		return this;
	}

	public virtual OwnedNavigationBuilder<TEntity, TDependentEntity> OwnsOne<TNewDependentEntity>([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, TNewDependentEntity>> navigationExpression, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity>> buildAction) where TNewDependentEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(OwnsOneBuilder<TNewDependentEntity>(new MemberIdentity(navigationExpression.GetPropertyAccess())));
		return this;
	}

	private OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity> OwnsOneBuilder<TNewDependentEntity>(MemberIdentity navigation) where TNewDependentEntity : class
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch conventionBatch = DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((navigation.MemberInfo == null) ? DependentEntityType.Builder.HasOwnership(typeof(TNewDependentEntity), navigation.Name, ConfigurationSource.Explicit) : DependentEntityType.Builder.HasOwnership(typeof(TNewDependentEntity), (PropertyInfo)navigation.MemberInfo, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(true, ConfigurationSource.Explicit);
			internalRelationshipBuilder = (InternalRelationshipBuilder)conventionBatch.Run(internalRelationshipBuilder.Metadata).Builder;
		}
		return new OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity>(internalRelationshipBuilder.Metadata.PrincipalEntityType, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity> OwnsMany<TNewDependentEntity>([JetBrains.Annotations.NotNull] string navigationName) where TNewDependentEntity : class
	{
		return OwnsManyBuilder<TNewDependentEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName")));
	}

	public virtual OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity> OwnsMany<TNewDependentEntity>([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, IEnumerable<TNewDependentEntity>>> navigationExpression) where TNewDependentEntity : class
	{
		return OwnsManyBuilder<TNewDependentEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression").GetPropertyAccess()));
	}

	public virtual OwnedNavigationBuilder<TEntity, TDependentEntity> OwnsMany<TNewDependentEntity>([JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity>> buildAction) where TNewDependentEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			buildAction(OwnsManyBuilder<TNewDependentEntity>(new MemberIdentity(navigationName)));
			return this;
		}
	}

	public virtual OwnedNavigationBuilder<TEntity, TDependentEntity> OwnsMany<TNewDependentEntity>([JetBrains.Annotations.NotNull] Expression<Func<TDependentEntity, IEnumerable<TNewDependentEntity>>> navigationExpression, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TDependentEntity, TNewDependentEntity>> buildAction) where TNewDependentEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		using (DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			buildAction(OwnsManyBuilder<TNewDependentEntity>(new MemberIdentity(navigationExpression.GetPropertyAccess())));
			return this;
		}
	}

	private OwnedNavigationBuilder<TDependentEntity, TNewRelatedEntity> OwnsManyBuilder<TNewRelatedEntity>(MemberIdentity navigation) where TNewRelatedEntity : class
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch conventionBatch = DependentEntityType.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((navigation.MemberInfo == null) ? DependentEntityType.Builder.HasOwnership(typeof(TNewRelatedEntity), navigation.Name, ConfigurationSource.Explicit) : DependentEntityType.Builder.HasOwnership(typeof(TNewRelatedEntity), (PropertyInfo)navigation.MemberInfo, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(false, ConfigurationSource.Explicit);
			internalRelationshipBuilder = (InternalRelationshipBuilder)conventionBatch.Run(internalRelationshipBuilder.Metadata).Builder;
		}
		return new OwnedNavigationBuilder<TDependentEntity, TNewRelatedEntity>(DependentEntityType, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual ReferenceNavigationBuilder<TDependentEntity, TNewRelatedEntity> HasOne<TNewRelatedEntity>([JetBrains.Annotations.CanBeNull] string navigationName) where TNewRelatedEntity : class
	{
		EntityType entityType = FindRelatedEntityType(typeof(TNewRelatedEntity), navigationName);
		return new ReferenceNavigationBuilder<TDependentEntity, TNewRelatedEntity>(DependentEntityType, entityType, navigationName, DependentEntityType.Builder.HasRelationship(entityType, navigationName, ConfigurationSource.Explicit, DependentEntityType == entityType).Metadata);
	}

	public virtual ReferenceNavigationBuilder<TDependentEntity, TNewRelatedEntity> HasOne<TNewRelatedEntity>([JetBrains.Annotations.CanBeNull] Expression<Func<TDependentEntity, TNewRelatedEntity>> navigationExpression = null) where TNewRelatedEntity : class
	{
		PropertyInfo propertyInfo = navigationExpression?.GetPropertyAccess();
		EntityType entityType = FindRelatedEntityType(typeof(TNewRelatedEntity), propertyInfo?.GetSimpleMemberName());
		return new ReferenceNavigationBuilder<TDependentEntity, TNewRelatedEntity>(DependentEntityType, entityType, propertyInfo, DependentEntityType.Builder.HasRelationship(entityType, propertyInfo, ConfigurationSource.Explicit, DependentEntityType == entityType).Metadata);
	}

	public new virtual OwnedNavigationBuilder<TEntity, TDependentEntity> HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy)
	{
		return (OwnedNavigationBuilder<TEntity, TDependentEntity>)base.HasChangeTrackingStrategy(changeTrackingStrategy);
	}

	public new virtual OwnedNavigationBuilder<TEntity, TDependentEntity> UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		return (OwnedNavigationBuilder<TEntity, TDependentEntity>)base.UsePropertyAccessMode(propertyAccessMode);
	}

	public virtual DataBuilder<TDependentEntity> HasData([JetBrains.Annotations.NotNull] params TDependentEntity[] data)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(data, "data");
		OwnedEntityType.AddData(data);
		return new DataBuilder<TDependentEntity>();
	}

	public new virtual DataBuilder<TDependentEntity> HasData([JetBrains.Annotations.NotNull] params object[] data)
	{
		base.HasData(data);
		return new DataBuilder<TDependentEntity>();
	}
}
