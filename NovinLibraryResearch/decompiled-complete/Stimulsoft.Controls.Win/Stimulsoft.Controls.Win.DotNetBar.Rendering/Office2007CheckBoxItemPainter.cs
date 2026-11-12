using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class Office2007CheckBoxItemPainter : CheckBoxItemPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	public override void Paint(CheckBoxItemRenderEventArgs e)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Invalid comparison between Unknown and I4
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Invalid comparison between Unknown and I4
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected I4, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		CheckBoxItem checkBoxItem = e.CheckBoxItem;
		bool rightToLeft = e.RightToLeft;
		Font font = e.Font;
		Office2007CheckBoxStateColorTable checkBoxStateColorTable = GetCheckBoxStateColorTable(e);
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = Rectangle.Empty;
		eTextFormat eTextFormat = eTextFormat.Default;
		if ((e.ItemPaintArgs == null || !(e.ItemPaintArgs.ContainerControl is CheckBoxX)) && (e.ItemPaintArgs == null || !(e.ItemPaintArgs.ContainerControl is ItemControl)) && (e.ItemPaintArgs == null || !(e.ItemPaintArgs.ContainerControl is Bar)) && (e.ItemPaintArgs == null || !(e.ItemPaintArgs.ContainerControl is MenuPanel)))
		{
			eTextFormat |= eTextFormat.NoPrefix;
		}
		Size checkSignSize = checkBoxItem.GetCheckSignSize();
		float num = font.SizeInPoints * (float)StiScale2.Y / (float)font.FontFamily.GetEmHeight(font.Style) * (float)font.FontFamily.GetCellAscent(font.Style) + 4f * (float)StiScale2.Y;
		if ((checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Left && !rightToLeft) || (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Right && rightToLeft))
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.X + checkBoxItem.CheckTextSpacing / 2, checkBoxItem.DisplayRectangle.Y + (checkBoxItem.DisplayRectangle.Height - checkSignSize.Height) / 2, checkSignSize.Width, checkSignSize.Height);
			rectangle2 = new Rectangle(rectangle.Right + checkBoxItem.CheckTextSpacing / 2, checkBoxItem.DisplayRectangle.Y, checkBoxItem.DisplayRectangle.Right - (rectangle.Right + checkBoxItem.CheckTextSpacing / 2), checkBoxItem.DisplayRectangle.Height);
			if (checkBoxItem.TextMarkupBody == null)
			{
				rectangle2.Y = rectangle.Bottom - (int)num - (int)Math.Ceiling(2.0 * StiScale2.Y);
			}
		}
		else if ((checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Right && !rightToLeft) || (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Left && rightToLeft))
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.Right - checkBoxItem.CheckTextSpacing / 2 - checkSignSize.Width, checkBoxItem.DisplayRectangle.Y + (checkBoxItem.DisplayRectangle.Height - checkSignSize.Height) / 2, checkSignSize.Width, checkSignSize.Height);
			rectangle2 = new Rectangle(checkBoxItem.DisplayRectangle.X, checkBoxItem.DisplayRectangle.Y, rectangle.X - (checkBoxItem.DisplayRectangle.X + checkBoxItem.CheckTextSpacing / 2), checkBoxItem.DisplayRectangle.Height);
			if (checkBoxItem.TextMarkupBody == null)
			{
				rectangle2.Y = rectangle.Bottom - (int)num - (int)Math.Ceiling(2.0 * StiScale2.Y);
			}
			eTextFormat |= eTextFormat.Right;
		}
		else if (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Top)
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.X + (checkBoxItem.DisplayRectangle.Width - checkSignSize.Width) / 2, checkBoxItem.DisplayRectangle.Y + checkBoxItem.VerticalPadding, checkSignSize.Width, checkSignSize.Height);
			rectangle2 = new Rectangle(checkBoxItem.DisplayRectangle.X, rectangle.Bottom, checkBoxItem.DisplayRectangle.Width, checkBoxItem.DisplayRectangle.Bottom - rectangle.Bottom);
			eTextFormat |= eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter;
		}
		else if (checkBoxItem.CheckBoxPosition == eCheckBoxPosition.Bottom)
		{
			rectangle = new Rectangle(checkBoxItem.DisplayRectangle.X + (checkBoxItem.DisplayRectangle.Width - checkSignSize.Width) / 2, checkBoxItem.DisplayRectangle.Bottom - checkBoxItem.VerticalPadding - checkSignSize.Height - 1, checkSignSize.Width, checkSignSize.Height);
			rectangle2 = new Rectangle(checkBoxItem.DisplayRectangle.X, checkBoxItem.DisplayRectangle.Y, checkBoxItem.DisplayRectangle.Width, rectangle.Y - (checkBoxItem.DisplayRectangle.Y + checkBoxItem.VerticalPadding));
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
			if (checkBoxItem.ThemeAware)
			{
				CheckBoxState val = (CheckBoxState)1;
				CheckState checkState = checkBoxItem.CheckState;
				switch ((int)checkState)
				{
				case 0:
					val = (checkBoxItem.Enabled ? ((!checkBoxItem.IsMouseDown) ? ((!checkBoxItem.IsMouseOver) ? ((CheckBoxState)1) : ((CheckBoxState)2)) : ((CheckBoxState)3)) : ((CheckBoxState)4));
					break;
				case 1:
					val = (checkBoxItem.Enabled ? ((!checkBoxItem.IsMouseDown) ? ((!checkBoxItem.IsMouseOver) ? ((CheckBoxState)5) : ((CheckBoxState)6)) : ((CheckBoxState)7)) : ((CheckBoxState)8));
					break;
				case 2:
					val = (checkBoxItem.Enabled ? ((!checkBoxItem.IsMouseDown) ? ((!checkBoxItem.IsMouseOver) ? ((CheckBoxState)9) : ((CheckBoxState)10)) : ((CheckBoxState)11)) : ((CheckBoxState)12));
					break;
				}
				CheckBoxRenderer.DrawCheckBox(graphics, rectangle.Location, Rectangle.Empty, "", (Font)null, checkBoxItem.Focused, val);
			}
			else
			{
				PaintCheckBox(graphics, rectangle, checkBoxStateColorTable, checkBoxItem.CheckState);
			}
		}
		else
		{
			PaintRadioButton(graphics, rectangle, checkBoxStateColorTable, checkBoxItem.Checked);
		}
		Color color = checkBoxStateColorTable.Text;
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
					rectangle2.Y = checkBoxItem.TopInternal + (checkBoxItem.Bounds.Height - rectangle2.Height) / 2;
				}
				else if ((eTextFormat & eTextFormat.Bottom) == eTextFormat.Bottom)
				{
					rectangle2.Y += checkBoxItem.Bounds.Height - rectangle2.Height + 1;
				}
				checkBoxItem.TextMarkupBody.Bounds = rectangle2;
				checkBoxItem.TextMarkupBody.Render(markupDrawContext);
			}
			else if (e.ItemPaintArgs != null && e.ItemPaintArgs.GlassEnabled && checkBoxItem.Parent is CaptionItemContainer && !(e.ItemPaintArgs.ContainerControl is QatToolbar))
			{
				if (!e.ItemPaintArgs.CachedPaint)
				{
					Office2007RibbonControlPainter.PaintTextOnGlass(graphics, checkBoxItem.Text, font, rectangle2, TextDrawing.GetTextFormat(eTextFormat));
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
			DesignTime.DrawDesignTimeSelection(graphics, displayRectangle, e.ColorScheme.ItemDesignTimeBorder);
		}
	}

	public void PaintRadioButton(Graphics g, Rectangle checkBoxPosition, Office2007CheckBoxStateColorTable ct, bool isChecked)
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
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		Rectangle rectangle = checkBoxPosition;
		rectangle.Inflate(-1, -1);
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddEllipse(rectangle);
			DisplayHelp.FillPath(g, val, ct.CheckBackground);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(ct.CheckBorder, 1f);
		try
		{
			g.DrawEllipse(val2, checkBoxPosition);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		checkBoxPosition.Inflate(-3, -3);
		Pen val3 = new Pen(ct.CheckInnerBorder, 1f);
		try
		{
			g.DrawEllipse(val3, checkBoxPosition);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		if (!ct.CheckInnerBackground.IsEmpty)
		{
			if (ct.CheckInnerBackground.End.IsEmpty)
			{
				rectangle = checkBoxPosition;
				rectangle.Inflate(-1, -1);
				SolidBrush val4 = new SolidBrush(ct.CheckInnerBackground.Start);
				try
				{
					g.FillEllipse((Brush)(object)val4, rectangle);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else
			{
				Region clip = g.Clip;
				g.SetClip(checkBoxPosition, (CombineMode)0);
				GraphicsPath val5 = new GraphicsPath();
				try
				{
					val5.AddEllipse(checkBoxPosition);
					PathGradientBrush val6 = new PathGradientBrush(val5);
					try
					{
						val6.CenterColor = ct.CheckInnerBackground.Start;
						val6.SurroundColors = new Color[1] { ct.CheckInnerBackground.End };
						val6.CenterPoint = new PointF(checkBoxPosition.X, checkBoxPosition.Y);
						g.FillEllipse((Brush)(object)val6, checkBoxPosition);
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
				g.Clip = clip;
			}
		}
		if (isChecked && !ct.CheckSign.IsEmpty)
		{
			rectangle = checkBoxPosition;
			GraphicsPath val7 = new GraphicsPath();
			try
			{
				val7.AddEllipse(rectangle);
				DisplayHelp.FillPath(g, val7, ct.CheckSign);
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
	}

	public void PaintCheckBox(Graphics g, Rectangle checkBoxPosition, Office2007CheckBoxStateColorTable ct, CheckState checkState)
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Invalid comparison between Unknown and I4
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Invalid comparison between Unknown and I4
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		Rectangle r = checkBoxPosition;
		r.Inflate(-1, -1);
		if (checkBoxPosition.Width < 5 || checkBoxPosition.Height < 5)
		{
			return;
		}
		DisplayHelp.FillRectangle(g, r, ct.CheckBackground);
		DisplayHelp.DrawRectangle(g, ct.CheckBorder, checkBoxPosition);
		checkBoxPosition.Inflate(-2, -2);
		if (!ct.CheckInnerBackground.IsEmpty)
		{
			if (ct.CheckInnerBackground.End.IsEmpty)
			{
				r = checkBoxPosition;
				r.Inflate(-1, -1);
				DisplayHelp.FillRectangle(g, r, ct.CheckInnerBackground.Start);
			}
			else
			{
				Region clip = g.Clip;
				g.SetClip(checkBoxPosition, (CombineMode)1);
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.AddRectangle(checkBoxPosition);
					PathGradientBrush val2 = new PathGradientBrush(val);
					try
					{
						val2.CenterColor = ct.CheckInnerBackground.Start;
						val2.SurroundColors = new Color[1] { ct.CheckInnerBackground.End };
						val2.CenterPoint = new PointF(checkBoxPosition.X, checkBoxPosition.Y);
						g.FillRectangle((Brush)(object)val2, checkBoxPosition);
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
				g.Clip = clip;
			}
		}
		DisplayHelp.DrawRectangle(g, ct.CheckInnerBorder, checkBoxPosition);
		if ((int)checkState == 2)
		{
			checkBoxPosition.Inflate(-2, -2);
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)3;
			DisplayHelp.FillRectangle(g, checkBoxPosition, ct.CheckSign);
			g.SmoothingMode = smoothingMode;
		}
		else if ((int)checkState == 1 && !ct.CheckSign.IsEmpty)
		{
			GraphicsPath checkSign = GetCheckSign(checkBoxPosition);
			try
			{
				DisplayHelp.FillPath(g, checkSign, ct.CheckSign);
			}
			finally
			{
				((IDisposable)checkSign)?.Dispose();
			}
		}
	}

	private GraphicsPath GetCheckSign(Rectangle outterRect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = outterRect;
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
			return m_ColorTable.CheckBoxItem;
		}
		Office2007ColorTable colorTable = m_ColorTable;
		string contextualKey = Office2007ColorTable.GetContextualKey(typeof(Office2007CheckBoxColorTable), ((object)container).GetType());
		object value = null;
		if (container is Bar && colorTable.ContextualTables.TryGetValue(contextualKey + "+" + ((Bar)(object)container).BarType, out value))
		{
			return (Office2007CheckBoxColorTable)value;
		}
		if (colorTable.ContextualTables.TryGetValue(contextualKey, out value))
		{
			return (Office2007CheckBoxColorTable)value;
		}
		return m_ColorTable.CheckBoxItem;
	}

	private Office2007CheckBoxStateColorTable GetCheckBoxStateColorTable(CheckBoxItemRenderEventArgs e)
	{
		CheckBoxItem checkBoxItem = e.CheckBoxItem;
		if (m_ColorTable != null && BarFunctions.IsOffice2007Style(e.CheckBoxItem.EffectiveStyle))
		{
			Office2007CheckBoxColorTable colorTable = GetColorTable(e.CheckBoxItem, e.ItemPaintArgs.ContainerControl);
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
		ColorScheme colorScheme = e.ColorScheme;
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
			office2007CheckBoxStateColorTable.CheckInnerBorder = ColorBlendFactory.SoftLight(colorScheme.PanelBorder, Color.White);
			office2007CheckBoxStateColorTable.CheckInnerBackground = new LinearGradientColorTable(colorScheme.MenuBackground, Color.Empty);
			office2007CheckBoxStateColorTable.CheckSign = new LinearGradientColorTable(colorScheme.ItemText, Color.Empty);
			office2007CheckBoxStateColorTable.Text = colorScheme.ItemText;
		}
		return office2007CheckBoxStateColorTable;
	}
}
