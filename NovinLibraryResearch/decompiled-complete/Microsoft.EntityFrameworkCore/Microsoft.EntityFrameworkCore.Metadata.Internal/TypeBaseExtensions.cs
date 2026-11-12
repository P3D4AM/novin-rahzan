using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class TypeBaseExtensions
{
	[DebuggerStepThrough]
	public static bool HasClrType([JetBrains.Annotations.NotNull] this ITypeBase type)
	{
		return type.ClrType != null;
	}

	public static IReadOnlyDictionary<string, PropertyInfo> GetRuntimeProperties(this ITypeBase type)
	{
		return (type as TypeBase).GetRuntimeProperties();
	}

	public static IReadOnlyDictionary<string, FieldInfo> GetRuntimeFields(this ITypeBase type)
	{
		return (type as TypeBase).GetRuntimeFields();
	}

	public static MemberInfo FindClrMember(this TypeBase type, string name)
	{
		if (type.GetRuntimeProperties().TryGetValue(name, out var value))
		{
			return value;
		}
		if (type.GetRuntimeFields().TryGetValue(name, out var value2))
		{
			return value2;
		}
		return null;
	}

	public static TypeBase AsTypeBase([JetBrains.Annotations.NotNull] this ITypeBase entityType, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<ITypeBase, TypeBase>(entityType, methodName);
	}
}
