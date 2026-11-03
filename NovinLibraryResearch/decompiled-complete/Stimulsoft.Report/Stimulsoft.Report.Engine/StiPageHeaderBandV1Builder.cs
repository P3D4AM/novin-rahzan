using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiPageHeaderBandV1Builder : StiBandV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiPageHeaderBand stiPageHeaderBand = masterComp as StiPageHeaderBand;
		if (!StiHeaderBandV1Builder.PrintOddEven(stiPageHeaderBand, stiPageHeaderBand.PrintOnEvenOddPages))
		{
			return true;
		}
		foreach (StiComponent component in stiPageHeaderBand.Components)
		{
			if (component.IsCross)
			{
				component.Prepare();
			}
		}
		return base.InternalRender((StiComponent)stiPageHeaderBand, ref renderedComponent, outContainer);
	}
}
