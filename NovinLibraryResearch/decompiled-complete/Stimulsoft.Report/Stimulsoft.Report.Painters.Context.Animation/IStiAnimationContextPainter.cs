using System.Collections.Generic;

namespace Stimulsoft.Report.Painters.Context.Animation;

public interface IStiAnimationContextPainter<T>
{
	List<T> Geoms { get; }

	StiAnimationEngine AnimationEngine { get; set; }
}
