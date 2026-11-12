using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public static class QueryableMethods
{
	public static MethodInfo All { get; }

	public static MethodInfo AnyWithoutPredicate { get; }

	public static MethodInfo AnyWithPredicate { get; }

	public static MethodInfo AsQueryable { get; }

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

	public static MethodInfo SelectManyWithoutCollectionSelector { get; }

	public static MethodInfo SelectManyWithCollectionSelector { get; }

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

	public static MethodInfo Union { get; }

	public static MethodInfo Where { get; }

	private static Dictionary<Type, MethodInfo> AverageWithoutSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> AverageWithSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> SumWithoutSelectorMethods { get; }

	private static Dictionary<Type, MethodInfo> SumWithSelectorMethods { get; }

	public static bool IsAverageWithoutSelector([JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		return AverageWithoutSelectorMethods.Values.Contains(methodInfo);
	}

	public static bool IsAverageWithSelector([JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		if (methodInfo.IsGenericMethod)
		{
			return AverageWithSelectorMethods.Values.Contains(methodInfo.GetGenericMethodDefinition());
		}
		return false;
	}

	public static bool IsSumWithoutSelector([JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		return SumWithoutSelectorMethods.Values.Contains(methodInfo);
	}

	public static bool IsSumWithSelector([JetBrains.Annotations.NotNull] MethodInfo methodInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(methodInfo, "methodInfo");
		if (methodInfo.IsGenericMethod)
		{
			return SumWithSelectorMethods.Values.Contains(methodInfo.GetGenericMethodDefinition());
		}
		return false;
	}

	public static MethodInfo GetAverageWithoutSelector([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return AverageWithoutSelectorMethods[type];
	}

	public static MethodInfo GetAverageWithSelector([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return AverageWithSelectorMethods[type];
	}

	public static MethodInfo GetSumWithoutSelector([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return SumWithoutSelectorMethods[type];
	}

	public static MethodInfo GetSumWithSelector([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return SumWithSelectorMethods[type];
	}

	static QueryableMethods()
	{
		Dictionary<string, List<MethodInfo>> queryableMethodGroups = (from mi in typeof(Queryable).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
			group mi by mi.Name).ToDictionary((IGrouping<string, MethodInfo> e) => e.Key, (IGrouping<string, MethodInfo> l) => l.ToList());
		All = GetMethod("All", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		AnyWithoutPredicate = GetMethod("Any", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		AnyWithPredicate = GetMethod("Any", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		AsQueryable = GetMethod("AsQueryable", 1, (Type[] types) => new Type[1] { typeof(IEnumerable<>).MakeGenericType(types[0]) });
		Cast = GetMethod("Cast", 1, (Type[] types) => new Type[1] { typeof(IQueryable) });
		Concat = GetMethod("Concat", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		Contains = GetMethod("Contains", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			types[0]
		});
		CountWithoutPredicate = GetMethod("Count", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		CountWithPredicate = GetMethod("Count", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		DefaultIfEmptyWithoutArgument = GetMethod("DefaultIfEmpty", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		DefaultIfEmptyWithArgument = GetMethod("DefaultIfEmpty", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			types[0]
		});
		Distinct = GetMethod("Distinct", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		ElementAt = GetMethod("ElementAt", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		ElementAtOrDefault = GetMethod("ElementAtOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		Except = GetMethod("Except", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		FirstWithoutPredicate = GetMethod("First", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		FirstWithPredicate = GetMethod("First", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		FirstOrDefaultWithoutPredicate = GetMethod("FirstOrDefault", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		FirstOrDefaultWithPredicate = GetMethod("FirstOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		GroupByWithKeySelector = GetMethod("GroupBy", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		GroupByWithKeyElementSelector = GetMethod("GroupBy", 3, (Type[] types) => new Type[3]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[2]))
		});
		GroupByWithKeyElementResultSelector = GetMethod("GroupBy", 4, (Type[] types) => new Type[4]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[2])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, , >).MakeGenericType(types[1], typeof(IEnumerable<>).MakeGenericType(types[2]), types[3]))
		});
		GroupByWithKeyResultSelector = GetMethod("GroupBy", 3, (Type[] types) => new Type[3]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, , >).MakeGenericType(types[1], typeof(IEnumerable<>).MakeGenericType(types[0]), types[2]))
		});
		GroupJoin = GetMethod("GroupJoin", 4, (Type[] types) => new Type[5]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[1]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[2])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[1], types[2])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, , >).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1]), types[3]))
		});
		Intersect = GetMethod("Intersect", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		Join = GetMethod("Join", 4, (Type[] types) => new Type[5]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[1]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[2])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[1], types[2])),
			typeof(Expression<>).MakeGenericType(typeof(Func<, , >).MakeGenericType(types[0], types[1], types[3]))
		});
		LastWithoutPredicate = GetMethod("Last", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		LastWithPredicate = GetMethod("Last", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		LastOrDefaultWithoutPredicate = GetMethod("LastOrDefault", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		LastOrDefaultWithPredicate = GetMethod("LastOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		LongCountWithoutPredicate = GetMethod("LongCount", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		LongCountWithPredicate = GetMethod("LongCount", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		MaxWithoutSelector = GetMethod("Max", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		MaxWithSelector = GetMethod("Max", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		MinWithoutSelector = GetMethod("Min", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		MinWithSelector = GetMethod("Min", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		OfType = GetMethod("OfType", 1, (Type[] types) => new Type[1] { typeof(IQueryable) });
		OrderBy = GetMethod("OrderBy", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		OrderByDescending = GetMethod("OrderByDescending", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		Reverse = GetMethod("Reverse", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		Select = GetMethod("Select", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		SelectManyWithoutCollectionSelector = GetMethod("SelectMany", 2, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1])))
		});
		SelectManyWithCollectionSelector = GetMethod("SelectMany", 3, (Type[] types) => new Type[3]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1]))),
			typeof(Expression<>).MakeGenericType(typeof(Func<, , >).MakeGenericType(types[0], types[1], types[2]))
		});
		SingleWithoutPredicate = GetMethod("Single", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		SingleWithPredicate = GetMethod("Single", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		SingleOrDefaultWithoutPredicate = GetMethod("SingleOrDefault", 1, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(types[0]) });
		SingleOrDefaultWithPredicate = GetMethod("SingleOrDefault", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		Skip = GetMethod("Skip", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		SkipWhile = GetMethod("SkipWhile", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		Take = GetMethod("Take", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(int)
		});
		TakeWhile = GetMethod("TakeWhile", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
		});
		ThenBy = GetMethod("ThenBy", 2, (Type[] types) => new Type[2]
		{
			typeof(IOrderedQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		ThenByDescending = GetMethod("ThenByDescending", 2, (Type[] types) => new Type[2]
		{
			typeof(IOrderedQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], types[1]))
		});
		Union = GetMethod("Union", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(IEnumerable<>).MakeGenericType(types[0])
		});
		Where = GetMethod("Where", 1, (Type[] types) => new Type[2]
		{
			typeof(IQueryable<>).MakeGenericType(types[0]),
			typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], typeof(bool)))
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
		SumWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
		SumWithSelectorMethods = new Dictionary<Type, MethodInfo>();
		Type[] array = obj;
		foreach (Type type in array)
		{
			AverageWithoutSelectorMethods[type] = GetMethod("Average", 0, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(type) });
			AverageWithSelectorMethods[type] = GetMethod("Average", 1, (Type[] types) => new Type[2]
			{
				typeof(IQueryable<>).MakeGenericType(types[0]),
				typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], type))
			});
			SumWithoutSelectorMethods[type] = GetMethod("Sum", 0, (Type[] types) => new Type[1] { typeof(IQueryable<>).MakeGenericType(type) });
			SumWithSelectorMethods[type] = GetMethod("Sum", 1, (Type[] types) => new Type[2]
			{
				typeof(IQueryable<>).MakeGenericType(types[0]),
				typeof(Expression<>).MakeGenericType(typeof(Func<, >).MakeGenericType(types[0], type))
			});
		}
		MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
		{
			return queryableMethodGroups[name].Single((MethodInfo mi) => ((genericParameterCount == 0 && !mi.IsGenericMethod) || (mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount)) && (from e in mi.GetParameters()
				select e.ParameterType).SequenceEqual(parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : Array.Empty<Type>())));
		}
	}
}
