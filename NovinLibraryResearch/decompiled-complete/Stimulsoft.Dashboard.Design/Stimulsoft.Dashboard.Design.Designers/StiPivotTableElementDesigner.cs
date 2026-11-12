using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiPivotTableElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiPivotTableElementDesignerForm((StiPivotTableElement)component);
	}

	public StiPivotTableElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
