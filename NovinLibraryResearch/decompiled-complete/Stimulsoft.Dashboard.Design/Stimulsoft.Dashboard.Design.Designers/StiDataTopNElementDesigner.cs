using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiDataTopNElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object comp)
	{
		StiElement element = comp as StiElement;
		return new StiDataTopNEditorForm(element);
	}

	public StiDataTopNElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
