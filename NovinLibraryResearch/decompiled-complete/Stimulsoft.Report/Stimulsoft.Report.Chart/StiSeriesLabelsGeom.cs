using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiSeriesLabelsGeom : StiCellGeom
{
	public virtual bool AllowMouseOver
	{
		get
		{
			if (GetHyperlink(GetValueIndex()) == null)
			{
				return Series.Interaction.DrillDownEnabled;
			}
			return true;
		}
	}

	public virtual bool IsMouseOver
	{
		get
		{
			if (Series == null)
			{
				return false;
			}
			return Series.Core.GetIsMouseOverSeriesElement(Index);
		}
		set
		{
			if (Series != null)
			{
				Series.Core.SetIsMouseOverSeriesElement(Index, value);
			}
		}
	}

	public double Value { get; }

	public int Index { get; }

	public IStiSeries Series { get; }

	public IStiSeriesLabels SeriesLabels { get; }

	public TimeSpan BeginTime { get; set; } = TimeSpan.Zero;


	public TimeSpan Duration { get; set; } = TimeSpan.Zero;


	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver && !IsMouseOver)
		{
			IsMouseOver = true;
			options.UpdateContext = true;
		}
	}

	public override void InvokeMouseLeave(StiInteractionOptions options)
	{
		if (AllowMouseOver && IsMouseOver)
		{
			IsMouseOver = false;
			options.UpdateContext = true;
		}
	}

	private int GetValueIndex()
	{
		int num = Index;
		if (Series is IStiClusteredBarSeries || Series is IStiStackedBarSeries || Series is IStiFullStackedBarSeries)
		{
			if (Series.Chart.Area is IStiAxisArea && !((IStiAxisArea)Series.Chart.Area).ReverseVert)
			{
				num = Series.Values.Length - num - 1;
			}
		}
		else if (Series.Chart.Area is IStiAxisArea && ((IStiAxisArea)Series.Chart.Area).ReverseHor)
		{
			num = Series.Values.Length - num - 1;
		}
		return num;
	}

	private string GetHyperlink(int valueIndex)
	{
		if (Series.Hyperlinks != null && valueIndex < Series.Hyperlinks.Length)
		{
			return Series.Hyperlinks[valueIndex];
		}
		return null;
	}

	private string GetToolTip(int valueIndex)
	{
		if (Series.ToolTips != null && valueIndex < Series.ToolTips.Length)
		{
			return Series.Hyperlinks[valueIndex];
		}
		return null;
	}

	protected virtual void DrawMarker(StiContext context, Rectangle itemRect, object markerColor, StiBrush markerBrush)
	{
		if (SeriesLabels.MarkerVisible)
		{
			StiChart stiChart = Series.Chart as StiChart;
			Rectangle empty = Rectangle.Empty;
			switch (SeriesLabels.MarkerAlignment)
			{
			case StiMarkerAlignment.Right:
				empty.X = (int)((float)itemRect.Right + 2f * context.Options.Zoom);
				break;
			case StiMarkerAlignment.Left:
				empty.X = (int)((float)itemRect.Left - (float)(2 + SeriesLabels.MarkerSize.Width) * context.Options.Zoom);
				break;
			case StiMarkerAlignment.Center:
				empty.X = (int)((float)(itemRect.Left + itemRect.Width / 2) - (float)((2 + SeriesLabels.MarkerSize.Width) / 2) * context.Options.Zoom);
				break;
			}
			empty.Y = (int)((float)itemRect.Y + ((float)itemRect.Height - (float)SeriesLabels.MarkerSize.Height * context.Options.Zoom) / 2f);
			empty.Width = (int)((float)SeriesLabels.MarkerSize.Width * context.Options.Zoom);
			empty.Height = (int)((float)SeriesLabels.MarkerSize.Height * context.Options.Zoom);
			Color color = (markerColor as Color?) ?? Color.Black;
			StiPenGeom pen = new StiPenGeom(color, 1f);
			if (stiChart.IsAnimation)
			{
				StiOpacityAnimation animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalBeginTimeElement);
				context.DrawAnimationRectangle(markerBrush, pen, empty, null, animation, null, GetToolTip(GetValueIndex()));
			}
			else
			{
				context.FillRectangle(markerBrush, empty.X, empty.Y, empty.Width, empty.Height);
				context.DrawRectangle(pen, empty.X, empty.Y, empty.Width, empty.Height);
			}
		}
	}

	public override void Draw(StiContext context)
	{
	}

	public StiSeriesLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle)
		: base(Rectangle.Round(clientRectangle))
	{
		SeriesLabels = seriesLabels;
		Series = series;
		Index = index;
		Value = value;
	}
}
