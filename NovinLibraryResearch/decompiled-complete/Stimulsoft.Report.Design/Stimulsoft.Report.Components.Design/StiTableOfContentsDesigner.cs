using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Components.Design;

public class StiTableOfContentsDesigner : StiInplaceDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiTableOfContentsEditorForm(component as StiTableOfContents);
	}

	public override bool IsAllowCenterWindow(object component)
	{
		return true;
	}

	public StiTableOfContentsDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
