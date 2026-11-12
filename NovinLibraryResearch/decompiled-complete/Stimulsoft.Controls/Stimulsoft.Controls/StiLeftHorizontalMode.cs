using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls;

public class StiLeftHorizontalMode : StiTabulatorMode
{
	public override StiTabTitlePosition TabTitlePosition => StiTabTitlePosition.LeftHorizontal;

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
		return new Rectangle(titleSpace, num * tabulator.MaxTitleHeight + startPosTitle, tabulator.MaxTitleWidth, tabulator.MaxTitleHeight);
	}

	public override Rectangle GetTitleRectangle()
	{
		if (((ArrangedElementCollection)((Control)tabulator).Controls).Count > 0)
		{
			return new Rectangle(0, 0, tabulator.MaxTitleWidth + titleSpace, ((Control)tabulator).Height);
		}
		return new Rectangle(0, 0, ((Control)tabulator).Width, ((Control)tabulator).Height);
	}

	public override Rectangle GetClientRectangle()
	{
		return new Rectangle(tabulator.MaxTitleWidth, 0, ((Control)tabulator).Width - tabulator.MaxTitleWidth, ((Control)tabulator).Height);
	}

	public override void SetLayout()
	{
		if (tabulator.IsDesignMode || tabulator.ShowTitle)
		{
			tabulator.DockPadding.Left = tabulator.MaxTitleWidth + titleSpace;
		}
		else
		{
			tabulator.DockPadding.Left = 0;
		}
		tabulator.DockPadding.Right = 0;
		tabulator.DockPadding.Top = 0;
		tabulator.DockPadding.Bottom = 0;
	}

	public StiLeftHorizontalMode(StiTabulator tabulator)
		: base(tabulator)
	{
	}
}
