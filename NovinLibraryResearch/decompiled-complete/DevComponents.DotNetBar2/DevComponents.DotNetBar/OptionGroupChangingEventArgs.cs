using System;

namespace DevComponents.DotNetBar;

public class OptionGroupChangingEventArgs : EventArgs
{
	public bool Cancel;

	public readonly BaseItem NewChecked;

	public readonly BaseItem OldChecked;

	public OptionGroupChangingEventArgs(BaseItem oldchecked, BaseItem newchecked)
	{
		NewChecked = newchecked;
		OldChecked = oldchecked;
	}
}
