using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiHierarchicalBandV1Builder : StiDataBandV1Builder
{
	protected override void ProcessRenderedContainer(StiDataBand masterDataBand, StiContainer container)
	{
		base.ProcessRenderedContainer(masterDataBand, container);
		StiHierarchicalBand stiHierarchicalBand = masterDataBand as StiHierarchicalBand;
		if (container != null && !stiHierarchicalBand.IsDataSourceEmpty && !masterDataBand.DataSource.IsEmpty)
		{
			int level = masterDataBand.DataSource.GetLevel();
			if (level > 0)
			{
				double num = (double)level * stiHierarchicalBand.Report.Unit.ConvertFromHInches(stiHierarchicalBand.Indent);
				foreach (StiComponent component in container.Components)
				{
					if (!component.Locked)
					{
						component.Left += num;
					}
				}
			}
		}
		if (container == null || stiHierarchicalBand.IsBusinessObjectEmpty || masterDataBand.BusinessObject.IsEmpty)
		{
			return;
		}
		int level2 = masterDataBand.BusinessObject.GetLevel();
		if (level2 <= 0)
		{
			return;
		}
		double num2 = (double)level2 * stiHierarchicalBand.Report.Unit.ConvertFromHInches(stiHierarchicalBand.Indent);
		foreach (StiComponent component2 in container.Components)
		{
			if (!component2.Locked)
			{
				component2.Left += num2;
			}
		}
	}
}
