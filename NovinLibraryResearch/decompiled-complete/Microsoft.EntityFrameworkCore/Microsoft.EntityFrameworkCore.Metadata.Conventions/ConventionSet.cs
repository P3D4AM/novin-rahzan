using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ConventionSet
{
	public virtual IList<IModelInitializedConvention> ModelInitializedConventions { get; } = new List<IModelInitializedConvention>();


	public virtual IList<IModelFinalizedConvention> ModelFinalizedConventions { get; } = new List<IModelFinalizedConvention>();


	public virtual IList<IModelAnnotationChangedConvention> ModelAnnotationChangedConventions { get; } = new List<IModelAnnotationChangedConvention>();


	public virtual IList<IEntityTypeAddedConvention> EntityTypeAddedConventions { get; } = new List<IEntityTypeAddedConvention>();


	public virtual IList<IEntityTypeIgnoredConvention> EntityTypeIgnoredConventions { get; } = new List<IEntityTypeIgnoredConvention>();


	public virtual IList<IEntityTypeRemovedConvention> EntityTypeRemovedConventions { get; } = new List<IEntityTypeRemovedConvention>();


	public virtual IList<IEntityTypeMemberIgnoredConvention> EntityTypeMemberIgnoredConventions { get; } = new List<IEntityTypeMemberIgnoredConvention>();


	public virtual IList<IEntityTypeBaseTypeChangedConvention> EntityTypeBaseTypeChangedConventions { get; } = new List<IEntityTypeBaseTypeChangedConvention>();


	public virtual IList<IEntityTypePrimaryKeyChangedConvention> EntityTypePrimaryKeyChangedConventions { get; } = new List<IEntityTypePrimaryKeyChangedConvention>();


	public virtual IList<IEntityTypeAnnotationChangedConvention> EntityTypeAnnotationChangedConventions { get; } = new List<IEntityTypeAnnotationChangedConvention>();


	public virtual IList<IForeignKeyAddedConvention> ForeignKeyAddedConventions { get; } = new List<IForeignKeyAddedConvention>();


	public virtual IList<IForeignKeyRemovedConvention> ForeignKeyRemovedConventions { get; } = new List<IForeignKeyRemovedConvention>();


	public virtual IList<IForeignKeyPrincipalEndChangedConvention> ForeignKeyPrincipalEndChangedConventions { get; } = new List<IForeignKeyPrincipalEndChangedConvention>();


	public virtual IList<IForeignKeyPropertiesChangedConvention> ForeignKeyPropertiesChangedConventions { get; } = new List<IForeignKeyPropertiesChangedConvention>();


	public virtual IList<IForeignKeyUniquenessChangedConvention> ForeignKeyUniquenessChangedConventions { get; } = new List<IForeignKeyUniquenessChangedConvention>();


	public virtual IList<IForeignKeyRequirednessChangedConvention> ForeignKeyRequirednessChangedConventions { get; } = new List<IForeignKeyRequirednessChangedConvention>();


	public virtual IList<IForeignKeyOwnershipChangedConvention> ForeignKeyOwnershipChangedConventions { get; } = new List<IForeignKeyOwnershipChangedConvention>();


	public virtual IList<IForeignKeyAnnotationChangedConvention> ForeignKeyAnnotationChangedConventions { get; } = new List<IForeignKeyAnnotationChangedConvention>();


	public virtual IList<INavigationAddedConvention> NavigationAddedConventions { get; } = new List<INavigationAddedConvention>();


	public virtual IList<INavigationRemovedConvention> NavigationRemovedConventions { get; } = new List<INavigationRemovedConvention>();


	public virtual IList<IKeyAddedConvention> KeyAddedConventions { get; } = new List<IKeyAddedConvention>();


	public virtual IList<IKeyRemovedConvention> KeyRemovedConventions { get; } = new List<IKeyRemovedConvention>();


	public virtual IList<IKeyAnnotationChangedConvention> KeyAnnotationChangedConventions { get; } = new List<IKeyAnnotationChangedConvention>();


	public virtual IList<IIndexAddedConvention> IndexAddedConventions { get; } = new List<IIndexAddedConvention>();


	public virtual IList<IIndexRemovedConvention> IndexRemovedConventions { get; } = new List<IIndexRemovedConvention>();


	public virtual IList<IIndexUniquenessChangedConvention> IndexUniquenessChangedConventions { get; } = new List<IIndexUniquenessChangedConvention>();


	public virtual IList<IIndexAnnotationChangedConvention> IndexAnnotationChangedConventions { get; } = new List<IIndexAnnotationChangedConvention>();


	public virtual IList<IPropertyAddedConvention> PropertyAddedConventions { get; } = new List<IPropertyAddedConvention>();


	public virtual IList<IPropertyNullabilityChangedConvention> PropertyNullabilityChangedConventions { get; } = new List<IPropertyNullabilityChangedConvention>();


	public virtual IList<IPropertyFieldChangedConvention> PropertyFieldChangedConventions { get; } = new List<IPropertyFieldChangedConvention>();


	public virtual IList<IPropertyAnnotationChangedConvention> PropertyAnnotationChangedConventions { get; } = new List<IPropertyAnnotationChangedConvention>();


	public static bool Replace<TConvention, TImplementation>([JetBrains.Annotations.NotNull] IList<TConvention> conventionsList, [JetBrains.Annotations.NotNull] TImplementation newConvention) where TImplementation : TConvention
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conventionsList, "conventionsList");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(newConvention, "newConvention");
		for (int i = 0; i < conventionsList.Count; i++)
		{
			if (conventionsList[i] is TImplementation)
			{
				conventionsList.RemoveAt(i);
				conventionsList.Insert(i, (TConvention)(object)newConvention);
				return true;
			}
		}
		return false;
	}

	public static bool AddBefore<TConvention>([JetBrains.Annotations.NotNull] IList<TConvention> conventionsList, [JetBrains.Annotations.NotNull] TConvention newConvention, [JetBrains.Annotations.NotNull] Type existingConventionType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conventionsList, "conventionsList");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(newConvention, "newConvention");
		for (int i = 0; i < conventionsList.Count; i++)
		{
			if (existingConventionType.IsInstanceOfType(conventionsList[i]))
			{
				conventionsList.Insert(i, newConvention);
				return true;
			}
		}
		return false;
	}

	public static bool AddAfter<TConvention>([JetBrains.Annotations.NotNull] IList<TConvention> conventionsList, [JetBrains.Annotations.NotNull] TConvention newConvention, [JetBrains.Annotations.NotNull] Type existingConventionType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conventionsList, "conventionsList");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(newConvention, "newConvention");
		for (int i = 0; i < conventionsList.Count; i++)
		{
			if (existingConventionType.IsInstanceOfType(conventionsList[i]))
			{
				conventionsList.Insert(i + 1, newConvention);
				return true;
			}
		}
		return false;
	}

	public static bool Remove<TConvention>([JetBrains.Annotations.NotNull] IList<TConvention> conventionsList, [JetBrains.Annotations.NotNull] Type existingConventionType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conventionsList, "conventionsList");
		for (int i = 0; i < conventionsList.Count; i++)
		{
			if (existingConventionType.IsInstanceOfType(conventionsList[i]))
			{
				conventionsList.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public static ConventionSet CreateConventionSet([JetBrains.Annotations.NotNull] DbContext context)
	{
		return context.GetService<IConventionSetBuilder>().CreateConventionSet();
	}
}
