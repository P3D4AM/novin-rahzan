using System;

namespace Stimulsoft.Report.Dashboard;

[Flags]
public enum StiIndicatorConditionPermissions
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
	Icon = 0x200,
	TargetIcon = 0x400,
	All = 0x7FF
}
