using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

public class EntityTypeBuilder : IInfrastructure<InternalEntityTypeBuilder>
{
	private InternalEntityTypeBuilder Builder
	{
		[DebuggerStepThrough]
		get;
	}

	InternalEntityTypeBuilder IInfrastructure<InternalEntityTypeBuilder>.Instance => Builder;

	public virtual IMutableEntityType Metadata => Builder.Metadata;

	[EntityFrameworkInternal]
	public EntityTypeBuilder([JetBrains.Annotations.NotNull] IMutableEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Builder = ((EntityType)entityType).Builder;
	}

	public virtual EntityTypeBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual EntityTypeBuilder HasBaseType([JetBrains.Annotations.CanBeNull] string name)
	{
		return new EntityTypeBuilder(Builder.HasBaseType(name, ConfigurationSource.Explicit).Metadata);
	}

	public virtual EntityTypeBuilder HasBaseType([JetBrains.Annotations.CanBeNull] Type entityType)
	{
		return new EntityTypeBuilder(Builder.HasBaseType(entityType, ConfigurationSource.Explicit).Metadata);
	}

	public virtual KeyBuilder HasKey([JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		return new KeyBuilder(Builder.PrimaryKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual KeyBuilder HasAlternateKey([JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		return new KeyBuilder(Builder.HasKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual EntityTypeBuilder HasNoKey()
	{
		Builder.HasNoKey(ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder Property([JetBrains.Annotations.NotNull] string propertyName)
	{
		return new PropertyBuilder(Builder.Property(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual PropertyBuilder<TProperty> Property<TProperty>([JetBrains.Annotations.NotNull] string propertyName)
	{
		return new PropertyBuilder<TProperty>(Builder.Property(typeof(TProperty), Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual PropertyBuilder Property([JetBrains.Annotations.NotNull] Type propertyType, [JetBrains.Annotations.NotNull] string propertyName)
	{
		return new PropertyBuilder(Builder.Property(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyType, "propertyType"), Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual EntityTypeBuilder Ignore([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		Builder.Ignore(propertyName, ConfigurationSource.Explicit);
		return this;
	}

	public virtual EntityTypeBuilder HasQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression filter)
	{
		Builder.HasQueryFilter(filter, ConfigurationSource.Explicit);
		return this;
	}

	public virtual IndexBuilder HasIndex([JetBrains.Annotations.NotNull] params string[] propertyNames)
	{
		return new IndexBuilder(Builder.HasIndex(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames"), ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnedNavigationBuilder OwnsOne([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName"));
		return OwnsOneBuilder(in ownedType, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual OwnedNavigationBuilder OwnsOne([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType2 = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType"), (Model)Metadata.Model);
		return OwnsOneBuilder(in ownedType2, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual EntityTypeBuilder OwnsOne([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		TypeIdentity ownedType = new TypeIdentity(ownedTypeName);
		buildAction(OwnsOneBuilder(in ownedType, navigationName));
		return this;
	}

	public virtual EntityTypeBuilder OwnsOne([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		TypeIdentity ownedType2 = new TypeIdentity(ownedType, (Model)Metadata.Model);
		buildAction(OwnsOneBuilder(in ownedType2, navigationName));
		return this;
	}

	private OwnedNavigationBuilder OwnsOneBuilder(in TypeIdentity ownedType, string navigationName)
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((ownedType.Type == null) ? Builder.HasOwnership(ownedType.Name, navigationName, ConfigurationSource.Explicit) : Builder.HasOwnership(ownedType.Type, navigationName, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(true, ConfigurationSource.Explicit);
		}
		return new OwnedNavigationBuilder(Builder.Metadata, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual OwnedNavigationBuilder OwnsMany([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName"));
		return OwnsManyBuilder(in ownedType, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual OwnedNavigationBuilder OwnsMany([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName)
	{
		TypeIdentity ownedType2 = new TypeIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType"), (Model)Metadata.Model);
		return OwnsManyBuilder(in ownedType2, Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName"));
	}

	public virtual EntityTypeBuilder OwnsMany([JetBrains.Annotations.NotNull] string ownedTypeName, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(ownedTypeName, "ownedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		TypeIdentity ownedType = new TypeIdentity(ownedTypeName);
		buildAction(OwnsManyBuilder(in ownedType, navigationName));
		return this;
	}

	public virtual EntityTypeBuilder OwnsMany([JetBrains.Annotations.NotNull] Type ownedType, [JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(ownedType, "ownedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		TypeIdentity ownedType2 = new TypeIdentity(ownedType, (Model)Metadata.Model);
		buildAction(OwnsManyBuilder(in ownedType2, navigationName));
		return this;
	}

	private OwnedNavigationBuilder OwnsManyBuilder(in TypeIdentity ownedType, string navigationName)
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((ownedType.Type == null) ? Builder.HasOwnership(ownedType.Name, navigationName, ConfigurationSource.Explicit) : Builder.HasOwnership(ownedType.Type, navigationName, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(false, ConfigurationSource.Explicit);
		}
		return new OwnedNavigationBuilder(Builder.Metadata, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual ReferenceNavigationBuilder HasOne([JetBrains.Annotations.NotNull] string relatedTypeName, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(relatedTypeName, "relatedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(relatedTypeName, navigationName);
		return new ReferenceNavigationBuilder(Builder.Metadata, entityType, navigationName, Builder.HasRelationship(entityType, navigationName, ConfigurationSource.Explicit, Builder.Metadata == entityType).Metadata);
	}

	public virtual ReferenceNavigationBuilder HasOne([JetBrains.Annotations.NotNull] Type relatedType, [JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relatedType, "relatedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(relatedType, navigationName);
		return new ReferenceNavigationBuilder(Builder.Metadata, entityType, navigationName, Builder.HasRelationship(entityType, navigationName, ConfigurationSource.Explicit, Builder.Metadata == entityType).Metadata);
	}

	public virtual ReferenceNavigationBuilder HasOne([JetBrains.Annotations.NotNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		if (!(Metadata.ClrType == null))
		{
			return HasOne(Metadata.GetNavigationMemberInfo(navigationName).GetMemberType(), navigationName);
		}
		return HasOne(navigationName, null);
	}

	public virtual CollectionNavigationBuilder HasMany([JetBrains.Annotations.NotNull] string relatedTypeName, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(relatedTypeName, "relatedTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(relatedTypeName, navigationName);
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch batch = Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = entityType.Builder.HasRelationship(Builder.Metadata, ConfigurationSource.Explicit).IsUnique(false, ConfigurationSource.Explicit).HasEntityTypes(Builder.Metadata, entityType, ConfigurationSource.Explicit)
				.HasNavigation(navigationName, pointsToPrincipal: false, ConfigurationSource.Explicit);
			internalRelationshipBuilder = batch.Run(internalRelationshipBuilder);
		}
		return new CollectionNavigationBuilder(Builder.Metadata, entityType, navigationName, internalRelationshipBuilder.Metadata);
	}

	public virtual CollectionNavigationBuilder HasMany([JetBrains.Annotations.NotNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(navigationName, "navigationName");
		if (Metadata.ClrType == null)
		{
			return HasMany(navigationName, null);
		}
		Type memberType = Metadata.GetNavigationMemberInfo(navigationName).GetMemberType();
		Type type = memberType.TryGetElementType(typeof(IEnumerable<>));
		if (type == null)
		{
			throw new InvalidOperationException(CoreStrings.NavigationCollectionWrongClrType(navigationName, Metadata.DisplayName(), memberType.ShortDisplayName(), "T"));
		}
		return HasMany(type, navigationName);
	}

	public virtual CollectionNavigationBuilder HasMany([JetBrains.Annotations.NotNull] Type relatedType, [JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relatedType, "relatedType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(relatedType, navigationName);
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch batch = Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = entityType.Builder.HasRelationship(Builder.Metadata, ConfigurationSource.Explicit).IsUnique(false, ConfigurationSource.Explicit).HasEntityTypes(Builder.Metadata, entityType, ConfigurationSource.Explicit)
				.HasNavigation(navigationName, pointsToPrincipal: false, ConfigurationSource.Explicit);
			internalRelationshipBuilder = batch.Run(internalRelationshipBuilder);
		}
		return new CollectionNavigationBuilder(Builder.Metadata, entityType, navigationName, internalRelationshipBuilder.Metadata);
	}

	[EntityFrameworkInternal]
	protected virtual EntityType FindRelatedEntityType([JetBrains.Annotations.NotNull] string relatedTypeName, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		return ((navigationName == null) ? null : Builder.ModelBuilder.Metadata.FindEntityType(relatedTypeName, navigationName, Builder.Metadata)) ?? Builder.ModelBuilder.Entity(relatedTypeName, ConfigurationSource.Explicit, false).Metadata;
	}

	[EntityFrameworkInternal]
	protected virtual EntityType FindRelatedEntityType([JetBrains.Annotations.NotNull] Type relatedType, [JetBrains.Annotations.CanBeNull] string navigationName)
	{
		return ((navigationName == null) ? null : Builder.ModelBuilder.Metadata.FindEntityType(relatedType, navigationName, Builder.Metadata)) ?? Builder.ModelBuilder.Entity(relatedType, ConfigurationSource.Explicit, false).Metadata;
	}

	public virtual EntityTypeBuilder HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy)
	{
		Builder.HasChangeTrackingStrategy(changeTrackingStrategy, ConfigurationSource.Explicit);
		return this;
	}

	public virtual EntityTypeBuilder UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		Builder.UsePropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
		return this;
	}

	public virtual DataBuilder HasData([JetBrains.Annotations.NotNull] params object[] data)
	{
		return HasData((IEnumerable<object>)data);
	}

	public virtual DataBuilder HasData([JetBrains.Annotations.NotNull] IEnumerable<object> data)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(data, "data");
		Metadata.AddData(data);
		return new DataBuilder();
	}

	public virtual DiscriminatorBuilder HasDiscriminator()
	{
		return Builder.DiscriminatorBuilder(Builder.GetOrCreateDiscriminatorProperty(null, null, fromDataAnnotation: true), ConfigurationSource.Explicit);
	}

	public virtual DiscriminatorBuilder HasDiscriminator([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return Builder.DiscriminatorBuilder(Property(type, name).GetInfrastructure(), ConfigurationSource.Explicit);
	}

	public virtual DiscriminatorBuilder<TDiscriminator> HasDiscriminator<TDiscriminator>([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return new DiscriminatorBuilder<TDiscriminator>(Builder.DiscriminatorBuilder(Property(typeof(TDiscriminator), name).GetInfrastructure(), ConfigurationSource.Explicit));
	}

	public virtual EntityTypeBuilder HasNoDiscriminator()
	{
		Builder.HasNoDeclaredDiscriminator(ConfigurationSource.Explicit);
		return this;
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
public class EntityTypeBuilder<TEntity> : EntityTypeBuilder where TEntity : class
{
	private InternalEntityTypeBuilder Builder => this.GetInfrastructure();

	[EntityFrameworkInternal]
	public EntityTypeBuilder([JetBrains.Annotations.NotNull] IMutableEntityType entityType)
		: base(entityType)
	{
	}

	public new virtual EntityTypeBuilder<TEntity> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (EntityTypeBuilder<TEntity>)base.HasAnnotation(annotation, value);
	}

	public new virtual EntityTypeBuilder<TEntity> HasBaseType([JetBrains.Annotations.CanBeNull] string name)
	{
		return new EntityTypeBuilder<TEntity>(Builder.HasBaseType(name, ConfigurationSource.Explicit).Metadata);
	}

	public new virtual EntityTypeBuilder<TEntity> HasBaseType([JetBrains.Annotations.CanBeNull] Type entityType)
	{
		return new EntityTypeBuilder<TEntity>(Builder.HasBaseType(entityType, ConfigurationSource.Explicit).Metadata);
	}

	public virtual EntityTypeBuilder<TEntity> HasBaseType<TBaseType>()
	{
		return HasBaseType(typeof(TBaseType));
	}

	public virtual KeyBuilder HasKey([JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> keyExpression)
	{
		return new KeyBuilder(Builder.PrimaryKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyExpression, "keyExpression").GetPropertyAccessList(), ConfigurationSource.Explicit).Metadata);
	}

	public virtual KeyBuilder HasAlternateKey([JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> keyExpression)
	{
		return new KeyBuilder(Builder.HasKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(keyExpression, "keyExpression").GetPropertyAccessList(), ConfigurationSource.Explicit).Metadata);
	}

	public new virtual EntityTypeBuilder<TEntity> HasNoKey()
	{
		return (EntityTypeBuilder<TEntity>)base.HasNoKey();
	}

	public virtual PropertyBuilder<TProperty> Property<TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, TProperty>> propertyExpression)
	{
		return new PropertyBuilder<TProperty>(Builder.Property(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression").GetPropertyAccess(), ConfigurationSource.Explicit).Metadata);
	}

	public virtual EntityTypeBuilder<TEntity> Ignore([JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> propertyExpression)
	{
		return (EntityTypeBuilder<TEntity>)base.Ignore(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression").GetPropertyAccess().GetSimpleMemberName());
	}

	public new virtual EntityTypeBuilder<TEntity> Ignore([JetBrains.Annotations.NotNull] string propertyName)
	{
		return (EntityTypeBuilder<TEntity>)base.Ignore(propertyName);
	}

	public virtual EntityTypeBuilder<TEntity> HasQueryFilter([JetBrains.Annotations.CanBeNull] Expression<Func<TEntity, bool>> filter)
	{
		return (EntityTypeBuilder<TEntity>)base.HasQueryFilter(filter);
	}

	public virtual EntityTypeBuilder<TEntity> ToQuery([JetBrains.Annotations.NotNull] Expression<Func<IQueryable<TEntity>>> query)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(query, "query");
		Builder.HasDefiningQuery(query, ConfigurationSource.Explicit);
		return this;
	}

	public virtual IndexBuilder<TEntity> HasIndex([JetBrains.Annotations.NotNull] Expression<Func<TEntity, object>> indexExpression)
	{
		return new IndexBuilder<TEntity>(Builder.HasIndex(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexExpression, "indexExpression").GetPropertyAccessList(), ConfigurationSource.Explicit).Metadata);
	}

	public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<TRelatedEntity>([JetBrains.Annotations.NotNull] string navigationName) where TRelatedEntity : class
	{
		return OwnsOneBuilder<TRelatedEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName")));
	}

	public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOne<TRelatedEntity>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, TRelatedEntity>> navigationExpression) where TRelatedEntity : class
	{
		return OwnsOneBuilder<TRelatedEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression").GetPropertyAccess()));
	}

	public virtual EntityTypeBuilder<TEntity> OwnsOne<TRelatedEntity>([JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(OwnsOneBuilder<TRelatedEntity>(new MemberIdentity(navigationName)));
		return this;
	}

	public virtual EntityTypeBuilder<TEntity> OwnsOne<TRelatedEntity>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, TRelatedEntity>> navigationExpression, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(OwnsOneBuilder<TRelatedEntity>(new MemberIdentity(navigationExpression.GetPropertyAccess())));
		return this;
	}

	private OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsOneBuilder<TRelatedEntity>(MemberIdentity navigation) where TRelatedEntity : class
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch conventionBatch = Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((navigation.MemberInfo == null) ? Builder.HasOwnership(typeof(TRelatedEntity), navigation.Name, ConfigurationSource.Explicit) : Builder.HasOwnership(typeof(TRelatedEntity), (PropertyInfo)navigation.MemberInfo, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(true, ConfigurationSource.Explicit);
			internalRelationshipBuilder = (InternalRelationshipBuilder)conventionBatch.Run(internalRelationshipBuilder.Metadata).Builder;
		}
		return new OwnedNavigationBuilder<TEntity, TRelatedEntity>(Builder.Metadata, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<TRelatedEntity>([JetBrains.Annotations.NotNull] string navigationName) where TRelatedEntity : class
	{
		return OwnsManyBuilder<TRelatedEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName")));
	}

	public virtual OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsMany<TRelatedEntity>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression) where TRelatedEntity : class
	{
		return OwnsManyBuilder<TRelatedEntity>(new MemberIdentity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression").GetPropertyAccess()));
	}

	public virtual EntityTypeBuilder<TEntity> OwnsMany<TRelatedEntity>([JetBrains.Annotations.NotNull] string navigationName, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(OwnsManyBuilder<TRelatedEntity>(new MemberIdentity(navigationName)));
		return this;
	}

	public virtual EntityTypeBuilder<TEntity> OwnsMany<TRelatedEntity>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression, [JetBrains.Annotations.NotNull] Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> buildAction) where TRelatedEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationExpression, "navigationExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(OwnsManyBuilder<TRelatedEntity>(new MemberIdentity(navigationExpression.GetPropertyAccess())));
		return this;
	}

	private OwnedNavigationBuilder<TEntity, TRelatedEntity> OwnsManyBuilder<TRelatedEntity>(MemberIdentity navigation) where TRelatedEntity : class
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch conventionBatch = Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = ((navigation.MemberInfo == null) ? Builder.HasOwnership(typeof(TRelatedEntity), navigation.Name, ConfigurationSource.Explicit) : Builder.HasOwnership(typeof(TRelatedEntity), (PropertyInfo)navigation.MemberInfo, ConfigurationSource.Explicit));
			internalRelationshipBuilder.IsUnique(false, ConfigurationSource.Explicit);
			internalRelationshipBuilder = (InternalRelationshipBuilder)conventionBatch.Run(internalRelationshipBuilder.Metadata).Builder;
		}
		return new OwnedNavigationBuilder<TEntity, TRelatedEntity>(Builder.Metadata, internalRelationshipBuilder.Metadata.DeclaringEntityType, internalRelationshipBuilder);
	}

	public virtual ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<TRelatedEntity>([JetBrains.Annotations.CanBeNull] string navigationName) where TRelatedEntity : class
	{
		EntityType entityType = FindRelatedEntityType(typeof(TRelatedEntity), navigationName);
		return new ReferenceNavigationBuilder<TEntity, TRelatedEntity>(Builder.Metadata, entityType, navigationName, Builder.HasRelationship(entityType, navigationName, ConfigurationSource.Explicit, Builder.Metadata == entityType).Metadata);
	}

	public virtual ReferenceNavigationBuilder<TEntity, TRelatedEntity> HasOne<TRelatedEntity>([JetBrains.Annotations.CanBeNull] Expression<Func<TEntity, TRelatedEntity>> navigationExpression = null) where TRelatedEntity : class
	{
		PropertyInfo propertyInfo = navigationExpression?.GetPropertyAccess();
		EntityType entityType = FindRelatedEntityType(typeof(TRelatedEntity), propertyInfo?.GetSimpleMemberName());
		return new ReferenceNavigationBuilder<TEntity, TRelatedEntity>(Builder.Metadata, entityType, propertyInfo, Builder.HasRelationship(entityType, propertyInfo, ConfigurationSource.Explicit, Builder.Metadata == entityType).Metadata);
	}

	public virtual CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<TRelatedEntity>([JetBrains.Annotations.CanBeNull] string navigationName) where TRelatedEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName");
		EntityType entityType = FindRelatedEntityType(typeof(TRelatedEntity), navigationName);
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch batch = Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = entityType.Builder.HasRelationship(Builder.Metadata, ConfigurationSource.Explicit).IsUnique(false, ConfigurationSource.Explicit).HasEntityTypes(Builder.Metadata, entityType, ConfigurationSource.Explicit)
				.HasNavigation(navigationName, pointsToPrincipal: false, ConfigurationSource.Explicit);
			internalRelationshipBuilder = batch.Run(internalRelationshipBuilder);
		}
		return new CollectionNavigationBuilder<TEntity, TRelatedEntity>(Builder.Metadata, entityType, navigationName, internalRelationshipBuilder.Metadata);
	}

	public virtual CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<TRelatedEntity>([JetBrains.Annotations.CanBeNull] Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression = null) where TRelatedEntity : class
	{
		PropertyInfo propertyInfo = navigationExpression?.GetPropertyAccess();
		EntityType entityType = FindRelatedEntityType(typeof(TRelatedEntity), propertyInfo?.GetSimpleMemberName());
		InternalRelationshipBuilder internalRelationshipBuilder;
		using (IConventionBatch batch = Builder.Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = entityType.Builder.HasRelationship(Builder.Metadata, ConfigurationSource.Explicit).IsUnique(false, ConfigurationSource.Explicit).HasEntityTypes(Builder.Metadata, entityType, ConfigurationSource.Explicit)
				.HasNavigation(propertyInfo, pointsToPrincipal: false, ConfigurationSource.Explicit);
			internalRelationshipBuilder = batch.Run(internalRelationshipBuilder);
		}
		return new CollectionNavigationBuilder<TEntity, TRelatedEntity>(Builder.Metadata, entityType, propertyInfo, internalRelationshipBuilder.Metadata);
	}

	public new virtual EntityTypeBuilder<TEntity> HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy)
	{
		return (EntityTypeBuilder<TEntity>)base.HasChangeTrackingStrategy(changeTrackingStrategy);
	}

	public new virtual EntityTypeBuilder<TEntity> UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		return (EntityTypeBuilder<TEntity>)base.UsePropertyAccessMode(propertyAccessMode);
	}

	public virtual DataBuilder<TEntity> HasData([JetBrains.Annotations.NotNull] params TEntity[] data)
	{
		return HasData((IEnumerable<object>)data);
	}

	public virtual DataBuilder<TEntity> HasData([JetBrains.Annotations.NotNull] IEnumerable<TEntity> data)
	{
		return HasData((IEnumerable<object>)data);
	}

	public new virtual DataBuilder<TEntity> HasData([JetBrains.Annotations.NotNull] params object[] data)
	{
		return HasData((IEnumerable<object>)data);
	}

	public new virtual DataBuilder<TEntity> HasData([JetBrains.Annotations.NotNull] IEnumerable<object> data)
	{
		base.HasData(data);
		return new DataBuilder<TEntity>();
	}

	public virtual DiscriminatorBuilder<TDiscriminator> HasDiscriminator<TDiscriminator>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, TDiscriminator>> propertyExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression");
		return new DiscriminatorBuilder<TDiscriminator>(Builder.DiscriminatorBuilder(Property(propertyExpression).GetInfrastructure(), ConfigurationSource.Explicit));
	}

	public new virtual EntityTypeBuilder<TEntity> HasNoDiscriminator()
	{
		return (EntityTypeBuilder<TEntity>)base.HasNoDiscriminator();
	}
}
