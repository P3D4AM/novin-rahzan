using System;
using System.Drawing;
using System.Threading;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiXRadarAxisCoreXF : StiRadarAxisCoreXF
{
	public IStiXRadarAxis XAxis => base.Axis as IStiXRadarAxis;

	public override void ApplyStyle(IStiChartStyle style)
	{
		if (base.Axis.AllowApplyStyle)
		{
			((IStiXRadarAxis)base.Axis).Labels.Core.ApplyStyle(style);
		}
	}

	public virtual StiXRadarAxisLabelGeom RenderLabel(StiContext context, IStiSeries series, PointF point, object argument, float angle, int colorIndex, int colorCount)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		string labelText = GetLabelText(argument);
		RectangleF labelRect = GetLabelRect(context, point, labelText, 0f);
		labelRect.X = (0f - labelRect.Width) / 2f;
		labelRect.Y = (0f - labelRect.Height) / 2f;
		RectangleF labelRect2 = GetLabelRect(context, point, labelText, angle);
		Color borderColor = ((IStiXRadarAxis)base.Axis).Labels.BorderColor;
		StiBrush labelBrush = (StiBrush)new StiSolidBrush(((IStiXRadarAxis)base.Axis).Labels.Color);
		return new StiXRadarAxisLabelGeom(base.Axis as IStiXRadarAxis, labelText, labelBrush, borderColor, angle, labelRect, labelRect2, point);
	}

	internal string GetLabelText(object value)
	{
		try
		{
			IStiXRadarAxis stiXRadarAxis = base.Axis as IStiXRadarAxis;
			if (stiXRadarAxis.Labels.Format != null && stiXRadarAxis.Labels.Format.Trim().Length != 0)
			{
				try
				{
					if (value is string && decimal.TryParse(value.ToString().Replace(".", ",").Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator), out var result))
					{
						value = result;
					}
					if (!StiChartOptions.OldChartPercentMode && stiXRadarAxis.Labels.Format.StartsWith("P", StringComparison.InvariantCulture))
					{
						int result2 = 0;
						if (stiXRadarAxis.Labels.Format.Length > 1)
						{
							int.TryParse(stiXRadarAxis.Labels.Format.Remove(0, 1), out result2);
						}
						return string.Format("{0}{1:N" + result2 + "}{2}{3}", stiXRadarAxis.Labels.TextBefore, value, "%", stiXRadarAxis.Labels.TextAfter);
					}
					return string.Format("{0}{1:" + stiXRadarAxis.Labels.Format + "}{2}", stiXRadarAxis.Labels.TextBefore, value, stiXRadarAxis.Labels.TextAfter);
				}
				catch
				{
				}
			}
			return $"{stiXRadarAxis.Labels.TextBefore}{value}{stiXRadarAxis.Labels.TextAfter}";
		}
		catch
		{
		}
		return value.ToString();
	}

	public virtual RectangleF GetLabelRect(StiContext context, PointF point, string text, float angle)
	{
		StiFontGeom font = StiFontGeom.ChangeFontSize(((IStiXRadarAxis)base.Axis).Labels.Font, ((IStiXRadarAxis)base.Axis).Labels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
		genericStringFormat.Trimming = (StringTrimming)0;
		if (!((IStiXRadarAxis)base.Axis).Labels.WordWrap)
		{
			genericStringFormat.FormatFlags = (StringFormatFlags)6144;
		}
		return context.MeasureRotatedString(text, font, point, genericStringFormat, (StiRotationMode)5, angle, (int)(((IStiXRadarAxis)base.Axis).Labels.Width * context.Options.Zoom));
	}

	public StiXRadarAxisCoreXF(IStiRadarAxis axis)
		: base(axis)
	{
	}
}
