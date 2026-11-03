using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRadarPointSeriesElementGeom : StiSeriesElementGeom
{
	private PointF point;

	public PointF Point => point;

	public override void InvokeMouseEnter(StiInteractionOptions options)
	{
		if (AllowMouseOver)
		{
			if (!IsMouseOver)
			{
				IsMouseOver = true;
				options.UpdateContext = true;
			}
			int valueIndex = GetValueIndex();
			options.InteractionToolTip = GetToolTip(valueIndex);
			options.InteractionHyperlink = GetHyperlink(valueIndex);
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

	public override void InvokeClick(StiInteractionOptions options)
	{
		int valueIndex = GetValueIndex();
		if (base.Series.Hyperlinks != null && valueIndex < base.Series.Hyperlinks.Length)
		{
			options.InteractionHyperlink = base.Series.Hyperlinks[valueIndex];
		}
		if (base.Series.Interaction.DrillDownEnabled)
		{
			options.SeriesInteractionData = Interaction;
			IsMouseOver = false;
			options.UpdateContext = true;
		}
	}

	private int GetValueIndex()
	{
		int num = base.Index;
		if (base.Series is IStiClusteredBarSeries || base.Series is IStiStackedBarSeries || base.Series is IStiFullStackedBarSeries)
		{
			if (base.Series.Chart.Area is IStiAxisArea && !((IStiAxisArea)base.Series.Chart.Area).ReverseVert)
			{
				num = base.Series.Values.Length - num - 1;
			}
		}
		else if (base.Series.Chart.Area is IStiAxisArea && ((IStiAxisArea)base.Series.Chart.Area).ReverseHor)
		{
			num = base.Series.Values.Length - num - 1;
		}
		return num;
	}

	private string GetHyperlink(int valueIndex)
	{
		if (base.Series.Hyperlinks != null && valueIndex < base.Series.Hyperlinks.Length)
		{
			return base.Series.Hyperlinks[valueIndex];
		}
		return null;
	}

	private string GetToolTip(int valueIndex)
	{
		if (base.Series.ToolTips != null && valueIndex < base.Series.ToolTips.Length)
		{
			return base.Series.ToolTips[valueIndex];
		}
		return null;
	}

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		return GetMouseOverRect().Contains(x, y);
	}

	internal RectangleF GetMouseOverRect()
	{
		RectangleF result = base.ClientRectangle;
		result.Inflate(result.Width / 2f, result.Height / 2f);
		return result;
	}

	public override void Draw(StiContext context)
	{
		IStiRadarSeries stiRadarSeries = base.Series as IStiRadarSeries;
		bool flag = !stiRadarSeries.Marker.Visible && stiRadarSeries.ToolTips.Length != 0;
		if (stiRadarSeries.Marker != null && (stiRadarSeries.Marker.Visible || flag))
		{
			context.PushSmoothingModeToAntiAlias();
			float zoom = context.Options.Zoom;
			if (IsMouseOver)
			{
				context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(stiRadarSeries.Marker.Brush), GetMouseOverRect());
			}
			StiChart stiChart = base.Series.Chart as StiChart;
			StiInteractionDataGeom interaction = (stiChart.IsAnimation ? new StiInteractionDataGeom
			{
				ComponentName = stiChart.Name,
				ComponentIndex = ((stiChart.Page != null) ? stiChart.Page.Components.IndexOf(stiChart).ToString() : ""),
				PageGuid = ((StiSeries)base.Series).DrillDownPageGuid,
				PageIndex = ((stiChart.Page != null) ? stiChart.Page.Report.RenderedPages.IndexOf(stiChart.Page).ToString() : ""),
				ElementIndex = base.ElementIndex.ToString(),
				InteractionData = Interaction
			} : null);
			stiRadarSeries.Marker.Core.Draw(context, stiRadarSeries.Marker, Point, zoom, stiRadarSeries.ShowShadow, IsMouseOver, flag, stiChart.IsAnimation, GetToolTip(), this, interaction);
			context.PopSmoothingMode();
		}
	}

	public StiRadarPointSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush brush, IStiRadarSeries series, PointF point, float zoom)
		: base(areaGeom, value, index, series, StiMarkerCoreXF.GetMarkerRect(point, series.Marker.Size, zoom), brush)
	{
		this.point = point;
	}
}
