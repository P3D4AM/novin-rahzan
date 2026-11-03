using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiFootersOnAllPages
{
	private Hashtable bands = new Hashtable();

	private StiEngine Engine { get; }

	public void Add(StiContainer container)
	{
		bands[container] = container;
	}

	public bool CanProcess(StiBand band)
	{
		if (band is StiFooterBand stiFooterBand)
		{
			return stiFooterBand.PrintOnAllPages;
		}
		return false;
	}

	public void Render(StiContainer outContainer, int startIndex, ref StiContainer markerContainer)
	{
		if (outContainer == null || bands.Count == 0)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		List<StiContainer> list = new List<StiContainer>();
		StiDataBand stiDataBand = null;
		bool flag = true;
		for (int i = startIndex; i < outContainer.Components.Count; i++)
		{
			if (!(outContainer.Components[i] is StiContainer stiContainer))
			{
				continue;
			}
			if (stiContainer.ContainerInfoV2.ParentBand is StiDataBand)
			{
				stiDataBand = stiContainer.ContainerInfoV2.ParentBand as StiDataBand;
			}
			if (stiContainer == markerContainer)
			{
				flag = false;
			}
			if (flag && bands[stiContainer] != null)
			{
				if (stiContainer.ContainerInfoV2.ParentBand is StiFooterBand)
				{
					((StiFooterBand)stiContainer.ContainerInfoV2.ParentBand).InvokeMoveFooterToBottom();
				}
				ArrayList arrayList = hashtable[stiContainer.Name] as ArrayList;
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					hashtable[stiContainer.Name] = arrayList;
				}
				arrayList.Add(stiContainer);
				list.Add(stiContainer);
			}
			if (!(stiContainer is StiFooterMarkerContainer) || !(hashtable[stiContainer.Name] is ArrayList { Count: >0 } arrayList2))
			{
				continue;
			}
			StiContainer stiContainer2 = arrayList2[arrayList2.Count - 1] as StiContainer;
			int index = outContainer.Components.IndexOf(stiContainer2);
			arrayList2.RemoveAt(arrayList2.Count - 1);
			outContainer.Components.RemoveAt(i);
			outContainer.Components.Insert(i, stiContainer2);
			outContainer.Components.RemoveAt(index);
			i--;
			list.Remove(stiContainer2);
			double height = stiContainer2.Height;
			stiContainer2.Top = stiContainer.Top;
			Engine.PositionY += stiContainer2.Height;
			for (int j = i + 1; j < outContainer.Components.Count; j++)
			{
				if (outContainer.Components[j] is StiContainer stiContainer3)
				{
					if (stiContainer3 == markerContainer)
					{
						break;
					}
					stiContainer3.Top += height;
				}
			}
			stiContainer2.ContainerInfoV2.IsAutoRendered = true;
			if (stiContainer == markerContainer)
			{
				markerContainer = stiContainer2;
			}
		}
		lock (((ICollection)list).SyncRoot)
		{
			foreach (StiContainer item in list)
			{
				StiComponent stiComponent = null;
				if (item.ContainerInfoV2.ParentBand is StiFooterBand)
				{
					foreach (StiComponent component in outContainer.Components)
					{
						if (component == item || !(component is StiContainer) || hashtable.ContainsKey(component.Name))
						{
							continue;
						}
						StiContainer stiContainer4 = component as StiContainer;
						if (stiContainer4.ContainerInfoV2 == null || (!(stiContainer4.ContainerInfoV2.ParentBand is StiFooterBand) && !(stiContainer4.ContainerInfoV2.ParentBand is StiReportSummaryBand)))
						{
							continue;
						}
						if (stiContainer4.ContainerInfoV2.ParentBand is StiReportSummaryBand)
						{
							stiComponent = component;
							break;
						}
						if (stiDataBand != null)
						{
							foreach (StiFooterBand footersOnAllPage in stiDataBand.DataBandInfoV2.FootersOnAllPages)
							{
								if (!(footersOnAllPage.Name != component.Name))
								{
									stiComponent = component;
									break;
								}
							}
							foreach (StiFooterBand item2 in stiDataBand.DataBandInfoV2.FootersOnLastPage)
							{
								if (!(item2.Name != component.Name))
								{
									stiComponent = component;
									int num = item.ContainerInfoV2.ParentBand.Parent.Components.IndexOf(item.ContainerInfoV2.ParentBand);
									int num2 = item2.Parent.Components.IndexOf(item2);
									if (num == -1 || num2 == -1 || num <= num2)
									{
										break;
									}
									int num3 = outContainer.Components.IndexOf(component);
									if (num3 == -1)
									{
										break;
									}
									if (num3 == outContainer.Components.Count - 1)
									{
										stiComponent = null;
										break;
									}
									stiComponent = outContainer.Components[num3 + 1];
								}
							}
							continue;
						}
						stiComponent = component;
						break;
					}
				}
				if (stiComponent != null)
				{
					outContainer.Components.Remove(item);
					int num4 = outContainer.Components.IndexOf(stiComponent);
					outContainer.Components.Insert(num4, item);
					item.Top = stiComponent.Top;
					for (int k = num4 + 1; k < outContainer.Components.Count; k++)
					{
						outContainer.Components[k].Top += item.Height;
					}
					Engine.PositionY += item.Height;
				}
				else
				{
					outContainer.Components.Remove(item);
					outContainer.Components.Add(item);
					item.Top = Engine.PositionY;
					Engine.PositionY += item.Height;
				}
				item.ContainerInfoV2.IsAutoRendered = true;
			}
		}
		bands.Clear();
	}

	internal StiFootersOnAllPages(StiEngine engine)
	{
		Engine = engine;
	}
}
