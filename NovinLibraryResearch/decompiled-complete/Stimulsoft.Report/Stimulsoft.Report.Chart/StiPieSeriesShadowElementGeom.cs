using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiPieSeriesShadowElementGeom : StiCellGeom
{
	private IStiPieSeries series;

	private StiContext shadowContext;

	private float radius;

	private TimeSpan duration;

	private TimeSpan beginTime;

	private bool isAnimation;

	public override bool Invisible => true;

	public IStiPieSeries Series => series;

	public StiContext ShadowContext => shadowContext;

	public float Radius => radius;

	public TimeSpan Duration => duration;

	public TimeSpan BeginTime => beginTime;

	public bool IsAnimation => isAnimation;

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (isAnimation)
		{
			StiOpacityAnimation animation = new StiOpacityAnimation(duration, beginTime);
			context.DrawShadowRect(rect, rect.Height / 2f, rect.Width / 2f, (int)radius, animation);
		}
		else
		{
			context.DrawShadow(shadowContext, rect, radius);
		}
	}

	public StiPieSeriesShadowElementGeom(IStiPieSeries series, RectangleF clientRectangle, float radius, StiContext shadowContext, TimeSpan duration, TimeSpan beginTime)
		: base(clientRectangle)
	{
		this.series = series;
		this.shadowContext = shadowContext;
		this.radius = radius;
		this.duration = duration;
		this.beginTime = beginTime;
		isAnimation = ((StiChart)series.Chart).IsAnimation;
	}
}
