using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiTreeViewElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiTreeViewElementEditorForm(component as StiElement);
	}

	public StiTreeViewElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
