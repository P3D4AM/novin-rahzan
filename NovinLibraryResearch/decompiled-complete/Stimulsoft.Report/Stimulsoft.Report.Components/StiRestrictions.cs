using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiRestrictions
{
	None = 0,
	AllowMove = 1,
	AllowResize = 2,
	AllowSelect = 4,
	AllowChange = 8,
	AllowDelete = 0x10,
	All = 0x1F
}
