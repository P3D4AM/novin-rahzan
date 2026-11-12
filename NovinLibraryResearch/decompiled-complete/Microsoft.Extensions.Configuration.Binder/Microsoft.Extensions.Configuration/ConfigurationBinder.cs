using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationBinder
{
	private const BindingFlags DeclaredOnlyLookup = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	private const string TrimmingWarningMessage = "In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.";

	private const string InstanceGetTypeTrimmingWarningMessage = "Cannot statically analyze the type of instance so its members may be trimmed";

	private const string PropertyTrimmingWarningMessage = "Cannot statically analyze property.PropertyType so its members may be trimmed.";

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static T Get<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration)
	{
		return configuration.Get<T>(delegate
		{
		});
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static T Get<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration, Action<BinderOptions> configureOptions)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		object obj = configuration.Get(typeof(T), configureOptions);
		if (obj == null)
		{
			return default(T);
		}
		return (T)obj;
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static object Get(this IConfiguration configuration, Type type)
	{
		return configuration.Get(type, delegate
		{
		});
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static object Get(this IConfiguration configuration, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, Action<BinderOptions> configureOptions)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		BinderOptions binderOptions = new BinderOptions();
		configureOptions?.Invoke(binderOptions);
		return BindInstance(type, null, configuration, binderOptions);
	}

	[RequiresUnreferencedCode("Cannot statically analyze the type of instance so its members may be trimmed")]
	public static void Bind(this IConfiguration configuration, string key, object instance)
	{
		configuration.GetSection(key).Bind(instance);
	}

	[RequiresUnreferencedCode("Cannot statically analyze the type of instance so its members may be trimmed")]
	public static void Bind(this IConfiguration configuration, object instance)
	{
		configuration.Bind(instance, delegate
		{
		});
	}

	[RequiresUnreferencedCode("Cannot statically analyze the type of instance so its members may be trimmed")]
	public static void Bind(this IConfiguration configuration, object instance, Action<BinderOptions> configureOptions)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		if (instance != null)
		{
			BinderOptions binderOptions = new BinderOptions();
			configureOptions?.Invoke(binderOptions);
			BindInstance(instance.GetType(), instance, configuration, binderOptions);
		}
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static T GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration, string key)
	{
		return configuration.GetValue(key, default(T));
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static T GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T>(this IConfiguration configuration, string key, T defaultValue)
	{
		return (T)configuration.GetValue(typeof(T), key, defaultValue);
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static object GetValue(this IConfiguration configuration, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string key)
	{
		return configuration.GetValue(type, key, null);
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	public static object GetValue(this IConfiguration configuration, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string key, object defaultValue)
	{
		IConfigurationSection section = configuration.GetSection(key);
		string value = section.Value;
		if (value != null)
		{
			return ConvertValue(type, value, section.Path);
		}
		return defaultValue;
	}

	[RequiresUnreferencedCode("Cannot statically analyze property.PropertyType so its members may be trimmed.")]
	private static void BindNonScalar(this IConfiguration configuration, object instance, BinderOptions options)
	{
		if (instance == null)
		{
			return;
		}
		List<PropertyInfo> allProperties = GetAllProperties(instance.GetType());
		if (options.ErrorOnUnknownConfiguration)
		{
			HashSet<string> propertyNames = new HashSet<string>(allProperties.Select((PropertyInfo mp) => mp.Name), StringComparer.OrdinalIgnoreCase);
			IEnumerable<IConfigurationSection> children = configuration.GetChildren();
			List<string> list = (from cs in children
				where !propertyNames.Contains(cs.Key)
				select cs into mp
				select "'" + mp.Key + "'").ToList();
			if (list.Count > 0)
			{
				throw new InvalidOperationException(System.SR.Format(System.SR.Error_MissingConfig, "ErrorOnUnknownConfiguration", "BinderOptions", instance.GetType(), string.Join(", ", list)));
			}
		}
		foreach (PropertyInfo item in allProperties)
		{
			BindProperty(item, instance, configuration, options);
		}
	}

	[RequiresUnreferencedCode("Cannot statically analyze property.PropertyType so its members may be trimmed.")]
	private static void BindProperty(PropertyInfo property, object instance, IConfiguration config, BinderOptions options)
	{
		if (property.GetMethod == null || (!options.BindNonPublicProperties && !property.GetMethod.IsPublic) || property.GetMethod.GetParameters().Length != 0)
		{
			return;
		}
		object value = property.GetValue(instance);
		bool flag = property.SetMethod != null && (property.SetMethod.IsPublic || options.BindNonPublicProperties);
		if (value != null || flag)
		{
			value = GetPropertyValue(property, instance, config, options);
			if (value != null && flag)
			{
				property.SetValue(instance, value);
			}
		}
	}

	[RequiresUnreferencedCode("Cannot statically analyze what the element type is of the object collection in type so its members may be trimmed.")]
	private static object BindToCollection(Type type, IConfiguration config, BinderOptions options)
	{
		Type type2 = typeof(List<>).MakeGenericType(type.GenericTypeArguments[0]);
		object obj = Activator.CreateInstance(type2);
		BindCollection(obj, type2, config, options);
		return obj;
	}

	[RequiresUnreferencedCode("In case type is a Dictionary, cannot statically analyze what the element type is of the value objects in the dictionary so its members may be trimmed.")]
	private static object AttemptBindToCollectionInterfaces([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, IConfiguration config, BinderOptions options)
	{
		if (!type.IsInterface)
		{
			return null;
		}
		Type type2 = FindOpenGenericInterface(typeof(IReadOnlyList<>), type);
		if (type2 != null)
		{
			return BindToCollection(type, config, options);
		}
		type2 = FindOpenGenericInterface(typeof(IReadOnlyDictionary<, >), type);
		if (type2 != null)
		{
			Type type3 = typeof(Dictionary<, >).MakeGenericType(type.GenericTypeArguments[0], type.GenericTypeArguments[1]);
			object obj = Activator.CreateInstance(type3);
			BindDictionary(obj, type3, config, options);
			return obj;
		}
		type2 = FindOpenGenericInterface(typeof(IDictionary<, >), type);
		if (type2 != null)
		{
			object obj2 = Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(type.GenericTypeArguments[0], type.GenericTypeArguments[1]));
			BindDictionary(obj2, type2, config, options);
			return obj2;
		}
		type2 = FindOpenGenericInterface(typeof(IReadOnlyCollection<>), type);
		if (type2 != null)
		{
			return BindToCollection(type, config, options);
		}
		type2 = FindOpenGenericInterface(typeof(ICollection<>), type);
		if (type2 != null)
		{
			return BindToCollection(type, config, options);
		}
		type2 = FindOpenGenericInterface(typeof(IEnumerable<>), type);
		if (type2 != null)
		{
			return BindToCollection(type, config, options);
		}
		return null;
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	private static object BindInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, object instance, IConfiguration config, BinderOptions options)
	{
		if (type == typeof(IConfigurationSection))
		{
			return config;
		}
		IConfigurationSection configurationSection = config as IConfigurationSection;
		string text = configurationSection?.Value;
		if (text != null && TryConvertValue(type, text, configurationSection.Path, out var result, out var error))
		{
			if (error != null)
			{
				throw error;
			}
			return result;
		}
		if (config != null && config.GetChildren().Any())
		{
			if (instance == null)
			{
				instance = AttemptBindToCollectionInterfaces(type, config, options);
				if (instance != null)
				{
					return instance;
				}
				instance = CreateInstance(type);
			}
			Type type2 = FindOpenGenericInterface(typeof(IDictionary<, >), type);
			if (type2 != null)
			{
				BindDictionary(instance, type2, config, options);
			}
			else if (type.IsArray)
			{
				instance = BindArray((Array)instance, config, options);
			}
			else
			{
				type2 = FindOpenGenericInterface(typeof(ICollection<>), type);
				if (type2 != null)
				{
					BindCollection(instance, type2, config, options);
				}
				else
				{
					config.BindNonScalar(instance, options);
				}
			}
		}
		return instance;
	}

	private static object CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type)
	{
		if (type.IsInterface || type.IsAbstract)
		{
			throw new InvalidOperationException(System.SR.Format(System.SR.Error_CannotActivateAbstractOrInterface, type));
		}
		if (type.IsArray)
		{
			if (type.GetArrayRank() > 1)
			{
				throw new InvalidOperationException(System.SR.Format(System.SR.Error_UnsupportedMultidimensionalArray, type));
			}
			return Array.CreateInstance(type.GetElementType(), 0);
		}
		if (!type.IsValueType && !type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Any((ConstructorInfo ctor) => ctor.IsPublic && ctor.GetParameters().Length == 0))
		{
			throw new InvalidOperationException(System.SR.Format(System.SR.Error_MissingParameterlessConstructor, type));
		}
		try
		{
			return Activator.CreateInstance(type);
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException(System.SR.Format(System.SR.Error_FailedToActivate, type), innerException);
		}
	}

	[RequiresUnreferencedCode("Cannot statically analyze what the element type is of the value objects in the dictionary so its members may be trimmed.")]
	private static void BindDictionary(object dictionary, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] Type dictionaryType, IConfiguration config, BinderOptions options)
	{
		Type type = dictionaryType.GenericTypeArguments[0];
		Type type2 = dictionaryType.GenericTypeArguments[1];
		bool isEnum = type.IsEnum;
		if (type != typeof(string) && !isEnum)
		{
			return;
		}
		PropertyInfo property = dictionaryType.GetProperty("Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (IConfigurationSection child in config.GetChildren())
		{
			object obj = BindInstance(type2, null, child, options);
			if (obj != null)
			{
				if (type == typeof(string))
				{
					string key = child.Key;
					property.SetValue(dictionary, obj, new object[1] { key });
				}
				else if (isEnum)
				{
					object obj2 = Enum.Parse(type, child.Key);
					property.SetValue(dictionary, obj, new object[1] { obj2 });
				}
			}
		}
	}

	[RequiresUnreferencedCode("Cannot statically analyze what the element type is of the object collection so its members may be trimmed.")]
	private static void BindCollection(object collection, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type collectionType, IConfiguration config, BinderOptions options)
	{
		Type type = collectionType.GenericTypeArguments[0];
		MethodInfo method = collectionType.GetMethod("Add", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (IConfigurationSection child in config.GetChildren())
		{
			try
			{
				object obj = BindInstance(type, null, child, options);
				if (obj != null)
				{
					method.Invoke(collection, new object[1] { obj });
				}
			}
			catch
			{
			}
		}
	}

	[RequiresUnreferencedCode("Cannot statically analyze what the element type is of the Array so its members may be trimmed.")]
	private static Array BindArray(Array source, IConfiguration config, BinderOptions options)
	{
		IConfigurationSection[] array = config.GetChildren().ToArray();
		int length = source.Length;
		Type elementType = source.GetType().GetElementType();
		Array array2 = Array.CreateInstance(elementType, length + array.Length);
		if (length > 0)
		{
			Array.Copy(source, array2, length);
		}
		for (int i = 0; i < array.Length; i++)
		{
			try
			{
				object obj = BindInstance(elementType, null, array[i], options);
				if (obj != null)
				{
					array2.SetValue(obj, length + i);
				}
			}
			catch
			{
			}
		}
		return array2;
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	private static bool TryConvertValue([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string value, string path, out object result, out Exception error)
	{
		error = null;
		result = null;
		if (type == typeof(object))
		{
			result = value;
			return true;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			if (string.IsNullOrEmpty(value))
			{
				return true;
			}
			return TryConvertValue(Nullable.GetUnderlyingType(type), value, path, out result, out error);
		}
		TypeConverter converter = TypeDescriptor.GetConverter(type);
		if (converter.CanConvertFrom(typeof(string)))
		{
			try
			{
				result = converter.ConvertFromInvariantString(value);
			}
			catch (Exception innerException)
			{
				error = new InvalidOperationException(System.SR.Format(System.SR.Error_FailedBinding, path, type), innerException);
			}
			return true;
		}
		if (type == typeof(byte[]))
		{
			try
			{
				result = Convert.FromBase64String(value);
			}
			catch (FormatException innerException2)
			{
				error = new InvalidOperationException(System.SR.Format(System.SR.Error_FailedBinding, path, type), innerException2);
			}
			return true;
		}
		return false;
	}

	[RequiresUnreferencedCode("In case the type is non-primitive, the trimmer cannot statically analyze the object's type so its members may be trimmed.")]
	private static object ConvertValue([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type, string value, string path)
	{
		TryConvertValue(type, value, path, out var result, out var error);
		if (error != null)
		{
			throw error;
		}
		return result;
	}

	private static Type FindOpenGenericInterface(Type expected, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type actual)
	{
		if (actual.IsGenericType && actual.GetGenericTypeDefinition() == expected)
		{
			return actual;
		}
		Type[] interfaces = actual.GetInterfaces();
		Type[] array = interfaces;
		foreach (Type type in array)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == expected)
			{
				return type;
			}
		}
		return null;
	}

	private static List<PropertyInfo> GetAllProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type type)
	{
		List<PropertyInfo> list = new List<PropertyInfo>();
		do
		{
			list.AddRange(type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			type = type.BaseType;
		}
		while (type != typeof(object));
		return list;
	}

	[RequiresUnreferencedCode("Cannot statically analyze property.PropertyType so its members may be trimmed.")]
	private static object GetPropertyValue(PropertyInfo property, object instance, IConfiguration config, BinderOptions options)
	{
		string propertyName = GetPropertyName(property);
		return BindInstance(property.PropertyType, property.GetValue(instance), config.GetSection(propertyName), options);
	}

	private static string GetPropertyName(MemberInfo property)
	{
		if (property == null)
		{
			throw new ArgumentNullException("property");
		}
		foreach (CustomAttributeData customAttributesDatum in property.GetCustomAttributesData())
		{
			if (!(customAttributesDatum.AttributeType != typeof(ConfigurationKeyNameAttribute)))
			{
				if (customAttributesDatum.ConstructorArguments.Count != 1)
				{
					break;
				}
				string text = customAttributesDatum.ConstructorArguments[0].Value?.ToString();
				return (!string.IsNullOrWhiteSpace(text)) ? text : property.Name;
			}
		}
		return property.Name;
	}
}
