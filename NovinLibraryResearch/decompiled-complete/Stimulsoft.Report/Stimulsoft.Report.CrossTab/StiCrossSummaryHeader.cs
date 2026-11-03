using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossSummaryHeader : StiCrossField
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossSummaryHeader;

	[Browsable(true)]
	public sealed override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	public override string CellText => GetTextInternal();

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossSummaryHeader");

	public override StiComponent CreateNew()
	{
		return new StiCrossSummaryHeader();
	}
}
