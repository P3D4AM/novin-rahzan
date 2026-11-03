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

public class StiPieSeriesElementGeom : StiSeriesElementGeom
{
	public List<StiSegmentGeom> Path { get; }

	public List<StiSegmentGeom> PathLight { get; }

	public Color BorderColor { get; }

	public float StartAngle { get; }

	public float EndAngle { get; }

	public float Radius { get; }

	public int Count { get; set; }

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
		if (num3 >= Radius)
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
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		RectangleF rectangleF = base.ClientRectangle;
		Color color = (Color.Transparent.Equals((object?)BorderColor) ? BorderColor : StiColorUtils.Dark(BorderColor, (byte)10));
		StiPenGeom stiPenGeom = new StiPenGeom(color, GetSeriesBorderThickness(context.Options.Zoom))
		{
			Alignment = StiPenAlignment.Inset
		};
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			float num = (float)(30.0 * StiScale.Factor);
			context.PushClipPath(Path);
			StiFontIconsExHelper.DrawFillIcons(context, base.SeriesBrush, rectangleF, new SizeF(num, num), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip());
			context.PopClip();
			return;
		}
		context.PushSmoothingModeToAntiAlias();
		StiAnimation animation = GetAnimation();
		if (animation != null)
		{
			StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
			context.DrawAnimationPathElement(base.SeriesBrush, brush, stiPenGeom, Path, rectangleF, GetToolTip(), this, animation, GetInteractionData());
			if (PathLight != null)
			{
				StiSolidBrush brush2 = new StiSolidBrush(Color.FromArgb(30, Color.Black));
				context.DrawAnimationPathElement(brush2, null, PathLight, rectangleF, GetToolTip(), null, animation, GetInteractionData());
			}
		}
		else
		{
			StiBrush brush3 = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
			context.FillPath(brush3, Path, rectangleF, GetInteractionData());
			if (IsMouseOver || base.Series.Core.IsMouseOver)
			{
				context.FillPath(StiMouseOverHelper.GetMouseOverColor(), Path, rectangleF);
			}
			if (PathLight != null)
			{
				StiSolidBrush brush4 = new StiSolidBrush(Color.FromArgb(30, Color.Black));
				context.FillPath(brush4, PathLight, rectangleF);
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
		if (base.Series.Chart.IsAnimationChangingValues)
		{
			float startAngle = StartAngle;
			float endAngleTo = EndAngle;
			RectangleF rect = base.ClientRectangle;
			if (Path[0] is StiPieSegmentGeom stiPieSegmentGeom)
			{
				startAngle = stiPieSegmentGeom.StartAngle;
				endAngleTo = stiPieSegmentGeom.StartAngle + stiPieSegmentGeom.SweepAngle;
				rect = stiPieSegmentGeom.Rect;
			}
			stiPieSegmentAnimation = new StiPieSegmentAnimation(base.ClientRectangle, rect, StartAngle, EndAngle, startAngle, endAngleTo, globalDurationElement, TimeSpan.Zero);
			stiPieSegmentAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{base.Index}";
			if (StiPieSegmentAnimation.IsAnimationChangingValues(base.Series, stiPieSegmentAnimation.Id))
			{
				stiPieSegmentAnimation.ApplyPreviousAnimation(base.Series.Chart.PreviousAnimations);
				stiAnimation = stiPieSegmentAnimation;
			}
		}
		if (stiAnimation == null)
		{
			TimeSpan beginTime = new TimeSpan(globalBeginTimeElement.Ticks / Count * base.Index);
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

	public StiPieSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, IStiPieSeries series, RectangleF clientRectangle, List<StiSegmentGeom> path, List<StiSegmentGeom> pathLight, Color borderColor, StiBrush brush, float startAngle, float endAngle, float radius)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		Path = path;
		PathLight = pathLight;
		BorderColor = borderColor;
		StartAngle = startAngle;
		EndAngle = endAngle;
		Radius = radius;
	}
}
