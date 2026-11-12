using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class RelationalForeignKeyBuilderExtensions
{
	public static ReferenceCollectionBuilder HasConstraintName([JetBrains.Annotations.NotNull] this ReferenceCollectionBuilder referenceCollectionBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(referenceCollectionBuilder, "referenceCollectionBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		referenceCollectionBuilder.Metadata.SetConstraintName(name);
		return referenceCollectionBuilder;
	}

	public static ReferenceCollectionBuilder<TEntity, TRelatedEntity> HasConstraintName<TEntity, TRelatedEntity>([JetBrains.Annotations.NotNull] this ReferenceCollectionBuilder<TEntity, TRelatedEntity> referenceCollectionBuilder, [JetBrains.Annotations.CanBeNull] string name) where TEntity : class where TRelatedEntity : class
	{
		return (ReferenceCollectionBuilder<TEntity, TRelatedEntity>)((ReferenceCollectionBuilder)referenceCollectionBuilder).HasConstraintName(name);
	}

	public static ReferenceReferenceBuilder HasConstraintName([JetBrains.Annotations.NotNull] this ReferenceReferenceBuilder referenceReferenceBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(referenceReferenceBuilder, "referenceReferenceBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		referenceReferenceBuilder.Metadata.SetConstraintName(name);
		return referenceReferenceBuilder;
	}

	public static ReferenceReferenceBuilder<TEntity, TRelatedEntity> HasConstraintName<TEntity, TRelatedEntity>([JetBrains.Annotations.NotNull] this ReferenceReferenceBuilder<TEntity, TRelatedEntity> referenceReferenceBuilder, [JetBrains.Annotations.CanBeNull] string name) where TEntity : class where TRelatedEntity : class
	{
		return (ReferenceReferenceBuilder<TEntity, TRelatedEntity>)((ReferenceReferenceBuilder)referenceReferenceBuilder).HasConstraintName(name);
	}

	public static OwnershipBuilder HasConstraintName([JetBrains.Annotations.NotNull] this OwnershipBuilder ownershipBuilder, [JetBrains.Annotations.CanBeNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(name, "name");
		ownershipBuilder.Metadata.SetConstraintName(name);
		return ownershipBuilder;
	}

	public static OwnershipBuilder<TEntity, TDependentEntity> HasConstraintName<TEntity, TDependentEntity>([JetBrains.Annotations.NotNull] this OwnershipBuilder<TEntity, TDependentEntity> ownershipBuilder, [JetBrains.Annotations.CanBeNull] string name) where TEntity : class where TDependentEntity : class
	{
		return (OwnershipBuilder<TEntity, TDependentEntity>)((OwnershipBuilder)ownershipBuilder).HasConstraintName(name);
	}

	public static IConventionRelationshipBuilder HasConstraintName([JetBrains.Annotations.NotNull] this IConventionRelationshipBuilder relationship, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		if (!relationship.CanSetConstraintName(name, fromDataAnnotation))
		{
			return null;
		}
		relationship.Metadata.SetConstraintName(name, fromDataAnnotation);
		return relationship;
	}

	public static bool CanSetConstraintName([JetBrains.Annotations.NotNull] this IConventionRelationshipBuilder relationship, [JetBrains.Annotations.CanBeNull] string name, bool fromDataAnnotation = false)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationship, "relationship").CanSetAnnotation("Relational:Name", name, fromDataAnnotation);
	}
}
