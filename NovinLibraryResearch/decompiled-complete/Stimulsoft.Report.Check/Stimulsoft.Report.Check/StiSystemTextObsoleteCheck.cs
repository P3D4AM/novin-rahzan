using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiSystemTextObsoleteCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiSystemTextObsoleteCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiSystemTextObsoleteCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj.GetType() == typeof(StiSystemText))
		{
			StiSystemTextObsoleteCheck stiSystemTextObsoleteCheck = new StiSystemTextObsoleteCheck();
			stiSystemTextObsoleteCheck.Element = obj;
			stiSystemTextObsoleteCheck.Actions.Add(new StiConversionSystemTextInTextAction());
			return stiSystemTextObsoleteCheck;
		}
		return null;
	}
}
