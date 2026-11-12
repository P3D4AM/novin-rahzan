using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Data.Comparers;

public class StiDataRowComparer : IComparer<DataRow>
{
	public int Compare(DataRow x, DataRow y)
	{
		if (x == null || y == null)
		{
			return 0;
		}
		if (x.ItemArray.Length != y.ItemArray.Length)
		{
			return -1;
		}
		for (int i = 0; i < x.ItemArray.Length; i++)
		{
			int num = StiObjectComparer.Compare(x, y);
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}
}
