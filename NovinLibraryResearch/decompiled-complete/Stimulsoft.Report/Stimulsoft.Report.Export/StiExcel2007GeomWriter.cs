using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiExcel2007GeomWriter : IStiExportGeomWriter
{
	private List<StiExcel2007ExportService.BarCodeLineData> barCodesList;

	private int row;

	private int column;

	private double offsetX;

	private double offsetY;

	public void BeginPath()
	{
	}

	public void CloseFigure()
	{
	}

	public void EndPath()
	{
	}

	public void FillPath(object brush)
	{
	}

	public void StrokePath(object pen)
	{
	}

	public void MoveTo(PointF point)
	{
	}

	public void DrawLine(PointF pointFrom, PointF pointTo, object pen)
	{
	}

	public void DrawLineTo(PointF pointTo, object pen)
	{
	}

	public void DrawRectangle(RectangleF rect, object pen)
	{
	}

	public void FillRectangle(RectangleF rect, Color color)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		StiExcel2007ExportService.BarCodeLineData item = default(StiExcel2007ExportService.BarCodeLineData);
		item.Row = row;
		item.Column = column;
		item.Position = new PointD(offsetX + (double)rect.X, offsetY + (double)rect.Top);
		item.Size = new SizeD((double)rect.Width, (double)rect.Height);
		item.Color = color;
		barCodesList.Add(item);
	}

	public void FillRectangle(RectangleF rect, object brush)
	{
		StiBrush val = (StiBrush)((brush is StiBrush) ? brush : null);
		if (val != null)
		{
			FillRectangle(rect, StiBrush.ToColor(val));
		}
	}

	public void DrawPolyline(PointF[] points, object pen)
	{
	}

	public void DrawPolygon(PointF[] points, object pen)
	{
	}

	public void DrawPolyline(PointF[] points, object pen, bool close, bool drawTo = false)
	{
	}

	public void DrawPolylineTo(PointF[] points, object pen)
	{
	}

	public void FillPolygon(PointF[] points, object brush)
	{
	}

	public void DrawBezier(PointF p1, PointF p2, PointF p3, PointF p4, object pen)
	{
	}

	public void DrawBezierTo(PointF p2, PointF p3, PointF p4, object pen)
	{
	}

	public void DrawSpline(PointF[] points, float tension, object pen)
	{
	}

	public void DrawEllipse(RectangleF rect, object pen)
	{
	}

	public void FillEllipse(RectangleF rect, object brush)
	{
	}

	public void DrawPie(RectangleF rect, float startAngle, float sweepAngle)
	{
	}

	public void DrawImage(Image img, RectangleF rect)
	{
	}

	public void DrawString(string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf, bool allowHtmlTags = false)
	{
	}

	public void SaveState()
	{
	}

	public void RestoreState()
	{
	}

	public void TranslateTransform(float x, float y)
	{
		offsetX += x;
		offsetY += y;
	}

	public void RotateTransform(float angle)
	{
	}

	public void SetClip(RectangleF rect)
	{
	}

	public void DrawArc(RectangleF rect, PointF p1, PointF p2, Pen pen)
	{
	}

	public void DrawText(PointF basePoint, string text, int[] charsOffset, Font font, Color textColor, float angle, EmfTextAlignmentMode textAlign)
	{
	}

	public void SetPixel(PointF point, Color color)
	{
	}

	public SizeF MeasureString(string st, Font font)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
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

	public StiExcel2007GeomWriter()
	{
	}

	internal StiExcel2007GeomWriter(List<StiExcel2007ExportService.BarCodeLineData> barCodeLinesList, int row, int column)
	{
		barCodesList = barCodeLinesList;
		this.row = row;
		this.column = column;
	}
}
