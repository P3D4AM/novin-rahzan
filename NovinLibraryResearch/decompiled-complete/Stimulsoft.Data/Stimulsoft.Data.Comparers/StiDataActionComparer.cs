using System.Collections.Generic;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Comparers;

public class StiDataActionComparer : IComparer<StiDataActionRule>
{
	public int Compare(StiDataActionRule x, StiDataActionRule y)
	{
		if (x == null || y == null)
		{
			return 0;
		}
		if (x.Type > y.Type)
		{
			return 1;
		}
		if (x.Type < y.Type)
		{
			return -1;
		}
		return 0;
	}
}
