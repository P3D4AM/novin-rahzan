using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCanGrowComponentInContainerCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiCanGrowComponentInContainerCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiCanGrowComponentInContainerCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiContainer { CanGrow: false } stiContainer && stiContainer.Parent != stiContainer.Page)
		{
			foreach (StiComponent component in stiContainer.Components)
			{
				if (component.CanGrow)
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
				StiCanGrowComponentInContainerCheck stiCanGrowComponentInContainerCheck = new StiCanGrowComponentInContainerCheck();
				stiCanGrowComponentInContainerCheck.Element = obj;
				stiCanGrowComponentInContainerCheck.Actions.Add(new StiCanGrowComponentInContainerAction());
				return stiCanGrowComponentInContainerCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
