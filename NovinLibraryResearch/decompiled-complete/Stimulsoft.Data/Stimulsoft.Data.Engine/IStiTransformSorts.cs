using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiTransformSorts
{
	List<StiDataSortRule> TransformSorts { get; set; }
}
