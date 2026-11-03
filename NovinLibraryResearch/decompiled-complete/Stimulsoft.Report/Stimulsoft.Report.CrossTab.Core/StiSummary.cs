using System.Collections;
using System.Collections.Generic;

namespace Stimulsoft.Report.CrossTab.Core;

public class StiSummary
{
	public ArrayList[] Sums { get; }

	public object[] HyperlinkValues { get; set; }

	public object[] TagValues { get; set; }

	public object[] ToolTipValues { get; set; }

	public Dictionary<string, object>[] DrillDownParameters { get; set; }

	public StiSummary(int level)
	{
		Sums = new ArrayList[level];
		HyperlinkValues = new object[level];
		TagValues = new object[level];
		ToolTipValues = new object[level];
		for (int i = 0; i < level; i++)
		{
			Sums[i] = new ArrayList();
		}
	}
}
