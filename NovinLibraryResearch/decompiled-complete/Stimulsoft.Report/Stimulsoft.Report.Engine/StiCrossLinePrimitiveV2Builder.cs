using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiCrossLinePrimitiveV2Builder : StiComponentV2Builder
{
	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		if (masterComp.PrintOn != 0)
		{
			StiStartPointPrimitive startPoint = (masterComp as StiCrossLinePrimitive).GetStartPoint();
			if (startPoint != null)
			{
				startPoint.PrintOn = masterComp.PrintOn;
			}
			StiEndPointPrimitive endPoint = (masterComp as StiCrossLinePrimitive).GetEndPoint();
			if (endPoint != null)
			{
				endPoint.PrintOn = masterComp.PrintOn;
			}
		}
	}
}
