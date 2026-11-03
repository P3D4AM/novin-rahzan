using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Report.Chart;

public static class StiBoxAndWhiskerHelper
{
	public static void CheckArgument(StiChart chart)
	{
		List<StiSeries> list = (from s in chart.Series.ToList()
			where s is StiBoxAndWhiskerSeries
			select s).ToList();
		if (list.Count > 0)
		{
			object[] array = new object[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = list[i].CoreTitle;
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Arguments = array;
				list[j].OriginalArguments = array;
			}
		}
	}
}
