using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCanBreakComponentInContainerCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiCanBreakComponentInContainerCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiCanBreakComponentInContainerCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiContainer { CanBreak: false } stiContainer)
		{
			foreach (StiComponent component in stiContainer.Components)
			{
				if (component is IStiBreakable && ((IStiBreakable)component).CanBreak)
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
		try
		{
			if (Check())
			{
				StiCanBreakComponentInContainerCheck stiCanBreakComponentInContainerCheck = new StiCanBreakComponentInContainerCheck();
				stiCanBreakComponentInContainerCheck.Element = obj;
				stiCanBreakComponentInContainerCheck.Actions.Add(new StiCanBreakComponentInContainerAction());
				return stiCanBreakComponentInContainerCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
