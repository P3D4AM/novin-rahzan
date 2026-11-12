using System.Collections;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiPivotTableElementViewHelper
{
	public static Hashtable GetPivotTableData(IStiPivotTableElement pivotElement)
	{
		return (Hashtable)StiInvokeMethodsHelper.InvokeStaticMethod("Stimulsoft.Dashboard", "Helpers.StiPivotTableHelper", "GetViewerData", new object[1] { pivotElement });
	}

	public static Hashtable GetPivotTableSettings(IStiPivotTableElement tableElement)
	{
		Hashtable hashtable = new Hashtable();
		StiPivotElementStyle pivotTableStyle = StiDashboardStyleHelper.GetPivotTableStyle(tableElement);
		hashtable["alternatingCellBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.AlternatingCellBackColor);
		hashtable["alternatingCellForeColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.AlternatingCellForeColor);
		hashtable["cellBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.CellBackColor);
		hashtable["cellForeColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.CellForeColor);
		hashtable["columnHeaderBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.ColumnHeaderBackColor);
		hashtable["columnHeaderForeColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.ColumnHeaderForeColor);
		hashtable["hotColumnHeaderBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.HotColumnHeaderBackColor);
		hashtable["hotRowHeaderBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.HotRowHeaderBackColor);
		hashtable["lineColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.LineColor);
		hashtable["rowHeaderBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.RowHeaderBackColor);
		hashtable["rowHeaderForeColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.RowHeaderForeColor);
		hashtable["selectedCellBackColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.SelectedCellBackColor);
		hashtable["selectedCellForeColor"] = StiReportHelper.GetHtmlColor(pivotTableStyle.SelectedCellForeColor);
		hashtable["fontName"] = StiElementConsts.Table.Font.Name;
		hashtable["fontSize"] = StiElementConsts.Table.Font.Size;
		hashtable["fontIsBold"] = StiElementConsts.Table.Font.IsBold;
		hashtable["cellHeight"] = StiElementConsts.Table.Height;
		return hashtable;
	}

	private static string GetCellAlignment(object component)
	{
		if (component is IStiHorAlignment stiHorAlignment)
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
}
