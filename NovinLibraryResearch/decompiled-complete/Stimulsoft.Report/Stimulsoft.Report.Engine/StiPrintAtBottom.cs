using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiPrintAtBottom
{
	private Hashtable bands = new Hashtable();

	internal StiEngine Engine { get; }

	public bool CanProcess(StiBand band)
	{
		if (band is IStiPrintAtBottom stiPrintAtBottom)
		{
			return stiPrintAtBottom.PrintAtBottom;
		}
		return false;
	}

	public void Add(StiContainer container)
	{
		bands[container] = container;
	}

	public void Render(StiContainer outContainer, int startIndex, StiContainer markerContainer)
	{
		if (outContainer == null || bands.Count == 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		int num = -1;
		bool flag = false;
		for (int i = startIndex; i < outContainer.Components.Count; i++)
		{
			if (!(outContainer.Components[i] is StiContainer stiContainer) || stiContainer is StiLevelContainer)
			{
				continue;
			}
			if (stiContainer == markerContainer)
			{
				flag = true;
			}
			if ((!(stiContainer.ContainerInfoV2.ParentBand is StiFooterBand) || !((StiFooterBand)stiContainer.ContainerInfoV2.ParentBand).PrintOnAllPages) && flag)
			{
				continue;
			}
			if (bands[stiContainer] != null)
			{
				arrayList.Insert(0, stiContainer);
				if (num == -1)
				{
					num = i;
				}
			}
			else
			{
				if (num == -1 || stiContainer is StiFooterMarkerContainer)
				{
					continue;
				}
				arrayList.Clear();
				double num2 = 0.0;
				for (int j = num; j < outContainer.Components.Count; j++)
				{
					StiContainer stiContainer2 = outContainer.Components[j] as StiContainer;
					if (stiContainer2 == markerContainer)
					{
						break;
					}
					stiContainer2.Top += num2;
					if (bands[stiContainer2] != null && j < i && (!(stiContainer2.ContainerInfoV2.ParentBand is StiFooterBand) || !((StiFooterBand)stiContainer2.ContainerInfoV2.ParentBand).PrintOnAllPages))
					{
						num2 += stiContainer2.Height;
						Engine.PositionY += stiContainer2.Height;
					}
				}
				num = -1;
			}
		}
		foreach (StiContainer item in arrayList)
		{
			item.Top = Engine.PositionBottomY - item.Height;
			if (Engine.Page != null && Engine.Page.SegmentPerHeight > 1)
			{
				item.Top += (Engine.Page.PageHeight - Engine.Page.Margins.Top - Engine.Page.Margins.Bottom) * (double)(Engine.Page.SegmentPerHeight - 1);
			}
			Engine.PositionBottomY -= item.Height;
			if (item.ContainerInfoV2.ParentBand is StiFooterBand && ((StiFooterBand)item.ContainerInfoV2.ParentBand).PrintOnAllPages)
			{
				Engine.PositionY -= item.Height;
			}
		}
		bands.Clear();
	}

	internal StiPrintAtBottom(StiEngine engine)
	{
		Engine = engine;
	}
}
