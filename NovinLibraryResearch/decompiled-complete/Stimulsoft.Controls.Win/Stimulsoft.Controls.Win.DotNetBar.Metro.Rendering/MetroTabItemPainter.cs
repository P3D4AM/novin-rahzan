using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroTabItemPainter : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		MetroTabItem metroTabItem = (MetroTabItem)renderingInfo.Control;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroTabItemColorTable metroTabItem2 = renderingInfo.ColorTable.MetroTab.MetroTabItem;
		MetroTabItemStateColorTable metroTabItemStateColorTable = metroTabItem2.Default;
		if (!metroTabItem.Enabled)
		{
			metroTabItemStateColorTable = metroTabItem2.Disabled;
		}
		else if (metroTabItem.Checked)
		{
			metroTabItemStateColorTable = metroTabItem2.Selected;
		}
		else if (metroTabItem.IsMouseDown && metroTabItem2.Pressed != null)
		{
			metroTabItemStateColorTable = metroTabItem2.Pressed;
		}
		else if (metroTabItem.IsMouseOver && metroTabItem2.MouseOver != null)
		{
			metroTabItemStateColorTable = metroTabItem2.MouseOver;
		}
		Rectangle bounds = metroTabItem.Bounds;
		Rectangle rectangle = metroTabItem.TextRenderBounds;
		Rectangle rectangle2 = metroTabItem.ImageRenderBounds;
		CompositeImage image = metroTabItem.GetImage();
		if (metroTabItemStateColorTable.Background != null)
		{
			Font defaultFont = renderingInfo.DefaultFont;
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(metroTabItemStateColorTable.Background, graphics, bounds);
			ElementStyleDisplay.Paint(elementStyleDisplayInfo);
			if (image != null && metroTabItem.ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				if (rectangle2.IsEmpty)
				{
					rectangle2 = GetImageRectangle(metroTabItem, image);
				}
				if (rectangle.IsEmpty)
				{
					rectangle = GetTextRectangle(metroTabItem, image, rectangle2);
				}
			}
			else if (rectangle.IsEmpty)
			{
				rectangle = bounds;
			}
			if (metroTabItem.TextMarkupBody == null)
			{
				elementStyleDisplayInfo.Bounds = rectangle;
				ElementStyleDisplay.PaintText(elementStyleDisplayInfo, metroTabItem.Text, defaultFont);
			}
			else
			{
				eTextFormat eTextFormat = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, defaultFont, metroTabItemStateColorTable.Background.TextColor, renderingInfo.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				markupDrawContext.ContextObject = metroTabItem;
				Rectangle bounds2 = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - metroTabItem.TextMarkupBody.Bounds.Height) / 2 + 1, metroTabItem.TextMarkupBody.Bounds.Width, metroTabItem.TextMarkupBody.Bounds.Height);
				if ((eTextFormat & eTextFormat.HorizontalCenter) != 0)
				{
					bounds2.Offset((rectangle.Width - bounds2.Width) / 2, 0);
				}
				if (metroTabItem._FixedSizeCenterText)
				{
					bounds2.Y--;
				}
				metroTabItem.TextMarkupBody.Bounds = bounds2;
				metroTabItem.TextMarkupBody.Render(markupDrawContext);
			}
			metroTabItem.TextRenderBounds = rectangle;
			metroTabItem.ImageRenderBounds = rectangle2;
		}
		if (image != null)
		{
			if (!metroTabItem.IsMouseOver && metroTabItem.HotTrackingStyle == eHotTrackingStyle.Color)
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
				image.DrawImage(graphics, rectangle2, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val);
			}
			else
			{
				image.DrawImage(graphics, rectangle2);
			}
		}
	}

	private Rectangle GetImageRectangle(MetroTabItem tab, CompositeImage image)
	{
		Rectangle result = Rectangle.Empty;
		if (image != null)
		{
			Size imageSize = tab.ImageSize;
			if (tab.ImagePosition == eImagePosition.Top || tab.ImagePosition == eImagePosition.Bottom)
			{
				result = new Rectangle(tab.ImageDrawRect.X, tab.ImageDrawRect.Y, tab.DisplayRectangle.Width, tab.ImageDrawRect.Height);
			}
			else if (tab.ImagePosition == eImagePosition.Left)
			{
				result = new Rectangle(tab.ImageDrawRect.X + 4, tab.ImageDrawRect.Y, tab.ImageDrawRect.Width, tab.ImageDrawRect.Height);
			}
			else if (tab.ImagePosition == eImagePosition.Right)
			{
				result = new Rectangle(tab.ImageDrawRect.X + tab.ImagePaddingHorizontal + 4, tab.ImageDrawRect.Y, tab.ImageDrawRect.Width, tab.ImageDrawRect.Height);
			}
			result.Offset(tab.DisplayRectangle.Left, tab.DisplayRectangle.Top);
			result.Offset((result.Width - imageSize.Width) / 2, (result.Height - imageSize.Height) / 2);
			result.Width = imageSize.Width;
			result.Height = imageSize.Height;
		}
		return result;
	}

	private Rectangle GetTextRectangle(MetroTabItem tab, CompositeImage image, Rectangle imageBounds)
	{
		Rectangle displayRectangle = tab.DisplayRectangle;
		Rectangle result = tab.TextDrawRect;
		if (tab.ImagePosition == eImagePosition.Top || tab.ImagePosition == eImagePosition.Bottom)
		{
			result = new Rectangle(1, result.Y, displayRectangle.Width - 2, result.Height);
		}
		result.Offset(displayRectangle.Left, displayRectangle.Top);
		if (tab.ImagePosition == eImagePosition.Left)
		{
			result.X = imageBounds.Right + tab.ImagePaddingHorizontal;
		}
		if (result.Right > displayRectangle.Right)
		{
			result.Width = displayRectangle.Right - result.Left;
		}
		return result;
	}
}
