using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

[TypeConverter(typeof(StiImageDashboardInteractionConverter))]
public class StiImageDashboardInteraction : StiDashboardInteraction
{
	public override StiInteractionIdent Ident { get; } = StiInteractionIdent.Image;


	internal override StiAvailableInteractionOnClick AvailableOnClick => StiAvailableInteractionOnClick.ShowDashboard | StiAvailableInteractionOnClick.OpenHyperlink;

	[DefaultValue(StiInteractionOnClick.OpenHyperlink)]
	public override StiInteractionOnClick OnClick { get; set; } = StiInteractionOnClick.OpenHyperlink;


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

	protected override bool ShouldSerializeOnClick()
	{
		return OnClick != StiInteractionOnClick.OpenHyperlink;
	}

	public StiImageDashboardInteraction()
	{
	}

	public StiImageDashboardInteraction(StiInteractionOnHover onHover, StiInteractionOnClick onClick, StiInteractionOpenHyperlinkDestination hyperlinkDestination, string toolTip, string hyperlink, string drillDownPageKey, List<StiDashboardDrillDownParameter> drillDownParameters)
		: base(onHover, onClick, hyperlinkDestination, toolTip, hyperlink, drillDownPageKey, drillDownParameters)
	{
	}
}
