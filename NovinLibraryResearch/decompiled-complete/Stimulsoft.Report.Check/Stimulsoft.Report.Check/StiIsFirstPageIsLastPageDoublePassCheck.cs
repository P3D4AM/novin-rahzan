using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiIsFirstPageIsLastPageDoublePassCheck : StiReportCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiIsFirstPageIsLastPageDoublePassCheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckComponent", "StiIsFirstPageIsLastPageDoublePassCheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		StiReport stiReport = base.Element as StiReport;
		if (stiReport.NumberOfPass != StiNumberOfPass.DoublePass)
		{
			string[] array = new string[4] { "IsFirstPage", "IsFirstPageThrough", "IsLastPage", "IsLastPageThrough" };
			for (int i = 0; i < array.Length; i++)
			{
				if (stiReport.Script.IndexOf(array[i]) != -1)
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
			StiIsFirstPageIsLastPageDoublePassCheck stiIsFirstPageIsLastPageDoublePassCheck = new StiIsFirstPageIsLastPageDoublePassCheck();
			stiIsFirstPageIsLastPageDoublePassCheck.Element = obj;
			stiIsFirstPageIsLastPageDoublePassCheck.Actions.Add(new StiAllowDoublePassAction());
			return stiIsFirstPageIsLastPageDoublePassCheck;
		}
		return null;
	}
}
