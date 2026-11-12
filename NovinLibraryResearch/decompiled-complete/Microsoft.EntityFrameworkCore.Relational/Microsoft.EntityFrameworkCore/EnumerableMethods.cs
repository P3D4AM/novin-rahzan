using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore;

internal static class EnumerableMethods
{
	public static MethodInfo All { get; }

	public static MethodInfo AnyWithoutPredicate { get; }

	public static MethodInfo AnyWithPredicate { get; }

	public static MethodInfo AsEnumerable { get; }

	public static MethodInfo Cast { get; }

	public static MethodInfo Concat { get; }

	public static MethodInfo Contains { get; }

	public static MethodInfo CountWithoutPredicate { get; }

	public static MethodInfo CountWithPredicate { get; }

	public static MethodInfo DefaultIfEmptyWithoutArgument { get; }

	public static MethodInfo DefaultIfEmptyWithArgument { get; }

	public static MethodInfo Distinct { get; }

	public static MethodInfo ElementAt { get; }

	public static MethodInfo ElementAtOrDefault { get; }

	public static MethodInfo Except { get; }

	public static MethodInfo FirstWithoutPredicate { get; }

	public static MethodInfo FirstWithPredicate { get; }

	public static MethodInfo FirstOrDefaultWithoutPredicate { get; }

	public static MethodInfo FirstOrDefaultWithPredicate { get; }

	public static MethodInfo GroupByWithKeySelector { get; }

	public static MethodInfo GroupByWithKeyElementSelector { get; }

	public static MethodInfo GroupByWithKeyElementResultSelector { get; }

	public static MethodInfo GroupByWithKeyResultSelector { get; }

	public static MethodInfo GroupJoin { get; }

	public static MethodInfo Intersect { get; }

	public static MethodInfo Join { get; }

	public static MethodInfo LastWithoutPredicate { get; }

	public static MethodInfo LastWithPredicate { get; }

	public static MethodInfo LastOrDefaultWithoutPredicate { get; }

	public static MethodInfo LastOrDefaultWithPredicate { get; }

	public static MethodInfo LongCountWithoutPredicate { get; }

	public static MethodInfo LongCountWithPredicate { get; }

	public static MethodInfo MaxWithoutSelector { get; }

	public static MethodInfo MaxWithSelector { get; }

	public static MethodInfo MinWithoutSelector { get; }

	public static MethodInfo MinWithSelector { get; }

	public static MethodInfo OfType { get; }

	public static MethodInfo OrderBy { get; }

	public static MethodInfo OrderByDescending { get; }

	public static MethodInfo Reverse { get; }

	public static MethodInfo Select { get; }

	public static MethodInfo SelectWithOrdinal { get; }

	public static MethodInfo SelectManyWithoutCollectionSelector { get; }

	public static MethodInfo SelectManyWithCollectionSelector { get; }

	public static MethodInfo SequenceEqual { get; }

	public static MethodInfo SingleWithoutPredicate { get; }

	public static MethodInfo SingleWithPredicate { get; }

	public static MethodInfo SingleOrDefaultWithoutPredicate { get; }

	public static MethodInfo SingleOrDefaultWithPredicate { get; }

	public static MethodInfo Skip { get; }

	public static MethodInfo SkipWhile { get; }

	public static MethodInfo Take { get; }

	public static MethodInfo TakeWhile { get; }

	public static MethodInfo ThenBy { get; }

	public static MethodInfo ThenByDescending { get; }

	public static MethodInfo ToArray { get; }

	public static MethodInfo ToList { get; }

	public static MethodInfo Union { get; }

	public static MethodInfo Where { get; }

	private static Dictionary<Type, MethodInfo> SumWithoutSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> SumWithSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> AverageWithoutSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> AverageWithSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> MaxWithoutSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> MaxWithSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> MinWithoutSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> MinWithSelectorMethods { get; }

	public static bool IsSumWithoutSelector(MethodInfo methodInfo)
	{
		return SumWithoutSelectorMethods.Values.Contains(methodInfo);
	}

	public static bool IsSumWithSelector(MethodInfo methodInfo)
	{
		if (methodInfo.IsGenericMethod)
		{
			return SumWithSelectorMethods.Values.Contains(methodInfo.GetGenericMethodDefinition());
		}
		return false;
	}

	public static bool IsAverageWithoutSelector(MethodInfo methodInfo)
	{
		return AverageWithoutSelectorMethods.Values.Contains(methodInfo);
	}

	public static bool IsAverageWithSelector(MethodInfo methodInfo)
	{
		if (methodInfo.IsGenericMethod)
		{
			return AverageWithSelectorMethods.Values.Contains(methodInfo.GetGenericMethodDefinition());
		}
		return false;
	}

	public static MethodInfo GetSumWithoutSelector(Type type)
	{
		return SumWithoutSelectorMethods[type];
	}

	public static MethodInfo GetSumWithSelector(Type type)
	{
		return SumWithSelectorMethods[type];
	}

	public static MethodInfo GetAverageWithoutSelector(Type type)
	{
		return AverageWithoutSelectorMethods[type];
	}

	public static MethodInfo GetAverageWithSelector(Type type)
	{
		return AverageWithSelectorMethods[type];
	}

	public static MethodInfo GetMaxWithoutSelector(Type type)
	{
		if (!MaxWithoutSelectorMethods.TryGetValue(type, out var value))
		{
			return MaxWithoutSelector;
		}
		return value;
	}

	public static MethodInfo GetMaxWithSelector(Type type)
	{
		if (!MaxWithSelectorMethods.TryGetValue(type, out var value))
		{
			return MaxWithSelector;
		}
		return value;
	}

	public static MethodInfo GetMinWithoutSelector(Type type)
	{
		if (!MinWithoutSelectorMethods.TryGetValue(type, out var value))
		{
			return MinWithoutSelector;
		}
		return value;
	}

	public static MethodInfo GetMinWithSelector(Type type)
	{
		if (!MinWithSelectorMethods.TryGetValue(type, out var value))
		{
			return MinWithSelector;
		}
		return value;
	}

	static EnumerableMethods()
	{
		Dictionary<string, List<MethodInfo>> queryableMethodGroups = (from mi in typeof(Enumerable).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
			group mi by mi.Name).ToDictionary((IGrouping<string, MethodInfo> e) => e.Key, (IGrouping<string, MethodInfo> l) => l.ToList());
		All = GetMethod("All", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		AnyWithoutPredicate = GetMethod("Any", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		AnyWithPredicate = GetMethod("Any", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		AsEnumerable = GetMethod("AsEnumerable", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		Cast = GetMethod("Cast", 1, (Type[] types) => new Type[1] { typeof(IEnumerable) });
		Concat = GetMethod("Concat", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		Contains = GetMethod("Contains", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			types[0]
		});
		CountWithoutPredicate = GetMethod("Count", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		CountWithPredicate = GetMethod("Count", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		DefaultIfEmptyWithoutArgument = GetMethod("DefaultIfEmpty", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		DefaultIfEmptyWithArgument = GetMethod("DefaultIfEmpty", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			types[0]
		});
		Distinct = GetMethod("Distinct", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		ElementAt = GetMethod("ElementAt", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		ElementAtOrDefault = GetMethod("ElementAtOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		Except = GetMethod("Except", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		FirstWithoutPredicate = GetMethod("First", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		FirstWithPredicate = GetMethod("First", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		FirstOrDefaultWithoutPredicate = GetMethod("FirstOrDefault", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		FirstOrDefaultWithPredicate = GetMethod("FirstOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		GroupByWithKeySelector = GetMethod("GroupBy", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		GroupByWithKeyElementSelector = GetMethod("GroupBy", 3, (Type[] types) => new Type[3]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1]),
			typeof(Func<, >).MakeGenericType(types[0], types[2])
		});
		GroupByWithKeyElementResultSelector = GetMethod("GroupBy", 4, (Type[] types) => new Type[4]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1]),
			typeof(Func<, >).MakeGenericType(types[0], types[2]),
			typeof(Func<, , >).MakeGenericType(types[1], typeof(IEnumerable<>).MakeGenericType(types[2]), types[3])
		});
		GroupByWithKeyResultSelector = GetMethod("GroupBy", 3, (Type[] types) => new Type[3]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1]),
			typeof(Func<, , >).MakeGenericType(types[1], typeof(IEnumerable<>).MakeGenericType(types[0]), types[2])
		});
		GroupJoin = GetMethod("GroupJoin", 4, (Type[] types) => new Type[5]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[1]),
			typeof(Func<, >).MakeGenericType(types[0], types[2]),
			typeof(Func<, >).MakeGenericType(types[1], types[2]),
			typeof(Func<, , >).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1]), types[3])
		});
		Intersect = GetMethod("Intersect", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		Join = GetMethod("Join", 4, (Type[] types) => new Type[5]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[1]),
			typeof(Func<, >).MakeGenericType(types[0], types[2]),
			typeof(Func<, >).MakeGenericType(types[1], types[2]),
			typeof(Func<, , >).MakeGenericType(types[0], types[1], types[3])
		});
		LastWithoutPredicate = GetMethod("Last", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		LastWithPredicate = GetMethod("Last", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		LastOrDefaultWithoutPredicate = GetMethod("LastOrDefault", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		LastOrDefaultWithPredicate = GetMethod("LastOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		LongCountWithoutPredicate = GetMethod("LongCount", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		LongCountWithPredicate = GetMethod("LongCount", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		MaxWithoutSelector = GetMethod("Max", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		MaxWithSelector = GetMethod("Max", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		MinWithoutSelector = GetMethod("Min", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		MinWithSelector = GetMethod("Min", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		OfType = GetMethod("OfType", 1, (Type[] types) => new Type[1] { typeof(IEnumerable) });
		OrderBy = GetMethod("OrderBy", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		OrderByDescending = GetMethod("OrderByDescending", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		Reverse = GetMethod("Reverse", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		Select = GetMethod("Select", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		SelectWithOrdinal = GetMethod("Select", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, , >).MakeGenericType(types[0], typeof(int), types[1])
		});
		SelectManyWithoutCollectionSelector = GetMethod("SelectMany", 2, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1]))
		});
		SelectManyWithCollectionSelector = GetMethod("SelectMany", 3, (Type[] types) => new Type[3]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1])),
			typeof(Func<, , >).MakeGenericType(types[0], types[1], types[2])
		});
		SequenceEqual = GetMethod("SequenceEqual", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		SingleWithoutPredicate = GetMethod("Single", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		SingleWithPredicate = GetMethod("Single", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		SingleOrDefaultWithoutPredicate = GetMethod("SingleOrDefault", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		SingleOrDefaultWithPredicate = GetMethod("SingleOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		Skip = GetMethod("Skip", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		SkipWhile = GetMethod("SkipWhile", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		ToArray = GetMethod("ToArray", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		ToList = GetMethod("ToList", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		Take = GetMethod("Take", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		TakeWhile = GetMethod("TakeWhile", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		ThenBy = GetMethod("ThenBy", 2, (Type[] types) => new Type[2]
		{
			typeof(IOrderedEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		ThenByDescending = GetMethod("ThenByDescending", 2, (Type[] types) => new Type[2]
		{
			typeof(IOrderedEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], types[1])
		});
		Union = GetMethod("Union", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		Where = GetMethod("Where", 1, (Type[] types) => new Type[2]
		{
			typeof(IEnumerable<>).MakeGenericType(types[0]),
			typeof(Func<, >).MakeGenericType(types[0], typeof(bool))
		});
		Type[] obj = new Type[10]
		{
			typeof(int),
			typeof(int?),
			typeof(long),
			typeof(long?),
			typeof(float),
			typeof(float?),
			typeof(double),
			typeof(double?),
			typeof(decimal),
			typeof(decimal?)
		};
		AverageWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
		AverageWithSelectorMethods = new Dictionary<Type, MethodInfo>();
		MaxWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
		MaxWithSelectorMethods = new Dictionary<Type, MethodInfo>();
		MinWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
		MinWithSelectorMethods = new Dictionary<Type, MethodInfo>();
		SumWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
		SumWithSelectorMethods = new Dictionary<Type, MethodInfo>();
		Type[] array = obj;
		foreach (Type type in array)
		{
			AverageWithoutSelectorMethods[type] = GetMethod("Average", 0, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(type) });
			AverageWithSelectorMethods[type] = GetMethod("Average", 1, (Type[] types) => new Type[2]
			{
				typeof(IEnumerable<>).MakeGenericType(types[0]),
				typeof(Func<, >).MakeGenericType(types[0], type)
			});
			MaxWithoutSelectorMethods[type] = GetMethod("Max", 0, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(type) });
			MaxWithSelectorMethods[type] = GetMethod("Max", 1, (Type[] types) => new Type[2]
			{
				typeof(IEnumerable<>).MakeGenericType(types[0]),
				typeof(Func<, >).MakeGenericType(types[0], type)
			});
			MinWithoutSelectorMethods[type] = GetMethod("Min", 0, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(type) });
			MinWithSelectorMethods[type] = GetMethod("Min", 1, (Type[] types) => new Type[2]
			{
				typeof(IEnumerable<>).MakeGenericType(types[0]),
				typeof(Func<, >).MakeGenericType(types[0], type)
			});
			SumWithoutSelectorMethods[type] = GetMethod("Sum", 0, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(type) });
			SumWithSelectorMethods[type] = GetMethod("Sum", 1, (Type[] types) => new Type[2]
			{
				typeof(IEnumerable<>).MakeGenericType(types[0]),
				typeof(Func<, >).MakeGenericType(types[0], type)
			});
		}
		MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
		{
			return queryableMethodGroups[name].Single((MethodInfo mi) => ((genericParameterCount == 0 && !mi.IsGenericMethod) || (mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount)) && (from e in mi.GetParameters()
				select e.ParameterType).SequenceEqual(parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : Array.Empty<Type>())));
		}
	}
}
