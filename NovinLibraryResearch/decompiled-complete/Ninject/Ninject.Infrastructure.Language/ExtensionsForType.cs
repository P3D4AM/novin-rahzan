using System;
using System.Collections.Generic;

namespace Ninject.Infrastructure.Language;

public static class ExtensionsForType
{
	public static IEnumerable<Type> GetAllBaseTypes(this Type type)
	{
		while (type != null)
		{
			yield return type;
			type = type.BaseType;
		}
	}
}
