using System;

namespace Stimulsoft.Dashboard.Viewer;

[Flags]
public enum StiDataTransformationRestrictions
{
	None = 0,
	Sorts = 1,
	Filters = 2,
	Actions = 4,
	ActionsBeforeGrouping = 8
}
