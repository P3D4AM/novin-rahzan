using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiStackedBarSeriesElementGeom : StiSeriesElementGeom
{
	public Color SeriesBorderColor { get; }

	public RectangleF ColumnRectStart { get; }

	public override void Draw(StiContext context)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(SeriesBorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		StiChart stiChart = base.Series.Chart as StiChart;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			bool? flag = ((stiChart.Area is IStiRoundValuesArea stiRoundValuesArea) ? new bool?(stiRoundValuesArea.RoundValues) : null);
			StiFontIconsExHelper.DrawDirectionIcons(context, base.SeriesBrush, rectangleF, new SizeF(rectangleF.Height, rectangleF.Height), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip(), verticalDirection: false, flag.GetValueOrDefault());
			return;
		}
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
		StiBrush brush2 = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
		base.Series.Chart.Style.Core.FillColumn(context, rectangleF, brush2, GetInteractionData());
		if (IsMouseOver || base.Series.Core.IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetMouseOverColor(), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		PointF[] array = null;
		array = ((!(base.Value > 0.0)) ? new PointF[4]
		{
			new PointF(rectangleF.X, rectangleF.Y),
			new PointF(rectangleF.Right, rectangleF.Y),
			new PointF(rectangleF.Right, rectangleF.Bottom),
			new PointF(rectangleF.X, rectangleF.Bottom)
		} : new PointF[4]
		{
			new PointF(rectangleF.Right, rectangleF.Y),
			new PointF(rectangleF.X, rectangleF.Y),
			new PointF(rectangleF.X, rectangleF.Bottom),
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
		object obj = (stiAxisArea.YAxis.Core.GetStartFromZero() ? stiAxisArea.YAxis.Info.StripLines[base.Index + 1].ValueObject : stiAxisArea.YAxis.Info.StripLines[base.Index].ValueObject);
		obj = ((obj == null) ? "" : obj.ToString());
		stiColumnAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{obj}";
		if (StiColumnAnimation.IsAnimationChangingValues(base.Series, stiColumnAnimation.Id))
		{
			stiColumnAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
		}
		return stiColumnAnimation;
	}

	public StiStackedBarSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush seriesBrush, Color seriesBorderColor, IStiSeries series, RectangleF clientRectangle, RectangleF columnRectStart)
		: base(areaGeom, value, index, series, clientRectangle, seriesBrush)
	{
		SeriesBorderColor = seriesBorderColor;
		ColumnRectStart = columnRectStart;
	}
}
