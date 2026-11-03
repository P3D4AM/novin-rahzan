using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiStyleComponentType
{
	Text = 1,
	Primitive = 2,
	Image = 4,
	CrossTab = 8,
	Chart = 0x10,
	CheckBox = 0x20
}
