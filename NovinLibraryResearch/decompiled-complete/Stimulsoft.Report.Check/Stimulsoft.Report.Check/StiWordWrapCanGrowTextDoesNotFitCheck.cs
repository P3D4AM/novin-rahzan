using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiWordWrapCanGrowTextDoesNotFitCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiWordWrapCanGrowTextDoesNotFitShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiWordWrapCanGrowTextDoesNotFitLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		StiText stiText = base.Element as StiText;
		if (stiText != null && stiText.Text.Value != null && (stiText.OnlyText || !stiText.Text.Value.Contains("{")))
		{
			return false;
		}
		if (stiText != null && stiText.WordWrap && !stiText.CanGrow && string.IsNullOrEmpty(stiText.RenderTo))
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
				StiWordWrapCanGrowTextDoesNotFitCheck stiWordWrapCanGrowTextDoesNotFitCheck = new StiWordWrapCanGrowTextDoesNotFitCheck();
				stiWordWrapCanGrowTextDoesNotFitCheck.Element = obj;
				stiWordWrapCanGrowTextDoesNotFitCheck.Actions.Add(new StiWordWrapCanGrowTextDoesNotFitAction());
				return stiWordWrapCanGrowTextDoesNotFitCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
