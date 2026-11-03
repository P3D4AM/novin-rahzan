using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart.Geoms.Series;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiFunnelSeriesElementGeom : StiSeriesElementGeom
{
	public List<StiSegmentGeom> Path { get; }

	public Color BorderColor { get; }

	public StiBrush Brush { get; }

	public TimeSpan BeginTime { get; }

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		StiLinesSegmentGeom stiLinesSegmentGeom = Path.OfType<StiLinesSegmentGeom>().FirstOrDefault();
		if (stiLinesSegmentGeom == null)
		{
			return false;
		}
		return IsPointInPolygon(new PointF(x, y), stiLinesSegmentGeom.Points);
	}

	private bool IsPointInPolygon(PointF p, PointF[] polygon)
	{
		double num = polygon[0].X;
		double num2 = polygon[0].X;
		double num3 = polygon[0].Y;
		double num4 = polygon[0].Y;
		for (int i = 1; i < polygon.Length; i++)
		{
			PointF pointF = polygon[i];
			num = Math.Min(pointF.X, num);
			num2 = Math.Max(pointF.X, num2);
			num3 = Math.Min(pointF.Y, num3);
			num4 = Math.Max(pointF.Y, num4);
		}
		if ((double)p.X < num || (double)p.X > num2 || (double)p.Y < num3 || (double)p.Y > num4)
		{
			return false;
		}
		bool flag = false;
		int num5 = 0;
		int num6 = polygon.Length - 1;
		while (num5 < polygon.Length)
		{
			if (polygon[num5].Y > p.Y != polygon[num6].Y > p.Y && p.X < (polygon[num6].X - polygon[num5].X) * (p.Y - polygon[num5].Y) / (polygon[num6].Y - polygon[num5].Y) + polygon[num5].X)
			{
				flag = !flag;
			}
			num6 = num5++;
		}
		return flag;
	}

	public override void Draw(StiContext context)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(StiColorUtils.Dark(BorderColor, (byte)10), GetSeriesBorderThickness(context.Options.Zoom));
		stiPenGeom.Alignment = StiPenAlignment.Inset;
		StiChart stiChart = base.Series.Chart as StiChart;
		if (base.Series is IStiFontIconsSeries { Icon: not null } stiFontIconsSeries)
		{
			float num = (float)(30.0 * StiScale.Factor);
			context.PushClipPath(Path);
			StiFontIconsExHelper.DrawFillIcons(context, base.SeriesBrush, rectangleF, new SizeF(num, num), stiFontIconsSeries.Icon.GetValueOrDefault(), GetToolTip());
			context.PopClip();
			return;
		}
		if (stiChart.IsAnimation)
		{
			StiOpacityAnimation animation = new StiOpacityAnimation(TimeSpan.FromSeconds(1.0), BeginTime);
			if (base.Series.ShowShadow)
			{
				context.PushTranslateTransform(4f, 4f);
				context.DrawAnimationPathElement(null, (object)new StiSolidBrush(Color.FromArgb(50, 100, 100, 100)), null, Path, rectangleF, null, this, animation);
				context.PopTransform();
			}
			StiBrush brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(Brush) : Brush);
			context.DrawAnimationPathElement(Brush, brush, null, Path, rectangleF, GetToolTip(), this, animation, GetInteractionData());
			if (!Color.Transparent.Equals((object?)BorderColor))
			{
				context.DrawAnimationPathElement(null, null, stiPenGeom, Path, rectangleF, null, this, animation);
			}
			return;
		}
		context.PushSmoothingModeToAntiAlias();
		if (base.Series.ShowShadow)
		{
			context.PushTranslateTransform(4f, 4f);
			context.FillPath(Color.FromArgb(50, 100, 100, 100), Path, rectangleF);
			context.PopTransform();
		}
		StiBrush brush2 = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(Brush) : Brush);
		context.FillPath(brush2, Path, rectangleF, GetInteractionData());
		if (IsMouseOver || base.Series.Core.IsMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), Path, rectangleF);
		}
		if (!Color.Transparent.Equals((object?)BorderColor))
		{
			context.DrawPath(stiPenGeom, Path, null);
		}
		context.PopSmoothingMode();
	}

	public StiFunnelSeriesElementGeom(StiAreaGeom areaGeom, double value, int index, IStiSeries series, RectangleF clientRectangle, StiBrush brush, Color borderColor, List<StiSegmentGeom> path, TimeSpan beginTime)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		Path = path;
		BorderColor = borderColor;
		Brush = brush;
		BeginTime = beginTime;
	}
}
