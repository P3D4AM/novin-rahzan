using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class SuperTabStripTabActionCancelEventArgs : CancelEventArgs
{
	private BaseItem _Ὴ;

	public BaseItem Tab => _Ὴ;

	public SuperTabStripTabActionCancelEventArgs(BaseItem tab)
	{
		_Ὴ = tab;
	}
}
