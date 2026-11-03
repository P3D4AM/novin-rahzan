using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;

namespace Stimulsoft.Report.Chart;

public class StiPieSeriesFullElementGeom : StiSeriesElementGeom
{
	private StiBrush brush;

	private Color borderColor;

	public StiBrush Brush => brush;

	public Color BorderColor => borderColor;

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		StiPenGeom pen = new StiPenGeom(BorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		context.PushSmoothingModeToAntiAlias();
		StiBrush val = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(Brush) : Brush);
		if (((StiChart)base.Series.Chart).IsAnimation)
		{
			TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
			TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
			StiOpacityAnimation animation = new StiOpacityAnimation(globalDurationElement, globalBeginTimeElement);
			context.FillDrawAnimationEllipse(val, pen, rect.X, rect.Y, rect.Width, rect.Height, GetToolTip(), this, animation, GetInteractionData());
		}
		else
		{
			context.FillEllipse(val, rect);
			if (IsMouseOver || base.Series.Core.IsMouseOver)
			{
				context.FillEllipse(StiMouseOverHelper.GetMouseOverColor(), rect);
			}
			context.DrawEllipse(pen, rect);
		}
		context.PopSmoothingMode();
	}

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		PointF pointF = new PointF(base.ClientRectangle.X + base.ClientRectangle.Width / 2f, base.ClientRectangle.Y + base.ClientRectangle.Height / 2f);
		float num = x - pointF.X;
		float num2 = y - pointF.Y;
		float num3 = (float)Math.Sqrt(num * num + num2 * num2);
		return num3 <= base.ClientRectangle.Width / 2f;
	}

	public StiPieSeriesFullElementGeom(StiAreaGeom areaGeom, double value, int index, IStiPieSeries series, RectangleF clientRectangle, StiBrush brush, Color borderColor)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		this.brush = brush;
		this.borderColor = borderColor;
	}
}
