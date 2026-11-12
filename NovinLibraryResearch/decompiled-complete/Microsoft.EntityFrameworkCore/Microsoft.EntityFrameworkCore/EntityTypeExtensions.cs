using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class EntityTypeExtensions
{
	public static IEnumerable<IEntityType> GetConcreteDerivedTypesInclusive([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return from et in entityType.GetDerivedTypesInclusive()
			where !et.IsAbstract()
			select et;
	}

	[DebuggerStepThrough]
	public static bool IsAbstract([JetBrains.Annotations.NotNull] this ITypeBase type)
	{
		return type.ClrType?.GetTypeInfo().IsAbstract ?? false;
	}

	public static IEntityType GetRootType([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.BaseType?.GetRootType() ?? entityType;
	}

	[Obsolete("Use GetRootType")]
	public static IEntityType RootType([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetRootType();
	}

	public static IEnumerable<IEntityType> GetDerivedTypes([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("GetDerivedTypes").GetDerivedTypes();
	}

	public static IEnumerable<IEntityType> GetDerivedTypesInclusive([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return new IEntityType[1] { entityType }.Concat(entityType.GetDerivedTypes());
	}

	public static IEnumerable<IEntityType> GetDirectlyDerivedTypes([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("GetDirectlyDerivedTypes").GetDirectlyDerivedTypes();
	}

	public static bool IsAssignableFrom([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IEntityType derivedType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(derivedType, "derivedType");
		for (IEntityType entityType2 = derivedType; entityType2 != null; entityType2 = entityType2.BaseType)
		{
			if (entityType2 == entityType)
			{
				return true;
			}
		}
		return false;
	}

	public static IEntityType GetClosestCommonParent([JetBrains.Annotations.NotNull] this IEntityType entityType1, [JetBrains.Annotations.NotNull] IEntityType entityType2)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType1, "entityType1");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType2, "entityType2");
		return entityType1.GetAllBaseTypesInclusiveAscending().FirstOrDefault((IEntityType i) => entityType2.GetAllBaseTypesInclusiveAscending().Any((IEntityType j) => j == i));
	}

	public static bool IsStrictlyDerivedFrom([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IEntityType baseType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(baseType, "baseType");
		if (entityType != baseType)
		{
			return baseType.IsAssignableFrom(entityType);
		}
		return false;
	}

	public static IEntityType LeastDerivedType([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IEntityType otherEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(otherEntityType, "otherEntityType");
		if (!entityType.IsAssignableFrom(otherEntityType))
		{
			if (!otherEntityType.IsAssignableFrom(entityType))
			{
				return null;
			}
			return otherEntityType;
		}
		return entityType;
	}

	public static IEnumerable<IEntityType> GetAllBaseTypesInclusive([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetAllBaseTypesInclusiveAscending().Reverse();
	}

	public static IEnumerable<IEntityType> GetAllBaseTypesInclusiveAscending([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		while (entityType != null)
		{
			yield return entityType;
			entityType = entityType.BaseType;
		}
	}

	public static IEnumerable<IKey> GetDeclaredKeys([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDeclaredKeys").GetDeclaredKeys();
	}

	public static IEnumerable<IForeignKey> GetDeclaredForeignKeys([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDeclaredForeignKeys").GetDeclaredForeignKeys();
	}

	public static IEnumerable<IForeignKey> GetDerivedForeignKeys([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDerivedForeignKeys").GetDerivedForeignKeys();
	}

	public static IEnumerable<INavigation> GetDeclaredNavigations([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return from m in entityType.GetDeclaredForeignKeys().Concat(entityType.GetDeclaredReferencingForeignKeys()).SelectMany((IForeignKey foreignKey) => foreignKey.FindNavigationsFrom(entityType))
				.Distinct()
			orderby m.Name
			select m;
	}

	public static IEnumerable<IProperty> GetDeclaredProperties([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDeclaredProperties").GetDeclaredProperties();
	}

	public static IEnumerable<IServiceProperty> GetDeclaredServiceProperties([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDeclaredServiceProperties").GetDeclaredServiceProperties();
	}

	public static IEnumerable<IIndex> GetDeclaredIndexes([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.AsEntityType("GetDeclaredIndexes").GetDeclaredIndexes();
	}

	private static string DisplayNameDefault(this ITypeBase type)
	{
		if (!(type.ClrType != null))
		{
			return type.Name;
		}
		return type.ClrType.ShortDisplayName();
	}

	[DebuggerStepThrough]
	public static string DisplayName([JetBrains.Annotations.NotNull] this ITypeBase type)
	{
		if (!(type is IEntityType entityType) || !entityType.HasDefiningNavigation())
		{
			return type.DisplayNameDefault();
		}
		StringBuilder stringBuilder = new StringBuilder();
		Stack<string> stack = new Stack<string>();
		IEntityType entityType2 = entityType;
		while (true)
		{
			string definingNavigationName = entityType2.DefiningNavigationName;
			if (definingNavigationName == null)
			{
				break;
			}
			entityType2 = entityType2.DefiningEntityType;
			stack.Push("#");
			stack.Push(definingNavigationName);
			stack.Push(".");
			stack.Push(entityType2.DisplayNameDefault());
		}
		if (entityType2 != entityType)
		{
			stringBuilder.AppendJoin(stack, "");
		}
		stringBuilder.Append(type.DisplayNameDefault());
		return stringBuilder.ToString();
	}

	[DebuggerStepThrough]
	public static string ShortName([JetBrains.Annotations.NotNull] this ITypeBase type)
	{
		if (type.ClrType != null)
		{
			return type.ClrType.ShortDisplayName();
		}
		int num = type.Name.LastIndexOf("+", StringComparison.Ordinal);
		int num2 = type.Name.LastIndexOf(".", StringComparison.Ordinal);
		if (num != -1)
		{
			return type.Name.Substring(num + 1, type.Name.Length - num - 1);
		}
		if (num2 != -1)
		{
			return type.Name.Substring(num2 + 1, type.Name.Length - num2 - 1);
		}
		return type.Name;
	}

	[DebuggerStepThrough]
	public static bool HasDefiningNavigation([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.DefiningEntityType != null;
	}

	[DebuggerStepThrough]
	public static bool IsOwned([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType.GetForeignKeys().Any((IForeignKey fk) => fk.IsOwnership);
	}

	public static IKey FindKey([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindKey(new IProperty[1] { property });
	}

	public static IEnumerable<IForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		return entityType.FindForeignKeys(new IProperty[1] { property });
	}

	public static IEnumerable<IForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		foreach (IForeignKey foreignKey in entityType.GetForeignKeys())
		{
			if (PropertyListComparer.Instance.Equals(foreignKey.Properties, properties))
			{
				yield return foreignKey;
			}
		}
	}

	public static IForeignKey FindForeignKey([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").FindForeignKey(new IProperty[1] { property }, principalKey, principalEntityType);
	}

	public static IEnumerable<IForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("GetReferencingForeignKeys").GetReferencingForeignKeys();
	}

	public static IEnumerable<IForeignKey> GetDeclaredReferencingForeignKeys([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("GetDeclaredReferencingForeignKeys").GetDeclaredReferencingForeignKeys();
	}

	public static IForeignKey FindOwnership([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return ((EntityType)entityType).FindOwnership();
	}

	public static INavigation FindNavigation([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo");
		return entityType.FindNavigation(memberInfo.GetSimpleMemberName());
	}

	public static INavigation FindNavigation([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("FindNavigation").FindNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"));
	}

	public static INavigation FindDeclaredNavigation([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("FindDeclaredNavigation").FindDeclaredNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"));
	}

	public static INavigation FindDefiningNavigation([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		if (!entityType.HasDefiningNavigation())
		{
			return null;
		}
		INavigation navigation = entityType.DefiningEntityType.FindNavigation(entityType.DefiningNavigationName);
		if (navigation?.GetTargetType() != entityType)
		{
			return null;
		}
		return navigation;
	}

	public static IEnumerable<INavigation> GetNavigations([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType").AsEntityType("GetNavigations").GetNavigations();
	}

	public static IProperty FindProperty([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo");
		return entityType.FindProperty(memberInfo.GetSimpleMemberName());
	}

	public static IReadOnlyList<IProperty> FindProperties([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames)
	{
		return entityType.AsEntityType("FindProperties").FindProperties(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyNames, "propertyNames"));
	}

	public static IProperty FindDeclaredProperty([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return entityType.AsEntityType("FindDeclaredProperty").FindDeclaredProperty(name);
	}

	public static IIndex FindIndex([JetBrains.Annotations.NotNull] this IEntityType entityType, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return entityType.FindIndex(new IProperty[1] { property });
	}

	public static ChangeTrackingStrategy GetChangeTrackingStrategy([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return ((ChangeTrackingStrategy?)entityType["ChangeTrackingStrategy"]) ?? entityType.Model.GetChangeTrackingStrategy();
	}

	public static IEnumerable<IDictionary<string, object>> GetSeedData([JetBrains.Annotations.NotNull] this IEntityType entityType, bool providerValues = false)
	{
		return entityType.AsEntityType("GetSeedData").GetSeedData(providerValues);
	}

	public static LambdaExpression GetQueryFilter([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return (LambdaExpression)entityType["QueryFilter"];
	}

	public static LambdaExpression GetDefiningQuery([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return (LambdaExpression)entityType["DefiningQuery"];
	}

	public static IProperty GetDiscriminatorProperty([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		if (entityType.BaseType != null)
		{
			return entityType.GetRootType().GetDiscriminatorProperty();
		}
		string text = (string)entityType["DiscriminatorProperty"];
		if (text != null)
		{
			return entityType.FindProperty(text);
		}
		return null;
	}

	public static object GetDiscriminatorValue([JetBrains.Annotations.NotNull] this IEntityType entityType)
	{
		return entityType["DiscriminatorValue"];
	}
}
