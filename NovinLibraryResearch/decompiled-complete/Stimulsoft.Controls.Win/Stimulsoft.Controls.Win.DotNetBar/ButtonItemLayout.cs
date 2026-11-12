using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ButtonItemLayout
{
	private struct ButtonTextSize
	{
		public Size TextSize;

		public Size ShortcutTextSize;

		public ButtonTextSize(Size textSize, Size shortcutTextSize)
		{
			TextSize = textSize;
			ShortcutTextSize = shortcutTextSize;
		}
	}

	public static void Arrange(ButtonItem button)
	{
		int num = 8;
		bool flag = button.IsOnMenu;
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		bool flag2 = false;
		Size empty = Size.Empty;
		if (!string.IsNullOrEmpty(button.Symbol))
		{
			flag2 = true;
			empty = button.ImageSize;
		}
		else
		{
			using (CompositeImage compositeImage = button.GetImage())
			{
				if (compositeImage != null)
				{
					flag2 = true;
				}
			}
			empty = GetLayoutImageSize(button, flag2, flag, startButtonType: false);
		}
		bool isRightToLeft = button.IsRightToLeft;
		Rectangle displayRectangle = button.DisplayRectangle;
		if (flag)
		{
			empty.Height += 2;
			empty.Width += 7;
			if (button.IsOnCustomizeMenu && !isRightToLeft)
			{
				button.ImageDrawRect = new Rectangle(displayRectangle.Height + 2, Math.Max(0, (displayRectangle.Height - empty.Height) / 2), empty.Width, empty.Height);
			}
			else
			{
				button.ImageDrawRect = new Rectangle(0, Math.Max(0, (displayRectangle.Height - empty.Height) / 2), empty.Width, empty.Height);
			}
			button.TextDrawRect = new Rectangle(button.ImageDrawRect.Right + 8, Math.Max(0, (displayRectangle.Height - button.TextDrawRect.Height) / 2), button.TextDrawRect.Width, button.TextDrawRect.Height);
			return;
		}
		int val = 0;
		if (!button.SubItemsRect.IsEmpty)
		{
			Rectangle subItemsRect = button.SubItemsRect;
			if (button.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
			{
				subItemsRect = new Rectangle(0, displayRectangle.Height - subItemsRect.Height, displayRectangle.Width, subItemsRect.Height);
				displayRectangle.Height -= subItemsRect.Height;
			}
			else if (button.Orientation == eOrientation.Horizontal)
			{
				if (isRightToLeft)
				{
					subItemsRect = new Rectangle(0, 0, subItemsRect.Width, displayRectangle.Height);
					val = subItemsRect.Width;
				}
				else
				{
					subItemsRect = new Rectangle(displayRectangle.Width - subItemsRect.Width, 0, subItemsRect.Width, displayRectangle.Height);
					displayRectangle.Width -= subItemsRect.Width;
				}
			}
			else
			{
				subItemsRect = new Rectangle(0, 0, displayRectangle.Width, subItemsRect.Height);
				displayRectangle.Height -= subItemsRect.Height;
			}
			button.SubItemsRect = subItemsRect;
		}
		if (!flag2 || button.ButtonStyle == eButtonStyle.TextOnlyAlways)
		{
			int num2 = displayRectangle.Height - 2;
			button.TextDrawRect = new Rectangle(2, Math.Max(0, (displayRectangle.Height - num2) / 2), displayRectangle.Width - 4, num2);
		}
		else if ((button.ButtonStyle == eButtonStyle.Default && button.ImagePosition != eImagePosition.Top && button.ImagePosition != eImagePosition.Bottom) || displayRectangle.Width < empty.Width + num)
		{
			button.ImageDrawRect = new Rectangle(Math.Max(0, (displayRectangle.Width - button.ImageDrawRect.Width) / 2), Math.Max(0, (displayRectangle.Height - button.ImageDrawRect.Height) / 2), button.ImageDrawRect.Width, button.ImageDrawRect.Height);
			if (displayRectangle.Width < empty.Width + num)
			{
				button.TextDrawRect = Rectangle.Empty;
			}
		}
		else if ((button.ImagePosition == eImagePosition.Left && !isRightToLeft) || (button.ImagePosition == eImagePosition.Right && isRightToLeft))
		{
			button.ImageDrawRect = new Rectangle(0, Math.Max(0, (displayRectangle.Height - button.ImageDrawRect.Height) / 2), button.ImageDrawRect.Width, button.ImageDrawRect.Height);
			button.TextDrawRect = new Rectangle(button.ImageDrawRect.Right - 2, Math.Max(0, (displayRectangle.Height - button.TextDrawRect.Height) / 2), Math.Min(button.TextDrawRect.Width, displayRectangle.Width - (button.ImageDrawRect.Right - 2)), button.TextDrawRect.Height);
		}
		else if ((button.ImagePosition == eImagePosition.Right && !isRightToLeft) || (button.ImagePosition == eImagePosition.Left && isRightToLeft))
		{
			button.ImageDrawRect = new Rectangle(displayRectangle.Width - button.ImageDrawRect.Width, Math.Max(0, (displayRectangle.Height - button.TextDrawRect.Height) / 2), button.ImageDrawRect.Width, button.ImageDrawRect.Height);
			button.TextDrawRect = new Rectangle(Math.Max(val, button.ImageDrawRect.X - button.TextDrawRect.Width + 2), Math.Max(0, (displayRectangle.Height - button.TextDrawRect.Height) / 2), Math.Min(button.TextDrawRect.Width, button.ImageDrawRect.X), button.TextDrawRect.Height);
		}
		else if (button.ImagePosition == eImagePosition.Top)
		{
			int y = Math.Max(2, (displayRectangle.Height - (button.TextDrawRect.Height + button.ImageDrawRect.Height - 2)) / 2);
			if (button.Name != "sysOverflowButton" && button.Parent is ItemContainer && ((ItemContainer)button.Parent).LayoutOrientation == eOrientation.Horizontal && ((ItemContainer)button.Parent).VerticalItemAlignment == eVerticalItemsAlignment.Top)
			{
				y = 2;
			}
			button.ImageDrawRect = new Rectangle(0, y, displayRectangle.Width, button.ImageDrawRect.Height);
			button.TextDrawRect = new Rectangle(Math.Max(0, (displayRectangle.Width - button.TextDrawRect.Width) / 2), button.ImageDrawRect.Bottom, button.TextDrawRect.Width, button.TextDrawRect.Height);
		}
		else if (button.ImagePosition == eImagePosition.Bottom)
		{
			int num3 = Math.Max(0, (displayRectangle.Height - (button.TextDrawRect.Height + button.ImageDrawRect.Height - 2)) / 2);
			button.ImageDrawRect = new Rectangle(0, displayRectangle.Height - num3 - button.ImageDrawRect.Height, displayRectangle.Width, button.ImageDrawRect.Height);
			button.TextDrawRect = new Rectangle(Math.Max(0, (displayRectangle.Width - button.ImageDrawRect.Width) / 2), Math.Max(0, button.ImageDrawRect.Y + 2 - button.TextDrawRect.Height), button.TextDrawRect.Width, Math.Min(button.TextDrawRect.Height, button.ImageDrawRect.Y));
		}
	}

	public static Size MeasureItemText(BaseItem item, Graphics g, int containerWidth, Font font, eTextFormat stringFormat, bool rightToLeft)
	{
		return MeasureItemText(item, g, containerWidth, font, stringFormat, rightToLeft, ribbonBarButton: false, eImagePosition.Left);
	}

	public static Size MeasureItemText(BaseItem item, Graphics g, int containerWidth, Font font, eTextFormat stringFormat, bool rightToLeft, bool ribbonBarButton, eImagePosition imagePosition)
	{
		if (item.Text == "" && item.TextMarkupBody == null)
		{
			return Size.Empty;
		}
		Size empty = Size.Empty;
		if (item.TextMarkupBody == null)
		{
			return TextDrawing.MeasureString(g, ButtonItemPainter.GetDrawText(item.Text), font, containerWidth, stringFormat);
		}
		Size availableSize = new Size(containerWidth, 1);
		if (containerWidth == 0)
		{
			availableSize.Width = 1600;
		}
		MarkupDrawContext markupDrawContext = new MarkupDrawContext(g, font, Color.Empty, rightToLeft: false);
		item.TextMarkupBody.Measure(availableSize, markupDrawContext);
		availableSize = item.TextMarkupBody.Bounds.Size;
		if (containerWidth != 0 && (!ribbonBarButton || imagePosition != eImagePosition.Top))
		{
			availableSize.Width = containerWidth;
		}
		markupDrawContext.RightToLeft = rightToLeft;
		item.TextMarkupBody.Arrange(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
		return item.TextMarkupBody.Bounds.Size;
	}

	public static void LayoutButton(ButtonItem button)
	{
		LayoutButton(button, startButtonType: false);
	}

	private static bool UseRibbonWordBreak(ButtonItem button)
	{
		if (button.TextMarkupBody == null)
		{
			return button.Text.IndexOf(' ') > 0;
		}
		if (button.Text.IndexOf(' ') <= 0 || button.TextMarkupBody.HasExpandElement)
		{
			if (button.Text.Split(new char[1] { ' ' }).Length > 2 && button.TextMarkupBody.HasExpandElement)
			{
				return button.ButtonStyle != eButtonStyle.Default;
			}
			return false;
		}
		return true;
	}

	public static void LayoutButton(ButtonItem button, bool startButtonType)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Invalid comparison between Unknown and I4
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		object containerControl = button.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.Disposing || val.IsDisposed)
		{
			return;
		}
		if (val is ButtonX && button._FitContainer)
		{
			LayoutButtonX(button);
			return;
		}
		if (button.FixedSize.Width > 0 && button.FixedSize.Height > 0)
		{
			button.SetDisplayRectangle(new Rectangle(button.DisplayRectangle.Location, button.FixedSize));
			LayoutButtonX(button);
			return;
		}
		bool flag = button.IsOnMenu;
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		bool flag2 = false;
		bool flag3 = false;
		if (!string.IsNullOrEmpty(button.Symbol))
		{
			flag2 = true;
			flag3 = true;
		}
		else
		{
			using CompositeImage compositeImage = button.GetImage();
			if (compositeImage != null || startButtonType)
			{
				flag2 = true;
			}
		}
		eImagePosition eImagePosition2 = button.ImagePosition;
		bool flag4 = (int)val.RightToLeft == 1;
		Rectangle textDrawRect = Rectangle.Empty;
		Rectangle imageDrawRect = Rectangle.Empty;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = new Rectangle(button.DisplayRectangle.Location, Size.Empty);
		if (flag4 && button.Orientation == eOrientation.Horizontal)
		{
			switch (eImagePosition2)
			{
			case eImagePosition.Left:
				eImagePosition2 = eImagePosition.Right;
				break;
			case eImagePosition.Right:
				eImagePosition2 = eImagePosition.Left;
				break;
			}
		}
		int num = 0;
		if (button._FitContainer)
		{
			num = button.DisplayRectangle.Width - 4;
		}
		rectangle2.Width = 0;
		rectangle2.Height = 0;
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			eTextFormat eTextFormat2 = GetTextFormat(button);
			Size empty = Size.Empty;
			if (flag3)
			{
				Font fontAwesome = Symbols.GetFontAwesome(button.SymbolSize);
				empty = ((!button.IsOnMenu) ? TextDrawing.MeasureString(val2, button.Symbol, fontAwesome) : TextDrawing.MeasureString(val2, "\uf00a", fontAwesome));
				int num2 = (int)Math.Ceiling((float)fontAwesome.FontFamily.GetCellDescent(fontAwesome.Style) * fontAwesome.Size / (float)fontAwesome.FontFamily.GetEmHeight(fontAwesome.Style));
				empty.Height -= num2;
				button.ImageSize = empty;
			}
			else
			{
				empty = GetLayoutImageSize(button, flag2, flag, startButtonType);
			}
			bool flag5 = false;
			if (button._FitContainer && flag2 && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
			{
				num -= empty.Width + 10;
			}
			else if (button.RibbonWordWrap && flag2 && eImagePosition2 == eImagePosition.Top && val is RibbonBar && UseRibbonWordBreak(button))
			{
				num = empty.Width + 4;
				eTextFormat2 |= eTextFormat.WordBreak;
				flag5 = true;
			}
			Font font = button.GetFont(null, isLayout: true);
			SizeF sizeF = SizeF.Empty;
			if ((button.Text != "" || button.TextMarkupBody != null) && (!flag2 || flag || button.ButtonStyle != 0 || (button.ImagePosition != eImagePosition.Left && flag2)))
			{
				sizeF = MeasureItemText(button, val2, num, font, eTextFormat2, flag4, flag5, eImagePosition2);
				int num3 = 0;
				int num4 = Math.Max(14, empty.Width / 2);
				while (flag5 && (double)sizeF.Height > (double)font.Height * 2.2 && num3 < 3)
				{
					num += num4;
					sizeF = MeasureItemText(button, val2, num, font, eTextFormat2, flag4, flag5, eImagePosition2);
					num3++;
				}
				if (val is RibbonBar && flag2 && eImagePosition2 == eImagePosition.Top)
				{
					sizeF.Height += 2f;
				}
			}
			if (flag)
			{
				if (empty.IsEmpty)
				{
					empty = new Size((int)Math.Ceiling(16.0 * StiScale2.Factor), (int)Math.Ceiling(16.0 * StiScale2.Factor));
				}
				empty.Height += (int)Math.Ceiling(2.0 * StiScale2.Factor);
				empty.Width += (int)Math.Ceiling(7.0 * StiScale2.Factor);
				if (sizeF.Height > (float)empty.Height)
				{
					rectangle2.Height = (int)sizeF.Height + (int)Math.Ceiling(4.0 * StiScale2.Factor);
				}
				else
				{
					rectangle2.Height = empty.Height + (int)Math.Ceiling(4.0 * StiScale2.Factor);
				}
				rectangle2.Height += button.VerticalPadding;
				imageDrawRect = ((!button.IsOnCustomizeMenu || flag4) ? new Rectangle(0, (rectangle2.Height - empty.Height) / 2, empty.Width, empty.Height) : new Rectangle(rectangle2.Height + (int)Math.Ceiling(2.0 * StiScale2.Factor), (rectangle2.Height - empty.Height) / 2, empty.Width, empty.Height));
				rectangle2.Width = (int)sizeF.Width;
				if (button.DrawShortcutText != "")
				{
					Size size = TextDrawing.MeasureString(val2, button.DrawShortcutText, font, 0, eTextFormat2);
					rectangle2.Width += size.Width + (int)Math.Ceiling(14.0 * StiScale2.Factor);
				}
				textDrawRect = new Rectangle(imageDrawRect.Right + (int)Math.Ceiling(2.0 * StiScale2.Factor), (int)Math.Ceiling(2.0 * StiScale2.Factor), rectangle2.Width, rectangle2.Height - (int)Math.Ceiling(4.0 * StiScale2.Factor));
				rectangle2.Width += imageDrawRect.Right + (int)Math.Ceiling(8.0 * StiScale2.Factor) + (int)Math.Ceiling(26.0 * StiScale2.Factor);
				rectangle2.Width += button.HorizontalPadding;
			}
			else
			{
				bool isThemed = button.IsThemed;
				if (button.Orientation == eOrientation.Horizontal && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
				{
					empty.Width += button.ImagePaddingHorizontal;
					if (sizeF.Height > (float)empty.Height)
					{
						rectangle2.Height = (int)sizeF.Height + button.ImagePaddingVertical;
					}
					else
					{
						rectangle2.Height = empty.Height + button.ImagePaddingVertical;
					}
					rectangle2.Height += button.VerticalPadding;
					if (isThemed && !button.IsOnMenuBar)
					{
						rectangle2.Height += (int)Math.Ceiling(4.0 * StiScale2.Factor);
					}
					imageDrawRect = Rectangle.Empty;
					if (button.ButtonStyle != eButtonStyle.TextOnlyAlways && flag2)
					{
						imageDrawRect = new Rectangle(0, (rectangle2.Height - empty.Height) / 2, empty.Width, empty.Height);
					}
					textDrawRect = Rectangle.Empty;
					if (button.ButtonStyle != 0 || !flag2)
					{
						if (imageDrawRect.Right > 0)
						{
							rectangle2.Width = (int)sizeF.Width + 1;
							textDrawRect = new Rectangle(imageDrawRect.Right - (int)Math.Ceiling(2.0 * StiScale2.Factor), (int)Math.Ceiling(2.0 * StiScale2.Factor), rectangle2.Width, rectangle2.Height - (int)Math.Ceiling(4.0 * StiScale2.Factor));
						}
						else
						{
							rectangle2.Width = (int)Math.Max(sizeF.Width, empty.Width) + (int)Math.Ceiling(6.0 * StiScale2.Factor);
							if (!flag2 && button.IsOnMenuBar)
							{
								rectangle2.Width += (int)Math.Ceiling(6.0 * StiScale2.Factor);
								textDrawRect = new Rectangle((int)Math.Ceiling(2.0 * StiScale2.Factor), (int)Math.Ceiling(2.0 * StiScale2.Factor), rectangle2.Width, rectangle2.Height - (int)Math.Ceiling(4.0 * StiScale2.Factor));
							}
							else
							{
								textDrawRect = new Rectangle((int)Math.Ceiling(2.0 * StiScale2.Factor), (int)Math.Ceiling(2.0 * StiScale2.Factor), rectangle2.Width + button.HorizontalPadding - (int)Math.Ceiling(4.0 * StiScale2.Factor), rectangle2.Height - (int)Math.Ceiling(4.0 * StiScale2.Factor));
							}
						}
					}
					rectangle2.Width += imageDrawRect.Right;
					if (eImagePosition2 == eImagePosition.Right && imageDrawRect.Right > 0 && flag2)
					{
						textDrawRect.X = (int)Math.Ceiling(3.0 * StiScale2.Factor);
						imageDrawRect.X = rectangle2.Width - imageDrawRect.Width;
					}
					rectangle2.Width += button.HorizontalPadding;
				}
				else if (button.Orientation == eOrientation.Horizontal)
				{
					if (sizeF.Width > (float)empty.Width)
					{
						rectangle2.Width = (int)sizeF.Width + button.ImagePaddingHorizontal;
					}
					else
					{
						rectangle2.Width = empty.Width + button.ImagePaddingHorizontal;
					}
					rectangle2.Height = (int)((float)empty.Height + sizeF.Height + (float)button.ImagePaddingVertical);
					rectangle2.Width += button.HorizontalPadding;
					rectangle2.Height += button.VerticalPadding;
					if (eImagePosition2 == eImagePosition.Top)
					{
						imageDrawRect = new Rectangle(0, button.VerticalPadding / 2 + (int)Math.Ceiling(2.0 * StiScale2.Factor), rectangle2.Width, empty.Height);
						textDrawRect = new Rectangle((int)((float)rectangle2.Width - sizeF.Width) / 2, imageDrawRect.Bottom, (int)sizeF.Width, (int)sizeF.Height + (int)Math.Ceiling(5.0 * StiScale2.Factor));
					}
					else
					{
						textDrawRect = new Rectangle((int)((float)rectangle2.Width - sizeF.Width) / 2, button.VerticalPadding / 2, (int)sizeF.Width, (int)sizeF.Height + (int)Math.Ceiling(5.0 * StiScale2.Factor));
						imageDrawRect = new Rectangle(0, textDrawRect.Bottom, rectangle2.Width, empty.Height + (int)Math.Ceiling(5.0 * StiScale2.Factor));
					}
				}
				else
				{
					if (sizeF.Height > (float)empty.Width && button.ButtonStyle != 0)
					{
						rectangle2.Width = (int)sizeF.Height + (int)Math.Ceiling(6.0 * StiScale2.Factor);
					}
					else
					{
						rectangle2.Width = empty.Width + (int)Math.Ceiling(10.0 * StiScale2.Factor);
					}
					rectangle2.Width += button.HorizontalPadding;
					if (button.ButtonStyle != 0 || !flag2)
					{
						if (flag2)
						{
							rectangle2.Height = (int)((float)empty.Height + sizeF.Width + (float)(int)Math.Ceiling(12.0 * StiScale2.Factor));
						}
						else
						{
							rectangle2.Height = (int)(sizeF.Width + (float)(int)Math.Ceiling(6.0 * StiScale2.Factor));
						}
					}
					else
					{
						rectangle2.Height = empty.Height + (int)Math.Ceiling(6.0 * StiScale2.Factor);
					}
					if (eImagePosition2 == eImagePosition.Top || eImagePosition2 == eImagePosition.Left)
					{
						if (flag2)
						{
							imageDrawRect = new Rectangle(0, 0, rectangle2.Width, empty.Height + (int)Math.Ceiling(6.0 * StiScale2.Factor));
						}
						textDrawRect = new Rectangle((int)((float)rectangle2.Width - sizeF.Height) / 2, imageDrawRect.Bottom + (int)Math.Ceiling(2.0 * StiScale2.Factor), (int)sizeF.Height, (int)sizeF.Width + (int)Math.Ceiling(5.0 * StiScale2.Factor));
					}
					else
					{
						textDrawRect = new Rectangle((int)((float)rectangle2.Width - sizeF.Height) / 2, (int)Math.Ceiling(5.0 * StiScale2.Factor), (int)sizeF.Height, (int)sizeF.Width + (int)Math.Ceiling(5.0 * StiScale2.Factor));
						if (flag2)
						{
							imageDrawRect = new Rectangle(0, textDrawRect.Bottom - (int)Math.Ceiling(3.0 * StiScale2.Factor), rectangle2.Width, empty.Height + (int)Math.Ceiling(5.0 * StiScale2.Factor));
						}
					}
					rectangle2.Height += button.VerticalPadding;
				}
				if (HasExpandPart(button))
				{
					rectangle = GetSubItemsButtonBounds(button, rectangle2, flag4);
					Rectangle b = rectangle;
					b.Offset(rectangle2.Location);
					if (flag4 && (!(val is RibbonBar) || (button.ImagePosition != eImagePosition.Top && button.ImagePosition != eImagePosition.Bottom)))
					{
						if (!textDrawRect.IsEmpty)
						{
							textDrawRect.Offset(rectangle.Width + 1, 0);
						}
						if (!imageDrawRect.IsEmpty && button.Orientation == eOrientation.Horizontal && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
						{
							imageDrawRect.Offset(rectangle.Width, 0);
						}
						rectangle2.X += rectangle.Width;
					}
					rectangle2 = Rectangle.Union(rectangle2, b);
				}
			}
		}
		finally
		{
			val2.TextRenderingHint = (TextRenderingHint)0;
			val2.SmoothingMode = (SmoothingMode)0;
			val2.Dispose();
		}
		val = null;
		button.SetDisplayRectangle(rectangle2);
		button.ImageDrawRect = imageDrawRect;
		button.TextDrawRect = textDrawRect;
		button.SubItemsRect = rectangle;
	}

	private static bool HasExpandPart(ButtonItem button)
	{
		if ((button.SubItems.Count > 0 || button.PopupType == ePopupType.Container || button.ShowArrow) && button.ShowSubItems && !button.IsOnMenuBar)
		{
			if (button.TextMarkupBody != null && button.TextMarkupBody.HasExpandElement)
			{
				return button.ButtonStyle == eButtonStyle.Default;
			}
			return true;
		}
		return false;
	}

	private static Size GetLayoutImageSize(ButtonItem button, bool hasImage, bool isOnMenu, bool startButtonType)
	{
		Size result = Size.Empty;
		result = ((button.Parent != null && button.ImageFixedSize.IsEmpty) ? ((!(button.Parent is ImageItem { SubItemsImageSize: { IsEmpty: false } } imageItem)) ? button.ImageSize : ((!hasImage || isOnMenu) ? new Size(imageItem.SubItemsImageSize.Width, imageItem.SubItemsImageSize.Height) : ((button.Orientation != 0) ? new Size(Math.Max(imageItem.SubItemsImageSize.Width, button.ImageSize.Width), button.ImageSize.Height) : new Size(button.ImageSize.Width, Math.Max(imageItem.SubItemsImageSize.Height, button.ImageSize.Height))))) : (button.ImageFixedSize.IsEmpty ? button.ImageSize : button.ImageFixedSize));
		if (startButtonType && GlobalManager.Renderer is Office2007Renderer { ColorTable: var colorTable } && colorTable.RibbonControl.StartButtonDefault != null)
		{
			if (result.Width < colorTable.RibbonControl.StartButtonDefault.Width)
			{
				result.Width = colorTable.RibbonControl.StartButtonDefault.Width;
			}
			if (result.Height < colorTable.RibbonControl.StartButtonDefault.Height)
			{
				result.Height = colorTable.RibbonControl.StartButtonDefault.Height;
			}
		}
		return result;
	}

	public static Rectangle GetSubItemsButtonBounds(ButtonItem button, Rectangle buttonBounds, bool rightToLeft)
	{
		Rectangle empty = Rectangle.Empty;
		return (button.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)) ? new Rectangle(0, buttonBounds.Height, buttonBounds.Width, button.SubItemsExpandWidth) : ((button.Orientation != 0) ? new Rectangle(0, buttonBounds.Height - 2, buttonBounds.Width, button.SubItemsExpandWidth) : (button.IsThemed ? ((!rightToLeft) ? new Rectangle(buttonBounds.Width, 0, button.SubItemsExpandWidth, buttonBounds.Height) : new Rectangle(0, 0, button.SubItemsExpandWidth, buttonBounds.Height)) : ((!rightToLeft) ? new Rectangle(buttonBounds.Width + 1, 0, button.SubItemsExpandWidth, buttonBounds.Height) : new Rectangle(0, 0, button.SubItemsExpandWidth, buttonBounds.Height))));
	}

	public static eTextFormat GetTextFormat(ButtonItem button)
	{
		eTextFormat eTextFormat2 = eTextFormat.Default;
		eTextFormat2 = (button._FitContainer ? (eTextFormat2 | eTextFormat.WordBreak) : (eTextFormat2 | eTextFormat.SingleLine));
		return eTextFormat2 | eTextFormat.VerticalCenter;
	}

	public static void LayoutButtonX(ButtonItem button)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		object containerControl = button.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		ButtonX buttonX = button.ContainerControl as ButtonX;
		if (!BarFunctions.IsHandleValid(val))
		{
			return;
		}
		bool flag = button.IsOnMenu;
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		bool flag2 = false;
		if (!string.IsNullOrEmpty(button.Symbol))
		{
			flag2 = true;
		}
		else
		{
			using CompositeImage compositeImage = button.GetImage();
			if (compositeImage != null)
			{
				flag2 = true;
			}
		}
		eImagePosition eImagePosition2 = button.ImagePosition;
		bool flag3 = (int)val.RightToLeft == 1;
		Rectangle textDrawRect = Rectangle.Empty;
		Rectangle imageDrawRect = Rectangle.Empty;
		Rectangle subItemsRect = Rectangle.Empty;
		Rectangle bounds = button.Bounds;
		if (button.SubItems.Count > 0 && button.ShowSubItems && (button.TextMarkupBody == null || !button.TextMarkupBody.HasExpandElement || button.ButtonStyle == eButtonStyle.Default))
		{
			if (button.Orientation == eOrientation.Horizontal)
			{
				subItemsRect = ((!flag3) ? new Rectangle(bounds.Width - button.SubItemsExpandWidth, 0, button.SubItemsExpandWidth, bounds.Height) : new Rectangle(0, 0, button.SubItemsExpandWidth, bounds.Height));
				if (flag3)
				{
					bounds.X += button.SubItemsExpandWidth + 1;
				}
				bounds.Width -= button.SubItemsExpandWidth + 1;
			}
			else
			{
				subItemsRect = new Rectangle(0, bounds.Height - button.SubItemsExpandWidth, bounds.Width, button.SubItemsExpandWidth);
				bounds.Height -= button.SubItemsExpandWidth + 1;
			}
		}
		if (flag3 && button.Orientation == eOrientation.Horizontal)
		{
			switch (eImagePosition2)
			{
			case eImagePosition.Left:
				eImagePosition2 = eImagePosition.Right;
				break;
			case eImagePosition.Right:
				eImagePosition2 = eImagePosition.Left;
				break;
			}
		}
		int num = 0;
		num = bounds.Width;
		Graphics val2 = BarFunctions.CreateGraphics(val);
		try
		{
			Size empty = Size.Empty;
			if (string.IsNullOrEmpty(button.Symbol))
			{
				empty = (button.ImageFixedSize.IsEmpty ? button.ImageSize : button.ImageFixedSize);
			}
			else
			{
				Font fontAwesome = Symbols.GetFontAwesome(button.SymbolSize);
				empty = TextDrawing.MeasureStringLegacy(val2, button.Symbol, fontAwesome, Size.Empty, eTextFormat.Default);
				int num2 = (int)Math.Ceiling((float)fontAwesome.FontFamily.GetCellDescent(fontAwesome.Style) * fontAwesome.Size / (float)fontAwesome.FontFamily.GetEmHeight(fontAwesome.Style));
				empty.Height -= num2;
				button.ImageSize = empty;
			}
			if (flag2 && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
			{
				num = ((buttonX == null) ? (num - (empty.Width + 8)) : (num - (empty.Width + buttonX.ImageTextSpacing * 2 + 3)));
			}
			if (flag2 && !empty.IsEmpty && buttonX != null && buttonX.ImageTextSpacing != 0)
			{
				if (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right)
				{
					empty.Width += buttonX.ImageTextSpacing * 2;
				}
				else
				{
					empty.Height += buttonX.ImageTextSpacing * 2;
				}
			}
			Font font = button.GetFont(null, isLayout: true);
			SizeF sizeF = SizeF.Empty;
			eTextFormat eTextFormat2 = eTextFormat.VerticalCenter;
			if (BarFunctions.IsWindowsXP && BarUtilities.UseTextRenderer)
			{
				eTextFormat2 |= eTextFormat.LeftAndRightPadding;
			}
			if (buttonX != null || (val is RibbonBar && button.RibbonWordWrap))
			{
				eTextFormat2 |= eTextFormat.WordBreak;
			}
			if (button.Text != "")
			{
				if (button.TextMarkupBody == null)
				{
					if (button.Orientation == eOrientation.Vertical && !flag)
					{
						sizeF = TextDrawing.MeasureStringLegacy(val2, ButtonItemPainter.GetDrawText(button.Text), font, new Size(num, 0), eTextFormat2);
					}
					else
					{
						sizeF = TextDrawing.MeasureString(val2, ButtonItemPainter.GetDrawText(button.Text), font, num, eTextFormat2);
						if (BarFunctions.IsWindowsXP && BarUtilities.UseTextRenderer)
						{
							sizeF.Width += 2f;
						}
					}
				}
				else
				{
					Size availableSize = new Size(num, 1);
					if (num == 0)
					{
						availableSize.Width = 1600;
					}
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, font, Color.Empty, rightToLeft: false);
					markupDrawContext.RightToLeft = flag3;
					button.TextMarkupBody.Measure(availableSize, markupDrawContext);
					availableSize = button.TextMarkupBody.Bounds.Size;
					button.TextMarkupBody.Arrange(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
					sizeF = button.TextMarkupBody.Bounds.Size;
				}
			}
			if (button.Orientation == eOrientation.Horizontal && (eImagePosition2 == eImagePosition.Left || eImagePosition2 == eImagePosition.Right))
			{
				if (button.ButtonStyle != 0 || !flag2)
				{
					empty.Width += 4;
				}
				imageDrawRect = Rectangle.Empty;
				if (button.ButtonStyle != eButtonStyle.TextOnlyAlways && flag2)
				{
					imageDrawRect = ((eImagePosition2 == eImagePosition.Left) ? ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Left) ? new Rectangle(2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Right) ? new Rectangle(bounds.Width - (empty.Width + (int)Math.Ceiling(sizeF.Width) + 4), (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX != null) ? new Rectangle((int)((float)bounds.Width - (sizeF.Width + (float)empty.Width)) / 2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((!subItemsRect.IsEmpty || button.ButtonStyle != 0) ? new Rectangle(0, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : new Rectangle((bounds.Width - empty.Width) / 2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height))))) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Left) ? new Rectangle((int)sizeF.Width + 4, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Right) ? new Rectangle(bounds.Width - empty.Width, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : ((buttonX == null) ? new Rectangle(bounds.Width - empty.Width, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height) : new Rectangle(bounds.Width - (int)((float)bounds.Width - (sizeF.Width + (float)empty.Width)) / 2 - empty.Width, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height)))));
				}
				textDrawRect = Rectangle.Empty;
				if (button.ButtonStyle != 0 || !flag2)
				{
					textDrawRect = (flag2 ? ((eImagePosition2 == eImagePosition.Left) ? ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Center) ? new Rectangle(imageDrawRect.Right + 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height) : ((buttonX == null || (buttonX.TextAlignment != eButtonTextAlignment.Right && buttonX.TextAlignment != 0)) ? new Rectangle(imageDrawRect.Right + 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), bounds.Width - 3 - imageDrawRect.Width, (int)sizeF.Height) : new Rectangle(imageDrawRect.Right + 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), bounds.Width - 3 - imageDrawRect.Width, (int)sizeF.Height))) : ((buttonX != null && buttonX.TextAlignment == eButtonTextAlignment.Center) ? new Rectangle((int)((float)imageDrawRect.X - sizeF.Width) - 1, (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height) : ((buttonX == null || buttonX.TextAlignment != eButtonTextAlignment.Right) ? new Rectangle(3, (int)(((float)bounds.Height - sizeF.Height) / 2f), imageDrawRect.X - 2, (int)sizeF.Height) : new Rectangle(imageDrawRect.X - ((int)sizeF.Width + 4), (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width + 2, (int)sizeF.Height)))) : (((buttonX == null || buttonX.TextAlignment != eButtonTextAlignment.Center) && !button._FixedSizeCenterText) ? new Rectangle(3, (int)(((float)bounds.Height - sizeF.Height) / 2f), bounds.Width - 6, (int)sizeF.Height) : new Rectangle((int)(((float)bounds.Width - sizeF.Width) / 2f), (int)(((float)bounds.Height - sizeF.Height) / 2f), (int)sizeF.Width, (int)sizeF.Height)));
				}
			}
			else if (button.Orientation == eOrientation.Horizontal)
			{
				if (eImagePosition2 == eImagePosition.Top)
				{
					if (button.Is20Button)
					{
						imageDrawRect = new Rectangle(0, 10, bounds.Width, empty.Height);
						textDrawRect = new Rectangle(0, imageDrawRect.Bottom + 5, (int)sizeF.Width, (int)sizeF.Height + 15);
					}
					else
					{
						imageDrawRect = new Rectangle(0, (int)((float)bounds.Height - ((float)empty.Height + sizeF.Height)) / 2, bounds.Width, empty.Height);
						textDrawRect = new Rectangle(0, imageDrawRect.Bottom, (int)sizeF.Width, (int)sizeF.Height + 5);
					}
				}
				else
				{
					textDrawRect = new Rectangle((int)((float)bounds.Width - sizeF.Width) / 2, (int)((float)bounds.Height - ((float)empty.Height + sizeF.Height)) / 2, (int)sizeF.Width, (int)sizeF.Height);
					imageDrawRect = new Rectangle(0, textDrawRect.Bottom, bounds.Width, empty.Height + 5);
				}
			}
			else if (eImagePosition2 == eImagePosition.Top || eImagePosition2 == eImagePosition.Left)
			{
				if (flag2)
				{
					imageDrawRect = ((!subItemsRect.IsEmpty || button.ButtonStyle != 0) ? new Rectangle(0, 0, bounds.Width, empty.Height + 6) : new Rectangle((bounds.Width - empty.Width) / 2, (bounds.Height - empty.Height) / 2, empty.Width, empty.Height));
				}
				textDrawRect = new Rectangle((int)((float)bounds.Width - sizeF.Height) / 2, imageDrawRect.Bottom + 2, (int)sizeF.Height, (int)sizeF.Width + 5);
			}
			else
			{
				textDrawRect = new Rectangle((int)((float)bounds.Width - sizeF.Width) / 2, 0, (int)sizeF.Height, (int)sizeF.Width + 5);
				if (flag2)
				{
					imageDrawRect = new Rectangle(0, textDrawRect.Bottom + 2, bounds.Width, empty.Height + 5);
				}
			}
		}
		finally
		{
			val2.TextRenderingHint = (TextRenderingHint)0;
			val2.SmoothingMode = (SmoothingMode)0;
			val2.Dispose();
		}
		button.ImageDrawRect = imageDrawRect;
		button.TextDrawRect = textDrawRect;
		button.SubItemsRect = subItemsRect;
	}
}
