using Stimulsoft.Report.Gauge.Collections;

namespace Stimulsoft.Report.Gauge.Primitives;

public interface IStiTickCustom
{
	float ValueObj { get; set; }

	StiCustomValuesCollection Values { get; set; }
}
