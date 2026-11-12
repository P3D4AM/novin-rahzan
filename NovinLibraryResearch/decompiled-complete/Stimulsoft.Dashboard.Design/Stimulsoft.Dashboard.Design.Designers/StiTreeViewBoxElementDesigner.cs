using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiTreeViewBoxElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiTreeViewBoxElementEditorForm(component as StiElement);
	}

	public StiTreeViewBoxElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
