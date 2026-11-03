using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiPointPrimitiveV2Builder : StiComponentV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiPointPrimitive stiPointPrimitive = masterComp as StiPointPrimitive;
		StiPointPrimitive stiPointPrimitive2 = base.InternalRender((StiComponent)stiPointPrimitive) as StiPointPrimitive;
		stiPointPrimitive2.StoredColumn = stiPointPrimitive.Report.Column;
		return stiPointPrimitive2;
	}
}
