using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiGroupHeaderNotEqualToGroupFooterShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiCrossGroupHeaderNotEqualToGroupCrossFooterOnContainerLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiContainer stiContainer)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < stiContainer.Components.Count; i++)
			{
				if (stiContainer.Components[i] is StiCrossGroupHeaderBand)
				{
					num++;
				}
				else if (stiContainer.Components[i] is StiCrossGroupFooterBand)
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
				StiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck stiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck = new StiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck();
				stiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck.Element = obj;
				return stiCrossGroupHeaderNotEqualToCrossGroupFooterOnContainerCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
