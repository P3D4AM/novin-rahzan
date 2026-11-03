using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

public interface IStiFilter
{
	StiFilterEventHandler FilterMethodHandler { get; set; }

	StiFilterMode FilterMode { get; set; }

	StiFiltersCollection Filters { get; set; }

	bool FilterOn { get; set; }
}
