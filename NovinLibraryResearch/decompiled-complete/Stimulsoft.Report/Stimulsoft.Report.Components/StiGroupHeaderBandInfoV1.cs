using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiGroupHeaderBandInfoV1 : StiComponentInfo
{
	public int LastPositionLineRendering { get; set; } = -1;


	public int LastPositionRendering { get; set; } = -1;


	public bool Rerender { get; set; }

	public bool ForceCanBreak { get; set; }

	public bool IsFirstPassOfBreak { get; set; } = true;


	public StiComponentsCollection BreakableComps { get; set; }

	public StiGroupFooterBand GroupFooter { get; set; }
}
