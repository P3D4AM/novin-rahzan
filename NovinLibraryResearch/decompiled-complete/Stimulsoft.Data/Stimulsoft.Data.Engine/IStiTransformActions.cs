using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiTransformActions
{
	List<StiDataActionRule> TransformActions { get; set; }
}
