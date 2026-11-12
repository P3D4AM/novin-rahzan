using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public abstract class ButtonItemPainter
{
	public abstract void PaintButton(ButtonItem button, ItemPaintArgs pa);

	public abstract void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image);

	public abstract void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r);

	public abstract void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle imagebounds);

	public abstract void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r);

	public abstract void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image);

	public abstract void PaintCustomizeCheck(ButtonItem button, ItemPaintArgs pa, Rectangle r);

	public abstract void PaintExpandButton(ButtonItem button, ItemPaintArgs pa);

	public abstract Rectangle GetCheckRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image);

	public abstract Rectangle GetCustomizeMenuCheckRectangle(ButtonItem button, ItemPaintArgs pa);

	public abstract Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image);

	public abstract Rectangle GetMouseOverRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image);

	public abstract eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image);

	public static string GetDrawText(string text)
	{
		return text;
	}

	public static bool IsItemEnabled(BaseItem item, ItemPaintArgs pa)
	{
		return item.GetEnabled(pa.ContainerControl);
	}

	public static Point[] GetExpandPolygon(Rectangle r, ePopupSide direction)
	{
		Point[] array = new Point[3];
		Size size = new Size(StiScale2.I4, StiScale2.I3);
		switch (direction)
		{
		case ePopupSide.Right:
			array[0].X = r.Left + StiScale2.I1;
			array[0].Y = r.Top + (r.Height - size.Height) / 2 - StiScale2.I1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + StiScale2.I6;
			array[2].X = array[0].X + StiScale2.I3;
			array[2].Y = array[0].Y + StiScale2.I3;
			break;
		case ePopupSide.Left:
			array[0].X = r.Left + StiScale2.I3;
			array[0].Y = r.Top + (r.Height - size.Height) / 2 - StiScale2.I1;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + StiScale2.I6;
			array[2].X = array[0].X - StiScale2.I3;
			array[2].Y = array[0].Y + StiScale2.I3;
			break;
		case ePopupSide.Top:
			array[0].X = r.Left + (r.Width - size.Width) / 2 - StiScale2.I1;
			array[0].Y = r.Top + (r.Height - size.Height) / 2 + size.Height;
			array[1].X = array[0].X + StiScale2.I6;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + StiScale2.I3;
			array[2].Y = array[0].Y - StiScale2.I4;
			break;
		case ePopupSide.Default:
		case ePopupSide.Bottom:
			array[0].X = r.Left + (r.Width - size.Width) / 2;
			array[0].Y = r.Top + (r.Height - size.Height) / 2 + StiScale2.I1;
			array[1].X = array[0].X + StiScale2.I5;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + StiScale2.I2;
			array[2].Y = array[0].Y + StiScale2.I3;
			break;
		}
		return array;
	}

	public static void PaintButtonExpandIndicator(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle = button.DisplayRectangle;
		Point[] array = new Point[3];
		Rectangle subItemsRect = button.SubItemsRect;
		if (button.PopupSide == ePopupSide.Default)
		{
			if (button.Orientation == eOrientation.Horizontal)
			{
				int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
				Rectangle rect = new Rectangle(displayRectangle.Right - verticalScrollBarWidth, displayRectangle.Top, verticalScrollBarWidth, displayRectangle.Height);
				StiButtonHelper.DrawDownArrow(graphics, rect, button.Enabled, pa.Colors.ItemText, pa.Colors.ItemDisabledText);
			}
			else
			{
				Rectangle rect2 = new Rectangle(displayRectangle.Left + StiScale2.I1, displayRectangle.Bottom - StiScale2.XXI(9.0), displayRectangle.Width, StiScale2.YYI(9.0));
				StiButtonHelper.DrawRightArrow(graphics, rect2, button.Enabled, pa.Colors.ItemText, pa.Colors.ItemDisabledText);
			}
			return;
		}
		switch (button.PopupSide)
		{
		case ePopupSide.Right:
			array[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2;
			array[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - StiScale2.I3;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + StiScale2.I6;
			array[2].X = array[0].X + StiScale2.I3;
			array[2].Y = array[0].Y + StiScale2.I3;
			array[0].X += StiScale2.XXI(button.DropArrowShift);
			array[1].X += StiScale2.XXI(button.DropArrowShift);
			array[2].X += StiScale2.XXI(button.DropArrowShift);
			break;
		case ePopupSide.Left:
			array[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2 + StiScale2.I3;
			array[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - StiScale2.I3;
			array[1].X = array[0].X;
			array[1].Y = array[0].Y + StiScale2.I6;
			array[2].X = array[0].X - StiScale2.I3;
			array[2].Y = array[0].Y + StiScale2.I3;
			break;
		case ePopupSide.Top:
			array[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - StiScale2.I5) / 2;
			array[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - StiScale2.I3) / 2 + StiScale2.I4;
			array[1].X = array[0].X + StiScale2.I6;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + StiScale2.I3;
			array[2].Y = array[0].Y - StiScale2.I4;
			break;
		case ePopupSide.Bottom:
			array[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - StiScale2.I5) / 2 + StiScale2.I1;
			array[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - StiScale2.I3) / 2 + StiScale2.I1;
			array[1].X = array[0].X + StiScale2.I5;
			array[1].Y = array[0].Y;
			array[2].X = array[0].X + StiScale2.I2;
			array[2].Y = array[0].Y + StiScale2.I3;
			break;
		}
		if (IsItemEnabled(button, pa))
		{
			SolidBrush val = new SolidBrush(pa.Colors.ItemText);
			try
			{
				graphics.FillPolygon((Brush)(object)val, array);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		SolidBrush val2 = new SolidBrush(pa.Colors.ItemDisabledText);
		try
		{
			graphics.FillPolygon((Brush)(object)val2, array);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
