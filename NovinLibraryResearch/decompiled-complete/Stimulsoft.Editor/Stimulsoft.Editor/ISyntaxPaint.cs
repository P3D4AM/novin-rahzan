using System.Drawing;

namespace Stimulsoft.Editor;

public interface ISyntaxPaint
{
	bool DisableColorPaint { get; set; }

	bool DisableSyntaxPaint { get; set; }

	ILexer Lexer { get; set; }

	event CustomDrawEvent CustomDraw;

	void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint);

	void DrawLine(int index, Point position, Rectangle clipRect);

	void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect);

	int MeasureLine(int index, int pos, int len);

	int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact);

	int MeasureLine(string line, short[] colorData, int pos, int len);

	int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool exact);

	bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, DrawInfo info);

	void ResetDisableColorPaint();

	void ResetDisableSyntaxPaint();
}
