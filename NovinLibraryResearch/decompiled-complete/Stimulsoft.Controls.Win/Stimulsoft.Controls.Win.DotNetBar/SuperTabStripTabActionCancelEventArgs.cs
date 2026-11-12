using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripTabActionCancelEventArgs : CancelEventArgs
{
	private BaseItem _Tab;

	public BaseItem Tab => _Tab;

	public SuperTabStripTabActionCancelEventArgs(BaseItem tab)
	{
		_Tab = tab;
	}
}
