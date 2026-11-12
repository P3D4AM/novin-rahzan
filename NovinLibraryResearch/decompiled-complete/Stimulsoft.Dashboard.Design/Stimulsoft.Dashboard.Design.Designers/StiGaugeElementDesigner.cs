using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiGaugeElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiGaugeElementEditorForm(component as StiElement);
	}

	public StiGaugeElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
