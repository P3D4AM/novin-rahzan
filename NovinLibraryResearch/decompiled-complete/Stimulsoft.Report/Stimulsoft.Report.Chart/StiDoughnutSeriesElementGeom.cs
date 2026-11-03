using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiDoughnutSeriesElementGeom : StiSeriesElementGeom
{
	public RectangleF ClientRectangleDt { get; }

	public List<StiSegmentGeom> Path { get; }

	public List<StiSegmentGeom> PathLight { get; }

	public List<StiSegmentGeom> PathDark { get; }

	public Color BorderColor { get; }

	public StiBrush Brush { get; }

	public StiBrush BrushLight { get; }

	public StiBrush BrushDark { get; }

	public float StartAngle { get; }

	public float EndAngle { get; }

	public float RadiusFrom { get; }

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
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(BorderColor, GetSeriesBorderThickness(context.Options.Zoom));
		stiPenGeom.Alignment = StiPenAlignment.Inset;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			float num = (float)(30.0 * StiScale.Factor);
			context.PushClipPath(Path);
			StiFontIconsExHelper.DrawFillIcons(context, base.SeriesBrush, rectangleF, new SizeF(num, num), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip());
			context.PopClip();
			return;
		}
		context.PushSmoothingModeToAntiAlias();
		StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(Brush) : Brush);
		StiAnimation animation = GetAnimation();
		if (animation != null)
		{
			context.DrawAnimationPathElement(Brush, brush, stiPenGeom, Path, rectangleF, GetToolTip(), this, animation, GetInteractionData());
			if (PathLight != null)
			{
				context.DrawAnimationPathElement(BrushLight, null, PathLight, rectangleF, GetToolTip(), null, animation, GetInteractionData());
			}
			if (PathDark != null)
			{
				context.DrawAnimationPathElement(BrushDark, null, PathDark, rectangleF, GetToolTip(), null, animation, GetInteractionData());
			}
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
			if (PathLight != null)
			{
				context.FillPath(BrushLight, PathLight, rectangleF);
			}
			if (PathDark != null)
			{
				context.FillPath(BrushDark, PathDark, rectangleF);
			}
			if (!Color.Transparent.Equals((object?)BorderColor))
			{
				context.DrawPath(stiPenGeom, Path, null);
			}
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
			stiPieSegmentAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{base.Index}";
			if (StiPieSegmentAnimation.IsAnimationChangingValues(base.Series, stiPieSegmentAnimation.Id))
			{
				stiPieSegmentAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
				stiAnimation = stiPieSegmentAnimation;
			}
		}
		if (stiAnimation == null)
		{
			TimeSpan beginTime = new TimeSpan(globalBeginTimeElement.Ticks / base.Series.Values.Length * base.Index);
			if (base.Series.Chart.PreviousAnimations.Count > 0)
			{
				beginTime = TimeSpan.FromMilliseconds(300.0);
			}
			StiOpacityAnimation stiOpacityAnimation = new StiOpacityAnimation(globalDurationElement, beginTime);
			stiOpacityAnimation.AnotherAnimation = stiPieSegmentAnimation;
			stiOpacityAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{base.Index}";
			stiAnimation = stiOpacityAnimation;
		}
		return stiAnimation;
	}

	public StiDoughnutSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, IStiDoughnutSeries series, RectangleF clientRectangle, RectangleF clientRectangleDt, List<StiSegmentGeom> path, List<StiSegmentGeom> pathLight, List<StiSegmentGeom> pathDark, Color borderColor, StiBrush brush, StiBrush brushLight, StiBrush brushDark, float startAngle, float endAngle, float radiusFrom, float radiusTo, TimeSpan beginTime)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		ClientRectangleDt = clientRectangleDt;
		Path = path;
		PathLight = pathLight;
		PathDark = pathDark;
		BorderColor = borderColor;
		Brush = brush;
		BrushLight = brushLight;
		BrushDark = brushDark;
		StartAngle = startAngle;
		EndAngle = endAngle;
		RadiusFrom = radiusFrom;
		RadiusTo = radiusTo;
		BeginTime = beginTime;
	}
}
