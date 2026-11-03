using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiHeaderBandV2Builder : StiBandV2Builder
{
	public static StiDataBand GetMaster(StiHeaderBand masterHeaderBand)
	{
		for (int i = masterHeaderBand.Parent.Components.IndexOf(masterHeaderBand) + 1; i < masterHeaderBand.Parent.Components.Count; i++)
		{
			if (masterHeaderBand.Parent.Components[i] is StiDataBand)
			{
				return masterHeaderBand.Parent.Components[i] as StiDataBand;
			}
		}
		return null;
	}
}
