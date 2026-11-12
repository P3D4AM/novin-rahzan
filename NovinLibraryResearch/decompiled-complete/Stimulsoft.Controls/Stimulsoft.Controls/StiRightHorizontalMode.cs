using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public class StiRightHorizontalMode : StiTabulatorMode
{
	public override StiTabTitlePosition TabTitlePosition => StiTabTitlePosition.RightHorizontal;

	public override Rectangle GetTitlePageRectangle(StiTabulatorPage page)
	{
		int num = 0;
		foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)tabulator).Controls)
		{
			if (!item.Invisible || tabulator.IsDesignMode)
			{
				if (item == page)
				{
					break;
				}
				num++;
			}
		}
		return new Rectangle(((Control)tabulator).Width - tabulator.MaxTitleWidth, num * tabulator.MaxTitleHeight, tabulator.MaxTitleWidth - titleSpace, tabulator.MaxTitleHeight);
	}

	public override Rectangle GetTitleRectangle()
	{
		if (((ArrangedElementCollection)((Control)tabulator).Controls).Count > 0)
		{
			return new Rectangle(((Control)tabulator).Width - tabulator.MaxTitleWidth - 2 * titleSpace, 0, tabulator.MaxTitleWidth + 2 * titleSpace, ((Control)tabulator).Height);
		}
		return new Rectangle(0, 0, ((Control)tabulator).Width, ((Control)tabulator).Height);
	}

	public override Rectangle GetClientRectangle()
	{
		return new Rectangle(0, 0, ((Control)tabulator).Width - tabulator.MaxTitleWidth - 2 * titleSpace, ((Control)tabulator).Height);
	}

	public override void SetLayout()
	{
		tabulator.DockPadding.Right = ((tabulator.IsDesignMode || tabulator.ShowTitle) ? (tabulator.MaxTitleWidth + titleSpace) : 0);
		tabulator.DockPadding.Left = 0;
		tabulator.DockPadding.Top = 0;
		tabulator.DockPadding.Bottom = 0;
	}

	public override Size MeasureTitle(Graphics g, StiTabulatorPage page)
	{
		Size result = MeasureWrappedString(g, ((Control)tabulator).Font, ((Control)page).Text, StiScaleUI.XXI(95.0));
		result.Width += StiScaleUI.I6;
		result.Height += StiScaleUI.I6;
		Rectangle titleImageRectangle = GetTitleImageRectangle(Rectangle.Empty, page);
		result.Width = Math.Max(result.Width, titleImageRectangle.Width + StiScaleUI.I6);
		result.Height += titleImageRectangle.Height + StiScaleUI.I6;
		if (tabulator.TitleWidth > 0)
		{
			result.Width = tabulator.TitleWidth;
		}
		if (tabulator.TitleHeight > 0)
		{
			result.Height = tabulator.TitleHeight;
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

	public override void DrawTitleText(Graphics g, Rectangle rect, StiTabulatorPage page, bool isSelected)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		Rectangle titleTextRectangle = GetTitleTextRectangle(rect, page);
		StringFormat val = new StringFormat
		{
			HotkeyPrefix = (HotkeyPrefix)1,
			LineAlignment = (StringAlignment)1,
			Alignment = (StringAlignment)1,
			Trimming = (StringTrimming)3
		};
		Color color = ((!StiUX.IsDark) ? SystemColors.ControlText : (isSelected ? StiUX.ItemForegroundSelected : StiUX.ItemForeground));
		SolidBrush val2 = new SolidBrush(color);
		try
		{
			g.DrawString(((Control)page).Text, ((Control)tabulator).Font, (Brush)(object)val2, (RectangleF)titleTextRectangle, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public override Rectangle GetTitleImageRectangle(Rectangle rect, StiTabulatorPage page)
	{
		if (tabulator.ImageList == null && page.Image == null)
		{
			return Rectangle.Empty;
		}
		if (tabulator.ImageList == null && page.Image == null)
		{
			return Rectangle.Empty;
		}
		Size size = ((page.Image != null) ? page.Image.Size : tabulator.ImageList.ImageSize);
		int num = StiScaleUI.I4;
		if (rect.Width > 0)
		{
			num = (rect.Width - size.Width) / 2;
		}
		return new Rectangle(rect.X + num, rect.Y + StiScaleUI.XXI(10.0), size.Width, size.Height);
	}

	public override Rectangle GetTitleTextRectangle(Rectangle rect, StiTabulatorPage page)
	{
		Rectangle titleImageRectangle = GetTitleImageRectangle(rect, page);
		if (titleImageRectangle.IsEmpty)
		{
			return rect;
		}
		rect.Y += titleImageRectangle.Height + StiScaleUI.I4;
		rect.Height -= titleImageRectangle.Height;
		return rect;
	}

	public StiRightHorizontalMode(StiTabulator tabulator)
		: base(tabulator)
	{
	}
}
