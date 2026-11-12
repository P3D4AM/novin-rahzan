using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiDataFiltersElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object comp)
	{
		StiElement element = comp as StiElement;
		return new StiDataFiltersEditorForm(element);
	}

	public StiDataFiltersElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
