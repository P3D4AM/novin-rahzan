using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class TextElement : MarkupElement
{
	private static bool PadText;

	private string m_Text = "";

	private bool m_TrailingSpace;

	private bool m_EnablePrefixHandling = true;

	public string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			m_Text = value;
			IsSizeValid = false;
		}
	}

	public bool TrailingSpace
	{
		get
		{
			return m_TrailingSpace;
		}
		set
		{
			m_TrailingSpace = value;
			IsSizeValid = false;
		}
	}

	public bool EnablePrefixHandling
	{
		get
		{
			return m_EnablePrefixHandling;
		}
		set
		{
			m_EnablePrefixHandling = value;
		}
	}

	static TextElement()
	{
		string twoLetterISOLanguageName = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
		if (twoLetterISOLanguageName == "ja")
		{
			PadText = true;
		}
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		if (BarUtilities.UseTextRenderer)
		{
			eTextFormat eTextFormat = eTextFormat.NoPadding;
			if (!d.HotKeyPrefixVisible || m_EnablePrefixHandling)
			{
				eTextFormat |= eTextFormat.HidePrefix;
			}
			Size empty = Size.Empty;
			if (m_TrailingSpace)
			{
				if (d.CurrentFont.Italic)
				{
					empty = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, m_Text, d.CurrentFont, 0, eTextFormat));
					empty.Width += (int)(d.Graphics.MeasureString("||", d.CurrentFont).Width / 4f);
				}
				else
				{
					empty = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, m_Text + ((BarFunctions.IsVista && m_Text.Length > 0) ? "|" : "||"), d.CurrentFont, 0, eTextFormat));
				}
			}
			else
			{
				empty = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, m_Text, d.CurrentFont, 0, eTextFormat));
			}
			if (PadText)
			{
				empty.Width += BarUtilities.TextMarkupCultureSpecificPadding;
				empty.Height += BarUtilities.TextMarkupCultureSpecificPadding;
			}
			base.Bounds = new Rectangle(Point.Empty, empty);
		}
		else
		{
			StringFormat val = new StringFormat(StringFormat.GenericTypographic);
			try
			{
				val.FormatFlags = (StringFormatFlags)4096;
				if (d.HotKeyPrefixVisible || !m_EnablePrefixHandling)
				{
					val.HotkeyPrefix = (HotkeyPrefix)1;
				}
				else
				{
					val.HotkeyPrefix = (HotkeyPrefix)2;
				}
				if (m_TrailingSpace)
				{
					if (d.CurrentFont.Italic)
					{
						Size size = Size.Ceiling(d.Graphics.MeasureString(m_Text, d.CurrentFont, 0, val));
						size.Width += (int)(d.Graphics.MeasureString("|", d.CurrentFont).Width / 4f);
						if (PadText)
						{
							size.Width += BarUtilities.TextMarkupCultureSpecificPadding;
							size.Height += BarUtilities.TextMarkupCultureSpecificPadding;
						}
						base.Bounds = new Rectangle(Point.Empty, size);
					}
					else
					{
						Size size2 = Size.Ceiling(d.Graphics.MeasureString(m_Text + "|", d.CurrentFont, 0, val));
						if (PadText)
						{
							size2.Width += BarUtilities.TextMarkupCultureSpecificPadding;
							size2.Height += BarUtilities.TextMarkupCultureSpecificPadding;
						}
						base.Bounds = new Rectangle(Point.Empty, size2);
					}
				}
				else
				{
					Size size3 = Size.Ceiling(d.Graphics.MeasureString(m_Text, d.CurrentFont, 0, val));
					if (PadText)
					{
						size3.Width += BarUtilities.TextMarkupCultureSpecificPadding;
						size3.Height += BarUtilities.TextMarkupCultureSpecificPadding;
					}
					base.Bounds = new Rectangle(Point.Empty, size3);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		IsSizeValid = true;
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!d.ClipRectangle.IsEmpty && !bounds.IntersectsWith(d.ClipRectangle))
		{
			return;
		}
		Graphics graphics = d.Graphics;
		if (BarUtilities.UseTextRenderer)
		{
			eTextFormat eTextFormat = eTextFormat.NoClipping | eTextFormat.NoPadding;
			if (d.RightToLeft)
			{
				eTextFormat |= eTextFormat.RightToLeft;
			}
			if (!d.HotKeyPrefixVisible)
			{
				eTextFormat |= eTextFormat.HidePrefix;
			}
			if (!d.ClipRectangle.IsEmpty && bounds.Right > d.ClipRectangle.Right)
			{
				eTextFormat |= eTextFormat.EndEllipsis;
				bounds.Width -= bounds.Right - d.ClipRectangle.Right;
			}
			TextDrawing.DrawString(graphics, m_Text, d.CurrentFont, d.CurrentForeColor, bounds, eTextFormat);
		}
		else
		{
			StringFormat val = new StringFormat(StringFormat.GenericTypographic);
			try
			{
				val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
				if (d.RightToLeft)
				{
					val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
				}
				if (d.HotKeyPrefixVisible)
				{
					val.HotkeyPrefix = (HotkeyPrefix)1;
				}
				else
				{
					val.HotkeyPrefix = (HotkeyPrefix)2;
				}
				if (!d.ClipRectangle.IsEmpty && bounds.Right > d.ClipRectangle.Right)
				{
					val.Trimming = (StringTrimming)3;
					bounds.Width -= bounds.Right - d.ClipRectangle.Right;
				}
				SolidBrush val2 = new SolidBrush(d.CurrentForeColor);
				try
				{
					graphics.DrawString(m_Text, d.CurrentFont, (Brush)(object)val2, (RectangleF)bounds, val);
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
		}
		if (d.StrikeOut)
		{
			float num = (float)d.CurrentFont.FontFamily.GetCellDescent(d.CurrentFont.Style) * d.CurrentFont.Size / (float)d.CurrentFont.FontFamily.GetEmHeight(d.CurrentFont.Style) + 1f;
			Pen val3 = new Pen(d.CurrentForeColor, 1f);
			try
			{
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				float num2 = (float)bounds.Top + ((float)bounds.Height + num) / 2f;
				graphics.DrawLine(val3, (float)bounds.X, num2, (float)(bounds.Right - 1), num2);
				graphics.SmoothingMode = smoothingMode;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if ((d.HyperLink && (d.HyperlinkStyle == null || d.HyperlinkStyle.UnderlineStyle != 0)) || d.Underline)
		{
			float num3 = (float)d.CurrentFont.FontFamily.GetCellDescent(d.CurrentFont.Style) * d.CurrentFont.Size / (float)d.CurrentFont.FontFamily.GetEmHeight(d.CurrentFont.Style);
			Pen val4 = new Pen(d.CurrentForeColor, 1f);
			try
			{
				if (d.HyperLink && d.HyperlinkStyle != null && d.HyperlinkStyle.UnderlineStyle == eHyperlinkUnderlineStyle.DashedLine)
				{
					val4.DashStyle = (DashStyle)1;
				}
				num3 -= 1f;
				SmoothingMode smoothingMode2 = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)0;
				graphics.DrawLine(val4, (float)bounds.X, (float)bounds.Bottom - num3, (float)(bounds.Right - 1), (float)bounds.Bottom - num3);
				graphics.SmoothingMode = smoothingMode2;
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		base.RenderBounds = bounds;
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
