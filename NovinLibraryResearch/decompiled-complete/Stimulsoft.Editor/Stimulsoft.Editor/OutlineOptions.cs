using System;

namespace Stimulsoft.Editor;

[Flags]
public enum OutlineOptions
{
	None = 0,
	DrawOnGutter = 1,
	DrawLines = 2,
	DrawButtons = 4,
	ShowHints = 8
}
