using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiHeaderBandV1Builder : StiBandV1Builder
{
	internal static void SecondPassBreak(StiHeaderBand masterHeaderBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		masterHeaderBand.HeaderBandInfoV1.IsFirstPassOfBreak = true;
		if (masterHeaderBand.HeaderBandInfoV1.BreakableComps != null)
		{
			masterHeaderBand.HeaderBandInfoV1.IsFirstPassOfBreak = false;
			StiComponentDivider.ProcessPreviousBreakedContainer(renderedComponent as StiContainer, masterHeaderBand.HeaderBandInfoV1.BreakableComps, ref freeSpace);
			masterHeaderBand.HeaderBandInfoV1.BreakableComps = null;
		}
	}

	internal static void FirstPassBreak(StiHeaderBand masterHeaderBand, StiComponent renderedComponent, StiComponent masterComponent, ref double freeSpace)
	{
		renderedComponent.Height += freeSpace;
		masterHeaderBand.HeaderBandInfoV1.BreakableComps = StiComponentDivider.BreakContainer(renderedComponent as StiContainer);
	}

	public static StiComponent GetMaster(StiHeaderBand masterHeaderBand)
	{
		if (masterHeaderBand.Parent == null)
		{
			return null;
		}
		for (int i = masterHeaderBand.Parent.Components.IndexOf(masterHeaderBand) + 1; i < masterHeaderBand.Parent.Components.Count; i++)
		{
			if (masterHeaderBand.Parent.Components[i] is StiDataBand && !(masterHeaderBand.Parent.Components[i] is StiEmptyBand))
			{
				return masterHeaderBand.Parent.Components[i];
			}
		}
		return null;
	}

	public static bool PrintOddEven(IStiPrintOnEvenOddPages component, StiPrintOnEvenOddPagesType printOnEvenOddPagesType)
	{
		int num = ((StiComponent)component).Report.PageNumber & 1;
		if (component.PrintOnEvenOddPages == StiPrintOnEvenOddPagesType.Ignore)
		{
			return true;
		}
		if (component.PrintOnEvenOddPages == StiPrintOnEvenOddPagesType.PrintOnEvenPages && num == 0)
		{
			return true;
		}
		if (component.PrintOnEvenOddPages == StiPrintOnEvenOddPagesType.PrintOnOddPages && num == 1)
		{
			return true;
		}
		return false;
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiHeaderBand stiHeaderBand = masterComp as StiHeaderBand;
		if (stiHeaderBand.CanBreak)
		{
			stiHeaderBand.HeaderBandInfoV1.ForceCanBreak = true;
			return;
		}
		foreach (StiComponent component in stiHeaderBand.Components)
		{
			if (component is IStiCrossTab)
			{
				stiHeaderBand.HeaderBandInfoV1.ForceCanBreak = true;
				break;
			}
		}
	}

	public override bool InternalRender(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiHeaderBand stiHeaderBand = masterComp as StiHeaderBand;
		if (!PrintOddEven(stiHeaderBand, stiHeaderBand.PrintOnEvenOddPages))
		{
			return true;
		}
		bool result = base.InternalRender((StiComponent)stiHeaderBand, ref renderedComponent, outContainer);
		if (stiHeaderBand is StiColumnHeaderBand)
		{
			StiDataBand stiDataBand = GetMaster(stiHeaderBand) as StiDataBand;
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
					stiHeaderBand.Report.Column = i + 1;
				}
				if ((i >= stiDataBand.Count || stiHeaderBand.PrintIfEmpty) && !stiHeaderBand.PrintIfEmpty)
				{
					break;
				}
				StiContainer stiContainer = new StiContainer
				{
					Name = stiHeaderBand.Name,
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
				base.InternalRender((StiComponent)stiHeaderBand, ref renderedComponent2, stiContainer);
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
				stiHeaderBand.Report.Column = 1;
			}
			return true;
		}
		return result;
	}

	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiHeaderBand stiHeaderBand = masterComp as StiHeaderBand;
		stiHeaderBand.RenderedCount++;
		stiHeaderBand.DoBookmark();
		return InternalRender(stiHeaderBand, ref renderedComponent, outContainer);
	}
}
