using System.Collections.Generic;

namespace DevComponents.DotNetBar;

public class SuperTabStripTabMovedEventArgs : SuperTabStripTabActionCancelEventArgs
{
	private List<BaseItem> _ῌ;

	private List<BaseItem> __1FCD;

	public List<BaseItem> OldOrder => _ῌ;

	public List<BaseItem> NewOrder
	{
		get
		{
			return __1FCD;
		}
		set
		{
			__1FCD = value;
		}
	}

	public SuperTabStripTabMovedEventArgs(BaseItem tab, List<BaseItem> oldOrder, List<BaseItem> newOrder)
		: base(tab)
	{
		_ῌ = oldOrder;
		__1FCD = newOrder;
	}
}
