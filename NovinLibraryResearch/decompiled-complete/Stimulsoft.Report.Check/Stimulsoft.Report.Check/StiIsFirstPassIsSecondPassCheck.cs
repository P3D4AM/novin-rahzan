using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiIsFirstPassIsSecondPassCheck : StiReportCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiIsFirstPassIsSecondPassCheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckComponent", "StiIsFirstPassIsSecondPassCheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		StiReport stiReport = base.Element as StiReport;
		if (stiReport.NumberOfPass == StiNumberOfPass.SinglePass)
		{
			string[] array = new string[2] { "IsFirstPass", "IsSecondPass" };
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
			StiIsFirstPassIsSecondPassCheck stiIsFirstPassIsSecondPassCheck = new StiIsFirstPassIsSecondPassCheck();
			stiIsFirstPassIsSecondPassCheck.Element = obj;
			stiIsFirstPassIsSecondPassCheck.Actions.Add(new StiAllowDoublePassAction());
			return stiIsFirstPassIsSecondPassCheck;
		}
		return null;
	}
}
