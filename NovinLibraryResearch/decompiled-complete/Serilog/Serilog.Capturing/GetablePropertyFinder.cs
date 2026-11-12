using System;
using System.Collections.Generic;
using System.Reflection;

namespace Serilog.Capturing;

internal static class GetablePropertyFinder
{
	internal static IEnumerable<PropertyInfo> GetPropertiesRecursive(this Type type)
	{
		HashSet<string> seenNames = new HashSet<string>();
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanRead && !seenNames.Contains(propertyInfo.Name) && (!(propertyInfo.Name == "Item") || propertyInfo.GetIndexParameters().Length == 0))
			{
				seenNames.Add(propertyInfo.Name);
				yield return propertyInfo;
			}
		}
	}
}
