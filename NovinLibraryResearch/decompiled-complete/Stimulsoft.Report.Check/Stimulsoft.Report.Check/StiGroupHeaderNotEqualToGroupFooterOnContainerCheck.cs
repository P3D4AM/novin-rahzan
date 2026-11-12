using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGroupHeaderNotEqualToGroupFooterOnContainerCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiGroupHeaderNotEqualToGroupFooterShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiGroupHeaderNotEqualToGroupFooterOnContainerLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiContainer stiContainer)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < stiContainer.Components.Count; i++)
			{
				if (stiContainer.Components[i] is StiGroupHeaderBand)
				{
					num++;
				}
				else if (stiContainer.Components[i] is StiGroupFooterBand)
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
				StiGroupHeaderNotEqualToGroupFooterOnContainerCheck stiGroupHeaderNotEqualToGroupFooterOnContainerCheck = new StiGroupHeaderNotEqualToGroupFooterOnContainerCheck();
				stiGroupHeaderNotEqualToGroupFooterOnContainerCheck.Element = obj;
				return stiGroupHeaderNotEqualToGroupFooterOnContainerCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
