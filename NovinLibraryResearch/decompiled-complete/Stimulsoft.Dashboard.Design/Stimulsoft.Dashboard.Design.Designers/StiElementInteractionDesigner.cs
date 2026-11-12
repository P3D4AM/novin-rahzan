using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiElementInteractionDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object comp)
	{
		IStiElementInteraction stiElementInteraction = comp as IStiElementInteraction;
		return new StiElementInteractionEditorForm(stiElementInteraction as StiElement, stiElementInteraction?.DashboardInteraction as StiDashboardInteraction);
	}

	public StiElementInteractionDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
