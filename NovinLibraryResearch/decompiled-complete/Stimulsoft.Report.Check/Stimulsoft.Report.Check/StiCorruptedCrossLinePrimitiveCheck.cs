using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCorruptedCrossLinePrimitiveCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage
	{
		get
		{
			StiComponent stiComponent = base.Element as StiComponent;
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiCorruptedCrossLinePrimitiveCheckShort"), ElementName);
		}
	}

	public override string LongMessage
	{
		get
		{
			StiComponent stiComponent = base.Element as StiComponent;
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiCorruptedCrossLinePrimitiveCheckLong"), ElementName);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiCrossLinePrimitive stiCrossLinePrimitive)
		{
			StiStartPointPrimitive startPoint = stiCrossLinePrimitive.GetStartPoint(stiCrossLinePrimitive.Page);
			StiEndPointPrimitive endPoint = stiCrossLinePrimitive.GetEndPoint(stiCrossLinePrimitive.Page);
			if (startPoint == null || endPoint == null)
			{
				StiCorruptedCrossLinePrimitiveCheck stiCorruptedCrossLinePrimitiveCheck = new StiCorruptedCrossLinePrimitiveCheck();
				stiCorruptedCrossLinePrimitiveCheck.Element = obj;
				stiCorruptedCrossLinePrimitiveCheck.Actions.Add(new StiFixCrossLinePrimitiveAction());
				stiCorruptedCrossLinePrimitiveCheck.Actions.Add(new StiDeleteComponentAction());
				return stiCorruptedCrossLinePrimitiveCheck;
			}
			return null;
		}
		return null;
	}
}
