using System;

namespace Stimulsoft.Editor;

[Flags]
public enum GutterOptions
{
	None = 0,
	PaintLineNumbers = 1,
	PaintLinesOnGutter = 2,
	PaintLinesBeyondEof = 4,
	PaintBookMarks = 8,
	PaintLineModificators = 0x10,
	PaintUserMargin = 0x20,
	SelectLineOnClick = 0x40
}
