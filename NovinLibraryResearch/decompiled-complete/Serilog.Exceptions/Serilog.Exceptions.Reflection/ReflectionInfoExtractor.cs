using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Serilog.Exceptions.Reflection;

internal class ReflectionInfoExtractor
{
	private readonly ConcurrentDictionary<Type, ReflectionInfo> reflectionInfoCache = new ConcurrentDictionary<Type, ReflectionInfo>();

	private readonly IList<PropertyInfo> baseExceptionPropertiesForDestructuring;

	public ReflectionInfoExtractor()
	{
		baseExceptionPropertiesForDestructuring = GetExceptionPropertiesForDestructuring(typeof(Exception));
	}

	public ReflectionInfo GetOrCreateReflectionInfo(Type valueType)
	{
		return reflectionInfoCache.GetOrAdd(valueType, GenerateReflectionInfoForType);
	}

	private static Func<object, object> GenerateFastGetterForProperty(Type type, PropertyInfo property)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "num");
		return Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Property(Expression.Convert(parameterExpression, type), property), typeof(object)), new ParameterExpression[1] { parameterExpression }).Compile();
	}

	private static List<PropertyInfo> GetExceptionPropertiesForDestructuring(Type valueType)
	{
		return (from x in valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			where x.CanRead && x.GetIndexParameters().Length == 0
			select x).ToList();
	}

	private static void MarkRedefinedPropertiesWithFullName(ReflectionPropertyInfo[] propertyInfos)
	{
		Dictionary<string, List<ReflectionPropertyInfo>> dictionary = new Dictionary<string, List<ReflectionPropertyInfo>>();
		foreach (ReflectionPropertyInfo reflectionPropertyInfo in propertyInfos)
		{
			if (dictionary.TryGetValue(reflectionPropertyInfo.Name, out var value))
			{
				value.Add(reflectionPropertyInfo);
				continue;
			}
			dictionary[reflectionPropertyInfo.Name] = new List<ReflectionPropertyInfo> { reflectionPropertyInfo };
		}
		foreach (KeyValuePair<string, List<ReflectionPropertyInfo>> item in dictionary)
		{
			List<ReflectionPropertyInfo> value2 = item.Value;
			if (value2.Count > 1)
			{
				FixGroupOfPropertiesWithConflictingNames(value2);
			}
		}
	}

	private static void FixGroupOfPropertiesWithConflictingNames(List<ReflectionPropertyInfo> properties)
	{
		foreach (ReflectionPropertyInfo property in properties)
		{
			foreach (ReflectionPropertyInfo property2 in properties)
			{
				property.MarkNameWithTypeNameIfRedefinesThatProperty(property2);
			}
		}
	}

	private ReflectionInfo GenerateReflectionInfoForType(Type valueType)
	{
		Type valueType2 = valueType;
		ReflectionPropertyInfo[] array = (from p in GetExceptionPropertiesForDestructuring(valueType2)
			select new ReflectionPropertyInfo(p.Name, p.DeclaringType, GenerateFastGetterForProperty(valueType2, p))).ToArray();
		MarkRedefinedPropertiesWithFullName(array);
		ReflectionPropertyInfo[] propertiesExceptBaseOnes = array.Where((ReflectionPropertyInfo p) => baseExceptionPropertiesForDestructuring.All((PropertyInfo bp) => bp.Name != p.Name)).ToArray();
		return new ReflectionInfo(array, propertiesExceptBaseOnes);
	}
}
