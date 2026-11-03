using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiColumnHeaderBandV2Builder : StiHeaderBandV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiColumnHeaderBand stiColumnHeaderBand = masterComp as StiColumnHeaderBand;
		StiDataBand master = StiHeaderBandV2Builder.GetMaster(stiColumnHeaderBand);
		if (master == null || master.Columns < 2)
		{
			return base.InternalRender((StiComponent)stiColumnHeaderBand);
		}
		StiContainer stiContainer = new StiContainer
		{
			Name = stiColumnHeaderBand.Name,
			Width = stiColumnHeaderBand.Width,
			Height = stiColumnHeaderBand.Height,
			CanGrow = stiColumnHeaderBand.CanGrow,
			CanShrink = stiColumnHeaderBand.CanShrink,
			ParentComponentIsBand = true
		};
		double num = master.GetColumnWidth() + master.ColumnGaps;
		for (int i = 0; i < master.Columns; i++)
		{
			stiColumnHeaderBand.Report.Column = i + 1;
			if ((master.Position + i >= master.Count || stiColumnHeaderBand.PrintIfEmpty) && !stiColumnHeaderBand.PrintIfEmpty)
			{
				break;
			}
			StiContainer stiContainer2 = base.InternalRender(masterComp) as StiContainer;
			stiContainer2.DockStyle = StiDockStyle.None;
			stiContainer2.Width = num;
			stiContainer.Components.Add(stiContainer2);
			if (master.RightToLeft)
			{
				stiContainer2.Left = stiContainer.Width - num * (double)(i + 1) + master.ColumnGaps;
			}
			else
			{
				stiContainer2.Left = num * (double)i;
			}
			stiContainer2.Top = 0.0;
		}
		double width = stiContainer.Width;
		StiContainerHelper.CheckSize(stiContainer);
		if (!stiContainer.ParentComponentIsCrossBand)
		{
			stiContainer.Width = width;
		}
		return stiContainer;
	}
}
