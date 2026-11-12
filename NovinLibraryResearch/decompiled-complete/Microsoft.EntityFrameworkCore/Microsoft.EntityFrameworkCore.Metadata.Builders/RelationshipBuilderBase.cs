using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class RelationshipBuilderBase : IInfrastructure<InternalRelationshipBuilder>
{
	private readonly IReadOnlyList<Property> _foreignKeyProperties;

	private readonly IReadOnlyList<Property> _principalKeyProperties;

	private readonly bool? _required;

	protected virtual IMutableEntityType PrincipalEntityType { get; }

	protected virtual IMutableEntityType DependentEntityType { get; }

	[EntityFrameworkInternal]
	protected virtual InternalRelationshipBuilder Builder
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual IMutableForeignKey Metadata => Builder.Metadata;

	InternalRelationshipBuilder IInfrastructure<InternalRelationshipBuilder>.Instance => Builder;

	[EntityFrameworkInternal]
	protected RelationshipBuilderBase([JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType, [JetBrains.Annotations.NotNull] IMutableEntityType dependentEntityType, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
		: this(((ForeignKey)foreignKey).Builder, null)
	{
		PrincipalEntityType = principalEntityType;
		DependentEntityType = dependentEntityType;
	}

	[EntityFrameworkInternal]
	protected RelationshipBuilderBase(InternalRelationshipBuilder builder, RelationshipBuilderBase oldBuilder, bool foreignKeySet = false, bool principalKeySet = false, bool requiredSet = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Builder = builder;
		if (oldBuilder != null)
		{
			PrincipalEntityType = oldBuilder.PrincipalEntityType;
			DependentEntityType = oldBuilder.DependentEntityType;
			_foreignKeyProperties = (foreignKeySet ? builder.Metadata.Properties : ((EntityType)oldBuilder.DependentEntityType).Builder.GetActualProperties(oldBuilder._foreignKeyProperties, null));
			_principalKeyProperties = (principalKeySet ? builder.Metadata.PrincipalKey.Properties : ((EntityType)oldBuilder.PrincipalEntityType).Builder.GetActualProperties(oldBuilder._principalKeyProperties, null));
			_required = (requiredSet ? new bool?(builder.Metadata.IsRequired) : oldBuilder._required);
			ForeignKey metadata = builder.Metadata;
			ForeignKey.AreCompatible((EntityType)oldBuilder.PrincipalEntityType, (EntityType)oldBuilder.DependentEntityType, metadata.DependentToPrincipal?.GetIdentifyingMemberInfo(), metadata.PrincipalToDependent?.GetIdentifyingMemberInfo(), _foreignKeyProperties, _principalKeyProperties, metadata.IsUnique, shouldThrow: true);
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
