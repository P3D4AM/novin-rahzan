using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiLinearTickLabelBase : StiTickLabelBase
{
	protected internal override void DrawElement(StiGaugeContextPainter context)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		if (!(base.Scale is StiLinearScale stiLinearScale))
		{
			return;
		}
		SizeF size = stiLinearScale.barGeometry.Size;
		RectangleF rectGeometry = stiLinearScale.barGeometry.RectGeometry;
		if (size.Width == 0f || size.Height == 0f)
		{
			return;
		}
		Dictionary<float, float> pointCollection = GetPointCollection();
		if (pointCollection.Count == 0)
		{
			return;
		}
		Dictionary<float, string> labels = StiTickLabelHelper.GetLabels(pointCollection);
		string textFormat = base.TextFormat;
		float[] skipValuesObj = base.SkipValuesObj;
		float[] skipIndicesObj = base.SkipIndicesObj;
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
		int num3 = -1;
		RectangleF? rectangleF = null;
		foreach (float key in pointCollection.Keys)
		{
			num3++;
			if (key < actualMinimum || key > actualMaximum || CheckTickValue(skipValuesObj, skipIndicesObj, key, num3) || (base.MinimumValue.HasValue && key < base.MinimumValue.Value) || (base.MaximumValue.HasValue && key > base.MaximumValue.Value))
			{
				continue;
			}
			string text = null;
			text = ((base.FormatService == null) ? ((string.IsNullOrEmpty(textFormat) && base.Scale.Gauge.ShortValue) ? labels[key] : GetTextForRender(key, textFormat)) : base.FormatService.Format(key));
			Font font = StiGaugeContextPainter.ChangeFontSize(base.Font, context.Zoom);
			SizeF size2 = context.MeasureString(text, font);
			float num4 = pointCollection[key] * num;
			float x;
			float y;
			if ((int)stiLinearScale.Orientation == 0)
			{
				x = (stiLinearScale.IsReversed ? (rectGeometry.Right - num4 - size2.Width / 2f) : (rectGeometry.Left + num4 - size2.Width / 2f));
				if (base.Placement == StiPlacement.Overlay)
				{
					y = StiRectangleHelper.CenterY(rectGeometry) - size2.Height / 2f - num2;
				}
				else
				{
					float num5 = (flag ? ((1f - pointCollection[key]) * restToLenght) : (restToLenght * pointCollection[key]));
					y = ((base.Placement != 0) ? (rectGeometry.Bottom + num2 - num5) : (rectGeometry.Top - size2.Height - num2 + num5));
				}
			}
			else
			{
				y = (stiLinearScale.IsReversed ? (rectGeometry.Top + num4 - size2.Height / 2f) : (rectGeometry.Bottom - num4 - size2.Height / 2f));
				if (base.Placement == StiPlacement.Overlay)
				{
					x = StiRectangleHelper.CenterX(rectGeometry) - size2.Width / 2f - num2;
				}
				else
				{
					float num6 = (flag ? ((1f - pointCollection[key]) * restToLenght) : (restToLenght * pointCollection[key]));
					x = ((base.Placement != 0) ? (rectGeometry.Right + num2 + 3f - num6) : (rectGeometry.Left - size2.Width - 3f - num2 + num6));
				}
			}
			RectangleF rectangleF2 = new RectangleF(new PointF(x, y), size2);
			if (!rectangleF.HasValue || !rectangleF.GetValueOrDefault().IntersectsWith(rectangleF2))
			{
				context.AddTextGaugeGeom(text, font, base.TextBrush, rectangleF2, null);
				rectangleF = rectangleF2;
			}
		}
	}
}
