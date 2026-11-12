using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

public class ReferenceNavigationBuilder : IInfrastructure<InternalRelationshipBuilder>
{
	private InternalRelationshipBuilder Builder
	{
		[DebuggerStepThrough]
		get;
	}

	[EntityFrameworkInternal]
	protected virtual string ReferenceName
	{
		[DebuggerStepThrough]
		get;
	}

	[EntityFrameworkInternal]
	protected virtual MemberInfo ReferenceMember
	{
		[DebuggerStepThrough]
		get;
	}

	[EntityFrameworkInternal]
	protected virtual IMutableEntityType RelatedEntityType
	{
		[DebuggerStepThrough]
		get;
	}

	[EntityFrameworkInternal]
	protected virtual IMutableEntityType DeclaringEntityType
	{
		[DebuggerStepThrough]
		get;
	}

	InternalRelationshipBuilder IInfrastructure<InternalRelationshipBuilder>.Instance => Builder;

	[EntityFrameworkInternal]
	public ReferenceNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] string navigationName, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relatedEntityType, "relatedEntityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		DeclaringEntityType = declaringEntityType;
		RelatedEntityType = relatedEntityType;
		ReferenceName = navigationName;
		Builder = ((ForeignKey)foreignKey).Builder;
	}

	[EntityFrameworkInternal]
	public ReferenceNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relatedEntityType, "relatedEntityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		DeclaringEntityType = declaringEntityType;
		RelatedEntityType = relatedEntityType;
		ReferenceMember = navigationMemberInfo;
		ReferenceName = navigationMemberInfo?.GetSimpleMemberName();
		Builder = ((ForeignKey)foreignKey).Builder;
	}

	public virtual ReferenceCollectionBuilder WithMany([JetBrains.Annotations.CanBeNull] string collection = null)
	{
		return new ReferenceCollectionBuilder(RelatedEntityType, DeclaringEntityType, WithManyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(collection, "collection")).Metadata);
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder WithManyBuilder([JetBrains.Annotations.CanBeNull] string navigationName)
	{
		return WithManyBuilder(MemberIdentity.Create(navigationName));
	}

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder WithManyBuilder([JetBrains.Annotations.CanBeNull] MemberInfo navigationMemberInfo)
	{
		return WithManyBuilder(MemberIdentity.Create(navigationMemberInfo));
	}

	private InternalRelationshipBuilder WithManyBuilder(MemberIdentity collection)
	{
		InternalRelationshipBuilder internalRelationshipBuilder = Builder.HasEntityTypes((EntityType)RelatedEntityType, (EntityType)DeclaringEntityType, ConfigurationSource.Explicit);
		string name = collection.Name;
		if (internalRelationshipBuilder.Metadata.IsUnique && internalRelationshipBuilder.Metadata.PrincipalToDependent != null && internalRelationshipBuilder.Metadata.GetPrincipalToDependentConfigurationSource() == ConfigurationSource.Explicit && name != null)
		{
			ThrowForConflictingNavigation(internalRelationshipBuilder.Metadata, name, newToPrincipal: false);
		}
		internalRelationshipBuilder = internalRelationshipBuilder.IsUnique(false, ConfigurationSource.Explicit);
		ForeignKey metadata = internalRelationshipBuilder.Metadata;
		if (name != null && metadata.PrincipalToDependent != null && metadata.GetPrincipalToDependentConfigurationSource() == ConfigurationSource.Explicit && metadata.PrincipalToDependent.Name != name)
		{
			ThrowForConflictingNavigation(metadata, name, newToPrincipal: false);
		}
		if (!(collection.MemberInfo == null) && !(ReferenceMember == null))
		{
			return internalRelationshipBuilder.HasNavigations(ReferenceMember, collection.MemberInfo, (EntityType)RelatedEntityType, (EntityType)DeclaringEntityType, ConfigurationSource.Explicit);
		}
		return internalRelationshipBuilder.HasNavigations(ReferenceName, collection.Name, (EntityType)RelatedEntityType, (EntityType)DeclaringEntityType, ConfigurationSource.Explicit);
	}

	public virtual ReferenceReferenceBuilder WithOne([JetBrains.Annotations.CanBeNull] string reference = null)
	{
		return new ReferenceReferenceBuilder(DeclaringEntityType, RelatedEntityType, WithOneBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(reference, "reference")).Metadata);
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
		string name = reference.Name;
		if (!Builder.Metadata.IsUnique && Builder.Metadata.PrincipalToDependent != null && Builder.Metadata.GetPrincipalToDependentConfigurationSource() == ConfigurationSource.Explicit && name != null)
		{
			ThrowForConflictingNavigation(Builder.Metadata, name, newToPrincipal: false);
		}
		using IConventionBatch batch = Builder.Metadata.DeclaringEntityType.Model.ConventionDispatcher.DelayConventions();
		InternalRelationshipBuilder internalRelationshipBuilder = Builder.IsUnique(true, ConfigurationSource.Explicit);
		ForeignKey metadata = internalRelationshipBuilder.Metadata;
		if (metadata.IsSelfReferencing() && name != null && ReferenceName == name)
		{
			throw new InvalidOperationException(CoreStrings.ConflictingPropertyOrNavigation(name, RelatedEntityType.DisplayName(), RelatedEntityType.DisplayName()));
		}
		bool flag = !metadata.IsSelfReferencing() && (!metadata.DeclaringEntityType.IsAssignableFrom(DeclaringEntityType) || !metadata.PrincipalEntityType.IsAssignableFrom(RelatedEntityType) || (metadata.DeclaringEntityType.IsAssignableFrom(RelatedEntityType) && metadata.PrincipalEntityType.IsAssignableFrom(DeclaringEntityType) && metadata.PrincipalToDependent != null && metadata.PrincipalToDependent.Name == ReferenceName));
		if (name != null && ((flag && metadata.DependentToPrincipal != null && metadata.GetDependentToPrincipalConfigurationSource() == ConfigurationSource.Explicit && metadata.DependentToPrincipal.Name != name) || (!flag && metadata.PrincipalToDependent != null && metadata.GetPrincipalToDependentConfigurationSource() == ConfigurationSource.Explicit && metadata.PrincipalToDependent.Name != name)))
		{
			ThrowForConflictingNavigation(metadata, name, flag);
		}
		MemberInfo memberInfo = reference.MemberInfo;
		internalRelationshipBuilder = ((!flag) ? ((memberInfo == null || ReferenceMember == null) ? internalRelationshipBuilder.HasNavigations(ReferenceName, name, (EntityType)RelatedEntityType, (EntityType)DeclaringEntityType, ConfigurationSource.Explicit) : internalRelationshipBuilder.HasNavigations(ReferenceMember, memberInfo, (EntityType)RelatedEntityType, (EntityType)DeclaringEntityType, ConfigurationSource.Explicit)) : ((memberInfo == null || ReferenceMember == null) ? internalRelationshipBuilder.HasNavigations(name, ReferenceName, (EntityType)DeclaringEntityType, (EntityType)RelatedEntityType, ConfigurationSource.Explicit) : internalRelationshipBuilder.HasNavigations(memberInfo, ReferenceMember, (EntityType)DeclaringEntityType, (EntityType)RelatedEntityType, ConfigurationSource.Explicit)));
		return batch.Run(internalRelationshipBuilder);
	}

	private static void ThrowForConflictingNavigation(ForeignKey foreignKey, string newInverseName, bool newToPrincipal)
	{
		throw new InvalidOperationException(CoreStrings.ConflictingRelationshipNavigation(foreignKey.PrincipalEntityType.DisplayName(), (!newToPrincipal) ? newInverseName : foreignKey.PrincipalToDependent?.Name, foreignKey.DeclaringEntityType.DisplayName(), newToPrincipal ? newInverseName : foreignKey.DependentToPrincipal?.Name, foreignKey.PrincipalEntityType.DisplayName(), foreignKey.PrincipalToDependent?.Name, foreignKey.DeclaringEntityType.DisplayName(), foreignKey.DependentToPrincipal?.Name));
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
public class ReferenceNavigationBuilder<TEntity, TRelatedEntity> : ReferenceNavigationBuilder where TEntity : class where TRelatedEntity : class
{
	[EntityFrameworkInternal]
	public ReferenceNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] string navigationName, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(declaringEntityType, relatedEntityType, navigationName, foreignKey)
	{
	}

	[EntityFrameworkInternal]
	public ReferenceNavigationBuilder([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationMemberInfo, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: base(declaringEntityType, relatedEntityType, navigationMemberInfo, foreignKey)
	{
	}

	public new virtual ReferenceCollectionBuilder<TRelatedEntity, TEntity> WithMany([JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		return new ReferenceCollectionBuilder<TRelatedEntity, TEntity>(RelatedEntityType, DeclaringEntityType, WithManyBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName")).Metadata);
	}

	public virtual ReferenceCollectionBuilder<TRelatedEntity, TEntity> WithMany([JetBrains.Annotations.CanBeNull] Expression<Func<TRelatedEntity, IEnumerable<TEntity>>> navigationExpression)
	{
		return new ReferenceCollectionBuilder<TRelatedEntity, TEntity>(RelatedEntityType, DeclaringEntityType, WithManyBuilder(navigationExpression?.GetPropertyAccess()).Metadata);
	}

	public new virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> WithOne([JetBrains.Annotations.CanBeNull] string navigationName = null)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(DeclaringEntityType, RelatedEntityType, WithOneBuilder(Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(navigationName, "navigationName")).Metadata);
	}

	public virtual ReferenceReferenceBuilder<TEntity, TRelatedEntity> WithOne([JetBrains.Annotations.CanBeNull] Expression<Func<TRelatedEntity, TEntity>> navigationExpression)
	{
		return new ReferenceReferenceBuilder<TEntity, TRelatedEntity>(DeclaringEntityType, RelatedEntityType, WithOneBuilder(navigationExpression?.GetPropertyAccess()).Metadata);
	}
}
