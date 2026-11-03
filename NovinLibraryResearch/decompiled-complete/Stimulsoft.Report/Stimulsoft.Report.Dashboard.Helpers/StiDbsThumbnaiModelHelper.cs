using System.Collections.Generic;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard.Helpers;

public static class StiDbsThumbnaiModelHelper
{
	private static object lockPage = new object();

	private static List<StiPage> cache = new List<StiPage>();

	public static void Add(StiPage page)
	{
		if (page == null)
		{
			return;
		}
		lock (lockPage)
		{
			if (!cache.Contains(page))
			{
				cache.Add(page);
			}
		}
	}

	public static void Remove(StiPage page)
	{
		if (page == null)
		{
			return;
		}
		lock (lockPage)
		{
			if (cache.Contains(page))
			{
				cache.Remove(page);
			}
		}
	}

	public static bool AllowPage(StiPage page)
	{
		if (page == null)
		{
			return false;
		}
		lock (lockPage)
		{
			return cache.Contains(page);
		}
	}
}
