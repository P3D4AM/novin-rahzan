using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiTotalPageCountDoublePassCheck : StiReportCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiIsFirstPageIsLastPageDoublePassCheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckComponent", "StiTotalPageCountDoublePassCheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		StiReport stiReport = base.Element as StiReport;
		if (stiReport.NumberOfPass != StiNumberOfPass.DoublePass)
		{
			string[] array = new string[2] { "TotalPageCount", "TotalPageCountThrough" };
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
			StiTotalPageCountDoublePassCheck stiTotalPageCountDoublePassCheck = new StiTotalPageCountDoublePassCheck();
			stiTotalPageCountDoublePassCheck.Element = obj;
			stiTotalPageCountDoublePassCheck.Actions.Add(new StiAllowDoublePassAction());
			return stiTotalPageCountDoublePassCheck;
		}
		return null;
	}
}
