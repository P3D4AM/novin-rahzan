using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.SqlServer.Management.Dmf;

internal class PropertyInfoNameComparer : IComparer<PropertyInfo>
{
	public int Compare(PropertyInfo x, PropertyInfo y)
	{
		return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
	}
}
