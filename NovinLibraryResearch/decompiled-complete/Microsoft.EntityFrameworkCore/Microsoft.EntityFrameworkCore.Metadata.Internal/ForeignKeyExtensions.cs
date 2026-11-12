using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ForeignKeyExtensions
{
	public static bool IsSelfReferencing([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return foreignKey.DeclaringEntityType == foreignKey.PrincipalEntityType;
	}

	public static bool IsIntraHierarchical([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return foreignKey.DeclaringEntityType.IsSameHierarchy(foreignKey.PrincipalEntityType);
	}

	public static bool IsSelfPrimaryKeyReferencing([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return foreignKey.DeclaringEntityType.FindPrimaryKey() == foreignKey.PrincipalKey;
	}

	public static IEnumerable<INavigation> GetNavigations([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		if (foreignKey.PrincipalToDependent != null)
		{
			yield return foreignKey.PrincipalToDependent;
		}
		if (foreignKey.DependentToPrincipal != null)
		{
			yield return foreignKey.DependentToPrincipal;
		}
	}

	public static IEnumerable<INavigation> FindNavigationsFrom([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (foreignKey.DeclaringEntityType != entityType && foreignKey.PrincipalEntityType != entityType)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationshipStrict(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (!foreignKey.IsSelfReferencing())
		{
			return foreignKey.FindNavigations(foreignKey.DeclaringEntityType == entityType);
		}
		return foreignKey.GetNavigations();
	}

	public static IEnumerable<INavigation> FindNavigationsFromInHierarchy([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (!foreignKey.DeclaringEntityType.IsAssignableFrom(entityType) && !foreignKey.PrincipalEntityType.IsAssignableFrom(entityType))
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationship(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (!foreignKey.IsIntraHierarchical())
		{
			return foreignKey.FindNavigations(foreignKey.DeclaringEntityType.IsAssignableFrom(entityType));
		}
		return foreignKey.GetNavigations();
	}

	public static IEnumerable<INavigation> FindNavigationsTo([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (foreignKey.DeclaringEntityType != entityType && foreignKey.PrincipalEntityType != entityType)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationshipStrict(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (!foreignKey.IsSelfReferencing())
		{
			return foreignKey.FindNavigations(foreignKey.PrincipalEntityType == entityType);
		}
		return foreignKey.GetNavigations();
	}

	public static IEnumerable<INavigation> FindNavigationsToInHierarchy([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (!foreignKey.DeclaringEntityType.IsAssignableFrom(entityType) && !foreignKey.PrincipalEntityType.IsAssignableFrom(entityType))
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationship(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (!foreignKey.IsIntraHierarchical())
		{
			return foreignKey.FindNavigations(foreignKey.PrincipalEntityType.IsAssignableFrom(entityType));
		}
		return foreignKey.GetNavigations();
	}

	private static IEnumerable<INavigation> FindNavigations(this IForeignKey foreignKey, bool toPrincipal)
	{
		if (toPrincipal)
		{
			if (foreignKey.DependentToPrincipal != null)
			{
				yield return foreignKey.DependentToPrincipal;
			}
		}
		else if (foreignKey.PrincipalToDependent != null)
		{
			yield return foreignKey.PrincipalToDependent;
		}
	}

	public static IEntityType ResolveOtherEntityTypeInHierarchy([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (!foreignKey.DeclaringEntityType.IsAssignableFrom(entityType) && !foreignKey.PrincipalEntityType.IsAssignableFrom(entityType))
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationship(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (foreignKey.IsIntraHierarchical())
		{
			throw new InvalidOperationException(CoreStrings.IntraHierarchicalAmbiguousTargetEntityType(entityType.DisplayName(), foreignKey.Properties.Format(), foreignKey.PrincipalEntityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName()));
		}
		if (!foreignKey.DeclaringEntityType.IsAssignableFrom(entityType))
		{
			return foreignKey.DeclaringEntityType;
		}
		return foreignKey.PrincipalEntityType;
	}

	public static IEntityType ResolveEntityTypeInHierarchy([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (!foreignKey.DeclaringEntityType.IsAssignableFrom(entityType) && !foreignKey.PrincipalEntityType.IsAssignableFrom(entityType))
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationship(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (foreignKey.IsIntraHierarchical())
		{
			throw new InvalidOperationException(CoreStrings.IntraHierarchicalAmbiguousTargetEntityType(entityType.DisplayName(), foreignKey.Properties.Format(), foreignKey.PrincipalEntityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName()));
		}
		if (!foreignKey.DeclaringEntityType.IsAssignableFrom(entityType))
		{
			return foreignKey.PrincipalEntityType;
		}
		return foreignKey.DeclaringEntityType;
	}

	public static IDependentKeyValueFactory<TKey> GetDependentKeyValueFactory<TKey>([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return (IDependentKeyValueFactory<TKey>)foreignKey.AsForeignKey("GetDependentKeyValueFactory").DependentKeyValueFactory;
	}

	public static IDependentsMap CreateDependentsMapFactory([JetBrains.Annotations.NotNull] this IForeignKey foreignKey)
	{
		return foreignKey.AsForeignKey("CreateDependentsMapFactory").DependentsMapFactory();
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, bool singleLine = true, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent);
		if (singleLine)
		{
			stringBuilder.Append("ForeignKey: ");
		}
		stringBuilder.Append(foreignKey.DeclaringEntityType.DisplayName()).Append(" ").Append(foreignKey.Properties.Format())
			.Append(" -> ")
			.Append(foreignKey.PrincipalEntityType.DisplayName())
			.Append(" ")
			.Append(foreignKey.PrincipalKey.Properties.Format());
		if (foreignKey.IsUnique)
		{
			stringBuilder.Append(" Unique");
		}
		if (foreignKey.IsOwnership)
		{
			stringBuilder.Append(" Ownership");
		}
		if (foreignKey.PrincipalToDependent != null)
		{
			stringBuilder.Append(" ToDependent: ").Append(foreignKey.PrincipalToDependent.Name);
		}
		if (foreignKey.DependentToPrincipal != null)
		{
			stringBuilder.Append(" ToPrincipal: ").Append(foreignKey.DependentToPrincipal.Name);
		}
		if (!singleLine)
		{
			stringBuilder.Append(foreignKey.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}

	public static ForeignKey AsForeignKey([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IForeignKey, ForeignKey>(foreignKey, methodName);
	}
}
