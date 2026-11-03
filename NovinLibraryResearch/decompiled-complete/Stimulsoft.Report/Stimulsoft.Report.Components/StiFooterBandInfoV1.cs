using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiFooterBandInfoV1 : StiComponentInfo
{
	public bool ForceCanBreak { get; set; }

	public bool IsFirstPassOfBreak { get; set; } = true;


	public StiComponentsCollection BreakableComps { get; set; }
}
