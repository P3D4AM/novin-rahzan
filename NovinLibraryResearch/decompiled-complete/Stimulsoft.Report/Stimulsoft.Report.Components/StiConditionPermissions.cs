using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiConditionPermissions
{
	None = 0,
	Font = 1,
	FontSize = 2,
	FontStyleBold = 4,
	FontStyleItalic = 8,
	FontStyleUnderline = 0x10,
	FontStyleStrikeout = 0x20,
	TextColor = 0x40,
	BackColor = 0x80,
	Borders = 0x100,
	All = 0x1FF
}
