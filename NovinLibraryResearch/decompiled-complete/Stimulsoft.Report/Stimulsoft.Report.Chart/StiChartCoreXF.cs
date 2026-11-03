using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiChartCoreXF : ICloneable, IStiApplyStyle
{
	private IStiChart chart;

	internal Dictionary<string, bool> SelectedSeriesElementHashtable;

	public IStiChart Chart
	{
		get
		{
			return chart;
		}
		set
		{
			chart = value;
		}
	}

	internal RectangleF FullRectangle { get; private set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void ApplyStyle(IStiChartStyle style)
	{
		Chart.Style = style;
		if (style != null)
		{
			if (((StiChart)Chart).AllowApplyStyle)
			{
				Chart.Brush = style.Core.ChartBrush;
			}
			if (Chart.Title != null)
			{
				Chart.Title.Core.ApplyStyle(style);
			}
			if (Chart.Area != null)
			{
				Chart.Area.Core.ApplyStyle(style);
			}
			if (Chart.Legend != null)
			{
				Chart.Legend.Core.ApplyStyle(style);
			}
			if (Chart.SeriesLabels != null)
			{
				Chart.SeriesLabels.Core.ApplyStyle(style);
			}
			if (Chart.Series != null)
			{
				Chart.Series.ApplyStyle(style);
			}
			if (Chart.ConstantLines != null)
			{
				Chart.ConstantLines.ApplyStyle(style);
			}
			if (Chart.Strips != null)
			{
				Chart.Strips.ApplyStyle(style);
			}
			if (Chart.Table != null)
			{
				Chart.Table.Core.ApplyStyle(style);
			}
		}
	}

	public StiCellGeom Render(StiContext context, RectangleF rect, bool useMargins)
	{
		StiChartGeom stiChartGeom = new StiChartGeom(rect);
		stiChartGeom.CreateChildGeoms();
		RectangleF fullRectangle;
		if (useMargins)
		{
			float zoom = context.Options.Zoom;
			fullRectangle = new RectangleF(rect.X + (float)chart.HorSpacing * zoom, rect.Y + (float)chart.VertSpacing * zoom, rect.Width - (float)(chart.HorSpacing * 2) * zoom, rect.Height - (float)(chart.VertSpacing * 2) * zoom);
		}
		else
		{
			fullRectangle = rect;
		}
		FullRectangle = fullRectangle;
		if (chart.Title.Core.Render(context, chart.Title, fullRectangle) is StiChartTitleGeom stiChartTitleGeom)
		{
			switch (chart.Title.Dock)
			{
			case StiChartTitleDock.Top:
				fullRectangle.Y += stiChartTitleGeom.ClientRectangle.Height;
				fullRectangle.Height -= stiChartTitleGeom.ClientRectangle.Height;
				break;
			case StiChartTitleDock.Right:
				fullRectangle.Width -= stiChartTitleGeom.ClientRectangle.Width;
				break;
			case StiChartTitleDock.Bottom:
				fullRectangle.Height -= stiChartTitleGeom.ClientRectangle.Height;
				break;
			case StiChartTitleDock.Left:
				fullRectangle.X += stiChartTitleGeom.ClientRectangle.Width;
				fullRectangle.Width -= stiChartTitleGeom.ClientRectangle.Width;
				break;
			}
			stiChartGeom.ChildGeoms.Add(stiChartTitleGeom);
		}
		StiAreaGeom stiAreaGeom = null;
		StiLegendGeom stiLegendGeom = null;
		RectangleF areaRect = fullRectangle;
		if (chart.Legend != null)
		{
			stiAreaGeom = chart.Area.Core.Render(context, areaRect) as StiAreaGeom;
			stiLegendGeom = chart.Legend.Core.Render(context, stiAreaGeom.ClientRectangle) as StiLegendGeom;
			if (stiLegendGeom != null)
			{
				RectangleF legendRect = stiLegendGeom.ClientRectangle;
				SetLegendRect(context, chart, ref fullRectangle, ref areaRect, ref legendRect);
			}
		}
		float num = 0f;
		float num2 = 0f;
		if (chart.Table.Core.ShowTable())
		{
			stiAreaGeom = chart.Area.Core.Render(context, areaRect) as StiAreaGeom;
			num = chart.Table.Core.GetWidthCellLegend(context) + stiAreaGeom.ClientRectangle.Width;
			num2 = chart.Table.Core.GetHeightTable(context, num);
			areaRect.Height -= num2;
		}
		if (chart.Area != null)
		{
			stiAreaGeom = chart.Area.Core.Render(context, areaRect) as StiAreaGeom;
			if (stiAreaGeom != null)
			{
				stiChartGeom.ChildGeoms.Add(stiAreaGeom);
			}
		}
		List<StiCellGeom> seriesElementGeoms = stiChartGeom.GetSeriesElementGeoms();
		for (int i = 0; i < seriesElementGeoms.Count; i++)
		{
			IStiSeriesElement stiSeriesElement = (IStiSeriesElement)seriesElementGeoms[i];
			if (stiSeriesElement != null)
			{
				stiSeriesElement.ElementIndex = i.ToString();
			}
		}
		if (chart.Table.Core.ShowTable())
		{
			StiSeriesCollection series = chart.Series;
			StiAxisArea stiAxisArea = chart.Area as StiAxisArea;
			if (series != null && series.Count > 0)
			{
				float x = areaRect.X;
				if (chart.Table.Core.GetWidthCellLegend(context) < stiAreaGeom.ClientRectangle.X - areaRect.X)
				{
					x = stiAreaGeom.ClientRectangle.X - chart.Table.Core.GetWidthCellLegend(context);
				}
				if (stiAxisArea.ReverseHor && stiAxisArea.YRightAxis.Visible)
				{
					x = stiAreaGeom.ClientRectangle.Left;
				}
				float bottom = stiAreaGeom.ClientRectangle.Bottom;
				if (chart.Table.Chart.Area is IStiClusteredBarArea)
				{
					bottom = areaRect.Bottom;
				}
				RectangleF rect2 = new RectangleF(x, bottom, num, num2);
				StiChartTableGeom item = chart.Table.Core.Render(context, rect2) as StiChartTableGeom;
				stiChartGeom.ChildGeoms.Add(item);
			}
		}
		stiChartGeom.CreateChildGeoms();
		if (stiLegendGeom != null)
		{
			areaRect = stiAreaGeom.ClientRectangle;
			RectangleF legendRect2 = stiLegendGeom.ClientRectangle;
			SetLegendRect(context, chart, ref fullRectangle, ref areaRect, ref legendRect2);
			stiLegendGeom.ClientRectangle = legendRect2;
			stiChartGeom.ChildGeoms.Add(stiLegendGeom);
		}
		return stiChartGeom;
	}

	private void SetLegendRect(StiContext context, IStiChart chart, ref RectangleF fullRectangle, ref RectangleF areaRect, ref RectangleF legendRect)
	{
		float num = (float)chart.HorSpacing * context.Options.Zoom;
		float num2 = (float)chart.VertSpacing * context.Options.Zoom;
		switch (chart.Legend.HorAlignment)
		{
		case StiLegendHorAlignment.LeftOutside:
			legendRect.X = fullRectangle.X;
			areaRect.Width -= legendRect.Width + num;
			areaRect.X += legendRect.Width + num;
			break;
		case StiLegendHorAlignment.Left:
			legendRect.X = areaRect.X + num;
			break;
		case StiLegendHorAlignment.Center:
			legendRect.X = areaRect.X + (areaRect.Width - legendRect.Width) / 2f;
			break;
		case StiLegendHorAlignment.Right:
			legendRect.X = areaRect.Right - num - legendRect.Width;
			if (chart.Legend.ShowShadow && chart.Legend.VertAlignment == StiLegendVertAlignment.BottomOutside)
			{
				legendRect.X -= 5f;
				areaRect.Width -= 5f;
			}
			break;
		case StiLegendHorAlignment.RightOutside:
			legendRect.X = fullRectangle.Right - legendRect.Width;
			areaRect.Width -= legendRect.Width + num;
			if (chart.Legend.ShowShadow)
			{
				legendRect.X -= 5f;
				areaRect.Width -= 5f;
			}
			break;
		}
		switch (chart.Legend.VertAlignment)
		{
		case StiLegendVertAlignment.TopOutside:
			legendRect.Y = fullRectangle.Y;
			areaRect.Height -= legendRect.Height + num2;
			areaRect.Y += legendRect.Height + num2;
			break;
		case StiLegendVertAlignment.Top:
			legendRect.Y = areaRect.Y + num2;
			break;
		case StiLegendVertAlignment.Center:
			legendRect.Y = areaRect.Y + (areaRect.Height - legendRect.Height) / 2f;
			break;
		case StiLegendVertAlignment.Bottom:
			legendRect.Y = areaRect.Bottom - num2 - legendRect.Height;
			if (chart.Legend.ShowShadow && (chart.Legend.HorAlignment == StiLegendHorAlignment.LeftOutside || chart.Legend.HorAlignment == StiLegendHorAlignment.RightOutside))
			{
				legendRect.Y -= 5f;
				areaRect.Height -= 5f;
			}
			break;
		case StiLegendVertAlignment.BottomOutside:
			legendRect.Y = fullRectangle.Bottom - legendRect.Height;
			areaRect.Height -= legendRect.Height + num2;
			if (chart.Legend.ShowShadow)
			{
				legendRect.Y -= 5f;
				areaRect.Height -= 5f;
			}
			break;
		}
		if (chart.Legend.VertAlignment == StiLegendVertAlignment.BottomOutside || chart.Legend.VertAlignment == StiLegendVertAlignment.TopOutside)
		{
			if (chart.Legend.HorAlignment == StiLegendHorAlignment.Left)
			{
				legendRect.X -= num;
			}
			if (chart.Legend.HorAlignment == StiLegendHorAlignment.Right)
			{
				legendRect.X += num;
			}
		}
		else if (chart.Legend.HorAlignment == StiLegendHorAlignment.RightOutside || chart.Legend.HorAlignment == StiLegendHorAlignment.LeftOutside)
		{
			if (chart.Legend.VertAlignment == StiLegendVertAlignment.Top)
			{
				legendRect.Y -= num2;
			}
			if (chart.Legend.VertAlignment == StiLegendVertAlignment.Bottom)
			{
				legendRect.Y += num2;
			}
		}
	}

	public bool GetIsSelectedSeriesElement(string key)
	{
		if (SelectedSeriesElementHashtable == null || !SelectedSeriesElementHashtable.ContainsKey(key))
		{
			return false;
		}
		return SelectedSeriesElementHashtable[key];
	}

	public void SetIsSelectedSeriesElement(string key, bool value)
	{
		if (SelectedSeriesElementHashtable == null)
		{
			SelectedSeriesElementHashtable = new Dictionary<string, bool>();
		}
		SelectedSeriesElementHashtable[key] = value;
	}

	public StiChartCoreXF(IStiChart chart)
	{
		this.chart = chart;
	}
}
