using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Metro.Rendering;

internal class _2235 : ᯊ
{
	private static RoundRectangleShapeDescriptor _ᯐ = new RoundRectangleShapeDescriptor(0);

	private static RoundRectangleShapeDescriptor __2236 = new RoundRectangleShapeDescriptor(2);

	protected override IShapeDescriptor GetButtonShape(ButtonItem button, ItemPaintArgs pa)
	{
		IShapeDescriptor result = ᒜ(button);
		if (pa.ContainerControl is ButtonX)
		{
			result = ((ButtonX)(object)pa.ContainerControl).ᒜ();
		}
		else if (pa.ContainerControl is NavigationBar)
		{
			result = ((NavigationBar)(object)pa.ContainerControl).ᯙ;
		}
		return result;
	}

	private static IShapeDescriptor ᒜ(ButtonItem ᤉ)
	{
		if (ᤉ.Shape != null)
		{
			return ᤉ.Shape;
		}
		if (StyleManager.Style == eStyle.OfficeMobile2014)
		{
			return __2236;
		}
		return _ᯐ;
	}

	public override Office2007ButtonItemColorTable GetColorTable(ButtonItem button, _21F0 buttonCont)
	{
		switch (buttonCont)
		{
		case _21F0._20AE:
		{
			Office2007ColorTable office2007ColorTable2 = base.ᘽ;
			object value2 = null;
			if (office2007ColorTable2.ContextualTables.TryGetValue(Office2007ColorTable.GetContextualKey(ᯊ.ᯑ, "MetroTabStrip"), out value2))
			{
				return (Office2007ButtonItemColorTable)value2;
			}
			break;
		}
		case _21F0._21F7:
		{
			Office2007ColorTable office2007ColorTable = base.ᘽ;
			object value = null;
			if (office2007ColorTable.ContextualTables.TryGetValue(Office2007ColorTable.GetContextualKey(ᯊ.ᯑ, "NavigationBar"), out value))
			{
				return (Office2007ButtonItemColorTable)value;
			}
			break;
		}
		}
		return base.GetColorTable(button, buttonCont);
	}
}
internal class _250E : _250F
{
	public override void Paint(SwitchButtonRenderEventArgs e)
	{
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		SwitchButtonItem switchButtonItem = e.SwitchButtonItem;
		bool enabled = switchButtonItem.Enabled;
		SwitchButtonColorTable switchButtonColorTable = (enabled ? base.ᘽ.SwitchButton.Default : base.ᘽ.SwitchButton.Disabled);
		if (switchButtonColorTable == null)
		{
			switchButtonColorTable = new SwitchButtonColorTable();
		}
		Rectangle bounds = switchButtonItem.Bounds;
		Graphics graphics = e.Graphics;
		int num = Dpi.Width(switchButtonItem.ButtonWidth);
		Padding padding = Dpi.Size(switchButtonItem.Margin);
		if (e.က != null && e.က.ContainerControl is DevComponents.AdvTree.AdvTree)
		{
			if (switchButtonItem.ItemAlignment == eItemAlignment.Far)
			{
				bounds.X = bounds.Right - padding.Right - num;
			}
			else if (switchButtonItem.ItemAlignment == eItemAlignment.Center)
			{
				bounds.X += (bounds.Width - num) / 2;
			}
		}
		else
		{
			bounds.X = bounds.Right - padding.Right - num;
		}
		bounds.Width = num;
		int num2 = Dpi.Height(switchButtonItem.ButtonHeight);
		bounds.Y += padding.Top + (bounds.Height - padding.Vertical - num2) / 2;
		bounds.Height = num2;
		switchButtonItem.ButtonBounds = bounds;
		bool flag = e.က != null && e.က.GlassEnabled && ((switchButtonItem.Parent is ᱭ && !(e.က.ContainerControl is Ꮮ)) || (switchButtonItem.Parent is ය && switchButtonItem.EffectiveStyle == eDotNetBarStyle.Office2010));
		if (switchButtonItem.TextVisible && !string.IsNullOrEmpty(switchButtonItem.Text))
		{
			Rectangle bounds2 = switchButtonItem.Bounds;
			Padding padding2 = Dpi.Size(switchButtonItem.TextPadding);
			bounds2.Width -= num + padding.Right + padding2.Horizontal;
			bounds2.Y += padding2.Top;
			bounds2.X += padding2.Left;
			bounds2.Height -= padding2.Vertical;
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
				if (!e.က.ໟ)
				{
					_21F8._21FC(graphics, switchButtonItem.Text, font, bounds2, TextDrawing.GetTextFormat(eTextFormat));
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
		int num3 = Dpi.Width(switchButtonItem.SwitchWidth);
		int num4 = Math.Min(bounds.X + switchButtonItem.ᢪ, bounds.Right);
		if (value)
		{
			num4 = Math.Max(bounds.Right - num3 - switchButtonItem.ᢪ, bounds.X);
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
		Rectangle bounds3 = (switchButtonItem.OnPartBounds = new Rectangle(num4 - (bounds.Width - num3), bounds.Y, bounds.Width - num3 + 2, bounds.Height));
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
		Rectangle bounds4 = (switchButtonItem.OffPartBounds = new Rectangle(num4 + num3 - 2, bounds.Y, bounds.Width - num3 + 2, bounds.Height));
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
		Rectangle rectangle5 = (switchButtonItem.SwitchBounds = new Rectangle(num4, bounds.Y, num3, bounds.Height));
		Color color7 = ((switchButtonItem.SwitchBorderColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBorderColor : switchButtonItem.SwitchBorderColor);
		Color color8 = ((switchButtonItem.SwitchBackColor.IsEmpty || !enabled) ? switchButtonColorTable.SwitchBackColor : switchButtonItem.SwitchBackColor);
		if (!switchButtonColorTable.SwitchOnBackColor.IsEmpty && switchButtonItem.Value && switchButtonItem.SwitchBackColor.IsEmpty)
		{
			color8 = switchButtonColorTable.SwitchOnBackColor;
		}
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
internal static class _252F
{
	public static void _२(Graphics _٥, RectangleF ശ, Thickness _2530, BorderColors ዾ)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Expected O, but got Unknown
		if (ዾ.IsEmpty || _2530.IsZero)
		{
			return;
		}
		ശ.Width -= (float)_2530.Right;
		ശ.Height -= (float)_2530.Bottom;
		if (_2530.Right > 1.0)
		{
			ശ.Width += 1f;
		}
		if (_2530.Bottom > 1.0)
		{
			ശ.Height += 1f;
		}
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		_٥.SmoothingMode = (SmoothingMode)3;
		if (_2530.Left > 0.0 && !ዾ.Left.IsEmpty)
		{
			Pen val = new Pen(ዾ.Left, (float)_2530.Left);
			try
			{
				val.Alignment = (PenAlignment)1;
				_٥.DrawLine(val, ശ.X, ശ.Y, ശ.X, ശ.Bottom);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (_2530.Top > 0.0 && !ዾ.Top.IsEmpty)
		{
			Pen val2 = new Pen(ዾ.Top, (float)_2530.Top);
			try
			{
				val2.Alignment = (PenAlignment)1;
				_٥.DrawLine(val2, ശ.X, ശ.Y, ശ.Right, ശ.Y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (_2530.Right > 0.0 && !ዾ.Right.IsEmpty)
		{
			Pen val3 = new Pen(ዾ.Right, (float)_2530.Right);
			try
			{
				val3.Alignment = (PenAlignment)1;
				_٥.DrawLine(val3, ശ.Right, ശ.Y, ശ.Right, ശ.Bottom + (float)_2530.Bottom / 2f);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (_2530.Bottom > 0.0 && !ዾ.Bottom.IsEmpty)
		{
			Pen val4 = new Pen(ዾ.Bottom, (float)_2530.Bottom);
			try
			{
				val4.Alignment = (PenAlignment)1;
				_٥.DrawLine(val4, ശ.X, ശ.Bottom, ശ.Right, ശ.Bottom);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		_٥.SmoothingMode = smoothingMode;
	}

	public static void _0EE5(Graphics _٥, RectangleF ശ, string _065B)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (string.IsNullOrEmpty(_065B))
		{
			return;
		}
		SolidBrush val = new SolidBrush(ColorScheme.GetColor(_065B));
		try
		{
			_٥.FillRectangle((Brush)(object)val, ശ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static RectangleF _2531(RectangleF ശ, Thickness _2530)
	{
		if (_2530.IsZero)
		{
			return ശ;
		}
		ശ.X += (float)_2530.Left;
		ശ.Width -= (float)(_2530.Left + _2530.Right);
		ശ.Y += (float)_2530.Top;
		ശ.Height -= (float)(_2530.Top + _2530.Bottom);
		return ശ;
	}
}
internal class _285B : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
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
		Rectangle rectangle = metroTabItem._136B;
		Rectangle rectangle2 = metroTabItem._136A;
		ᇎ ᇎ = metroTabItem._0EC5();
		if (metroTabItemStateColorTable.Background != null)
		{
			Font val = renderingInfo.DefaultFont;
			if (!metroTabItem.Checked && val.Bold && renderingInfo.DefaultPlainFont != null)
			{
				val = renderingInfo.DefaultPlainFont;
			}
			ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo(metroTabItemStateColorTable.Background, graphics, bounds);
			ElementStyleDisplay.Paint(elementStyleDisplayInfo);
			if (ᇎ != null && metroTabItem.ButtonStyle != eButtonStyle.TextOnlyAlways)
			{
				if (rectangle2.IsEmpty)
				{
					rectangle2 = _2512(metroTabItem, ᇎ);
				}
				if (rectangle.IsEmpty)
				{
					rectangle = ഷ(metroTabItem, ᇎ, rectangle2);
				}
			}
			else if (rectangle.IsEmpty)
			{
				rectangle = bounds;
			}
			if (metroTabItem.TextMarkupBody == null)
			{
				elementStyleDisplayInfo.Bounds = rectangle;
				ElementStyleDisplay.PaintText(elementStyleDisplayInfo, metroTabItem.Text, val);
			}
			else
			{
				eTextFormat eTextFormat = eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val, metroTabItemStateColorTable.Background.TextColor, renderingInfo.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				markupDrawContext.ContextObject = metroTabItem;
				Rectangle bounds2 = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - metroTabItem.TextMarkupBody.Bounds.Height) / 2 + 1, metroTabItem.TextMarkupBody.Bounds.Width, metroTabItem.TextMarkupBody.Bounds.Height);
				if ((eTextFormat & eTextFormat.HorizontalCenter) != 0)
				{
					bounds2.Offset((rectangle.Width - bounds2.Width) / 2, 0);
				}
				if (metroTabItem._ບ)
				{
					bounds2.Y--;
				}
				metroTabItem.TextMarkupBody.Bounds = bounds2;
				metroTabItem.TextMarkupBody.Render(markupDrawContext);
			}
			metroTabItem._136B = rectangle;
			metroTabItem._136A = rectangle2;
		}
		if (ᇎ != null)
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
				ImageAttributes val2 = new ImageAttributes();
				val2.SetColorMatrix(colorMatrix);
				ᇎ.ᇗ(graphics, rectangle2, 0, 0, ᇎ.ᇕ, ᇎ.ᇔ, (GraphicsUnit)2, val2);
			}
			else
			{
				ᇎ.ᇗ(graphics, rectangle2);
			}
		}
	}

	private Rectangle _2512(MetroTabItem ᆰ, ᇎ ແ)
	{
		Rectangle result = Rectangle.Empty;
		if (ແ != null)
		{
			Size imageSize = ᆰ.ImageSize;
			if (ᆰ.ImagePosition == eImagePosition.Top || ᆰ.ImagePosition == eImagePosition.Bottom)
			{
				result = new Rectangle(ᆰ._0E60.X, ᆰ._0E60.Y, ᆰ.DisplayRectangle.Width, ᆰ._0E60.Height);
			}
			else if (ᆰ.ImagePosition == eImagePosition.Left)
			{
				result = new Rectangle(ᆰ._0E60.X + 4, ᆰ._0E60.Y, ᆰ._0E60.Width, ᆰ._0E60.Height);
			}
			else if (ᆰ.ImagePosition == eImagePosition.Right)
			{
				result = new Rectangle(ᆰ._0E60.X + ᆰ.ImagePaddingHorizontal + 4, ᆰ._0E60.Y, ᆰ._0E60.Width, ᆰ._0E60.Height);
			}
			result.Offset(ᆰ.DisplayRectangle.Left, ᆰ.DisplayRectangle.Top);
			result.Offset((result.Width - imageSize.Width) / 2, (result.Height - imageSize.Height) / 2);
			result.Width = imageSize.Width;
			result.Height = imageSize.Height;
		}
		return result;
	}

	private Rectangle ഷ(MetroTabItem ᆰ, ᇎ ແ, Rectangle _2513)
	{
		Rectangle displayRectangle = ᆰ.DisplayRectangle;
		Rectangle result = ᆰ.TextDrawRect;
		if (ᆰ.ImagePosition == eImagePosition.Top || ᆰ.ImagePosition == eImagePosition.Bottom)
		{
			result = new Rectangle(1, result.Y, displayRectangle.Width - 2, result.Height);
		}
		result.Offset(displayRectangle.Left, displayRectangle.Top);
		if (ᆰ.ImagePosition == eImagePosition.Left)
		{
			result.X = _2513.Right + ᆰ.ImagePaddingHorizontal;
		}
		if (result.Right > displayRectangle.Right)
		{
			result.Width = displayRectangle.Right - result.Left;
		}
		return result;
	}
}
internal class _285C : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Invalid comparison between Unknown and I4
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
		if (metroTabStrip._0D65.IsEmpty || metroTabStrip._൦.IsEmpty || metroTabStrip.QuickToolbarBounds.IsEmpty)
		{
			_285D(metroTabStrip, renderingInfo);
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
		bool flag3 = metroTabStrip._0D62 != null;
		Rectangle rectangle = metroTabStrip._0D65;
		rectangle.X += 12;
		rectangle.Width -= 12;
		if (StyleManager.Style == eStyle.OfficeMobile2014)
		{
			rectangle.Y -= 2;
			rectangle.Height -= 2;
			if (!metroTabStrip._൦.IsEmpty && rectangle.Width > metroTabStrip._൦.Width)
			{
				rectangle.X += metroTabStrip._൦.Width / 2 + 18;
				rectangle.Width -= metroTabStrip._൦.Width / 2 + 18;
			}
			if (!metroTabStrip.QuickToolbarBounds.IsEmpty && rectangle.Width > metroTabStrip.QuickToolbarBounds.Width)
			{
				rectangle.X -= metroTabStrip.QuickToolbarBounds.Width / 2;
				rectangle.Width += metroTabStrip.QuickToolbarBounds.Width / 2;
			}
		}
		if (!flag3)
		{
			TextDrawing.DrawString(graphics, text, val2, color, rectangle, captionTextFormat);
		}
		else
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val2, color, (int)((Control)metroTabStrip).RightToLeft == 1, rectangle, hotKeyPrefixVisible: false);
			markupDrawContext.AllowMultiLine = false;
			markupDrawContext.IgnoreFormattingColors = !flag;
			_0CB4 obj = metroTabStrip._0D62;
			if (metroTabStrip.අ != rectangle)
			{
				obj.Measure(rectangle.Size, markupDrawContext);
				obj._0CC6(rectangle, markupDrawContext);
				metroTabStrip.අ = rectangle;
				Rectangle bounds = obj.Bounds;
				if (bounds.Width < rectangle.Width)
				{
					bounds.Offset((rectangle.Width - bounds.Width) / 2, 0);
				}
				if (bounds.Height < rectangle.Height)
				{
					bounds.Offset(0, (rectangle.Height - bounds.Height) / 2);
				}
				obj.Bounds = bounds;
			}
			Region clip = graphics.Clip;
			graphics.SetClip(rectangle, (CombineMode)1);
			obj.Render(markupDrawContext);
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

	private void _285D(MetroTabStrip _251B, MetroRendererInfo _251C)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!_251B.CaptionVisible)
		{
			return;
		}
		bool flag = (int)((Control)_251B).RightToLeft == 1;
		Form val = ((Control)_251B).FindForm();
		if (val != null)
		{
			_ = val.WindowState;
		}
		int num = 0;
		int num2 = 0;
		_ = Size.Empty;
		for (int num3 = _251B.QuickToolbarItems.Count - 1; num3 >= 0; num3--)
		{
			BaseItem baseItem = _251B.QuickToolbarItems[num3];
			if (baseItem.Visible && baseItem.Displayed)
			{
				if (baseItem is QatCustomizeItem)
				{
					_ = baseItem.DisplayRectangle.Size;
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
		if (_251B.CaptionContainerItem is ᱭ && ((ᱭ)_251B.CaptionContainerItem)._0DBC != null)
		{
			num = ((!flag) ? ((ᱭ)_251B.CaptionContainerItem)._0DBC.DisplayRectangle.Right : ((ᱭ)_251B.CaptionContainerItem)._0DBC.DisplayRectangle.X);
			_ = ((ᱭ)_251B.CaptionContainerItem)._0DBC.DisplayRectangle.Size;
		}
		Rectangle quickToolbarBounds = (_251B._0D65 = new Rectangle(num, 2, _251B.CaptionContainerItem.WidthInternal - num - (_251B.CaptionContainerItem.WidthInternal - num2), _251B.ඛ()));
		if (num2 > 0)
		{
			if (flag)
			{
				_251B._൦ = new Rectangle(quickToolbarBounds.X, quickToolbarBounds.Y, num2, quickToolbarBounds.Height);
			}
			else
			{
				_251B._൦ = new Rectangle(num2, quickToolbarBounds.Y, _251B.CaptionContainerItem.WidthInternal - num2, quickToolbarBounds.Height);
			}
		}
		if (num == 0 || quickToolbarBounds.Height <= 0 || quickToolbarBounds.Width <= 0)
		{
			return;
		}
		BaseItem baseItem2 = _251B.ඩ();
		if (baseItem2 != null)
		{
			int num4 = _251B.QuickToolbarItems.IndexOf(baseItem2);
			if (_251B.QuickToolbarItems.Count - 1 > num4)
			{
				BaseItem baseItem3 = _251B.QuickToolbarItems[num4 + 1];
				if (flag)
				{
					quickToolbarBounds.Width -= quickToolbarBounds.Right - baseItem3.DisplayRectangle.Right;
				}
				else
				{
					quickToolbarBounds.Width -= baseItem3.DisplayRectangle.X - quickToolbarBounds.X;
					quickToolbarBounds.X = baseItem3.DisplayRectangle.X;
				}
			}
		}
		quickToolbarBounds.Height = ((ᱭ)_251B.CaptionContainerItem).ᱮ + 6;
		quickToolbarBounds.X = 0;
		quickToolbarBounds.Width = num;
		_251B.QuickToolbarBounds = quickToolbarBounds;
	}
}
internal class _285E : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Invalid comparison between Unknown and I4
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		MetroStatusBar metroStatusBar = (MetroStatusBar)renderingInfo.Control;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroStatusBarColorTable metroStatusBar2 = renderingInfo.ColorTable.MetroStatusBar;
		Rectangle clientRectangle = ((Control)metroStatusBar).ClientRectangle;
		if (metroStatusBar2.BackgroundStyle != null)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(metroStatusBar2.BackgroundStyle, graphics, clientRectangle);
			ElementStyleDisplay.PaintBackground(e);
		}
		if (metroStatusBar2.TopBorders != null && metroStatusBar2.TopBorders.Length > 0)
		{
			for (int i = 0; i < metroStatusBar2.TopBorders.Length; i++)
			{
				Pen val = new Pen(metroStatusBar2.TopBorders[i]);
				try
				{
					graphics.DrawLine(val, clientRectangle.X, clientRectangle.Y + i, clientRectangle.Right, clientRectangle.Y + i);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		if (metroStatusBar2.BottomBorders != null && metroStatusBar2.BottomBorders.Length > 0)
		{
			for (int j = 0; j < metroStatusBar2.BottomBorders.Length; j++)
			{
				Pen val2 = new Pen(metroStatusBar2.BottomBorders[j]);
				try
				{
					graphics.DrawLine(val2, clientRectangle.X, clientRectangle.Bottom - j - 1, clientRectangle.Right, clientRectangle.Bottom - j - 1);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		if (metroStatusBar.ResizeHandleVisible)
		{
			Form val3 = ((Control)metroStatusBar).FindForm();
			if (val3 != null && (int)val3.WindowState == 0)
			{
				_26E2._26E3(graphics, clientRectangle, metroStatusBar2.ResizeMarkerLightColor, metroStatusBar2.ResizeMarkerColor, (int)((Control)metroStatusBar).RightToLeft == 1);
			}
		}
		Rectangle bounds = clientRectangle;
		bounds.Inflate(-2, -2);
		Font val4 = new Font(((Control)metroStatusBar).Font.FontFamily, 8f, (FontStyle)0);
		try
		{
			TextDrawing.DrawString(graphics, "", val4, Color.FromArgb(80, Color.Black), bounds, eTextFormat.Bottom | eTextFormat.HorizontalCenter);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}
}
internal class _285F : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		MetroToolbar metroToolbar = (MetroToolbar)renderingInfo.Control;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroToolbarColorTable metroToolbar2 = renderingInfo.ColorTable.MetroToolbar;
		Rectangle clientRectangle = ((Control)metroToolbar).ClientRectangle;
		if (metroToolbar2.BackgroundStyle != null)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(metroToolbar2.BackgroundStyle, graphics, clientRectangle);
			ElementStyleDisplay.PaintBackground(e);
		}
		if (metroToolbar.Expanded)
		{
			Rectangle bounds = clientRectangle;
			bounds.Inflate(-2, -2);
			Font val = new Font(((Control)metroToolbar).Font.FontFamily, 8f, (FontStyle)0);
			try
			{
				TextDrawing.DrawString(graphics, "", val, Color.FromArgb(32, Color.Black), bounds, eTextFormat.Bottom | eTextFormat.Right);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
internal class _2860 : MetroRenderer
{
	private static readonly int _2861 = 1;

	private static readonly int _2862 = 2;

	private static readonly int _2863 = -4;

	private static readonly int _2864 = 10;

	private static readonly Size _2865 = new Size(27, 27);

	public override void Render(MetroRendererInfo renderingInfo)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		MetroTileItem metroTileItem = (MetroTileItem)renderingInfo.Control;
		if (metroTileItem.Frames.Count == 0)
		{
			HatchBrush val = new HatchBrush((HatchStyle)2, Color.Red);
			try
			{
				renderingInfo.PaintEventArgs.Graphics.FillRectangle((Brush)(object)val, metroTileItem.Bounds);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Matrix val2 = null;
		float num = 0.95f;
		Matrix val3 = null;
		if (renderingInfo.ItemPaintArgs.DragInProgress)
		{
			val2 = renderingInfo.PaintEventArgs.Graphics.Transform;
			val3 = new Matrix(num, 0f, 0f, num, 0f, 0f);
			float num2 = ((float)metroTileItem.WidthInternal * (1f / num) - (float)metroTileItem.WidthInternal) / 2f;
			float num3 = ((float)metroTileItem.HeightInternal * (1f / num) - (float)metroTileItem.HeightInternal) / 2f;
			val3.Translate(num2, num3);
			renderingInfo.PaintEventArgs.Graphics.Transform = val3;
		}
		if (metroTileItem.CurrentFrameOffset != 0 && metroTileItem.ᨤ != metroTileItem.CurrentFrame)
		{
			Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
			if (val2 == null)
			{
				val2 = graphics.Transform;
			}
			graphics.TranslateTransform(0f, (float)(-(metroTileItem.HeightInternal - metroTileItem.CurrentFrameOffset - _2861 * 2)), (MatrixOrder)1);
			_2866(renderingInfo, metroTileItem.ᨤ);
			if (val3 != null)
			{
				graphics.Transform = val3;
			}
			else
			{
				graphics.Transform = val2;
			}
			graphics.TranslateTransform(0f, (float)metroTileItem.CurrentFrameOffset, (MatrixOrder)1);
			_2866(renderingInfo, metroTileItem.CurrentFrame);
		}
		else
		{
			_2866(renderingInfo, metroTileItem.CurrentFrame);
		}
		if (val2 != null)
		{
			renderingInfo.PaintEventArgs.Graphics.Transform = val2;
			val2.Dispose();
		}
	}

	private void _2866(MetroRendererInfo _251C, int _2867)
	{
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Invalid comparison between Unknown and I4
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Invalid comparison between Unknown and I4
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Invalid comparison between Unknown and I4
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Expected O, but got Unknown
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Invalid comparison between Unknown and I4
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2e: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Invalid comparison between Unknown and I4
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Invalid comparison between Unknown and I4
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Invalid comparison between Unknown and I4
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Invalid comparison between Unknown and I4
		//IL_0cc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Expected O, but got Unknown
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Invalid comparison between Unknown and I4
		MetroTileItem metroTileItem = (MetroTileItem)_251C.Control;
		MetroTileFrame metroTileFrame = metroTileItem.Frames[_2867];
		MetroTileColorTable metroTile = _251C.ColorTable.MetroTile;
		Graphics graphics = _251C.PaintEventArgs.Graphics;
		Rectangle bounds = metroTileItem.Bounds;
		Region val = null;
		if (!metroTileItem.DragStartPoint.IsEmpty)
		{
			bounds.Location = _251C.ItemPaintArgs.ContainerControl.PointToClient(Control.MousePosition);
			bounds.Location.Offset(-metroTileItem.DragStartPoint.X, -metroTileItem.DragStartPoint.Y);
			val = graphics.Clip;
			graphics.SetClip(bounds, (CombineMode)0);
		}
		object containerControl = metroTileItem.ContainerControl;
		Control ॼ = (Control)((containerControl is Control) ? containerControl : null);
		bounds.Inflate(-_2861, -_2861);
		if (metroTileItem.IsLeftMouseButtonDown)
		{
			bounds.Inflate(-_2862, -_2862);
		}
		else if (metroTileItem.IsMouseOver)
		{
			bounds.Inflate(_2861, _2861);
		}
		switch (metroTileItem.DesignInsertMarker)
		{
		case ᥡ.ᥢ:
			if (metroTileItem.IsDesignMarkHorizontal)
			{
				bounds.Offset(-_2864, 0);
			}
			else
			{
				bounds.Offset(0, -_2864);
			}
			graphics.ResetClip();
			break;
		case ᥡ.ᆍ:
			if (metroTileItem.IsDesignMarkHorizontal)
			{
				bounds.Offset(_2864, 0);
			}
			else
			{
				bounds.Offset(0, _2864);
			}
			graphics.ResetClip();
			break;
		}
		bool ല = false;
		bool enabled = metroTileItem.GetEnabled(_251C.ItemPaintArgs.ContainerControl);
		ElementStyle elementStyle = ElementStyleDisplay.റ(metroTileFrame.EffectiveStyle, out ല);
		if (bounds.Width > 2048)
		{
			bounds.Width = 2048;
		}
		if (bounds.Height > 1600)
		{
			bounds.Height = 1600;
		}
		if (enabled)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(elementStyle, graphics, bounds);
			ElementStyleDisplay.Paint(e);
		}
		else
		{
			SolidBrush val2 = new SolidBrush(metroTileItem.DisabledBackColor.IsEmpty ? _251C.ColorTable.MetroPartColors.CanvasColorLighterShade : metroTileItem.DisabledBackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val2, bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle rectangle = bounds;
		rectangle.X += elementStyle.PaddingLeft;
		rectangle.Y += elementStyle.PaddingTop;
		rectangle.Width -= elementStyle.PaddingHorizontal;
		rectangle.Height -= elementStyle.PaddingVertical;
		if (metroTileItem.IsLeftMouseButtonDown)
		{
			rectangle.Width += _2862 * 2;
			rectangle.Height += _2862 * 2;
		}
		else if (metroTileItem.IsMouseOver)
		{
			rectangle.Inflate(-_2861, -_2861);
		}
		Size size = Dpi.Size(metroTileItem.TileSize);
		if (metroTileItem.SubItems.Count > 0 && _2867 < metroTileItem.SubItems.Count)
		{
			BaseItem baseItem = metroTileItem.SubItems[_2867];
			if (baseItem.Displayed)
			{
				baseItem.TopInternal = bounds.Y + elementStyle.PaddingTop + (bounds.Height - elementStyle.PaddingTop - metroTileFrame.TitleTextBounds.Height - baseItem.HeightInternal) / 2;
				baseItem.LeftInternal = bounds.X + elementStyle.PaddingLeft;
				baseItem.WidthInternal = size.Width - elementStyle.PaddingHorizontal;
				baseItem.Paint(_251C.ItemPaintArgs);
			}
		}
		Image image = metroTileFrame.Image;
		ContentAlignment imageTextAlignment = metroTileFrame.ImageTextAlignment;
		Color color = (enabled ? elementStyle.TextColor : _251C.ColorTable.MetroPartColors.CanvasColorLightShade);
		Color color2 = color;
		if (!metroTileFrame.SymbolColor.IsEmpty)
		{
			color2 = metroTileFrame.SymbolColor;
		}
		if (image != null || !string.IsNullOrEmpty(metroTileFrame.SymbolRealized))
		{
			Font val3 = null;
			Rectangle empty = Rectangle.Empty;
			if (string.IsNullOrEmpty(metroTileFrame.SymbolRealized))
			{
				empty = new Rectangle(0, 0, Dpi.ImageWidth(image.Width), Dpi.ImageHeight(image.Height));
			}
			else
			{
				val3 = Symbols.GetFont(metroTileFrame.SymbolSize, metroTileFrame.SymbolSet);
				Size size2 = TextDrawing.MeasureString(graphics, metroTileFrame.SymbolRealized, val3);
				int num = (int)Math.Ceiling((float)val3.FontFamily.GetCellDescent(val3.Style) * val3.Size / (float)val3.FontFamily.GetEmHeight(val3.Style));
				size2.Height -= num;
				empty = new Rectangle(0, 0, size2.Width, size2.Height);
			}
			empty.Offset(bounds.Location);
			if ((int)imageTextAlignment == 1)
			{
				rectangle.X += empty.Width + metroTileFrame.ImageIndent.X;
				rectangle.Width -= empty.Width + metroTileFrame.ImageIndent.X;
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
			}
			else if ((int)imageTextAlignment == 2)
			{
				empty.X += (size.Width - empty.Width) / 2;
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				rectangle.Y += empty.Height + metroTileFrame.ImageIndent.Y;
				rectangle.Height -= empty.Height;
			}
			else if ((int)imageTextAlignment == 4)
			{
				empty.X += size.Width - empty.Width - metroTileFrame.ImageIndent.X;
				empty.Offset(0, metroTileFrame.ImageIndent.Y);
				rectangle.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 512)
			{
				empty.Offset((size.Width - empty.Width) / 2, size.Height - empty.Height);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				rectangle.Height -= empty.Height;
			}
			else if ((int)imageTextAlignment == 256)
			{
				empty.Offset(0, size.Height - empty.Height);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				rectangle.X += empty.Width + metroTileFrame.ImageIndent.X;
				rectangle.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 1024)
			{
				empty.Offset(size.Width - empty.Width - metroTileFrame.ImageIndent.X, size.Height - empty.Height);
				empty.Offset(0, metroTileFrame.ImageIndent.Y);
				rectangle.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 32)
			{
				empty.Offset((size.Width - empty.Width) / 2, (size.Height - empty.Height) / 2);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				rectangle.Height = Math.Max(0, rectangle.Bottom - empty.Bottom);
				rectangle.Y = empty.Bottom + 1;
			}
			else if ((int)imageTextAlignment == 16)
			{
				empty.Offset(0, (size.Height - empty.Height) / 2);
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
				rectangle.X += empty.Width + metroTileFrame.ImageIndent.X;
				rectangle.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else if ((int)imageTextAlignment == 64)
			{
				empty.Offset(size.Width - empty.Width - metroTileFrame.ImageIndent.X, (size.Height - empty.Height) / 2);
				empty.Offset(0, metroTileFrame.ImageIndent.Y);
				rectangle.Width -= empty.Width + metroTileFrame.ImageIndent.X;
			}
			else
			{
				empty.Offset(metroTileFrame.ImageIndent.X, metroTileFrame.ImageIndent.Y);
			}
			if (string.IsNullOrEmpty(metroTileFrame.SymbolRealized))
			{
				graphics.DrawImage(image, empty);
			}
			else
			{
				TextDrawing.DrawStringLegacy(graphics, metroTileFrame.SymbolRealized, val3, color2, new Rectangle(empty.X, empty.Y, 0, 0), eTextFormat.Default);
			}
		}
		if (rectangle.Width > 0 && rectangle.Height > 0 && metroTileFrame.Text != null)
		{
			Font val4 = _251C.DefaultFont;
			if (elementStyle.Font != null)
			{
				val4 = elementStyle.Font;
			}
			bool rightToLeft = _251C.RightToLeft;
			if (metroTileFrame.TextMarkupBody == null)
			{
				eTextFormat eTextFormat = eTextFormat.NoClipping | eTextFormat.WordBreak;
				if (elementStyle.TextLineAlignment == eStyleTextAlignment.Center)
				{
					eTextFormat |= eTextFormat.VerticalCenter;
				}
				else if (elementStyle.TextLineAlignment == eStyleTextAlignment.Far)
				{
					eTextFormat |= eTextFormat.Bottom;
				}
				if (elementStyle.TextAlignment == eStyleTextAlignment.Center)
				{
					eTextFormat |= eTextFormat.HorizontalCenter;
				}
				else if (elementStyle.TextAlignment == eStyleTextAlignment.Far)
				{
					eTextFormat |= eTextFormat.Right;
				}
				TextDrawing.DrawString(graphics, metroTileFrame.Text, val4, color, rectangle, eTextFormat);
			}
			else
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, val4, color, rightToLeft);
				markupDrawContext.HotKeyPrefixVisible = false;
				markupDrawContext.ContextObject = metroTileItem;
				Rectangle bounds2 = rectangle;
				metroTileFrame.TextMarkupBody.Bounds = bounds2;
				metroTileFrame.TextMarkupBody.Render(markupDrawContext);
			}
		}
		if (metroTileFrame.TitleText != null)
		{
			Color color3 = (enabled ? metroTileFrame.TitleTextColor : _251C.ColorTable.MetroPartColors.CanvasColorLightShade);
			if (color3.IsEmpty)
			{
				color3 = elementStyle.TextColor;
			}
			Font val5 = metroTileItem.ᨶ(metroTileFrame, elementStyle, ॼ);
			Rectangle titleTextBounds = metroTileFrame.TitleTextBounds;
			titleTextBounds.Offset(bounds.Location);
			if (metroTileItem.IsLeftMouseButtonDown)
			{
				titleTextBounds.Offset(1, 1);
			}
			else if (metroTileItem.IsMouseOver)
			{
				titleTextBounds.Offset(_2861, _2861);
			}
			if (metroTileFrame.TitleTextMarkupBody == null)
			{
				TextDrawing.DrawString(graphics, metroTileFrame.TitleText, val5, color3, titleTextBounds, eTextFormat.SingleLine);
			}
			else
			{
				MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, val5, color3, _251C.RightToLeft);
				markupDrawContext2.HotKeyPrefixVisible = false;
				markupDrawContext2.ContextObject = metroTileItem;
				Rectangle bounds3 = titleTextBounds;
				metroTileFrame.TitleTextMarkupBody.Bounds = bounds3;
				metroTileFrame.TitleTextMarkupBody.Render(markupDrawContext2);
			}
		}
		if (metroTileItem.Checked)
		{
			Size size3 = Dpi.Size(_2865);
			Rectangle rectangle2 = new Rectangle(bounds.Right - size3.Width, bounds.Y, size3.Width, size3.Height);
			GraphicsPath val6 = new GraphicsPath();
			try
			{
				val6.AddLine(rectangle2.X, rectangle2.Y, rectangle2.Right - 1, rectangle2.Y);
				val6.AddLine(rectangle2.Right - 1, rectangle2.Y, rectangle2.Right - 1, rectangle2.Bottom - 1);
				val6.CloseFigure();
				SolidBrush val7 = new SolidBrush(metroTile.CheckBackground);
				try
				{
					graphics.FillPath((Brush)(object)val7, val6);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
			SolidBrush val8 = new SolidBrush(metroTile.CheckForeground);
			try
			{
				Rectangle rectangle3 = default(Rectangle);
				rectangle3.Size = new Size(Dpi.Width11, Dpi.Height11);
				rectangle3.X = rectangle2.Right - rectangle3.Size.Width - Dpi.Width3;
				rectangle3.Y = rectangle2.Y + Dpi.Height2;
				Pen val9 = new Pen(metroTile.CheckForeground, (float)Dpi.Width2);
				try
				{
					graphics.DrawEllipse(val9, rectangle3);
					graphics.DrawLine(val9, rectangle3.X + Dpi.Width3, rectangle3.Y + Dpi.Height5, rectangle3.X + Dpi.Width6, rectangle3.Y + Dpi.Height8);
					graphics.DrawLine(val9, rectangle3.X + Dpi.Width6, rectangle3.Y + Dpi.Height8, rectangle3.X + Dpi.Width9, rectangle3.Y + Dpi.Height3);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		if (ല)
		{
			elementStyle.Dispose();
		}
		if (val != null)
		{
			graphics.Clip = val;
			val.Dispose();
		}
	}
}
