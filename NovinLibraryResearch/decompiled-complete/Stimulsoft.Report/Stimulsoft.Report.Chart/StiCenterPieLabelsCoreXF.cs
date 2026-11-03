using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCenterPieLabelsCoreXF : StiPieSeriesLabelsCoreXF
{
	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Pie;

	public override int Position => 11;

	public override string LocalizedName => StiLocalization.Get("Chart", "LabelsCenter");

	public override StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, PointF centerPie, float radius, float radius2, float pieAngle, int pointIndex, double? value, double? labelValue, string argumentText, string tag, bool measure, int colorIndex, int colorCount, float percentPerValue, out RectangleF measureRect, bool drawValue, float deltaY)
	{
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		measureRect = RectangleF.Empty;
		base.percentPerValue = percentPerValue;
		if (!labelValue.HasValue && !base.SeriesLabels.ShowNulls)
		{
			return null;
		}
		if (labelValue == 0.0 && !base.SeriesLabels.ShowZeros)
		{
			return null;
		}
		if (!value.HasValue)
		{
			value = 0.0;
		}
		if (!labelValue.HasValue)
		{
			labelValue = 0.0;
		}
		if (float.IsNaN(radius) || float.IsNaN(radius2))
		{
			return null;
		}
		string text = (drawValue ? GetFormattedValue(series, labelValue.Value) : GetLabelText(series, labelValue, argumentText, tag, series.CoreTitle));
		float angleRad = (float)(Math.PI * (double)pieAngle / 180.0);
		PointF labelPoint = GetLabelPoint(centerPie, (radius - radius2) / 1.75f + radius2, angleRad);
		if (!drawValue)
		{
			if (this is StiOutsidePieLabelsCoreXF)
			{
				labelPoint = GetLabelPoint(centerPie, radius + ((IStiOutsidePieLabels)base.SeriesLabels).LineLength * context.Options.Zoom, angleRad);
			}
			if (this is StiInsideEndPieLabelsCoreXF)
			{
				labelPoint = GetLabelPoint(centerPie, (radius - radius2) / 1.2f + radius2, angleRad);
			}
		}
		if (float.IsNaN(labelPoint.X))
		{
			labelPoint.X = 0f;
		}
		if (float.IsNaN(labelPoint.Y))
		{
			labelPoint.Y = 0f;
		}
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
		Rectangle rectangle = Rectangle.Round(GetLabelRect(context, labelPoint, text, font, stringFormatGeom));
		float num = base.SeriesLabels.Angle;
		StiRotationMode val = (StiRotationMode)4;
		Rectangle rectangle2 = rectangle;
		rectangle2.X = -rectangle2.Width / 2;
		rectangle2.Y = -rectangle2.Height / 2;
		if (((IStiCenterPieLabels)base.SeriesLabels).AutoRotate)
		{
			num = pieAngle;
			if ((num > 0f && num < 90f) || num > 270f)
			{
				if (base.SeriesLabels is IStiOutsidePieLabels)
				{
					val = (StiRotationMode)1;
				}
				rectangle2.X = 0;
			}
			else
			{
				if (base.SeriesLabels is IStiOutsidePieLabels)
				{
					val = (StiRotationMode)7;
				}
				num += 180f;
				rectangle2.X = -rectangle2.Width;
			}
		}
		if (this is StiOutsidePieLabelsCoreXF)
		{
			if (!((IStiCenterPieLabels)base.SeriesLabels).AutoRotate)
			{
				if ((double)pieAngle > 337.5 || (double)pieAngle < 22.5)
				{
					rectangle2.X = 0;
					rectangle2.Y = -rectangle2.Height / 2;
					val = (StiRotationMode)1;
				}
				else if ((double)pieAngle >= 22.5 && (double)pieAngle < 67.5)
				{
					rectangle2.X = 0;
					rectangle2.Y = 0;
					val = (StiRotationMode)0;
				}
				else if ((double)pieAngle >= 67.5 && (double)pieAngle < 112.5)
				{
					rectangle2.X = -rectangle2.Width / 2;
					rectangle2.Y = 0;
					val = (StiRotationMode)3;
				}
				else if ((double)pieAngle >= 112.5 && (double)pieAngle < 157.5)
				{
					rectangle2.X = -rectangle2.Width;
					rectangle2.Y = 0;
					val = (StiRotationMode)6;
				}
				else if ((double)pieAngle >= 157.5 && (double)pieAngle < 202.5)
				{
					rectangle2.X = -rectangle2.Width;
					rectangle2.Y = -rectangle2.Height / 2;
					val = (StiRotationMode)7;
				}
				else if ((double)pieAngle >= 202.5 && (double)pieAngle < 247.5)
				{
					rectangle2.X = -rectangle2.Width;
					rectangle2.Y = -rectangle2.Height;
					val = (StiRotationMode)8;
				}
				else if ((double)pieAngle >= 247.5 && (double)pieAngle < 292.5)
				{
					rectangle2.X = -rectangle2.Width / 2;
					rectangle2.Y = -rectangle2.Height;
					val = (StiRotationMode)5;
				}
				else if ((double)pieAngle >= 292.5)
				{
					rectangle2.X = 0;
					rectangle2.Y = -rectangle2.Height;
					val = (StiRotationMode)2;
				}
			}
		}
		else
		{
			rectangle2.X = -rectangle2.Width / 2;
			rectangle2.Y = -rectangle2.Height / 2;
		}
		if (measure)
		{
			measureRect = context.MeasureRotatedString(text, font, rectangle, stringFormatGeom, val, num);
			return null;
		}
		Color borderColor = GetBorderColor(series, colorIndex, colorCount);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(colorIndex, colorCount);
		StiSolidBrush labelBrush = new StiSolidBrush(GetLabelColor(series, colorIndex, colorCount));
		seriesBrush = ProcessSeriesColors(pointIndex, seriesBrush, series);
		StiBrush seriesLabelsBrush = ProcessSeriesColors(pointIndex, base.SeriesLabels.Brush, series);
		measureRect = RectangleF.Empty;
		if (base.SeriesLabels is IStiOutsidePieLabels)
		{
			StiOutsidePieLabelsCoreXF stiOutsidePieLabelsCoreXF = this as StiOutsidePieLabelsCoreXF;
			Color lineColor = stiOutsidePieLabelsCoreXF.GetLineColor(series, colorIndex, colorCount);
			labelPoint = GetLabelPoint(centerPie, radius + ((IStiOutsidePieLabels)stiOutsidePieLabelsCoreXF.SeriesLabels).LineLength * context.Options.Zoom, angleRad);
			PointF labelPoint2 = GetLabelPoint(centerPie, radius, angleRad);
			return new StiOutsidePieLabelsGeom(base.SeriesLabels, series, pointIndex, value.Value, rectangle2, text, seriesBrush, (StiBrush)(object)labelBrush, seriesLabelsBrush, borderColor, seriesBorderColor, val, rectangle, num, lineColor, labelPoint, labelPoint2);
		}
		return new StiCenterPieLabelsGeom(base.SeriesLabels, series, pointIndex, value.Value, rectangle2, text, seriesBrush, (StiBrush)(object)labelBrush, seriesLabelsBrush, borderColor, seriesBorderColor, val, rectangle, num, null);
	}

	public PointF GetLabelPoint(PointF centerPie, float radius, float angleRad)
	{
		return new PointF(centerPie.X + (float)Math.Cos(angleRad) * radius, centerPie.Y + (float)Math.Sin(angleRad) * radius);
	}

	public virtual RectangleF GetLabelRect(StiContext context, PointF labelPoint, string labelText, StiFontGeom font, StiStringFormatGeom sf)
	{
		return context.MeasureRotatedString(labelText, font, labelPoint, sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
	}

	public StiCenterPieLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
