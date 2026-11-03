using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;

namespace Stimulsoft.Report.Chart;

public class StiWaterfallLineGeom : StiCellGeom
{
	public StiPenGeom Pen { get; private set; }

	public PointF PointStart { get; private set; }

	public PointF PointEnd { get; private set; }

	public bool Animation { get; }

	public override void Draw(StiContext context)
	{
		if (Animation)
		{
			StiTranslationAnimation animation = new StiTranslationAnimation(StiChartHelper.GlobalDurationElement, TimeSpan.Zero);
			context.DrawAnimationLines(Pen, new PointF[2] { PointStart, PointEnd }, animation);
		}
		else
		{
			context.DrawLine(Pen, PointStart.X, PointStart.Y, PointEnd.X, PointEnd.Y);
		}
	}

	public StiWaterfallLineGeom(PointF pointStart, PointF pointEnd, StiPenGeom pen, RectangleF clientRectangle, bool animation)
		: base(clientRectangle)
	{
		Pen = pen;
		PointStart = pointStart;
		PointEnd = pointEnd;
		Animation = animation;
	}
}
