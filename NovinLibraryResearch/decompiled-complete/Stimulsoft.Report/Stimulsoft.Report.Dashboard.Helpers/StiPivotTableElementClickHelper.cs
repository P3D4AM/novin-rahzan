using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Helpers;

public static class StiPivotTableElementClickHelper
{
	private static Hashtable hash { get; set; } = new Hashtable();


	public static event StiPivotTableElementClickEventHandler TableHeaderClick;

	public static void InvokeTableHeaderClick(IStiPivotTableElement table, string meterKey)
	{
		ResetClick(table);
		StiPivotTableElementClickHelper.TableHeaderClick?.Invoke(table, new StiPivotTableElementClickEventArgs
		{
			MeterKey = meterKey
		});
	}

	public static void SetPointClick(IStiElement table, PointD point)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		hash[((IStiAppCell)table).GetKey()] = point;
	}

	public static PointD? GetPointClick(IStiPivotTableElement table)
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

	public static void ResetClick(IStiPivotTableElement table)
	{
		if (table != null && hash.ContainsKey(((IStiAppCell)table).GetKey()))
		{
			hash.Remove(((IStiAppCell)table).GetKey());
		}
	}
}
