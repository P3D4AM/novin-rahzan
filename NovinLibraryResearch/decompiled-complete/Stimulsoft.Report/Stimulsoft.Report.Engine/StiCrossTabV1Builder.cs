using System;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;

namespace Stimulsoft.Report.Engine;

public class StiCrossTabV1Builder : StiContainerV1Builder
{
	private void MakeHorAlignment(StiCrossTab masterCrossTab, StiContainer outContainer, int startIndex)
	{
		if (masterCrossTab.HorAlignment == StiCrossHorAlignment.Width)
		{
			StiCrossTabHelper.MakeHorAlignmentByWidth(outContainer, startIndex);
		}
		else
		{
			if (masterCrossTab.HorAlignment == StiCrossHorAlignment.None)
			{
				return;
			}
			double maxLeft = 0.0;
			double maxRight = 0.0;
			StiCrossTabHelper.CalculateMaxAndMin(outContainer, ref maxLeft, ref maxRight, startIndex);
			for (int i = startIndex; i < outContainer.Components.Count; i++)
			{
				StiComponent stiComponent = outContainer.Components[i];
				if (masterCrossTab.HorAlignment == StiCrossHorAlignment.Left)
				{
					stiComponent.Left -= maxLeft;
				}
				else if (masterCrossTab.HorAlignment == StiCrossHorAlignment.Right)
				{
					stiComponent.Left += outContainer.Width - maxRight;
				}
				else if (masterCrossTab.HorAlignment == StiCrossHorAlignment.Center)
				{
					double num = (outContainer.Width - (maxRight - maxLeft)) / 2.0;
					stiComponent.Left -= maxLeft - num;
				}
			}
		}
	}

	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		StiCrossTab stiCrossTab = masterComp as StiCrossTab;
		stiCrossTab.CrossTabInfoV1.FinishRender = false;
		StiFilterHelper.SetFilter(stiCrossTab);
		if (!(stiCrossTab.Parent is StiBand))
		{
			StiCrossTabHelper.CreateCross(stiCrossTab);
		}
	}

	public override bool Render(StiComponent masterComp, ref StiComponent renderedComponent, StiContainer outContainer)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		StiCrossTab stiCrossTab = masterComp as StiCrossTab;
		if ((stiCrossTab.DataSource == null || stiCrossTab.DataSource.IsEmpty) && !stiCrossTab.PrintIfEmpty)
		{
			return true;
		}
		if (stiCrossTab.CrossTabInfoV1.FinishRender)
		{
			return true;
		}
		bool flag = stiCrossTab.Parent is StiBand;
		if (flag)
		{
			StiCrossTabHelper.CreateCross(stiCrossTab);
		}
		StiCross cross = stiCrossTab.CrossTabInfo.Cross;
		RectangleD val = stiCrossTab.GetDockRegion(outContainer);
		RectangleM val2 = ((RectangleD)(ref val)).ToRectangleM();
		renderedComponent = StiContainerV1Builder.GetRenderContainer(outContainer, stiCrossTab);
		if (!flag)
		{
			outContainer.Components.Add(renderedComponent);
		}
		if (stiCrossTab.CanGrow)
		{
			renderedComponent.Width = (double)((RectangleM)(ref val2)).Right - renderedComponent.Left;
			renderedComponent.Height = (double)((RectangleM)(ref val2)).Bottom - renderedComponent.Top;
			if (flag)
			{
				renderedComponent.Height = 100000000.0;
			}
		}
		val = stiCrossTab.CrossTabInfoV1.RenderRect;
		if (((RectangleD)(ref val)).IsEmpty)
		{
			stiCrossTab.CrossTabInfoV1.RenderRect = renderedComponent.ClientRectangle;
		}
		else
		{
			renderedComponent.ClientRectangle = stiCrossTab.CrossTabInfoV1.RenderRect;
		}
		val = renderedComponent.ClientRectangle;
		val2 = ((RectangleD)(ref val)).ToRectangleM();
		int startRow = stiCrossTab.CrossTabInfoV1.StartRow;
		int startCol = stiCrossTab.CrossTabInfoV1.StartCol;
		if (stiCrossTab.Wrap)
		{
			stiCrossTab.CrossTabInfoV1.StartCol = 0;
			startRow = StiCrossTabHelper.GetEndRow(stiCrossTab, 0, val2);
			while (stiCrossTab.CrossTabInfoV1.StartCol < cross.ColCount)
			{
				startCol = StiCrossTabHelper.GetEndCol(stiCrossTab, stiCrossTab.CrossTabInfoV1.StartCol, val2);
				int count = outContainer.Components.Count;
				StiCrossTabHelper.RenderCells(stiCrossTab, outContainer, stiCrossTab.CrossTabInfoV1.StartCol, 0, startCol, startRow, val2);
				decimal num = default(decimal);
				for (int i = count; i < outContainer.Components.Count; i++)
				{
					StiComponent stiComponent = outContainer.Components[i];
					num = Math.Max((decimal)stiComponent.Bottom, num);
				}
				((RectangleM)(ref val2)).Y = num + (decimal)stiCrossTab.WrapGap;
				stiCrossTab.CrossTabInfoV1.StartCol = startCol;
			}
			return true;
		}
		if (flag)
		{
			stiCrossTab.CrossTabInfoV1.StartCol = 0;
			StiPage page = outContainer.Page;
			decimal num2 = (decimal)(page.PageWidth - page.Margins.Left - page.Margins.Right);
			decimal num3 = num2;
			startRow = StiCrossTabHelper.GetEndRow(stiCrossTab, 0, val2);
			int num4 = 1;
			if (!page.UnlimitedBreakable)
			{
				((RectangleM)(ref val2)).Width = 1000000000m;
			}
			int count2 = outContainer.Components.Count;
			while (stiCrossTab.CrossTabInfoV1.StartCol < cross.ColCount)
			{
				startCol = StiCrossTabHelper.GetEndCol(stiCrossTab, stiCrossTab.CrossTabInfoV1.StartCol, val2);
				StiCrossTabHelper.RenderCells(stiCrossTab, outContainer, stiCrossTab.CrossTabInfoV1.StartCol, 0, startCol, startRow, val2);
				if (((RectangleM)(ref val2)).X < num3)
				{
					((RectangleM)(ref val2)).X = num3;
				}
				else
				{
					((RectangleM)(ref val2)).X = ((RectangleM)(ref val2)).X + num3;
				}
				((RectangleM)(ref val2)).Width = num2;
				stiCrossTab.CrossTabInfoV1.StartCol = startCol;
				num4++;
				if (stiCrossTab.CrossTabInfoV1.StartCol < cross.ColCount)
				{
					page.SegmentPerWidth = num4;
				}
			}
			MakeHorAlignment(stiCrossTab, outContainer, count2);
			if (!page.UnlimitedBreakable)
			{
				double num5 = 0.0;
				for (int j = count2; j < outContainer.Components.Count; j++)
				{
					StiComponent stiComponent2 = outContainer.Components[j];
					num5 = Math.Max(stiComponent2.Right, num5);
				}
				while (page.Width < num5)
				{
					page.SegmentPerWidth++;
				}
			}
			return true;
		}
		((RectangleM)(ref val2)).X = 0m;
		((RectangleM)(ref val2)).Y = 0m;
		startCol = StiCrossTabHelper.GetEndCol(stiCrossTab, stiCrossTab.CrossTabInfoV1.StartCol, val2);
		startRow = StiCrossTabHelper.GetEndRow(stiCrossTab, stiCrossTab.CrossTabInfoV1.StartRow, val2);
		bool flag2 = cross.LeftCrossTitle != null && cross.LeftCrossTitle.Enabled && cross.RightCrossTitle != null && cross.RightCrossTitle.Enabled;
		bool flag3 = cross.LeftCrossTitle != null && cross.LeftCrossTitle.PrintOnAllPages && cross.RightCrossTitle != null && cross.RightCrossTitle.PrintOnAllPages && !stiCrossTab.RightToLeft;
		decimal x = ((RectangleM)(ref val2)).X;
		decimal y = ((RectangleM)(ref val2)).Y;
		decimal x2 = ((RectangleM)(ref val2)).X;
		decimal y2 = ((RectangleM)(ref val2)).Y;
		if (stiCrossTab.CrossTabInfoV1.StartCol != 0)
		{
			for (int k = 0; k < cross.RowFields.Count; k++)
			{
				if (((StiCrossHeader)cross.RowFields[k]).PrintOnAllPages && !stiCrossTab.RightToLeft)
				{
					decimal num6 = cross.Widths[k];
					x += num6;
					((RectangleM)(ref val2)).Width = ((RectangleM)(ref val2)).Width - num6;
				}
			}
		}
		if (stiCrossTab.CrossTabInfoV1.StartRow != 0)
		{
			for (int l = 0; l < cross.ColFields.Count; l++)
			{
				if (((StiCrossHeader)cross.ColFields[l]).PrintOnAllPages && !stiCrossTab.RightToLeft)
				{
					y += cross.Heights[l];
					((RectangleM)(ref val2)).Height = ((RectangleM)(ref val2)).Height - cross.Heights[l];
				}
			}
		}
		if (flag2 && flag3 && stiCrossTab.CrossTabInfoV1.StartRow != 0 && !stiCrossTab.RightToLeft)
		{
			y += cross.Heights[1];
			((RectangleM)(ref val2)).Height = ((RectangleM)(ref val2)).Height - cross.Heights[1];
		}
		if (stiCrossTab.CrossTabInfoV1.StartCol != 0 && !StiCrossTabHelper.IsRowFieldsEmpty(stiCrossTab))
		{
			((RectangleM)(ref val2)).X = x2;
			((RectangleM)(ref val2)).Y = y;
			decimal height = ((RectangleM)(ref val2)).Height;
			int num7 = stiCrossTab.CrossTabInfoV1.StartRow;
			for (int m = stiCrossTab.CrossTabInfoV1.StartRow; m < startRow; m++)
			{
				height -= cross.Heights[m];
				if (height < 0m)
				{
					break;
				}
				num7++;
			}
			startRow = num7;
			int num8 = 0;
			while (num8 < cross.RowFields.Count)
			{
				int n;
				for (n = num8; n < cross.RowFields.Count && ((StiCrossHeader)cross.RowFields[n]).PrintOnAllPages; n++)
				{
					if (stiCrossTab.RightToLeft)
					{
						break;
					}
				}
				if (num8 != n)
				{
					if (stiCrossTab.CrossTabInfoV1.StartRow > 0)
					{
						RectangleM rect = val2;
						((RectangleM)(ref rect)).Y = 0m;
						StiCrossTabHelper.RenderCells(stiCrossTab, renderedComponent as StiContainer, 0, 0, cross.RowFields.Count, 2, rect);
					}
					StiCrossTabHelper.RenderCells(stiCrossTab, renderedComponent as StiContainer, num8, stiCrossTab.CrossTabInfoV1.StartRow, n, startRow, val2);
					((RectangleM)(ref val2)).X = ((RectangleM)(ref val2)).X + cross.Widths[num8];
				}
				num8 = n + 1;
			}
		}
		if (stiCrossTab.CrossTabInfoV1.StartRow != 0)
		{
			((RectangleM)(ref val2)).X = x;
			((RectangleM)(ref val2)).Y = y2;
			decimal width = ((RectangleM)(ref val2)).Width;
			int num9 = stiCrossTab.CrossTabInfoV1.StartCol;
			for (int num10 = stiCrossTab.CrossTabInfoV1.StartCol; num10 < startCol; num10++)
			{
				width -= cross.Widths[num10];
				if (width < 0m)
				{
					break;
				}
				num9++;
			}
			startCol = num9;
			if (flag2 && flag3 && !stiCrossTab.RightToLeft)
			{
				StiCrossTabHelper.RenderCells(stiCrossTab, renderedComponent as StiContainer, stiCrossTab.CrossTabInfoV1.StartCol, 0, startCol, 1, val2);
				((RectangleM)(ref val2)).Y = ((RectangleM)(ref val2)).Y + cross.Heights[0];
			}
			int num11 = 0;
			while (num11 < cross.ColFields.Count)
			{
				int num12;
				for (num12 = num11; num12 < cross.ColFields.Count && ((StiCrossHeader)cross.ColFields[num12]).PrintOnAllPages; num12++)
				{
					if (stiCrossTab.RightToLeft)
					{
						break;
					}
				}
				if (num11 != num12)
				{
					if (flag2)
					{
						StiCrossTabHelper.RenderCells(stiCrossTab, renderedComponent as StiContainer, stiCrossTab.CrossTabInfoV1.StartCol, num11 + 1, startCol, num12 + 1, val2);
						((RectangleM)(ref val2)).Y = ((RectangleM)(ref val2)).Y + cross.Heights[num11 + 1];
					}
					else
					{
						StiCrossTabHelper.RenderCells(stiCrossTab, renderedComponent as StiContainer, stiCrossTab.CrossTabInfoV1.StartCol, num11, startCol, num12, val2);
						((RectangleM)(ref val2)).Y = ((RectangleM)(ref val2)).Y + cross.Heights[num11];
					}
				}
				num11 = num12 + 1;
			}
		}
		((RectangleM)(ref val2)).X = x;
		((RectangleM)(ref val2)).Y = y;
		startCol = StiCrossTabHelper.GetEndCol(stiCrossTab, stiCrossTab.CrossTabInfoV1.StartCol, val2);
		startRow = StiCrossTabHelper.GetEndRow(stiCrossTab, stiCrossTab.CrossTabInfoV1.StartRow, val2);
		if (stiCrossTab.CrossTabInfoV1.StartRow < startRow)
		{
			StiCrossTabHelper.RenderCells(stiCrossTab, renderedComponent as StiContainer, stiCrossTab.CrossTabInfoV1.StartCol, stiCrossTab.CrossTabInfoV1.StartRow, startCol, startRow, val2);
		}
		bool flag4 = startCol == cross.ColCount && startRow == cross.RowCount;
		stiCrossTab.CrossTabInfoV1.StartCol = startCol;
		if (startCol == cross.ColCount)
		{
			stiCrossTab.CrossTabInfoV1.StartCol = 0;
			stiCrossTab.CrossTabInfoV1.StartRow = startRow;
		}
		if (stiCrossTab.CanGrow)
		{
			renderedComponent.CanGrow = false;
			renderedComponent.CanShrink = true;
			SizeD actualSize = renderedComponent.GetActualSize();
			renderedComponent.Width = ((SizeD)(ref actualSize)).Width;
			renderedComponent.Height = ((SizeD)(ref actualSize)).Height;
			renderedComponent.CanGrow = true;
		}
		if (!flag)
		{
			if (stiCrossTab.HorAlignment == StiCrossHorAlignment.Left)
			{
				renderedComponent.Left = 0.0;
			}
			else if (stiCrossTab.HorAlignment == StiCrossHorAlignment.Right)
			{
				renderedComponent.Left = renderedComponent.Parent.Width - renderedComponent.Width;
			}
			else if (stiCrossTab.HorAlignment == StiCrossHorAlignment.Center)
			{
				renderedComponent.Left = (renderedComponent.Parent.Width - renderedComponent.Width) / 2.0;
			}
			else if (stiCrossTab.HorAlignment == StiCrossHorAlignment.Width)
			{
				renderedComponent.Left = 0.0;
				renderedComponent.Width = renderedComponent.Parent.Width;
				StiCrossTabHelper.MakeHorAlignmentByWidth(renderedComponent as StiContainer, 0);
			}
		}
		stiCrossTab.CrossTabInfoV1.FinishRender = flag4;
		return flag4;
	}
}
