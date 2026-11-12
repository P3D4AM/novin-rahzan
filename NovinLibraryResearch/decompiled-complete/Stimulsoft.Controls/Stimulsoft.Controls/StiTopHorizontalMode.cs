using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls;

public class StiTopHorizontalMode : StiTabulatorMode
{
	public override StiTabTitlePosition TabTitlePosition => StiTabTitlePosition.TopHorizontal;

	public override Rectangle GetTitlePageRectangle(StiTabulatorPage page)
	{
		int num = startPosTitle;
		foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)page).Parent.Controls)
		{
			if (item == page)
			{
				break;
			}
			if (!item.Invisible || tabulator.IsDesignMode)
			{
				num += item.TitleSize.Width;
			}
		}
		return new Rectangle(num, titleSpace, page.TitleSize.Width, tabulator.MaxTitleHeight);
	}

	public override Rectangle GetTitleRectangle()
	{
		if (((ArrangedElementCollection)((Control)tabulator).Controls).Count > 0)
		{
			return new Rectangle(0, 0, ((Control)tabulator).Width, tabulator.MaxTitleHeight + titleSpace);
		}
		return new Rectangle(0, 0, ((Control)tabulator).Width, ((Control)tabulator).Height);
	}

	public override Rectangle GetClientRectangle()
	{
		return new Rectangle(0, tabulator.MaxTitleHeight, ((Control)tabulator).Width, ((Control)tabulator).Height - tabulator.MaxTitleHeight);
	}

	public override void SetLayout()
	{
		if (tabulator.IsDesignMode || tabulator.ShowTitle)
		{
			tabulator.DockPadding.Top = tabulator.MaxTitleHeight + titleSpace;
		}
		else
		{
			tabulator.DockPadding.Top = 0;
		}
		tabulator.DockPadding.Left = 0;
		tabulator.DockPadding.Right = 0;
		tabulator.DockPadding.Bottom = 0;
	}

	public override void MeasureTitles()
	{
		base.MeasureTitles();
	}

	public StiTopHorizontalMode(StiTabulator tabulator)
		: base(tabulator)
	{
	}
}
