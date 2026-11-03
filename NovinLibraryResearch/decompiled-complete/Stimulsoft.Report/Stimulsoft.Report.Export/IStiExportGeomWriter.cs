using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public interface IStiExportGeomWriter
{
	void BeginPath();

	void CloseFigure();

	void EndPath();

	void FillPath(object brush);

	void StrokePath(object pen);

	void MoveTo(PointF point);

	void DrawLine(PointF pointFrom, PointF pointTo, object pen);

	void DrawLineTo(PointF pointTo, object pen);

	void DrawRectangle(RectangleF rect, object pen);

	void FillRectangle(RectangleF rect, Color color);

	void FillRectangle(RectangleF rect, object brush);

	void DrawPolyline(PointF[] points, object pen);

	void DrawPolylineTo(PointF[] points, object pen);

	void DrawPolygon(PointF[] points, object pen);

	void FillPolygon(PointF[] points, object brush);

	void DrawBezier(PointF p1, PointF p2, PointF p3, PointF p4, object pen);

	void DrawBezierTo(PointF p2, PointF p3, PointF p4, object pen);

	void DrawEllipse(RectangleF rect, object pen);

	void FillEllipse(RectangleF rect, object brush);

	void SetPixel(PointF point, Color color);

	void DrawImage(Image img, RectangleF rect);

	void DrawText(PointF basePoint, string text, int[] charsOffset, Font font, Color textColor, float angle, EmfTextAlignmentMode textAlign);

	void DrawString(string st, Font font, StiBrush brush, RectangleF rect, StringFormat sf, bool allowHtmlTags = false);

	void SaveState();

	void RestoreState();

	void TranslateTransform(float x, float y);

	void RotateTransform(float angle);

	SizeF MeasureString(string st, Font font);
}
