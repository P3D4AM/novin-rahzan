using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiHeaderBandInfoV1 : StiComponentInfo
{
	public bool StartNewPageProcessed { get; set; }

	public bool ForceCanBreak { get; set; }

	public bool IsFirstPassOfBreak { get; set; } = true;


	public StiComponentsCollection BreakableComps { get; set; }
}
