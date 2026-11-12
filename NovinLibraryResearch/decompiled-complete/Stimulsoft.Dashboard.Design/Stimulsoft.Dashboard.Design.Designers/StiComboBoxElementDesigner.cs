using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiComboBoxElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiComboBoxElementEditorForm(component as StiElement);
	}

	public StiComboBoxElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
