using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሚ : _0CBF
{
	private static bool ም;

	private string ө = "";

	private bool ሞ;

	private bool ሟ = true;

	public string ᆺ
	{
		get
		{
			return ө;
		}
		set
		{
			ө = value;
			IsSizeValid = false;
		}
	}

	public bool ማ
	{
		get
		{
			return ሞ;
		}
		set
		{
			ሞ = value;
			IsSizeValid = false;
		}
	}

	public bool ሜ
	{
		get
		{
			return ሟ;
		}
		set
		{
			ሟ = value;
		}
	}

	static ሚ()
	{
		string twoLetterISOLanguageName = Application.CurrentCulture.TwoLetterISOLanguageName;
		if (twoLetterISOLanguageName == "ja")
		{
			ም = true;
		}
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		if (BarUtilities.UseTextRenderer)
		{
			eTextFormat eTextFormat = eTextFormat.NoPadding;
			if (!d.HotKeyPrefixVisible || ሟ)
			{
				eTextFormat |= eTextFormat.HidePrefix;
			}
			Size empty = Size.Empty;
			if (ሞ)
			{
				if (d.CurrentFont.Italic)
				{
					empty = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, ө, d.CurrentFont, 0, eTextFormat));
					empty.Width += (int)(d.Graphics.MeasureString("||", d.CurrentFont).Width / 4f);
				}
				else
				{
					empty = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, ө + ((BarFunctions.IsVista && ө.Length > 0) ? "|" : "||"), d.CurrentFont, 0, eTextFormat));
				}
			}
			else
			{
				empty = Size.Ceiling(TextDrawing.MeasureString(d.Graphics, ө, d.CurrentFont, 0, eTextFormat));
			}
			if (ም)
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
				if (d.HotKeyPrefixVisible || !ሟ)
				{
					val.HotkeyPrefix = (HotkeyPrefix)1;
				}
				else
				{
					val.HotkeyPrefix = (HotkeyPrefix)2;
				}
				if (ሞ)
				{
					if (d.CurrentFont.Italic)
					{
						Size size = Size.Ceiling(d.Graphics.MeasureString(ө, d.CurrentFont, 0, val));
						size.Width += (int)(d.Graphics.MeasureString("|", d.CurrentFont).Width / 4f);
						if (ም)
						{
							size.Width += BarUtilities.TextMarkupCultureSpecificPadding;
							size.Height += BarUtilities.TextMarkupCultureSpecificPadding;
						}
						base.Bounds = new Rectangle(Point.Empty, size);
					}
					else
					{
						Size size2 = Size.Ceiling(d.Graphics.MeasureString(ө + "|", d.CurrentFont, 0, val));
						if (ም)
						{
							size2.Width += BarUtilities.TextMarkupCultureSpecificPadding;
							size2.Height += BarUtilities.TextMarkupCultureSpecificPadding;
						}
						base.Bounds = new Rectangle(Point.Empty, size2);
					}
				}
				else
				{
					Size size3 = Size.Ceiling(d.Graphics.MeasureString(ө, d.CurrentFont, 0, val));
					if (ም)
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
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
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
			TextDrawing.DrawString(graphics, ө, d.CurrentFont, d.CurrentForeColor, bounds, eTextFormat);
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
					graphics.DrawString(ө, d.CurrentFont, (Brush)(object)val2, (RectangleF)bounds, val);
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
		base._0CC0 = bounds;
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}
}
