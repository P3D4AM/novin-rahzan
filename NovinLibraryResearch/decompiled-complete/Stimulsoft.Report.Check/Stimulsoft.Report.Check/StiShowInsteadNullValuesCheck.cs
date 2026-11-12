using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiShowInsteadNullValuesCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiShowInsteadNullValuesCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiShowInsteadNullValuesCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiText { NullValue: not null } stiText && stiText.NullValue.Length > 0 && stiText.Text.Value.Length == 0)
		{
			return true;
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (Check())
			{
				StiShowInsteadNullValuesCheck stiShowInsteadNullValuesCheck = new StiShowInsteadNullValuesCheck();
				stiShowInsteadNullValuesCheck.Element = obj;
				return stiShowInsteadNullValuesCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
