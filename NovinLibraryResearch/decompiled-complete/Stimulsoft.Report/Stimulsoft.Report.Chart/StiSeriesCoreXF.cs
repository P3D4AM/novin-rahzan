using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public abstract class StiSeriesCoreXF : ICloneable, IStiApplyStyleSeries
{
	private Dictionary<int, bool> isMouseOverSeriesElementHashtable;

	private bool isDateTimeValues;

	private bool isDateTimeArguments;

	private IStiSeries series;

	public bool IsMouseOver { get; set; }

	public abstract string LocalizedName { get; }

	[Browsable(false)]
	[StiNonSerialized]
	internal Color[] SeriesColors { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public bool IsDateTimeValues
	{
		get
		{
			return isDateTimeValues;
		}
		set
		{
			isDateTimeValues = value;
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public bool IsDateTimeArguments
	{
		get
		{
			return isDateTimeArguments;
		}
		set
		{
			isDateTimeArguments = value;
		}
	}

	public IStiSeries Series
	{
		get
		{
			return series;
		}
		set
		{
			series = value;
		}
	}

	public IStiSeriesInteraction Interaction
	{
		get
		{
			return Series.Interaction;
		}
		set
		{
			Series.Interaction = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public virtual void ApplyStyle(IStiChartStyle style, Color color)
	{
		if (Series.AllowApplyStyle)
		{
			Series.ShowShadow = style.Core.SeriesShowShadow;
			if (Series.SeriesLabels != null)
			{
				Series.SeriesLabels.Core.ApplyStyle(style);
			}
		}
		foreach (StiTrendLine trendLine in series.TrendLines)
		{
			trendLine.Core.ApplyStyle(style);
		}
	}

	protected RectangleF CheckLabelsRect(IStiSeriesLabels labels, StiAreaGeom geom, RectangleF labelsRect)
	{
		return CheckLabelsRect(labels, geom.ClientRectangle, labelsRect);
	}

	protected RectangleF CheckLabelsRect(IStiSeriesLabels labels, RectangleF rect, RectangleF labelsRect)
	{
		if (labels != null && labels.PreventIntersection)
		{
			if (labelsRect.X < 0f)
			{
				labelsRect.X = 0f;
			}
			if (labelsRect.Y < 0f)
			{
				labelsRect.Y = 0f;
			}
			if (labelsRect.Right > rect.Width)
			{
				labelsRect.X = rect.Width - labelsRect.Width;
			}
			if (labelsRect.Bottom > rect.Height)
			{
				labelsRect.Y = rect.Height - labelsRect.Height;
			}
			RectangleF drawRectangle = GetDrawRectangle(labelsRect, labels.Angle);
			if (drawRectangle.Y < 0f)
			{
				labelsRect.Y = 25f;
				return labelsRect;
			}
			if (drawRectangle.Y + drawRectangle.Height > rect.Height)
			{
				labelsRect.Y -= drawRectangle.Height / 2f;
			}
		}
		return labelsRect;
	}

	private RectangleF GetDrawRectangle(RectangleF labelsRect, double angle)
	{
		PointF pointToRotate = new PointF(labelsRect.Left, labelsRect.Top);
		PointF pointToRotate2 = new PointF(labelsRect.Right, labelsRect.Top);
		PointF pointToRotate3 = new PointF(labelsRect.Right, labelsRect.Bottom);
		PointF pointToRotate4 = new PointF(labelsRect.Left, labelsRect.Bottom);
		PointF centerPoint = new PointF((labelsRect.Left + labelsRect.Right) / 2f, (labelsRect.Top + labelsRect.Bottom) / 2f);
		PointF pointF = RotatePoint(pointToRotate, centerPoint, angle);
		PointF pointF2 = RotatePoint(pointToRotate2, centerPoint, angle);
		PointF pointF3 = RotatePoint(pointToRotate3, centerPoint, angle);
		PointF pointF4 = RotatePoint(pointToRotate4, centerPoint, angle);
		float num = Math.Min(Math.Min(pointF.Y, pointF2.Y), Math.Min(pointF3.Y, pointF4.Y));
		float num2 = Math.Max(Math.Max(pointF.Y, pointF2.Y), Math.Max(pointF3.Y, pointF4.Y));
		float num3 = Math.Min(Math.Min(pointF.X, pointF2.X), Math.Min(pointF3.X, pointF4.X));
		float num4 = Math.Max(Math.Max(pointF.X, pointF2.X), Math.Max(pointF3.X, pointF4.X));
		return new RectangleF(num3, num, num4 - num3, num2 - num);
	}

	private PointF RotatePoint(PointF pointToRotate, PointF centerPoint, double angleInDegrees)
	{
		double num = angleInDegrees * (Math.PI / 180.0);
		double num2 = Math.Cos(num);
		double num3 = Math.Sin(num);
		PointF result = default(PointF);
		result.X = (float)(num2 * (double)(pointToRotate.X - centerPoint.X) - num3 * (double)(pointToRotate.Y - centerPoint.Y) + (double)centerPoint.X);
		result.Y = (float)(num3 * (double)(pointToRotate.X - centerPoint.X) + num2 * (double)(pointToRotate.Y - centerPoint.Y) + (double)centerPoint.Y);
		return result;
	}

	protected virtual void CheckIntersectionLabels(StiAreaGeom geom)
	{
		List<StiCellGeom> childGeoms = geom.ChildGeoms;
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		if (childGeoms != null)
		{
			foreach (StiCellGeom item in childGeoms)
			{
				if (item is StiSeriesLabelsGeom)
				{
					list.Add((StiSeriesLabelsGeom)item);
				}
			}
		}
		int count = list.Count;
		bool flag = true;
		int num = 0;
		while (flag && num < 29)
		{
			num++;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (j == i)
					{
						continue;
					}
					RectangleF drawRectangle = GetDrawRectangle(list[i].ClientRectangle, list[i].SeriesLabels.Angle);
					RectangleF drawRectangle2 = GetDrawRectangle(list[j].ClientRectangle, list[j].SeriesLabels.Angle);
					bool flag2 = false;
					bool flag3 = false;
					float num2 = drawRectangle.Height - Math.Abs(drawRectangle2.Y - drawRectangle.Y) + 2f;
					if (drawRectangle.IntersectsWith(drawRectangle2))
					{
						if (drawRectangle.Y > drawRectangle2.Y)
						{
							drawRectangle.Y += num2 / 2f;
							drawRectangle2.Y -= num2 / 2f;
							flag2 = true;
							flag3 = true;
						}
						else
						{
							drawRectangle.Y -= num2 / 2f;
							drawRectangle2.Y += num2 / 2f;
							flag2 = true;
							flag3 = true;
						}
					}
					if (drawRectangle.Y < 0f)
					{
						drawRectangle.Y = 2f;
						flag2 = true;
					}
					if (drawRectangle2.Y < 0f)
					{
						drawRectangle2.Y = 2f;
						flag3 = true;
					}
					if (drawRectangle.Y + drawRectangle.Height > geom.ClientRectangle.Height)
					{
						drawRectangle.Y = geom.ClientRectangle.Height - drawRectangle.Height - 2f;
						flag2 = true;
					}
					if (drawRectangle2.Y + num2 / 2f + drawRectangle2.Height > geom.ClientRectangle.Height)
					{
						drawRectangle2.Y = geom.ClientRectangle.Height - drawRectangle2.Height - 2f;
						flag3 = true;
					}
					if (flag2)
					{
						list[i].ClientRectangle = GetDrawRectangle(drawRectangle, 0f - list[i].SeriesLabels.Angle);
					}
					if (flag3)
					{
						list[j].ClientRectangle = GetDrawRectangle(drawRectangle2, 0f - list[j].SeriesLabels.Angle);
					}
				}
			}
		}
	}

	private RectangleF GetLabelRectangle(float angle, RectangleF rect)
	{
		double num = Math.Pow(Math.Pow(rect.Width, 2.0) + Math.Pow(rect.Height, 2.0), 0.5);
		double num2 = Math.Atan(rect.Height / rect.Width) / Math.PI * 180.0;
		angle += (float)num2;
		return new RectangleF(rect.X, rect.Y, (float)(num * Math.Cos((double)angle * Math.PI / 180.0)), (float)(num * (double)(float)Math.Sin((double)angle * Math.PI / 180.0)));
	}

	public virtual void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesArray)
	{
	}

	public virtual StiBrush GetSeriesBrush(int colorIndex, int colorCount)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		if (Series.Chart == null || Series.Chart.Area == null)
		{
			return null;
		}
		if ((Series.Chart.Area.ColorEach || this is StiBoxAndWhiskerSeriesCoreXF || this is StiDoughnutSeriesCoreXF || this is StiSunburstSeriesCoreXF || this is StiPictorialSeriesCoreXF || this is StiWaterfallSeriesCoreXF) && string.IsNullOrEmpty(((StiSeries)series).AutoSeriesColorDataColumn))
		{
			StiStyleCoreXF stiStyleCoreXF = ((Series.Chart.Style != null) ? Series.Chart.Style.Core : new StiStyleCoreXF29());
			Color colorByIndex = stiStyleCoreXF.GetColorByIndex(colorIndex, colorCount, SeriesColors);
			StiBrush columnBrush = stiStyleCoreXF.GetColumnBrush(colorByIndex);
			if (Series.Chart.Area is IStiClusteredBarArea)
			{
				if (columnBrush is StiGradientBrush)
				{
					StiGradientBrush val = (StiGradientBrush)columnBrush;
					val.Angle += 90.0;
				}
				if (columnBrush is StiGlareBrush)
				{
					StiGlareBrush val2 = (StiGlareBrush)columnBrush;
					val2.Angle += 90.0;
				}
			}
			return columnBrush;
		}
		return null;
	}

	public virtual object GetSeriesBorderColor(int colorIndex, int colorCount)
	{
		if (Series.Chart == null || Series.Chart.Area == null)
		{
			return null;
		}
		if ((Series.Chart.Area.ColorEach && Series.AllowApplyStyle) || this is StiBoxAndWhiskerSeriesCoreXF || this is StiDoughnutSeriesCoreXF || this is StiSunburstSeriesCoreXF)
		{
			StiStyleCoreXF stiStyleCoreXF = ((Series.Chart.Style != null) ? Series.Chart.Style.Core : new StiStyleCoreXF29());
			Color colorByIndex = stiStyleCoreXF.GetColorByIndex(colorIndex, colorCount, SeriesColors);
			return stiStyleCoreXF.GetColumnBorder(colorByIndex);
		}
		return null;
	}

	public IStiAxisSeriesLabels GetSeriesLabels()
	{
		if (Series.ShowSeriesLabels == StiShowSeriesLabels.FromChart)
		{
			return Series.Chart.SeriesLabels as IStiAxisSeriesLabels;
		}
		if (Series.ShowSeriesLabels == StiShowSeriesLabels.FromSeries)
		{
			return Series.SeriesLabels as IStiAxisSeriesLabels;
		}
		return null;
	}

	public string GetTag(int tagIndex)
	{
		if (series.Tags != null && tagIndex < series.Tags.Length && series.Tags[tagIndex] != null)
		{
			return series.Tags[tagIndex].ToString();
		}
		return string.Empty;
	}

	public bool GetIsMouseOverSeriesElement(int seriesIndex)
	{
		if (isMouseOverSeriesElementHashtable == null || !isMouseOverSeriesElementHashtable.ContainsKey(seriesIndex))
		{
			return false;
		}
		return isMouseOverSeriesElementHashtable[seriesIndex];
	}

	public void SetIsMouseOverSeriesElement(int seriesIndex, bool value)
	{
		if (isMouseOverSeriesElementHashtable == null)
		{
			isMouseOverSeriesElementHashtable = new Dictionary<int, bool>();
		}
		isMouseOverSeriesElementHashtable[seriesIndex] = value;
	}

	public StiSeriesCoreXF(IStiSeries series)
	{
		this.series = series;
	}
}
