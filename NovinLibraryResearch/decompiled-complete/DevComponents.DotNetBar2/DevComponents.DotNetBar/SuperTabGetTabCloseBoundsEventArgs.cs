using System.Drawing;

namespace DevComponents.DotNetBar;

public class SuperTabGetTabCloseBoundsEventArgs : SuperTabGetTabImageBoundsEventArgs
{
	public SuperTabGetTabCloseBoundsEventArgs(SuperTabItem tab, Rectangle bounds)
		: base(tab, bounds)
	{
	}
}
