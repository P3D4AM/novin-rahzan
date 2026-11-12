using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class RelationshipSnapshot
{
	public virtual InternalRelationshipBuilder Relationship
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual EntityType.Snapshot DefinedEntityTypeSnapshot
	{
		[DebuggerStepThrough]
		get;
	}

	public RelationshipSnapshot([JetBrains.Annotations.NotNull] InternalRelationshipBuilder relationship, [JetBrains.Annotations.CanBeNull] EntityType.Snapshot definedEntityTypeSnapshot)
	{
		Relationship = relationship;
		DefinedEntityTypeSnapshot = definedEntityTypeSnapshot;
	}

	public virtual InternalRelationshipBuilder Attach([JetBrains.Annotations.CanBeNull] InternalEntityTypeBuilder entityTypeBuilder = null)
	{
		if (entityTypeBuilder == null)
		{
			entityTypeBuilder = Relationship.Metadata.DeclaringEntityType.Builder;
		}
		InternalRelationshipBuilder internalRelationshipBuilder = Relationship.Attach(entityTypeBuilder);
		if (internalRelationshipBuilder != null)
		{
			DefinedEntityTypeSnapshot?.Attach(internalRelationshipBuilder.Metadata.ResolveOtherEntityType(entityTypeBuilder.Metadata).Builder);
		}
		return internalRelationshipBuilder;
	}
}
