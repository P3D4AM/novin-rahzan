using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoConditionAtGroupCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiNoConditionAtGroupCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiNoConditionAtGroupCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is IStiGroup && string.IsNullOrEmpty(((IStiGroup)base.Element).Condition.Value))
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
				StiNoConditionAtGroupCheck stiNoConditionAtGroupCheck = new StiNoConditionAtGroupCheck();
				stiNoConditionAtGroupCheck.Element = obj;
				stiNoConditionAtGroupCheck.Actions.Add(new StiDeleteComponentAction());
				return stiNoConditionAtGroupCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
