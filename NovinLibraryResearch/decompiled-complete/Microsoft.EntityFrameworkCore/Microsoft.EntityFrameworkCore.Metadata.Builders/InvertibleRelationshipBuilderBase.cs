using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class InvertibleRelationshipBuilderBase : IInfrastructure<InternalRelationshipBuilder>
{
	private readonly IReadOnlyList<Property> _foreignKeyProperties;

	private readonly IReadOnlyList<Property> _principalKeyProperties;

	private readonly bool? _required;

	protected virtual IMutableEntityType DeclaringEntityType { get; }

	protected virtual IMutableEntityType RelatedEntityType { get; }

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder Builder
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	InternalRelationshipBuilder IInfrastructure<InternalRelationshipBuilder>.Instance => Builder;

	public virtual IMutableForeignKey Metadata => Builder.Metadata;

	[EntityFrameworkInternal]
	protected InvertibleRelationshipBuilderBase([JetBrains.Annotations.NotNull] IMutableEntityType declaringEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType relatedEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: this(((ForeignKey)foreignKey).Builder, null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(declaringEntityType, "declaringEntityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relatedEntityType, "relatedEntityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		DeclaringEntityType = declaringEntityType;
		RelatedEntityType = relatedEntityType;
	}

	[EntityFrameworkInternal]
	protected InvertibleRelationshipBuilderBase(InternalRelationshipBuilder builder, InvertibleRelationshipBuilderBase oldBuilder, bool inverted = false, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
	{
		Builder = builder;
		if (oldBuilder != null)
		{
			if (inverted && (oldBuilder._foreignKeyProperties != null || oldBuilder._principalKeyProperties != null))
			{
				throw new InvalidOperationException(CoreStrings.RelationshipCannotBeInverted);
			}
			DeclaringEntityType = oldBuilder.DeclaringEntityType;
			RelatedEntityType = oldBuilder.RelatedEntityType;
			_foreignKeyProperties = (foreignKeySet ? builder.Metadata.Properties : oldBuilder._foreignKeyProperties);
			_principalKeyProperties = (principalKeySet ? builder.Metadata.PrincipalKey.Properties : oldBuilder._principalKeyProperties);
			_required = (requiredSet ? new bool?(builder.Metadata.IsRequired) : oldBuilder._required);
			ForeignKey metadata = builder.Metadata;
			ForeignKey.AreCompatible(metadata.PrincipalEntityType, metadata.DeclaringEntityType, metadata.DependentToPrincipal?.GetIdentifyingMemberInfo(), metadata.PrincipalToDependent?.GetIdentifyingMemberInfo(), _foreignKeyProperties, _principalKeyProperties, metadata.IsUnique, shouldThrow: true);
		}
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
