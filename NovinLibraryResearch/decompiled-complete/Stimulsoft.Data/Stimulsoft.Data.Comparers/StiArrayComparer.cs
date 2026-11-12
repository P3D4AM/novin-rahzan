using System.Collections.Generic;

namespace Stimulsoft.Data.Comparers;

public class StiArrayComparer : IComparer<object[]>
{
	public int Compare(object[] x, object[] y)
	{
		if (x == null || y == null)
		{
			return 0;
		}
		if (x.Length != y.Length)
		{
			return -1;
		}
		for (int i = 0; i < x.Length; i++)
		{
			int num = StiObjectComparer.Compare(x[i], y[i]);
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}
}
