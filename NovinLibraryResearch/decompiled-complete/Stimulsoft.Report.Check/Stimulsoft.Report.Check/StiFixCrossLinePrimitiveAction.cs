using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiFixCrossLinePrimitiveAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Fix");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiFixCrossLinePrimitiveActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		if (element is StiCrossLinePrimitive stiCrossLinePrimitive)
		{
			StiStartPointPrimitive startPoint = stiCrossLinePrimitive.GetStartPoint(stiCrossLinePrimitive.Page);
			StiEndPointPrimitive endPoint = stiCrossLinePrimitive.GetEndPoint(stiCrossLinePrimitive.Page);
			double left = stiCrossLinePrimitive.Left;
			double top = stiCrossLinePrimitive.Top;
			double bottom = stiCrossLinePrimitive.Bottom;
			double right = stiCrossLinePrimitive.Right;
			if (startPoint == null)
			{
				startPoint = new StiStartPointPrimitive();
				startPoint.ReferenceToGuid = stiCrossLinePrimitive.Guid;
				startPoint.Left = left;
				startPoint.Top = top;
				startPoint.Name = StiNameCreation.CreateName(report, StiNameCreation.GenerateName(startPoint), addOne: true, removeIncorrectSymbols: true, forceAdvancedNamingRule: true);
				stiCrossLinePrimitive.Page.Components.Add(startPoint);
			}
			if (endPoint == null)
			{
				endPoint = new StiEndPointPrimitive();
				endPoint.ReferenceToGuid = stiCrossLinePrimitive.Guid;
				endPoint.Left = right;
				endPoint.Top = bottom;
				endPoint.Name = StiNameCreation.CreateName(report, StiNameCreation.GenerateName(endPoint), addOne: true, removeIncorrectSymbols: true, forceAdvancedNamingRule: true);
				stiCrossLinePrimitive.Page.Components.Add(endPoint);
			}
			stiCrossLinePrimitive.Page.Correct();
		}
	}
}
