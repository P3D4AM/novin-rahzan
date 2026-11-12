using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007ItemContainerPainter : ItemContainerPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void PaintBackground(ItemContainerRendererEventArgs e)
	{
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		if (!BarFunctions.IsOffice2007Style(e.ItemContainer.EffectiveStyle))
		{
			return;
		}
		ItemContainer itemContainer = e.ItemContainer;
		Office2007ItemGroupColorTable itemGroup = m_ColorTable.ItemGroup;
		if ((itemContainer.SubItems.Count == 0 && (itemContainer.WidthInternal <= 0 || itemContainer.HeightInternal <= 0)) || itemGroup == null)
		{
			return;
		}
		Rectangle displayRectangle = itemContainer.DisplayRectangle;
		if (!itemContainer.BeginGroup || (itemContainer.IsOnMenu && !(itemContainer.Parent is ItemContainer)))
		{
			if (itemContainer.BackgroundStyle.Class == ElementStyleClassKeys.Office2007StatusBarBackground2Key && e.ItemPaintArgs != null && e.ItemPaintArgs.ContainerControl is Bar && ((Bar)(object)e.ItemPaintArgs.ContainerControl).BarType == eBarType.StatusBar)
			{
				displayRectangle.Y = 1;
				if (!m_ColorTable.Bar.StatusBarTopBorderLight.IsEmpty)
				{
					displayRectangle.Y++;
				}
				displayRectangle.Height = e.ItemPaintArgs.ContainerControl.Height - 1;
				if (itemContainer.Parent.SubItems.IndexOf(itemContainer) == itemContainer.Parent.SubItems.Count - 1)
				{
					if (e.ItemPaintArgs.RightToLeft)
					{
						displayRectangle.Width += displayRectangle.X;
						displayRectangle.X = 0;
					}
					else
					{
						displayRectangle.Width += e.ItemPaintArgs.ContainerControl.Width - displayRectangle.Right;
					}
				}
			}
			Region clip = e.Graphics.Clip;
			e.Graphics.SetClip(displayRectangle);
			ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(itemContainer.BackgroundStyle, e.Graphics, displayRectangle));
			e.Graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
			return;
		}
		displayRectangle.Width -= itemContainer.BackgroundStyle.MarginHorizontal;
		displayRectangle.Height -= itemContainer.BackgroundStyle.MarginVertical;
		displayRectangle.X += itemContainer.BackgroundStyle.MarginLeft;
		displayRectangle.Y += itemContainer.BackgroundStyle.MarginTop;
		Graphics graphics = e.Graphics;
		int roundCornerSize = 2;
		if (itemGroup.OuterBorder != null && !itemGroup.OuterBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundGradientRectangle(graphics, displayRectangle, itemGroup.OuterBorder, 1, roundCornerSize);
		}
		if (itemGroup.InnerBorder != null && !itemGroup.InnerBorder.IsEmpty)
		{
			displayRectangle.Inflate(-1, -1);
			DisplayHelp.DrawRoundGradientRectangle(graphics, displayRectangle, itemGroup.InnerBorder, 1, roundCornerSize);
		}
		float num = 0.4f;
		Rectangle r = displayRectangle;
		r.Inflate(-1, -1);
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		if (itemGroup.TopBackground != null && !itemGroup.TopBackground.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, r, itemGroup.TopBackground);
		}
		int num2 = (int)((float)r.Height * num);
		r.Height -= num2;
		r.Y += num2;
		if (itemGroup.BottomBackground != null && !itemGroup.BottomBackground.IsEmpty)
		{
			DisplayHelp.FillRectangle(graphics, r, itemGroup.BottomBackground);
		}
		graphics.SmoothingMode = smoothingMode;
		bool flag = itemContainer.LayoutOrientation == eOrientation.Vertical;
		Pen val = new Pen(itemGroup.ItemGroupDividerLight, 1f);
		try
		{
			Pen val2 = new Pen(itemGroup.ItemGroupDividerDark);
			try
			{
				int num3 = itemContainer.SubItems.Count - 1;
				for (int i = 0; i <= num3; i++)
				{
					BaseItem baseItem = itemContainer.SubItems[i];
					if (baseItem.Visible && baseItem.DisplayRectangle.Right != itemContainer.DisplayRectangle.Right && i != num3)
					{
						if (flag)
						{
							graphics.DrawLine(val2, itemContainer.DisplayRectangle.X + 1, baseItem.DisplayRectangle.Bottom - 1, itemContainer.DisplayRectangle.Right - 1, baseItem.DisplayRectangle.Bottom - 1);
							graphics.DrawLine(val, itemContainer.DisplayRectangle.X + 1, baseItem.DisplayRectangle.Bottom, itemContainer.DisplayRectangle.Right, baseItem.DisplayRectangle.Bottom);
						}
						else
						{
							graphics.DrawLine(val2, baseItem.DisplayRectangle.Right - 1, baseItem.DisplayRectangle.Y + 1, baseItem.DisplayRectangle.Right - 1, baseItem.DisplayRectangle.Bottom - 2);
							graphics.DrawLine(val, baseItem.DisplayRectangle.Right, baseItem.DisplayRectangle.Y + 1, baseItem.DisplayRectangle.Right, baseItem.DisplayRectangle.Bottom - 2);
						}
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override void PaintItemSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		if (!(e.Item is ItemContainer))
		{
			Graphics graphics = e.Graphics;
			BaseItem item = e.Item;
			Size size = Size.Empty;
			if (item is ImageItem)
			{
				size = ((ImageItem)item).ImageSize;
			}
			ItemContainer itemContainer = e.ItemContainer;
			Color itemSeparator = m_ColorTable.LegacyColors.ItemSeparator;
			Color itemSeparatorShade = m_ColorTable.LegacyColors.ItemSeparatorShade;
			Point start = Point.Empty;
			Point end = Point.Empty;
			Point empty = Point.Empty;
			Point empty2 = Point.Empty;
			if (itemContainer.LayoutOrientation == eOrientation.Horizontal)
			{
				start = new Point(item.DisplayRectangle.X - 2, item.DisplayRectangle.Y + 3);
				end = new Point(start.X, item.DisplayRectangle.Bottom - 4);
				empty = new Point(start.X + 1, start.Y);
				empty2 = new Point(end.X + 1, end.Y);
			}
			else if (item.IsOnMenu)
			{
				start = new Point(item.DisplayRectangle.X + size.Width, item.DisplayRectangle.Y - 2);
				end = new Point(item.DisplayRectangle.Right - 1, start.Y);
				empty = new Point(start.X, start.Y + 1);
				empty2 = new Point(end.X, end.Y + 1);
			}
			else
			{
				start = new Point(item.DisplayRectangle.X + 3, item.DisplayRectangle.Y - 2);
				end = new Point(item.DisplayRectangle.Right - 4, start.Y);
				empty = new Point(start.X, start.Y + 1);
				empty2 = new Point(end.X, end.Y + 1);
			}
			DisplayHelp.DrawLine(graphics, start, end, itemSeparator, 1);
			DisplayHelp.DrawLine(graphics, empty, empty2, itemSeparatorShade, 1);
		}
	}
}
