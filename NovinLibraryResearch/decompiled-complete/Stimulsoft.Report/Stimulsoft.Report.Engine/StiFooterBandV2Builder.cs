using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiFooterBandV2Builder : StiBandV2Builder
{
	public static StiDataBand GetMaster(StiFooterBand masterFooterBand)
	{
		for (int num = masterFooterBand.Parent.Components.IndexOf(masterFooterBand) - 1; num >= 0; num--)
		{
			if (masterFooterBand.Parent.Components[num] is StiDataBand)
			{
				return masterFooterBand.Parent.Components[num] as StiDataBand;
			}
		}
		return null;
	}
}
