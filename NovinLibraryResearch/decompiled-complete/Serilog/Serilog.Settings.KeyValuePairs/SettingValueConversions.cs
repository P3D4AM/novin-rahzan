using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Serilog.Settings.KeyValuePairs;

internal class SettingValueConversions
{
	private static Regex StaticMemberAccessorRegex = new Regex("^(?<shortTypeName>[^:]+)::(?<memberName>[A-Za-z][A-Za-z0-9]*)(?<typeNameExtraQualifiers>[^:]*)$");

	private static Dictionary<Type, Func<string, object>> ExtendedTypeConversions = new Dictionary<Type, Func<string, object>>
	{
		{
			typeof(Uri),
			(string s) => new Uri(s)
		},
		{
			typeof(TimeSpan),
			(string s) => TimeSpan.Parse(s)
		},
		{
			typeof(Type),
			(string s) => Type.GetType(s, throwOnError: true)
		}
	};

	public static object? ConvertToType(string value, Type toType)
	{
		Type toType2 = toType;
		if (toType2.IsGenericType && toType2.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			if (value == string.Empty)
			{
				return null;
			}
			toType2 = toType2.GenericTypeArguments[0];
		}
		if (toType2.IsEnum)
		{
			return Enum.Parse(toType2, value);
		}
		Func<string, object> func = (from t in ExtendedTypeConversions
			where t.Key.IsAssignableFrom(toType2)
			select t.Value).FirstOrDefault();
		if (func != null)
		{
			return func(value);
		}
		if ((toType2.IsInterface || toType2.IsAbstract) && !string.IsNullOrWhiteSpace(value))
		{
			if (TryParseStaticMemberAccessor(value, out string accessorTypeName, out string memberName))
			{
				Type type = Type.GetType(accessorTypeName, throwOnError: true);
				PropertyInfo propertyInfo = type.GetProperties(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((PropertyInfo x) => x.Name == memberName && x.GetMethod != null);
				if (propertyInfo != null)
				{
					return propertyInfo.GetValue(null);
				}
				FieldInfo fieldInfo = type.GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => x.Name == memberName);
				if (fieldInfo != null)
				{
					return fieldInfo.GetValue(null);
				}
				throw new InvalidOperationException("Could not find a public static property or field with name `" + memberName + "` on type `" + accessorTypeName + "`");
			}
			Type type2 = Type.GetType(value.Trim(), throwOnError: false);
			if (type2 != null)
			{
				ConstructorInfo? constructorInfo = type2.GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(delegate(ConstructorInfo ci)
				{
					ParameterInfo[] parameters2 = ci.GetParameters();
					return parameters2.Length == 0 || parameters2.All((ParameterInfo pi) => pi.HasDefaultValue);
				});
				if (constructorInfo == null)
				{
					throw new InvalidOperationException("A default constructor was not found on " + type2.FullName + ".");
				}
				object[] parameters = (from pi in constructorInfo.GetParameters()
					select pi.DefaultValue).ToArray();
				return constructorInfo.Invoke(parameters);
			}
		}
		if (toType2.IsArray && toType2.GetArrayRank() == 1)
		{
			Type elementType = toType2.GetElementType();
			if (string.IsNullOrEmpty(value))
			{
				return Array.CreateInstance(elementType, 0);
			}
			string[] array = value.Split(new char[1] { ',' });
			int num = array.Length;
			Array array2 = Array.CreateInstance(elementType, array.Length);
			for (int i = 0; i < num; i++)
			{
				object value2 = ConvertToType(array[i], elementType);
				array2.SetValue(value2, i);
			}
			return array2;
		}
		return Convert.ChangeType(value, toType2);
	}

	internal static bool TryParseStaticMemberAccessor(string input, [NotNullWhen(true)] out string? accessorTypeName, [NotNullWhen(true)] out string? memberName)
	{
		if (StaticMemberAccessorRegex.IsMatch(input))
		{
			Match match = StaticMemberAccessorRegex.Match(input);
			string value = match.Groups["shortTypeName"].Value;
			string value2 = match.Groups["memberName"].Value;
			string value3 = match.Groups["typeNameExtraQualifiers"].Value;
			memberName = value2.Trim();
			accessorTypeName = value.Trim() + value3.TrimEnd(Array.Empty<char>());
			return true;
		}
		accessorTypeName = null;
		memberName = null;
		return false;
	}
}
