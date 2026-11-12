using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class PropertyBaseExtensions
{
	public static MemberInfo GetMemberInfo([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase, bool forMaterialization, bool forSet)
	{
		if (propertyBase.TryGetMemberInfo(forMaterialization, forSet, out var memberInfo, out var errorMessage))
		{
			return memberInfo;
		}
		throw new InvalidOperationException(errorMessage);
	}

	public static IClrPropertyGetter GetGetter([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.AsPropertyBase("GetGetter").Getter;
	}

	public static string GetFieldName([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.FieldInfo?.GetSimpleMemberName();
	}

	public static bool IsShadowProperty([JetBrains.Annotations.NotNull] this IPropertyBase property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").GetIdentifyingMemberInfo() == null;
	}

	public static PropertyAccessMode GetPropertyAccessMode([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return (PropertyAccessMode)(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyBase, "propertyBase")["PropertyAccessMode"] ?? ((object)((propertyBase is INavigation) ? propertyBase.DeclaringType.GetNavigationAccessMode() : propertyBase.DeclaringType.GetPropertyAccessMode())));
	}
}
