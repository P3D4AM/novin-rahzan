using System;

namespace DevComponents.DotNetBar;

public class ListBoxAdvItemCheckEventArgs : EventArgs
{
	public bool Cancel;

	public readonly ListBoxItem Item;

	public readonly object Value;

	public ListBoxAdvItemCheckEventArgs(ListBoxItem item, object value)
	{
		Item = item;
		Value = value;
	}
}
