using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public abstract class StiRadarSeriesCoreXF : StiSeriesCoreXF
{
	public override void ApplyStyle(IStiChartStyle style, Color color)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		base.ApplyStyle(style, color);
		IStiRadarSeries stiRadarSeries = base.Series as IStiRadarSeries;
		if (stiRadarSeries.AllowApplyStyle && stiRadarSeries.Marker != null)
		{
			stiRadarSeries.Marker.Brush = (StiBrush)new StiSolidBrush(StiColorUtils.Light(color, (byte)100));
			stiRadarSeries.Marker.BorderColor = StiColorUtils.Dark(color, (byte)100);
		}
	}

	private List<List<PointF?>> GetPointsList(double?[] values, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		List<List<PointF?>> list = new List<List<PointF?>>();
		IStiRadarArea stiRadarArea = geom.Area as IStiRadarArea;
		StiRadarAreaCoreXF stiRadarAreaCoreXF = stiRadarArea.Core as StiRadarAreaCoreXF;
		for (int i = 0; i < seriesArray.Length; i++)
		{
			IStiRadarSeries stiRadarSeries = (IStiRadarSeries)seriesArray[i];
			List<PointF?> list2 = new List<PointF?>();
			list.Add(list2);
			int num = 0;
			foreach (PointF point in stiRadarAreaCoreXF.Points)
			{
				PointF? pointF = point;
				double? num2 = ((num < stiRadarSeries.Values.Length) ? stiRadarSeries.Values[num] : null);
				if (!num2.HasValue && stiRadarSeries.ShowNulls)
				{
					num2 = 0.0;
				}
				if (!num2.HasValue)
				{
					list2.Add(null);
				}
				else
				{
					if (num2.HasValue)
					{
						num2 = (num2 - stiRadarArea.YAxis.Info.Minimum) * stiRadarArea.YAxis.Info.Dpi;
					}
					PointF value = new PointF((float)((double)stiRadarAreaCoreXF.CenterPoint.X + num2.GetValueOrDefault() * (double)pointF.Value.X), (float)((double)stiRadarAreaCoreXF.CenterPoint.Y + num2.GetValueOrDefault() * (double)pointF.Value.Y));
					value.X -= geom.ClientRectangle.X;
					value.Y -= geom.ClientRectangle.Y;
					list2.Add(value);
				}
				num++;
			}
		}
		return list;
	}

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
		if (seriesArray == null || seriesArray.Length == 0 || base.Series.Chart == null)
		{
			return;
		}
		bool isAnimationChangingValues = ((StiChart)base.Series.Chart).IsAnimationChangingValues;
		List<List<PointF?>> pointsList = GetPointsList(base.Series.Values, geom, seriesArray);
		List<List<PointF?>> pointsList2 = GetPointsList(base.Series.ValuesStart, geom, seriesArray);
		int num = 0;
		for (int j = 0; j < seriesArray.Length; j++)
		{
			IStiRadarSeries stiRadarSeries = (IStiRadarSeries)seriesArray[j];
			List<PointF?> list = pointsList[num];
			list.Add(list[0]);
			List<string> list2 = stiRadarSeries.Arguments.Cast<string>().ToList();
			if (list2.Count < list.Count - 1)
			{
				list2 = list.Select((PointF? p, int i) => i.ToString()).ToList();
			}
			list2.Add(list2[0] + "_e");
			List<PointF?> list3 = pointsList2[num];
			list3.Add(list3[0]);
			PointF centerPoint = new PointF(geom.ClientRectangle.Width / 2f, geom.ClientRectangle.Height / 2f);
			StiSeriesPointsInfo pointsInfo = new StiSeriesPointsInfo
			{
				PointsFrom = list3.Select((PointF? p) => p ?? new PointF?(centerPoint)).ToArray(),
				Points = list.Select((PointF? p) => p ?? new PointF?(centerPoint)).ToArray(),
				PointsIds = list2.ToArray()
			};
			RenderAreas(context, stiRadarSeries, pointsInfo, geom);
			RenderLines(context, stiRadarSeries, pointsInfo, geom);
			list.RemoveAt(list.Count - 1);
			RenderPoints(context, stiRadarSeries, list, geom);
			list.RemoveAt(list.Count - 1);
			num++;
		}
	}

	public virtual void RenderAreas(StiContext context, IStiRadarSeries series, StiSeriesPointsInfo pointsInfo, StiAreaGeom geom)
	{
	}

	public virtual void RenderLines(StiContext context, IStiRadarSeries series, StiSeriesPointsInfo pointsInfo, StiAreaGeom geom)
	{
	}

	public virtual void RenderPoints(StiContext context, IStiRadarSeries series, List<PointF?> points, StiAreaGeom geom)
	{
		int num = 0;
		foreach (PointF? point in points)
		{
			if (point.HasValue)
			{
				double? num2 = ((num < series.Values.Length) ? series.Values[num] : null);
				StiRadarPointSeriesElementGeom item = new StiRadarPointSeriesElementGeom(geom, num2.GetValueOrDefault(), num, series.Marker.Brush, series, point.Value, context.Options.Zoom);
				if (geom != null)
				{
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(item);
				}
				IStiAxisSeriesLabels seriesLabels = series.Core.GetSeriesLabels();
				if (seriesLabels != null && seriesLabels.Visible && num2.HasValue)
				{
					PointF valueOrDefault = point.GetValueOrDefault();
					if (seriesLabels.Step == 0 || num % seriesLabels.Step == 0)
					{
						StiSeriesLabelsGeom stiSeriesLabelsGeom = ((StiAxisSeriesLabelsCoreXF)seriesLabels.Core).RenderLabel(series, context, valueOrDefault, valueOrDefault, num, num2, num2, GetArgument(series, num), series.Core.GetTag(num), 0, 1, geom.ClientRectangle);
						if (stiSeriesLabelsGeom != null)
						{
							geom.CreateChildGeoms();
							geom.ChildGeoms.Add(stiSeriesLabelsGeom);
							stiSeriesLabelsGeom.ClientRectangle = CheckLabelsRect(seriesLabels, geom, stiSeriesLabelsGeom.ClientRectangle);
						}
					}
				}
			}
			num++;
		}
	}

	private string GetArgument(IStiRadarSeries series, int pointIndex)
	{
		if (series.Arguments.Length > pointIndex && series.Arguments[pointIndex] != null)
		{
			return series.Arguments[pointIndex].ToString();
		}
		return string.Empty;
	}

	public override StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		StiBrush seriesBrush = base.GetSeriesBrush(colorIndex, colorCount);
		if (seriesBrush == null)
		{
			return (StiBrush)new StiSolidBrush(StiColorUtils.Dark(Color.White, (byte)20));
		}
		return seriesBrush;
	}

	public override object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		object seriesBorderColor = base.GetSeriesBorderColor(colorIndex, colorCount);
		if (seriesBorderColor == null)
		{
			return Color.Black;
		}
		return seriesBorderColor;
	}

	public StiRadarSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
