using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroSwitchButtonPainter : Office2010SwitchButtonPainter
{
	public override void Paint(SwitchButtonRenderEventArgs e)
	{
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		SwitchButtonItem switchButtonItem = e.SwitchButtonItem;
		bool enabled = switchButtonItem.Enabled;
		SwitchButtonColorTable switchButtonColorTable = (enabled ? base.ColorTable.SwitchButton.Default : base.ColorTable.SwitchButton.Disabled);
		if (switchButtonColorTable == null)
		{
			switchButtonColorTable = new SwitchButtonColorTable();
		}
		Rectangle bounds = switchButtonItem.Bounds;
		Graphics graphics = e.Graphics;
		bounds.X = bounds.Right - switchButtonItem.Margin.Right - switchButtonItem.ButtonWidth;
		bounds.Width = switchButtonItem.ButtonWidth;
		bounds.Y += switchButtonItem.Margin.Top + (bounds.Height - switchButtonItem.Margin.Vertical - switchButtonItem.ButtonHeight) / 2;
		bounds.Height = switchButtonItem.ButtonHeight;
		switchButtonItem.ButtonBounds = bounds;
		bool flag = e.ItemPaintArgs != null && e.ItemPaintArgs.GlassEnabled && ((switchButtonItem.Parent is CaptionItemContainer && !(e.ItemPaintArgs.ContainerControl is QatToolbar)) || (switchButtonItem.Parent is RibbonTabItemContainer && switchButtonItem.EffectiveStyle == eDotNetBarStyle.Office2010));
		if (switchButtonItem.TextVisible && !string.IsNullOrEmpty(switchButtonItem.Text))
		{
			Rectangle bounds2 = switchButtonItem.Bounds;
			bounds2.Width -= switchButtonItem.ButtonWidth + switchButtonItem.Margin.Right;
			bounds2.Y += switchButtonItem.TextPadding.Top;
			bounds2.Height -= switchButtonItem.TextPadding.Vertical;
			bool rightToLeft = e.RightToLeft;
			Color color = ((switchButtonItem.TextColor.IsEmpty || !enabled) ? switchButtonColorTable.TextColor : switchButtonItem.TextColor);
			Font font = e.Font;
			eTextFormat eTextFormat = eTextFormat.VerticalCenter;
			if (switchButtonItem.TextMarkupBody != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, color, rightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				if ((eTextFormat & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
				{
					bounds2.Y = switchButtonItem.TopInternal + (switchButtonItem.Bounds.Height - switchButtonItem.TextMarkupBody.Bounds.Height) / 2;
				}
				else if ((eTextFormat & eTextFormat.Bottom) == eTextFormat.Bottom)
				{
					bounds2.Y += switchButtonItem.TextMarkupBody.Bounds.Height - bounds2.Height + 1;
				}
				bounds2.Height = switchButtonItem.TextMarkupBody.Bounds.Height;
				switchButtonItem.TextMarkupBody.Bounds = bounds2;
				switchButtonItem.TextMarkupBody.Render(markupDrawContext);
			}
			else if (flag)
			{
				if (!e.ItemPaintArgs.CachedPaint)
				{
					Office2007RibbonControlPainter.PaintTextOnGlass(graphics, switchButtonItem.Text, font, bounds2, TextDrawing.GetTextFormat(eTextFormat));
				}
			}
			else
			{
				TextDrawing.DrawString(graphics, switchButtonItem.Text, font, color, bounds2, eTextFormat);
			}
		}
		bool value = switchButtonItem.Value;
		string offText = switchButtonItem.OffText;
		string onText = switchButtonItem.OnText;
		Font font2 = (Font)((switchButtonItem.SwitchFont != null) ? ((object)switchButtonItem.SwitchFont) : ((object)new Font(e.Font, (FontStyle)1)));
		Color color2 = ((switchButtonItem.OffTextColor.IsEmpty || !enabled) ? switchButtonColorTable.OffTextColor : switchButtonItem.OffTextColor);
		Color color3 = ((switchButtonItem.OnTextColor.IsEmpty || !enabled) ? switchButtonColorTable.OnTextColor : switchButtonItem.OnTextColor);
		int switchWidth = switchButtonItem.SwitchWidth;
		int num = Math.Min(bounds.X + switchButtonItem.SwitchOffset, bounds.Right);
		if (value)
		{
			num = Math.Max(bounds.Right - switchWidth - switchButtonItem.SwitchOffset, bounds.X);
		}
		Color color4 = ((switchButtonItem.BorderColor.IsEmpty || !enabled) ? switchButtonColorTable.BorderColor : switchButtonItem.BorderColor);
		Color color5 = ((switchButtonItem.OffBackColor.IsEmpty || !enabled) ? switchButtonColorTable.OffBackColor : switchButtonItem.OffBackColor);
		Color color6 = ((switchButtonItem.OnBackColor.IsEmpty || !enabled) ? switchButtonColorTable.OnBackColor : switchButtonItem.OnBackColor);
		DisplayHelp.DrawRectangle(graphics, color4, bounds);
		Rectangle rectangle = bounds;
		rectangle.Inflate(-2, -2);
		GraphicsPath val = new GraphicsPath();
		val.AddRectangle(rectangle);
		Region clip = graphics.Clip;
		graphics.SetClip(val, (CombineMode)1);
		val.Dispose();
		Rectangle bounds3 = (switchButtonItem.OnPartBounds = new Rectangle(num - (bounds.Width - switchWidth), bounds.Y, bounds.Width - switchWidth + 2, bounds.Height));
		bounds3.Inflate(-2, -2);
		DisplayHelp.FillRectangle(graphics, bounds3, color6);
		if (!string.IsNullOrEmpty(onText))
		{
			if (flag && BarUtilities.UseTextRenderer)
			{
				TextDrawing.DrawStringLegacy(graphics, onText, font2, color3, bounds3, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
			}
			else
			{
				TextDrawing.DrawString(graphics, onText, font2, color3, bounds3, eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.VerticalCenter);
			}
		}
		Rectangle bounds4 = (switchButtonItem.OffPartBounds = new Rectangle(num + switchWidth - 2, bounds.Y, bounds.Width - switchWidth + 2, bounds.Height));
		bounds4.Inflate(-2, -2);
		DisplayHelp.FillRectangle(graphics, bounds4, color5);
		if (!string.IsNullOrEmpty(offText))
		{
			if (flag && BarUtilities.UseTextRenderer)
			{
				TextDrawing.DrawStringLegacy(graphics, offText, font2, color2, bounds4, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
			}
			else
			{
				TextDrawing.DrawString(graphics, offText, font2, color2, bounds4, eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.VerticalCenter);
			}
		}
		graphics.Clip = clip;
		clip.Dispose();
		Rectangle rectangle5 = (switchButtonItem.SwitchBounds = new Rectangle(num, bounds.Y, switchWidth, bounds.Height));
		Color color7 = ((switchButtonItem.SwitchBorderColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBorderColor : switchButtonItem.SwitchBorderColor);
		Color color8 = ((switchButtonItem.SwitchBackColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBackColor : switchButtonItem.SwitchBackColor);
		DisplayHelp.FillRectangle(graphics, rectangle5, color8);
		if (!color7.IsEmpty)
		{
			DisplayHelp.DrawRectangle(graphics, color7, rectangle5);
		}
		if (switchButtonItem.IsReadOnly && switchButtonItem.ShowReadOnlyMarker)
		{
			Color readOnlyMarkerColor = switchButtonItem.ReadOnlyMarkerColor;
			Rectangle rectangle6 = new Rectangle(rectangle5.X + (rectangle5.Width - 7) / 2, rectangle5.Y + (rectangle5.Height - 10) / 2, 7, 10);
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)3;
			SolidBrush val2 = new SolidBrush(readOnlyMarkerColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, new Rectangle(rectangle6.X, rectangle6.Y + 4, rectangle6.Width, rectangle6.Height - 4));
				graphics.FillRectangle(Brushes.White, new Rectangle(rectangle6.X + 3, rectangle6.Y + 5, 1, 2));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Pen val3 = new Pen(readOnlyMarkerColor, 1f);
			try
			{
				graphics.DrawLine(val3, rectangle6.X + 2, rectangle6.Y, rectangle6.X + 4, rectangle6.Y);
				graphics.DrawLine(val3, rectangle6.X + 1, rectangle6.Y + 1, rectangle6.X + 1, rectangle6.Y + 3);
				graphics.DrawLine(val3, rectangle6.X + 5, rectangle6.Y + 1, rectangle6.X + 5, rectangle6.Y + 3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
		}
	}
}
