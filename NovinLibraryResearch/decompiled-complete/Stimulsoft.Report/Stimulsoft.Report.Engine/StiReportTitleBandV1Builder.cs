using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiReportTitleBandV1Builder : StiBandV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiReportTitleBand stiReportTitleBand = masterComp as StiReportTitleBand;
		if (stiReportTitleBand.ResetPageNumber && !stiReportTitleBand.IsRendered)
		{
			stiReportTitleBand.Report.EngineV1.ResetPageNumber();
		}
		if (!stiReportTitleBand.PrintIfEmpty)
		{
			bool flag = true;
			foreach (StiComponent component in stiReportTitleBand.Parent.Components)
			{
				if (component is StiDataBand && !((StiDataBand)component).IsEmpty)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return true;
			}
		}
		foreach (StiComponent component2 in stiReportTitleBand.Components)
		{
			if (component2.IsCross)
			{
				component2.Prepare();
			}
		}
		if (!stiReportTitleBand.IsRendered)
		{
			return base.InternalRender((StiComponent)stiReportTitleBand, ref renderedComponent, outContainer);
		}
		return true;
	}
}
