using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export;

public class StiBarCodeExportPainter : IStiBarCodePainter
{
	private IStiExportGeomWriter geomWriter;

	private float pdfGeomWriterInflate = -0.09f;

	public bool OnlyAssembleData
	{
		get
		{
			if (geomWriter is StiPdfGeomWriter)
			{
				return (geomWriter as StiPdfGeomWriter).assembleData;
			}
			return false;
		}
	}

	public void BaseTransform(object context, float x, float y, float angle, float dx, float dy)
	{
		geomWriter.TranslateTransform(x, y);
		geomWriter.RotateTransform(angle);
		geomWriter.TranslateTransform(dx, dy);
	}

	public void BaseRollbackTransform(object context)
	{
		if (geomWriter is StiSvgGeomWriter)
		{
			StiSvgGeomWriter stiSvgGeomWriter = geomWriter as StiSvgGeomWriter;
			stiSvgGeomWriter.EndTransform();
			stiSvgGeomWriter.EndTransform();
			stiSvgGeomWriter.EndTransform();
		}
	}

	public void BaseFillRectangle(object context, StiBrush brush, float x, float y, float width, float height)
	{
		RectangleF rect = new RectangleF(x, y, width, height);
		if (geomWriter is StiPdfGeomWriter)
		{
			rect.Inflate(pdfGeomWriterInflate, pdfGeomWriterInflate);
		}
		geomWriter.FillRectangle(rect, brush);
	}

	public void BaseFillRectangle2D(object context, StiBrush brush, float x, float y, float width, float height)
	{
		RectangleF rect = new RectangleF(x, y, width, height);
		if (geomWriter is StiPdfGeomWriter)
		{
			rect.Inflate(pdfGeomWriterInflate, pdfGeomWriterInflate);
		}
		geomWriter.FillRectangle(rect, brush);
	}

	public void BaseFillPolygon(object context, StiBrush brush, PointF[] points)
	{
		geomWriter.FillPolygon(points, brush);
	}

	public void BaseFillEllipse(object context, StiBrush brush, float x, float y, float width, float height)
	{
		geomWriter.FillEllipse(new RectangleF(x, y, width, height), brush);
	}

	public void BaseDrawRectangle(object context, Color penColor, float penSize, float x, float y, float width, float height)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		RectangleF rect = new RectangleF(x, y, width, height);
		geomWriter.DrawRectangle(rect, (object)new Pen(penColor, penSize));
	}

	public void BaseDrawImage(object context, Image image, StiReport report, float x, float y, float width, float height)
	{
		if (image != null)
		{
			RectangleF rect = new RectangleF(x, y, width, height);
			geomWriter.DrawImage(image, rect);
		}
	}

	public void BaseDrawString(object context, string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Brush brush2 = StiBrush.GetBrush(brush, rect);
		try
		{
			geomWriter.DrawString(st, font, brush, rect, (StringFormat)(((object)sf) ?? ((object)new StringFormat())));
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
	}

	public SizeF BaseMeasureString(object context, string st, Font font)
	{
		return geomWriter.MeasureString(st, font);
	}

	public StiBarCodeExportPainter(StiSvgGeomWriter geomWriter)
	{
		this.geomWriter = geomWriter;
	}

	public StiBarCodeExportPainter(StiPdfGeomWriter geomWriter)
	{
		this.geomWriter = geomWriter;
	}

	public StiBarCodeExportPainter(StiExcel2007GeomWriter geomWriter)
	{
		this.geomWriter = geomWriter;
	}
}
