using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoNamePageCheck : StiPageCheck
{
	private StiPage Page => base.Element as StiPage;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiNoNamePageCheckShort"), Page.Report.Pages.IndexOf(Page));

	public override string LongMessage
	{
		get
		{
			if (Page != null)
			{
				return string.Format(StiLocalizationExt.Get("CheckComponent", "StiNoNamePageCheckLong"), Page.Report.Pages.IndexOf(Page));
			}
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiNoNamePageCheckLong"), "");
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		StiComponent stiComponent = obj as StiComponent;
		bool flag = false;
		if (string.IsNullOrEmpty(stiComponent.Name))
		{
			flag = true;
		}
		if (flag)
		{
			StiNoNamePageCheck stiNoNamePageCheck = new StiNoNamePageCheck();
			stiNoNamePageCheck.Element = obj;
			stiNoNamePageCheck.Actions.Add(new StiGenerateNewNamePageAction());
			stiNoNamePageCheck.Actions.Add(new StiDeletePageAction());
			return stiNoNamePageCheck;
		}
		return null;
	}
}
