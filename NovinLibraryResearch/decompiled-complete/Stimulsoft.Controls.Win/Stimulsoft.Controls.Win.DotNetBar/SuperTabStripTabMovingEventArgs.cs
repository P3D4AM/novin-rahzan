using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabStripTabMovingEventArgs : EventArgs
{
	private BaseItem _MoveTab;

	private BaseItem _InsertTab;

	private bool _InsertBefore;

	private bool _CanMove = true;

	public BaseItem MoveTab => _MoveTab;

	public BaseItem InsertTab => _InsertTab;

	public bool InsertBefore => _InsertBefore;

	public bool CanMove
	{
		get
		{
			return _CanMove;
		}
		set
		{
			_CanMove = value;
		}
	}

	public SuperTabStripTabMovingEventArgs(BaseItem moveTab, BaseItem insertTab, bool insertBefore)
	{
		_MoveTab = moveTab;
		_InsertTab = insertTab;
		_InsertBefore = insertBefore;
	}
}
