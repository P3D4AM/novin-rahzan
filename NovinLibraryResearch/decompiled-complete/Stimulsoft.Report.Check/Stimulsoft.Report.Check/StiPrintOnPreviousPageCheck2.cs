using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPrintOnPreviousPageCheck2 : StiPageCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiPrintOnPreviousPageCheck2Short");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPrintOnPreviousPageCheck2Long"), base.Element);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiPage { PrintOnPreviousPage: not false } stiPage)
		{
			foreach (StiComponent component in stiPage.Components)
			{
				if (!(component is StiBand))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (Check())
		{
			StiPrintOnPreviousPageCheck2 stiPrintOnPreviousPageCheck = new StiPrintOnPreviousPageCheck2();
			stiPrintOnPreviousPageCheck.Element = obj;
			return stiPrintOnPreviousPageCheck;
		}
		return null;
	}
}
