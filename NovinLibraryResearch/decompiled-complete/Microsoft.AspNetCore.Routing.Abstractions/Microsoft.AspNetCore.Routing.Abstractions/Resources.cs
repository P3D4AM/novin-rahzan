using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.AspNetCore.Routing.Abstractions;

internal static class Resources
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.AspNetCore.Routing.Abstractions.Resources", typeof(Resources).GetTypeInfo().Assembly);

	internal static string RouteValueDictionary_DuplicateKey => GetString("RouteValueDictionary_DuplicateKey");

	internal static string RouteValueDictionary_DuplicatePropertyName => GetString("RouteValueDictionary_DuplicatePropertyName");

	internal static string FormatRouteValueDictionary_DuplicateKey(object p0, object p1)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("RouteValueDictionary_DuplicateKey"), p0, p1);
	}

	internal static string FormatRouteValueDictionary_DuplicatePropertyName(object p0, object p1, object p2, object p3)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("RouteValueDictionary_DuplicatePropertyName"), p0, p1, p2, p3);
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		if (formatterNames != null)
		{
			for (int i = 0; i < formatterNames.Length; i++)
			{
				text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
			}
		}
		return text;
	}
}
