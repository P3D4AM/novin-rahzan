using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiOrientationPageCheck : StiPageCheck
{
	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckPage", "StiOrientationPageCheckShort"), ElementName);

	public override string LongMessage
	{
		get
		{
			if (base.Element is StiPage stiPage)
			{
				if (stiPage.Orientation == StiPageOrientation.Portrait)
				{
					return string.Format(StiLocalizationExt.Get("CheckPage", "StiOrientationPageCheckLongPortrait"), ElementName);
				}
				return string.Format(StiLocalizationExt.Get("CheckPage", "StiOrientationPageCheckLongLandscape"), ElementName);
			}
			return string.Format(StiLocalizationExt.Get("CheckPage", "StiOrientationPageCheckLongPortrait"), ElementName);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		StiPage stiPage = obj as StiPage;
		bool flag = false;
		if (stiPage.Orientation == StiPageOrientation.Portrait)
		{
			if (stiPage.PageWidth > stiPage.PageHeight)
			{
				flag = true;
			}
		}
		else if (stiPage.PageWidth < stiPage.PageHeight)
		{
			flag = true;
		}
		if (stiPage is StiForm)
		{
			flag = false;
		}
		if (flag)
		{
			StiOrientationPageCheck stiOrientationPageCheck = new StiOrientationPageCheck();
			stiOrientationPageCheck.Element = obj;
			if (stiPage.Orientation == StiPageOrientation.Portrait)
			{
				stiOrientationPageCheck.Actions.Add(new StiOrientationPageToLandscapeAction());
			}
			else
			{
				stiOrientationPageCheck.Actions.Add(new StiOrientationPageToPortraitAction());
			}
			stiOrientationPageCheck.Actions.Add(new StiSwitchWidthAndHeightOfPageAction());
			return stiOrientationPageCheck;
		}
		return null;
	}
}
