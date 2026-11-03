using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiInsideBaseAxisLabelsCoreXF : StiCenterAxisLabelsCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsInsideBase");

	public override int Position => 2;

	protected override RectangleF GetLabelRect(StiContext context, PointF endPoint, PointF startPoint, double? value, string labelText, bool checkHeight, StiFontGeom font, StiStringFormatGeom sf)
	{
		SizeF sizeF = context.MeasureString(labelText, font);
		if (base.SeriesLabels.Chart.Area.Core.SeriesOrientation == StiChartSeriesOrientation.Vertical)
		{
			if (value > 0.0)
			{
				float num = startPoint.Y - (float)(Math.Abs((double)sizeF.Height * Math.Cos((double)base.SeriesLabels.Angle * Math.PI / 180.0)) + Math.Abs((double)(sizeF.Width / 2f + 2f) * Math.Sin((double)base.SeriesLabels.Angle * Math.PI / 180.0)));
				if (num < endPoint.Y)
				{
					num = endPoint.Y + (startPoint.Y - endPoint.Y) / 2f;
				}
				return context.MeasureRotatedString(labelText, font, new PointF(endPoint.X, num), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
			}
			float num2 = startPoint.Y + (float)(Math.Abs((double)sizeF.Height * Math.Cos((double)base.SeriesLabels.Angle * Math.PI / 180.0)) + Math.Abs((double)(sizeF.Width / 2f + 2f) * Math.Sin((double)base.SeriesLabels.Angle * Math.PI / 180.0)));
			if (num2 > endPoint.Y)
			{
				num2 = endPoint.Y - (endPoint.Y - startPoint.Y) / 2f;
			}
			return context.MeasureRotatedString(labelText, font, new PointF(endPoint.X, num2), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
		}
		if (value > 0.0)
		{
			return context.MeasureRotatedString(labelText, font, new PointF(startPoint.X - (float)(Math.Abs((double)sizeF.Height * Math.Sin((double)base.SeriesLabels.Angle * Math.PI / 180.0)) + Math.Abs((double)(sizeF.Width / 2f + 2f) * Math.Cos((double)base.SeriesLabels.Angle * Math.PI / 180.0))), endPoint.Y), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
		}
		return context.MeasureRotatedString(labelText, font, new PointF(startPoint.X + (float)(Math.Abs((double)sizeF.Height * Math.Sin((double)base.SeriesLabels.Angle * Math.PI / 180.0)) + Math.Abs((double)(sizeF.Width / 2f + 2f) * Math.Cos((double)base.SeriesLabels.Angle * Math.PI / 180.0))), endPoint.Y), sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
	}

	public StiInsideBaseAxisLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
