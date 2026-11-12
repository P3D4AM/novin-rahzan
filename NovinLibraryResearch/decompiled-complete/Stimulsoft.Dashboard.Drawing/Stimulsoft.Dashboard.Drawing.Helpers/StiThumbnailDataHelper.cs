using System.Collections.Generic;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Helpers;

internal static class StiThumbnailDataHelper
{
	public static StiDataTable Get(IStiElement element)
	{
		StiDataTable stiDataTable = new StiDataTable();
		List<IStiMeter> meters = element.GetMeters();
		if (meters.Count > 0)
		{
			List<object> list = new List<object>();
			for (int i = 0; i < meters.Count; i++)
			{
				list.Add(i + 1);
			}
			foreach (IStiMeter item in meters)
			{
				stiDataTable.Meters.Add(item);
				stiDataTable.Rows.Add(list.ToArray());
			}
		}
		return stiDataTable;
	}
}
