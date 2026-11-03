using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Helpers;

public static class StiImageDialogPainter
{
	public static void Paint(Control control, Graphics g, bool drawDragDrop, bool drawBorder, bool isImage, bool drawBackground = true)
	{
		Paint(control, g, new byte[0], drawDragDrop, drawBorder, isImage, drawBackground);
	}

	public static void Paint(Control control, Graphics g, Image image, bool drawDragDrop, bool drawBorder, bool isImage, bool drawBackground = true)
	{
		Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
		Paint(rect, g, image, drawDragDrop, drawBorder, isImage, drawBackground);
	}

	public static void Paint(Control control, Graphics g, byte[] image, bool drawDragDrop, bool drawBorder, bool isImage, bool drawBackground = true)
	{
		Image val = StiImageConverter.TryBytesToImage(image, control.Width, control.Height, true, false);
		try
		{
			Paint(control, g, val, drawDragDrop, drawBorder, isImage, drawBackground);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void Paint(Rectangle rect, Graphics g, Image image, bool drawDragDrop, bool drawBorder, bool isImage, bool drawBackground = true)
	{
		try
		{
			if (drawBackground)
			{
				DrawBackground(g);
			}
			if (image == null && drawDragDrop)
			{
				DrawDragDropLabel(g, rect, isImage);
			}
			if (image != null)
			{
				DrawImage(g, image, rect);
			}
			if (drawBorder)
			{
				DrawBorder(g, rect);
			}
		}
		catch
		{
		}
	}

	private static void DrawDragDropLabel(Graphics g, Rectangle rect, bool isImage)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		string text = (isImage ? StiLocalization.Get("FormDictionaryDesigner", "TextDropImageHere") : StiLocalization.Get("FormDictionaryDesigner", "TextDropFileHere"));
		rect.Inflate(-10, -10);
		Font val = new Font("Arial", 8f);
		try
		{
			StringFormat val2 = new StringFormat
			{
				Alignment = (StringAlignment)1,
				LineAlignment = (StringAlignment)1
			};
			try
			{
				g.DrawString(text, val, Brushes.DimGray, (RectangleF)rect, val2);
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

	public static void DrawImage(Graphics g, Image image, Rectangle rect)
	{
		Rectangle rectangle = rect;
		rect.Inflate(-2, -2);
		if (image.Width < rect.Width && image.Height < rect.Height)
		{
			g.DrawImage(image, (rectangle.Width - image.Width) / 2, (rectangle.Height - image.Height) / 2, image.Width, image.Height);
			return;
		}
		float num = (float)rect.Width / (float)image.Width;
		float num2 = (float)rect.Height / (float)image.Height;
		if (num < num2)
		{
			float num3 = (float)image.Width * num;
			float num4 = (float)image.Height * num;
			g.DrawImage(image, ((float)rectangle.Width - num3) / 2f, ((float)rectangle.Height - num4) / 2f, num3, num4);
		}
		else
		{
			float num5 = (float)image.Width * num2;
			float num6 = (float)image.Height * num2;
			g.DrawImage(image, ((float)rectangle.Width - num5) / 2f, ((float)rectangle.Height - num6) / 2f, num5, num6);
		}
	}

	private static void DrawBackground(Graphics g)
	{
		g.Clear(StiUX.IsDark ? StiUX.BackgroundDark : Color.FromArgb(249, 249, 249));
	}

	private static void DrawBorder(Graphics g, Rectangle rect)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		Pen val = new Pen(StiUX.IsDark ? StiUX.ButtonBorder : SystemColors.ControlDark);
		try
		{
			if (!StiUX.IsDark)
			{
				val.DashStyle = (DashStyle)1;
			}
			g.DrawRectangle(val, 0, 0, rect.Width - 1, rect.Height - 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
