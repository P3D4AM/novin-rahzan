using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Rendering;

internal class ᢏ : ᢘ, ᙓ
{
	private Office2007ColorTable ຊ;

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	public override void Paint(CheckBoxItemRenderEventArgs e)
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Invalid comparison between Unknown and I4
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Invalid comparison between Unknown and I4
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		CheckBoxItem checkBoxItem = e.CheckBoxItem;
		bool rightToLeft = e.RightToLeft;
		Font font = e.Font;
		Office2007CheckBoxStateColorTable office2007CheckBoxStateColorTable = ᢗ(e);
		bool flag = e.က.ContainerControl is CheckBoxX;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = Rectangle.Empty;
		eTextFormat eTextFormat = eTextFormat.Default;
		if ((e.က == null || !(e.က.ContainerControl is CheckBoxX)) && (e.က == null || !(e.က.ContainerControl is ItemControl)) && (e.က == null || !(e.က.ContainerControl is Bar)) && (e.က == null || !(e.က.ContainerControl is MenuPanel)))
		{
			eTextFormat |= eTextFormat.NoPrefix;
		}
		Size size = checkBoxItem._18A9();
		float num = graphics.DpiY / 72f * (font.SizeInPoints / (float)font.FontFamily.GetEmHeight(font.Style) * (float)font.FontFamily.GetCellAscent(font.Style)) + 0.5f;
		if ((checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Left && !rightToLeft) || (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Right && rightToLeft))
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.X + ((!flag) ? (checkBoxItem.ᢙ / 2) : 0), checkBoxItem.DisplayRectangle.Y + (checkBoxItem.DisplayRectangle.Height - size.Height) / 2, size.Width, size.Height);
			rectangle2 = new Rectangle(rectangle.Right + checkBoxItem.ᢙ / 2, checkBoxItem.DisplayRectangle.Y, checkBoxItem.DisplayRectangle.Right - (rectangle.Right + checkBoxItem.ᢙ / 2), checkBoxItem.DisplayRectangle.Height);
			if (checkBoxItem.TextMarkupBody == null)
			{
				if (!checkBoxItem.Text.Contains(Environment.NewLine) && !checkBoxItem.ᢚ && checkBoxItem.ᆾ.Width <= rectangle2.Width)
				{
					rectangle2.Y = rectangle.Bottom - (int)num - Dpi.Height(3);
				}
				else
				{
					eTextFormat |= eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				}
			}
			else
			{
				eTextFormat |= eTextFormat.VerticalCenter;
			}
		}
		else if ((checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Right && !rightToLeft) || (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Left && rightToLeft))
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.Right - (flag ? 1 : (checkBoxItem.ᢙ / 2)) - size.Width, checkBoxItem.DisplayRectangle.Y + (checkBoxItem.DisplayRectangle.Height - size.Height) / 2, size.Width, size.Height);
			rectangle2 = new Rectangle(checkBoxItem.DisplayRectangle.X, checkBoxItem.DisplayRectangle.Y, rectangle.X - (checkBoxItem.DisplayRectangle.X + checkBoxItem.ᢙ / 2), checkBoxItem.DisplayRectangle.Height);
			if (checkBoxItem.TextMarkupBody == null)
			{
				if (!checkBoxItem.Text.Contains(Environment.NewLine) && checkBoxItem.ᆾ.Width <= rectangle2.Width)
				{
					rectangle2.Y = rectangle.Bottom - (int)num - Dpi.Height(3);
				}
				else
				{
					eTextFormat |= eTextFormat.VerticalCenter | eTextFormat.WordBreak;
				}
			}
			else
			{
				eTextFormat |= eTextFormat.VerticalCenter;
			}
			eTextFormat |= eTextFormat.Right;
		}
		else if (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Top)
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.X + (checkBoxItem.DisplayRectangle.Width - size.Width) / 2, checkBoxItem.DisplayRectangle.Y + checkBoxItem._0E62, size.Width, size.Height);
			rectangle2 = new Rectangle(checkBoxItem.DisplayRectangle.X, rectangle.Bottom, checkBoxItem.DisplayRectangle.Width, checkBoxItem.DisplayRectangle.Bottom - rectangle.Bottom);
			eTextFormat |= eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
		}
		else if (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Bottom)
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.X + (checkBoxItem.DisplayRectangle.Width - size.Width) / 2, checkBoxItem.DisplayRectangle.Bottom - checkBoxItem._0E62 - size.Height - 1, size.Width, size.Height);
			rectangle2 = new Rectangle(checkBoxItem.DisplayRectangle.X, checkBoxItem.DisplayRectangle.Y, checkBoxItem.DisplayRectangle.Width, rectangle.Y - (checkBoxItem.DisplayRectangle.Y + checkBoxItem._0E62));
			eTextFormat |= eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
		}
		if ((int)checkBoxItem.CheckState == 0 && checkBoxItem.CheckBoxImageUnChecked != null)
		{
			graphics.DrawImage(checkBoxItem.CheckBoxImageUnChecked, rectangle);
		}
		else if ((int)checkBoxItem.CheckState == 1 && checkBoxItem.CheckBoxImageChecked != null)
		{
			graphics.DrawImage(checkBoxItem.CheckBoxImageChecked, rectangle);
		}
		else if ((int)checkBoxItem.CheckState == 2 && checkBoxItem.CheckBoxImageIndeterminate != null)
		{
			graphics.DrawImage(checkBoxItem.CheckBoxImageIndeterminate, rectangle);
		}
		else if (checkBoxItem.CheckBoxStyle == eCheckBoxStyle.CheckBox)
		{
			ᢓ(graphics, rectangle, office2007CheckBoxStateColorTable, checkBoxItem.CheckState);
		}
		else
		{
			ᢐ(graphics, rectangle, office2007CheckBoxStateColorTable, checkBoxItem.Checked);
		}
		Color color = office2007CheckBoxStateColorTable.Text;
		if (!checkBoxItem.TextColor.IsEmpty)
		{
			color = checkBoxItem.TextColor;
		}
		if (checkBoxItem.Text != "" && !rectangle2.IsEmpty && !color.IsEmpty && checkBoxItem.Orientation != eOrientation.Vertical && checkBoxItem.TextVisible)
		{
			if (checkBoxItem.TextMarkupBody != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, color, rightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				if ((eTextFormat & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
				{
					rectangle2.Y = checkBoxItem.TopInternal + (checkBoxItem.Bounds.Height - checkBoxItem.TextMarkupBody.Bounds.Height) / 2;
					rectangle2.Height = checkBoxItem.TextMarkupBody.Bounds.Height;
				}
				else if ((eTextFormat & eTextFormat.Bottom) == eTextFormat.Bottom)
				{
					rectangle2.Y += checkBoxItem.Bounds.Height - checkBoxItem.TextMarkupBody.Bounds.Height + 1;
					rectangle2.Height = checkBoxItem.TextMarkupBody.Bounds.Height;
				}
				checkBoxItem.TextMarkupBody.Bounds = rectangle2;
				checkBoxItem.TextMarkupBody.Render(markupDrawContext);
			}
			else if (e.က != null && e.က.GlassEnabled && checkBoxItem.Parent is ᱭ && !(e.က.ContainerControl is Ꮮ))
			{
				if (!e.က.ໟ)
				{
					_21F8._21FC(graphics, checkBoxItem.Text, font, rectangle2, TextDrawing.GetTextFormat(eTextFormat));
				}
			}
			else
			{
				TextDrawing.DrawString(graphics, checkBoxItem.Text, font, color, rectangle2, eTextFormat);
			}
		}
		if (checkBoxItem.Focused && checkBoxItem.DesignMode)
		{
			Rectangle displayRectangle = checkBoxItem.DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			_1A6B._1A6C(graphics, displayRectangle, e.ColorScheme.ItemDesignTimeBorder);
		}
	}

	public void ᢐ(Graphics _٥, Rectangle ᢑ, Office2007CheckBoxStateColorTable ዅ, bool ᢒ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		Rectangle rectangle = ᢑ;
		rectangle.Inflate(-1, -1);
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddEllipse(rectangle);
			DisplayHelp.FillPath(_٥, val, ዅ.CheckBackground);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(ዅ.CheckBorder, 1f);
		try
		{
			_٥.DrawEllipse(val2, ᢑ);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		ᢑ.Inflate(-3, -3);
		Pen val3 = new Pen(ዅ.CheckInnerBorder, 1f);
		try
		{
			_٥.DrawEllipse(val3, ᢑ);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		if (!ዅ.CheckInnerBackground.IsEmpty)
		{
			if (ዅ.CheckInnerBackground.End.IsEmpty)
			{
				rectangle = ᢑ;
				rectangle.Inflate(-1, -1);
				SolidBrush val4 = new SolidBrush(ዅ.CheckInnerBackground.Start);
				try
				{
					_٥.FillEllipse((Brush)(object)val4, rectangle);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else
			{
				Region clip = _٥.Clip;
				_٥.SetClip(ᢑ, (CombineMode)0);
				GraphicsPath val5 = new GraphicsPath();
				try
				{
					val5.AddEllipse(ᢑ);
					PathGradientBrush val6 = new PathGradientBrush(val5);
					try
					{
						val6.CenterColor = ዅ.CheckInnerBackground.Start;
						val6.SurroundColors = new Color[1] { ዅ.CheckInnerBackground.End };
						val6.CenterPoint = new PointF(ᢑ.X, ᢑ.Y);
						_٥.FillEllipse((Brush)(object)val6, ᢑ);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				_٥.Clip = clip;
			}
		}
		if (ᢒ && !ዅ.CheckSign.IsEmpty)
		{
			rectangle = ᢑ;
			GraphicsPath val7 = new GraphicsPath();
			try
			{
				val7.AddEllipse(rectangle);
				DisplayHelp.FillPath(_٥, val7, ዅ.CheckSign);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
	}

	public void ᢓ(Graphics _٥, Rectangle ᢑ, Office2007CheckBoxStateColorTable ዅ, CheckState ᢔ)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Invalid comparison between Unknown and I4
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Invalid comparison between Unknown and I4
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		Rectangle r = ᢑ;
		r.Inflate(-1, -1);
		if (ᢑ.Width < 5 || ᢑ.Height < 5)
		{
			return;
		}
		DisplayHelp.FillRectangle(_٥, r, ዅ.CheckBackground);
		DisplayHelp.DrawRectangle(_٥, ዅ.CheckBorder, ᢑ);
		ᢑ.Inflate(-2, -2);
		if (!ዅ.CheckInnerBackground.IsEmpty)
		{
			if (ዅ.CheckInnerBackground.End.IsEmpty)
			{
				r = ᢑ;
				r.Inflate(-1, -1);
				DisplayHelp.FillRectangle(_٥, r, ዅ.CheckInnerBackground.Start);
			}
			else
			{
				Region clip = _٥.Clip;
				_٥.SetClip(ᢑ, (CombineMode)1);
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddRectangle(ᢑ);
					PathGradientBrush val2 = new PathGradientBrush(val);
					try
					{
						val2.CenterColor = ዅ.CheckInnerBackground.Start;
						val2.SurroundColors = new Color[1] { ዅ.CheckInnerBackground.End };
						val2.CenterPoint = new PointF(ᢑ.X, ᢑ.Y);
						_٥.FillRectangle((Brush)(object)val2, ᢑ);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				_٥.Clip = clip;
			}
		}
		DisplayHelp.DrawRectangle(_٥, ዅ.CheckInnerBorder, ᢑ);
		if ((int)ᢔ == 2)
		{
			ᢑ.Inflate(-2, -2);
			SmoothingMode smoothingMode = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)3;
			DisplayHelp.FillRectangle(_٥, ᢑ, ዅ.CheckSign);
			_٥.SmoothingMode = smoothingMode;
		}
		else if ((int)ᢔ == 1 && !ዅ.CheckSign.IsEmpty)
		{
			GraphicsPath val3 = ᢕ(ᢑ);
			try
			{
				DisplayHelp.FillPath(_٥, val3, ዅ.CheckSign);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
	}

	private GraphicsPath ᢕ(Rectangle ᢖ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = ᢖ;
		rectangle.Inflate(-1, -1);
		val.AddLine((float)rectangle.X, (float)rectangle.Y + (float)rectangle.Height * 0.75f, (float)rectangle.X + (float)rectangle.Width * 0.3f, (float)rectangle.Bottom);
		val.AddLine((float)rectangle.X + (float)rectangle.Width * 0.4f, (float)rectangle.Bottom, (float)rectangle.Right, (float)rectangle.Y + (float)rectangle.Height * 0.05f);
		val.AddLine((float)rectangle.Right - (float)rectangle.Width * 0.3f, (float)rectangle.Y, (float)rectangle.X + (float)rectangle.Width * 0.25f, (float)rectangle.Y + (float)rectangle.Height * 0.75f);
		val.AddLine((float)rectangle.X + (float)rectangle.Width * 0.1f, (float)rectangle.Y + (float)rectangle.Height * 0.5f, (float)rectangle.X, (float)rectangle.Y + (float)rectangle.Height * 0.55f);
		val.CloseAllFigures();
		return val;
	}

	protected virtual Office2007CheckBoxColorTable GetColorTable(CheckBoxItem item, Control container)
	{
		if (container == null)
		{
			return ຊ.CheckBoxItem;
		}
		Office2007ColorTable office2007ColorTable = ຊ;
		string contextualKey = Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), ((object)container).GetType());
		object value = null;
		if (container is Bar && office2007ColorTable.ContextualTables.TryGetValue(contextualKey + "+" + ((Bar)(object)container).BarType, out value))
		{
			return (Office2007CheckBoxColorTable)value;
		}
		if (office2007ColorTable.ContextualTables.TryGetValue(contextualKey, out value))
		{
			return (Office2007CheckBoxColorTable)value;
		}
		return ຊ.CheckBoxItem;
	}

	private Office2007CheckBoxStateColorTable ᢗ(CheckBoxItemRenderEventArgs ؿ)
	{
		CheckBoxItem checkBoxItem = ؿ.CheckBoxItem;
		if (ຊ != null && BarFunctions.IsOffice2007Style(ؿ.CheckBoxItem.EffectiveStyle))
		{
			Office2007CheckBoxColorTable colorTable = GetColorTable(ؿ.CheckBoxItem, ؿ.က.ContainerControl);
			if (!checkBoxItem.GetEnabled())
			{
				return colorTable.Disabled;
			}
			if (checkBoxItem.IsMouseDown)
			{
				return colorTable.Pressed;
			}
			if (checkBoxItem.IsMouseOver)
			{
				return colorTable.MouseOver;
			}
			return colorTable.Default;
		}
		ColorScheme colorScheme = ؿ.ColorScheme;
		Office2007CheckBoxStateColorTable office2007CheckBoxStateColorTable = new Office2007CheckBoxStateColorTable();
		if (!checkBoxItem.GetEnabled())
		{
			office2007CheckBoxStateColorTable.CheckBackground = new LinearGradientColorTable(colorScheme.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckBorder = colorScheme.ItemDisabledText;
			office2007CheckBoxStateColorTable.CheckInnerBorder = colorScheme.ItemDisabledText;
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable();
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(colorScheme.ItemDisabledText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = colorScheme.ItemDisabledText;
		}
		else if (checkBoxItem.IsMouseDown)
		{
			office2007CheckBoxStateColorTable.CheckBackground = new LinearGradientColorTable(colorScheme.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckBorder = colorScheme.ItemPressedBorder;
			office2007CheckBoxStateColorTable.CheckInnerBorder = colorScheme.ItemPressedBorder;
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable(colorScheme.ItemPressedBackground, colorScheme.ItemPressedBackground2);
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(colorScheme.ItemPressedText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = colorScheme.ItemPressedText;
		}
		else if (checkBoxItem.IsMouseOver)
		{
			office2007CheckBoxStateColorTable.CheckBackground = new LinearGradientColorTable(colorScheme.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckBorder = colorScheme.ItemHotBorder;
			office2007CheckBoxStateColorTable.CheckInnerBorder = colorScheme.ItemHotBorder;
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable(colorScheme.ItemHotBackground, colorScheme.ItemHotBackground2);
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(colorScheme.ItemHotText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = colorScheme.ItemHotText;
		}
		else
		{
			office2007CheckBoxStateColorTable.CheckBackground = new LinearGradientColorTable(colorScheme.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckBorder = colorScheme.PanelBorder;
			office2007CheckBoxStateColorTable.CheckInnerBorder = ColorBlendFactory.ዀ(colorScheme.PanelBorder, Color.White);
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable(colorScheme.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(colorScheme.ItemText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = colorScheme.ItemText;
		}
		return office2007CheckBoxStateColorTable;
	}
}
