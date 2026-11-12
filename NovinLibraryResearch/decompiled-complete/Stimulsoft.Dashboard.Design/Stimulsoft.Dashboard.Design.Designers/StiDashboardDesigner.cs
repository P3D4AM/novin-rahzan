using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiDashboardDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiDashboardEditorForm(component as StiDashboard);
	}

	public StiDashboardDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
