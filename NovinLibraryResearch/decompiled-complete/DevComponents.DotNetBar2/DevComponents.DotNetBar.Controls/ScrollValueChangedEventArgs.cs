using System;

namespace DevComponents.DotNetBar.Controls;

public class ScrollValueChangedEventArgs : EventArgs
{
	public readonly eScrollBarScrollChange ScrollChange;

	public ScrollValueChangedEventArgs(eScrollBarScrollChange scrollChange)
	{
		ScrollChange = scrollChange;
	}
}
