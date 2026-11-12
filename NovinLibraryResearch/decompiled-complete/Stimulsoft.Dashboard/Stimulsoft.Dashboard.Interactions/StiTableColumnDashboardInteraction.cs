using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

public class StiTableColumnDashboardInteraction : StiDashboardInteraction
{
	public override StiInteractionIdent Ident { get; } = StiInteractionIdent.TableColumn;


	internal override StiAvailableInteractionOnClick AvailableOnClick => StiAvailableInteractionOnClick.ShowDashboard | StiAvailableInteractionOnClick.ApplyFilter;

	[DefaultValue(StiInteractionOnHover.None)]
	public override StiInteractionOnHover OnHover { get; set; }

	[DefaultValue(StiInteractionOnClick.None)]
	public override StiInteractionOnClick OnClick { get; set; }

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (!ShouldSerializeOnHover() && !ShouldSerializeOnClick() && HyperlinkDestination == StiInteractionOpenHyperlinkDestination.NewTab && string.IsNullOrEmpty(Hyperlink) && string.IsNullOrEmpty(DrillDownPageKey) && DrillDownParameters != null && DrillDownParameters.Count == 0)
			{
				return string.IsNullOrEmpty(ToolTip);
			}
			return false;
		}
	}

	protected override bool ShouldSerializeOnHover()
	{
		return OnHover != StiInteractionOnHover.None;
	}

	protected override bool ShouldSerializeOnClick()
	{
		return OnClick != StiInteractionOnClick.None;
	}

	public StiTableColumnDashboardInteraction()
	{
	}

	public StiTableColumnDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
	}
}
