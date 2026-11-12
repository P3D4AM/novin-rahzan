using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiComponentStyleIsNotFoundAtPageCheck : StiPageCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiComponentStyleIsNotFoundCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiComponentStyleIsNotFoundCheckAtPageLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiPage stiPage && !(stiPage is IStiForm) && !string.IsNullOrWhiteSpace(stiPage.ComponentStyle) && !stiPage.Report.Styles.Contains(stiPage.ComponentStyle))
		{
			return true;
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (Check())
		{
			StiComponentStyleIsNotFoundAtPageCheck stiComponentStyleIsNotFoundAtPageCheck = new StiComponentStyleIsNotFoundAtPageCheck();
			stiComponentStyleIsNotFoundAtPageCheck.Element = obj;
			stiComponentStyleIsNotFoundAtPageCheck.Actions.Add(new StiComponentStyleIsNotFoundAtComponentAction());
			return stiComponentStyleIsNotFoundAtPageCheck;
		}
		return null;
	}
}
