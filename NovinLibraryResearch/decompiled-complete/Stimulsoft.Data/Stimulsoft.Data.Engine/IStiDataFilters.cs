using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiDataFilters
{
	List<StiDataFilterRule> DataFilters { get; set; }
}
