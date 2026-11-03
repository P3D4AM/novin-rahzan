using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiGroupHeaderBandInfoV2 : StiComponentInfo
{
	public bool SkipKeepGroups;

	public StiGroupFooterBand GroupFooter;

	public bool SilentModeEnabled;

	public bool OldSilentMode;

	internal bool IsTableGroupHeader;
}
