using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCanGrowGrowToHeightComponentInContainerCheck : StiComponentCheck
{
	public override bool DefaultStateEnabled => false;

	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiCanGrowGrowToHeightComponentInContainerShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiCanGrowGrowToHeightComponentInContainerLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiContainer stiContainer && stiContainer.Components.Count > 1)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (StiComponent component in stiContainer.Components)
			{
				if (!(component is StiBand))
				{
					if (component.CanGrow)
					{
						flag = true;
					}
					if (!component.GrowToHeight)
					{
						flag2 = true;
					}
					if (flag && flag2)
					{
						return true;
					}
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
				StiCanGrowGrowToHeightComponentInContainerCheck stiCanGrowGrowToHeightComponentInContainerCheck = new StiCanGrowGrowToHeightComponentInContainerCheck();
				stiCanGrowGrowToHeightComponentInContainerCheck.Element = obj;
				stiCanGrowGrowToHeightComponentInContainerCheck.Actions.Add(new StiCanGrowGrowToHeightComponentInContainerAction());
				return stiCanGrowGrowToHeightComponentInContainerCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
