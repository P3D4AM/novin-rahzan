using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiTextInCellsV2Builder : StiSimpleTextV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiTextInCells masterTextInCells = masterComp as StiTextInCells;
		StiComponent stiComponent = base.InternalRender(masterComp);
		string textInternal = (stiComponent as StiText).GetTextInternal();
		if (textInternal == null || !textInternal.StartsWith("#%#"))
		{
			return StiTextInCells.SplitByCells(masterTextInCells, stiComponent, textInternal, measure: true);
		}
		return stiComponent;
	}
}
