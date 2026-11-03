using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiCenterPie3dLabelsCoreXF : StiSeriesLabelsCoreXF
{
	[Browsable(false)]
	public float percentPerValue;

	public override StiSeriesLabelsType SeriesLabelsType => StiSeriesLabelsType.Pie3d;

	public override int Position => 18;

	public override string LocalizedName => "3D " + StiLocalization.Get("Chart", "LabelsCenter");

	internal StiSeriesLabelsGeom RenderLabel(IStiSeries series, StiContext context, StiPie3dSlice pie3dSlice)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		string labelText = GetLabelText(series, pie3dSlice.Value, pie3dSlice.ArgumentText, pie3dSlice.Tag, series.CoreTitle);
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom stringFormatGeom = GetStringFormatGeom(context);
		Rectangle rectangle = Rectangle.Round(GetLabelRect(context, pie3dSlice.TextPosition, labelText, font, stringFormatGeom));
		Rectangle rectangle2 = rectangle;
		rectangle2.X = -rectangle2.Width / 2;
		rectangle2.Y = -rectangle2.Height / 2;
		Color borderColor = GetBorderColor(series, pie3dSlice.ColorIndex, pie3dSlice.ColorCount);
		Color seriesBorderColor = (Color)series.Core.GetSeriesBorderColor(pie3dSlice.ColorIndex, pie3dSlice.ColorCount);
		StiBrush seriesBrush = series.Core.GetSeriesBrush(pie3dSlice.ColorIndex, pie3dSlice.ColorCount);
		StiSolidBrush labelBrush = new StiSolidBrush(GetLabelColor(series, pie3dSlice.ColorIndex, pie3dSlice.ColorCount));
		seriesBrush = ProcessSeriesColors(pie3dSlice.Index, seriesBrush, series);
		StiBrush seriesLabelsBrush = ProcessSeriesColors(pie3dSlice.Index, base.SeriesLabels.Brush, series);
		return new StiCenterPieLabelsGeom(base.SeriesLabels, series, pie3dSlice.Index, pie3dSlice.Value, rectangle2, labelText, seriesBrush, (StiBrush)(object)labelBrush, seriesLabelsBrush, borderColor, seriesBorderColor, (StiRotationMode)4, rectangle, base.SeriesLabels.Angle, null);
	}

	public virtual RectangleF GetLabelRect(StiContext context, PointF labelPoint, string labelText, StiFontGeom font, StiStringFormatGeom sf)
	{
		return context.MeasureRotatedString(labelText, font, labelPoint, sf, (StiRotationMode)4, 0f, (int)((float)base.SeriesLabels.Width * context.Options.Zoom));
	}

	internal override double RecalcValue(double value, int signs)
	{
		if (((IStiPieSeriesLabels)base.SeriesLabels).ShowInPercent)
		{
			List<IStiSeries> series = base.SeriesLabels.Chart.Area.Core.GetSeries();
			if (series.Count > 0)
			{
				IStiSeries[] array = new IStiSeries[series.Count];
				series.CopyTo(array);
				if (array.Length != 0 && array[0] is StiPieSeries)
				{
					percentPerValue = ((StiPieSeriesCoreXF)array[0].Core).GetPercentPerValue(array);
				}
			}
			return Math.Round(value * (double)percentPerValue, signs);
		}
		return value;
	}

	public StiCenterPie3dLabelsCoreXF(IStiSeriesLabels seriesLabels)
		: base(seriesLabels)
	{
	}
}
