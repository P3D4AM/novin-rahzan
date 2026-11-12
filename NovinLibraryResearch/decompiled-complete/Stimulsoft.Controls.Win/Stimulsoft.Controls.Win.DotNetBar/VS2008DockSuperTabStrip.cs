using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class VS2008DockSuperTabStrip : SuperTabStripBaseDisplay
{
	internal override Size TabLayoutOffset => new Size(3, 2);

	public VS2008DockSuperTabStrip(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}
}
