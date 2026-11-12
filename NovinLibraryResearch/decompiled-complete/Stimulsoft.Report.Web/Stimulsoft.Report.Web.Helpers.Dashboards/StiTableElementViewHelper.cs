using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiTableElementViewHelper
{
	public static List<object> GetTableData(IStiTableElement tableElement, double scaleX = 1.0)
	{
		List<object> list = new List<object>();
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(tableElement);
		if (orCreate != null)
		{
			List<object> list2 = new List<object>();
			List<IStiMeter> list3 = (from m in orCreate.Meters.AsEnumerable()
				where m is IStiTableColumn
				select m).ToList();
			int num = 0;
			foreach (IStiMeter item in list3)
			{
				if (Convert.ToBoolean(StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard", "Components.Table.StiTableColumnVisibilityHelper", "GetVisible", new object[2]
				{
					item as IStiTableColumn,
					tableElement.Report
				})))
				{
					StiDataColumn stiDataColumn = (StiDataColumn)StiDataExpressionHelper.GetDataColumnFromExpression((IStiDashboard)tableElement.Page, item.Expression);
					Hashtable hashtable = new Hashtable();
					hashtable["columnIndex"] = num;
					hashtable["key"] = item.Key;
					hashtable["label"] = StiLabelHelper.GetLabel(item);
					hashtable["labelCorrect"] = StiNameValidator.CorrectName(StiLabelHelper.GetLabel(item));
					hashtable["path"] = ((stiDataColumn != null) ? stiDataColumn.GetColumnPath() : string.Empty);
					hashtable["type"] = item.GetType().Name;
					hashtable["dataType"] = StiDataFiltersHelper.TypeToString((stiDataColumn != null) ? stiDataColumn.Type : typeof(string));
					hashtable["align"] = GetCellAlignment(item);
					hashtable["sortLabel"] = GetSortLabel(tableElement, item.Key);
					hashtable["filterLabel"] = GetFilterLabel(tableElement, item.Key, (stiDataColumn != null) ? stiDataColumn.GetColumnPath() : string.Empty);
					hashtable["cellForeColor"] = StiReportHelper.GetHtmlColor(GetCellForeColor(tableElement, item as IStiTableColumn));
					hashtable["cellForeColorInterlaced"] = StiReportHelper.GetHtmlColor(GetCellForeColor(tableElement, item as IStiTableColumn, isInterlacedForeColor: true));
					hashtable["interaction"] = StiDashboardElementViewHelper.GetDashboardInteraction(item);
					hashtable["showTotalSummary"] = (item as IStiTableColumn).ShowTotalSummary;
					hashtable["summaryType"] = (item as IStiTableColumn).SummaryType;
					hashtable["summaryAlignment"] = (item as IStiTableColumn).SummaryAlignment;
					if (item is IStiDimensionColumn)
					{
						hashtable["showHyperlink"] = ((IStiDimensionColumn)item).ShowHyperlink;
						hashtable["hyperlinkPattern"] = ((IStiDimensionColumn)item).HyperlinkPattern;
					}
					if (item is IStiTableColumnSize)
					{
						StiTableColumnSize size = ((IStiTableColumnSize)item).Size;
						hashtable["sizeWidth"] = size.Width;
						hashtable["sizeMaxWidth"] = size.MaxWidth;
						hashtable["sizeMinWidth"] = size.MinWidth;
						hashtable["sizeWordWrap"] = size.WordWrap;
					}
					list2.Add(hashtable);
				}
				num++;
			}
			list.Add(list2);
			if (StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard.Export", "Tools.StiTableElementExportTool", "RenderCellsForViewer", new object[2]
			{
				tableElement,
				((StiComponent)tableElement).Width * scaleX
			}) is List<object> collection)
			{
				list.AddRange(collection);
			}
		}
		return list;
	}

	public static List<object> GetTableHiddenData(IStiTableElement tableElement)
	{
		List<object> list = new List<object>();
		StiDataTable orCreate = StiElementDataCache.GetOrCreate(tableElement);
		if (orCreate != null)
		{
			List<object> list2 = new List<object>();
			List<object> list3 = new List<object>();
			List<IStiMeter> list4 = (from m in orCreate.Meters.AsEnumerable()
				where m is IStiTableColumn
				select m).ToList();
			int num = 0;
			foreach (IStiMeter item in list4)
			{
				if (!Convert.ToBoolean(StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard", "Components.Table.StiTableColumnVisibilityHelper", "GetVisible", new object[2]
				{
					item as IStiTableColumn,
					tableElement.Report
				})))
				{
					Hashtable hashtable = new Hashtable();
					hashtable["columnIndex"] = num;
					hashtable["label"] = StiLabelHelper.GetLabel(item);
					hashtable["labelCorrect"] = StiNameValidator.CorrectName(StiLabelHelper.GetLabel(item));
					hashtable["key"] = item.Key;
					list2.Add(hashtable);
					int num2 = 0;
					foreach (object[] row in orCreate.Rows)
					{
						if (num < row.Length)
						{
							if (list3.Count < orCreate.Rows.Count)
							{
								list3.Add(new List<object>());
							}
							Hashtable hashtable2 = new Hashtable();
							hashtable2["value"] = row[num]?.ToString();
							((List<object>)list3[num2]).Add(hashtable2);
						}
						num2++;
					}
				}
				num++;
			}
			list.Add(list2);
			list.AddRange(list3);
		}
		return list;
	}

	public static Hashtable GetTableSettings(IStiTableElement tableElement)
	{
		Hashtable hashtable = new Hashtable();
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(tableElement);
		if (tableStyle != null)
		{
			hashtable["tableBorderColor"] = StiReportHelper.GetHtmlColor(tableStyle.LineColor);
			hashtable["headerForeColor"] = StiReportHelper.GetHtmlColor(GetHeaderForeColor(tableElement));
			hashtable["headerFont"] = StiDashboardElementViewHelper.GetFontJson(tableElement.HeaderFont);
			hashtable["headerBackground"] = StiReportHelper.GetHtmlColor(tableStyle.HeaderBackColor);
			hashtable["headerMouseOverBackground"] = StiReportHelper.GetHtmlColor(tableStyle.HotHeaderBackColor);
			hashtable["headerSelectedBackground"] = StiReportHelper.GetHtmlColor(tableStyle.HotHeaderBackColor);
			hashtable["headerHeight"] = StiElementConsts.Table.GetHeight(tableElement.HeaderFont);
			hashtable["cellBackColor"] = StiReportHelper.GetHtmlColor(tableStyle.CellBackColor);
			hashtable["cellBackColorInterlaced"] = StiReportHelper.GetHtmlColor(tableStyle.AlternatingCellBackColor);
			hashtable["cellSelectedBackColor"] = StiReportHelper.GetHtmlColor(tableStyle.SelectedCellBackColor);
			hashtable["cellSelectedForeColor"] = StiReportHelper.GetHtmlColor(tableStyle.SelectedCellForeColor);
			hashtable["cellHeight"] = StiElementConsts.Table.GetHeight(tableElement.Font);
			hashtable["footerForeColor"] = StiReportHelper.GetHtmlColor(GetFooterForeColor(tableElement));
			hashtable["footerFont"] = StiDashboardElementViewHelper.GetFontJson(tableElement.FooterFont);
			hashtable["footerBackground"] = StiReportHelper.GetHtmlColor(tableStyle.FooterBackColor);
			hashtable["footerHeight"] = StiElementConsts.Table.GetHeight(tableElement.FooterFont);
		}
		hashtable["sizeMode"] = tableElement.SizeMode;
		hashtable["frozenColumns"] = tableElement.FrozenColumns;
		return hashtable;
	}

	private static Color GetCellForeColor(IStiTableElement tableElement, IStiTableColumn column, bool isInterlacedForeColor = false)
	{
		if (column != null && column is IStiForeColor && (column as IStiForeColor).ForeColor != Color.Transparent)
		{
			return (column as IStiForeColor).ForeColor;
		}
		if (tableElement != null && tableElement is IStiForeColor)
		{
			Color foreColor = StiDashboardExpressionHelper.GetForeColor(tableElement, (tableElement as IStiForeColor).ForeColor);
			if (foreColor != Color.Transparent)
			{
				return foreColor;
			}
		}
		StiTableElementStyle tableStyle = StiDashboardStyleHelper.GetTableStyle(tableElement);
		if (tableStyle == null)
		{
			return StiElementConsts.Table.Font.Color;
		}
		if (!isInterlacedForeColor)
		{
			return tableStyle.CellForeColor;
		}
		return tableStyle.AlternatingCellForeColor;
	}

	private static Color GetHeaderForeColor(IStiTableElement tableElement)
	{
		if (tableElement.HeaderForeColor != Color.Transparent)
		{
			return tableElement.HeaderForeColor;
		}
		return StiDashboardStyleHelper.GetTableStyle(tableElement).HeaderForeColor;
	}

	private static Color GetFooterForeColor(IStiTableElement tableElement)
	{
		if (tableElement.FooterForeColor != Color.Transparent)
		{
			return tableElement.FooterForeColor;
		}
		return StiDashboardStyleHelper.GetTableStyle(tableElement).FooterForeColor;
	}

	private static string GetCellAlignment(object element)
	{
		if (element is IStiHorAlignment stiHorAlignment)
		{
			switch (stiHorAlignment.HorAlignment)
			{
			case StiHorAlignment.Left:
				return "left";
			case StiHorAlignment.Center:
				return "center";
			case StiHorAlignment.Right:
				return "right";
			}
		}
		return "center";
	}

	private static Hashtable GetSortLabel(IStiTableElement tableElement, string meterKey)
	{
		int num = 1;
		foreach (StiDataSortRule userSort in tableElement.UserSorts)
		{
			if (userSort.Key == meterKey)
			{
				Hashtable hashtable = new Hashtable();
				hashtable["direction"] = userSort.Direction;
				hashtable["sortIndex"] = num;
				return hashtable;
			}
			num++;
		}
		return null;
	}

	private static Hashtable GetFilterLabel(IStiTableElement tableElement, string meterKey, string columnPath)
	{
		foreach (StiDataFilterRule userFilter in tableElement.UserFilters)
		{
			if ((userFilter.Key != null) ? (userFilter.Key == meterKey) : (userFilter.Path == columnPath))
			{
				Hashtable hashtable = new Hashtable();
				hashtable["condition"] = userFilter.Condition;
				return hashtable;
			}
		}
		return null;
	}

	internal static Hashtable ChangeTableElementSelectColumns(StiReport report, StiRequestParams requestParams)
	{
		string @string = requestParams.GetString("tableElementName");
		Hashtable hiddenColumns = requestParams.GetHashtable("tableElementHiddenColumns");
		IStiTableElement tableElement = ((report != null && @string != null) ? (report.Pages.GetComponentByName(@string) as IStiTableElement) : null);
		if (tableElement != null)
		{
			List<IStiMeter> meters = tableElement.GetMeters();
			meters.ForEach(delegate(IStiMeter meter)
			{
				if (hiddenColumns[tableElement.Key + meter.Key] != null)
				{
					if (Convert.ToBoolean(hiddenColumns[tableElement.Key + meter.Key]))
					{
						((IStiTableColumn)meter).Visibility = StiTableColumnVisibility.False;
					}
					else
					{
						((IStiTableColumn)meter).Visibility = ((!string.IsNullOrEmpty(((IStiTableColumn)meter).VisibilityExpression)) ? StiTableColumnVisibility.Expression : StiTableColumnVisibility.True);
					}
				}
			});
			List<IStiElement> elements = (from e in ((IStiDashboard)tableElement.Page).GetElements(nested: true)
				where e.IsEnabled
				select e).ToList();
			int totalFixedHeight = 0;
			List<StiRangeBand> bands = null;
			StiReportHelper.CalculatePositionForEachBand(requestParams, elements, tableElement.Page, out bands, out totalFixedHeight, tableElement as StiComponent);
			return StiReportHelper.GetElementAttributes(tableElement.Page, tableElement, renderSingleElement: false, requestParams, bands, totalFixedHeight);
		}
		return null;
	}
}
