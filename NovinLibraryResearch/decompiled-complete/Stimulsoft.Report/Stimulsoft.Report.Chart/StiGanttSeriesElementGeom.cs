using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiGanttSeriesElementGeom : StiSeriesElementGeom
{
	public RectangleF RectFrom { get; }

	public override void Draw(StiContext context)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		IStiGanttSeries stiGanttSeries = base.Series as IStiGanttSeries;
		RectangleF rectangleF = base.ClientRectangle;
		StiChart stiChart = base.Series.Chart as StiChart;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			bool? flag = ((stiChart.Area is IStiRoundValuesArea stiRoundValuesArea) ? new bool?(stiRoundValuesArea.RoundValues) : null);
			StiFontIconsExHelper.DrawDirectionIcons(context, base.SeriesBrush, rectangleF, new SizeF(rectangleF.Height, rectangleF.Height), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip(), verticalDirection: false, flag.GetValueOrDefault());
			return;
		}
		StiPenGeom stiPenGeom = new StiPenGeom(stiGanttSeries.BorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		StiAnimation animation = GetAnimation();
		if (animation != null)
		{
			if (base.Series.ShowShadow)
			{
				IStiAxisArea stiAxisArea = base.AreaGeom.Area as IStiAxisArea;
				TimeSpan timeSpan = new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / ((StiAxisAreaCoreXF)stiAxisArea.Core).ValuesCount * base.Index);
				StiOpacityAnimation animation2 = new StiOpacityAnimation(StiChartHelper.GlobalDurationElement, new TimeSpan(timeSpan.Ticks + StiChartHelper.GlobalDurationElement.Ticks));
				context.DrawShadowRect(rectangleF, 5, animation2);
			}
			StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
			context.DrawAnimationBar(base.SeriesBrush, brush, stiPenGeom, rectangleF, base.Value, GetToolTip(), this, animation, GetInteractionData());
			return;
		}
		base.Draw(context);
		if (stiGanttSeries.ShowShadow && rectangleF.Width > 4f && rectangleF.Height > 4f)
		{
			context.DrawCachedShadow(rectangleF, (StiShadowSides)31, context.Options.IsPrinting);
		}
		StiBrush brush2 = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
		context.FillRectangle(brush2, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height, GetInteractionData());
		if (IsMouseOver || base.Series.Core.IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetMouseOverColor(), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		context.DrawRectangle(stiPenGeom, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
	}

	protected StiAnimation GetAnimation()
	{
		if (!((StiChart)base.Series.Chart).IsAnimation)
		{
			return null;
		}
		IStiAxisArea stiAxisArea = base.AreaGeom.Area as IStiAxisArea;
		TimeSpan beginTime = new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / ((StiAxisAreaCoreXF)stiAxisArea.Core).ValuesCount * base.Index);
		StiColumnAnimation stiColumnAnimation = new StiColumnAnimation(RectFrom, base.ClientRectangle, StiChartHelper.GlobalDurationElement, beginTime);
		bool startFromZero = stiAxisArea.YAxis.Core.GetStartFromZero();
		object obj = null;
		if (startFromZero && stiAxisArea.YAxis.Info.StripLines.Count > base.Index + 1)
		{
			obj = stiAxisArea.YAxis.Info.StripLines[base.Index + 1].ValueObject;
		}
		else if (stiAxisArea.YAxis.Info.StripLines.Count > base.Index)
		{
			obj = stiAxisArea.YAxis.Info.StripLines[base.Index].ValueObject;
		}
		obj = ((obj == null) ? "" : obj.ToString());
		stiColumnAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{obj}";
		if (StiColumnAnimation.IsAnimationChangingValues(base.Series, stiColumnAnimation.Id))
		{
			stiColumnAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
		}
		return stiColumnAnimation;
	}

	public StiGanttSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, IStiSeries series, RectangleF clientRectangle, RectangleF rectFrom, StiBrush brush)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		RectFrom = rectFrom;
	}
}
