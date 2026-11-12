using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Components.Design;

public class StiSparklineDesigner : StiInplaceDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiSparklineEditorForm(component as StiSparkline);
	}

	public StiSparklineDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
