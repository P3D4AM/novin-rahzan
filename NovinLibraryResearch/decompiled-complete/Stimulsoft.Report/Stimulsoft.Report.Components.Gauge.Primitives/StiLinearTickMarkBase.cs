using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiLinearTickMarkBase : StiTickMarkBase
{
	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		if (!(base.Scale is StiLinearScale stiLinearScale))
		{
			return;
		}
		SizeF size = stiLinearScale.barGeometry.Size;
		RectangleF rectGeometry = stiLinearScale.barGeometry.RectGeometry;
		Dictionary<float, float> pointCollection = GetPointCollection();
		if (pointCollection.Count == 0)
		{
			return;
		}
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
		StiGaugeElementSkin actualSkin = GetActualSkin();
		float actualMinimum = base.Scale.ScaleHelper.ActualMinimum;
		float actualMaximum = base.Scale.ScaleHelper.ActualMaximum;
		bool flag = stiLinearScale.StartWidth < stiLinearScale.EndWidth;
		float num;
		float num2;
		if ((int)stiLinearScale.Orientation == 0)
		{
			num = rectGeometry.Width;
			num2 = size.Height;
		}
		else
		{
			num = rectGeometry.Height;
			num2 = size.Width;
		}
		float restToLenght = stiLinearScale.barGeometry.GetRestToLenght();
		num2 *= base.Offset;
		float num3 = size.Width * base.RelativeWidth;
		float num4 = size.Height * base.RelativeHeight;
		int num5 = -1;
		foreach (float key in pointCollection.Keys)
		{
			num5++;
			if (key < actualMinimum || key > actualMaximum || CheckTickValue(skipValuesObj, skipIndicesObj, key, num5) || (base.MinimumValue.HasValue && key < base.MinimumValue.Value) || (base.MaximumValue.HasValue && key > base.MaximumValue.Value))
			{
				continue;
			}
			float num6 = pointCollection[key] * num;
			float num7;
			float num8;
			if ((int)stiLinearScale.Orientation == 0)
			{
				num7 = (stiLinearScale.IsReversed ? (rectGeometry.Right - num6 - num3) : (rectGeometry.Left + num6 - num3 / 2f));
				if (base.Placement == StiPlacement.Overlay)
				{
					num8 = StiRectangleHelper.CenterY(rectGeometry) - num4 / 2f - num2;
				}
				else
				{
					float num9 = (flag ? ((1f - pointCollection[key]) * restToLenght) : (restToLenght * pointCollection[key]));
					num8 = ((base.Placement != 0) ? (rectGeometry.Bottom + num2 - num9) : (rectGeometry.Top - num4 - num2 + num9));
				}
			}
			else
			{
				num8 = (stiLinearScale.IsReversed ? (rectGeometry.Top + num6 - num4 / 2f) : (rectGeometry.Bottom - num6 - num4 / 2f));
				if (base.Placement == StiPlacement.Overlay)
				{
					num7 = StiRectangleHelper.CenterX(rectGeometry) - num3 / 2f - num2;
				}
				else
				{
					float num10 = (flag ? ((1f - pointCollection[key]) * restToLenght) : (restToLenght * pointCollection[key]));
					num7 = ((base.Placement != 0) ? (rectGeometry.Right + num2 + 3f - num10) : (rectGeometry.Left - num3 - 3f - num2 + num10));
				}
			}
			actualSkin.Draw(context, this, new RectangleF((float)Math.Round(num7), (float)Math.Round(num8), (float)Math.Ceiling(num3), (float)Math.Ceiling(num4)));
		}
	}
}
