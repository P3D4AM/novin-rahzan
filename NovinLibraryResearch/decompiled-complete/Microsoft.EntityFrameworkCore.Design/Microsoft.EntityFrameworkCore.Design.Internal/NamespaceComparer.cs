using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public class NamespaceComparer : IComparer<string>
{
	public virtual int Compare(string x, string y)
	{
		bool flag = x != null && (string.Equals(x, "System") || x.StartsWith("System."));
		bool flag2 = y != null && (string.Equals(y, "System") || y.StartsWith("System."));
		if (!flag || flag2)
		{
			if (!(!flag && flag2))
			{
				return string.CompareOrdinal(x, y);
			}
			return 1;
		}
		return -1;
	}
}
