using System.Collections;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;

namespace Stimulsoft.Report.Engine;

public class StiBandV2Builder : StiContainerV2Builder
{
	public static StiComponentsCollection GetChildBands(StiBand masterBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		for (int i = masterBand.Parent.Components.IndexOf(masterBand) + 1; i < masterBand.Parent.Components.Count && masterBand.Parent.Components[i] is StiChildBand; i++)
		{
			stiComponentsCollection.Add(masterBand.Parent.Components[i] as StiChildBand);
		}
		return stiComponentsCollection;
	}

	public static StiComponentsCollection GetSubReports(StiBand masterBand)
	{
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		lock (((ICollection)masterBand.Components).SyncRoot)
		{
			foreach (StiComponent component in masterBand.Components)
			{
				if (component is StiSubReport)
				{
					stiComponentsCollection.Add(component as StiSubReport);
				}
			}
			return stiComponentsCollection;
		}
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiBand stiBand = masterComp as StiBand;
		StiComponentsCollection components = stiBand.GetComponents();
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent item in components)
			{
				if (item is StiSubReport stiSubReport)
				{
					if (!stiSubReport.KeepSubReportTogether)
					{
						stiBand.BandInfoV2.ForceCanBreak = true;
					}
					stiBand.BandInfoV2.ForceCanGrow = true;
					break;
				}
				if (item is StiCrossTab stiCrossTab)
				{
					if (!stiCrossTab.KeepCrossTabTogether)
					{
						stiBand.BandInfoV2.ForceCanBreak = true;
					}
					stiBand.BandInfoV2.ForceCanGrow = true;
					break;
				}
			}
		}
	}
}
