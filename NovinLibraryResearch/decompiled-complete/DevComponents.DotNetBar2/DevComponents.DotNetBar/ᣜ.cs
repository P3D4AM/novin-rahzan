using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

internal class ᣜ : _24B7, ᙓ
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

	public override void PaintSystemIcon(SystemCaptionItemRendererEventArgs e, bool isEnabled)
	{
		Graphics graphics = e.Graphics;
		SystemCaptionItem systemCaptionItem = e.SystemCaptionItem;
		Rectangle displayRectangle = systemCaptionItem.DisplayRectangle;
		displayRectangle.Offset((displayRectangle.Width - Dpi.Width16) / 2, (displayRectangle.Height - Dpi.Height16) / 2);
		if (systemCaptionItem.Icon == null)
		{
			return;
		}
		if (Environment.Version.Build <= 3705 && Environment.Version.Revision == 288 && Environment.Version.Major == 1 && Environment.Version.Minor == 0)
		{
			IntPtr hdc = graphics.GetHdc();
			try
			{
				_089F.DrawIconEx(hdc, displayRectangle.X, displayRectangle.Y, systemCaptionItem.Icon.Handle, displayRectangle.Width, displayRectangle.Height, 0, IntPtr.Zero, 3);
				return;
			}
			finally
			{
				graphics.ReleaseHdc(hdc);
			}
		}
		graphics.DrawIcon(systemCaptionItem.Icon, displayRectangle);
	}

	public override void PaintFormButtons(SystemCaptionItemRendererEventArgs e)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Invalid comparison between Unknown and I4
		if (e.GlassEnabled)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		SystemCaptionItem systemCaptionItem = e.SystemCaptionItem;
		Office2007SystemButtonColorTable office2007SystemButtonColorTable = ຊ.SystemButton;
		Rectangle displayRectangle = systemCaptionItem.DisplayRectangle;
		Region clip = graphics.Clip;
		Rectangle clip2 = displayRectangle;
		clip2.Height++;
		graphics.SetClip(clip2);
		Size size = systemCaptionItem.ᱼ();
		if (size.Height > displayRectangle.Height)
		{
			size = new Size(displayRectangle.Height, displayRectangle.Height);
		}
		bool enabled = systemCaptionItem.GetEnabled();
		Rectangle r = new Rectangle(displayRectangle.X, displayRectangle.Y + (displayRectangle.Height - size.Height) / 2, size.Width, size.Height);
		eDotNetBarStyle effectiveStyle = systemCaptionItem.EffectiveStyle;
		if (StyleManager.IsMetro(effectiveStyle) && systemCaptionItem.ContainerControl is MetroForm)
		{
			Form val = (Form)systemCaptionItem.ContainerControl;
			if ((int)val.FormBorderStyle == 3)
			{
				r.Inflate(0, -2);
			}
		}
		Office2007SystemButtonStateColorTable ct = office2007SystemButtonColorTable.Default;
		LinearGradientColorTable linearGradientColorTable = null;
		bool flag = systemCaptionItem.ContainerControl is MetroTabStrip && StyleManager.Style != eStyle.OfficeMobile2014;
		linearGradientColorTable = ((!flag) ? null : new LinearGradientColorTable(ຊ.Form.TextColor));
		if (systemCaptionItem.HelpVisible && (!systemCaptionItem.IsRightToLeft || (systemCaptionItem.CloseVisible && systemCaptionItem.IsRightToLeft)))
		{
			Office2007SystemButtonColorTable office2007SystemButtonColorTable2 = office2007SystemButtonColorTable;
			if (systemCaptionItem.CloseEnabled && enabled && systemCaptionItem.IsRightToLeft && ຊ.SystemButtonClose != null)
			{
				office2007SystemButtonColorTable = ຊ.SystemButtonClose;
			}
			if ((systemCaptionItem.CloseEnabled && enabled && systemCaptionItem.IsRightToLeft) || !systemCaptionItem.IsRightToLeft)
			{
				if ((systemCaptionItem.ᲀ == _1886._1C8E && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.ᲀ == _1886.ᆤ && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.Pressed;
					linearGradientColorTable = null;
				}
				else if ((systemCaptionItem._1C7F == _1886._1C8E && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem._1C7F == _1886.ᆤ && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.MouseOver;
					linearGradientColorTable = null;
				}
			}
			PaintBackground(graphics, r, ct);
			if (systemCaptionItem.IsRightToLeft)
			{
				PaintClose(graphics, r, ct, systemCaptionItem.CloseEnabled && enabled, linearGradientColorTable);
			}
			else
			{
				PaintHelp(graphics, r, ct, enabled, linearGradientColorTable);
			}
			r.Offset(r.Width + 1, 0);
			office2007SystemButtonColorTable = office2007SystemButtonColorTable2;
		}
		if ((systemCaptionItem.MinimizeVisible && systemCaptionItem.HelpVisible) || (systemCaptionItem.MinimizeVisible && !systemCaptionItem.HelpVisible && (!systemCaptionItem.IsRightToLeft || (systemCaptionItem.CloseVisible && systemCaptionItem.IsRightToLeft))))
		{
			ct = office2007SystemButtonColorTable.Default;
			linearGradientColorTable = ((!flag) ? null : new LinearGradientColorTable(ຊ.Form.TextColor));
			if ((systemCaptionItem.CloseEnabled && enabled && systemCaptionItem.IsRightToLeft) || !systemCaptionItem.IsRightToLeft)
			{
				if ((systemCaptionItem.ᲀ == _1886._1C8C && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.ᲀ == _1886.ᆤ && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.Pressed;
					linearGradientColorTable = null;
				}
				else if ((systemCaptionItem._1C7F == _1886._1C8C && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem._1C7F == _1886.ᆤ && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.MouseOver;
					linearGradientColorTable = null;
				}
			}
			PaintBackground(graphics, r, ct);
			if (systemCaptionItem.IsRightToLeft)
			{
				PaintClose(graphics, r, ct, systemCaptionItem.CloseEnabled && enabled, linearGradientColorTable);
			}
			else
			{
				PaintMinimize(graphics, r, ct, enabled, linearGradientColorTable);
			}
			r.Offset(r.Width + 1, 0);
		}
		if (systemCaptionItem.RestoreMaximizeVisible)
		{
			if (systemCaptionItem.RestoreEnabled && enabled)
			{
				ct = office2007SystemButtonColorTable.Default;
				linearGradientColorTable = ((!flag) ? null : new LinearGradientColorTable(ຊ.Form.TextColor));
				if (systemCaptionItem.ᲀ == _1886._1C8D)
				{
					ct = office2007SystemButtonColorTable.Pressed;
					linearGradientColorTable = null;
				}
				else if (systemCaptionItem._1C7F == _1886._1C8D)
				{
					ct = office2007SystemButtonColorTable.MouseOver;
					linearGradientColorTable = null;
				}
				PaintBackground(graphics, r, ct);
				PaintRestore(graphics, r, ct, enabled, linearGradientColorTable);
			}
			else
			{
				ct = office2007SystemButtonColorTable.Default;
				linearGradientColorTable = ((!flag) ? null : new LinearGradientColorTable(ຊ.Form.TextColor));
				if (systemCaptionItem.ᲀ == _1886._1C8F)
				{
					ct = office2007SystemButtonColorTable.Pressed;
					linearGradientColorTable = null;
				}
				else if (systemCaptionItem._1C7F == _1886._1C8F)
				{
					ct = office2007SystemButtonColorTable.MouseOver;
					linearGradientColorTable = null;
				}
				PaintBackground(graphics, r, ct);
				PaintMaximize(graphics, r, ct, enabled, linearGradientColorTable);
			}
			r.Offset(r.Width + 1, 0);
		}
		if ((systemCaptionItem.CloseVisible && !systemCaptionItem.IsRightToLeft) || (!systemCaptionItem.HelpVisible && systemCaptionItem.MinimizeVisible && systemCaptionItem.IsRightToLeft) || (systemCaptionItem.HelpVisible && systemCaptionItem.IsRightToLeft))
		{
			Office2007SystemButtonColorTable office2007SystemButtonColorTable3 = office2007SystemButtonColorTable;
			if (systemCaptionItem.CloseEnabled && enabled && !systemCaptionItem.IsRightToLeft && ຊ.SystemButtonClose != null)
			{
				office2007SystemButtonColorTable = ຊ.SystemButtonClose;
			}
			ct = office2007SystemButtonColorTable.Default;
			linearGradientColorTable = ((!flag) ? null : new LinearGradientColorTable(ຊ.Form.TextColor));
			if ((systemCaptionItem.CloseEnabled && enabled && !systemCaptionItem.IsRightToLeft) || systemCaptionItem.IsRightToLeft)
			{
				if ((systemCaptionItem.ᲀ == _1886.ᆤ && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem.ᲀ == _1886._1C8C && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.Pressed;
					linearGradientColorTable = null;
				}
				else if ((systemCaptionItem._1C7F == _1886.ᆤ && !systemCaptionItem.IsRightToLeft) || (systemCaptionItem._1C7F == _1886._1C8C && systemCaptionItem.IsRightToLeft))
				{
					ct = office2007SystemButtonColorTable.MouseOver;
					linearGradientColorTable = null;
				}
			}
			PaintBackground(graphics, r, ct);
			if (systemCaptionItem.IsRightToLeft)
			{
				if (systemCaptionItem.HelpVisible)
				{
					PaintHelp(graphics, r, ct, enabled, linearGradientColorTable);
				}
				else
				{
					PaintMinimize(graphics, r, ct, enabled, linearGradientColorTable);
				}
			}
			else
			{
				PaintClose(graphics, r, ct, systemCaptionItem.CloseEnabled && enabled, linearGradientColorTable);
			}
			office2007SystemButtonColorTable = office2007SystemButtonColorTable3;
		}
		graphics.Clip = clip;
		if (clip != null)
		{
			clip.Dispose();
		}
	}

	protected virtual void PaintBackground(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct)
	{
		int roundCornerSize = 2;
		Rectangle bounds = r;
		if (ct.OuterBorder != null && !ct.OuterBorder.IsEmpty)
		{
			r.Inflate(-1, -1);
		}
		Rectangle r2 = new Rectangle(r.X, r.Y, r.Width, r.Height / 2);
		if (ct.TopBackground != null && !ct.TopBackground.IsEmpty)
		{
			r2.Height++;
			if (ct.BottomBackground == null)
			{
				r2 = r;
			}
			DisplayHelp.FillRectangle(g, r2, ct.TopBackground);
			r2.Height--;
		}
		if (ct.BottomBackground != null && !ct.BottomBackground.IsEmpty)
		{
			r2.Y += r2.Height;
			r2.Height = r.Height - r2.Height;
			DisplayHelp.FillRectangle(g, r2, ct.BottomBackground);
		}
		if (ct.TopHighlight != null && !ct.TopHighlight.IsEmpty)
		{
			Rectangle ल = r;
			ल.Height /= 2;
			_24B6(g, ct.TopHighlight, ल, new PointF(ल.X + ल.Width / 2, ल.Bottom));
		}
		if (ct.BottomHighlight != null && !ct.BottomHighlight.IsEmpty)
		{
			Rectangle ल2 = r;
			ल2.Height /= 2;
			ल2.Y += r.Height - ल2.Height;
			_24B6(g, ct.BottomHighlight, ल2, new PointF(ल2.X + ल2.Width / 2, ल2.Bottom));
		}
		if (ct.OuterBorder != null && !ct.OuterBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundGradientRectangle(g, bounds, ct.OuterBorder, 1, roundCornerSize);
			bounds.Inflate(-1, -1);
		}
		if (ct.InnerBorder != null && !ct.InnerBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundGradientRectangle(g, bounds, ct.InnerBorder, 1, roundCornerSize);
		}
	}

	private void _24B6(Graphics _٥, LinearGradientColorTable _0BDE, Rectangle ल, PointF _23A7)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(ल.X, ल.Y, ल.Width, ल.Height * 2);
		GraphicsPath val = new GraphicsPath();
		val.AddEllipse(rectangle);
		PathGradientBrush val2 = new PathGradientBrush(val);
		val2.CenterColor = _0BDE.Start;
		val2.SurroundColors = new Color[1] { _0BDE.End };
		val2.CenterPoint = _23A7;
		Blend val3 = new Blend();
		val3.Factors = new float[3] { 0f, 0.5f, 1f };
		val3.Positions = new float[3] { 0f, 0.4f, 1f };
		val2.Blend = val3;
		_٥.FillRectangle((Brush)(object)val2, ल);
		((Brush)val2).Dispose();
		val.Dispose();
	}

	protected virtual void PaintMinimize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(Dpi.Width7, Dpi.Height3);
		Rectangle signRect = GetSignRect(r, s);
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right, signRect.Y, ct.DarkShade, 1);
		signRect.Offset(0, 1);
		LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right, signRect.Y, linearGradientColorTable.Start, 1);
		signRect.Offset(0, 1);
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right, signRect.Y, ct.LightShade, 1);
		g.SmoothingMode = smoothingMode;
	}

	protected virtual Rectangle GetSignRect(Rectangle r, Size s)
	{
		if (r.Height < 10)
		{
			return Rectangle.Empty;
		}
		return new Rectangle(r.X + (r.Width - s.Width) / 2, r.Bottom - r.Height / 4 - s.Height, s.Width, s.Height);
	}

	protected virtual void PaintRestore(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(Dpi.Width9, Dpi.Height8);
		Rectangle signRect = GetSignRect(r, s);
		Rectangle rectangle = new Rectangle(signRect.X, signRect.Y + Dpi.Height1, Dpi.Width7, Dpi.Height7);
		Region clip = g.Clip;
		for (int i = 0; i < 2; i++)
		{
			LinearGradientColorTable table = foregroundOverride ?? ct.Foreground;
			DisplayHelp.DrawGradientRectangle(g, new Rectangle(rectangle.X, rectangle.Y + 1, rectangle.Width, rectangle.Height - 1), table, 1);
			DisplayHelp.DrawLine(g, rectangle.X, rectangle.Y, rectangle.Right - 1, rectangle.Y, ct.DarkShade, 1);
			DisplayHelp.DrawLine(g, rectangle.X + 1, rectangle.Y + 2, rectangle.Right - 2, rectangle.Y + 2, ct.LightShade, 1);
			DisplayHelp.DrawLine(g, rectangle.X + 1, rectangle.Y + 2, rectangle.Right - 2, rectangle.Y + 2, ct.LightShade, 1);
			g.SetClip(rectangle, (CombineMode)4);
			rectangle.Offset(Dpi.Width2, -Dpi.Height1);
		}
		if (clip != null)
		{
			g.Clip = clip;
		}
		else
		{
			g.ResetClip();
		}
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintMaximize(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(Dpi.Width9, Dpi.Height8);
		Rectangle signRect = GetSignRect(r, s);
		Color color = (isEnabled ? ct.DarkShade : Color.FromArgb(128, ct.DarkShade));
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right - 1, signRect.Y, color, 1);
		signRect.Y++;
		signRect.Height--;
		LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
		color = (isEnabled ? linearGradientColorTable.Start : Color.FromArgb(128, linearGradientColorTable.Start));
		DisplayHelp.DrawLine(g, signRect.X, signRect.Y, signRect.Right - 1, signRect.Y, color, 1);
		signRect.Y++;
		signRect.Height--;
		Rectangle bounds = signRect;
		bounds.Height--;
		if (isEnabled)
		{
			DisplayHelp.DrawGradientRectangle(g, bounds, linearGradientColorTable, 1);
		}
		else
		{
			DisplayHelp.DrawGradientRectangle(g, bounds, Color.FromArgb(128, linearGradientColorTable.Start), Color.FromArgb(128, linearGradientColorTable.End), linearGradientColorTable.GradientAngle, 1);
		}
		color = (isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade));
		DisplayHelp.DrawLine(g, signRect.X, signRect.Bottom - 1, signRect.Right - 1, signRect.Bottom - 1, color, 1);
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintClose(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)0;
		Size s = new Size(Dpi.Width11, Dpi.Height9);
		Rectangle signRect = GetSignRect(r, s);
		Rectangle rectangle = signRect;
		rectangle.Inflate(-Dpi.Width1, 0);
		rectangle.Height--;
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddLine(rectangle.X, rectangle.Y, rectangle.X + Dpi.Width2, rectangle.Y);
			val.AddLine(rectangle.X + Dpi.Width2, rectangle.Y, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Width2);
			val.AddLine(rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Width2, rectangle.X + Dpi.Width6, rectangle.Y);
			val.AddLine(rectangle.X + Dpi.Width6, rectangle.Y, rectangle.X + Dpi.Width8, rectangle.Y);
			val.AddLine(rectangle.X + Dpi.Width8, rectangle.Y, rectangle.X + Dpi.Width5, rectangle.Y + Dpi.Height3);
			val.AddLine(rectangle.X + Dpi.Width5, rectangle.Y + Dpi.Height4, rectangle.X + Dpi.Width8, rectangle.Y + Dpi.Height7);
			val.AddLine(rectangle.X + Dpi.Width8, rectangle.Y + Dpi.Height7, rectangle.X + Dpi.Width6, rectangle.Y + Dpi.Height7);
			val.AddLine(rectangle.X + Dpi.Width6, rectangle.Y + Dpi.Height7, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height5);
			val.AddLine(rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Height5, rectangle.X + Dpi.Width2, rectangle.Y + Dpi.Height7);
			val.AddLine(rectangle.X + Dpi.Width2, rectangle.Y + Dpi.Height7, rectangle.X, rectangle.Y + Dpi.Height7);
			val.AddLine(rectangle.X, rectangle.Y + Dpi.Height7, rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height4);
			val.AddLine(rectangle.X + Dpi.Width3, rectangle.Y + Dpi.Height3, rectangle.X, rectangle.Y);
			LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
			if (isEnabled)
			{
				DisplayHelp.FillPath(g, val, linearGradientColorTable);
				DisplayHelp.DrawGradientPathBorder(g, val, linearGradientColorTable, 1);
			}
			else
			{
				LinearGradientColorTable table = new LinearGradientColorTable(linearGradientColorTable.Start.IsEmpty ? linearGradientColorTable.Start : Color.FromArgb(128, linearGradientColorTable.Start), linearGradientColorTable.End.IsEmpty ? linearGradientColorTable.End : Color.FromArgb(128, linearGradientColorTable.End), linearGradientColorTable.GradientAngle);
				DisplayHelp.FillPath(g, val, table);
				DisplayHelp.DrawGradientPathBorder(g, val, table, 1);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!ct.DarkShade.IsEmpty)
		{
			Pen val2 = new Pen(isEnabled ? ct.DarkShade : Color.FromArgb(128, ct.DarkShade), 1f);
			try
			{
				g.DrawLine(val2, rectangle.X, rectangle.Y, rectangle.X + Dpi.Width2, rectangle.Y);
				g.DrawLine(val2, rectangle.X + Dpi.Width2, rectangle.Y, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Width2);
				g.DrawLine(val2, rectangle.X + Dpi.Width4, rectangle.Y + Dpi.Width2, rectangle.X + Dpi.Width6, rectangle.Y);
				g.DrawLine(val2, rectangle.X + Dpi.Width6, rectangle.Y, rectangle.X + Dpi.Width8, rectangle.Y);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!ct.LightShade.IsEmpty)
		{
			Pen val3 = new Pen(isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade), 1f);
			try
			{
				g.DrawLine(val3, signRect.X, signRect.Y + Dpi.Height8, signRect.X + Dpi.Width3, signRect.Y + Dpi.Height8);
				g.DrawLine(val3, signRect.X + Dpi.Width3, signRect.Y + Dpi.Height8, signRect.X + Dpi.Width5, signRect.Y + Dpi.Height6);
				g.DrawLine(val3, signRect.X + Dpi.Width5, signRect.Y + Dpi.Height6, signRect.X + Dpi.Width7, signRect.Y + Dpi.Height8);
				g.DrawLine(val3, signRect.X + Dpi.Width7, signRect.Y + Dpi.Height8, signRect.X + Dpi.Width10, signRect.Y + Dpi.Height8);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintHelp(Graphics g, Rectangle r, Office2007SystemButtonStateColorTable ct, bool isEnabled, LinearGradientColorTable foregroundOverride)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		g.SmoothingMode = (SmoothingMode)0;
		g.TextRenderingHint = (TextRenderingHint)5;
		Font val = new Font(SystemFonts.DefaultFont, (FontStyle)1);
		try
		{
			Size s = TextDrawing.MeasureString(g, "?", val);
			s.Width += 4;
			s.Height -= 2;
			Rectangle signRect = GetSignRect(r, s);
			signRect.Offset(1, 1);
			Color color = (isEnabled ? ct.LightShade : Color.FromArgb(128, ct.LightShade));
			SolidBrush val2 = new SolidBrush(color);
			try
			{
				g.DrawString("?", val, (Brush)(object)val2, (RectangleF)signRect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			signRect.Offset(-1, -1);
			LinearGradientColorTable linearGradientColorTable = foregroundOverride ?? ct.Foreground;
			color = (isEnabled ? linearGradientColorTable.Start : Color.FromArgb(128, linearGradientColorTable.Start));
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				g.DrawString("?", val, (Brush)(object)val3, (RectangleF)signRect);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
		g.TextRenderingHint = textRenderingHint;
	}
}
