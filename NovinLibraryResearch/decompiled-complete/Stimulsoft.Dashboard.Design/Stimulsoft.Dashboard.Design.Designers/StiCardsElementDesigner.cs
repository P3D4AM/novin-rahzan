using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiCardsElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiCardsElementEditorForm(component as StiElement);
	}

	public StiCardsElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
