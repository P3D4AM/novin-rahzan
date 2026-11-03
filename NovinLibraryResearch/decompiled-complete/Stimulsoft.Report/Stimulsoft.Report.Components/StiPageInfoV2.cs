using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiPageInfoV2 : StiComponentInfo
{
	internal StiComponentsCollection Overlays;

	public int IndexOfStartRenderedPages = -1;

	public StiDataBand MasterDataBand;

	public bool IsReportTitlesRendered;

	public int RenderedCount;

	public double PositionFromTop;

	public double PositionFromBottom;
}
