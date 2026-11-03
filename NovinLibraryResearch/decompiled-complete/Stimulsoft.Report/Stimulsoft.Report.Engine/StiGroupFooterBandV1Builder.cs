using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiGroupFooterBandV1Builder : StiBandV1Builder
{
	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiGroupFooterBand stiGroupFooterBand = masterComp as StiGroupFooterBand;
		int line = stiGroupFooterBand.Report.Line;
		stiGroupFooterBand.Report.Line = stiGroupFooterBand.GroupFooterBandInfoV1.GroupHeader.Line;
		stiGroupFooterBand.RenderedCount++;
		bool result = InternalRender(stiGroupFooterBand, ref renderedComponent, outContainer);
		stiGroupFooterBand.Report.Line = line;
		return result;
	}
}
