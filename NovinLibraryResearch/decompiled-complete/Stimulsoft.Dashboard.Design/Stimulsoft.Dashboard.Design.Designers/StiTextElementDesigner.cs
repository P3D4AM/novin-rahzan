using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiTextElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiTextElementEditorForm(component as StiElement);
	}

	public StiTextElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
