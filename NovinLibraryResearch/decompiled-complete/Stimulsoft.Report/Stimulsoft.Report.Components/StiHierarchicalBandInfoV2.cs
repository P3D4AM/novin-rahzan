using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiHierarchicalBandInfoV2 : StiComponentInfo
{
	internal int SpecifiedLevel { get; set; } = -1;


	internal bool FinalFooterCalculation { get; set; }
}
