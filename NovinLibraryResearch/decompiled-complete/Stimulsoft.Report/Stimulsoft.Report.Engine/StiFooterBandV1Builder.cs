using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiFooterBandV1Builder : StiBandV1Builder
{
	internal static void SecondPassBreak(StiFooterBand masterFooterBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		masterFooterBand.FooterBandInfoV1.IsFirstPassOfBreak = true;
		if (masterFooterBand.FooterBandInfoV1.BreakableComps != null)
		{
			masterFooterBand.FooterBandInfoV1.IsFirstPassOfBreak = false;
			StiComponentDivider.ProcessPreviousBreakedContainer(renderedComponent as StiContainer, masterFooterBand.FooterBandInfoV1.BreakableComps, ref freeSpace);
			masterFooterBand.FooterBandInfoV1.BreakableComps = null;
		}
	}

	internal static void FirstPassBreak(StiFooterBand masterFooterBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		renderedComponent.Height += freeSpace;
		masterFooterBand.FooterBandInfoV1.BreakableComps = StiComponentDivider.BreakContainer(renderedComponent as StiContainer);
	}

	public static StiComponent GetMaster(StiFooterBand masterFooterBand)
	{
		for (int num = masterFooterBand.Parent.Components.IndexOf(masterFooterBand) - 1; num >= 0; num--)
		{
			if (masterFooterBand.Parent.Components[num] is StiDataBand && !(masterFooterBand.Parent.Components[num] is StiEmptyBand))
			{
				return masterFooterBand.Parent.Components[num];
			}
		}
		return null;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiFooterBand stiFooterBand = masterComp as StiFooterBand;
		if (stiFooterBand.CanBreak)
		{
			stiFooterBand.FooterBandInfoV1.ForceCanBreak = true;
			return;
		}
		foreach (StiComponent component in stiFooterBand.Components)
		{
			if (component is IStiCrossTab)
			{
				stiFooterBand.FooterBandInfoV1.ForceCanBreak = true;
				break;
			}
		}
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiFooterBand stiFooterBand = masterComp as StiFooterBand;
		if (!StiHeaderBandV1Builder.PrintOddEven(stiFooterBand, stiFooterBand.PrintOnEvenOddPages))
		{
			return true;
		}
		bool result = base.InternalRender((StiComponent)stiFooterBand, ref renderedComponent, outContainer);
		if (stiFooterBand is StiColumnFooterBand)
		{
			StiDataBand stiDataBand = GetMaster(stiFooterBand) as StiDataBand;
			double columnWidth = stiDataBand.GetColumnWidth();
			double columnGaps = stiDataBand.ColumnGaps;
			int num = stiDataBand.Columns;
			if (num < 1)
			{
				num = 1;
			}
			((StiContainer)renderedComponent).Components.Clear();
			for (int i = 0; i < num; i++)
			{
				if (num > 1)
				{
					stiFooterBand.Report.Column = i + 1;
				}
				if ((i >= stiDataBand.Count || stiFooterBand.PrintIfEmpty) && !stiFooterBand.PrintIfEmpty)
				{
					break;
				}
				StiContainer stiContainer = new StiContainer
				{
					Name = stiFooterBand.Name,
					Width = columnWidth + columnGaps
				};
				((StiContainer)renderedComponent).Components.Add(stiContainer);
				if (stiDataBand.RightToLeft)
				{
					stiContainer.DockStyle = StiDockStyle.Right;
				}
				else
				{
					stiContainer.DockStyle = StiDockStyle.Left;
				}
				StiComponent renderedComponent2 = null;
				base.InternalRender((StiComponent)stiFooterBand, ref renderedComponent2, stiContainer);
				if (!stiDataBand.RightToLeft || !(renderedComponent2 is StiContainer))
				{
					continue;
				}
				foreach (StiComponent component in (renderedComponent2 as StiContainer).Components)
				{
					component.Left += columnGaps;
				}
			}
			if (num > 1)
			{
				stiFooterBand.Report.Column = 1;
			}
			return true;
		}
		return result;
	}

	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiFooterBand stiFooterBand = masterComp as StiFooterBand;
		stiFooterBand.RenderedCount++;
		stiFooterBand.DoBookmark();
		return InternalRender(stiFooterBand, ref renderedComponent, outContainer);
	}
}
