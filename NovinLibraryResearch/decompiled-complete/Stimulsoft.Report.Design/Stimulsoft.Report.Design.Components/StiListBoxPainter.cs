using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Components;

public static class StiListBoxPainter
{
	public static int CloseButtonWidth = StiScale.XXI(32.0);

	private const int GWL_STYLE = -16;

	private const int WS_VSCROLL = 2097152;

	private const int WM_VSCROLL = 277;

	public static void PaintBorder(Graphics g, Rectangle rect, int itemsCount, bool isDragOverFromDictionary = false)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		StiDrawing.FillRectangle(g, StiUX.BackgroundDark, rect);
		if (isDragOverFromDictionary)
		{
			Color color = (StiUX.IsDark ? StiUX.Accent : Color.FromArgb(200, StiElementConsts.Highlight.Color));
			Pen val = new Pen(color, 2f);
			try
			{
				if (!StiUX.IsDark)
				{
					val.DashStyle = (DashStyle)1;
				}
				val.Alignment = (PenAlignment)1;
				g.DrawRectangle(val, rect);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		Pen val2 = new Pen(StiUX.ButtonBorder);
		try
		{
			if (!StiUX.IsDark)
			{
				val2.DashStyle = (DashStyle)(itemsCount == 0);
			}
			g.DrawRectangle(val2, 0, 0, rect.Width - 1, rect.Height - 1);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static void PaintCloseButton(Graphics g, Rectangle rect, bool isFirstItem, bool isMouseOver)
	{
		Rectangle rectangle = new Rectangle(0, rect.Y, rect.Width, rect.Height);
		Image val = StiReportDesignImages.SystemParts.Remove(StiImageSize.Normal, allowCache: false);
		try
		{
			Rectangle rect2 = new Rectangle(rectangle.Right - CloseButtonWidth - 1, rectangle.Y, CloseButtonWidth, rectangle.Height);
			if (isFirstItem)
			{
				rect2.Y++;
				rect2.Height--;
			}
			Rectangle rectangle2 = new Rectangle(rect2.X + (rect2.Width - val.Width) / 2, rect2.Y + (rect2.Height - val.Height) / 2, val.Width, val.Height);
			rect2.Inflate(-StiScale.I3, -StiScale.I3);
			Color color = StiUX.ButtonBackground;
			if (isMouseOver)
			{
				color = (StiUX.IsDark ? StiUX.ButtonBackgroundHover : Color.LightGray);
			}
			StiDrawing.FillRectangle(g, color, rect2);
			if (StiUX.IsDark && isMouseOver)
			{
				StiDrawing.FillRectangle(g, StiUX.Accent.ChangeAlpha(50), rect2);
				rect.Width--;
				rect.Height--;
				StiDrawing.DrawRectangle(g, StiUX.Accent, rect2);
			}
			g.DrawImage(val, rectangle2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void PaintLabel(Graphics g, Rectangle rect, string text)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		rect.Inflate(-StiScale.I10, -StiScale.I10);
		StringFormat val = new StringFormat();
		try
		{
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
			try
			{
				SolidBrush val2 = new SolidBrush(StiUX.IsDark ? StiUX.InputForegroundHint : StiElementConsts.DragDrop.Font.Color);
				try
				{
					val.LineAlignment = (StringAlignment)1;
					val.Alignment = (StringAlignment)1;
					g.DrawString(text, gdiFont, (Brush)(object)val2, (RectangleF)rect, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static Color GetBackgroundColor(bool isSelected, bool isHotLight, bool isFocused)
	{
		if (isSelected && isHotLight)
		{
			return StiColorUtils.Light(StiUX.IsDark ? StiUX.ItemBackgroundSelected : SystemColors.MenuHighlight, 30);
		}
		if (isSelected && isFocused)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.MenuHighlight;
			}
			return StiUX.ItemBackgroundSelected;
		}
		if (isSelected && !isFocused)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.InactiveCaption;
			}
			return StiUX.ItemBackgroundInactive;
		}
		if (isHotLight)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#e0ecff");
			}
			return StiUX.ItemBackgroundHover;
		}
		if (!StiUX.IsDark)
		{
			return Color.White;
		}
		return StiUX.ItemBackground;
	}

	public static Color GetForegroundColor(bool isSelected, bool isHotLight, bool isFocused, Color? baseColor = null)
	{
		if (isSelected && isHotLight)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.HighlightText;
			}
			return StiUX.ItemForegroundSelected;
		}
		if (isSelected && isFocused)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.HighlightText;
			}
			return StiUX.ItemForegroundSelected;
		}
		if (isSelected && !isFocused)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.InactiveCaptionText;
			}
			return StiUX.ItemForegroundInactive;
		}
		if (isHotLight)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.ControlText;
			}
			return StiUX.ItemForegroundHover;
		}
		if (baseColor.HasValue)
		{
			return baseColor.Value;
		}
		if (!StiUX.IsDark)
		{
			return Color.DimGray;
		}
		return StiUX.ItemForeground;
	}

	public static bool IsVerticalScrollbarVisible(Control control)
	{
		if (!control.IsHandleCreated)
		{
			return false;
		}
		int windowLong = GetWindowLong(control.Handle, -16);
		return (windowLong & 0x200000) != 0;
	}

	public static bool IsVerticalScrollbar(Message message)
	{
		return ((Message)(ref message)).Msg == 277;
	}

	[DllImport("user32", CharSet = CharSet.Auto)]
	private static extern int GetWindowLong(IntPtr hwnd, int nIndex);
}
