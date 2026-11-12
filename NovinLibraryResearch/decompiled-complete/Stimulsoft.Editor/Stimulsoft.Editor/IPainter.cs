using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IPainter
{
	Matrix Transformation { get; }

	Graphics Graphics { get; }

	Font Font { get; set; }

	FontStyle FontStyle { get; set; }

	Color BackColor { get; set; }

	Color ForeColor { get; set; }

	Color TextColor { get; set; }

	StringFormat StringFormat { get; set; }

	bool Opaque { get; set; }

	bool IsMonoSpaced { get; }

	int FontWidth { get; }

	int FontHeight { get; }

	void Clear();

	int StringWidth(string text);

	int StringWidth(string text, int pos, int len);

	int StringWidth(string text, int width, out int count, bool exact);

	int StringWidth(string text, int pos, int len, int width, out int count);

	int StringWidth(string text, int pos, int len, int width, out int count, bool exact);

	int CharWidth(char ch, int count);

	int CharWidth(char ch, int width, out int count);

	void BeginPaint(Graphics graphics);

	void EndPaint();

	void FillRectangle(Rectangle rect);

	void FillRectangle(int x, int y, int width, int height);

	void FillRectangle(Color color, Rectangle rect);

	void FillRectangle(Color color, int x, int y, int width, int height);

	void FillPolygon(Color color, Point[] points);

	void FillGradient(Rectangle rect, Color beginColor, Color endColor, Point point1, Point point2);

	void FillGradient(int x, int y, int width, int height, Color beginColor, Color endColor, Point point1, Point point2);

	void DrawThemeBackground(IntPtr handle, int partID, int stateID, Rectangle rect);

	void DrawRectangle(Rectangle rect);

	void DrawFocusRect(int x, int y, int width, int height, Color color);

	void DrawFocusRect(Rectangle rect, Color color);

	void DrawRectangle(int x, int y, int width, int height);

	void DrawRoundRectangle(int left, int top, int right, int bottom, int width, int height);

	void DrawLine(int x1, int y1, int x2, int y2);

	void DrawLine(int x1, int y1, int x2, int y2, Color color, int width, DashStyle penStyle);

	void DrawDotLine(int x1, int y1, int x2, int y2, Color color);

	void DrawWave(Rectangle rect, Color color);

	void DrawImage(ImageList images, int index, Rectangle rect);

	void DrawImage(ImageList images, int index, Rectangle rect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr);

	void StretchDrawImage(Rectangle rect, Rectangle stretchRect, Rectangle imageRect, Bitmap image);

	void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides);

	void DrawEdge(ref Rectangle rect, Border3DStyle border, Border3DSide sides, int flags);

	void DrawPolygon(Point[] points, Color color);

	void TextOut(string text, int len, int x, int y);

	void TextOut(string text, int len, int x, int y, bool clipped, bool opaque);

	void TextOut(string text, int len, Rectangle rect);

	void TextOut(string text, int len, Rectangle rect, bool clipped, bool opaque);

	void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque);

	void TextOut(string text, int len, Rectangle rect, int x, int y, bool clipped, bool opaque, int space);

	void DrawText(string text, int len, Rectangle rect);

	void Transform(int x, int y, float scaleX, float scaleY);

	void EndTransform();

	void IntersectClipRect(int x, int y, int width, int height);

	void ExcludeClipRect(int x, int y, int width, int height);

	void IntersectClipRect(Rectangle rect);

	void ExcludeClipRect(Rectangle rect);

	IntPtr SaveClip(Rectangle rect);

	void RestoreClip(IntPtr rgn);
}
