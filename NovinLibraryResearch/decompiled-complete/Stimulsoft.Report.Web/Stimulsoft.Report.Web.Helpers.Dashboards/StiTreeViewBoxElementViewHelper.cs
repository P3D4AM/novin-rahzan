using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiTreeViewBoxElementViewHelper
{
	public static List<Hashtable> GetElementItems(IStiTreeViewBoxElement treeViewBoxElement)
	{
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(treeViewBoxElement);
		if (orCreate == null)
		{
			return null;
		}
		if (orCreate == null || orCreate.Rows.Count == 0)
		{
			return null;
		}
		Hashtable hashtable = TreeViewBoxItem(treeViewBoxElement);
		foreach (object[] row in orCreate.Rows)
		{
			int num = 0;
			Hashtable hashtable2 = hashtable;
			object[] array = row;
			foreach (object data in array)
			{
				bool flag = data == null || data == DBNull.Value;
				bool flag2 = data is string && string.IsNullOrEmpty(data as string);
				bool flag3 = flag || flag2;
				if (treeViewBoxElement.ShowBlanks || !flag3)
				{
					Hashtable hashtable3 = ((hashtable2["items"] is List<Hashtable> source) ? source.FirstOrDefault((Hashtable n) => object.Equals(n["key"], StiDataFiltersHelper.ToFilterString(data))) : null);
					if (hashtable3 == null)
					{
						hashtable3 = TreeViewBoxItem(treeViewBoxElement, data, orCreate.Meters[num]);
						if (hashtable2["items"] == null)
						{
							hashtable2["items"] = new List<Hashtable>();
						}
						(hashtable2["items"] as List<Hashtable>).Add(hashtable3);
					}
					hashtable2 = hashtable3;
				}
				num++;
			}
		}
		return hashtable["items"] as List<Hashtable>;
	}

	public static Hashtable TreeViewBoxItem(IStiTreeViewBoxElement treeViewBoxElement, object key = null, IStiMeter meter = null)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["key"] = StiDataFiltersHelper.ToFilterString(key);
		hashtable["text"] = StiDashboardElementViewHelper.Format(treeViewBoxElement, key);
		hashtable["columnPath"] = meter?.Expression;
		hashtable["meterKey"] = meter?.Key;
		hashtable["items"] = null;
		return hashtable;
	}

	public static Hashtable GetSettings(IStiTreeViewBoxElement treeViewBoxElement)
	{
		Hashtable controlElementSettings = StiDashboardElementViewHelper.GetControlElementSettings(treeViewBoxElement);
		controlElementSettings["itemHeight"] = StiElementConsts.TreeView.ItemHeight;
		return controlElementSettings;
	}

	public static string GetColumnPath(IStiTreeViewBoxElement treeViewBoxElement)
	{
		return treeViewBoxElement.FetchAllMeters()?.LastOrDefault()?.Expression;
	}

	public static string GetMeterKey(IStiTreeViewBoxElement treeViewBoxElement)
	{
		return treeViewBoxElement.FetchAllMeters()?.LastOrDefault()?.Key;
	}
}
