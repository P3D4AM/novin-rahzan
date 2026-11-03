using System;

namespace Stimulsoft.Report;

[Flags]
public enum StiStyleConditionType
{
	ComponentType = 1,
	Placement = 2,
	PlacementNestedLevel = 4,
	ComponentName = 8,
	Location = 0x10
}
