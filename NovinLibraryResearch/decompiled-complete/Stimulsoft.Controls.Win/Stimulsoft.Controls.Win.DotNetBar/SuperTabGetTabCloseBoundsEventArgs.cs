using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabGetTabCloseBoundsEventArgs : SuperTabGetTabImageBoundsEventArgs
{
	public SuperTabGetTabCloseBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
		: base(tab, bounds)
	{
	}
}
