using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Design.CrossTab;

public class StiCrossTabDesigner : StiInplaceDesigner
{
	public override bool IsAllowCenterWindow(object component)
	{
		return true;
	}

	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiCrossTabDesignerForm(component as StiCrossTab);
	}

	public StiCrossTabDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
