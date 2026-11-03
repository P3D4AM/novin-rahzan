using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;

namespace Stimulsoft.Report.Engine;

public class StiCrossTabV2Builder : StiContainerV2Builder
{
	public static string GetCollapsingName(StiCrossHeader header)
	{
		return GetCollapsingName(header.Name, header.HeaderLevel, header.HeaderValue);
	}

	public static string GetCollapsingName(string componentName, int level, string value)
	{
		return $"{componentName}.{level}.{value}";
	}

	public static bool IsCollapsed(StiCrossHeader masterHeader)
	{
		return IsCollapsed(masterHeader, masterHeader.HeaderLevel, masterHeader.HeaderValue);
	}

	public static bool IsCollapsed(StiCrossHeader masterHeader, int level, string textValue)
	{
		if (StiOptions.Engine.ForceDisableCollapsing)
		{
			return false;
		}
		if (masterHeader.Interaction == null || (masterHeader.Interaction is StiCrossHeaderInteraction && !((StiCrossHeaderInteraction)masterHeader.Interaction).CollapsingEnabled))
		{
			return false;
		}
		if (masterHeader.Report.InteractionCollapsingStates == null)
		{
			return true;
		}
		object obj = masterHeader.Report.InteractionCollapsingStates[GetCollapsingName(masterHeader.Name, level, textValue)];
		if (obj is bool)
		{
			return (bool)obj;
		}
		return true;
	}

	public static void SetCollapsed(StiCrossHeader masterHeader, bool isCollapsed)
	{
		if (!StiOptions.Engine.ForceDisableCollapsing && masterHeader.Interaction != null && (!(masterHeader.Interaction is StiCrossHeaderInteraction) || ((StiCrossHeaderInteraction)masterHeader.Interaction).CollapsingEnabled))
		{
			if (masterHeader.Report.InteractionCollapsingStates == null)
			{
				masterHeader.Report.InteractionCollapsingStates = new Hashtable();
			}
			masterHeader.Report.InteractionCollapsingStates[GetCollapsingName(masterHeader)] = isCollapsed;
		}
	}

	internal static void MakeHorAlignment(StiCrossTab master, StiContainer destination, int startIndex, double parentWidth, int segmentPerWidth)
	{
		if (master.Wrap)
		{
			return;
		}
		if (master.HorAlignment == StiCrossHorAlignment.Width)
		{
			destination.Left = 0.0;
			destination.Width = parentWidth;
			StiCrossTabHelper.MakeHorAlignmentByWidth(destination, startIndex);
		}
		else
		{
			if (master.HorAlignment == StiCrossHorAlignment.None)
			{
				return;
			}
			destination.Left = 0.0;
			destination.Width = parentWidth * (double)((segmentPerWidth <= 1) ? 1 : segmentPerWidth);
			double maxLeft = 0.0;
			double maxRight = 0.0;
			StiCrossTabHelper.CalculateMaxAndMin(destination, ref maxLeft, ref maxRight, startIndex);
			for (int i = startIndex; i < destination.Components.Count; i++)
			{
				StiComponent stiComponent = destination.Components[i];
				switch (master.HorAlignment)
				{
				case StiCrossHorAlignment.Left:
					stiComponent.Left -= maxLeft;
					break;
				case StiCrossHorAlignment.Right:
					stiComponent.Left += destination.Width - maxRight;
					break;
				case StiCrossHorAlignment.Center:
				{
					double num = (destination.Width - (maxRight - maxLeft)) / 2.0;
					stiComponent.Left -= maxLeft - num;
					break;
				}
				}
			}
		}
	}

	private static void FinalizeCross(StiContainer renderedComponent)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		renderedComponent.CanGrow = false;
		renderedComponent.CanShrink = true;
		SizeD actualSize = renderedComponent.GetActualSize();
		renderedComponent.Width = ((SizeD)(ref actualSize)).Width;
		renderedComponent.Height = ((SizeD)(ref actualSize)).Height;
		renderedComponent.CanGrow = true;
	}

	public StiComponent RenderCrossTab(StiCrossTabParams pars, StiCrossTab master)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = pars.DestinationContainer;
		if (stiContainer == null)
		{
			stiContainer = base.InternalRender((StiComponent)master) as StiContainer;
			stiContainer.Components.Clear();
		}
		stiContainer.DockStyle = StiDockStyle.None;
		if (master.Page.UnlimitedHeight && !master.Page.UnlimitedBreakable)
		{
			if (master.Page.UnlimitedHeight && master.Page.UnlimitedBreakable)
			{
				stiContainer.CanBreak = false;
			}
		}
		else
		{
			stiContainer.CanBreak = true;
		}
		if (!pars.AllowRendering)
		{
			return stiContainer;
		}
		if ((master.DataSource == null || master.DataSource.IsEmpty) && !master.PrintIfEmpty)
		{
			pars.RenderingIsFinished = true;
			return stiContainer;
		}
		RectangleM destinationRectangle = pars.DestinationRectangle;
		((RectangleM)(ref destinationRectangle)).X = 0m;
		((RectangleM)(ref destinationRectangle)).Y = 0m;
		if (StiSubReportsHelper.GetParentBand(master) != null)
		{
			StiPage page = master.Page;
			if (!master.RightToLeft)
			{
				RenderCrossTabOnDataBand(pars, master, stiContainer, destinationRectangle);
			}
			else if (!page.UnlimitedBreakable)
			{
				decimal num = master.CrossTabInfo.Cross.Widths.Sum();
				decimal num2 = (decimal)(page.PageWidth - page.Margins.Left - page.Margins.Right);
				int num3 = (int)(num / num2) + 1;
				pars.ShiftX = (decimal)num3 * num2 - num;
				RenderCrossTabOnDataBand(pars, master, stiContainer, destinationRectangle);
			}
			else
			{
				master.RightToLeft = false;
				RenderCrossTabOnDataBand(pars, master, stiContainer, destinationRectangle);
				decimal num4 = (decimal)stiContainer.Components.ToList().Max((StiComponent c) => c.Right);
				decimal num5 = (decimal)(page.PageWidth - page.Margins.Left - page.Margins.Right);
				int num6 = (int)(num4 / num5) + 1;
				pars.ShiftX = (decimal)num6 * num5 - num4 - (decimal)stiContainer.Left;
				stiContainer.Components.Clear();
				master.RightToLeft = true;
				StiCrossTabHelper.MakeRightToLeft(master);
				master.CrossTabInfo.Cross.DoAutoSize();
				RenderCrossTabOnDataBand(pars, master, stiContainer, destinationRectangle);
			}
		}
		else if (master.Wrap)
		{
			RenderCrossTabWrapped(pars, master, stiContainer, destinationRectangle);
		}
		else if (master.Page.UnlimitedHeight)
		{
			((RectangleM)(ref destinationRectangle)).Width = 100000000m;
			((RectangleM)(ref destinationRectangle)).Height = 100000000m;
			if (master.Page.UnlimitedBreakable)
			{
				RenderCrossTabUnlimitedBreakable(pars, master, stiContainer, destinationRectangle);
			}
			else
			{
				RenderCrossTabUnlimited(pars, master, stiContainer, destinationRectangle);
			}
		}
		else
		{
			RenderCrossTabOnPage(pars, master, stiContainer, destinationRectangle, out var _, out var _);
		}
		return stiContainer;
	}

	public StiComponent RenderCrossTabOnDataBand(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, RectangleM rect)
	{
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		StiPage page = master.Page;
		StiCross cross = master.CrossTabInfo.Cross;
		StiEngine engine = master.Report.Engine;
		decimal num = (decimal)(page.PageWidth - page.Margins.Left - page.Margins.Right);
		decimal num2 = (page.UnlimitedBreakable ? num : 100000m);
		int count = destination.Components.Count;
		pars.StartRow = 0;
		bool flag = false;
		bool flag2 = true;
		decimal num3 = default(decimal);
		decimal num4 = default(decimal);
		decimal num5 = default(decimal);
		List<StiBand> bandsList = engine.BandsOnAllPages.GetBandsList();
		foreach (StiBand item in bandsList)
		{
			num5 += (decimal)item.Height;
		}
		foreach (StiComponent component in page.Components)
		{
			if (component is StiPageHeaderBand || component is StiPageFooterBand)
			{
				if ((component.PrintOn & StiPrintOnType.ExceptFirstPage) > StiPrintOnType.AllPages)
				{
					num5 += (decimal)component.Height;
				}
				if ((component.PrintOn & StiPrintOnType.OnlyFirstPage) > StiPrintOnType.AllPages)
				{
					num5 -= (decimal)component.Height;
				}
			}
		}
		RectangleM rect3 = default(RectangleM);
		while (pars.StartRow < cross.RowCount)
		{
			pars.StartColumn = ((!flag2) ? pars.StartColumn : 0);
			bool flag3 = master.KeepCrossTabTogether && master.Parent != null && !master.Parent.CanBreak;
			decimal num6 = ((pars.StartRow == 0 && !flag && !flag3) ? ((decimal)engine.FreeSpace - (decimal)master.Top) : ((decimal)engine.StaticBands.ReservedFreeSpace - num5));
			if (page.UnlimitedHeight && !page.UnlimitedBreakable)
			{
				num6 = 100000000m;
			}
			if (num6 < 0m)
			{
				num6 = (decimal)engine.StaticBands.ReservedFreeSpace;
			}
			if (pars.StartRow == 0 || !flag2)
			{
				RectangleM rect2 = rect;
				if (RenderColHeaders(pars, master, destination, ref rect2, measure: true, skipStartRowCheck: true) > num6)
				{
					if (!flag)
					{
						num4 = num6;
						num3 += num6;
						flag = true;
						continue;
					}
					num3 -= num4;
					num6 = num4;
				}
				else if (master.Parent != null && master.Parent != null && !((IStiBreakable)master.Parent).CanBreak)
				{
					num3 -= num4;
				}
			}
			decimal shiftX = pars.ShiftX;
			int endRow = 0;
			bool flag4 = true;
			while (pars.StartColumn < cross.ColCount)
			{
				((RectangleM)(ref rect3))._002Ector(shiftX, num3, num2 - (decimal)destination.Left - (flag4 ? pars.ShiftX : 0m), num6);
				RenderCrossTabSegment(pars, master, destination, rect3, out var endCol, out endRow);
				pars.StartColumn = endCol;
				if (master.Wrap)
				{
					flag2 = pars.StartColumn >= cross.ColCount;
					break;
				}
				if (flag4)
				{
					shiftX -= pars.ShiftX;
				}
				shiftX += num2;
				flag4 = false;
			}
			if (flag2)
			{
				num3 += num6;
				pars.StartRow = endRow;
			}
			if (destination.Components.Count > 0)
			{
				num3 = Math.Max(num3, (decimal)destination.Components.Cast<StiComponent>().Max((StiComponent c) => c.Bottom));
			}
			if (!flag2)
			{
				num3 += (decimal)master.WrapGap;
			}
		}
		if (destination.Components.Count > 0 && !master.Wrap && master.HorAlignment != StiCrossHorAlignment.Width)
		{
			decimal num7 = (decimal)(destination.Components.ToList().Max((StiComponent c) => c.Right) + master.Left) / num;
			if (num7 > 1m)
			{
				while (num7 > (decimal)destination.ContainerInfoV2.SetSegmentPerWidth)
				{
					destination.ContainerInfoV2.SetSegmentPerWidth++;
				}
			}
		}
		MakeHorAlignment(master, destination, count, master.Parent.Width, destination.ContainerInfoV2.SetSegmentPerWidth);
		FinalizeCross(destination);
		return destination;
	}

	private void RenderCrossTabWrapped(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, RectangleM rect)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		StiCross cross = master.CrossTabInfo.Cross;
		decimal width = ((RectangleM)(ref rect)).Width;
		while (pars.StartColumn < cross.ColCount)
		{
			RenderRowHeaders(pars, master, destination, ref rect);
			int endCol = StiCrossTabHelper.GetEndCol(master, pars.StartColumn, rect);
			int endRow = StiCrossTabHelper.GetEndRow(master, pars.StartRow, rect);
			endRow = StiCrossTabHelper.CheckMergedRowCells(master, pars.StartRow, endRow, pars.StartColumn, endCol);
			StiCrossTabHelper.RenderCells(master, destination, pars.StartColumn, pars.StartRow, endCol, endRow, rect);
			SizeM cellsRect = StiCrossTabHelper.GetCellsRect(master, pars.StartColumn, pars.StartRow, endCol, endRow);
			pars.RenderingIsFinished = endCol == cross.ColCount && endRow == cross.RowCount;
			if (pars.RenderingIsFinished)
			{
				return;
			}
			if (endCol == cross.ColCount)
			{
				pars.StartColumn = 0;
				pars.StartRow = endRow;
			}
			else
			{
				pars.StartColumn = endCol;
			}
			if (((RectangleM)(ref rect)).Height - (decimal)master.WrapGap - ((SizeM)(ref cellsRect)).Height > ((SizeM)(ref cellsRect)).Height)
			{
				((RectangleM)(ref rect)).Y = ((RectangleM)(ref rect)).Y + (((SizeM)(ref cellsRect)).Height + (decimal)master.WrapGap);
				((RectangleM)(ref rect)).Height = ((RectangleM)(ref rect)).Height - (((SizeM)(ref cellsRect)).Height + (decimal)master.WrapGap);
				((RectangleM)(ref rect)).X = 0m;
				((RectangleM)(ref rect)).Width = width;
				continue;
			}
			pars.RenderingIsFinished = false;
			return;
		}
		pars.RenderingIsFinished = true;
	}

	public void RenderCrossTabUnlimitedBreakable(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, RectangleM rect)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		StiPage stiPage = GetPageForCrossTab(destination) ?? master.Report.RenderedPages.ToList().LastOrDefault();
		int endCol = StiCrossTabHelper.GetEndCol(master, pars.StartColumn, rect, pars.ForceNoBreak);
		int endRow = StiCrossTabHelper.GetEndRow(master, pars.StartRow, rect, pars.ForceNoBreak);
		double width = stiPage.Width;
		double height = stiPage.Height;
		int num = 0;
		int num2 = 0;
		RectangleM rect2 = default(RectangleM);
		while (num2 < endRow)
		{
			stiPage.SegmentPerHeight = Math.Max(stiPage.SegmentPerHeight, num + 1);
			int num3 = 0;
			int endCol2 = 0;
			int endRow2 = 0;
			while (endCol2 < endCol)
			{
				stiPage.SegmentPerWidth = Math.Max(stiPage.SegmentPerWidth, num3 + 1);
				decimal num4 = ((RectangleM)(ref rect)).Left + (decimal)((double)num3 * width);
				decimal num5 = ((RectangleM)(ref rect)).Top + (decimal)((double)num * height);
				RectangleM destinationRectangle = pars.DestinationRectangle;
				decimal width2 = ((RectangleM)(ref destinationRectangle)).Width;
				destinationRectangle = pars.DestinationRectangle;
				((RectangleM)(ref rect2))._002Ector(num4, num5, width2, ((RectangleM)(ref destinationRectangle)).Height);
				pars.StartColumn = endCol2;
				pars.StartRow = num2;
				RenderCrossTabOnPage(pars, master, destination, rect2, out endCol2, out endRow2);
				num3++;
			}
			num2 = endRow2;
			num++;
		}
	}

	private void RenderCrossTabUnlimited(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, RectangleM rect)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		StiCross cross = master.CrossTabInfo.Cross;
		StiPage stiPage = GetPageForCrossTab(destination) ?? master.Report.RenderedPages.ToList().LastOrDefault();
		RenderCrossTabOnPage(pars, master, destination, rect, out var _, out var _);
		decimal num = cross.Widths.Sum() + (decimal)master.Left;
		RectangleM destinationRectangle = pars.DestinationRectangle;
		decimal width = ((RectangleM)(ref destinationRectangle)).Width;
		destinationRectangle = pars.DestinationRectangle;
		decimal num2 = num / (width + ((RectangleM)(ref destinationRectangle)).X);
		while (num2 > (decimal)stiPage.SegmentPerWidth)
		{
			stiPage.SegmentPerWidth++;
		}
		decimal num3 = cross.Heights.Sum() + (decimal)master.Top;
		destinationRectangle = pars.DestinationRectangle;
		decimal height = ((RectangleM)(ref destinationRectangle)).Height;
		destinationRectangle = pars.DestinationRectangle;
		decimal num4 = num3 / (height + ((RectangleM)(ref destinationRectangle)).Y);
		while (num4 > (decimal)stiPage.SegmentPerHeight)
		{
			stiPage.SegmentPerHeight++;
		}
	}

	private void RenderCrossTabOnPage(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, RectangleM rect, out int endCol, out int endRow)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		RenderCrossTabSegment(pars, master, destination, rect, out endCol, out endRow);
		MakeHorAlignment(master, destination, 0, (pars.DestinationContainer != null) ? pars.DestinationContainer.Width : master.Parent.Width, 1);
		FinalizeCross(destination);
	}

	private void RenderCrossTabSegment(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, RectangleM rect, out int endCol, out int endRow)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		StiCross cross = master.CrossTabInfo.Cross;
		RenderRowHeaders(pars, master, destination, ref rect);
		RenderColHeaders(pars, master, destination, ref rect);
		endRow = StiCrossTabHelper.GetEndRow(master, pars.StartRow, rect, pars.ForceNoBreak);
		endCol = StiCrossTabHelper.GetEndCol(master, pars.StartColumn, rect, pars.ForceNoBreak);
		endRow = StiCrossTabHelper.CheckMergedRowCells(master, pars.StartRow, endRow, pars.StartColumn, endCol);
		if (pars.StartRow < endRow)
		{
			StiCrossTabHelper.RenderCells(master, destination, pars.StartColumn, pars.StartRow, endCol, endRow, rect);
		}
		pars.RenderingIsFinished = endCol == cross.ColCount && endRow == cross.RowCount;
		pars.StartColumn = endCol;
		if (endCol == cross.ColCount)
		{
			pars.StartColumn = 0;
			pars.StartRow = endRow;
		}
	}

	private decimal RenderColHeaders(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, ref RectangleM rect, bool measure = false, bool skipStartRowCheck = false)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		if (pars.StartRow == 0 && !skipStartRowCheck)
		{
			return 0m;
		}
		if (!measure)
		{
			RectangleM rect2 = rect;
			((RectangleM)(ref rect2)).Height = 100000m;
			decimal num = RenderColHeaders(pars, master, destination, ref rect2, measure: true, skipStartRowCheck);
			if (num > ((RectangleM)(ref rect)).Height)
			{
				return 0m;
			}
		}
		StiCross cross = master.CrossTabInfo.Cross;
		int endColumn = GetEndColumn(pars, rect, cross, master);
		if (!measure)
		{
			RectangleM rect3 = rect;
			if (RenderColHeaders(pars, master, destination, ref rect3, measure: true) > ((RectangleM)(ref rect)).Height)
			{
				return 0m;
			}
		}
		decimal result = default(decimal);
		int num2 = 0;
		int num3 = ((StiCrossTabHelper.IsColFieldsEmpty(master) && master.SummaryDirection == StiSummaryDirection.LeftToRight && cross.SumFields.Count > 1) ? 1 : 0);
		if (cross.IsTopCrossTitleVisible)
		{
			if (cross.IsCrossTitlePrintOnAllPages)
			{
				if (!measure)
				{
					StiCrossTabHelper.RenderCells(master, destination, pars.StartColumn, 0, endColumn, 1 + num3, rect);
				}
				for (int i = 0; i < 1 + num3; i++)
				{
					((RectangleM)(ref rect)).Y = ((RectangleM)(ref rect)).Y + cross.Heights[i];
					((RectangleM)(ref rect)).Height = ((RectangleM)(ref rect)).Height - cross.Heights[i];
					result += cross.Heights[i];
				}
			}
			num2++;
		}
		if (StiCrossTabHelper.IsColFieldsEmpty(master))
		{
			return result;
		}
		int num4 = ((master.SummaryDirection == StiSummaryDirection.LeftToRight && cross.SumFields.Count > 1) ? 1 : 0);
		if (!measure && AllColFieldsPresentOnAllPages(cross))
		{
			StiCrossTabHelper.RenderCells(master, destination, pars.StartColumn, num2, endColumn, num2 + cross.ColFields.Count + num4, rect);
		}
		for (int j = 0; j < cross.ColFields.Count + num4; j++)
		{
			if ((j < cross.ColFields.Count && (cross.ColFields[j] as StiCrossHeader).PrintOnAllPages) || j >= cross.ColFields.Count)
			{
				if (!measure && !AllColFieldsPresentOnAllPages(cross))
				{
					StiCrossTabHelper.RenderCells(master, destination, pars.StartColumn, num2, endColumn, num2 + 1, rect);
				}
				decimal num5 = ((cross.Heights.Length > num2) ? cross.Heights[num2] : 0m);
				((RectangleM)(ref rect)).Y = ((RectangleM)(ref rect)).Y + num5;
				((RectangleM)(ref rect)).Height = ((RectangleM)(ref rect)).Height - num5;
				result += num5;
			}
			num2++;
		}
		return result;
	}

	private decimal RenderRowHeaders(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, ref RectangleM rect, bool measure = false)
	{
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		StiCross cross = master.CrossTabInfo.Cross;
		if (!master.RightToLeft && pars.StartColumn == 0)
		{
			return 0m;
		}
		if (master.RightToLeft && !measure)
		{
			int endCol = StiCrossTabHelper.GetEndCol(master, pars.StartColumn, rect);
			if (endCol == cross.Cells.Length)
			{
				if (master.RightToLeft && ((RectangleM)(ref rect)).Width != 100000m - pars.ShiftX - (decimal)destination.Left)
				{
					decimal num = default(decimal);
					for (int i = pars.StartColumn; i < endCol; i++)
					{
						num += cross.Widths[i];
					}
					((RectangleM)(ref rect)).X = ((RectangleM)(ref rect)).X + (((RectangleM)(ref rect)).Width - num);
				}
				return 0m;
			}
		}
		decimal num2 = default(decimal);
		if (!measure)
		{
			RectangleM rect2 = rect;
			((RectangleM)(ref rect2)).Width = 100000m;
			num2 = RenderRowHeaders(pars, master, destination, ref rect2, measure: true);
			if (num2 > ((RectangleM)(ref rect)).Width)
			{
				return 0m;
			}
			if (master.RightToLeft)
			{
				rect2 = rect;
				((RectangleM)(ref rect2)).Width = ((RectangleM)(ref rect2)).Width - num2;
				int endCol2 = StiCrossTabHelper.GetEndCol(master, pars.StartColumn, rect2);
				decimal num3 = default(decimal);
				for (int j = pars.StartColumn; j < endCol2; j++)
				{
					num3 += cross.Widths[j];
				}
				if (((RectangleM)(ref rect)).Width != 100000m)
				{
					((RectangleM)(ref rect)).X = ((RectangleM)(ref rect)).X + (((RectangleM)(ref rect)).Width - (num3 + num2));
				}
				((RectangleM)(ref rect)).Width = num3 + num2;
			}
		}
		decimal height = ((RectangleM)(ref rect)).Height;
		decimal y = ((RectangleM)(ref rect)).Y;
		decimal x = ((RectangleM)(ref rect)).X;
		RenderCorner(pars, master, destination, ref rect, cross, measure);
		if (!measure)
		{
			RectangleM rect3 = rect;
			if (RenderRowHeaders(pars, master, destination, ref rect3, measure: true) > ((RectangleM)(ref rect)).Width)
			{
				return 0m;
			}
		}
		decimal result = default(decimal);
		int endRow = GetEndRow(pars, rect, cross);
		int num4 = 0;
		if (cross.IsLeftCrossTitleVisible || (cross.IsTopCrossTitleVisible && cross.IsColsEmpty))
		{
			if (cross.IsCrossTitlePrintOnAllPages)
			{
				if (!measure)
				{
					StiCrossTabHelper.RenderCells(master, destination, 0, pars.StartRow, 1, endRow, rect);
				}
				((RectangleM)(ref rect)).X = ((RectangleM)(ref rect)).X + cross.Widths[0];
				((RectangleM)(ref rect)).Width = ((RectangleM)(ref rect)).Width - cross.Widths[0];
				result += cross.Widths[0];
			}
			num4++;
		}
		if (StiCrossTabHelper.IsRowFieldsEmpty(master))
		{
			return result;
		}
		int num5 = 1;
		if (master.RightToLeft)
		{
			num4 = cross.Cells.Length - 1 - num4;
			num5 = -1;
		}
		int num6 = num4;
		decimal num7 = default(decimal);
		foreach (StiCrossHeader rowField in cross.RowFields)
		{
			decimal num8 = ((cross.Widths.Length > num4) ? cross.Widths[num4] : 0m);
			if (rowField.PrintOnAllPages)
			{
				num7 += num8;
				result += num8;
			}
			if ((!rowField.PrintOnAllPages || rowField == cross.RowFields[cross.RowFields.Count - 1]) && num7 > 0m)
			{
				if (master.RightToLeft)
				{
					((RectangleM)(ref rect)).X = x + ((RectangleM)(ref rect)).Width - num7;
				}
				if (!measure)
				{
					StiCrossTabHelper.RenderCells(master, destination, num6, pars.StartRow, num4 + 1, endRow, rect);
				}
				if (!master.RightToLeft)
				{
					((RectangleM)(ref rect)).X = ((RectangleM)(ref rect)).X + num7;
				}
				((RectangleM)(ref rect)).Width = ((RectangleM)(ref rect)).Width - num7;
				num7 = default(decimal);
				num6 = num4 + num5;
			}
			if (!master.RightToLeft)
			{
				num4++;
			}
			else
			{
				num6--;
			}
		}
		if (master.RightToLeft)
		{
			((RectangleM)(ref rect)).X = x;
		}
		((RectangleM)(ref rect)).Y = y;
		((RectangleM)(ref rect)).Height = height;
		return result;
	}

	private decimal RenderCorner(StiCrossTabParams pars, StiCrossTab master, StiContainer destination, ref RectangleM rect, StiCross cross, bool measure = false)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		if (pars.StartRow == 0 || StiCrossTabHelper.IsColFieldsEmpty(master))
		{
			return 0m;
		}
		if (!measure)
		{
			RectangleM rect2 = rect;
			if (RenderCorner(pars, master, destination, ref rect2, cross, measure: true) > ((RectangleM)(ref rect)).Height)
			{
				return 0m;
			}
		}
		decimal result = default(decimal);
		int num = 0;
		int endCol = cross.RowFields.Cast<StiCrossHeader>().Count((StiCrossHeader r) => r.PrintOnAllPages);
		if (cross.IsTopCrossTitleVisible)
		{
			if (cross.IsCrossTitlePrintOnAllPages)
			{
				if (!measure)
				{
					StiCrossTabHelper.RenderCells(master, destination, 0, 0, endCol, 1, rect);
				}
				((RectangleM)(ref rect)).Y = ((RectangleM)(ref rect)).Y + cross.Heights[0];
				((RectangleM)(ref rect)).Height = ((RectangleM)(ref rect)).Height - cross.Heights[0];
				result += cross.Heights[0];
			}
			num++;
		}
		if (!measure && AllColFieldsPresentOnAllPages(cross))
		{
			StiCrossTabHelper.RenderCells(master, destination, 0, num, endCol, num + cross.ColFields.Count, rect);
		}
		foreach (StiCrossHeader colField in cross.ColFields)
		{
			if (colField.PrintOnAllPages)
			{
				if (!measure && !AllColFieldsPresentOnAllPages(cross))
				{
					StiCrossTabHelper.RenderCells(master, destination, 0, num, endCol, num + 1, rect);
				}
				decimal num2 = ((cross.Heights.Length > num) ? cross.Heights[num] : 0m);
				((RectangleM)(ref rect)).Y = ((RectangleM)(ref rect)).Y + num2;
				((RectangleM)(ref rect)).Height = ((RectangleM)(ref rect)).Height - num2;
				result += num2;
			}
			num++;
		}
		return result;
	}

	private int GetEndColumn(StiCrossTabParams pars, RectangleM rect, StiCross cross, StiCrossTab master)
	{
		decimal width = ((RectangleM)(ref rect)).Width;
		int i;
		for (i = pars.StartColumn; i < cross.ColCount; i++)
		{
			width -= cross.Widths[i];
			if (width < 0m && master.HorAlignment != StiCrossHorAlignment.Width)
			{
				break;
			}
		}
		if (i == pars.StartColumn && i + 1 <= cross.ColCount)
		{
			i++;
		}
		return i;
	}

	private int GetEndRow(StiCrossTabParams pars, RectangleM rect, StiCross cross)
	{
		decimal height = ((RectangleM)(ref rect)).Height;
		int i;
		for (i = pars.StartRow; i < cross.RowCount; i++)
		{
			height -= cross.Heights[i];
			if (height < 0m)
			{
				break;
			}
		}
		if (i == pars.StartRow && i + 1 <= cross.RowCount)
		{
			i++;
		}
		return i;
	}

	private static StiPage GetPageForCrossTab(StiContainer destination)
	{
		StiPage stiPage = null;
		StiContainer stiContainer = destination;
		while (stiPage == null && stiContainer != null)
		{
			stiPage = stiContainer.Page;
			stiContainer = stiContainer.Parent;
		}
		return stiPage;
	}

	private bool AllColFieldsPresentOnAllPages(StiCross cross)
	{
		return cross.ColFields.Cast<StiCrossHeader>().All((StiCrossHeader c) => c.PrintOnAllPages);
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiFilterHelper.SetFilter(masterComp);
	}

	public override void UnPrepare(StiComponent masterComp)
	{
		base.UnPrepare(masterComp);
		StiCrossTabHelper.ClearCross(masterComp as StiCrossTab);
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		StiCrossTab stiCrossTab = masterComp as StiCrossTab;
		StiCrossTabParams stiCrossTabParams = new StiCrossTabParams();
		bool flag2 = (stiCrossTabParams.AllowRendering = StiSubReportsHelper.GetParentBand(stiCrossTab) != null);
		RectangleD clientRectangle;
		if (masterComp.Parent is StiDataBand stiDataBand)
		{
			clientRectangle = stiDataBand.Parent.ClientRectangle;
			stiCrossTabParams.DestinationRectangle = ((RectangleD)(ref clientRectangle)).ToRectangleM();
		}
		else
		{
			clientRectangle = stiCrossTab.ClientRectangle;
			stiCrossTabParams.DestinationRectangle = ((RectangleD)(ref clientRectangle)).ToRectangleM();
		}
		if (flag2)
		{
			StiCrossTabHelper.CreateCross(stiCrossTab);
			return RenderCrossTab(stiCrossTabParams, stiCrossTab);
		}
		StiContainer stiContainer = base.InternalRender(masterComp) as StiContainer;
		if (stiContainer != null)
		{
			stiContainer.Width = masterComp.Width;
			stiContainer.Height = masterComp.Height;
			stiContainer.Components.Clear();
		}
		return stiContainer;
	}
}
