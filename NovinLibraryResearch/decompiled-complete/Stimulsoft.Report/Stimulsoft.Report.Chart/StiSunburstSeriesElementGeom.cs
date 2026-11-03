using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;

namespace Stimulsoft.Report.Chart;

public class StiSunburstSeriesElementGeom : StiSeriesElementGeom
{
	private int index2;

	private int index3;

	public RectangleF ClientRectangleDt { get; }

	public float StartAngle { get; }

	public float EndAngle { get; }

	public float RadiusFrom { get; }

	public List<StiSegmentGeom> Path { get; }

	public Color BorderColor { get; }

	public StiBrush Brush { get; }

	public float RadiusTo { get; }

	public TimeSpan BeginTime { get; }

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
		if (num3 < RadiusTo || num3 > RadiusFrom)
		{
			return false;
		}
		float num4 = (float)(Math.Atan2(num2, num) * 180.0 / Math.PI);
		if (num4 < 0f)
		{
			num4 += 360f;
		}
		if (num4 >= StartAngle)
		{
			return num4 <= EndAngle;
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(BorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		stiPenGeom.Alignment = StiPenAlignment.Inset;
		context.PushSmoothingModeToAntiAlias();
		StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(Brush) : Brush);
		StiAnimation animation = GetAnimation();
		if (animation != null)
		{
			context.DrawAnimationPathElement(brush, stiPenGeom, Path, rectangleF, GetToolTip(), null, animation, GetInteractionData());
		}
		else
		{
			if (Path != null)
			{
				context.FillPath(brush, Path, rectangleF, GetInteractionData());
				if (IsMouseOver || base.Series.Core.IsMouseOver)
				{
					context.FillPath(StiMouseOverHelper.GetMouseOverColor(), Path, rectangleF);
				}
			}
			context.DrawPath(stiPenGeom, Path, StiPathGeom.GetBoundsState);
		}
		context.PopSmoothingMode();
	}

	private StiAnimation GetAnimation()
	{
		if (!((StiChart)base.Series.Chart).IsAnimation)
		{
			return null;
		}
		TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
		TimeSpan globalBeginTimeElement = StiChartHelper.GlobalBeginTimeElement;
		StiAnimation stiAnimation = null;
		StiPieSegmentAnimation stiPieSegmentAnimation = null;
		if (((StiChart)base.Series.Chart).IsAnimationChangingValues)
		{
			float startAngle = StartAngle;
			float endAngleTo = EndAngle;
			RectangleF rect = base.ClientRectangle;
			RectangleF rect2 = base.ClientRectangle;
			if (Path.Count == 4 && Path[0] is StiArcSegmentGeom stiArcSegmentGeom && Path[2] is StiArcSegmentGeom stiArcSegmentGeom2)
			{
				startAngle = stiArcSegmentGeom.StartAngle;
				endAngleTo = stiArcSegmentGeom.StartAngle + stiArcSegmentGeom.SweepAngle;
				rect = stiArcSegmentGeom.Rect;
				rect2 = stiArcSegmentGeom2.Rect;
			}
			stiPieSegmentAnimation = new StiPieSegmentAnimation(base.ClientRectangle, rect, ClientRectangleDt, rect2, StartAngle, EndAngle, startAngle, endAngleTo, globalDurationElement, TimeSpan.Zero);
			stiPieSegmentAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{base.Index}_{index2}_{index3}";
			if (StiPieSegmentAnimation.IsAnimationChangingValues(base.Series, stiPieSegmentAnimation.Id))
			{
				stiPieSegmentAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
				stiAnimation = stiPieSegmentAnimation;
			}
		}
		if (stiAnimation == null)
		{
			TimeSpan beginTime = BeginTime;
			if (base.Series.Chart.PreviousAnimations.Count > 0)
			{
				beginTime = TimeSpan.FromMilliseconds(300.0);
			}
			StiOpacityAnimation stiOpacityAnimation = new StiOpacityAnimation(globalDurationElement, beginTime);
			stiOpacityAnimation.AnotherAnimation = stiPieSegmentAnimation;
			stiOpacityAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{base.Index}_{index2}_{index3}";
			stiAnimation = stiOpacityAnimation;
		}
		return stiAnimation;
	}

	public StiSunburstSeriesElementGeom(StiAreaGeom areaGeom, double value, int index1, int index2, int index3, IStiSeries series, RectangleF clientRectangle, RectangleF clientRectangleDt, List<StiSegmentGeom> path, Color borderColor, StiBrush brush, float startAngle, float endAngle, float radiusFrom, float radiusTo, TimeSpan beginTime)
		: base(areaGeom, value, index1, series, clientRectangle, brush)
	{
		this.index2 = index2;
		this.index3 = index3;
		ClientRectangleDt = clientRectangleDt;
		Path = path;
		BorderColor = borderColor;
		Brush = brush;
		StartAngle = startAngle;
		EndAngle = endAngle;
		RadiusFrom = radiusFrom;
		RadiusTo = radiusTo;
		BeginTime = beginTime;
	}
}
