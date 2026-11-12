using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CollectionNavigationBuilder : IInfrastructure<InternalRelationshipBuilder>
{
	private InternalRelationshipBuilder Builder { get; }

	[EntityFrameworkInternal]
	protected virtual string CollectionName { get; }

	[EntityFrameworkInternal]
	protected virtual MemberInfo CollectionMember { get; }

	[EntityFrameworkInternal]
	protected virtual IMutableEntityType RelatedEntityType { get; }

	[EntityFrameworkInternal]
	protected virtual IMutableEntityType DeclaringEntityType { get; }

	InternalRelationshipBuilder IInfrastructure<InternalRelationshipBuilder>.Instance => Builder;

	[EntityFrameworkInternal]
	public CollectionNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] string navigationName, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		DeclaringEntityType = declaringEntityType;
		RelatedEntityType = relatedEntityType;
		CollectionName = navigationName;
		Builder = ((ForeignKey)foreignKey).Builder;
	}

	[EntityFrameworkInternal]
	public CollectionNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		DeclaringEntityType = declaringEntityType;
		RelatedEntityType = relatedEntityType;
		CollectionMember = navigationMemberInfo;
		CollectionName = navigationMemberInfo?.GetSimpleMemberName();
		Builder = ((ForeignKey)foreignKey).Builder;
	}

	public virtual ReferenceCollectionBuilder WithOne([JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		return new ReferenceCollectionBuilder(DeclaringEntityType, RelatedEntityType, WithOneBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName")).Metadata);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder WithOneBuilder([JetBrains.Annotations.CanBeNull] string navigationName)
	{
		return WithOneBuilder(MemberIdentity.Create(navigationName));
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder WithOneBuilder([JetBrains.Annotations.CanBeNull] MemberInfo navigationMemberInfo)
	{
		return WithOneBuilder(MemberIdentity.Create(navigationMemberInfo));
	}

	private InternalRelationshipBuilder WithOneBuilder(MemberIdentity reference)
	{
		ForeignKey metadata = Builder.Metadata;
		string name = reference.Name;
		if (name != null && metadata.DependentToPrincipal != null && metadata.GetDependentToPrincipalConfigurationSource() == ConfigurationSource.Explicit && metadata.DependentToPrincipal.Name != name)
		{
			throw new InvalidOperationException(CoreStrings.ConflictingRelationshipNavigation(metadata.PrincipalEntityType.DisplayName(), metadata.PrincipalToDependent.Name, metadata.DeclaringEntityType.DisplayName(), name, metadata.PrincipalEntityType.DisplayName(), metadata.PrincipalToDependent.Name, metadata.DeclaringEntityType.DisplayName(), metadata.DependentToPrincipal.Name));
		}
		if (!(reference.MemberInfo == null) && !(CollectionMember == null))
		{
			return Builder.HasNavigations(reference.MemberInfo, CollectionMember, (EntityType)DeclaringEntityType, (EntityType)RelatedEntityType, ConfigurationSource.Explicit);
		}
		return Builder.HasNavigations(reference.Name, CollectionName, (EntityType)DeclaringEntityType, (EntityType)RelatedEntityType, ConfigurationSource.Explicit);
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
public class CollectionNavigationBuilder<TEntity, TRelatedEntity> : CollectionNavigationBuilder where TEntity : class where TRelatedEntity : class
{
	[EntityFrameworkInternal]
	public CollectionNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] string navigationName, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(declaringEntityType, relatedEntityType, navigationName, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	public CollectionNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(declaringEntityType, relatedEntityType, navigationMemberInfo, foreignKey)
	{
	}

	public new virtual ReferenceCollectionBuilder<TEntity, TRelatedEntity> WithOne([JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		return new ReferenceCollectionBuilder<TEntity, TRelatedEntity>(DeclaringEntityType, RelatedEntityType, WithOneBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName")).Metadata);
	}

	public virtual ReferenceCollectionBuilder<TEntity, TRelatedEntity> WithOne([JetBrains.Annotations.CanBeNull] Expression<Func<TRelatedEntity, TEntity>> navigationExpression)
	{
		return new ReferenceCollectionBuilder<TEntity, TRelatedEntity>(DeclaringEntityType, RelatedEntityType, WithOneBuilder(navigationExpression?.GetPropertyAccess()).Metadata);
	}
}
