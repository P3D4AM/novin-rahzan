using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ThemedButtonXPainter
{
	public static void PaintButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		ThemeButton themeButton = pa.ThemeButton;
		ThemeButtonParts pushButton = ThemeButtonParts.PushButton;
		ThemeButtonStates themeButtonStates = ThemeButtonStates.PushButtonNormal;
		Color textColor = ButtonItemPainterHelper.GetTextColor(button, pa);
		ButtonX buttonX = pa.ContainerControl as ButtonX;
		bool flag = true;
		if (buttonX != null)
		{
			flag = buttonX.FocusCuesEnabled;
		}
		Rectangle rectangle = Rectangle.Empty;
		Rectangle displayRectangle = button.DisplayRectangle;
		Font val = null;
		CompositeImage image = button.GetImage();
		eTextFormat eTextFormat2 = GetStringFormat(button, pa, image);
		val = button.GetFont(pa, isLayout: false);
		bool flag2 = (button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems && !button.SubItemsRect.IsEmpty;
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
			themeButtonStates = ThemeButtonStates.PushButtonDisabled;
		}
		else if (button.IsMouseDown || button.Expanded)
		{
			themeButtonStates = ThemeButtonStates.PushButtonPressed;
		}
		else if (button.IsMouseOver && button.Checked)
		{
			themeButtonStates = ThemeButtonStates.PushButtonPressed;
		}
		else if (button.IsMouseOver)
		{
			themeButtonStates = ThemeButtonStates.PushButtonHot;
		}
		else if (button.Checked || button.Expanded)
		{
			themeButtonStates = ThemeButtonStates.PushButtonPressed;
		}
		else if (button.Focused || pa.ContainerControl.Focused)
		{
			themeButtonStates = ThemeButtonStates.PushButtonDefaulted;
		}
		Rectangle r = button.DisplayRectangle;
		if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			r = rectangle;
			r.Inflate(3, 3);
		}
		if (button.HotTrackingStyle != eHotTrackingStyle.None)
		{
			themeButton.DrawBackground(graphics, pushButton, themeButtonStates, r);
		}
		if (image != null && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
		{
			if (themeButtonStates == ThemeButtonStates.PushButtonNormal && button.HotTrackingStyle == eHotTrackingStyle.Color)
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
			else if (themeButtonStates == ThemeButtonStates.PushButtonNormal && !image.IsIcon)
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
			Rectangle bounds = button.TextDrawRect;
			if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				bounds = ((button.Orientation != eOrientation.Vertical) ? new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, button.TextDrawRect.Width, button.TextDrawRect.Height) : new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, button.TextDrawRect.Width, button.TextDrawRect.Height));
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			bounds.Offset(displayRectangle.Left, displayRectangle.Top);
			if (button.Orientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, ButtonItemPainter.GetDrawText(button.Text), val, textColor, new Rectangle(bounds.Top, -bounds.Right, bounds.Height, bounds.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (bounds.Right > button.DisplayRectangle.Right)
				{
					bounds.Width = button.DisplayRectangle.Right - bounds.Left;
				}
				TextDrawing.DrawString(graphics, ButtonItemPainter.GetDrawText(button.Text), val, textColor, bounds, eTextFormat2);
				if (!button.DesignMode && button.Focused && flag && !pa.IsOnMenu && !pa.IsOnMenuBar)
				{
					Rectangle bounds2 = button.Bounds;
					bounds2.Inflate(-3, -3);
					ControlPaint.DrawFocusRectangle(graphics, bounds2);
				}
			}
		}
		if (flag2)
		{
			ButtonItemPainter.PaintButtonExpandIndicator(button, pa);
		}
		image?.Dispose();
	}

	private static eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		if (!IsOnMenu(button, pa) && (image == null || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
		{
			eTextFormat2 |= eTextFormat.HorizontalCenter;
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
