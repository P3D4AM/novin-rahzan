using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;

namespace Stimulsoft.Report.Chart;

public class StiRangeBarElementGeom : StiSeriesElementGeom
{
	public RectangleF RectFrom { get; }

	public override void Draw(StiContext context)
	{
		IStiRangeBarSeries stiRangeBarSeries = base.Series as IStiRangeBarSeries;
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(stiRangeBarSeries.BorderColor, GetSeriesBorderThickness(context.Options.Zoom));
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
			context.DrawAnimationColumn(base.SeriesBrush, brush, stiPenGeom, rectangleF, base.Value, GetToolTip(), this, animation, GetInteractionData());
			return;
		}
		if (stiRangeBarSeries.ShowShadow && rectangleF.Width > 4f && rectangleF.Height > 4f)
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
		IStiRangeBarSeries stiRangeBarSeries = (IStiRangeBarSeries)base.Series;
		IStiAxisArea stiAxisArea = base.AreaGeom.Area as IStiAxisArea;
		int num = base.Series.Values.Length;
		if (stiRangeBarSeries.ValuesEnd.Length < num)
		{
			num = stiRangeBarSeries.ValuesEnd.Length;
		}
		int val = stiRangeBarSeries.Arguments.Length;
		int num2 = Math.Min(num, val);
		TimeSpan beginTime = new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / num2 * base.Index);
		int num3 = stiAxisArea.XAxis.Info.StripLines.Count - 1;
		foreach (StiStripLineXF stripLine in stiAxisArea.XAxis.Info.StripLines)
		{
			if (base.Series.Arguments[base.Index].ToString() == stripLine.ValueObject?.ToString())
			{
				break;
			}
			num3--;
		}
		num3 = stiAxisArea.XAxis.Info.StripLines.Count - 1 - num3;
		StiColumnAnimation stiColumnAnimation = new StiColumnAnimation(RectFrom, base.ClientRectangle, StiChartHelper.GlobalDurationElement, beginTime);
		object valueObject = stiAxisArea.XAxis.Info.StripLines[stiAxisArea.ReverseVert ? (stiAxisArea.XAxis.Info.StripLines.Count - 1 - num3) : num3].ValueObject;
		valueObject = ((valueObject == null) ? "" : valueObject.ToString());
		stiColumnAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{valueObject}";
		if (StiColumnAnimation.IsAnimationChangingValues(base.Series, stiColumnAnimation.Id))
		{
			stiColumnAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
		}
		return stiColumnAnimation;
	}

	public StiRangeBarElementGeom(StiAreaGeom areaGeom, double value, int index, IStiSeries series, StiBrush brush, RectangleF clientRectangle, RectangleF rectFrom)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		RectFrom = rectFrom;
	}
}
