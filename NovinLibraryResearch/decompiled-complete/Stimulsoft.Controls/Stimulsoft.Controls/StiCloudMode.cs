using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

internal sealed class StiCloudMode
{
	internal int titleSpace = StiScaleUI.I3;

	private StiCloudTabulator tabulator;

	public void DrawTitleImage(Graphics g, Rectangle rect, StiCloudTabulatorPage page)
	{
		Rectangle titleImageRectangle = GetTitleImageRectangle(rect, page);
		if (!titleImageRectangle.IsEmpty && page.Image != null)
		{
			g.DrawImage(page.Image, titleImageRectangle.X, titleImageRectangle.Y, titleImageRectangle.Width, titleImageRectangle.Height);
		}
	}

	public void DrawMarker(Graphics g, Rectangle rect, StiCloudTabulatorPage page)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (!page.ShowMarker)
		{
			return;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			SolidBrush val = new SolidBrush(StiColor.Get("2276d5"));
			try
			{
				Rectangle rectangle = new Rectangle((tabulator.TitlePosition == StiTabTitlePosition.LeftHorizontal) ? (rect.Right - StiScaleUI.I(22)) : (rect.X + StiScaleUI.I10), rect.Y + (rect.Height - StiScaleUI.I12) / 2, StiScaleUI.I12, StiScaleUI.I12);
				g.SmoothingMode = (SmoothingMode)4;
				g.FillEllipse(Brushes.White, rectangle);
				rectangle.Inflate(-StiScaleUI.I2, -StiScaleUI.I2);
				g.FillEllipse((Brush)(object)val, rectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public Size MeasureTitle(Graphics g, StiCloudTabulatorPage page)
	{
		SizeF sizeF = g.MeasureString(((Control)page).Text, ((Control)tabulator).Font);
		Size result = MeasureWrappedString(g, ((Control)tabulator).Font, ((Control)page).Text, tabulator.TitleWidth);
		sizeF.Width += StiScaleUI.I6;
		sizeF.Height += StiScaleUI.I6;
		result.Width += StiScaleUI.I6;
		result.Height += StiScaleUI.I6;
		Rectangle titleImageRectangle = GetTitleImageRectangle(Rectangle.Empty, page);
		if (tabulator.TitleInLine)
		{
			if (sizeF.Width + (float)titleImageRectangle.Width < (float)tabulator.TitleWidth)
			{
				result.Width = (int)(sizeF.Width + (float)titleImageRectangle.Width);
				result.Height = (int)(sizeF.Height + (float)titleImageRectangle.Height);
			}
			else
			{
				result.Width += titleImageRectangle.Width;
				result.Height += titleImageRectangle.Height;
			}
			if (tabulator.Win11Look)
			{
				result.Height = StiScale.I(28);
			}
		}
		else
		{
			result.Width = Math.Max(result.Width, titleImageRectangle.Width + StiScaleUI.I6);
			result.Height += titleImageRectangle.Height + StiScaleUI.I6;
		}
		return result;
	}

	private Size MeasureWrappedString(Graphics g, Font font, string str, int maxWidth, int lineSpacing = 0)
	{
		SizeF sizeF = g.MeasureString(str, font);
		string text = "";
		float height = sizeF.Height;
		int num = (int)height;
		foreach (char c in str)
		{
			if (g.MeasureString(text, font).Width >= (float)maxWidth)
			{
				num += (int)(height + (float)lineSpacing);
				text = "";
			}
			text += c;
		}
		return new Size(maxWidth, num);
	}

	public void DrawTitleText(Graphics g, Rectangle rect, StiCloudTabulatorPage page)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		Rectangle titleTextRectangle = GetTitleTextRectangle(rect, page);
		StringFormat val = new StringFormat
		{
			HotkeyPrefix = (HotkeyPrefix)1,
			LineAlignment = (StringAlignment)1,
			Alignment = (StringAlignment)(!tabulator.TitleInLine),
			Trimming = (StringTrimming)3
		};
		if (tabulator.TitleInLine && tabulator.Win11Look && page.Image == null)
		{
			titleTextRectangle.X += StiScaleUI.I10;
			titleTextRectangle.Width -= StiScaleUI.I10;
		}
		SolidBrush val2 = new SolidBrush(StiUX.ItemForeground);
		try
		{
			g.DrawString(((Control)page).Text, ((Control)tabulator).Font, (Brush)(object)val2, (RectangleF)titleTextRectangle, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public Rectangle GetTitleImageRectangle(Rectangle rect, StiCloudTabulatorPage page)
	{
		if (page.Image == null)
		{
			return Rectangle.Empty;
		}
		Size size = page.Image.Size;
		int num = StiScaleUI.I4;
		if (tabulator.TitleInLine)
		{
			num = StiScaleUI.I10;
		}
		else if (rect.Width > 0)
		{
			num = (rect.Width - size.Width) / 2;
		}
		if (tabulator.Win11Look)
		{
			rect.Y += StiScaleUI.I6;
		}
		else
		{
			rect.Y += StiScaleUI.I10;
		}
		return new Rectangle(rect.X + num, rect.Y, size.Width, size.Height);
	}

	public Rectangle GetTitleTextRectangle(Rectangle rect, StiCloudTabulatorPage page)
	{
		Rectangle titleImageRectangle = GetTitleImageRectangle(rect, page);
		if (titleImageRectangle.IsEmpty)
		{
			return rect;
		}
		if (tabulator.TitleInLine)
		{
			rect.X += titleImageRectangle.Right + StiScaleUI.I10;
		}
		else
		{
			rect.Y += titleImageRectangle.Height + StiScaleUI.I4;
			rect.Height -= titleImageRectangle.Height;
		}
		return rect;
	}

	public StiCloudMode(StiCloudTabulator tabulator)
	{
		this.tabulator = tabulator;
	}
}
