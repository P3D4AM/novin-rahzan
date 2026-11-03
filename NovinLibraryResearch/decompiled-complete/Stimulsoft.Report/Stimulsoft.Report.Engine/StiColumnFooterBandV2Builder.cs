using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiColumnFooterBandV2Builder : StiFooterBandV2Builder
{
	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiColumnFooterBand stiColumnFooterBand = masterComp as StiColumnFooterBand;
		StiDataBand master = StiFooterBandV2Builder.GetMaster(stiColumnFooterBand);
		if (master == null || master.Columns < 2)
		{
			return base.InternalRender((StiComponent)stiColumnFooterBand);
		}
		StiContainer stiContainer = new StiContainer
		{
			Name = stiColumnFooterBand.Name,
			Width = stiColumnFooterBand.Width,
			Height = stiColumnFooterBand.Height
		};
		double num = master.GetColumnWidth() + master.ColumnGaps;
		for (int i = 0; i < master.Columns; i++)
		{
			stiColumnFooterBand.Report.Column = i + 1;
			if ((master.Position + i >= master.Count || stiColumnFooterBand.PrintIfEmpty) && !stiColumnFooterBand.PrintIfEmpty)
			{
				break;
			}
			StiContainer stiContainer2 = base.InternalRender((StiComponent)stiColumnFooterBand) as StiContainer;
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
		return stiContainer;
	}
}
