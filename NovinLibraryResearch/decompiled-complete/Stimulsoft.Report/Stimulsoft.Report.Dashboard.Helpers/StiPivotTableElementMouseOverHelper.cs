using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Helpers;

public static class StiPivotTableElementMouseOverHelper
{
	private static IStiPivotTableElement TableElement { get; set; }

	private static PointD? MouseOverPoint { get; set; }

	public static void SetMouseOverPoint(IStiPivotTableElement table, PointD point)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		TableElement = table;
		MouseOverPoint = point;
	}

	public static PointD? GetMouseOverPoint(IStiPivotTableElement table)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		IStiPivotTableElement tableElement = TableElement;
		if (((tableElement != null) ? ((IStiAppCell)tableElement).GetKey() : null) == ((table != null) ? ((IStiAppCell)table).GetKey() : null) && MouseOverPoint.HasValue)
		{
			PointD value = MouseOverPoint.Value;
			double num = ((PointD)(ref value)).X * table.Zoom * StiScale.Factor;
			value = MouseOverPoint.Value;
			return new PointD(num, ((PointD)(ref value)).Y * table.Zoom * StiScale.Factor);
		}
		return null;
	}

	public static void ResetMouseOverPoint(IStiPivotTableElement table)
	{
		IStiPivotTableElement tableElement = TableElement;
		if (((tableElement != null) ? ((IStiAppCell)tableElement).GetKey() : null) == ((table != null) ? ((IStiAppCell)table).GetKey() : null))
		{
			TableElement = null;
			MouseOverPoint = null;
		}
	}
}
