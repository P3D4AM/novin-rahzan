using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiUserFilters
{
	List<StiDataFilterRule> UserFilters { get; set; }
}
