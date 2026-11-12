using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiContourTextObsoleteCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiContourTextObsoleteCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiContourTextObsoleteCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj.GetType() == typeof(StiContourText))
		{
			StiContourTextObsoleteCheck stiContourTextObsoleteCheck = new StiContourTextObsoleteCheck();
			stiContourTextObsoleteCheck.Element = obj;
			stiContourTextObsoleteCheck.Actions.Add(new StiConversionContourTextInTextAction());
			return stiContourTextObsoleteCheck;
		}
		return null;
	}
}
