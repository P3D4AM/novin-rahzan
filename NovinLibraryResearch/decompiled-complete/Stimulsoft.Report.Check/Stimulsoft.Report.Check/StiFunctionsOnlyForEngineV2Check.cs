using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiFunctionsOnlyForEngineV2Check : StiReportCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiFunctionsOnlyForEngineV2CheckShort");

	public override string LongMessage => StiLocalizationExt.Get("CheckComponent", "StiFunctionsOnlyForEngineV2CheckLong");

	public override StiCheckStatus Status => StiCheckStatus.Error;

	private bool Check()
	{
		StiReport stiReport = base.Element as StiReport;
		if (stiReport.EngineVersion == StiEngineVersion.EngineV1)
		{
			string[] array = new string[6] { "GroupLine", "PageNumberThrough", "PageNofMThrough", "TotalPageCountThrough", "IsFirstPageThrough", "IsLastPageThrough" };
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
			StiFunctionsOnlyForEngineV2Check stiFunctionsOnlyForEngineV2Check = new StiFunctionsOnlyForEngineV2Check();
			stiFunctionsOnlyForEngineV2Check.Element = obj;
			stiFunctionsOnlyForEngineV2Check.Actions.Add(new StiApplyEngineV2Action());
			return stiFunctionsOnlyForEngineV2Check;
		}
		return null;
	}
}
