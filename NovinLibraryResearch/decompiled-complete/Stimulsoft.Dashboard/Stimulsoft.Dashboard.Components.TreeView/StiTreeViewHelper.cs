using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.TreeView;

public class StiTreeViewHelper
{
	public static List<StiTreeItem> FetchItems(StiDataTable dataTable, bool showBlanks, int maxRows = 0)
	{
		StiTreeItem stiTreeItem = new StiTreeItem();
		int num = 0;
		foreach (object[] row in dataTable.Rows)
		{
			int num2 = 0;
			StiTreeItem stiTreeItem2 = stiTreeItem;
			object[] array = row;
			foreach (object data in array)
			{
				bool flag = data == null || data == DBNull.Value;
				bool flag2 = data is string && string.IsNullOrEmpty(data as string);
				bool flag3 = flag || flag2;
				if (showBlanks || !flag3)
				{
					StiTreeItem stiTreeItem3 = stiTreeItem2.Items?.FirstOrDefault((StiTreeItem n) => object.Equals(n.Key, data));
					if (stiTreeItem3 == null)
					{
						stiTreeItem3 = new StiTreeItem(data, dataTable.Meters[num2]);
						if (stiTreeItem2.Items == null)
						{
							stiTreeItem2.Items = new List<StiTreeItem>();
						}
						stiTreeItem2.Items.Add(stiTreeItem3);
					}
					stiTreeItem2 = stiTreeItem3;
				}
				num2++;
			}
			num++;
			if (num >= maxRows && maxRows != 0)
			{
				break;
			}
		}
		return stiTreeItem.Items;
	}

	public static List<StiDataFilterRule> FetchDefaultUserFilters(StiTreeViewBoxElement treeViewBoxElement)
	{
		if (treeViewBoxElement.ShowAllValue)
		{
			return null;
		}
		if (treeViewBoxElement.SelectionMode == StiItemSelectionMode.Multi)
		{
			return null;
		}
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(treeViewBoxElement);
		List<StiTreeItem> list = FetchItems(orCreate, treeViewBoxElement.ShowBlanks);
		if (list == null)
		{
			return null;
		}
		StiTreeItem stiTreeItem = list.FirstOrDefault();
		if (stiTreeItem == null)
		{
			return null;
		}
		IStiMeter meter = stiTreeItem.Meter;
		string expression = meter.Expression;
		return StiDataFilterCreator.CreateEqualBasedOnValue(stiTreeItem.Key, expression, treeViewBoxElement).ToList();
	}

	public static List<StiDataFilterRule> FetchDefaultUserFilters(StiTreeViewElement treeViewElement)
	{
		if (treeViewElement.ShowAllValue)
		{
			return null;
		}
		if (treeViewElement.SelectionMode == StiItemSelectionMode.Multi)
		{
			return null;
		}
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(treeViewElement);
		List<StiTreeItem> list = FetchItems(orCreate, treeViewElement.ShowBlanks);
		if (list == null)
		{
			return null;
		}
		StiTreeItem stiTreeItem = list.FirstOrDefault();
		if (stiTreeItem == null)
		{
			return null;
		}
		IStiMeter meter = stiTreeItem.Meter;
		string expression = meter.Expression;
		return StiDataFilterCreator.CreateEqualBasedOnValue(stiTreeItem.Key, expression, treeViewElement).ToList();
	}

	public static string Format(StiTreeViewBoxElement treeViewBoxElement, object value)
	{
		if (value is DateTime && !(treeViewBoxElement.TextFormat is StiDateFormatService))
		{
			return ((DateTime)value).ToShortDateString();
		}
		if (StiExtValueConverter.IsEligable(value, treeViewBoxElement?.Report?.Culture))
		{
			return StiExtValueConverter.Convert(value, treeViewBoxElement?.Report?.Culture);
		}
		return treeViewBoxElement.TextFormat.Format(value);
	}

	public static string Format(StiTreeViewElement treeViewElement, object value)
	{
		if (value is DateTime && !(treeViewElement.TextFormat is StiDateFormatService))
		{
			return ((DateTime)value).ToShortDateString();
		}
		if (StiExtValueConverter.IsEligable(value, treeViewElement?.Report?.Culture))
		{
			return StiExtValueConverter.Convert(value, treeViewElement?.Report?.Culture);
		}
		return treeViewElement.TextFormat.Format(value);
	}
}
