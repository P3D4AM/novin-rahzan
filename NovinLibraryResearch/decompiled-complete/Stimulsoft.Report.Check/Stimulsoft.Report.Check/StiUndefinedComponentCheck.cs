using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiUndefinedComponentCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiUndefinedComponentCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiUndefinedComponentCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiUndefinedComponent)
		{
			StiUndefinedComponentCheck stiUndefinedComponentCheck = new StiUndefinedComponentCheck();
			stiUndefinedComponentCheck.Element = obj;
			stiUndefinedComponentCheck.Actions.Add(new StiDeleteComponentAction());
			return stiUndefinedComponentCheck;
		}
		return null;
	}
}
