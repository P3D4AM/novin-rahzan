using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiSteppedLineSeriesGeom : StiBaseLineSeriesGeom
{
	protected PointF?[] GetSteppedPoints(PointF?[] points)
	{
		IStiSteppedLineSeries stiSteppedLineSeries = base.Series as IStiSteppedLineSeries;
		List<PointF?> list = new List<PointF?>();
		float num = (float)((IStiAxisArea)stiSteppedLineSeries.Chart.Area).XAxis.Info.Dpi / 2f;
		for (int i = 0; i < points.Length - 1; i++)
		{
			PointF? item = points[i];
			PointF? pointF = points[i + 1];
			if (item.HasValue && pointF.HasValue)
			{
				if (stiSteppedLineSeries.PointAtCenter)
				{
					item = new PointF(item.Value.X - num, item.Value.Y);
					pointF = new PointF(pointF.Value.X - num, pointF.Value.Y);
				}
				list.Add(item);
				list.Add(new PointF(pointF.Value.X, item.Value.Y));
			}
			else
			{
				list.Add(null);
				list.Add(null);
			}
		}
		PointF? item2 = points[^1];
		if (item2.HasValue)
		{
			if (stiSteppedLineSeries.PointAtCenter)
			{
				item2 = new PointF(item2.Value.X - num, item2.Value.Y);
				list.Add(item2);
				item2 = new PointF(item2.Value.X + num * 2f, item2.Value.Y);
				list.Add(item2);
			}
			else
			{
				list.Add(item2);
			}
		}
		else
		{
			list.Add(null);
		}
		return list.ToArray();
	}

	protected string[] GetSteppedPointsIds(string[] ids)
	{
		IStiSteppedLineSeries stiSteppedLineSeries = base.Series as IStiSteppedLineSeries;
		List<string> list = new List<string>();
		if (stiSteppedLineSeries.PointAtCenter && ids.Length != 0)
		{
			list.Add(ids[0] + "_h");
		}
		for (int i = 0; i < ids.Length - 1; i++)
		{
			string item = ids[i] + "_l";
			string item2 = ids[i + 1] + "_h";
			list.Add(item);
			list.Add(item2);
		}
		string item3 = ids[^1] + "_l";
		list.Add(item3);
		return list.ToArray();
	}

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		PointF?[] steppedPoints = GetSteppedPoints(base.Points);
		int num = 0;
		PointF?[] array = steppedPoints;
		for (int i = 0; i < array.Length; i++)
		{
			PointF? pointF = array[i];
			if (base.Points.Length <= num + 1)
			{
				continue;
			}
			PointF? pointF2 = steppedPoints[num + 1];
			if (pointF.HasValue && pointF2.HasValue)
			{
				RectangleF rectangleF = RectangleF.Empty;
				if (pointF2.Value.X > pointF.Value.X)
				{
					rectangleF = new RectangleF(pointF.Value.X, pointF.Value.Y, pointF2.Value.X - pointF.Value.X, 1f);
					rectangleF.Inflate(0f, 5f);
				}
				else if (pointF2.Value.Y > pointF.Value.Y)
				{
					rectangleF = new RectangleF(pointF.Value.X, pointF.Value.Y, 1f, pointF2.Value.Y - pointF.Value.Y);
					rectangleF.Inflate(5f, 0f);
				}
				else
				{
					rectangleF = new RectangleF(pointF2.Value.X, pointF2.Value.Y, 1f, pointF.Value.Y - pointF2.Value.Y);
					rectangleF.Inflate(5f, 0f);
				}
				if (rectangleF.Contains(x, y))
				{
					return true;
				}
				num++;
				if (num == steppedPoints.Length - 1)
				{
					break;
				}
			}
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		IStiSteppedLineSeries stiSteppedLineSeries = base.Series as IStiSteppedLineSeries;
		context.PushSmoothingModeToAntiAlias();
		StiPenStyle lineStyle = stiSteppedLineSeries.LineStyle;
		float num = stiSteppedLineSeries.LineWidth * context.Options.Zoom;
		StiPenGeom stiPenGeom = new StiPenGeom(stiSteppedLineSeries.LineColor, num);
		stiPenGeom.PenStyle = lineStyle;
		List<PointF?[]> nullablePointsList = StiNullableDrawing.GetNullablePointsList(base.Points);
		StiChart stiChart = base.Series.Chart as StiChart;
		foreach (PointF?[] item in nullablePointsList)
		{
			PointF?[] steppedPoints = GetSteppedPoints(item);
			PointF?[] steppedPoints2 = GetSteppedPoints(base.PointsFrom);
			string[] steppedPointsIds = GetSteppedPointsIds(base.PointsIds);
			StiAnimation animation = GetAnimation(steppedPoints2, steppedPoints, steppedPointsIds);
			if (stiSteppedLineSeries.ShowShadow)
			{
				StiPenGeom stiPenGeom2 = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num + 0.5f * context.Options.Zoom);
				stiPenGeom2.PenStyle = lineStyle;
				context.PushTranslateTransform(num, num);
				StiNullableDrawing.DrawLines(context, stiPenGeom2, steppedPoints, animation);
				if (stiSteppedLineSeries.LineMarker.Visible)
				{
					StiSolidBrush brushMarker = new StiSolidBrush(Color.FromArgb(50, 0, 0, 0));
					stiSteppedLineSeries.LineMarker.Core.DrawLines(context, steppedPoints, context.Options.Zoom, brushMarker, null, stiSteppedLineSeries.LineMarker.Type, (float)stiSteppedLineSeries.LineMarker.Step, stiSteppedLineSeries.LineMarker.Size, stiSteppedLineSeries.LineMarker.Angle);
				}
				context.PopTransform();
			}
			if (base.Series.Core.IsMouseOver)
			{
				float lineWidth = stiSteppedLineSeries.LineWidth;
				float zoom = context.Options.Zoom;
				float num2 = 11f + lineWidth;
				PointF?[] points = base.Points;
				for (int i = 0; i < points.Length; i++)
				{
					PointF? pointF = points[i];
					if (pointF.HasValue)
					{
						RectangleF rect = new RectangleF(pointF.Value.X - num2 / 2f * zoom, pointF.Value.Y - num2 / 2f * zoom, num2 * zoom, num2 * zoom);
						context.FillEllipse(StiMouseOverHelper.GetLineMouseOverColor(stiSteppedLineSeries.LineColor), rect);
					}
				}
				StiPenGeom stiPenGeom3 = new StiPenGeom(StiMouseOverHelper.GetLineMouseOverColor(stiSteppedLineSeries.LineColor), (4f + lineWidth) * context.Options.Zoom);
				stiPenGeom3.StartCap = (StiPenLineCap)2;
				stiPenGeom3.EndCap = (StiPenLineCap)2;
				StiNullableDrawing.DrawLines(context, stiPenGeom3, steppedPoints, animation);
			}
			IStiAxisArea stiAxisArea = base.Series.Chart.Area as IStiAxisArea;
			float dividerY = stiAxisArea.AxisCore.GetDividerY();
			if (stiSteppedLineSeries.AllowApplyColorNegative)
			{
				StiPenGeom stiPenGeom4 = new StiPenGeom(stiSteppedLineSeries.LineColorNegative, num);
				stiPenGeom4.PenStyle = lineStyle;
				List<PointF?> list = new List<PointF?>();
				List<PointF?> list2 = new List<PointF?>();
				for (int j = 0; j < steppedPoints.Length; j++)
				{
					PointF? pointF2 = steppedPoints[j];
					PointF? pointNext = ((j != steppedPoints.Length - 1) ? steppedPoints[j + 1] : null);
					if (pointF2.Value.Y > dividerY)
					{
						list.Add(pointF2);
						if (!pointNext.HasValue || pointNext.Value.Y <= dividerY)
						{
							IntersectionAxis(pointF2, pointNext, list, list2, dividerY);
							PointF?[] array = new PointF?[list.Count];
							list.CopyTo(array);
							StiNullableDrawing.DrawLines(context, stiPenGeom4, array, animation);
							if (num >= 2f && stiSteppedLineSeries.Lighting)
							{
								float num3 = 0.5f * context.Options.Zoom;
								context.PushTranslateTransform(0f - num3, 0f - num3);
								StiPenGeom stiPenGeom5 = new StiPenGeom(StiColorUtils.Light(stiSteppedLineSeries.LineColorNegative, (byte)70), num);
								stiPenGeom5.PenStyle = lineStyle;
								StiNullableDrawing.DrawLines(context, stiPenGeom5, array, animation);
								context.PopTransform();
							}
							list.Clear();
						}
						continue;
					}
					list2.Add(pointF2);
					if (!pointNext.HasValue || pointNext.Value.Y > dividerY)
					{
						IntersectionAxis(pointF2, pointNext, list2, list, dividerY);
						PointF?[] array2 = new PointF?[list2.Count];
						list2.CopyTo(array2);
						StiNullableDrawing.DrawLines(context, stiPenGeom, array2, animation);
						if (num >= 2f && stiSteppedLineSeries.Lighting)
						{
							float num4 = 0.5f * context.Options.Zoom;
							context.PushTranslateTransform(0f - num4, 0f - num4);
							StiPenGeom stiPenGeom6 = new StiPenGeom(StiColorUtils.Light(stiSteppedLineSeries.LineColor, (byte)70), num);
							stiPenGeom6.PenStyle = lineStyle;
							StiNullableDrawing.DrawLines(context, stiPenGeom6, array2, animation);
							context.PopTransform();
						}
						list2.Clear();
					}
				}
			}
			else
			{
				if (nullablePointsList.Count == 1)
				{
					PointF?[] pointsStart = ((base.PointsFrom != null) ? GetSteppedPoints(base.PointsFrom) : null);
					StiNullableDrawing.DrawLines(context, stiPenGeom, pointsStart, steppedPoints, animation);
				}
				else
				{
					StiNullableDrawing.DrawLines(context, stiPenGeom, steppedPoints, animation);
				}
				if (num >= 2f && stiSteppedLineSeries.Lighting)
				{
					float num5 = 0.5f * context.Options.Zoom;
					context.PushTranslateTransform(0f - num5, 0f - num5);
					StiPenGeom stiPenGeom7 = new StiPenGeom(StiColorUtils.Light(stiSteppedLineSeries.LineColor, (byte)70), 1f);
					stiPenGeom7.PenStyle = lineStyle;
					StiNullableDrawing.DrawLines(context, stiPenGeom7, steppedPoints, animation);
					context.PopTransform();
				}
			}
			if (stiSteppedLineSeries.LineMarker.Visible)
			{
				StiPenGeom penMarker = new StiPenGeom(stiSteppedLineSeries.LineMarker.BorderColor);
				stiSteppedLineSeries.LineMarker.Core.DrawLines(context, steppedPoints, context.Options.Zoom, stiSteppedLineSeries.LineMarker.Brush, penMarker, stiSteppedLineSeries.LineMarker.Type, (float)stiSteppedLineSeries.LineMarker.Step, stiSteppedLineSeries.LineMarker.Size, stiSteppedLineSeries.LineMarker.Angle);
			}
		}
		context.PopSmoothingMode();
	}

	private void IntersectionAxis(PointF? point, PointF? pointNext, List<PointF?> points, List<PointF?> pointsNext, float posY)
	{
		if (pointNext.HasValue)
		{
			PointF? item = new PointF(point.Value.X, posY);
			points.Add(item);
			pointsNext.Add(item);
		}
	}

	public StiSteppedLineSeriesGeom(StiAreaGeom areaGeom, StiSeriesPointsInfo pointsInfo, IStiSeries series)
		: base(areaGeom, pointsInfo, series)
	{
	}
}
