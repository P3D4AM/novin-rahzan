using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiRegionMapElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiRegionMapElementEditorForm(component as StiElement);
	}

	public StiRegionMapElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
