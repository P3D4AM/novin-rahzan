using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ThemedButtonItemPainter
{
	public static void PaintButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		ThemeToolbar themeToolbar = pa.ThemeToolbar;
		ThemeToolbarParts part = ThemeToolbarParts.Button;
		ThemeToolbarStates themeToolbarStates = ThemeToolbarStates.Normal;
		Color textColor = ButtonItemPainterHelper.GetTextColor(button, pa);
		Rectangle rectangle = Rectangle.Empty;
		Rectangle displayRectangle = button.DisplayRectangle;
		Font val = null;
		CompositeImage image = button.GetImage();
		val = button.GetFont(pa, isLayout: false);
		eTextFormat eTextFormat2 = GetStringFormat(button, pa, image);
		bool flag = (button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems && !button.SubItemsRect.IsEmpty;
		if (flag)
		{
			part = ThemeToolbarParts.SplitButton;
		}
		if (image != null)
		{
			rectangle = ((button.ImagePosition != eImagePosition.Top && button.ImagePosition != eImagePosition.Bottom) ? new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, button.ImageDrawRect.Width, button.ImageDrawRect.Height) : new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, displayRectangle.Width, button.ImageDrawRect.Height));
			rectangle.Offset(displayRectangle.Left, displayRectangle.Top);
			rectangle.Offset((rectangle.Width - button.ImageSize.Width) / 2, (rectangle.Height - button.ImageSize.Height) / 2);
			rectangle.Width = button.ImageSize.Width;
			rectangle.Height = button.ImageSize.Height;
		}
		if (!ButtonItemPainter.IsItemEnabled(button, pa))
		{
			themeToolbarStates = ThemeToolbarStates.Disabled;
		}
		else if (button.IsMouseDown)
		{
			themeToolbarStates = ThemeToolbarStates.Pressed;
		}
		else if (button.IsMouseOver && button.Checked)
		{
			themeToolbarStates = ThemeToolbarStates.HotChecked;
		}
		else if (button.IsMouseOver || button.Expanded)
		{
			themeToolbarStates = ThemeToolbarStates.Hot;
		}
		else if (button.Checked)
		{
			themeToolbarStates = ThemeToolbarStates.Checked;
		}
		Rectangle r = button.DisplayRectangle;
		if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			r = rectangle;
			r.Inflate(3, 3);
		}
		else if (flag)
		{
			r.Width -= button.SubItemsRect.Width;
		}
		if (button.HotTrackingStyle != eHotTrackingStyle.None)
		{
			themeToolbar.DrawBackground(graphics, part, themeToolbarStates, r);
		}
		if (image != null && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
		{
			if (themeToolbarStates == ThemeToolbarStates.Normal && button.HotTrackingStyle == eHotTrackingStyle.Color)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
					new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
					new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
				});
				ImageAttributes val2 = new ImageAttributes();
				val2.SetColorMatrix(colorMatrix);
				image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val2);
			}
			else if (themeToolbarStates == ThemeToolbarStates.Normal && !image.IsIcon)
			{
				ImageAttributes val3 = new ImageAttributes();
				val3.SetGamma(0.7f, (ColorAdjustType)1);
				image.DrawImage(graphics, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val3);
			}
			else
			{
				image.DrawImage(graphics, rectangle);
			}
		}
		if (button.ButtonStyle == eButtonStyle.ImageAndText || button.ButtonStyle == eButtonStyle.TextOnlyAlways || image == null)
		{
			Rectangle rectangle2 = button.TextDrawRect;
			if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				if (button.Orientation == eOrientation.Vertical)
				{
					rectangle2 = new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, button.TextDrawRect.Width, button.TextDrawRect.Height);
				}
				else
				{
					rectangle2 = new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, displayRectangle.Width, button.TextDrawRect.Height);
					if ((button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems)
					{
						rectangle2.Width -= 10;
					}
				}
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			rectangle2.Offset(displayRectangle.Left, displayRectangle.Top);
			if (button.Orientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, ButtonItemPainter.GetDrawText(button.Text), val, textColor, new Rectangle(rectangle2.Top, -rectangle2.Right, rectangle2.Height, rectangle2.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (rectangle2.Right > button.DisplayRectangle.Right)
				{
					rectangle2.Width = button.DisplayRectangle.Right - rectangle2.Left;
				}
				TextDrawing.DrawString(graphics, ButtonItemPainter.GetDrawText(button.Text), val, textColor, rectangle2, eTextFormat2);
				if (!button.DesignMode && button.Focused && !pa.IsOnMenu && !pa.IsOnMenuBar)
				{
					Rectangle rectangle3 = rectangle2;
					ControlPaint.DrawFocusRectangle(graphics, rectangle3);
				}
			}
		}
		if (flag)
		{
			part = ThemeToolbarParts.SplitButtonDropDown;
			themeToolbarStates = (ButtonItemPainter.IsItemEnabled(button, pa) ? ThemeToolbarStates.Normal : ThemeToolbarStates.Disabled);
			if (button.HotTrackingStyle != eHotTrackingStyle.None && button.HotTrackingStyle != eHotTrackingStyle.Image && ButtonItemPainter.IsItemEnabled(button, pa))
			{
				if (button.Expanded || button.IsMouseDown)
				{
					themeToolbarStates = ThemeToolbarStates.Pressed;
				}
				else if (button.IsMouseOver && button.Checked)
				{
					themeToolbarStates = ThemeToolbarStates.HotChecked;
				}
				else if (button.Checked)
				{
					themeToolbarStates = ThemeToolbarStates.Checked;
				}
				else if (button.IsMouseOver)
				{
					themeToolbarStates = ThemeToolbarStates.Hot;
				}
			}
			if (!button.AutoExpandOnClick)
			{
				if (button.Orientation == eOrientation.Horizontal)
				{
					Rectangle subItemsRect = button.SubItemsRect;
					subItemsRect.Offset(displayRectangle.X, displayRectangle.Y);
					themeToolbar.DrawBackground(graphics, part, themeToolbarStates, subItemsRect);
				}
				else
				{
					Rectangle subItemsRect2 = button.SubItemsRect;
					subItemsRect2.Offset(displayRectangle.X, displayRectangle.Y);
					themeToolbar.DrawBackground(graphics, part, themeToolbarStates, subItemsRect2);
				}
			}
		}
		if (button.Focused && button.DesignMode)
		{
			Rectangle r2 = displayRectangle;
			r2.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(graphics, r2, pa.Colors.ItemDesignTimeBorder);
		}
		image?.Dispose();
	}

	private static eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		if (!IsOnMenu(button, pa))
		{
			if (button.ContainerControl is RibbonStrip && (image == null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (pa.IsOnMenuBar || (pa.ContainerControl is Bar && image == null) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
		}
		return eTextFormat2;
	}

	private static bool IsOnMenu(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = pa.IsOnMenu;
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		return flag;
	}
}
