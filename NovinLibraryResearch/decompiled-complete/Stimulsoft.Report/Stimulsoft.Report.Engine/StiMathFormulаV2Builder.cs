using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiMathFormulаV2Builder : StiComponentV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiComponent stiComponent = base.InternalRender(masterComp);
		StiMathFormula stiMathFormula = stiComponent as StiMathFormula;
		stiMathFormula.Value = stiMathFormula.GetFormulaString();
		return stiMathFormula;
	}
}
