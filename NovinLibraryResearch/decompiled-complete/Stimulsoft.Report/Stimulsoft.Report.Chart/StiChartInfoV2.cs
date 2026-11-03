using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Chart;

public class StiChartInfoV2 : StiComponentInfo
{
	public StiChart StoredForProcessAtEndChart { get; set; }

	public List<StiText> InteractiveComps { get; set; }
}
