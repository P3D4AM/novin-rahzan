using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiIndicatorElementMouseOverHelper
{
	internal static IStiIndicatorElement IndicatorElement { get; set; }

	internal static PointD? MouseOverPoint { get; set; }

	internal static StiIndicatorElementMouseOverData MouseOverData { get; set; }

	internal static void SetMouseOverPoint(IStiIndicatorElement indicator, PointD point)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		IndicatorElement = indicator;
		MouseOverPoint = point;
	}

	internal static void SetMouseOverData(IStiIndicatorElement indicator, StiIndicatorElementMouseOverData data)
	{
		IndicatorElement = indicator;
		MouseOverData = data;
	}

	public static PointD? GetMouseOverPoint(IStiIndicatorElement indicator, bool useZoom = false)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		IStiIndicatorElement indicatorElement = IndicatorElement;
		if (((indicatorElement != null) ? ((IStiAppCell)indicatorElement).GetKey() : null) == ((indicator != null) ? ((IStiAppCell)indicator).GetKey() : null) && MouseOverPoint.HasValue)
		{
			if (useZoom)
			{
				PointD value = MouseOverPoint.Value;
				double num = ((PointD)(ref value)).X * indicator.Zoom;
				value = MouseOverPoint.Value;
				return new PointD(num, ((PointD)(ref value)).Y * indicator.Zoom);
			}
			return MouseOverPoint;
		}
		return null;
	}

	internal static StiIndicatorElementMouseOverData GetMouseOverData(IStiIndicatorElement indicator)
	{
		IStiIndicatorElement indicatorElement = IndicatorElement;
		if (((indicatorElement != null) ? ((IStiAppCell)indicatorElement).GetKey() : null) == ((indicator != null) ? ((IStiAppCell)indicator).GetKey() : null) && MouseOverData != null)
		{
			return MouseOverData;
		}
		return null;
	}

	public static void ResetMouseOver(IStiIndicatorElement indicator)
	{
		IStiIndicatorElement indicatorElement = IndicatorElement;
		if (((indicatorElement != null) ? ((IStiAppCell)indicatorElement).GetKey() : null) == ((indicator != null) ? ((IStiAppCell)indicator).GetKey() : null))
		{
			IndicatorElement = null;
			MouseOverPoint = null;
			MouseOverData = null;
		}
	}
}
