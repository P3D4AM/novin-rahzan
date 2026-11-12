using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiListBoxElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiListBoxElementEditorForm(component as StiElement);
	}

	public StiListBoxElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
