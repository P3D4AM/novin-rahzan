using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.AspNetCore.Routing;

public class RouteValueEqualityComparer : IEqualityComparer<object>
{
	public new bool Equals(object x, object y)
	{
		string text = (x as string) ?? Convert.ToString(x, CultureInfo.InvariantCulture);
		string text2 = (y as string) ?? Convert.ToString(y, CultureInfo.InvariantCulture);
		if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
		{
			return true;
		}
		return string.Equals(text, text2, StringComparison.OrdinalIgnoreCase);
	}

	public int GetHashCode(object obj)
	{
		string text = (obj as string) ?? Convert.ToString(obj, CultureInfo.InvariantCulture);
		if (string.IsNullOrEmpty(text))
		{
			return StringComparer.OrdinalIgnoreCase.GetHashCode(string.Empty);
		}
		return StringComparer.OrdinalIgnoreCase.GetHashCode(text);
	}
}
