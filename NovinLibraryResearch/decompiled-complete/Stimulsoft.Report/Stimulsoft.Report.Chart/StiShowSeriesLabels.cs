using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Chart;

public enum StiShowSeriesLabels
{
	[Obsolete("Please use StiShowSeriesLabels.FromChart")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	None,
	FromChart,
	FromSeries
}
