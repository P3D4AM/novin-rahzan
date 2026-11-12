using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiDrillDownElement
{
	List<StiDataFilterRule> DrillDownFilters { get; set; }

	List<List<StiDataFilterRule>> DrillDownFiltersList { get; set; }

	int DrillDownCurrentLevel { get; set; }

	int DrillDownLevelCount { get; }
}
