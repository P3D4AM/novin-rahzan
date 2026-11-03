using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiStyleElements
{
	Font = 1,
	Border = 2,
	Brush = 4,
	TextBrush = 8,
	TextOptions = 0x10,
	HorAlignment = 0x20,
	VertAlignment = 0x40,
	All = 0x7F
}
