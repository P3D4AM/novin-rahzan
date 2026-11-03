using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiGroupFooterBandInfoV1 : StiComponentInfo
{
	internal StiComponent PrintAtBottomComponent { get; set; }

	public StiGroupHeaderBand GroupHeader { get; set; }
}
