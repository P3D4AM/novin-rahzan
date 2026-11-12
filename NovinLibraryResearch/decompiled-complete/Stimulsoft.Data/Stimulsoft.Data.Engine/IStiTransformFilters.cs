using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiTransformFilters
{
	List<StiDataFilterRule> TransformFilters { get; set; }
}
