using System;
using Stimulsoft.Dashboard.Components.ListBox;

namespace Stimulsoft.Dashboard.Viewer.Controls.ListBox;

public class StiListBoxItemSelectionChangedEventArgs : EventArgs
{
	public StiListBoxItem Item { get; }

	public int ItemIndex { get; }

	public StiListBoxItemSelectionChangedEventArgs(StiListBoxItem item, int itemIndex)
	{
		Item = item;
		ItemIndex = itemIndex;
	}
}
