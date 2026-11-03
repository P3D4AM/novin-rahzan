using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiBubbleSeriesElementGeom : StiSeriesElementGeom
{
	private Color seriesBorderColor;

	private TimeSpan beginTime;

	public Color SeriesBorderColor => seriesBorderColor;

	public TimeSpan BeginTime => beginTime;

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		PointF pointF = new PointF(base.ClientRectangle.X + base.ClientRectangle.Width / 2f, base.ClientRectangle.Y + base.ClientRectangle.Height / 2f);
		float num = Math.Abs(pointF.X - x);
		float num2 = Math.Abs(pointF.Y - y);
		float num3 = (float)Math.Sqrt(num * num + num2 * num2);
		return num3 <= base.ClientRectangle.Width / 2f;
	}

	public override void Draw(StiContext context)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom pen = new StiPenGeom(SeriesBorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		StiChart stiChart = base.Series.Chart as StiChart;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			StiFontIconsExHelper.DrawDirectionIcons(context, base.SeriesBrush, rectangleF, new SizeF(rectangleF.Height, rectangleF.Height), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip(), verticalDirection: false);
			return;
		}
		context.PushSmoothingModeToAntiAlias();
		if (stiChart.IsAnimation)
		{
			StiScaleAnimation animation = new StiScaleAnimation(StiChartHelper.GlobalDurationElement, beginTime);
			StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
			context.FillDrawAnimationEllipse(base.SeriesBrush, brush, pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height, GetToolTip(), this, animation, GetInteractionData());
		}
		else
		{
			if (base.Series.ShowShadow)
			{
				StiSolidBrush brush2 = new StiSolidBrush(Color.FromArgb(100, Color.Black));
				StiContext stiContext = context.CreateShadowGraphics();
				RectangleF rect = rectangleF;
				rect.X = 0f;
				rect.Y = 0f;
				rect.X += 4f * context.Options.Zoom;
				rect.Y += 4f * context.Options.Zoom;
				stiContext.FillEllipse(brush2, rect);
				context.DrawShadow(stiContext, rectangleF, 0f);
			}
			StiBrush brush3 = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
			context.FillEllipse(brush3, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height, GetToolTip(), GetInteractionData());
			if (IsMouseOver || base.Series.Core.IsMouseOver)
			{
				context.FillEllipse(StiMouseOverHelper.GetMouseOverColor(), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height, GetToolTip(), GetInteractionData());
			}
			context.DrawEllipse(pen, rectangleF);
		}
		context.PopSmoothingMode();
	}

	public StiBubbleSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush seriesBrush, Color seriesBorderColor, IStiSeries series, RectangleF clientRectangle, TimeSpan beginTime)
		: base(areaGeom, value, index, series, clientRectangle, seriesBrush)
	{
		this.seriesBorderColor = seriesBorderColor;
		this.beginTime = beginTime;
	}
}
