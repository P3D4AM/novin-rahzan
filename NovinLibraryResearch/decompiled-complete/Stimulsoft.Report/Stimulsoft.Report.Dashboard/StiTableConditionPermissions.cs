using System;

namespace Stimulsoft.Report.Dashboard;

[Flags]
public enum StiTableConditionPermissions
{
	None = 0,
	Font = 1,
	FontSize = 2,
	FontStyleBold = 4,
	FontStyleItalic = 8,
	FontStyleUnderline = 0x10,
	FontStyleStrikeout = 0x20,
	ForeColor = 0x40,
	BackColor = 0x80,
	All = 0xFF
}
