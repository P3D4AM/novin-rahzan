using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGroupHeaderNotEqualToGroupFooterOnPageCheck : StiPageCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiGroupHeaderNotEqualToGroupFooterShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiGroupHeaderNotEqualToGroupFooterOnPageLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiPage stiPage && !(stiPage is IStiForm))
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < stiPage.Components.Count; i++)
			{
				if (stiPage.Components[i] is StiGroupHeaderBand)
				{
					num++;
				}
				else if (stiPage.Components[i] is StiGroupFooterBand)
				{
					num2++;
				}
			}
			if (num < num2)
			{
				return true;
			}
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
				StiGroupHeaderNotEqualToGroupFooterOnPageCheck stiGroupHeaderNotEqualToGroupFooterOnPageCheck = new StiGroupHeaderNotEqualToGroupFooterOnPageCheck();
				stiGroupHeaderNotEqualToGroupFooterOnPageCheck.Element = obj;
				return stiGroupHeaderNotEqualToGroupFooterOnPageCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
