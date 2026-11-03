using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public class StiPdfGeomWriter : IStiExportGeomWriter
{
	private const double penWidthDefault = 0.1;

	private const int precision_digits = 3;

	private const double hiToTwips = 0.72;

	private const float pdfCKT = 0.55228f;

	private PointF lastPoint;

	private bool makePath;

	private bool pathClosed;

	private StringBuilder path;

	private NumberFormatInfo currentNumberFormat;

	private StreamWriter pageStream;

	private StiPdfExportService pdfService;

	internal bool assembleData;

	internal int pageNumber;

	internal Stack<Matrix> matrixCache;

	private float? xmin;

	private float? xmax;

	private float? ymin;

	private float? ymax;

	private void CalculateMinMax(PointF pt)
	{
		if (!xmin.HasValue)
		{
			xmin = pt.X;
		}
		else
		{
			xmin = Math.Min(xmin.Value, pt.X);
		}
		if (!xmax.HasValue)
		{
			xmax = pt.X;
		}
		else
		{
			xmax = Math.Max(xmax.Value, pt.X);
		}
		if (!ymin.HasValue)
		{
			ymin = pt.Y;
		}
		else
		{
			ymin = Math.Min(ymin.Value, pt.Y);
		}
		if (!ymax.HasValue)
		{
			ymax = pt.Y;
		}
		else
		{
			ymax = Math.Max(ymax.Value, pt.Y);
		}
	}

	private string ConvertToString(double Value)
	{
		return Math.Round((decimal)Value, 3).ToString("G", currentNumberFormat);
	}

	private bool IsPenEmpty(object objPen)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		if (objPen == null)
		{
			return true;
		}
		if (objPen is Pen)
		{
			return ((Pen)((objPen is Pen) ? objPen : null)).Color.A == 0;
		}
		if (objPen is StiPenGeom)
		{
			StiPenGeom stiPenGeom = objPen as StiPenGeom;
			Color color = Color.Transparent;
			if (stiPenGeom.Brush is Color)
			{
				color = (Color)stiPenGeom.Brush;
			}
			if (stiPenGeom.Brush is StiBrush)
			{
				object brush = stiPenGeom.Brush;
				color = StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null));
			}
			if (color.A != 0)
			{
				return (int)stiPenGeom.PenStyle == 6;
			}
			return true;
		}
		return false;
	}

	public bool SetPen(object objPen, bool saveState = false)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Invalid comparison between Unknown and I4
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (objPen is Pen)
		{
			Pen val = (Pen)((objPen is Pen) ? objPen : null);
			pdfService.SetStrokeColor(val.Color);
			double num = ((val.Width > 1f) ? ((double)val.Width) : ((double)val.Width * 0.72));
			if (num == 0.0)
			{
				num = 0.1;
			}
			pageStream.WriteLine("{0} w", ConvertToString(num));
			flag = saveState && (int)val.DashStyle > 0;
			if (flag)
			{
				pageStream.WriteLine("q");
			}
			string penStyleDashString = GetPenStyleDashString(val.DashStyle, (double)val.Width * 0.025);
			if (penStyleDashString != null)
			{
				pageStream.WriteLine(penStyleDashString);
			}
		}
		if (objPen is StiPenGeom)
		{
			StiPenGeom stiPenGeom = objPen as StiPenGeom;
			Color strokeColor = Color.Transparent;
			if (stiPenGeom.Brush is Color)
			{
				strokeColor = (Color)stiPenGeom.Brush;
			}
			if (stiPenGeom.Brush is StiBrush)
			{
				object brush = stiPenGeom.Brush;
				strokeColor = StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null));
			}
			pdfService.SetStrokeColor(strokeColor);
			double num2 = ((stiPenGeom.Thickness > 1f) ? ((double)stiPenGeom.Thickness) : ((double)stiPenGeom.Thickness * 0.72));
			if (num2 == 0.0)
			{
				num2 = 0.1;
			}
			pageStream.WriteLine("{0} w", ConvertToString(num2));
			flag = saveState && (int)stiPenGeom.PenStyle > 0;
			if (flag)
			{
				pageStream.WriteLine("q");
			}
			string penStyleDashString2 = GetPenStyleDashString(StiPenUtils.GetPenStyle(stiPenGeom.PenStyle), (double)stiPenGeom.Thickness * 0.025);
			if (penStyleDashString2 != null)
			{
				pageStream.WriteLine(penStyleDashString2);
			}
		}
		return flag;
	}

	public bool SetBrush(object brush, RectangleF rect, out bool isTransparent, bool saveState = false)
	{
		isTransparent = false;
		if (assembleData)
		{
			if (brush is StiGradientBrush || brush is StiGlareBrush || brush is StiHatchBrush || brush is StiGlassBrush)
			{
				pdfService.StoreShadingData1((StiBrush)((brush is StiBrush) ? brush : null), pageNumber);
				pdfService.StoreHatchData((StiBrush)((brush is StiBrush) ? brush : null));
			}
			return false;
		}
		bool flag = false;
		Color nonStrokeColor = Color.Transparent;
		if (brush is Color)
		{
			nonStrokeColor = (Color)brush;
		}
		if (brush is StiBrush)
		{
			nonStrokeColor = StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null));
		}
		pdfService.SetNonStrokeColor(nonStrokeColor);
		if (nonStrokeColor.A == 0)
		{
			isTransparent = true;
		}
		if (brush is StiGradientBrush || brush is StiGlareBrush || brush is StiHatchBrush || brush is StiGlassBrush)
		{
			Matrix val = matrixCache.Peek();
			PointF pointF = new PointF(rect.X, rect.Bottom);
			PointF pointF2 = new PointF(rect.Right, rect.Y);
			PointF[] array = new PointF[2] { pointF, pointF2 };
			val.TransformPoints(array);
			RectangleF rectangleF = new RectangleF(array[0].X, array[0].Y, array[1].X - array[0].X, array[1].Y - array[0].Y);
			int num = pdfService.StoreShadingData2(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height, (StiBrush)((brush is StiBrush) ? brush : null));
			if (brush is StiGradientBrush || brush is StiGlareBrush)
			{
				pageStream.WriteLine("/Pattern cs /P{0} scn", 1 + num);
			}
			if (brush is StiHatchBrush)
			{
				StiHatchBrush brush2 = (StiHatchBrush)((brush is StiHatchBrush) ? brush : null);
				pageStream.WriteLine("/Cs1 cs /PH{0} scn", pdfService.GetHatchNumber(brush2) + 1);
			}
			flag = true;
		}
		flag = flag && saveState;
		if (flag)
		{
			pageStream.WriteLine("q");
		}
		return flag;
	}

	private void OutputLineString(string st, object pen)
	{
		if (!IsPenEmpty(pen))
		{
			bool flag = SetPen(pen, saveState: true);
			pageStream.WriteLine(st);
			if (flag)
			{
				pageStream.WriteLine("Q");
			}
		}
	}

	private PointF[] ConvertArcToBezierPoints(RectangleF rect, float startAngle, float sweepAngle)
	{
		float num = rect.X + rect.Width / 2f;
		float num2 = rect.Y + rect.Height / 2f;
		float num3 = rect.Width / 2f;
		float num4 = rect.Height / 2f;
		startAngle *= (float)Math.PI / 180f;
		sweepAngle *= (float)Math.PI / 180f;
		int num5 = (int)Math.Ceiling((double)Math.Abs(sweepAngle) / (Math.PI / 2.0));
		PointF[] array = new PointF[3 * num5 + 1];
		float num6 = sweepAngle / (float)num5;
		double num7 = Math.Sin(num6 / 2f);
		double num8 = Math.Cos(num6 / 2f);
		double num9 = (1.0 - num8) * 4.0 / 3.0;
		double num10 = num7 - num9 * num8 / (num7 + 0.0001);
		double[] array2 = new double[4];
		double[] array3 = new double[4];
		array2[0] = num8;
		array3[0] = 0.0 - num7;
		array2[1] = num8 + num9;
		array3[1] = 0.0 - num10;
		array2[2] = num8 + num9;
		array3[2] = num10;
		array2[3] = num8;
		array3[3] = num7;
		double num11 = Math.Sin(startAngle + num6 / 2f);
		double num12 = Math.Cos(startAngle + num6 / 2f);
		array[0].X = (float)((double)num + (double)num3 * (array2[0] * num12 - array3[0] * num11));
		array[0].Y = (float)((double)num2 + (double)num4 * (array2[0] * num11 + array3[0] * num12));
		for (int i = 0; i < num5; i++)
		{
			float num13 = startAngle + num6 * (float)i;
			num11 = Math.Sin(num13 + num6 / 2f);
			num12 = Math.Cos(num13 + num6 / 2f);
			for (int j = 1; j <= 3; j++)
			{
				array[j + i * 3].X = (float)((double)num + (double)num3 * (array2[j] * num12 - array3[j] * num11));
				array[j + i * 3].Y = (float)((double)num2 + (double)num4 * (array2[j] * num11 + array3[j] * num12));
			}
		}
		return array;
	}

	public PointF[] ConvertSplineToCubicBezier(PointF[] points, float tension)
	{
		int num = points.Length;
		int num2 = num * 3 - 2;
		PointF[] array = new PointF[num2];
		tension *= 0.3f;
		array[0] = points[0];
		array[1] = CalculateCurveBezierEndPoints(points[0], points[1], tension);
		for (int i = 0; i < num - 2; i++)
		{
			PointF[] array2 = CalculateCurveBezier(points, i, tension);
			array[3 * i + 2] = array2[0];
			array[3 * i + 3] = points[i + 1];
			array[3 * i + 4] = array2[1];
		}
		array[num2 - 2] = CalculateCurveBezierEndPoints(points[num - 1], points[num - 2], tension);
		array[num2 - 1] = points[num - 1];
		return array;
	}

	private PointF[] CalculateCurveBezier(PointF[] points, int index, float tension)
	{
		float num = points[index + 2].X - points[index].X;
		float num2 = points[index + 2].Y - points[index].Y;
		return new PointF[2]
		{
			new PointF(points[index + 1].X - tension * num, points[index + 1].Y - tension * num2),
			new PointF(points[index + 1].X + tension * num, points[index + 1].Y + tension * num2)
		};
	}

	private PointF CalculateCurveBezierEndPoints(PointF end, PointF adj, float tension)
	{
		return new PointF(tension * (adj.X - end.X) + end.X, tension * (adj.Y - end.Y) + end.Y);
	}

	internal string GetPointString(PointF point)
	{
		CalculateMinMax(point);
		return string.Format("{0} {1} {2} ", ConvertToString(point.X), ConvertToString(point.Y), (makePath && path.Length > 0) ? "l" : "m");
	}

	internal string GetLineToString(PointF pointTo)
	{
		CalculateMinMax(pointTo);
		return $"{ConvertToString(pointTo.X)} {ConvertToString(pointTo.Y)} l ";
	}

	internal string GetRectString(double x, double y, double width, double height)
	{
		return GetRectString(new RectangleF((float)x, (float)y, (float)width, (float)height));
	}

	internal string GetRectString(RectangleF rect)
	{
		return $"{ConvertToString(rect.X)} {ConvertToString(rect.Top)} {ConvertToString(rect.Right - rect.X)} {ConvertToString(rect.Bottom - rect.Y)} re ";
	}

	internal string GetBezierString(PointF p1, PointF p2, PointF p3)
	{
		CalculateMinMax(p1);
		CalculateMinMax(p2);
		CalculateMinMax(p3);
		return $"{ConvertToString(p1.X)} {ConvertToString(p1.Y)} {ConvertToString(p2.X)} {ConvertToString(p2.Y)} {ConvertToString(p3.X)} {ConvertToString(p3.Y)} c ";
	}

	internal string GetBezierString(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
	{
		CalculateMinMax(new PointF(p1x, p1y));
		CalculateMinMax(new PointF(p2x, p2y));
		CalculateMinMax(new PointF(p3x, p3y));
		return $"{ConvertToString(p1x)} {ConvertToString(p1y)} {ConvertToString(p2x)} {ConvertToString(p2y)} {ConvertToString(p3x)} {ConvertToString(p3y)} c ";
	}

	internal string GetPolylineString(PointF[] points, bool close, bool drawTo)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!drawTo)
		{
			stringBuilder.Append(GetPointString(points[0]));
		}
		for (int i = ((!drawTo) ? 1 : 0); i < points.Length; i++)
		{
			stringBuilder.Append(GetLineToString(points[i]));
		}
		if (close)
		{
			stringBuilder.Append("h ");
		}
		return stringBuilder.ToString();
	}

	internal string GetEllipseString(double x, double y, double width, double height)
	{
		return GetEllipseString(new RectangleF((float)x, (float)y, (float)width, (float)height));
	}

	internal string GetEllipseString(RectangleF rect)
	{
		float num = rect.Width / 2f * 0.44772f;
		float num2 = rect.Height / 2f * 0.44772f;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0} {1} m ", ConvertToString(rect.X), ConvertToString(rect.Y + rect.Height / 2f));
		stringBuilder.Append(GetBezierString(rect.X, rect.Y + rect.Height - num2, rect.X + num, rect.Y + rect.Height, rect.X + rect.Width / 2f, rect.Y + rect.Height));
		stringBuilder.Append(GetBezierString(rect.X + rect.Width - num, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height - num2, rect.X + rect.Width, rect.Y + rect.Height / 2f));
		stringBuilder.Append(GetBezierString(rect.X + rect.Width, rect.Y + num2, rect.X + rect.Width - num, rect.Y, rect.X + rect.Width / 2f, rect.Y));
		stringBuilder.Append(GetBezierString(rect.X + num, rect.Y, rect.X, rect.Y + num2, rect.X, rect.Y + rect.Height / 2f));
		return stringBuilder.ToString();
	}

	private string GetPenStyleDashString(DashStyle style, double step)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected I4, but got Unknown
		double num = 1.0 / Math.Pow(10.0, 3.0);
		if (step < num)
		{
			step = num;
		}
		return (style - 1) switch
		{
			1 => $"[{ConvertToString(step)} {ConvertToString(step * 58.0)}] 0 d", 
			0 => $"[{ConvertToString(step * 49.5)} {ConvertToString(step * 62.0)}] 0 d", 
			2 => string.Format("[{0} {1} {2} {1}] 0 d", ConvertToString(step * 50.0), ConvertToString(step * 55.0), ConvertToString(step)), 
			3 => string.Format("[{0} {1} {2} {1} {2} {1}] 0 d", ConvertToString(step * 50.0), ConvertToString(step * 55.0), ConvertToString(step)), 
			_ => null, 
		};
	}

	public void BeginPath()
	{
		if (!assembleData)
		{
			path = new StringBuilder();
			makePath = true;
			pathClosed = false;
			xmin = null;
			xmax = null;
			ymin = null;
			ymax = null;
		}
	}

	public void CloseFigure()
	{
		if (!assembleData)
		{
			path.Append("h ");
			pathClosed = true;
		}
	}

	public void EndPath()
	{
		if (!assembleData)
		{
			makePath = false;
		}
	}

	public void FillPath(object brush)
	{
		RectangleF rect = (assembleData ? default(RectangleF) : new RectangleF(xmin.Value, ymin.Value, xmax.Value - xmin.Value, ymax.Value - ymin.Value));
		bool isTransparent;
		bool flag = SetBrush(brush, rect, out isTransparent, saveState: true);
		if (!assembleData)
		{
			if (!pathClosed)
			{
				EndPath();
			}
			if (!isTransparent)
			{
				pageStream.Write(path.ToString());
				pageStream.WriteLine("f");
			}
			if (flag)
			{
				pageStream.WriteLine("Q");
			}
		}
	}

	public void StrokePath(object pen)
	{
		if (!assembleData && !IsPenEmpty(pen))
		{
			SetPen(pen);
			pageStream.Write(path.ToString());
			pageStream.WriteLine("S");
		}
	}

	public void MoveTo(PointF point)
	{
		if (!assembleData)
		{
			lastPoint = point;
			string pointString = GetPointString(point);
			if (makePath)
			{
				path.AppendLine(pointString);
			}
			else
			{
				pageStream.WriteLine(pointString);
			}
		}
	}

	public void DrawLine(PointF pointFrom, PointF pointTo, object pen)
	{
		if (!assembleData)
		{
			string text = GetPointString(pointFrom) + GetLineToString(pointTo);
			if (makePath)
			{
				path.AppendLine(text);
			}
			else
			{
				OutputLineString(text + "S", pen);
			}
			lastPoint = pointTo;
		}
	}

	public void DrawLineTo(PointF pointTo, object pen)
	{
		if (!assembleData)
		{
			string lineToString = GetLineToString(pointTo);
			if (makePath)
			{
				path.AppendLine(lineToString);
			}
			else
			{
				OutputLineString(lineToString + "S", pen);
			}
			lastPoint = pointTo;
		}
	}

	public void DrawRectangle(RectangleF rect, object pen)
	{
		if (!assembleData)
		{
			if (makePath)
			{
				throw new Exception();
			}
			OutputLineString(GetRectString(rect) + "S", pen);
		}
	}

	public void FillRectangle(RectangleF rect, Color color)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		if (color.A != 0)
		{
			FillRectangle(rect, (object)new StiSolidBrush(color));
		}
	}

	public void FillRectangle(RectangleF rect, object brush)
	{
		bool isTransparent;
		bool flag = SetBrush(brush, rect, out isTransparent, saveState: true);
		if (assembleData)
		{
			return;
		}
		if (makePath)
		{
			throw new Exception();
		}
		if (brush != null && !isTransparent)
		{
			StiGlassBrush val = (StiGlassBrush)((brush is StiGlassBrush) ? brush : null);
			if (val != null)
			{
				if (val.DrawHatch)
				{
					StreamWriter streamWriter = pageStream;
					StiPdfExportService stiPdfExportService = pdfService;
					Brush topBrush = val.GetTopBrush();
					streamWriter.WriteLine("/Cs1 cs /PH{0} scn", stiPdfExportService.GetHatchNumber((HatchBrush)(object)((topBrush is HatchBrush) ? topBrush : null)) + 1);
				}
				else
				{
					pdfService.SetNonStrokeColor(val.GetTopColor());
				}
				pageStream.WriteLine(GetRectString(rect.X, rect.Y, rect.Width, rect.Height / 2f) + "f");
				if (val.DrawHatch)
				{
					StreamWriter streamWriter2 = pageStream;
					StiPdfExportService stiPdfExportService2 = pdfService;
					Brush bottomBrush = val.GetBottomBrush();
					streamWriter2.WriteLine("/Cs1 cs /PH{0} scn", stiPdfExportService2.GetHatchNumber((HatchBrush)(object)((bottomBrush is HatchBrush) ? bottomBrush : null)) + 1);
				}
				else
				{
					pdfService.SetNonStrokeColor(val.GetBottomColor());
				}
				pageStream.WriteLine(GetRectString(rect.X, rect.Y + rect.Height / 2f, rect.Width, rect.Height / 2f) + "f");
			}
			else
			{
				pageStream.WriteLine(GetRectString(rect) + "f");
			}
		}
		if (flag)
		{
			pageStream.WriteLine("Q");
		}
	}

	public void DrawPolyline(PointF[] points, object pen)
	{
		DrawPolyline(points, pen, close: false);
	}

	public void DrawPolygon(PointF[] points, object pen)
	{
		if (makePath)
		{
			throw new Exception();
		}
		DrawPolyline(points, pen, close: true);
	}

	public void DrawPolyline(PointF[] points, object pen, bool close, bool drawTo = false)
	{
		if (!assembleData)
		{
			string polylineString = GetPolylineString(points, close, drawTo);
			if (makePath)
			{
				path.AppendLine(polylineString);
			}
			else
			{
				OutputLineString(polylineString + "S", pen);
			}
			lastPoint = points[^1];
		}
	}

	public void DrawPolylineTo(PointF[] points, object pen)
	{
		DrawPolyline(points, pen, close: false, drawTo: true);
	}

	public void FillPolygon(PointF[] points, object brush)
	{
		foreach (PointF pt in points)
		{
			CalculateMinMax(pt);
		}
		RectangleF rect = new RectangleF(xmin.Value, ymin.Value, xmax.Value - xmin.Value, ymax.Value - ymin.Value);
		bool isTransparent;
		bool flag = SetBrush(brush, rect, out isTransparent, saveState: true);
		if (!assembleData)
		{
			if (makePath)
			{
				throw new Exception();
			}
			if (brush != null && !isTransparent)
			{
				string polylineString = GetPolylineString(points, close: true, drawTo: false);
				pageStream.WriteLine(polylineString + "f");
			}
			if (flag)
			{
				pageStream.WriteLine("Q");
			}
		}
	}

	public void DrawBezier(PointF p1, PointF p2, PointF p3, PointF p4, object pen)
	{
		if (!assembleData)
		{
			string text = GetPointString(p1) + GetBezierString(p2, p3, p4);
			if (makePath)
			{
				path.AppendLine(text);
			}
			else
			{
				OutputLineString(text + "S", pen);
			}
			lastPoint = p4;
		}
	}

	public void DrawBezierTo(PointF p2, PointF p3, PointF p4, object pen)
	{
		if (!assembleData)
		{
			string bezierString = GetBezierString(p2, p3, p4);
			if (makePath)
			{
				path.AppendLine(bezierString);
			}
			else
			{
				OutputLineString(bezierString + "S", pen);
			}
			lastPoint = p4;
		}
	}

	public void DrawSpline(PointF[] points, float tension, object pen)
	{
		if (!assembleData)
		{
			PointF[] array = ConvertSplineToCubicBezier(points, tension);
			int num = (array.Length - 1) / 3;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(GetPointString(array[0]) + GetBezierString(array[1], array[2], array[3]));
			for (int i = 1; i < num; i++)
			{
				stringBuilder.Append(GetBezierString(array[i * 3 + 1], array[i * 3 + 2], array[i * 3 + 3]));
			}
			if (makePath)
			{
				path.AppendLine(stringBuilder.ToString());
			}
			else
			{
				OutputLineString(stringBuilder.ToString() + "S", pen);
			}
			lastPoint = array[^1];
		}
	}

	public void DrawArc(RectangleF rect, float startAngle, float sweepAngle)
	{
		if (!assembleData)
		{
			PointF[] array = ConvertArcToBezierPoints(rect, startAngle, sweepAngle);
			int num = (array.Length - 1) / 3;
			DrawBezier(array[0], array[1], array[2], array[3], null);
			for (int i = 1; i < num; i++)
			{
				DrawBezierTo(array[i * 3 + 1], array[i * 3 + 2], array[i * 3 + 3], null);
			}
		}
	}

	public void DrawEllipse(RectangleF rect, object pen)
	{
		if (!assembleData)
		{
			if (makePath)
			{
				throw new Exception();
			}
			OutputLineString(GetEllipseString(rect) + "S", pen);
		}
	}

	public void FillEllipse(RectangleF rect, object brush)
	{
		bool isTransparent;
		bool flag = SetBrush(brush, rect, out isTransparent, saveState: true);
		if (!assembleData)
		{
			if (makePath)
			{
				throw new Exception();
			}
			if (brush != null && !isTransparent)
			{
				pageStream.WriteLine(GetEllipseString(rect) + "f");
			}
			if (flag)
			{
				pageStream.WriteLine("Q");
			}
		}
	}

	public void DrawPie(RectangleF rect, float startAngle, float sweepAngle)
	{
		if (!assembleData)
		{
			PointF[] array = ConvertArcToBezierPoints(rect, startAngle, sweepAngle);
			int num = (array.Length - 1) / 3;
			float x = rect.X + rect.Width / 2f;
			float y = rect.Y + rect.Height / 2f;
			DrawBezier(array[0], array[1], array[2], array[3], null);
			for (int i = 1; i < num; i++)
			{
				DrawBezierTo(array[i * 3 + 1], array[i * 3 + 2], array[i * 3 + 3], null);
			}
			DrawLineTo(new PointF(x, y), null);
			DrawLineTo(array[0], null);
		}
	}

	public void DrawImage(Image img, RectangleF rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		StiImage stiImage = new StiImage();
		stiImage.ClientRectangle = RectangleD.CreateFromRectangle(rect);
		stiImage.PutImageToDraw(img);
		stiImage.Smoothing = true;
		stiImage.Stretch = true;
		stiImage.Page = pdfService.report.RenderedPages[0] ?? new StiPage(pdfService.report);
		if (assembleData)
		{
			pdfService.StoreImageDataForGeom(stiImage);
			return;
		}
		StiPdfExportService.StiPdfData pp = default(StiPdfExportService.StiPdfData);
		pp.Component = stiImage;
		pp.Width = rect.Width;
		pp.Height = rect.Height;
		pageStream.WriteLine("q");
		pdfService.PushColorToStack();
		pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(rect.X), ConvertToString(rect.Y + rect.Height));
		pageStream.WriteLine("1 0 0 -1 0 0 cm");
		pdfService.RenderImage(pp, 100f, forceResolutionModeAuto: true);
		pageStream.WriteLine("Q");
		pdfService.PopColorFromStack();
	}

	public void DrawString(string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf, bool allowHtmlTags = false)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Invalid comparison between Unknown and I4
		StiText stiText = new StiText();
		stiText.ClientRectangle = pdfService.report.Unit.ConvertFromHInches(RectangleD.CreateFromRectangle(rect));
		stiText.Text = st;
		stiText.Font = font;
		stiText.TextBrush = brush;
		if ((int)sf.Alignment == 1)
		{
			stiText.HorAlignment = (StiTextHorAlignment)1;
		}
		if ((int)sf.Alignment == 2)
		{
			stiText.HorAlignment = (StiTextHorAlignment)2;
		}
		if ((int)sf.LineAlignment == 1)
		{
			stiText.VertAlignment = (StiVertAlignment)1;
		}
		if ((int)sf.LineAlignment == 2)
		{
			stiText.VertAlignment = (StiVertAlignment)2;
		}
		if ((sf.FormatFlags & 0x1000) == 0)
		{
			stiText.WordWrap = true;
		}
		stiText.TextQuality = StiTextQuality.Standard;
		if (allowHtmlTags)
		{
			stiText.TextQuality = StiTextQuality.Wysiwyg;
			stiText.AllowHtmlTags = true;
		}
		stiText.Page = pdfService.report.RenderedPages[0] ?? new StiPage(pdfService.report);
		if (assembleData)
		{
			if (allowHtmlTags)
			{
				pdfService.StoreWysiwygSymbols(stiText);
				return;
			}
			if (font != null)
			{
				int fontNumber = pdfService.pdfFont.GetFontNumber(font);
			}
			bool rightToLeft = (sf.FormatFlags & 1) > 0;
			StringBuilder sb = new StringBuilder(StiBidirectionalConvert2.ConvertString(st, rightToLeft));
			pdfService.pdfFont.StoreUnicodeSymbolsInMap(sb);
			return;
		}
		StiPdfExportService.StiPdfData pp = default(StiPdfExportService.StiPdfData);
		pp.Component = stiText;
		pp.Width = (double)rect.Width * 0.72;
		pp.Height = (double)rect.Height * 0.72;
		pageStream.WriteLine("q");
		pdfService.PushColorToStack();
		pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(rect.X), ConvertToString(rect.Y + rect.Height));
		pageStream.WriteLine("1.39 0 0 -1.39 0 0 cm");
		if (allowHtmlTags)
		{
			pdfService.RenderText2(pp);
		}
		else
		{
			pdfService.RenderTextFont(pp);
			pdfService.RenderText(pp);
		}
		pageStream.WriteLine("Q");
		pdfService.PopColorFromStack();
	}

	public void SaveState()
	{
		if (!assembleData)
		{
			pageStream.WriteLine("q");
			pdfService.PushColorToStack();
			if (matrixCache.Count > 0)
			{
				matrixCache.Push(matrixCache.Peek().Clone());
			}
		}
	}

	public void RestoreState()
	{
		if (!assembleData)
		{
			pageStream.WriteLine("Q");
			pdfService.PopColorFromStack();
			if (matrixCache.Count > 0)
			{
				matrixCache.Pop();
			}
		}
	}

	public void TranslateTransform(float x, float y)
	{
		if (!assembleData)
		{
			pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(x), ConvertToString(y));
			if (matrixCache.Count > 0)
			{
				matrixCache.Peek().Translate(x, y);
			}
		}
	}

	public void RotateTransform(float angle)
	{
		if (!assembleData)
		{
			double num = (double)angle * Math.PI / 180.0;
			pageStream.WriteLine("{0} {1} {2} {3} 0 0 cm", ConvertToString(Math.Cos(num)), ConvertToString(Math.Sin(num)), ConvertToString(0.0 - Math.Sin(num)), ConvertToString(Math.Cos(num)));
			if (matrixCache.Count > 0)
			{
				matrixCache.Peek().Rotate(angle);
			}
		}
	}

	public void RotateTransform(float angle, float x, float y)
	{
		if (!assembleData)
		{
			double num = (double)angle * Math.PI / 180.0;
			pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num)), ConvertToString(Math.Sin(num)), ConvertToString(0.0 - Math.Sin(num)), ConvertToString(Math.Cos(num)), ConvertToString(x), ConvertToString(y));
			if (matrixCache.Count > 0)
			{
				matrixCache.Peek().RotateAt(angle, new PointF(x, y));
			}
		}
	}

	public void ScaleTransform(float scaleX, float scaleY)
	{
		if (!assembleData)
		{
			pageStream.WriteLine("{0} 0 0 {1} 0 0 cm", ConvertToString(scaleX), ConvertToString(scaleY));
			if (matrixCache.Count > 0)
			{
				matrixCache.Peek().Scale(scaleX, scaleY);
			}
		}
	}

	public void SetClipPath()
	{
		if (!assembleData)
		{
			pageStream.Write(path.ToString());
			pageStream.WriteLine("W n");
		}
	}

	public void SetClip(RectangleF rect)
	{
		if (!assembleData)
		{
			pageStream.WriteLine(GetRectString(rect) + "W n");
		}
	}

	public void DrawArc(RectangleF rect, PointF p1, PointF p2, Pen pen)
	{
		if (!assembleData)
		{
			lastPoint = p2;
		}
	}

	public void DrawText(PointF basePoint, string text, int[] charsOffset, Font font, Color textColor, float angle, EmfTextAlignmentMode textAlign)
	{
		if (assembleData || !makePath)
		{
			return;
		}
		throw new Exception();
	}

	public void SetPixel(PointF point, Color color)
	{
		_ = assembleData;
	}

	public SizeF MeasureString(string st, Font font)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		if (assembleData)
		{
			return default(SizeF);
		}
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			SizeF result = val2.MeasureString(st, font);
			val2.Dispose();
			return result;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiPdfGeomWriter()
	{
	}

	public StiPdfGeomWriter(StreamWriter stream, StiPdfExportService service, bool assembleData = false)
	{
		pageStream = stream;
		pdfService = service;
		this.assembleData = assembleData;
		matrixCache = new Stack<Matrix>();
		currentNumberFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
		currentNumberFormat.NumberDecimalSeparator = ".";
	}
}
