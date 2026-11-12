using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiPrintOnPreviousPageCheck : StiPageCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiPrintOnPreviousPageCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiPrintOnPreviousPageCheckLong"), base.Element);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private int GetPageCount(StiReport report)
	{
		int num = 0;
		foreach (StiPage page in report.Pages)
		{
			if (!(page is IStiForm))
			{
				num++;
			}
		}
		return num;
	}

	private bool Check()
	{
		StiPage stiPage = base.Element as StiPage;
		StiReport report = stiPage.Report;
		if (stiPage != null && stiPage.PrintOnPreviousPage && (GetPageCount(report) == 1 || report.Pages.IndexOf(stiPage) == 0))
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
			StiPrintOnPreviousPageCheck stiPrintOnPreviousPageCheck = new StiPrintOnPreviousPageCheck();
			stiPrintOnPreviousPageCheck.Element = obj;
			stiPrintOnPreviousPageCheck.Actions.Add(new StiPrintOnPreviousPageAction());
			return stiPrintOnPreviousPageCheck;
		}
		return null;
	}
}
