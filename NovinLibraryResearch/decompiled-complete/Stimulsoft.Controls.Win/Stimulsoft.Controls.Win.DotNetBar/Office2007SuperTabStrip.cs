using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007SuperTabStrip : SuperTabStripBaseDisplay
{
	internal override Size TabLayoutOffset => new Size(3, 2);

	public Office2007SuperTabStrip(SuperTabStripItem tabStripItem)
		: base(tabStripItem)
	{
	}
}
