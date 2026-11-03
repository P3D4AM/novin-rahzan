using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiPageV2Builder : StiContainerV2Builder
{
	public override void Prepare(StiComponent masterComp)
	{
		StiPage stiPage = masterComp as StiPage;
		base.Prepare((StiComponent)stiPage);
		stiPage.PageInfoV2.IsReportTitlesRendered = false;
		stiPage.PageInfoV2.RenderedCount = 0;
		stiPage.PageInfoV2.Overlays = new StiComponentsCollection();
		lock (((ICollection)stiPage.Components).SyncRoot)
		{
			foreach (StiComponent component2 in stiPage.Components)
			{
				if (component2 is StiOverlayBand component)
				{
					stiPage.PageInfoV2.Overlays.Add(component);
				}
			}
		}
	}

	public override void UnPrepare(StiComponent masterComp)
	{
		base.UnPrepare(masterComp);
		StiPage stiPage = masterComp as StiPage;
		stiPage.PageInfoV2.Overlays = null;
	}
}
