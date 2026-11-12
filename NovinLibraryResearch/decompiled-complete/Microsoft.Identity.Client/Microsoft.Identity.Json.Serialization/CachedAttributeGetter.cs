using System;
using Microsoft.Identity.Json.Utilities;

namespace Microsoft.Identity.Json.Serialization;

internal static class CachedAttributeGetter<T> where T : Attribute
{
	private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(JsonTypeReflector.GetAttribute<T>);

	public static T GetAttribute(object type)
	{
		return TypeAttributeCache.Get(type);
	}
}
