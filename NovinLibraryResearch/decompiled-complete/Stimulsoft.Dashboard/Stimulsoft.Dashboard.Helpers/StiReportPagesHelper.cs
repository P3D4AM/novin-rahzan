using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Dashboard.Helpers;

public static class StiReportPagesHelper
{
	private static object lockObject = new object();

	public static List<StiPage> FetchAllPages(StiReport report)
	{
		return (from p in report?.Pages.OfType<StiPage>()
			where p.IsEnabled
			select p).Except(FetchAllNestedPages(report)).ToList();
	}

	public static List<StiPage> FetchAllNestedPages(StiReport report)
	{
		List<StiPage> list = report?.Pages.OfType<StiPage>().ToList();
		List<StiPage> nestedPages = new List<StiPage>();
		foreach (StiPage item in list)
		{
			IEnumerable<StiComponent> enumerable = from c in item.GetComponents().ToList()
				where c is StiElement
				select c;
			foreach (StiElement item2 in enumerable)
			{
				List<StiPage> nestedPages2 = item2.GetNestedPages();
				if (nestedPages2 != null)
				{
					nestedPages.AddRange(nestedPages2);
				}
			}
		}
		lock (lockObject)
		{
			StiRenderProviderV2.PrepareSubReportsAndDrillDownPages(report);
			List<StiPage> list2 = (from p in report.Pages.ToList()
				where p.IsPage
				select p).ToList();
			list2.Where((StiPage p) => p.Skip && !nestedPages.Contains(p)).ToList().ForEach(delegate(StiPage p)
			{
				nestedPages.Add(p);
			});
			list2.ForEach(delegate(StiPage p)
			{
				p.Skip = false;
			});
		}
		return nestedPages;
	}
}
