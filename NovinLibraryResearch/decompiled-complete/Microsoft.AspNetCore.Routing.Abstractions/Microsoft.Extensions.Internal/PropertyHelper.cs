using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.Internal;

internal class PropertyHelper
{
	private delegate TValue ByRefFunc<TDeclaringType, TValue>(ref TDeclaringType arg);

	private static readonly MethodInfo CallPropertyGetterOpenGenericMethod = typeof(PropertyHelper).GetTypeInfo().GetDeclaredMethod("CallPropertyGetter");

	private static readonly MethodInfo CallPropertyGetterByReferenceOpenGenericMethod = typeof(PropertyHelper).GetTypeInfo().GetDeclaredMethod("CallPropertyGetterByReference");

	private static readonly MethodInfo CallNullSafePropertyGetterOpenGenericMethod = typeof(PropertyHelper).GetTypeInfo().GetDeclaredMethod("CallNullSafePropertyGetter");

	private static readonly MethodInfo CallNullSafePropertyGetterByReferenceOpenGenericMethod = typeof(PropertyHelper).GetTypeInfo().GetDeclaredMethod("CallNullSafePropertyGetterByReference");

	private static readonly MethodInfo CallPropertySetterOpenGenericMethod = typeof(PropertyHelper).GetTypeInfo().GetDeclaredMethod("CallPropertySetter");

	private static readonly ConcurrentDictionary<Type, PropertyHelper[]> PropertiesCache = new ConcurrentDictionary<Type, PropertyHelper[]>();

	private static readonly ConcurrentDictionary<Type, PropertyHelper[]> VisiblePropertiesCache = new ConcurrentDictionary<Type, PropertyHelper[]>();

	private Action<object, object> _valueSetter;

	private Func<object, object> _valueGetter;

	public PropertyInfo Property { get; }

	public virtual string Name { get; protected set; }

	public Func<object, object> ValueGetter
	{
		get
		{
			if (_valueGetter == null)
			{
				_valueGetter = MakeFastPropertyGetter(Property);
			}
			return _valueGetter;
		}
	}

	public Action<object, object> ValueSetter
	{
		get
		{
			if (_valueSetter == null)
			{
				_valueSetter = MakeFastPropertySetter(Property);
			}
			return _valueSetter;
		}
	}

	public PropertyHelper(PropertyInfo property)
	{
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		Property = property;
		Name = property.Name;
	}

	public object GetValue(object instance)
	{
		return ValueGetter(instance);
	}

	public void SetValue(object instance, object value)
	{
		ValueSetter(instance, value);
	}

	public static PropertyHelper[] GetProperties(TypeInfo typeInfo)
	{
		return GetProperties(typeInfo.AsType());
	}

	public static PropertyHelper[] GetProperties(Type type)
	{
		return GetProperties(type, CreateInstance, PropertiesCache);
	}

	public static PropertyHelper[] GetVisibleProperties(TypeInfo typeInfo)
	{
		return GetVisibleProperties(typeInfo.AsType(), CreateInstance, PropertiesCache, VisiblePropertiesCache);
	}

	public static PropertyHelper[] GetVisibleProperties(Type type)
	{
		return GetVisibleProperties(type, CreateInstance, PropertiesCache, VisiblePropertiesCache);
	}

	public static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo)
	{
		return MakeFastPropertyGetter(propertyInfo, CallPropertyGetterOpenGenericMethod, CallPropertyGetterByReferenceOpenGenericMethod);
	}

	public static Func<object, object> MakeNullSafeFastPropertyGetter(PropertyInfo propertyInfo)
	{
		return MakeFastPropertyGetter(propertyInfo, CallNullSafePropertyGetterOpenGenericMethod, CallNullSafePropertyGetterByReferenceOpenGenericMethod);
	}

	private static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo, MethodInfo propertyGetterWrapperMethod, MethodInfo propertyGetterByRefWrapperMethod)
	{
		MethodInfo getMethod = propertyInfo.GetMethod;
		if (getMethod.DeclaringType.GetTypeInfo().IsValueType)
		{
			return MakeFastPropertyGetter(typeof(ByRefFunc<, >), getMethod, propertyGetterByRefWrapperMethod);
		}
		return MakeFastPropertyGetter(typeof(Func<, >), getMethod, propertyGetterWrapperMethod);
	}

	private static Func<object, object> MakeFastPropertyGetter(Type openGenericDelegateType, MethodInfo propertyGetMethod, MethodInfo openGenericWrapperMethod)
	{
		Type declaringType = propertyGetMethod.DeclaringType;
		Type returnType = propertyGetMethod.ReturnType;
		Type delegateType = openGenericDelegateType.MakeGenericType(declaringType, returnType);
		Delegate target = propertyGetMethod.CreateDelegate(delegateType);
		return (Func<object, object>)openGenericWrapperMethod.MakeGenericMethod(declaringType, returnType).CreateDelegate(typeof(Func<object, object>), target);
	}

	public static Action<object, object> MakeFastPropertySetter(PropertyInfo propertyInfo)
	{
		MethodInfo? setMethod = propertyInfo.SetMethod;
		ParameterInfo[] parameters = setMethod.GetParameters();
		Type declaringType = setMethod.DeclaringType;
		Type parameterType = parameters[0].ParameterType;
		Delegate target = setMethod.CreateDelegate(typeof(Action<, >).MakeGenericType(declaringType, parameterType));
		return (Action<object, object>)CallPropertySetterOpenGenericMethod.MakeGenericMethod(declaringType, parameterType).CreateDelegate(typeof(Action<object, object>), target);
	}

	public static IDictionary<string, object> ObjectToDictionary(object value)
	{
		if (value is IDictionary<string, object> dictionary)
		{
			return new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
		}
		IDictionary<string, object> dictionary2 = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		if (value != null)
		{
			PropertyHelper[] properties = GetProperties(value.GetType());
			foreach (PropertyHelper propertyHelper in properties)
			{
				dictionary2[propertyHelper.Name] = propertyHelper.GetValue(value);
			}
		}
		return dictionary2;
	}

	private static PropertyHelper CreateInstance(PropertyInfo property)
	{
		return new PropertyHelper(property);
	}

	private static object CallPropertyGetter<TDeclaringType, TValue>(Func<TDeclaringType, TValue> getter, object target)
	{
		return getter((TDeclaringType)target);
	}

	private static object CallPropertyGetterByReference<TDeclaringType, TValue>(ByRefFunc<TDeclaringType, TValue> getter, object target)
	{
		TDeclaringType arg = (TDeclaringType)target;
		return getter(ref arg);
	}

	private static object CallNullSafePropertyGetter<TDeclaringType, TValue>(Func<TDeclaringType, TValue> getter, object target)
	{
		if (target == null)
		{
			return null;
		}
		return getter((TDeclaringType)target);
	}

	private static object CallNullSafePropertyGetterByReference<TDeclaringType, TValue>(ByRefFunc<TDeclaringType, TValue> getter, object target)
	{
		if (target == null)
		{
			return null;
		}
		TDeclaringType arg = (TDeclaringType)target;
		return getter(ref arg);
	}

	private static void CallPropertySetter<TDeclaringType, TValue>(Action<TDeclaringType, TValue> setter, object target, object value)
	{
		setter((TDeclaringType)target, (TValue)value);
	}

	protected static PropertyHelper[] GetVisibleProperties(Type type, Func<PropertyInfo, PropertyHelper> createPropertyHelper, ConcurrentDictionary<Type, PropertyHelper[]> allPropertiesCache, ConcurrentDictionary<Type, PropertyHelper[]> visiblePropertiesCache)
	{
		if (visiblePropertiesCache.TryGetValue(type, out var value))
		{
			return value;
		}
		bool flag = true;
		PropertyHelper[] properties = GetProperties(type, createPropertyHelper, allPropertiesCache);
		PropertyHelper[] array = properties;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Property.DeclaringType != type)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			value = properties;
			visiblePropertiesCache.TryAdd(type, value);
			return value;
		}
		List<PropertyHelper> list = new List<PropertyHelper>(properties.Length);
		array = properties;
		foreach (PropertyHelper propertyHelper in array)
		{
			Type declaringType = propertyHelper.Property.DeclaringType;
			if (declaringType == type)
			{
				list.Add(propertyHelper);
				continue;
			}
			bool flag2 = false;
			TypeInfo typeInfo = type.GetTypeInfo();
			TypeInfo typeInfo2 = declaringType.GetTypeInfo();
			while (typeInfo != null && typeInfo != typeInfo2)
			{
				if (typeInfo.GetDeclaredProperty(propertyHelper.Name) != null)
				{
					flag2 = true;
					break;
				}
				typeInfo = typeInfo.BaseType?.GetTypeInfo();
			}
			if (!flag2)
			{
				list.Add(propertyHelper);
			}
		}
		value = list.ToArray();
		visiblePropertiesCache.TryAdd(type, value);
		return value;
	}

	protected static PropertyHelper[] GetProperties(Type type, Func<PropertyInfo, PropertyHelper> createPropertyHelper, ConcurrentDictionary<Type, PropertyHelper[]> cache)
	{
		type = Nullable.GetUnderlyingType(type) ?? type;
		if (!cache.TryGetValue(type, out var value))
		{
			IEnumerable<PropertyInfo> enumerable = type.GetRuntimeProperties().Where(IsInterestingProperty);
			TypeInfo typeInfo = type.GetTypeInfo();
			if (typeInfo.IsInterface)
			{
				enumerable = enumerable.Concat(typeInfo.ImplementedInterfaces.SelectMany((Type interfaceType) => interfaceType.GetRuntimeProperties().Where(IsInterestingProperty)));
			}
			value = enumerable.Select((PropertyInfo p) => createPropertyHelper(p)).ToArray();
			cache.TryAdd(type, value);
		}
		return value;
	}

	private static bool IsInterestingProperty(PropertyInfo property)
	{
		if (property.GetMethod != null && property.GetMethod.IsPublic && !property.GetMethod.IsStatic)
		{
			return property.GetMethod.GetParameters().Length == 0;
		}
		return false;
	}
}
