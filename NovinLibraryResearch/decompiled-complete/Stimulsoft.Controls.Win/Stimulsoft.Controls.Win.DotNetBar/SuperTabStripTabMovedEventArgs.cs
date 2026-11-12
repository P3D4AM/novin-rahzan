using System.Collections.Generic;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripTabMovedEventArgs : SuperTabStripTabActionCancelEventArgs
{
	private List<BaseItem> _OldOrder;

	private List<BaseItem> _NewOrder;

	public List<BaseItem> OldOrder => _OldOrder;

	public List<BaseItem> NewOrder
	{
		get
		{
			return _NewOrder;
		}
		set
		{
			_NewOrder = value;
		}
	}

	public SuperTabStripTabMovedEventArgs(BaseItem tab, List<BaseItem> oldOrder, List<BaseItem> newOrder)
		: base(tab)
	{
		_OldOrder = oldOrder;
		_NewOrder = newOrder;
	}
}
