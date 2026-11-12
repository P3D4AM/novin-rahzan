using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiComboBoxElementViewHelper
{
	public static List<Hashtable> GetElementItems(IStiComboBoxElement comboBoxElement)
	{
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(comboBoxElement);
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
				object data = row[num];
				if (comboBoxElement.ShowBlanks || !StiDataFiltersHelper.IsBlankData(data))
				{
					list.Add(ComboBoxItem(StiDashboardElementViewHelper.Format(comboBoxElement, row[num]), row[num2]));
				}
			}
		}
		return list;
	}

	public static Hashtable ComboBoxItem(string label, object value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["label"] = label;
		hashtable["value"] = StiDataFiltersHelper.ToFilterString(value);
		return hashtable;
	}

	public static Hashtable GetSettings(IStiComboBoxElement comboBoxElement)
	{
		Hashtable controlElementSettings = StiDashboardElementViewHelper.GetControlElementSettings(comboBoxElement);
		controlElementSettings["itemHeight"] = StiElementConsts.ComboBox.ItemHeight;
		return controlElementSettings;
	}

	protected static int GetNameMeterIndex(StiDataTable table)
	{
		if (table == null)
		{
			return -1;
		}
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f.GetType().Name == "StiNameComboBoxMeter");
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
		IStiMeter stiMeter = table.Meters.FirstOrDefault((IStiMeter f) => f.GetType().Name == "StiKeyComboBoxMeter");
		if (stiMeter == null)
		{
			return -1;
		}
		return table.Meters.IndexOf(stiMeter);
	}

	public static string GetColumnPath(IStiComboBoxElement comboBoxElement)
	{
		if (comboBoxElement.GetKeyMeter() != null)
		{
			return comboBoxElement.GetKeyMeter()?.Expression;
		}
		if (comboBoxElement.GetNameMeter() != null)
		{
			return comboBoxElement.GetNameMeter()?.Expression;
		}
		return null;
	}
}
