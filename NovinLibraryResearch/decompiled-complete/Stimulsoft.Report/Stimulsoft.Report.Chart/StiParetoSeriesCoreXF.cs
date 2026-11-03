using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiParetoSeriesCoreXF : StiClusteredColumnSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Pareto");

	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		base.ApplyStyle(style, color);
		IStiParetoSeries stiParetoSeries = base.Series as IStiParetoSeries;
		if (base.Series.AllowApplyStyle)
		{
			stiParetoSeries.Brush = style.Core.GetColumnBrush(color);
			stiParetoSeries.BorderColor = style.Core.GetColumnBorder(color);
			stiParetoSeries.Lighting = style.Core.SeriesLighting;
			stiParetoSeries.Marker.Visible = style.Core.MarkerVisible;
			if (stiParetoSeries.Marker != null)
			{
				stiParetoSeries.Marker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)100));
				stiParetoSeries.Marker.BorderColor = StiColorUtils.Dark(color, (byte)100);
			}
			if (stiParetoSeries.LineMarker != null)
			{
				stiParetoSeries.LineMarker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)50));
				stiParetoSeries.LineMarker.BorderColor = StiColorUtils.Dark(color, (byte)150);
			}
		}
		if (!stiParetoSeries.AllowApplyLineColor)
		{
			stiParetoSeries.LineColor = StiColorUtils.Light(color, (byte)50);
		}
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		if (series != null && series.Length != 0 && base.Series.Chart != null)
		{
			base.RenderSeries(context, rect, geom, series);
			RenderLinePareto(context, rect, geom, series);
		}
	}

	private void RenderLinePareto(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] series)
	{
		IStiArea area = geom.Area;
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		IStiAxisArea stiAxisArea = area as IStiAxisArea;
		bool startFromZero = stiAxisArea.XAxis.Core.GetStartFromZero();
		rect.Width += 0.001f;
		List<PointF?[]> list = new List<PointF?[]>();
		List<PointF?[]> list2 = new List<PointF?[]>();
		List<string[]> list3 = new List<string[]>();
		for (int i = 0; i < series.Length; i++)
		{
			IStiBaseLineSeries stiBaseLineSeries = series[i] as IStiBaseLineSeries;
			List<double> paretoValues = GetParetoValues(stiBaseLineSeries.Values);
			int count = paretoValues.Count;
			PointF?[] array = new PointF?[count];
			PointF?[] array2 = new PointF?[count];
			string[] array3 = new string[count];
			for (int j = 0; j < count; j++)
			{
				int num = (startFromZero ? (j + 1) : j);
				if (num >= stiAxisArea.XAxis.Info.StripPositions.Length)
				{
					break;
				}
				float posX = stiAxisArea.XAxis.Info.StripPositions[num];
				double? value = (stiAxisArea.ReverseHor ? paretoValues[paretoValues.Count - j - 1] : paretoValues[j]);
				array[j] = GetPointValue(value, stiBaseLineSeries, stiAxisArea, posX);
				List<double> paretoValues2 = GetParetoValues(stiBaseLineSeries.ValuesStart);
				double? value2 = 0.0;
				if (paretoValues2.Count > j)
				{
					value2 = (stiAxisArea.ReverseHor ? paretoValues2[paretoValues2.Count - j - 1] : paretoValues2[j]);
				}
				array2[j] = GetPointValue(value2, stiBaseLineSeries, stiAxisArea, posX);
				object valueObject = stiAxisArea.XAxis.Info.StripLines[num].ValueObject;
				array3[j] = ((valueObject == null) ? "" : valueObject.ToString());
			}
			if (array.Length != 0)
			{
				array = StiPointHelper.OptimizePoints(array);
				StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
				{
					PointsFrom = array2,
					Points = array,
					PointsIds = array3
				};
				RenderLines(context, geom, pointsInfo, stiBaseLineSeries);
			}
			list.Add(array);
			if (isAnimationChangingValues)
			{
				list2.Add(array2);
			}
		}
	}

	private void RenderLines(StiContext context, StiAreaGeom geom, StiSeriesPointsInfo pointsInfo, IStiBaseLineSeries series)
	{
		if (pointsInfo.Points != null && pointsInfo.Points.Length > 1)
		{
			StiLineSeriesGeom item = new StiLineSeriesGeom(geom, pointsInfo, series);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(item);
		}
	}

	private List<double> GetParetoValues(double?[] values)
	{
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		double num = 0.0;
		foreach (double? num2 in values)
		{
			num += num2.GetValueOrDefault();
			list.Add(num);
		}
		foreach (double item2 in list)
		{
			double item = item2 / num * 100.0;
			list2.Add(item);
		}
		return list2;
	}

	private PointF? GetPointValue(double? value, IStiBaseLineSeries currentSeries, IStiAxisArea axisArea, float posX)
	{
		if (axisArea.ReverseVert && value.HasValue)
		{
			value = 0.0 - value;
		}
		double num = (0.0 - value.GetValueOrDefault()) * (double)(float)axisArea.YRightAxis.Info.Dpi + (double)axisArea.AxisCore.GetDividerRightY();
		return new PointF(posX, (float)num);
	}

	public StiParetoSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
