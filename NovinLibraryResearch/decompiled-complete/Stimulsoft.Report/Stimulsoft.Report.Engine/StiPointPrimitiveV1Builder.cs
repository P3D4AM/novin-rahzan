using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiPointPrimitiveV1Builder : StiComponentV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		bool result = base.InternalRender(masterComp, ref renderedComponent, outContainer);
		((StiPointPrimitive)renderedComponent).StoredColumn = masterComp.Report.Column;
		return result;
	}
}
