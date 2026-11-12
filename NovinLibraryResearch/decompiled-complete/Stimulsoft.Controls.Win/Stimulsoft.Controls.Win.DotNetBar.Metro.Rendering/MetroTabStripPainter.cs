using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroTabStripPainter : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Invalid comparison between Unknown and I4
		MetroTabStrip metroTabStrip = (MetroTabStrip)renderingInfo.Control;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroTabStripColorTable tabStrip = renderingInfo.ColorTable.MetroTab.TabStrip;
		Rectangle clientRectangle = ((Control)metroTabStrip).ClientRectangle;
		if (tabStrip.BackgroundStyle != null)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(tabStrip.BackgroundStyle, graphics, clientRectangle);
			ElementStyleDisplay.PaintBackground(e);
		}
		if (!metroTabStrip.CaptionVisible)
		{
			return;
		}
		if (metroTabStrip.CaptionBounds.IsEmpty || metroTabStrip.SystemCaptionItemBounds.IsEmpty || metroTabStrip.QuickToolbarBounds.IsEmpty)
		{
			SetQatAndCaptionItemBounds(metroTabStrip, renderingInfo);
		}
		Color color = renderingInfo.ColorTable.MetroTab.ActiveCaptionText;
		eTextFormat captionTextFormat = renderingInfo.ColorTable.MetroTab.CaptionTextFormat;
		Form val = ((Control)metroTabStrip).FindForm();
		bool flag = true;
		if (val != null && ((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)))
		{
			color = renderingInfo.ColorTable.MetroTab.InactiveCaptionText;
			flag = false;
		}
		Font val2 = SystemFonts.DefaultFont;
		bool flag2 = true;
		if (metroTabStrip.CaptionFont != null)
		{
			val2.Dispose();
			val2 = metroTabStrip.CaptionFont;
			flag2 = false;
		}
		string text = metroTabStrip.TitleText;
		if (string.IsNullOrEmpty(text) && val != null)
		{
			text = ((Control)val).Text;
		}
		bool flag3 = metroTabStrip.TitleTextMarkupBody != null;
		Rectangle captionBounds = metroTabStrip.CaptionBounds;
		captionBounds.X += 12;
		captionBounds.Width -= 12;
		if (!flag3)
		{
			TextDrawing.DrawString(graphics, text, val2, color, captionBounds, captionTextFormat);
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val2, color, (int)((Control)metroTabStrip).RightToLeft == 1, captionBounds, hotKeyPrefixVisible: false);
			markupDrawContext.AllowMultiLine = false;
			markupDrawContext.IgnoreFormattingColors = !flag;
			BodyElement titleTextMarkupBody = metroTabStrip.TitleTextMarkupBody;
			if (metroTabStrip.TitleTextMarkupLastArrangeBounds != captionBounds)
			{
				titleTextMarkupBody.Measure(captionBounds.Size, markupDrawContext);
				titleTextMarkupBody.Arrange(captionBounds, markupDrawContext);
				metroTabStrip.TitleTextMarkupLastArrangeBounds = captionBounds;
				Rectangle bounds = titleTextMarkupBody.Bounds;
				if (bounds.Width < captionBounds.Width)
				{
					bounds.Offset((captionBounds.Width - bounds.Width) / 2, 0);
				}
				if (bounds.Height < captionBounds.Height)
				{
					bounds.Offset(0, (captionBounds.Height - bounds.Height) / 2);
				}
				titleTextMarkupBody.Bounds = bounds;
			}
			Region clip = graphics.Clip;
			graphics.SetClip(captionBounds, (CombineMode)1);
			titleTextMarkupBody.Render(markupDrawContext);
			graphics.Clip = clip;
			if (clip != null)
			{
				clip.Dispose();
			}
		}
		if (flag2)
		{
			val2.Dispose();
		}
	}

	private void SetQatAndCaptionItemBounds(MetroTabStrip strip, MetroRendererInfo renderingInfo)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		if (!strip.CaptionVisible)
		{
			return;
		}
		bool flag = (int)((Control)strip).RightToLeft == 1;
		Form val = ((Control)strip).FindForm();
		bool flag2 = false;
		if (val != null)
		{
			flag2 = (int)val.WindowState == 2;
		}
		int num = 0;
		int num2 = 0;
		Size empty = Size.Empty;
		for (int num3 = strip.QuickToolbarItems.Count - 1; num3 >= 0; num3--)
		{
			BaseItem baseItem = strip.QuickToolbarItems[num3];
			if (baseItem.Visible && baseItem.Displayed)
			{
				if (baseItem is QatCustomizeItem)
				{
					empty = baseItem.DisplayRectangle.Size;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Near && baseItem.Visible && num3 > 0)
				{
					num = ((!flag) ? baseItem.DisplayRectangle.Right : baseItem.DisplayRectangle.X);
					break;
				}
				if (baseItem.ItemAlignment == eItemAlignment.Far && baseItem.Visible)
				{
					num2 = ((!flag) ? baseItem.DisplayRectangle.X : baseItem.DisplayRectangle.Right);
				}
			}
		}
		if (strip.CaptionContainerItem is CaptionItemContainer && ((CaptionItemContainer)strip.CaptionContainerItem).MoreItems != null)
		{
			num = ((!flag) ? ((CaptionItemContainer)strip.CaptionContainerItem).MoreItems.DisplayRectangle.Right : ((CaptionItemContainer)strip.CaptionContainerItem).MoreItems.DisplayRectangle.X);
			empty = ((CaptionItemContainer)strip.CaptionContainerItem).MoreItems.DisplayRectangle.Size;
		}
		Rectangle quickToolbarBounds = (strip.CaptionBounds = new Rectangle(num, 2, strip.CaptionContainerItem.WidthInternal - num - (strip.CaptionContainerItem.WidthInternal - num2), strip.GetTotalCaptionHeight()));
		if (num2 > 0)
		{
			if (flag)
			{
				strip.SystemCaptionItemBounds = new Rectangle(quickToolbarBounds.X, quickToolbarBounds.Y, num2, quickToolbarBounds.Height);
			}
			else
			{
				strip.SystemCaptionItemBounds = new Rectangle(num2, quickToolbarBounds.Y, strip.CaptionContainerItem.WidthInternal - num2, quickToolbarBounds.Height);
			}
		}
		if (num == 0 || quickToolbarBounds.Height <= 0 || quickToolbarBounds.Width <= 0)
		{
			return;
		}
		BaseItem applicationButton = strip.GetApplicationButton();
		if (applicationButton != null)
		{
			int num4 = strip.QuickToolbarItems.IndexOf(applicationButton);
			if (strip.QuickToolbarItems.Count - 1 > num4)
			{
				BaseItem baseItem2 = strip.QuickToolbarItems[num4 + 1];
				if (flag)
				{
					quickToolbarBounds.Width -= quickToolbarBounds.Right - baseItem2.DisplayRectangle.Right;
				}
				else
				{
					quickToolbarBounds.Width -= baseItem2.DisplayRectangle.X - quickToolbarBounds.X;
					quickToolbarBounds.X = baseItem2.DisplayRectangle.X;
				}
			}
		}
		quickToolbarBounds.Height = ((CaptionItemContainer)strip.CaptionContainerItem).MaxItemHeight + 6;
		quickToolbarBounds.X = 0;
		quickToolbarBounds.Width = num;
		strip.QuickToolbarBounds = quickToolbarBounds;
	}
}
