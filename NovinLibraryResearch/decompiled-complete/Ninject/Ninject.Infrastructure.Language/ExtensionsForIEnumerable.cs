using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Ninject.Infrastructure.Language;

internal static class ExtensionsForIEnumerable
{
	private static readonly MethodInfo Cast = typeof(Enumerable).GetMethod("Cast");

	private static readonly MethodInfo ToArray = typeof(Enumerable).GetMethod("ToArray");

	private static readonly MethodInfo ToList = typeof(Enumerable).GetMethod("ToList");

	public static IEnumerable CastSlow(this IEnumerable series, Type elementType)
	{
		MethodInfo methodInfo = Cast.MakeGenericMethod(elementType);
		object[] parameters = new IEnumerable[1] { series };
		return methodInfo.Invoke(null, parameters) as IEnumerable;
	}

	public static Array ToArraySlow(this IEnumerable series, Type elementType)
	{
		MethodInfo methodInfo = ToArray.MakeGenericMethod(elementType);
		object[] parameters = new IEnumerable[1] { series };
		return methodInfo.Invoke(null, parameters) as Array;
	}

	public static IList ToListSlow(this IEnumerable series, Type elementType)
	{
		MethodInfo methodInfo = ToList.MakeGenericMethod(elementType);
		object[] parameters = new IEnumerable[1] { series };
		return methodInfo.Invoke(null, parameters) as IList;
	}
}
