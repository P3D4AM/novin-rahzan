using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiOnlineMapElementDesigner : StiElementDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiOnlineMapElementEditorForm(component as StiElement);
	}

	public StiOnlineMapElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
