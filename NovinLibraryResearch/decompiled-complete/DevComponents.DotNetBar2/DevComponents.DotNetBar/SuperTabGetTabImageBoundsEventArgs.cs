using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabGetTabImageBoundsEventArgs : SuperTabGetTabTextBoundsEventArgs
{
	public SuperTabGetTabImageBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
		: base(tab, bounds)
	{
	}
}
