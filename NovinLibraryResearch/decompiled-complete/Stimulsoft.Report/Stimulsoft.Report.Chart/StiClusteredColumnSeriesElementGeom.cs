using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiClusteredColumnSeriesElementGeom : StiSeriesElementGeom
{
	public Color SeriesBorderColor { get; }

	public RectangleF ColumnRectStart { get; }

	public override void Draw(StiContext context)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		StiChart stiChart = base.Series.Chart as StiChart;
		RectangleF rectangleF = base.ClientRectangle;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			bool? flag = ((stiChart.Area is IStiRoundValuesArea stiRoundValuesArea) ? new bool?(stiRoundValuesArea.RoundValues) : null);
			StiFontIconsExHelper.DrawDirectionIcons(context, base.SeriesBrush, rectangleF, new SizeF(rectangleF.Width, rectangleF.Width), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip(), verticalDirection: true, flag.GetValueOrDefault());
			return;
		}
		StiAnimation animation = GetAnimation();
		StiPenGeom stiPenGeom = new StiPenGeom(SeriesBorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		if (animation != null)
		{
			if (base.Series.ShowShadow && rectangleF.Width > 1f)
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
		base.Draw(context);
		if (base.Series.ShowShadow && rectangleF.Width > 4f && rectangleF.Height > 4f)
		{
			if (base.Value > 0.0)
			{
				context.DrawCachedShadow(rectangleF, (StiShadowSides)3, context.Options.IsPrinting);
			}
			else if (base.Value < 0.0)
			{
				context.DrawCachedShadow(new RectangleF(rectangleF.X, rectangleF.Y - 8f, rectangleF.Width, rectangleF.Height + 8f), (StiShadowSides)30, context.Options.IsPrinting, rectangleF);
			}
		}
		StiBrush brush2 = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
		base.Series.Chart.Style.Core.FillColumn(context, rectangleF, brush2, GetInteractionData());
		if (IsMouseOver || base.Series.Core.IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetMouseOverColor(), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		PointF[] array = null;
		if (base.Series is StiWaterfallSeries)
		{
			context.DrawRectangle(stiPenGeom, rectangleF);
			return;
		}
		array = ((!(base.Value > 0.0)) ? new PointF[4]
		{
			new PointF(rectangleF.X, rectangleF.Y),
			new PointF(rectangleF.X, rectangleF.Bottom),
			new PointF(rectangleF.Right, rectangleF.Bottom),
			new PointF(rectangleF.Right, rectangleF.Y)
		} : new PointF[4]
		{
			new PointF(rectangleF.X, rectangleF.Bottom),
			new PointF(rectangleF.X, rectangleF.Y),
			new PointF(rectangleF.Right, rectangleF.Y),
			new PointF(rectangleF.Right, rectangleF.Bottom)
		});
		context.DrawLines(stiPenGeom, array);
	}

	protected StiAnimation GetAnimation()
	{
		if (!((StiChart)base.Series.Chart).IsAnimation)
		{
			return null;
		}
		IStiAxisArea stiAxisArea = base.AreaGeom.Area as IStiAxisArea;
		TimeSpan beginTime = new TimeSpan(StiChartHelper.GlobalBeginTimeElement.Ticks / ((StiAxisAreaCoreXF)stiAxisArea.Core).ValuesCount * base.Index);
		StiColumnAnimation stiColumnAnimation = new StiColumnAnimation(ColumnRectStart, base.ClientRectangle, StiChartHelper.GlobalDurationElement, beginTime);
		object obj = (stiAxisArea.XAxis.Core.GetStartFromZero() ? stiAxisArea.XAxis.Info.StripLines[base.Index + 1].ValueObject : stiAxisArea.XAxis.Info.StripLines[base.Index].ValueObject);
		obj = ((obj == null) ? "" : obj.ToString());
		stiColumnAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{obj}";
		if (StiColumnAnimation.IsAnimationChangingValues(base.Series, stiColumnAnimation.Id))
		{
			stiColumnAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
		}
		return stiColumnAnimation;
	}

	public StiClusteredColumnSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush seriesBrush, Color seriesBorderColor, IStiSeries series, RectangleF columnRect, RectangleF columnRectStart)
		: base(areaGeom, value, index, series, columnRect, seriesBrush)
	{
		SeriesBorderColor = seriesBorderColor;
		ColumnRectStart = columnRectStart;
	}
}
