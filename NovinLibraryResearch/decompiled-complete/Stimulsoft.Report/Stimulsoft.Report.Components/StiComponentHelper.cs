namespace Stimulsoft.Report.Components;

public class StiComponentHelper
{
	public static void FillComponentPlacement(StiComponent component)
	{
		StiContainer stiContainer = component.Parent;
		while (stiContainer != null && !(stiContainer is StiPage) && (!(stiContainer is StiBand) || stiContainer.IsCross))
		{
			stiContainer = stiContainer.Parent;
		}
		if (stiContainer == null)
		{
			return;
		}
		if (stiContainer is StiPage)
		{
			if (StiOptions.Export.OptimizeDataOnlyMode && component is StiBand)
			{
				stiContainer = component as StiContainer;
			}
			else
			{
				component.ComponentPlacement = "p";
			}
		}
		if (stiContainer is StiBand)
		{
			if (stiContainer is StiChildBand && stiContainer.Parent != null)
			{
				StiContainer parent = stiContainer.Parent;
				int num = parent.Components.IndexOf(stiContainer);
				if (num > 0)
				{
					while (num > 0 && (parent.Components[num] is StiChildBand || !(parent.Components[num] is StiBand)))
					{
						num--;
					}
					if (parent.Components[num] is StiBand)
					{
						stiContainer = (StiContainer)parent.Components[num];
					}
				}
			}
			if (stiContainer is StiReportTitleBand)
			{
				component.ComponentPlacement = "rt";
			}
			if (stiContainer is StiReportSummaryBand)
			{
				component.ComponentPlacement = "rs";
			}
			if (stiContainer is StiPageHeaderBand)
			{
				component.ComponentPlacement = "ph";
			}
			if (stiContainer is StiPageFooterBand)
			{
				component.ComponentPlacement = "pf";
			}
			if (stiContainer is StiHeaderBand)
			{
				component.ComponentPlacement = (((StiHeaderBand)stiContainer).PrintOnAllPages ? "h.ap" : "h");
			}
			if (stiContainer is StiFooterBand)
			{
				component.ComponentPlacement = (((StiFooterBand)stiContainer).PrintOnAllPages ? "f.ap" : "f");
			}
			if (stiContainer is StiDataBand)
			{
				component.ComponentPlacement = "d";
			}
			if (stiContainer is StiGroupHeaderBand)
			{
				component.ComponentPlacement = "gh";
				StiGroupHeaderBand stiGroupHeaderBand = stiContainer as StiGroupHeaderBand;
				if (stiGroupHeaderBand.GroupHeaderBandInfoV2.IsTableGroupHeader)
				{
					component.ComponentPlacement = (stiGroupHeaderBand.PrintOnAllPages ? "h.ap" : "h");
				}
			}
			if (stiContainer is StiGroupFooterBand)
			{
				component.ComponentPlacement = "gf";
			}
			if (stiContainer is StiEmptyBand)
			{
				component.ComponentPlacement = "e";
			}
		}
		if (component.ComponentPlacement.Length > 0)
		{
			component.ComponentPlacement += (component.Parent.IsCross ? ("." + stiContainer.Name) : ("." + component.Parent.Name));
		}
	}
}
