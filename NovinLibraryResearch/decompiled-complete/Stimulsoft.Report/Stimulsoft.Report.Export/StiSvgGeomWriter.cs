using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Xml;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export.Services.Helpers;

namespace Stimulsoft.Report.Export;

public class StiSvgGeomWriter : IStiExportGeomWriter
{
	private XmlTextWriter writer;

	public void BeginPath()
	{
	}

	public void CloseFigure()
	{
	}

	public void DrawBezier(PointF p1, PointF p2, PointF p3, PointF p4, object pen)
	{
	}

	public void DrawBezierTo(PointF p2, PointF p3, PointF p4, object pen)
	{
	}

	public void DrawImage(Image img, RectangleF rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		StiImage stiImage = new StiImage();
		stiImage.ClientRectangle = RectangleD.CreateFromRectangle(rect);
		stiImage.PutImageToDraw(img);
		stiImage.Smoothing = true;
		stiImage.Stretch = true;
		stiImage.Page = new StiPage();
		StiSvgData svgData = default(StiSvgData);
		svgData.X = rect.X;
		svgData.Y = rect.Y;
		svgData.Width = rect.Width;
		svgData.Height = rect.Height;
		svgData.Component = stiImage;
		ImageCodecInfo imageCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
		StiSvgHelper.WriteImage2(writer, svgData, img, 1f, img.RawFormat, 0.75f, imageCodec);
	}

	public void DrawLine(PointF pointFrom, PointF pointTo, object pen)
	{
	}

	public void DrawLineTo(PointF pointTo, object pen)
	{
	}

	public void DrawPolygon(PointF[] points, object pen)
	{
	}

	public void DrawPolyline(PointF[] points, object pen)
	{
	}

	public void DrawPolylineTo(PointF[] points, object pen)
	{
	}

	public void DrawRectangle(RectangleF rect, object pen)
	{
	}

	public void DrawString(string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf, bool allowHtmlTags = false)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected I4, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected I4, but got Unknown
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected I4, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected I4, but got Unknown
		writer.WriteStartElement("text");
		PointF pointF = default(PointF);
		StringAlignment alignment = sf.Alignment;
		switch ((int)alignment)
		{
		case 0:
			pointF.X = rect.X;
			break;
		case 1:
			pointF.X = rect.X + rect.Width / 2f;
			break;
		case 2:
			pointF.X = rect.X + rect.Width;
			break;
		}
		StringAlignment lineAlignment = sf.LineAlignment;
		switch ((int)lineAlignment)
		{
		case 0:
			pointF.Y = rect.Y;
			break;
		case 1:
			pointF.Y = rect.Y + rect.Height / 2f;
			break;
		case 2:
			pointF.Y = rect.Y + rect.Height;
			break;
		}
		writer.WriteAttributeString("transform", "translate(" + pointF.X.ToString().Replace(",", ".") + ", " + pointF.Y.ToString().Replace(",", ".") + ")");
		writer.WriteAttributeString("width", rect.Width.ToString().Replace(",", "."));
		writer.WriteAttributeString("height", rect.Height.ToString().Replace(",", "."));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"font-size:{font.SizeInPoints}pt;");
		stringBuilder.Append("font-family:'" + font.Name + "';");
		if (font.Bold)
		{
			stringBuilder.Append("font-weight:bold;");
		}
		if (font.Italic)
		{
			stringBuilder.Append("font-style:italic;");
		}
		if (font.Underline || font.Strikeout)
		{
			string text = ((!font.Underline) ? (font.Strikeout ? "line-through" : null) : (font.Strikeout ? "underline line-through" : "underline"));
			stringBuilder.Append("text-decoration:" + text + ";");
		}
		stringBuilder.Append(StiContextSvgHelper.WriteFillBrush(writer, brush, rect));
		StringAlignment alignment2 = sf.Alignment;
		switch ((int)alignment2)
		{
		case 0:
			stringBuilder.AppendFormat("text-anchor:{0};", "start");
			break;
		case 1:
			stringBuilder.AppendFormat("text-anchor:{0};", "middle");
			break;
		case 2:
			stringBuilder.AppendFormat("text-anchor:{0};", "end");
			break;
		}
		StringAlignment lineAlignment2 = sf.LineAlignment;
		switch ((int)lineAlignment2)
		{
		case 0:
			writer.WriteAttributeString("dy", "1em");
			break;
		case 1:
			writer.WriteAttributeString("dy", "0.5em");
			break;
		}
		writer.WriteAttributeString("style", stringBuilder.ToString());
		writer.WriteValue(st);
		writer.WriteEndElement();
	}

	public void DrawText(PointF basePoint, string text, int[] charsOffset, Font font, Color textColor, float angle, EmfTextAlignmentMode textAlign)
	{
	}

	public void EndPath()
	{
	}

	public void FillPath(object brush)
	{
	}

	public void FillPolygon(PointF[] points, object brush)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(StiContextSvgHelper.WriteFillBrush(writer, brush, default(RectangleF)));
		string text = "";
		for (int i = 0; i < points.Length; i++)
		{
			PointF pointF = points[i];
			text = text + pointF.X.ToString().Replace(",", ".") + "," + pointF.Y.ToString().Replace(",", ".") + " ";
		}
		writer.WriteStartElement("polygon");
		writer.WriteAttributeString("points", text);
		writer.WriteAttributeString("style", stringBuilder.ToString());
		writer.WriteEndElement();
	}

	public void FillRectangle(RectangleF rect, object brush)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(StiContextSvgHelper.WriteFillBrush(writer, brush, rect));
		writer.WriteStartElement("rect");
		writer.WriteAttributeString("x", rect.X.ToString().Replace(",", "."));
		writer.WriteAttributeString("y", rect.Y.ToString().Replace(",", "."));
		writer.WriteAttributeString("width", rect.Width.ToString().Replace(",", "."));
		writer.WriteAttributeString("height", rect.Height.ToString().Replace(",", "."));
		writer.WriteAttributeString("fill", "red");
		writer.WriteAttributeString("style", stringBuilder.ToString());
		writer.WriteAttributeString("shape-rendering", "crispEdges");
		writer.WriteEndElement();
	}

	public void FillRectangle(RectangleF rect, Color color)
	{
		FillRectangle(rect, (object)color);
	}

	public void DrawEllipse(RectangleF rect, object pen)
	{
	}

	public void FillEllipse(RectangleF rect, object brush)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(StiContextSvgHelper.WriteFillBrush(writer, brush, rect));
		writer.WriteStartElement("ellipse");
		writer.WriteAttributeString("cx", (rect.X + rect.Width / 2f).ToString().Replace(",", "."));
		writer.WriteAttributeString("cy", (rect.Y + rect.Height / 2f).ToString().Replace(",", "."));
		writer.WriteAttributeString("rx", (rect.Width / 2f).ToString().Replace(",", "."));
		writer.WriteAttributeString("ry", (rect.Height / 2f).ToString().Replace(",", "."));
		writer.WriteAttributeString("style", stringBuilder.ToString());
		writer.WriteEndElement();
	}

	public SizeF MeasureString(string st, Font font)
	{
		return default(SizeF);
	}

	public void MoveTo(PointF point)
	{
	}

	public void RestoreState()
	{
	}

	public void RotateTransform(float angle)
	{
		writer.WriteStartElement("g");
		writer.WriteAttributeString("transform", "rotate(" + angle.ToString().Replace(",", ".") + ")");
	}

	public void SaveState()
	{
	}

	public void SetPixel(PointF point, Color color)
	{
	}

	public void StrokePath(object pen)
	{
	}

	public void TranslateTransform(float x, float y)
	{
		writer.WriteStartElement("g");
		writer.WriteAttributeString("transform", "translate(" + x.ToString().Replace(",", ".") + "," + y.ToString().Replace(",", ".") + ")");
	}

	public void EndTransform()
	{
		writer.WriteEndElement();
	}

	public StiSvgGeomWriter(XmlTextWriter writer)
	{
		this.writer = writer;
	}
}
