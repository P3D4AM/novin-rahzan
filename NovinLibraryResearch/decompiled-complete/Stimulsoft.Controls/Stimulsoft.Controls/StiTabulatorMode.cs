using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public abstract class StiTabulatorMode
{
	internal int startPosTitle = StiScaleUI.I8;

	internal int endPosTitle = StiScaleUI.I8;

	internal int titleSpace = StiScaleUI.I3;

	protected StiTabulator tabulator;

	public abstract StiTabTitlePosition TabTitlePosition { get; }

	public abstract Rectangle GetTitlePageRectangle(StiTabulatorPage page);

	public abstract Rectangle GetTitleRectangle();

	public void DrawPage(Graphics g, StiTabulatorPage page)
	{
		tabulator.Style.DrawPage(g, TabTitlePosition, page);
	}

	public void DrawPageTitle(Graphics g, StiTabulatorPage page)
	{
		tabulator.Style.DrawPageTitle(g, TabTitlePosition, page);
	}

	public abstract Rectangle GetClientRectangle();

	public abstract void SetLayout();

	public virtual Size MeasureTitle(Graphics g, StiTabulatorPage page)
	{
		Size result = g.MeasureString(((Control)page).Text, ((Control)tabulator).Font).ToSize();
		result.Width += (int)Math.Ceiling(6.0 * StiScaleUI.X);
		result.Height += (int)Math.Ceiling(6.0 * StiScaleUI.Y);
		Rectangle titleImageRectangle = GetTitleImageRectangle(Rectangle.Empty, page);
		result.Width += titleImageRectangle.Width + (int)Math.Ceiling(6.0 * StiScaleUI.X);
		result.Height = Math.Max(result.Height, titleImageRectangle.Height + (int)Math.Ceiling(6.0 * StiScaleUI.Y));
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

	public virtual Rectangle GetTitleImageRectangle(Rectangle rect, StiTabulatorPage page)
	{
		SizeF empty = SizeF.Empty;
		if (tabulator.ImageList == null && page.Image == null)
		{
			return Rectangle.Empty;
		}
		if (tabulator.ImageList == null && page.Image == null)
		{
			return Rectangle.Empty;
		}
		empty = ((page.Image != null) ? page.Image.Size : tabulator.ImageList.ImageSize);
		return new Rectangle(rect.X + StiScaleUI.I4, (int)((float)rect.Y + ((float)rect.Height - empty.Height) / 2f), (int)empty.Width, (int)empty.Height);
	}

	public virtual Rectangle GetTitleTextRectangle(Rectangle rect, StiTabulatorPage page)
	{
		Rectangle titleImageRectangle = GetTitleImageRectangle(rect, page);
		if (titleImageRectangle.IsEmpty)
		{
			return rect;
		}
		rect.Width -= titleImageRectangle.Width;
		rect.X += titleImageRectangle.Width;
		return rect;
	}

	public virtual void DrawTitleImage(Graphics g, Rectangle rect, StiTabulatorPage page)
	{
		Rectangle titleImageRectangle = GetTitleImageRectangle(rect, page);
		if (!titleImageRectangle.IsEmpty)
		{
			int pageIndex = page.PageIndex;
			if (tabulator.ImageList != null && tabulator.ImageList.Images.Count > pageIndex)
			{
				tabulator.ImageList.Draw(g, titleImageRectangle.X, titleImageRectangle.Y, titleImageRectangle.Width, titleImageRectangle.Height, pageIndex);
			}
			else if (page.Image != null)
			{
				g.DrawImage(page.Image, titleImageRectangle.X, titleImageRectangle.Y, titleImageRectangle.Width, titleImageRectangle.Height);
			}
		}
	}

	public virtual void DrawTitleText(Graphics g, Rectangle rect, StiTabulatorPage page, bool isSelected)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		Rectangle titleTextRectangle = GetTitleTextRectangle(rect, page);
		Color color = ((!StiUX.IsDark) ? SystemColors.ControlText : (isSelected ? StiUX.ItemForegroundSelected : StiUX.ItemForeground));
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.DrawString(((Control)page).Text, ((Control)tabulator).Font, (Brush)(object)val, (RectangleF)titleTextRectangle, tabulator.sfTabs);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void MeasureTitles()
	{
		foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)tabulator).Controls)
		{
			item.MeasureTitle();
		}
	}

	public StiTabulatorMode(StiTabulator tabulator)
	{
		this.tabulator = tabulator;
	}
}
