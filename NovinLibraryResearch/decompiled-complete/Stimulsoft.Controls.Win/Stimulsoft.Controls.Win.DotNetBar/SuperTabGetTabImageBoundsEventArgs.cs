using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabGetTabImageBoundsEventArgs : SuperTabGetTabTextBoundsEventArgs
{
	public SuperTabGetTabImageBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
		: base(tab, bounds)
	{
	}
}
