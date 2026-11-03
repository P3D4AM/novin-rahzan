using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiHierarchicalBandV2Builder : StiDataBandV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiHierarchicalBand stiHierarchicalBand = masterComp as StiHierarchicalBand;
		if (IsCollapsed(stiHierarchicalBand))
		{
			return new StiContainer();
		}
		StiContainer stiContainer = base.InternalRender((StiComponent)stiHierarchicalBand) as StiContainer;
		if (stiContainer != null)
		{
			if (!stiHierarchicalBand.IsDataSourceEmpty && !stiHierarchicalBand.DataSource.IsEmpty)
			{
				int level = stiHierarchicalBand.DataSource.GetLevel();
				if (level > 0)
				{
					CreateIndention(stiHierarchicalBand, stiContainer, level);
				}
			}
			if (!stiHierarchicalBand.IsBusinessObjectEmpty && !stiHierarchicalBand.BusinessObject.IsEmpty)
			{
				int level2 = stiHierarchicalBand.BusinessObject.GetLevel();
				if (level2 > 0)
				{
					CreateIndention(stiHierarchicalBand, stiContainer, level2);
				}
			}
		}
		return stiContainer;
	}

	private bool IsCollapsed(StiHierarchicalBand band)
	{
		if (band.CollapsingIndex == 0 || band.Position == 0)
		{
			return false;
		}
		int collapsingIndex = band.CollapsingIndex;
		int position = band.Position;
		int num = 0;
		bool flag = !band.IsBusinessObjectEmpty;
		object currentObject = null;
		if (!band.IsDataSourceEmpty)
		{
			num = band.DataSource.GetLevel();
		}
		if (flag)
		{
			num = band.BusinessObject.GetLevel();
		}
		try
		{
			if (flag)
			{
				int num2 = band.Position;
				currentObject = band.BusinessObject.Current;
				band.Position = 0;
				ArrayList arrayList = new ArrayList();
				while (arrayList.Count < num2 && !band.IsEof)
				{
					arrayList.Add(band.BusinessObject.Current);
					band.Next();
				}
				do
				{
					if (band.CollapsingIndex == 0 || num2 == 0)
					{
						return false;
					}
					band.CollapsingIndex--;
					num2--;
					band.BusinessObject.currentObject = arrayList[num2];
				}
				while (num <= band.BusinessObject.GetLevel() || !StiDataBandV2Builder.IsCollapsed(band, isRendering: false));
				return true;
			}
			if (!band.IsDataSourceEmpty)
			{
				do
				{
					if (band.CollapsingIndex == 0 || band.Position == 0)
					{
						return false;
					}
					band.CollapsingIndex--;
					band.Position--;
				}
				while (num <= band.DataSource.GetLevel() || !StiDataBandV2Builder.IsCollapsed(band, isRendering: false));
				return true;
			}
			return false;
		}
		finally
		{
			if (flag)
			{
				band.BusinessObject.currentObject = currentObject;
			}
			band.CollapsingIndex = collapsingIndex;
			band.Position = position;
		}
	}

	internal static void CreateIndention(StiHierarchicalBand masterHierarchical, StiContainer container, int level)
	{
		double num = (double)level * masterHierarchical.Report.Unit.ConvertFromHInches(masterHierarchical.Indent);
		lock (((ICollection)container.Components).SyncRoot)
		{
			foreach (StiComponent component in container.Components)
			{
				if (!component.Locked)
				{
					component.Left += num;
				}
			}
		}
	}
}
