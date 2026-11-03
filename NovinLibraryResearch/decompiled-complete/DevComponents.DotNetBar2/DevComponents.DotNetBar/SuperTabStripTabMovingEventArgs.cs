using System;

namespace DevComponents.DotNetBar;

public class SuperTabStripTabMovingEventArgs : EventArgs
{
	private BaseItem _Ή;

	private BaseItem _ᾙ;

	private bool _ᾚ;

	private bool __0BC3 = true;

	public BaseItem MoveTab => _Ή;

	public BaseItem InsertTab => _ᾙ;

	public bool InsertBefore => _ᾚ;

	public bool CanMove
	{
		get
		{
			return __0BC3;
		}
		set
		{
			__0BC3 = value;
		}
	}

	public SuperTabStripTabMovingEventArgs(BaseItem moveTab, BaseItem insertTab, bool insertBefore)
	{
		_Ή = moveTab;
		_ᾙ = insertTab;
		_ᾚ = insertBefore;
	}
}
