using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2010SwitchButtonPainter : SwitchButtonPainter, IOffice2007Painter
{
	private static readonly int SwitchCornerSize = 2;

	private Office2007ColorTable _ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return _ColorTable;
		}
		set
		{
			_ColorTable = value;
		}
	}

	public override void Paint(SwitchButtonRenderEventArgs e)
	{
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Expected O, but got Unknown
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		SwitchButtonItem switchButtonItem = e.SwitchButtonItem;
		bool enabled = switchButtonItem.Enabled;
		SwitchButtonColorTable switchButtonColorTable = (enabled ? ColorTable.SwitchButton.Default : ColorTable.SwitchButton.Disabled);
		if (switchButtonColorTable == null)
		{
			switchButtonColorTable = new SwitchButtonColorTable();
		}
		Rectangle bounds = switchButtonItem.Bounds;
		bounds.X = bounds.Right - switchButtonItem.Margin.Right - switchButtonItem.ButtonWidth;
		bounds.Width = switchButtonItem.ButtonWidth;
		bounds.Y += switchButtonItem.Margin.Top + (bounds.Height - switchButtonItem.Margin.Vertical - switchButtonItem.ButtonHeight) / 2;
		bounds.Height = switchButtonItem.ButtonHeight;
		switchButtonItem.ButtonBounds = bounds;
		bool flag = e.ItemPaintArgs != null && e.ItemPaintArgs.GlassEnabled && ((switchButtonItem.Parent is CaptionItemContainer && !(e.ItemPaintArgs.ContainerControl is QatToolbar)) || (switchButtonItem.Parent is RibbonTabItemContainer && switchButtonItem.EffectiveStyle == eDotNetBarStyle.Office2010));
		Graphics graphics = e.Graphics;
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
		Rectangle r = bounds;
		r.Inflate(-1, -1);
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, SwitchCornerSize - 1);
		Region clip = graphics.Clip;
		graphics.SetClip(roundedRectanglePath, (CombineMode)1);
		roundedRectanglePath.Dispose();
		Rectangle bounds3 = (switchButtonItem.OnPartBounds = new Rectangle(num - (bounds.Width - switchWidth), bounds.Y, bounds.Width - switchWidth + 2, bounds.Height));
		DisplayHelp.FillRoundedRectangle(graphics, bounds3, SwitchCornerSize - 1, color6);
		DisplayHelp.FillRoundedRectangle(graphics, bounds3, SwitchCornerSize - 1, Color.FromArgb((color6.GetBrightness() > 0.9f) ? 8 : 24, Color.Black), Color.FromArgb(32, Color.White));
		if (flag && BarUtilities.UseTextRenderer)
		{
			TextDrawing.DrawStringLegacy(graphics, onText, font2, color3, bounds3, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		else
		{
			TextDrawing.DrawString(graphics, onText, font2, color3, bounds3, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		Rectangle bounds4 = (switchButtonItem.OffPartBounds = new Rectangle(num + switchWidth - 2, bounds.Y, bounds.Width - switchWidth + 2, bounds.Height));
		DisplayHelp.FillRoundedRectangle(graphics, bounds4, SwitchCornerSize, color5);
		DisplayHelp.FillRoundedRectangle(graphics, bounds4, SwitchCornerSize, Color.FromArgb((color5.GetBrightness() > 0.9f) ? 12 : 32, Color.Black), Color.FromArgb(64, Color.White));
		if (flag && BarUtilities.UseTextRenderer)
		{
			TextDrawing.DrawStringLegacy(graphics, offText, font2, color2, bounds4, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		else
		{
			TextDrawing.DrawString(graphics, offText, font2, color2, bounds4, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
		graphics.Clip = clip;
		clip.Dispose();
		DisplayHelp.DrawRoundedRectangle(graphics, color4, bounds, SwitchCornerSize);
		LinearGradientColorTable linearGradientColorTable = new LinearGradientColorTable(Color.FromArgb(96, Color.DarkGray), Color.Transparent, 90);
		Rectangle rectangle3 = bounds;
		rectangle3.Inflate(-SwitchCornerSize / 2, -1);
		DisplayHelp.DrawLine(graphics, rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Y, Color.FromArgb(24, Color.Black), 1);
		rectangle3.Y++;
		DisplayHelp.DrawLine(graphics, rectangle3.X, rectangle3.Y, rectangle3.Right, rectangle3.Y, Color.FromArgb(8, Color.Black), 1);
		Rectangle bounds5 = (switchButtonItem.SwitchBounds = new Rectangle(num, bounds.Y, switchWidth, bounds.Height));
		Color color7 = ((switchButtonItem.SwitchBorderColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBorderColor : switchButtonItem.SwitchBorderColor);
		Color fillColor = ((switchButtonItem.SwitchBackColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBackColor : switchButtonItem.SwitchBackColor);
		if (bounds5.Right + 2 < bounds.Right)
		{
			DisplayHelp.DrawLine(graphics, bounds5.Right, bounds5.Y + 3, bounds5.Right, bounds5.Bottom - 2, Color.FromArgb(24, Color.Black), 1);
			DisplayHelp.DrawLine(graphics, bounds5.Right + 1, bounds5.Y + 3, bounds5.Right + 1, bounds5.Bottom - 2, Color.FromArgb(8, Color.Black), 1);
		}
		DisplayHelp.DrawRoundedRectangle(graphics, color7, fillColor, bounds5, SwitchCornerSize);
		DisplayHelp.FillRoundedRectangle(graphics, bounds5, SwitchCornerSize, Color.FromArgb((fillColor.GetBrightness() > 0.9f) ? 42 : 72, Color.Black), Color.FromArgb(64, Color.White));
		DisplayHelp.DrawLine(graphics, bounds5.X + 2, bounds5.Y + 1, bounds5.Right - 3, bounds5.Y + 1, Color.FromArgb(164, Color.White), 1);
		if (switchButtonItem.IsReadOnly && switchButtonItem.ShowReadOnlyMarker)
		{
			Color readOnlyMarkerColor = switchButtonItem.ReadOnlyMarkerColor;
			Rectangle rectangle5 = new Rectangle(bounds5.X + (bounds5.Width - 7) / 2, bounds5.Y + (bounds5.Height - 10) / 2, 7, 10);
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)3;
			SolidBrush val = new SolidBrush(readOnlyMarkerColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, new Rectangle(rectangle5.X, rectangle5.Y + 4, rectangle5.Width, rectangle5.Height - 4));
				graphics.FillRectangle(Brushes.White, new Rectangle(rectangle5.X + 3, rectangle5.Y + 5, 1, 2));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Pen val2 = new Pen(readOnlyMarkerColor, 1f);
			try
			{
				graphics.DrawLine(val2, rectangle5.X + 2, rectangle5.Y, rectangle5.X + 4, rectangle5.Y);
				graphics.DrawLine(val2, rectangle5.X + 1, rectangle5.Y + 1, rectangle5.X + 1, rectangle5.Y + 3);
				graphics.DrawLine(val2, rectangle5.X + 5, rectangle5.Y + 1, rectangle5.X + 5, rectangle5.Y + 3);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
		}
	}
}
