using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class TypeExtensions
{
	private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>
	{
		{
			typeof(bool),
			"bool"
		},
		{
			typeof(byte),
			"byte"
		},
		{
			typeof(char),
			"char"
		},
		{
			typeof(decimal),
			"decimal"
		},
		{
			typeof(double),
			"double"
		},
		{
			typeof(float),
			"float"
		},
		{
			typeof(int),
			"int"
		},
		{
			typeof(long),
			"long"
		},
		{
			typeof(object),
			"object"
		},
		{
			typeof(sbyte),
			"sbyte"
		},
		{
			typeof(short),
			"short"
		},
		{
			typeof(string),
			"string"
		},
		{
			typeof(uint),
			"uint"
		},
		{
			typeof(ulong),
			"ulong"
		},
		{
			typeof(ushort),
			"ushort"
		},
		{
			typeof(void),
			"void"
		}
	};

	public static bool IsDefaultValue([JetBrains.Annotations.NotNull] this Type type, [JetBrains.Annotations.CanBeNull] object value)
	{
		return value?.Equals(type.GetDefaultValue()) ?? true;
	}

	public static string DisplayName([JetBrains.Annotations.NotNull] this Type type, bool fullName = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		ProcessType(stringBuilder, type, fullName);
		return stringBuilder.ToString();
	}

	private static void ProcessType(StringBuilder builder, Type type, bool fullName)
	{
		string value;
		if (type.IsGenericType)
		{
			Type[] genericArguments = type.GetGenericArguments();
			ProcessGenericType(builder, type, genericArguments, genericArguments.Length, fullName);
		}
		else if (type.IsArray)
		{
			ProcessArrayType(builder, type, fullName);
		}
		else if (_builtInTypeNames.TryGetValue(type, out value))
		{
			builder.Append(value);
		}
		else if (!type.IsGenericParameter)
		{
			builder.Append(fullName ? type.FullName : type.Name);
		}
	}

	private static void ProcessArrayType(StringBuilder builder, Type type, bool fullName)
	{
		Type type2 = type;
		while (type2.IsArray)
		{
			type2 = type2.GetElementType();
		}
		ProcessType(builder, type2, fullName);
		while (type.IsArray)
		{
			builder.Append('[');
			builder.Append(',', type.GetArrayRank() - 1);
			builder.Append(']');
			type = type.GetElementType();
		}
	}

	private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length, bool fullName)
	{
		int num = (type.IsNested ? type.DeclaringType.GetGenericArguments().Length : 0);
		if (fullName)
		{
			if (type.IsNested)
			{
				ProcessGenericType(builder, type.DeclaringType, genericArguments, num, fullName);
				builder.Append('+');
			}
			else
			{
				builder.Append(type.Namespace);
				builder.Append('.');
			}
		}
		int num2 = type.Name.IndexOf('`');
		if (num2 <= 0)
		{
			builder.Append(type.Name);
			return;
		}
		builder.Append(type.Name, 0, num2);
		builder.Append('<');
		for (int i = num; i < length; i++)
		{
			ProcessType(builder, genericArguments[i], fullName);
			if (i + 1 != length)
			{
				builder.Append(',');
				if (!genericArguments[i + 1].IsGenericParameter)
				{
					builder.Append(' ');
				}
			}
		}
		builder.Append('>');
	}

	public static FieldInfo GetFieldInfo([JetBrains.Annotations.NotNull] this Type type, [JetBrains.Annotations.NotNull] string fieldName)
	{
		return type.GetRuntimeFields().FirstOrDefault((FieldInfo f) => f.Name == fieldName && !f.IsStatic);
	}

	public static IEnumerable<string> GetNamespaces([JetBrains.Annotations.NotNull] this Type type)
	{
		if (_builtInTypeNames.ContainsKey(type))
		{
			yield break;
		}
		yield return type.Namespace;
		if (!type.GetTypeInfo().IsGenericType)
		{
			yield break;
		}
		Type[] genericTypeArguments = type.GetTypeInfo().GenericTypeArguments;
		foreach (Type type2 in genericTypeArguments)
		{
			foreach (string @namespace in type2.GetNamespaces())
			{
				yield return @namespace;
			}
		}
	}

	public static string GenerateParameterName(this Type type)
	{
		string text = new StringBuilder().Append(type.Name.Where(char.IsUpper).ToArray()).ToString();
		if (text.Length <= 0)
		{
			return type.Name.ToLower().Substring(0, 1);
		}
		return text.ToLower();
	}

	public static bool IsQueryableType(this Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IQueryable<>))
		{
			return true;
		}
		return type.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryable<>));
	}
}
