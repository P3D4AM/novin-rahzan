using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard.Helpers;

public class StiBorderElementHelper
{
	public static RectangleD GetBorderContentRect(RectangleD rect, IStiElement element, bool skipMinimalSize = true, float scale = 1f)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		IStiSimpleBorder stiSimpleBorder = element as IStiSimpleBorder;
		if (stiSimpleBorder?.Border == null)
		{
			return rect;
		}
		return GetBorderContentRect(rect, stiSimpleBorder.Border, StiElementScale.Factor(element) * StiScale.Factor * (double)scale, skipMinimalSize);
	}

	public static RectangleD GetBorderContentRect(RectangleD rect, IStiComponentUI comp, bool skipMinimalSize = true, float scale = 1f)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		IStiSimpleBorder stiSimpleBorder = comp as IStiSimpleBorder;
		if (stiSimpleBorder?.Border == null)
		{
			return rect;
		}
		return GetBorderContentRect(rect, stiSimpleBorder.Border, StiElementScale.Factor(comp) * StiScale.Factor * (double)scale, skipMinimalSize);
	}

	public static RectangleD GetBorderContentRect(RectangleD rect, StiSimpleBorder border, double scale, bool skipMinimalSize = true)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		float num = border.GetSizeIncludingSide();
		if (num >= 1f)
		{
			num -= (float)(skipMinimalSize ? 1 : 0);
		}
		((RectangleD)(ref rect)).Inflate(Math.Ceiling((double)(0f - num) * scale), Math.Ceiling((double)(0f - num) * scale));
		return rect;
	}

	public static RectangleD GetBorderContentRect(RectangleD rect, StiBorder border, double scale, bool skipMinimalSize = true)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		float num = border.GetSizeIncludingSide();
		if (num >= 1f)
		{
			num -= (float)(skipMinimalSize ? 1 : 0);
		}
		((RectangleD)(ref rect)).Inflate(Math.Ceiling((double)(0f - num) * scale), Math.Ceiling((double)(0f - num) * scale));
		return rect;
	}
}
