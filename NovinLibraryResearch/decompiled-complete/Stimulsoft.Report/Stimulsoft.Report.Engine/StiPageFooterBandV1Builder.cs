using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiPageFooterBandV1Builder : StiBandV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiPageFooterBand stiPageFooterBand = masterComp as StiPageFooterBand;
		if (!StiHeaderBandV1Builder.PrintOddEven(stiPageFooterBand, stiPageFooterBand.PrintOnEvenOddPages))
		{
			return true;
		}
		foreach (StiComponent component in stiPageFooterBand.Components)
		{
			if (component.IsCross)
			{
				component.Prepare();
			}
		}
		bool result = base.InternalRender((StiComponent)stiPageFooterBand, ref renderedComponent, outContainer);
		renderedComponent.DockStyle = StiDockStyle.Bottom;
		return result;
	}
}
