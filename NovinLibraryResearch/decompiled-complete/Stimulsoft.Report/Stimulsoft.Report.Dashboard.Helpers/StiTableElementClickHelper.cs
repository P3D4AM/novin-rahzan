using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard.Helpers;

public static class StiTableElementClickHelper
{
	private static Hashtable hash { get; set; } = new Hashtable();


	public static event StiTableElementClickEventHandler TableHeaderClick;

	public static void InvokeTableHeaderClick(IStiTableElement table, StiDataTable dataTable, string columnKey, RectangleD rect)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		ResetClick(table);
		StiTableElementClickHelper.TableHeaderClick?.Invoke(table, new StiTableElementClickEventArgs
		{
			DataTable = dataTable,
			ColumnKey = columnKey,
			Rect = rect
		});
	}

	public static void SetPointClick(IStiElement table, PointD point)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		hash[((IStiAppCell)table).GetKey()] = point;
	}

	public static PointD? GetPointClick(IStiTableElement table)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (table != null && hash.ContainsKey(((IStiAppCell)table).GetKey()))
		{
			PointD val = (PointD)hash[((IStiAppCell)table).GetKey()];
			return new PointD(((PointD)(ref val)).X * table.Zoom * StiScale.Factor, ((PointD)(ref val)).Y * table.Zoom * StiScale.Factor);
		}
		return null;
	}

	public static void ResetClick(IStiTableElement table)
	{
		if (table != null && hash.ContainsKey(((IStiAppCell)table).GetKey()))
		{
			hash.Remove(((IStiAppCell)table).GetKey());
		}
	}
}
