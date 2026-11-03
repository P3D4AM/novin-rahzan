using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart.Geoms.Series.Pie;

public class StiPie3dSidesSeriesElementGeom : StiSeriesElementGeom
{
	private static float s_shadowAngle = 20f;

	private StiPenGeom pen;

	private float startnAngle;

	private float sweepAngle;

	private RectangleF m_boundingRectangle;

	private float m_sliceHeight;

	private float m_startAngle;

	private float m_sweepAngle;

	private PointF m_center;

	private PointF m_centerBelow;

	private PointF m_pointStart;

	private PointF m_pointStartBelow;

	private PointF m_pointEnd;

	private PointF m_pointEndBelow;

	private StiBrush m_brushStartSide;

	private StiBrush m_brushEndSide;

	private StiBrush m_brushPeripherySurface;

	private StiBrush m_brushSurface;

	private StiPie3dQuadrilateral m_startSide;

	private StiPie3dQuadrilateral m_endSide;

	private int count;

	public float StartAngle => m_startAngle;

	public float EndAngle => (m_startAngle + m_sweepAngle) % 360f;

	public override StiSeriesInteractionData Interaction => Pie3DSlice.Interaction;

	public StiPie3dSlice Pie3DSlice { get; private set; }

	public override bool Contains(float px, float py)
	{
		PointF point = new PointF(px, py);
		if (PeripheryContainsPoint(point))
		{
			return true;
		}
		if (m_startSide.Contains(point))
		{
			return true;
		}
		if (m_endSide.Contains(point))
		{
			return true;
		}
		return false;
	}

	internal bool PeripheryContainsPoint(PointF point)
	{
		List<StiPie3dPeripherySurfaceBounds> visiblePeripherySurfaceBounds = GetVisiblePeripherySurfaceBounds();
		foreach (StiPie3dPeripherySurfaceBounds item in visiblePeripherySurfaceBounds)
		{
			if (CylinderSurfaceSectionContainsPoint(point, item.StartAngle, item.EndAngle, item.StartPoint, item.EndPoint))
			{
				return true;
			}
		}
		return false;
	}

	private bool CylinderSurfaceSectionContainsPoint(PointF point, float startAngle, float endAngle, PointF point1, PointF point2)
	{
		if (m_sliceHeight > 0f)
		{
			return StiPie3dQuadrilateral.Contains(point, new PointF[4]
			{
				point1,
				new PointF(point1.X, point1.Y + m_sliceHeight),
				new PointF(point2.X, point2.Y + m_sliceHeight),
				point2
			});
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		context.PushSmoothingModeToAntiAlias();
		DrawHiddenPeriphery(context);
		if (StartAngle > 90f && StartAngle < 270f)
		{
			DrawEndSide(context);
			DrawStartSide(context);
		}
		else
		{
			DrawStartSide(context);
			DrawEndSide(context);
		}
		DrawVisiblePeriphery(context);
		context.PopSmoothingMode();
	}

	private void DrawStartSide(StiContext context)
	{
		if (m_startSide != null)
		{
			if (StartAngle > 90f && StartAngle < 270f)
			{
				m_startSide.Draw(context, pen, m_brushStartSide, IsMouseOver);
			}
			else
			{
				m_startSide.Draw(context, pen, m_brushSurface, IsMouseOver);
			}
		}
	}

	private void DrawEndSide(StiContext context)
	{
		if (m_endSide != null)
		{
			if (EndAngle > 90f && EndAngle < 270f)
			{
				m_endSide.Draw(context, pen, m_brushSurface, IsMouseOver);
			}
			else
			{
				m_endSide.Draw(context, pen, m_brushEndSide, IsMouseOver);
			}
		}
	}

	private void DrawVisiblePeriphery(StiContext context)
	{
		List<StiPie3dPeripherySurfaceBounds> visiblePeripherySurfaceBounds = GetVisiblePeripherySurfaceBounds();
		foreach (StiPie3dPeripherySurfaceBounds item in visiblePeripherySurfaceBounds)
		{
			DrawCylinderSurfaceSection(context, pen, m_brushPeripherySurface, item);
		}
	}

	private void DrawHiddenPeriphery(StiContext context)
	{
		List<StiPie3dPeripherySurfaceBounds> hiddenPeripherySurfaceBounds = GetHiddenPeripherySurfaceBounds();
		foreach (StiPie3dPeripherySurfaceBounds item in hiddenPeripherySurfaceBounds)
		{
			DrawCylinderSurfaceSection(context, pen, m_brushSurface, item);
		}
	}

	private List<StiPie3dPeripherySurfaceBounds> GetHiddenPeripherySurfaceBounds()
	{
		List<StiPie3dPeripherySurfaceBounds> list = new List<StiPie3dPeripherySurfaceBounds>();
		if (m_sweepAngle != 0f && (!(m_startAngle >= 0f) || !(m_startAngle + m_sweepAngle <= 180f)) && m_startAngle + m_sweepAngle > 180f)
		{
			float num = m_startAngle;
			PointF startPoint = new PointF(m_pointStart.X, m_pointStart.Y);
			float num2 = m_startAngle + m_sweepAngle;
			PointF endPoint = new PointF(m_pointEnd.X, m_pointEnd.Y);
			float realStartAngle = startnAngle;
			float realEndAngle = startnAngle + sweepAngle;
			if (num < 180f)
			{
				num = (realStartAngle = 180f);
				startPoint.X = m_boundingRectangle.Left;
				startPoint.Y = m_center.Y;
			}
			if (num2 > 360f)
			{
				num2 = (realEndAngle = 360f);
				endPoint.X = m_boundingRectangle.Right;
				endPoint.Y = m_center.Y;
			}
			list.Add(new StiPie3dPeripherySurfaceBounds(num, num2, startPoint, endPoint, realStartAngle, realEndAngle));
			if (m_startAngle < 360f && m_startAngle + m_sweepAngle > 540f)
			{
				num = 180f;
				startPoint = new PointF(m_boundingRectangle.Left, m_center.Y);
				num2 = EndAngle;
				endPoint = new PointF(m_pointEnd.X, m_pointEnd.Y);
				list.Add(new StiPie3dPeripherySurfaceBounds(num, num2, startPoint, endPoint, realStartAngle, realEndAngle));
			}
		}
		return list;
	}

	private List<StiPie3dPeripherySurfaceBounds> GetVisiblePeripherySurfaceBounds()
	{
		List<StiPie3dPeripherySurfaceBounds> list = new List<StiPie3dPeripherySurfaceBounds>();
		if (m_sweepAngle != 0f && (!(m_startAngle >= 180f) || !(m_startAngle + m_sweepAngle <= 360f)))
		{
			if (StartAngle < 180f)
			{
				float startAngle = m_startAngle;
				PointF startPoint = new PointF(m_pointStart.X, m_pointStart.Y);
				float endAngle = EndAngle;
				PointF endPoint = new PointF(m_pointEnd.X, m_pointEnd.Y);
				float realStartAngle = startnAngle;
				float realEndAngle = startnAngle + sweepAngle;
				if (m_startAngle + m_sweepAngle > 180f)
				{
					endAngle = (realEndAngle = 180f);
					endPoint.X = m_boundingRectangle.X;
					endPoint.Y = m_center.Y;
				}
				list.Add(new StiPie3dPeripherySurfaceBounds(startAngle, endAngle, startPoint, endPoint, realStartAngle, realEndAngle));
			}
			if (m_startAngle + m_sweepAngle > 360f)
			{
				int num = 0;
				PointF startPoint2 = new PointF(m_boundingRectangle.Right, m_center.Y);
				float num2 = EndAngle;
				PointF endPoint2 = new PointF(m_pointEnd.X, m_pointEnd.Y);
				float realStartAngle2 = startnAngle;
				float realEndAngle2 = startnAngle + sweepAngle;
				if (num2 > 180f)
				{
					num2 = (realEndAngle2 = 180f);
					endPoint2.X = m_boundingRectangle.Left;
					endPoint2.Y = m_center.Y;
				}
				list.Add(new StiPie3dPeripherySurfaceBounds(num, num2, startPoint2, endPoint2, realStartAngle2, realEndAngle2));
			}
		}
		return list;
	}

	protected void DrawCylinderSurfaceSection(StiContext context, StiPenGeom pen, StiBrush brush, StiPie3dPeripherySurfaceBounds peripherySurfaceBounds)
	{
		brush = (base.IsSelected ? StiSelectedHelper.GetSelectedBrush(brush) : brush);
		List<StiSegmentGeom> list = CreatePathForCylinderSurfaceSection(peripherySurfaceBounds);
		RectangleF pathBounds = context.GetPathBounds(list);
		StiAnimation animation = GetAnimation();
		if (animation != null)
		{
			context.DrawAnimationPathElement(brush, brush, pen, list, pathBounds, GetToolTip(), this, animation, GetInteractionData());
		}
		else
		{
			context.FillPath(brush, list, pathBounds);
			context.DrawPath(pen, list, pathBounds);
		}
		if (IsMouseOver)
		{
			context.FillPath(StiMouseOverHelper.GetMouseOverColor(), list, base.ClientRectangle);
		}
	}

	public PointF GetTextPosition()
	{
		if (sweepAngle >= 180f)
		{
			return PeripheralPoint(m_center.X, m_center.Y, m_boundingRectangle.Width / 3f, m_boundingRectangle.Height / 3f, GetActualAngle(StartAngle) + sweepAngle / 2f);
		}
		float num = (m_pointStart.X + m_pointEnd.X) / 2f;
		float num2 = (m_pointStart.Y + m_pointEnd.Y) / 2f;
		float transformedAngle = (float)(Math.Atan2(num2 - m_center.Y, num - m_center.X) * 180.0 / Math.PI);
		return PeripheralPoint(m_center.X, m_center.Y, m_boundingRectangle.Width / 3f, m_boundingRectangle.Height / 3f, GetActualAngle(transformedAngle));
	}

	protected float GetActualAngle(float transformedAngle)
	{
		double x = (double)m_boundingRectangle.Height * Math.Cos((double)transformedAngle * Math.PI / 180.0);
		double y = (double)m_boundingRectangle.Width * Math.Sin((double)transformedAngle * Math.PI / 180.0);
		float num = (float)(Math.Atan2(y, x) * 180.0 / Math.PI);
		if (num < 0f)
		{
			return num + 360f;
		}
		return num;
	}

	private List<StiSegmentGeom> CreatePathForCylinderSurfaceSection(StiPie3dPeripherySurfaceBounds peripherySurfaceBounds)
	{
		float startAngle = peripherySurfaceBounds.StartAngle;
		float endAngle = peripherySurfaceBounds.EndAngle;
		PointF startPoint = peripherySurfaceBounds.StartPoint;
		PointF endPoint = peripherySurfaceBounds.EndPoint;
		float realStartAngle = peripherySurfaceBounds.RealStartAngle;
		float realEndAngle = peripherySurfaceBounds.RealEndAngle;
		float realSweepAngle = peripherySurfaceBounds.RealEndAngle - peripherySurfaceBounds.RealStartAngle;
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiArcSegmentGeom(m_boundingRectangle, startAngle, endAngle - startAngle, realStartAngle, realSweepAngle));
		list.Add(new StiLineSegmentGeom(endPoint.X, endPoint.Y, endPoint.X, endPoint.Y + m_sliceHeight));
		RectangleF rect = new RectangleF(m_boundingRectangle.X, m_boundingRectangle.Y + m_sliceHeight, m_boundingRectangle.Width, m_boundingRectangle.Height);
		list.Add(new StiArcSegmentGeom(rect, endAngle, startAngle - endAngle, realEndAngle, realStartAngle - realEndAngle));
		list.Add(new StiLineSegmentGeom(startPoint.X, startPoint.Y + m_sliceHeight, startPoint.X, startPoint.Y));
		return list;
	}

	private void InitializePieSlice(float xBoundingRect, float yBoundingRect, float widthBoundingRect, float heightBoundingRect, float sliceHeight, StiPie3dLightingStyle shadowStyle)
	{
		m_boundingRectangle = new RectangleF(xBoundingRect, yBoundingRect, widthBoundingRect, heightBoundingRect);
		m_sliceHeight = sliceHeight;
		float num = startnAngle;
		m_startAngle = TransformAngle(startnAngle);
		m_sweepAngle = sweepAngle;
		if (m_sweepAngle % 180f != 0f)
		{
			m_sweepAngle = TransformAngle(num + sweepAngle) - m_startAngle;
		}
		if (m_sweepAngle < 0f)
		{
			m_sweepAngle += 360f;
		}
		CreateSurfaceBrushes(shadowStyle);
		float num2 = xBoundingRect + widthBoundingRect / 2f;
		float num3 = yBoundingRect + heightBoundingRect / 2f;
		m_center = new PointF(num2, num3);
		m_centerBelow = new PointF(num2, num3 + sliceHeight);
		m_pointStart = PeripheralPoint(num2, num3, widthBoundingRect / 2f, heightBoundingRect / 2f, num);
		m_pointStartBelow = new PointF(m_pointStart.X, m_pointStart.Y + sliceHeight);
		m_pointEnd = PeripheralPoint(num2, num3, widthBoundingRect / 2f, heightBoundingRect / 2f, num + sweepAngle);
		m_pointEndBelow = new PointF(m_pointEnd.X, m_pointEnd.Y + sliceHeight);
		InitializeSides(startSideExists: true, endSideExists: true);
	}

	private void InitializeSides(bool startSideExists, bool endSideExists)
	{
		StiAnimation animation = GetAnimation();
		if (startSideExists)
		{
			m_startSide = new StiPie3dQuadrilateral(m_boundingRectangle, m_center, m_pointStart, m_pointStartBelow, m_centerBelow, m_sweepAngle != 180f, animation);
		}
		else
		{
			m_startSide = StiPie3dQuadrilateral.Empty;
		}
		if (endSideExists)
		{
			m_endSide = new StiPie3dQuadrilateral(m_boundingRectangle, m_center, m_pointEnd, m_pointEndBelow, m_centerBelow, m_sweepAngle != 180f, animation);
		}
		else
		{
			m_endSide = StiPie3dQuadrilateral.Empty;
		}
	}

	protected virtual void CreateSurfaceBrushes(StiPie3dLightingStyle shadowStyle)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0084: Expected O, but got Unknown
		Color color = StiBrush.ToColor(base.SeriesBrush);
		if (base.IsSelected)
		{
			StiSolidBrush val = new StiSolidBrush(color);
			m_brushStartSide = (m_brushEndSide = (m_brushPeripherySurface = StiBrush.Light(m_brushSurface = (StiBrush)(base.IsSelected ? ((object)StiSelectedHelper.GetSelectedBrush((StiBrush)(object)val)) : ((object)val)), (byte)30)));
			return;
		}
		m_brushSurface = (StiBrush)new StiSolidBrush(color);
		switch (shadowStyle)
		{
		case StiPie3dLightingStyle.No:
		{
			StiSolidBrush val2 = new StiSolidBrush(color);
			StiBrush brushEndSide = (StiBrush)val2;
			m_brushPeripherySurface = (StiBrush)val2;
			m_brushStartSide = (m_brushEndSide = brushEndSide);
			break;
		}
		case StiPie3dLightingStyle.Solid:
			m_brushStartSide = (m_brushEndSide = (m_brushPeripherySurface = StiBrush.Light(base.SeriesBrush, (byte)30)));
			break;
		case StiPie3dLightingStyle.Gradient:
		{
			double num = m_startAngle - 180f - s_shadowAngle;
			if (num < 0.0)
			{
				num += 360.0;
			}
			m_brushStartSide = CreateBrushForSide(color, num);
			num = m_startAngle + m_sweepAngle - s_shadowAngle;
			if (num < 0.0)
			{
				num += 360.0;
			}
			m_brushEndSide = CreateBrushForSide(color, num);
			m_brushPeripherySurface = CreateBrushForPeriphery(color);
			break;
		}
		}
	}

	protected virtual StiBrush CreateBrushForSide(Color color, double angle)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		return (StiBrush)new StiSolidBrush(StiPie3dHelper.CreateColorWithCorrectedLightness(color, 0f - (float)((double)StiPie3dHelper.BrightnessEnhancementFactor1 * (1.0 - 0.8 * Math.Cos(angle * Math.PI / 180.0)))));
	}

	protected virtual StiBrush CreateBrushForPeriphery(Color color)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		return (StiBrush)new StiGlareBrush(color, CreateColorWithCorrectedLightness(color, 0f - StiPie3dHelper.BrightnessEnhancementFactor1), 90.0, 0.1f, 1f);
	}

	public static Color CreateColorWithCorrectedLightness(Color color, float correctionFactor)
	{
		if (correctionFactor == 0f)
		{
			return color;
		}
		float num = (int)color.R;
		float num2 = (int)color.G;
		float num3 = (int)color.B;
		if (correctionFactor < 0f)
		{
			correctionFactor = 1f + correctionFactor;
			num *= correctionFactor;
			num2 *= correctionFactor;
			num3 *= correctionFactor;
		}
		else
		{
			num = (255f - num) * correctionFactor + num;
			num2 = (255f - num2) * correctionFactor + num2;
			num3 = (255f - num3) * correctionFactor + num3;
		}
		return Color.FromArgb(color.A, (int)num, (int)num2, (int)num3);
	}

	protected PointF PeripheralPoint(float xCenter, float yCenter, float semiMajor, float semiMinor, float angleDegrees)
	{
		double num = (double)angleDegrees * Math.PI / 180.0;
		return new PointF(xCenter + (float)((double)semiMajor * Math.Cos(num)), yCenter + (float)((double)semiMinor * Math.Sin(num)));
	}

	protected float TransformAngle(float angle)
	{
		double x = (double)m_boundingRectangle.Width * Math.Cos((double)angle * Math.PI / 180.0);
		double y = (double)m_boundingRectangle.Height * Math.Sin((double)angle * Math.PI / 180.0);
		float num = (float)(Math.Atan2(y, x) * 180.0 / Math.PI);
		if (num < 0f)
		{
			return num + 360f;
		}
		return num;
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

	public StiPie3dSidesSeriesElementGeom(StiPie3dSlice pie3DSlice, StiAreaGeom areaGeom, double value, int index, int count, IStiPie3dSeries series, RectangleF clientRectangle, Color borderColor, StiBrush brush, float startAngle, float sweepAngle, float pieHeight)
		: base(areaGeom, value, index, series, clientRectangle, brush)
	{
		Pie3DSlice = pie3DSlice;
		this.count = count;
		startnAngle = startAngle;
		pen = new StiPenGeom(borderColor);
		this.sweepAngle = sweepAngle;
		InitializePieSlice(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height, pieHeight, series.Options3D.Lighting);
	}
}
