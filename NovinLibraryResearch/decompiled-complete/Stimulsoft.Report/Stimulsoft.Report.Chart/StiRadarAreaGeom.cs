using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiRadarAreaGeom : StiAreaGeom
{
	private int valuesCount;

	public int ValuesCount => valuesCount;

	private void DrawHor(StiContext context, bool fill, bool draw)
	{
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		if (stiRadarArea.YAxis.Info.StripPositions == null)
		{
			return;
		}
		StiRadarAreaCoreXF stiRadarAreaCoreXF = stiRadarArea.Core as StiRadarAreaCoreXF;
		int num = 0;
		float[] stripPositions = stiRadarArea.YAxis.Info.StripPositions;
		foreach (float num2 in stripPositions)
		{
			if (num2 != 0f)
			{
				RectangleF rect = new RectangleF(stiRadarAreaCoreXF.CenterPoint.X - num2, stiRadarAreaCoreXF.CenterPoint.Y - num2, num2 * 2f, num2 * 2f);
				if (stiRadarArea.RadarStyle == StiRadarStyle.Circle)
				{
					if (fill && stiRadarArea.InterlacingHor.Visible && num != stiRadarArea.YAxis.Info.StripPositions.Length - 1 && (num & 1) == 0)
					{
						List<StiSegmentGeom> list = new List<StiSegmentGeom>();
						float num3 = stiRadarArea.YAxis.Info.StripPositions[num + 1];
						RectangleF rect2 = new RectangleF(stiRadarAreaCoreXF.CenterPoint.X - num3, stiRadarAreaCoreXF.CenterPoint.Y - num3, num3 * 2f, num3 * 2f);
						list.Add(new StiArcSegmentGeom(rect, 0f, 360f));
						list.Add(new StiArcSegmentGeom(rect2, 0f, 360f));
						context.FillPath(stiRadarArea.InterlacingHor.InterlacedBrush, list, rectangleF);
					}
					if (draw && stiRadarArea.GridLinesHor.Visible)
					{
						StiPenGeom stiPenGeom;
						if (num == stiRadarArea.YAxis.Info.StripPositions.Length - 1)
						{
							stiPenGeom = new StiPenGeom(stiRadarArea.BorderColor);
						}
						else
						{
							stiPenGeom = new StiPenGeom(stiRadarArea.GridLinesHor.Color);
							stiPenGeom.PenStyle = stiRadarArea.GridLinesHor.Style;
						}
						context.DrawEllipse(stiPenGeom, rect);
					}
				}
				else
				{
					if (fill && stiRadarArea.InterlacingHor.Visible && num != stiRadarArea.YAxis.Info.StripPositions.Length - 1 && (num & 1) == 1)
					{
						List<StiSegmentGeom> list2 = new List<StiSegmentGeom>();
						int num4 = 0;
						foreach (PointF point in stiRadarAreaCoreXF.Points)
						{
							PointF pointF = ((num4 != stiRadarAreaCoreXF.Points.Count - 1) ? stiRadarAreaCoreXF.Points[num4 + 1] : stiRadarAreaCoreXF.Points[0]);
							PointF start = new PointF(stiRadarAreaCoreXF.CenterPoint.X + num2 * point.X, stiRadarAreaCoreXF.CenterPoint.Y + num2 * point.Y);
							PointF end = new PointF(stiRadarAreaCoreXF.CenterPoint.X + num2 * pointF.X, stiRadarAreaCoreXF.CenterPoint.Y + num2 * pointF.Y);
							list2.Add(new StiLineSegmentGeom(start, end));
							num4++;
						}
						float num5 = stiRadarArea.YAxis.Info.StripPositions[num + 1];
						num4 = 0;
						foreach (PointF point2 in stiRadarAreaCoreXF.Points)
						{
							PointF pointF2 = ((num4 != stiRadarAreaCoreXF.Points.Count - 1) ? stiRadarAreaCoreXF.Points[num4 + 1] : stiRadarAreaCoreXF.Points[0]);
							PointF start2 = new PointF(stiRadarAreaCoreXF.CenterPoint.X + num5 * point2.X, stiRadarAreaCoreXF.CenterPoint.Y + num5 * point2.Y);
							PointF end2 = new PointF(stiRadarAreaCoreXF.CenterPoint.X + num5 * pointF2.X, stiRadarAreaCoreXF.CenterPoint.Y + num5 * pointF2.Y);
							list2.Add(new StiLineSegmentGeom(start2, end2));
							num4++;
						}
						context.FillPath(stiRadarArea.InterlacingHor.InterlacedBrush, list2, rectangleF);
					}
					if (draw && stiRadarArea.GridLinesHor.Visible)
					{
						int num6 = 0;
						foreach (PointF point3 in stiRadarAreaCoreXF.Points)
						{
							PointF pointF3 = ((num6 != stiRadarAreaCoreXF.Points.Count - 1) ? stiRadarAreaCoreXF.Points[num6 + 1] : stiRadarAreaCoreXF.Points[0]);
							StiPenGeom stiPenGeom2;
							if (num == stiRadarArea.YAxis.Info.StripPositions.Length - 1)
							{
								stiPenGeom2 = new StiPenGeom(stiRadarArea.BorderColor);
							}
							else
							{
								stiPenGeom2 = new StiPenGeom(stiRadarArea.GridLinesHor.Color);
								stiPenGeom2.PenStyle = stiRadarArea.GridLinesHor.Style;
							}
							context.DrawLine(stiPenGeom2, stiRadarAreaCoreXF.CenterPoint.X + num2 * point3.X, stiRadarAreaCoreXF.CenterPoint.Y + num2 * point3.Y, stiRadarAreaCoreXF.CenterPoint.X + num2 * pointF3.X, stiRadarAreaCoreXF.CenterPoint.Y + num2 * pointF3.Y);
							num6++;
						}
					}
				}
			}
			num++;
		}
	}

	private void DrawVert(StiContext context, bool fill, bool draw)
	{
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		StiRadarAreaCoreXF stiRadarAreaCoreXF = stiRadarArea.Core as StiRadarAreaCoreXF;
		int num = 0;
		float num2 = 360f / (float)stiRadarAreaCoreXF.Points.Count;
		float num3 = 0f;
		foreach (PointF point in stiRadarAreaCoreXF.Points)
		{
			PointF centerPoint = stiRadarAreaCoreXF.CenterPoint;
			PointF pointF = new PointF(centerPoint.X + rectangleF.Width / 2f * point.X, centerPoint.Y + rectangleF.Width / 2f * point.Y);
			if (fill && stiRadarArea.InterlacingVert.Visible)
			{
				PointF pointF2 = ((num < stiRadarAreaCoreXF.Points.Count - 1) ? new PointF(centerPoint.X + rectangleF.Width / 2f * stiRadarAreaCoreXF.Points[num + 1].X, centerPoint.Y + rectangleF.Height / 2f * stiRadarAreaCoreXF.Points[num + 1].Y) : new PointF(centerPoint.X + rectangleF.Width / 2f * stiRadarAreaCoreXF.Points[0].X, centerPoint.Y + rectangleF.Height / 2f * stiRadarAreaCoreXF.Points[0].Y));
				if ((num & 1) == 1)
				{
					List<StiSegmentGeom> list = new List<StiSegmentGeom>();
					if (stiRadarArea.RadarStyle == StiRadarStyle.Circle)
					{
						list.Add(new StiLineSegmentGeom(centerPoint, pointF));
						list.Add(new StiArcSegmentGeom(rectangleF, num3 - 90f, num2));
						list.Add(new StiLineSegmentGeom(pointF2, centerPoint));
					}
					else
					{
						list.Add(new StiLineSegmentGeom(centerPoint, pointF));
						list.Add(new StiLineSegmentGeom(pointF, pointF2));
						list.Add(new StiLineSegmentGeom(pointF2, centerPoint));
					}
					context.FillPath(stiRadarArea.InterlacingVert.InterlacedBrush, list, rectangleF);
				}
			}
			if (draw && stiRadarArea.GridLinesVert.Visible)
			{
				StiPenGeom stiPenGeom = new StiPenGeom(stiRadarArea.GridLinesVert.Color);
				stiPenGeom.PenStyle = stiRadarArea.GridLinesVert.Style;
				context.DrawLine(stiPenGeom, centerPoint.X, centerPoint.Y, pointF.X, pointF.Y);
			}
			num3 += num2;
			num++;
		}
	}

	private void DrawBackground(StiContext context)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		RectangleF rectangleF = base.ClientRectangle;
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		if (stiRadarArea.YAxis.Info.StripPositions == null)
		{
			return;
		}
		StiRadarAreaCoreXF stiRadarAreaCoreXF = stiRadarArea.Core as StiRadarAreaCoreXF;
		int num = 3;
		StiSolidBrush brush = new StiSolidBrush(Color.FromArgb(50, 0, 0, 0));
		if (stiRadarArea.RadarStyle == StiRadarStyle.Circle)
		{
			if (base.Area.ShowShadow && !(stiRadarArea.Brush is StiEmptyBrush) && !StiBrush.IsTransparent(stiRadarArea.Brush))
			{
				context.PushTranslateTransform(num, num);
				context.FillEllipse(brush, rectangleF);
				context.PopTransform();
			}
			context.FillEllipse(stiRadarArea.Brush, rectangleF);
			return;
		}
		float num2 = stiRadarArea.YAxis.Info.StripPositions[stiRadarArea.YAxis.Info.StripPositions.Length - 1];
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		int num3 = 0;
		foreach (PointF point in stiRadarAreaCoreXF.Points)
		{
			PointF pointF = ((num3 == stiRadarAreaCoreXF.Points.Count - 1) ? stiRadarAreaCoreXF.Points[0] : stiRadarAreaCoreXF.Points[num3 + 1]);
			list.Add(new StiLineSegmentGeom(stiRadarAreaCoreXF.CenterPoint.X + num2 * point.X, stiRadarAreaCoreXF.CenterPoint.Y + num2 * point.Y, stiRadarAreaCoreXF.CenterPoint.X + num2 * pointF.X, stiRadarAreaCoreXF.CenterPoint.Y + num2 * pointF.Y));
			num3++;
		}
		if (base.Area.ShowShadow && !(stiRadarArea.Brush is StiEmptyBrush) && !StiBrush.IsTransparent(stiRadarArea.Brush))
		{
			context.PushTranslateTransform(num, num);
			context.FillPath(brush, list, rectangleF);
			context.PopTransform();
		}
		context.FillPath(stiRadarArea.Brush, list, rectangleF);
	}

	public override void Draw(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		IStiRadarArea stiRadarArea = base.Area as IStiRadarArea;
		StiRadarAreaCoreXF stiRadarAreaCoreXF = stiRadarArea.Core as StiRadarAreaCoreXF;
		if (rectangleF.Width > 0f && rectangleF.Height > 0f)
		{
			context.PushSmoothingModeToAntiAlias();
			DrawBackground(context);
			DrawVert(context, fill: true, draw: false);
			DrawHor(context, fill: true, draw: false);
			DrawVert(context, fill: false, draw: true);
			DrawHor(context, fill: false, draw: true);
			context.PopSmoothingMode();
		}
	}

	public StiRadarAreaGeom(IStiArea area, RectangleF clientRectangle, int valuesCount)
		: base(area, clientRectangle)
	{
		this.valuesCount = valuesCount;
	}
}
