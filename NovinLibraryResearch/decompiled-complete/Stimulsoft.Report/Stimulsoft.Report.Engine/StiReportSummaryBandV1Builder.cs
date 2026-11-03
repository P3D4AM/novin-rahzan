using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiReportSummaryBandV1Builder : StiBandV1Builder
{
	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		StiReportSummaryBand stiReportSummaryBand = masterComp as StiReportSummaryBand;
		if (!stiReportSummaryBand.PrintIfEmpty)
		{
			bool flag = true;
			foreach (StiComponent component in stiReportSummaryBand.Parent.Components)
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
		foreach (StiComponent component2 in stiReportSummaryBand.Components)
		{
			if (component2.IsCross)
			{
				component2.Prepare();
			}
		}
		RectangleD dockRegion = stiReportSummaryBand.GetDockRegion(outContainer);
		double height = ((RectangleD)(ref dockRegion)).Height;
		bool result = stiReportSummaryBand.RenderContainer(ref renderedComponent, outContainer);
		if (height < renderedComponent.Height)
		{
			outContainer.Components.Remove(renderedComponent);
			result = false;
		}
		else if (stiReportSummaryBand.PrintAtBottom)
		{
			renderedComponent.DockStyle = StiDockStyle.Bottom;
		}
		else
		{
			renderedComponent.DockStyle = StiDockStyle.Top;
		}
		renderedComponent.InvokeEvents();
		return result;
	}
}
