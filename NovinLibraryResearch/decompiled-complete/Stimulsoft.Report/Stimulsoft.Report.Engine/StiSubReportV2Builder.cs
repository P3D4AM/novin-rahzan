using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiSubReportV2Builder : StiContainerV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiSubReport stiSubReport = masterComp as StiSubReport;
		if (stiSubReport.SubReportPage != null)
		{
			stiSubReport.SubReportPage.InvokeBeforePrint(stiSubReport.SubReportPage, EventArgs.Empty);
		}
		StiContainer stiContainer = base.InternalRender((StiComponent)stiSubReport) as StiContainer;
		stiContainer.CanBreak = true;
		stiContainer.CanGrow = true;
		stiContainer.CanShrink = true;
		if (stiSubReport.Parent is StiPage)
		{
			stiContainer.CanGrow = false;
		}
		if (StiSubReportsHelper.GetParentBand(stiSubReport) != null)
		{
			StiSubReportsHelper.RenderSubReport(stiContainer, stiSubReport);
			StiContainerHelper.CheckSize(stiContainer);
		}
		else
		{
			stiContainer.CanShrink = false;
		}
		if (stiSubReport.SubReportPage != null)
		{
			stiSubReport.SubReportPage.InvokeAfterPrint(stiSubReport.SubReportPage, EventArgs.Empty);
		}
		return stiContainer;
	}
}
