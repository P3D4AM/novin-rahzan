using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiDatePickerElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiDatePickerElementEditorForm(component as StiElement);
	}

	public StiDatePickerElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
