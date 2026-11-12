using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class Office2003ButtonItemPainter : ButtonItemPainter
{
	protected virtual Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		return ButtonItemPainterHelper.GetTextColor(button, pa);
	}

	public override void PaintButton(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = IsOnMenu(button, pa);
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		bool isOnMenuBar = pa.IsOnMenuBar;
		bool isThemed = button.IsThemed;
		Graphics graphics = pa.Graphics;
		Region clip = graphics.Clip;
		try
		{
			graphics.SetClip(button.DisplayRectangle, (CombineMode)1);
			if (!pa.IsOnMenu && !isOnMenuBar && isThemed)
			{
				if (pa.ContainerControl is ButtonX)
				{
					ThemedButtonXPainter.PaintButton(button, pa);
				}
				else
				{
					ThemedButtonItemPainter.PaintButton(button, pa);
				}
				return;
			}
			Rectangle displayRectangle = button.DisplayRectangle;
			Color textColor = GetTextColor(button, pa);
			CompositeImage image = button.GetImage(textColor);
			Rectangle imageRectangle = GetImageRectangle(button, pa, image);
			PaintButtonBackground(button, pa, image);
			Rectangle customizeMenuCheckRectangle = GetCustomizeMenuCheckRectangle(button, pa);
			Rectangle checkRectangle = GetCheckRectangle(button, pa, image);
			Rectangle mouseOverRectangle = GetMouseOverRectangle(button, pa, image);
			bool flag2 = button.IsMouseOver;
			if (button.Expanded && !flag)
			{
				flag2 = false;
			}
			if (flag && button.Expanded && pa.ContainerControl != null && pa.ContainerControl.Parent != null && !pa.ContainerControl.Parent.Bounds.Contains(Control.MousePosition))
			{
				flag2 = true;
			}
			if (button.HotTrackingStyle != eHotTrackingStyle.None && (flag2 || (button.IsMouseDown && !button.DesignMode)))
			{
				PaintButtonMouseOver(button, pa, image, mouseOverRectangle);
			}
			if (flag && button.IsOnCustomizeMenu && button.Visible && !button.SystemItem)
			{
				PaintCustomizeCheck(button, pa, customizeMenuCheckRectangle);
			}
			if (button.Checked && !button.IsOnCustomizeMenu && (button.GetEnabled(pa.ContainerControl) || flag))
			{
				PaintButtonCheck(button, pa, image, checkRectangle);
			}
			if ((image != null || !string.IsNullOrEmpty(button.Symbol)) && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				PaintButtonImage(button, pa, image, imageRectangle);
			}
			if (!(button is ApplicationButton) || button.EffectiveStyle == eDotNetBarStyle.Office2010 || button.EffectiveStyle == eDotNetBarStyle.Metro)
			{
				PaintButtonText(button, pa, textColor, image);
			}
			if (button.ShowExpandArrow)
			{
				PaintExpandButton(button, pa);
			}
			if (button.Focused && button.DesignMode)
			{
				Rectangle r = displayRectangle;
				r.Inflate(-1, -1);
				DesignTime.DrawDesignTimeSelection(graphics, r, pa.Colors.ItemDesignTimeBorder);
			}
			image?.Dispose();
		}
		finally
		{
			if (clip != null)
			{
				graphics.Clip = clip;
				clip.Dispose();
			}
			else
			{
				graphics.ResetClip();
			}
		}
	}

	protected virtual bool IsOnMenu(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = pa.IsOnMenu;
		if (flag && button.Parent is ItemContainer)
		{
			flag = false;
		}
		return flag;
	}

	public override void PaintButtonImage(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle imagebounds)
	{
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		bool flag = IsOnMenu(button, pa);
		if (imagebounds.Width <= 0 || imagebounds.Height <= 0)
		{
			return;
		}
		if (!string.IsNullOrEmpty(button.Symbol))
		{
			Font font = ((button.SymbolSize > 0f) ? Symbols.GetFontAwesome(button.SymbolSize) : Symbols.FontAwesome);
			if (flag)
			{
				TextDrawing.DrawStringLegacy(pa.Graphics, button.Symbol, font, GetTextColor(button, pa), new Rectangle(imagebounds.X + imagebounds.Width / 2, imagebounds.Y, 0, 0), eTextFormat.HorizontalCenter);
			}
			else
			{
				TextDrawing.DrawStringLegacy(pa.Graphics, button.Symbol, font, GetTextColor(button, pa), new Rectangle(imagebounds.X, imagebounds.Y, 0, 0), eTextFormat.Default);
			}
		}
		else if (flag)
		{
			image.DrawImage(pa.Graphics, imagebounds);
		}
		else if (!button.IsMouseOver && button.HotTrackingStyle == eHotTrackingStyle.Color)
		{
			ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
			{
				new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
				new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
				new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
				new float[5] { 0f, 0f, 0f, 1f, 0f },
				new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
			});
			ImageAttributes val = new ImageAttributes();
			val.SetColorMatrix(colorMatrix);
			image.DrawImage(pa.Graphics, imagebounds, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val);
		}
		else
		{
			image.DrawImage(pa.Graphics, imagebounds);
		}
	}

	public override Rectangle GetImageRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		Rectangle result = Rectangle.Empty;
		bool flag = IsOnMenu(button, pa);
		if (image != null || !string.IsNullOrEmpty(button.Symbol))
		{
			Size imageSize = button.ImageSize;
			if (pa.RightToLeft && flag)
			{
				result = new Rectangle(button.DisplayRectangle.Right - (button.ImageDrawRect.X + imageSize.Width + 2), button.ImageDrawRect.Y + button.DisplayRectangle.Y, imageSize.Width, imageSize.Height);
			}
			else
			{
				result = ((flag || (button.ImagePosition != eImagePosition.Top && button.ImagePosition != eImagePosition.Bottom)) ? new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, button.ImageDrawRect.Width, button.ImageDrawRect.Height) : new Rectangle(button.ImageDrawRect.X, button.ImageDrawRect.Y, button.DisplayRectangle.Width, button.ImageDrawRect.Height));
				result.Offset(button.DisplayRectangle.Left, button.DisplayRectangle.Top);
				if ((button.EffectiveStyle == eDotNetBarStyle.Windows7 || button.EffectiveStyle == eDotNetBarStyle.Office2010 || button.EffectiveStyle == eDotNetBarStyle.Metro) && (button is ApplicationButton || button is MetroAppButton))
				{
					result.Offset(14, (result.Height - imageSize.Height) / 2);
				}
				else
				{
					result.Offset((result.Width - imageSize.Width) / 2, (result.Height - imageSize.Height) / 2);
				}
				result.Width = imageSize.Width;
				result.Height = imageSize.Height;
			}
		}
		return result;
	}

	public override Rectangle GetCheckRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		Rectangle result = Rectangle.Empty;
		if (IsOnMenu(button, pa))
		{
			if (pa.RightToLeft)
			{
				result = new Rectangle(button.DisplayRectangle.Right - button.ImageDrawRect.Width + 1, button.DisplayRectangle.Y, button.ImageDrawRect.Width - 2, button.DisplayRectangle.Height);
				result.Inflate(-1, -1);
			}
			else
			{
				result = new Rectangle(button.DisplayRectangle.X, button.DisplayRectangle.Y, button.ImageDrawRect.Width - 2, button.DisplayRectangle.Height);
				result.Inflate(-1, -1);
			}
		}
		else if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			result = GetImageRectangle(button, pa, image);
			result.Inflate(2, 2);
		}
		else
		{
			result = button.DisplayRectangle;
		}
		return result;
	}

	public override Rectangle GetCustomizeMenuCheckRectangle(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = IsOnMenu(button, pa);
		Rectangle result = Rectangle.Empty;
		if (flag && button.IsOnCustomizeMenu && button.Visible && !button.SystemItem)
		{
			result = new Rectangle(button.DisplayRectangle.Left, button.DisplayRectangle.Top, button.DisplayRectangle.Height, button.DisplayRectangle.Height);
			result.Inflate(-1, -1);
		}
		return result;
	}

	public override void PaintCustomizeCheck(ButtonItem button, ItemPaintArgs pa, Rectangle r)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		Color color = pa.Colors.ItemCheckedBackground;
		Graphics graphics = pa.Graphics;
		if (button.IsMouseOver && !pa.Colors.ItemHotBackground2.IsEmpty)
		{
			LinearGradientBrush val = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
			try
			{
				graphics.FillRectangle((Brush)(object)val, r);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			if (button.IsMouseOver)
			{
				color = pa.Colors.ItemHotBackground;
			}
			if (!pa.Colors.ItemCheckedBackground2.IsEmpty && !button.IsMouseOver)
			{
				LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, r);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				SolidBrush val3 = new SolidBrush(color);
				try
				{
					graphics.FillRectangle((Brush)(object)val3, r);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		Pen val4 = new Pen(pa.Colors.ItemCheckedBorder, 1f);
		DisplayHelp.DrawRectangle(graphics, val4, r);
		val4.Dispose();
		val4 = new Pen(pa.Colors.ItemCheckedText);
		Point[] array = new Point[3];
		array[0].X = r.Left + (r.Width - 5) / 2 - 1;
		array[0].Y = r.Top + (r.Height - 6) / 2 + 3;
		array[1].X = array[0].X + 2;
		array[1].Y = array[0].Y + 2;
		array[2].X = array[1].X + 4;
		array[2].Y = array[1].Y - 4;
		graphics.DrawLines(val4, array);
		array[0].X++;
		array[1].X++;
		array[2].X++;
		graphics.DrawLines(val4, array);
		val4.Dispose();
	}

	public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
	{
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		bool isMouseDown = button.IsMouseDown;
		bool flag = IsOnMenu(button, pa);
		Graphics graphics = pa.Graphics;
		Brush val = null;
		Pen val2 = null;
		if (isMouseDown && !flag)
		{
			val = (Brush)((!pa.Colors.ItemPressedBackground2.IsEmpty) ? ((object)BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle)) : ((object)new SolidBrush(pa.Colors.ItemPressedBackground)));
			val2 = new Pen(pa.Colors.ItemPressedBorder, 1f);
		}
		else if (ButtonItemPainter.IsItemEnabled(button, pa))
		{
			val = (Brush)(pa.Colors.ItemHotBackground2.IsEmpty ? ((object)new SolidBrush(pa.Colors.ItemHotBackground)) : ((object)BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle)));
			val2 = new Pen(pa.Colors.ItemHotBorder, 1f);
		}
		else if (flag)
		{
			val2 = new Pen(pa.Colors.ItemHotBorder, 1f);
		}
		if (val != null)
		{
			graphics.FillRectangle(val, r);
		}
		if (val2 != null)
		{
			DisplayHelp.DrawRectangle(graphics, val2, r);
		}
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	public virtual void PaintButtonCheckBackground(ButtonItem button, ItemPaintArgs pa, Rectangle r)
	{
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		if (!button.IsMouseOver || flag)
		{
			DisplayHelp.FillRectangle(graphics, r, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
			DisplayHelp.DrawRectangle(graphics, pa.Colors.ItemCheckedBorder, r);
		}
	}

	public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		if (r.IsEmpty)
		{
			return;
		}
		bool flag = IsOnMenu(button, pa);
		Graphics graphics = pa.Graphics;
		PaintButtonCheckBackground(button, pa, r);
		if (!(((image == null && string.IsNullOrEmpty(button.Symbol)) || button.ButtonStyle == eButtonStyle.TextOnlyAlways) && flag))
		{
			return;
		}
		Pen val = new Pen(ButtonItemPainter.IsItemEnabled(button, pa) ? pa.Colors.ItemCheckedText : pa.Colors.ItemDisabledText);
		try
		{
			double factor = StiScale2.Factor;
			Point[] array = new Point[3];
			array[0].X = (int)((double)r.Left + ((double)r.Width - 5.0 * StiScale2.X) / 2.0 - 1.0 * StiScale2.X);
			array[0].Y = (int)((double)r.Top + ((double)r.Height - 6.0 * StiScale2.Y) / 2.0 + 3.0 * StiScale2.Y);
			array[1].X = array[0].X + (int)(2.0 * StiScale2.X);
			array[1].Y = array[0].Y + (int)(2.0 * StiScale2.Y);
			array[2].X = array[1].X + (int)(4.0 * StiScale2.X);
			array[2].Y = array[1].Y - (int)(4.0 * StiScale2.Y);
			graphics.DrawLines(val, array);
			array[0].X++;
			array[1].X++;
			array[2].X++;
			graphics.DrawLines(val, array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override Rectangle GetMouseOverRectangle(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		Rectangle displayRectangle = button.DisplayRectangle;
		if (button.HotTrackingStyle == eHotTrackingStyle.None || button.HotTrackingStyle == eHotTrackingStyle.Color)
		{
			return Rectangle.Empty;
		}
		if (button.HotTrackingStyle == eHotTrackingStyle.Image && image != null)
		{
			displayRectangle = GetImageRectangle(button, pa, image);
			displayRectangle.Inflate(2, 2);
			return displayRectangle;
		}
		if (IsOnMenu(button, pa))
		{
			displayRectangle.X++;
			displayRectangle.Width -= 2;
		}
		return displayRectangle;
	}

	public override eTextFormat GetStringFormat(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		eTextFormat eTextFormat2 = pa.ButtonStringFormat;
		if (!IsOnMenu(button, pa))
		{
			if ((pa.ContainerControl is RibbonStrip && ((image == null && string.IsNullOrEmpty(button.Symbol)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)) || button._FixedSizeCenterText)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (pa.ContainerControl is ButtonX)
			{
				ButtonX buttonX = pa.ContainerControl as ButtonX;
				if (buttonX.TextAlignment == eButtonTextAlignment.Center)
				{
					eTextFormat2 |= eTextFormat.HorizontalCenter;
				}
				else if (buttonX.TextAlignment == eButtonTextAlignment.Left)
				{
					eTextFormat2 |= eTextFormat.Default;
				}
				else if (buttonX.TextAlignment == eButtonTextAlignment.Right && ((image == null && string.IsNullOrEmpty(button.Symbol)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
				{
					eTextFormat2 |= eTextFormat.Right;
				}
			}
			else if (pa.IsOnMenuBar || ((pa.ContainerControl is Bar || pa.ContainerControl is ButtonX || button.Orientation == eOrientation.Vertical) && image == null && string.IsNullOrEmpty(button.Symbol)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				if (image != null || !string.IsNullOrEmpty(button.Symbol))
				{
					eTextFormat2 |= eTextFormat.WordBreak;
				}
				eTextFormat2 = (eTextFormat2 & ~(eTextFormat2 & eTextFormat.VerticalCenter)) | eTextFormat.Default;
			}
		}
		if (pa.RightToLeft)
		{
			eTextFormat2 |= eTextFormat.RightToLeft;
		}
		return eTextFormat2;
	}

	protected virtual bool IsTextCentered(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		if (!IsOnMenu(button, pa))
		{
			Control containerControl = pa.ContainerControl;
			if ((containerControl is RibbonStrip && ((image == null && string.IsNullOrEmpty(button.Symbol)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)) || button.Name.StartsWith("sysgallery") || button._FixedSizeCenterText)
			{
				return true;
			}
			if (containerControl is ButtonX)
			{
				ButtonX buttonX = containerControl as ButtonX;
				if (buttonX.TextAlignment == eButtonTextAlignment.Center)
				{
					return true;
				}
			}
			else if (pa.IsOnMenuBar || (containerControl is Bar && image == null && string.IsNullOrEmpty(button.Symbol)) || button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual Rectangle GetTextRectangle(ButtonItem button, ItemPaintArgs pa, eTextFormat stringFormat, CompositeImage image)
	{
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		bool isOnMenuBar = pa.IsOnMenuBar;
		Rectangle displayRectangle = button.DisplayRectangle;
		Rectangle result = button.TextDrawRect;
		Rectangle imageDrawRect = button.ImageDrawRect;
		bool rightToLeft = pa.RightToLeft;
		if (flag || button.ButtonStyle != 0 || (image == null && string.IsNullOrEmpty(button.Symbol)) || (!flag && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)))
		{
			if (flag)
			{
				result = ((!rightToLeft) ? new Rectangle(result.X, result.Y, displayRectangle.Width - imageDrawRect.Right - 26, result.Height) : ((!button.IsOnCustomizeMenu) ? new Rectangle(17, result.Y, displayRectangle.Width - imageDrawRect.Width - 28, result.Height) : new Rectangle(displayRectangle.Height, result.Y, displayRectangle.Width - imageDrawRect.Width - 11 - displayRectangle.Height, result.Height)));
			}
			else if ((button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom) && button.Orientation != eOrientation.Vertical)
			{
				result = new Rectangle(1, result.Y + 2, displayRectangle.Width - 2, result.Height);
				if (button.SplitButton || pa.ContainerControl is RibbonBar)
				{
					result.Y += 2;
				}
			}
			if (image == null && string.IsNullOrEmpty(button.Symbol) && (stringFormat & eTextFormat.HorizontalCenter) != eTextFormat.HorizontalCenter && !flag && !isOnMenuBar && result.X == 0 && !pa.RightToLeft)
			{
				result.X = 3;
			}
			result.Offset(displayRectangle.Left, displayRectangle.Top);
			if (button.Orientation == eOrientation.Vertical && !flag)
			{
				if (result.Bottom > displayRectangle.Bottom)
				{
					result.Height = displayRectangle.Bottom - result.Y;
				}
			}
			else if (result.Right > displayRectangle.Right)
			{
				result.Width = displayRectangle.Right - result.Left;
			}
		}
		return result;
	}

	public override void PaintButtonText(ButtonItem button, ItemPaintArgs pa, Color textColor, CompositeImage image)
	{
		if (!button.RenderText)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		eTextFormat stringFormat = GetStringFormat(button, pa, image);
		bool flag = IsOnMenu(button, pa);
		bool isOnMenuBar = pa.IsOnMenuBar;
		Rectangle displayRectangle = button.DisplayRectangle;
		Rectangle textRectangle = GetTextRectangle(button, pa, stringFormat, image);
		Font font = button.GetFont(pa, isLayout: false);
		bool rightToLeft = pa.RightToLeft;
		if (flag || button.ButtonStyle != 0 || (image == null && string.IsNullOrEmpty(button.Symbol)) || (!flag && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)))
		{
			if (button.Orientation == eOrientation.Vertical && !flag)
			{
				graphics.RotateTransform(90f);
				if (button.TextMarkupBody == null)
				{
					TextDrawing.DrawStringLegacy(graphics, ButtonItemPainter.GetDrawText(button.Text), font, textColor, new Rectangle(textRectangle.Top, -textRectangle.Right, textRectangle.Height, textRectangle.Width), stringFormat);
				}
				else
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, textColor, rightToLeft);
					markupDrawContext.HotKeyPrefixVisible = (stringFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					button.TextMarkupBody.Bounds = new Rectangle(textRectangle.Top, -textRectangle.Right, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
					button.TextMarkupBody.Render(markupDrawContext);
				}
				graphics.ResetTransform();
			}
			else
			{
				if (button.TextMarkupBody == null)
				{
					if (pa.GlassEnabled && (button.Parent is CaptionItemContainer || (button.Parent is RibbonTabItemContainer && (button.EffectiveStyle == eDotNetBarStyle.Office2010 || button.EffectiveStyle == eDotNetBarStyle.Metro))) && !(pa.ContainerControl is QatToolbar))
					{
						if (!pa.CachedPaint)
						{
							Office2007RibbonControlPainter.PaintTextOnGlass(graphics, button.Text, font, textRectangle, TextDrawing.GetTextFormat(stringFormat), textColor, copySourceBackground: true, !button.IsMouseOver && !ColorFunctions.IsEqual(textColor, Color.White), 7);
						}
					}
					else
					{
						TextDrawing.DrawString(graphics, ButtonItemPainter.GetDrawText(button.Text), font, textColor, textRectangle, stringFormat);
					}
				}
				else
				{
					MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, font, textColor, rightToLeft);
					markupDrawContext2.HotKeyPrefixVisible = (stringFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
					markupDrawContext2.ContextObject = button;
					Rectangle bounds = new Rectangle((rightToLeft && flag) ? (textRectangle.X + textRectangle.Width - button.TextMarkupBody.Bounds.Width) : textRectangle.X, textRectangle.Y + (textRectangle.Height - button.TextMarkupBody.Bounds.Height) / 2 + 1, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
					if (pa.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
					{
						bounds = new Rectangle(textRectangle.X + (textRectangle.Width - button.TextMarkupBody.Bounds.Width) / 2, textRectangle.Y, button.TextMarkupBody.Bounds.Width, button.TextMarkupBody.Bounds.Height);
					}
					else if (IsTextCentered(button, pa, image))
					{
						bounds.Offset((textRectangle.Width - bounds.Width) / 2, 0);
					}
					if (button._FixedSizeCenterText)
					{
						bounds.Y--;
					}
					button.TextMarkupBody.Bounds = bounds;
					button.TextMarkupBody.Render(markupDrawContext2);
				}
				if (!button.DesignMode && button.Focused && !flag && !isOnMenuBar && (!(pa.ContainerControl is ButtonX) || ((ButtonX)(object)pa.ContainerControl).FocusCuesEnabled))
				{
					Rectangle rectangle = displayRectangle;
					rectangle.Inflate(-2, -2);
					ControlPaint.DrawFocusRectangle(graphics, rectangle);
				}
			}
		}
		if (button.DrawShortcutText != "" && flag && !button.IsOnCustomizeDialog)
		{
			stringFormat |= eTextFormat.HidePrefix;
			stringFormat |= eTextFormat.Right;
			TextDrawing.DrawString(graphics, button.DrawShortcutText, font, textColor, textRectangle, stringFormat);
		}
	}

	protected virtual Rectangle GetTotalSubItemsRect(ButtonItem button)
	{
		return button.GetTotalSubItemsRect();
	}

	public override void PaintExpandButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		Rectangle displayRectangle = button.DisplayRectangle;
		bool isMouseOver = button.IsMouseOver;
		Color textColor = GetTextColor(button, pa);
		SolidBrush val = new SolidBrush(textColor);
		try
		{
			if ((button.SubItems.Count <= 0 && button.PopupType != ePopupType.Container && !button.ShowArrow) || !button.ShowSubItems)
			{
				return;
			}
			if (flag)
			{
				Point[] array = new Point[3];
				if (pa.RightToLeft)
				{
					array[0].X = displayRectangle.Left + StiScale2.XXI(8.0);
					array[0].Y = displayRectangle.Top + (displayRectangle.Height - StiScale2.YYI(8.0)) / 2;
					array[1].X = array[0].X;
					array[1].Y = array[0].Y + StiScale2.YYI(8.0);
					array[2].X = array[0].X - StiScale2.XXI(4.0);
					array[2].Y = array[0].Y + StiScale2.YYI(4.0);
					graphics.FillPolygon((Brush)(object)val, array);
				}
				else
				{
					array[0].X = displayRectangle.Left + displayRectangle.Width - StiScale2.XXI(12.0);
					array[0].Y = displayRectangle.Top + (displayRectangle.Height - StiScale2.YYI(8.0)) / 2;
					array[1].X = array[0].X;
					array[1].Y = array[0].Y + StiScale2.YYI(8.0);
					array[2].X = array[0].X + StiScale2.XXI(4.0);
					array[2].Y = array[0].Y + StiScale2.YYI(4.0);
					int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
					Rectangle rect = new Rectangle(displayRectangle.Right - verticalScrollBarWidth, displayRectangle.Y, verticalScrollBarWidth, displayRectangle.Height);
					StiButtonHelper.DrawRightArrow(graphics, rect, button.Enabled, pa.Colors.ItemText, pa.Colors.ItemDisabledText);
				}
			}
			else
			{
				if (button.SubItemsRect.IsEmpty)
				{
					return;
				}
				if (ButtonItemPainter.IsItemEnabled(button, pa) && (isMouseOver || button.Checked) && !button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.None && button.HotTrackingStyle != eHotTrackingStyle.Image && !button.AutoExpandOnClick)
				{
					Rectangle totalSubItemsRect = GetTotalSubItemsRect(button);
					totalSubItemsRect.Offset(displayRectangle.Location);
					Pen val2 = new Pen(isMouseOver ? pa.Colors.ItemHotBorder : pa.Colors.ItemCheckedBorder);
					try
					{
						DisplayHelp.DrawRectangle(graphics, val2, totalSubItemsRect);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				ButtonItemPainter.PaintButtonExpandIndicator(button, pa);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void PaintMenuItemSide(ButtonItem button, ItemPaintArgs pa, Rectangle sideRect)
	{
		Graphics graphics = pa.Graphics;
		Region val = graphics.Clip.Clone();
		graphics.SetClip(sideRect);
		sideRect.Inflate(0, 1);
		if (button.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !button.RecentlyUsed)
		{
			DisplayHelp.FillRectangle(graphics, sideRect, pa.Colors.MenuUnusedSide, pa.Colors.MenuUnusedSide2, pa.Colors.MenuUnusedSideGradientAngle);
		}
		else
		{
			DisplayHelp.FillRectangle(graphics, sideRect, pa.Colors.MenuSide, pa.Colors.MenuSide2, pa.Colors.MenuSideGradientAngle);
		}
		if (val != null)
		{
			graphics.Clip = val;
		}
		else
		{
			graphics.ResetClip();
		}
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		if (flag)
		{
			Rectangle sideRect = new Rectangle(button.DisplayRectangle.Left, button.DisplayRectangle.Top, button.ImageDrawRect.Right, button.DisplayRectangle.Height);
			if (pa.RightToLeft)
			{
				sideRect = new Rectangle(button.DisplayRectangle.Right - button.ImageDrawRect.Right, button.DisplayRectangle.Top, button.ImageDrawRect.Right, button.DisplayRectangle.Height);
			}
			PaintMenuItemSide(button, pa, sideRect);
		}
		else if (!pa.Colors.ItemBackground.IsEmpty)
		{
			if (pa.Colors.ItemBackground2.IsEmpty)
			{
				SolidBrush val = new SolidBrush(pa.Colors.ItemBackground);
				try
				{
					graphics.FillRectangle((Brush)(object)val, button.DisplayRectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(button.DisplayRectangle, pa.Colors.ItemBackground, pa.Colors.ItemBackground2, pa.Colors.ItemBackgroundGradientAngle);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, button.DisplayRectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		else if (!ButtonItemPainter.IsItemEnabled(button, pa) && !pa.Colors.ItemDisabledBackground.IsEmpty)
		{
			SolidBrush val3 = new SolidBrush(pa.Colors.ItemDisabledBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val3, button.DisplayRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		Rectangle displayRectangle = button.DisplayRectangle;
		if ((!ButtonItemPainter.IsItemEnabled(button, pa) && !button.DesignMode) || !button.Expanded || flag)
		{
			return;
		}
		if (pa.Colors.ItemExpandedBackground2.IsEmpty)
		{
			Rectangle displayRectangle2 = button.DisplayRectangle;
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				displayRectangle2.Width -= 2;
			}
			SolidBrush val4 = new SolidBrush(pa.Colors.ItemExpandedBackground);
			try
			{
				graphics.FillRectangle((Brush)(object)val4, displayRectangle2);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		else
		{
			LinearGradientBrush val5 = BarFunctions.CreateLinearGradientBrush(new Rectangle(displayRectangle.Left, displayRectangle.Top, displayRectangle.Width - 2, displayRectangle.Height), pa.Colors.ItemExpandedBackground, pa.Colors.ItemExpandedBackground2, pa.Colors.ItemExpandedBackgroundGradientAngle);
			Rectangle rectangle = new Rectangle(displayRectangle.Left, displayRectangle.Top, displayRectangle.Width, displayRectangle.Height);
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				rectangle.Width -= 2;
			}
			graphics.FillRectangle((Brush)(object)val5, rectangle);
			((Brush)val5).Dispose();
		}
		Point[] array = ((button.Orientation != 0 || button.PopupSide != 0) ? new Point[5] : new Point[4]);
		array[0].X = displayRectangle.Left;
		array[0].Y = displayRectangle.Top + displayRectangle.Height - 1;
		array[1].X = displayRectangle.Left;
		array[1].Y = displayRectangle.Top;
		if (button.Orientation == eOrientation.Horizontal)
		{
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				array[2].X = displayRectangle.Left + displayRectangle.Width - 3;
			}
			else
			{
				array[2].X = displayRectangle.Right - 1;
			}
		}
		else
		{
			array[2].X = displayRectangle.Left + displayRectangle.Width - 1;
		}
		array[2].Y = displayRectangle.Top;
		if (button.Orientation == eOrientation.Horizontal)
		{
			if (!pa.Colors.ItemExpandedShadow.IsEmpty)
			{
				array[3].X = displayRectangle.Left + displayRectangle.Width - 3;
			}
			else
			{
				array[3].X = displayRectangle.Right - 1;
			}
		}
		else
		{
			array[3].X = displayRectangle.Left + displayRectangle.Width - 1;
		}
		array[3].Y = displayRectangle.Top + displayRectangle.Height - 1;
		if (button.Orientation == eOrientation.Vertical || button.PopupSide != 0)
		{
			array[4].X = displayRectangle.Left;
			array[4].Y = displayRectangle.Top + displayRectangle.Height - 1;
		}
		if (!pa.Colors.ItemExpandedBorder.IsEmpty)
		{
			Pen val6 = new Pen(pa.Colors.ItemExpandedBorder, 1f);
			try
			{
				graphics.DrawLines(val6, array);
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		if (!pa.Colors.ItemExpandedShadow.IsEmpty && button.Orientation == eOrientation.Horizontal)
		{
			SolidBrush val7 = new SolidBrush(pa.Colors.ItemExpandedShadow);
			try
			{
				graphics.FillRectangle((Brush)(object)val7, displayRectangle.Left + displayRectangle.Width - 2, displayRectangle.Top + 2, 2, displayRectangle.Height - 2);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
	}
}
