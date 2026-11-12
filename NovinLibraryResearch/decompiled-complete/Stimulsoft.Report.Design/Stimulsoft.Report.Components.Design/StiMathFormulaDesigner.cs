using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Components.Design;

public class StiMathFormulaDesigner : StiInplaceDesigner
{
	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiMathFormulaEditorForm(component as StiMathFormula);
	}

	public StiMathFormulaDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
