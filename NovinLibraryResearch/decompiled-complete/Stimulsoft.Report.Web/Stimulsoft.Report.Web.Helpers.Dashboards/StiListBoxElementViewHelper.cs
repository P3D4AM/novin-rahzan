using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiListBoxElementViewHelper
{
	public static List<Hashtable> GetElementItems(IStiListBoxElement listBoxElement)
	{
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(listBoxElement);
		if (orCreate == null)
		{
			return null;
		}
		if (orCreate == null || orCreate.Rows.Count == 0)
		{
			return null;
		}
		int num = GetNameMeterIndex(orCreate);
		int num2 = GetKeyMeterIndex(orCreate);
		if (num == -1 && num2 == 1)
		{
			return null;
		}
		if (num2 == -1)
		{
			num2 = num;
		}
		if (num == -1)
		{
			num = num2;
		}
		List<Hashtable> list = new List<Hashtable>();
		foreach (object[] row in orCreate.Rows)
		{
			object obj = row[num2];
			if (obj != null)
			{
				object obj2 = row[num];
				if (listBoxElement.ShowBlanks || !StiDataFiltersHelper.IsBlankData(row[num]))
				{
					list.Add(ListBoxItem(StiDashboardElementViewHelper.Format(listBoxElement, row[num]), row[num2]));
				}
			}
		}
		return list;
	}

	public static Hashtable ListBoxItem(string label, object value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["label"] = label;
		hashtable["value"] = StiDataFiltersHelper.ToFilterString(value);
		return hashtable;
	}

	public static Hashtable GetSettings(IStiListBoxElement listBoxElement)
	{
		Hashtable controlElementSettings = StiDashboardElementViewHelper.GetControlElementSettings(listBoxElement);
		controlElementSettings["itemHeight"] = StiElementConsts.ListBox.ItemHeight;
		return controlElementSettings;
	}

	protected static int GetNameMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f.GetType().Name == "StiNameListBoxMeter");
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	protected static int GetKeyMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f.GetType().Name == "StiKeyListBoxMeter");
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	public static string GetColumnPath(IStiListBoxElement listBoxElement)
	{
		if (listBoxElement.GetKeyMeter() != null)
		{
			return listBoxElement.GetKeyMeter()?.Expression;
		}
		if (listBoxElement.GetNameMeter() != null)
		{
			return listBoxElement.GetNameMeter()?.Expression;
		}
		return null;
	}
}
