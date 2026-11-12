using System;
using System.ComponentModel;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Report.Design;

[Flags]
public enum StiDataTransformationRestrictions
{
	None = 0,
	Sorts = 1,
	Filters = 2,
	Actions = 4,
	Priority = 8,
	[Obsolete("This option is obsolete. It will be removed in next versions. Please use the option Priority instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[JsonIgnore]
	ActionsBeforeGrouping = 8
}
