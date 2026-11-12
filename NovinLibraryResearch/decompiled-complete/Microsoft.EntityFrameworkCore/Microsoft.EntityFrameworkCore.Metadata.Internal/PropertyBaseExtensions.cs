using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class PropertyBaseExtensions
{
	public static int GetShadowIndex([JetBrains.Annotations.NotNull] this IPropertyBase property)
	{
		return property.GetPropertyIndexes().ShadowIndex;
	}

	public static int GetStoreGeneratedIndex([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.GetPropertyIndexes().StoreGenerationIndex;
	}

	public static int GetRelationshipIndex([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.GetPropertyIndexes().RelationshipIndex;
	}

	public static int GetIndex([JetBrains.Annotations.NotNull] this IPropertyBase property)
	{
		return property.GetPropertyIndexes().Index;
	}

	public static int GetOriginalValueIndex([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.GetPropertyIndexes().OriginalValueIndex;
	}

	public static PropertyIndexes GetPropertyIndexes([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.AsPropertyBase("GetPropertyIndexes")?.PropertyIndexes;
	}

	public static PropertyAccessors GetPropertyAccessors([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return propertyBase.AsPropertyBase("GetPropertyAccessors").Accessors;
	}

	public static MemberInfo GetIdentifyingMemberInfo([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase)
	{
		return (MemberInfo)(((object)propertyBase.PropertyInfo) ?? ((object)propertyBase.FieldInfo));
	}

	public static bool TryGetMemberInfo([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase, bool forConstruction, bool forSet, out MemberInfo memberInfo, out string errorMessage)
	{
		memberInfo = null;
		errorMessage = null;
		PropertyInfo propertyInfo = propertyBase.PropertyInfo;
		FieldInfo fieldInfo = propertyBase.FieldInfo;
		PropertyInfo propertyInfo2 = propertyInfo?.FindSetterProperty();
		PropertyInfo propertyInfo3 = propertyInfo?.FindGetterProperty();
		bool flag = (propertyBase as INavigation)?.IsCollection() ?? false;
		bool flag2 = fieldInfo != null;
		bool flag3 = propertyInfo2 != null;
		bool flag4 = propertyInfo3 != null;
		PropertyAccessMode propertyAccessMode = propertyBase.GetPropertyAccessMode();
		if (forConstruction)
		{
			switch (propertyAccessMode)
			{
			case PropertyAccessMode.Field:
			case PropertyAccessMode.FieldDuringConstruction:
				if (flag2)
				{
					memberInfo = fieldInfo;
					return true;
				}
				if (flag)
				{
					return true;
				}
				errorMessage = GetNoFieldErrorMessage(propertyBase);
				return false;
			case PropertyAccessMode.Property:
				if (flag3)
				{
					memberInfo = propertyInfo2;
					return true;
				}
				if (flag)
				{
					return true;
				}
				errorMessage = (flag4 ? CoreStrings.NoSetter(propertyBase.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode") : CoreStrings.NoProperty(fieldInfo?.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode"));
				return false;
			case PropertyAccessMode.PreferField:
			case PropertyAccessMode.PreferFieldDuringConstruction:
				if (flag2)
				{
					memberInfo = fieldInfo;
					return true;
				}
				if (flag3)
				{
					memberInfo = propertyInfo2;
					return true;
				}
				break;
			}
			if (propertyAccessMode == PropertyAccessMode.PreferProperty)
			{
				if (flag3)
				{
					memberInfo = propertyInfo2;
					return true;
				}
				if (flag2)
				{
					memberInfo = fieldInfo;
					return true;
				}
			}
			if (flag)
			{
				return true;
			}
			errorMessage = CoreStrings.NoFieldOrSetter(propertyBase.Name, propertyBase.DeclaringType.DisplayName());
			return false;
		}
		if (forSet)
		{
			switch (propertyAccessMode)
			{
			case PropertyAccessMode.Field:
				if (flag2)
				{
					memberInfo = fieldInfo;
					return true;
				}
				if (flag)
				{
					return true;
				}
				errorMessage = GetNoFieldErrorMessage(propertyBase);
				return false;
			case PropertyAccessMode.Property:
				if (flag3)
				{
					memberInfo = propertyInfo2;
					return true;
				}
				if (flag)
				{
					return true;
				}
				errorMessage = (flag4 ? CoreStrings.NoSetter(propertyBase.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode") : CoreStrings.NoProperty(fieldInfo?.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode"));
				return false;
			case PropertyAccessMode.PreferField:
				if (flag2)
				{
					memberInfo = fieldInfo;
					return true;
				}
				if (flag3)
				{
					memberInfo = propertyInfo2;
					return true;
				}
				break;
			}
			if (propertyAccessMode == PropertyAccessMode.PreferProperty || propertyAccessMode == PropertyAccessMode.FieldDuringConstruction || propertyAccessMode == PropertyAccessMode.PreferFieldDuringConstruction)
			{
				if (flag3)
				{
					memberInfo = propertyInfo2;
					return true;
				}
				if (flag2)
				{
					memberInfo = fieldInfo;
					return true;
				}
			}
			if (flag)
			{
				return true;
			}
			errorMessage = CoreStrings.NoFieldOrSetter(propertyBase.Name, propertyBase.DeclaringType.DisplayName());
			return false;
		}
		switch (propertyAccessMode)
		{
		case PropertyAccessMode.Field:
			if (flag2)
			{
				memberInfo = fieldInfo;
				return true;
			}
			errorMessage = GetNoFieldErrorMessage(propertyBase);
			return false;
		case PropertyAccessMode.Property:
			if (flag4)
			{
				memberInfo = propertyInfo3;
				return true;
			}
			errorMessage = (flag3 ? CoreStrings.NoGetter(propertyBase.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode") : CoreStrings.NoProperty(fieldInfo?.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode"));
			return false;
		case PropertyAccessMode.PreferField:
			if (flag2)
			{
				memberInfo = fieldInfo;
				return true;
			}
			if (flag4)
			{
				memberInfo = propertyInfo3;
				return true;
			}
			break;
		}
		if (propertyAccessMode == PropertyAccessMode.PreferProperty || propertyAccessMode == PropertyAccessMode.FieldDuringConstruction || propertyAccessMode == PropertyAccessMode.PreferFieldDuringConstruction)
		{
			if (flag4)
			{
				memberInfo = propertyInfo3;
				return true;
			}
			if (flag2)
			{
				memberInfo = fieldInfo;
				return true;
			}
		}
		errorMessage = CoreStrings.NoFieldOrGetter(propertyBase.Name, propertyBase.DeclaringType.DisplayName());
		return false;
	}

	private static string GetNoFieldErrorMessage(IPropertyBase propertyBase)
	{
		InstantiationBinding obj = (InstantiationBinding)propertyBase.DeclaringType["ConstructorBinding"];
		if (obj == null || !obj.ParameterBindings.OfType<ServiceParameterBinding>().Any((ServiceParameterBinding b) => b.ServiceType == typeof(ILazyLoader)))
		{
			return CoreStrings.NoBackingField(propertyBase.Name, propertyBase.DeclaringType.DisplayName(), "PropertyAccessMode");
		}
		return CoreStrings.NoBackingFieldLazyLoading(propertyBase.Name, propertyBase.DeclaringType.DisplayName());
	}

	public static PropertyBase AsPropertyBase([JetBrains.Annotations.NotNull] this IPropertyBase propertyBase, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IPropertyBase, PropertyBase>(propertyBase, methodName);
	}
}
