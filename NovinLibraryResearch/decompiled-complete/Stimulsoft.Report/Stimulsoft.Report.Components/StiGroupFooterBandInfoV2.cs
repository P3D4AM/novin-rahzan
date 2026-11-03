using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiGroupFooterBandInfoV2 : StiComponentInfo
{
	public StiComponent PrintAtBottomComponent { get; set; }

	public StiGroupHeaderBand GroupHeader { get; set; }

	public bool IsTableGroupFooter { get; set; }
}
