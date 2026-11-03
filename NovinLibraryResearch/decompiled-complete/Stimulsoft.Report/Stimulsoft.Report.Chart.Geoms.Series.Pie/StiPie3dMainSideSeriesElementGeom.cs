using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart.Geoms.Series.Pie;

public class StiPie3dMainSideSeriesElementGeom : StiSeriesElementGeom
{
	private Color borderColor;

	private float startAngle;

	private float sweepAngle;

	private float realStartAngle;

	private float realSweepAngle;

	private int count;

	public override StiSeriesInteractionData Interaction => Pie3DSlice.Interaction;

	public StiPie3dSlice Pie3DSlice { get; private set; }

	public override bool Contains(float px, float py)
	{
		if (Invisible)
		{
			return false;
		}
		double num = px - base.ClientRectangle.X - base.ClientRectangle.Width / 2f;
		double num2 = py - base.ClientRectangle.Y - base.ClientRectangle.Height / 2f;
		double num3 = Math.Atan2(num2, num);
		if (num3 < 0.0)
		{
			num3 += Math.PI * 2.0;
		}
		double num4 = num3 * 180.0 / Math.PI;
		if ((num4 >= (double)startAngle && num4 <= (double)(startAngle + sweepAngle)) || (startAngle + sweepAngle > 360f && num4 + 360.0 <= (double)(startAngle + sweepAngle)))
		{
			double num5 = Math.Sqrt(num2 * num2 + num * num);
			return GetEllipseRadius(num3) > num5;
		}
		return false;
	}

	private double GetEllipseRadius(double angle)
	{
		double num = base.ClientRectangle.Width / 2f;
		double num2 = base.ClientRectangle.Height / 2f;
		double num3 = num * num;
		double num4 = num2 * num2;
		double num5 = Math.Cos(angle);
		double num6 = Math.Sin(angle);
		return num * num2 / Math.Sqrt(num4 * num5 * num5 + num3 * num6 * num6);
	}

	public override void Draw(StiContext context)
	{
		StiPenGeom stiPenGeom = new StiPenGeom(borderColor, 1f);
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiPieSegmentGeom(new RectangleF(base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.Width, base.ClientRectangle.Height), startAngle, sweepAngle, realStartAngle, realSweepAngle, null));
		context.PushSmoothingModeToAntiAlias();
		StiBrush val = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(base.SeriesBrush) : base.SeriesBrush);
		StiAnimation animation = GetAnimation();
		if (animation != null)
		{
			context.DrawAnimationPathElement(val, val, stiPenGeom, list, base.ClientRectangle, GetToolTip(), this, animation, GetInteractionData());
		}
		else
		{
			context.FillPath(val, list, base.ClientRectangle);
			context.DrawPath(stiPenGeom, list, base.ClientRectangle);
		}
		if (IsMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, base.ClientRectangle);
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
		StiPieSegmentAnimation anotherAnimation = null;
		if (stiAnimation == null)
		{
			TimeSpan beginTime = new TimeSpan(globalBeginTimeElement.Ticks / count * base.Index);
			if (base.Series.Chart.PreviousAnimations.Count > 0)
			{
				beginTime = TimeSpan.FromMilliseconds(300.0);
			}
			StiOpacityAnimation stiOpacityAnimation = new StiOpacityAnimation(globalDurationElement, beginTime);
			stiOpacityAnimation.AnotherAnimation = anotherAnimation;
			stiOpacityAnimation.Id = $"series_{base.Series.Chart.Series.IndexOf(base.Series)}_{base.Index}";
			stiAnimation = stiOpacityAnimation;
		}
		return stiAnimation;
	}

	public StiPie3dMainSideSeriesElementGeom(StiPie3dSlice pie3DSlice, StiAreaGeom areaGeom, double value, int index, int count, IStiSeries series, RectangleF clientRectangle, Color borderColor, StiBrush brush, float startAngle, float sweepAngle)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		Pie3DSlice = pie3DSlice;
		this.count = count;
		this.borderColor = borderColor;
		realStartAngle = startAngle;
		realSweepAngle = sweepAngle;
		this.startAngle = StiPie3dHelper.TransformAngle(base.ClientRectangle, startAngle);
		this.sweepAngle = sweepAngle;
		this.sweepAngle = sweepAngle;
		if (this.sweepAngle % 180f != 0f)
		{
			this.sweepAngle = StiPie3dHelper.TransformAngle(base.ClientRectangle, startAngle + sweepAngle) - this.startAngle;
		}
		if (this.sweepAngle < 0f)
		{
			this.sweepAngle += 360f;
		}
	}
}
