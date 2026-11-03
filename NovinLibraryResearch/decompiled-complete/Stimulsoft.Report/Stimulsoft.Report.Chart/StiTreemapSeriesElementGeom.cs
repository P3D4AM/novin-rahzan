using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiTreemapSeriesElementGeom : StiSeriesElementGeom
{
	private Color seriesBorderColor;

	private StiAnimation animation;

	public Color SeriesBorderColor => seriesBorderColor;

	public StiAnimation Animation => animation;

	public override void Draw(StiContext context)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		StiChart stiChart = base.Series.Chart as StiChart;
		RectangleF rectangleF = base.ClientRectangle;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			float num = (float)(30.0 * StiScale.Factor);
			context.PushClip(rectangleF);
			StiFontIconsExHelper.DrawFillIcons(context, base.SeriesBrush, rectangleF, new SizeF(num, num), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip());
			context.PopClip();
			return;
		}
		StiPenGeom pen = new StiPenGeom(SeriesBorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		if (stiChart.IsAnimation)
		{
			context.DrawAnimationRectangle(base.SeriesBrush, pen, rectangleF, this, animation, GetInteractionData(), GetToolTip());
			return;
		}
		base.Draw(context);
		StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
		base.Series.Chart.Style.Core.FillColumn(context, rectangleF, brush, GetInteractionData());
		if (IsMouseOver || base.Series.Core.IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetMouseOverColor(), rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
		context.DrawRectangle(pen, rectangleF);
	}

	public StiTreemapSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, StiBrush seriesBrush, Color seriesBorderColor, IStiSeries series, RectangleF clientRectangle, StiAnimation animation)
		: base(areaGeom, value, index, series, clientRectangle, seriesBrush)
	{
		this.seriesBorderColor = seriesBorderColor;
		this.animation = animation;
	}
}
