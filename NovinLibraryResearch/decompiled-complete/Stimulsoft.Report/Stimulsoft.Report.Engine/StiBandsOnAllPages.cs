using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiBandsOnAllPages
{
	private class BandItem
	{
		public StiDataBand DataBand { get; }

		public StiBand Band { get; }

		public BandItem(StiDataBand dataBand, StiBand band)
		{
			DataBand = dataBand;
			Band = band;
		}
	}

	private List<BandItem> bands = new List<BandItem>();

	internal StiEngine Engine { get; }

	public bool DenyRendering { get; set; }

	internal void Add(StiDataBand dataBand, StiBand band)
	{
		bands.Add(new BandItem(dataBand, band));
	}

	internal void Remove(StiDataBand dataBand)
	{
		int num = 0;
		while (num < bands.Count)
		{
			BandItem bandItem = bands[num];
			if (bandItem.DataBand == dataBand)
			{
				bands.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	private bool AllowRenderBand(StiBand band)
	{
		if (Engine.BandsInProgress.Count == 0)
		{
			return true;
		}
		StiBand stiBand = Engine.BandsInProgress[Engine.BandsInProgress.Count - 1];
		if (band == stiBand && !(band is StiHierarchicalBand))
		{
			return false;
		}
		if (!(stiBand is StiGroupHeaderBand) || !(band is StiGroupHeaderBand))
		{
			return true;
		}
		if (((StiGroupHeaderBand)stiBand).GetDataBand() != ((StiGroupHeaderBand)band).GetDataBand())
		{
			return true;
		}
		int num = band.Parent.Components.IndexOf(band);
		int num2 = band.Parent.Components.IndexOf(stiBand);
		return num <= num2;
	}

	internal void Render()
	{
		if (DenyRendering)
		{
			return;
		}
		bool denyRendering = DenyRendering;
		try
		{
			DenyRendering = true;
			Engine.DenyClearPrintOnAllPagesIgnoreList = true;
			lock (((ICollection)bands).SyncRoot)
			{
				foreach (BandItem band2 in bands)
				{
					StiBand band = band2.Band;
					if (!AllowRenderBand(band))
					{
						continue;
					}
					StiComponentsCollection stiComponentsCollection = RenderBand(band, ignorePageBreaks: true, ignoreRenderingEvents: false);
					if (stiComponentsCollection == null)
					{
						continue;
					}
					lock (((ICollection)stiComponentsCollection).SyncRoot)
					{
						foreach (StiContainer item in stiComponentsCollection)
						{
							if (item.ContainerInfoV2.ParentBand != null)
							{
								item.ContainerInfoV2.IsAutoRendered = true;
								bool flag = !(band is StiHierarchicalBand) || !((StiHierarchicalBand)band).PrintOnAllPages;
								if (band is StiDataBand)
								{
									flag = false;
								}
								if (flag)
								{
									Engine.PrintOnAllPagesIgnoreList[item.ContainerInfoV2.ParentBand] = item.ContainerInfoV2.ParentBand;
								}
							}
						}
					}
				}
			}
		}
		finally
		{
			Engine.DenyClearPrintOnAllPagesIgnoreList = false;
			DenyRendering = denyRendering;
		}
	}

	private StiComponentsCollection RenderBand(StiBand band, bool ignorePageBreaks, bool ignoreRenderingEvents)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		if (!(band is StiHierarchicalBand))
		{
			return Engine.RenderBand(band, ignorePageBreaks, ignoreRenderingEvents);
		}
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		StiHierarchicalBand stiHierarchicalBand = band as StiHierarchicalBand;
		stiHierarchicalBand.SaveState("TreeBandPrintOnAllPages");
		bool printOnAllPages = stiHierarchicalBand.PrintOnAllPages;
		stiHierarchicalBand.PrintOnAllPages = false;
		try
		{
			List<int> list = new List<int>();
			stiHierarchicalBand.Brush = (StiBrush)new StiSolidBrush(Color.Green);
			int num = stiHierarchicalBand.DataSource.GetLevel();
			stiHierarchicalBand.Prior();
			while (num >= 0 && !stiHierarchicalBand.IsBof)
			{
				int level = stiHierarchicalBand.DataSource.GetLevel();
				if (num > level)
				{
					list.Add(stiHierarchicalBand.Position);
					num = level;
				}
				stiHierarchicalBand.Prior();
			}
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				stiHierarchicalBand.Position = list[num2];
				StiComponentsCollection stiComponentsCollection2 = Engine.RenderBand(stiHierarchicalBand, ignorePageBreaks, ignoreRenderingEvents);
				lock (((ICollection)stiComponentsCollection2).SyncRoot)
				{
					foreach (StiComponent item in stiComponentsCollection2)
					{
						stiComponentsCollection.Add(item);
					}
				}
			}
			stiHierarchicalBand.Brush = (StiBrush)new StiSolidBrush(Color.Transparent);
			stiHierarchicalBand.RestoreState("TreeBandPrintOnAllPages");
			return stiComponentsCollection;
		}
		finally
		{
			stiHierarchicalBand.PrintOnAllPages = printOnAllPages;
		}
	}

	internal bool IsBandInBandsList(StiBand band)
	{
		if (band == null)
		{
			return false;
		}
		foreach (BandItem band2 in bands)
		{
			if (band2.Band == band)
			{
				return true;
			}
		}
		return false;
	}

	internal List<StiBand> GetBandsList()
	{
		List<StiBand> list = new List<StiBand>();
		foreach (BandItem band in bands)
		{
			list.Add(band.Band);
		}
		return list;
	}

	internal StiBandsOnAllPages(StiEngine engine)
	{
		Engine = engine;
	}
}
