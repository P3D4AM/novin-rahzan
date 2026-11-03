using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Maps;

public enum StiMapType
{
	[Obsolete("Please use StiMapType.Individual")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	None = 0,
	Individual = 4,
	Group = 1,
	Heatmap = 2,
	HeatmapWithGroup = 3
}
