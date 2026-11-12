using System;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

public class ScrollValueChangedEventArgs : EventArgs
{
	public readonly eScrollBarScrollChange ScrollChange;

	public ScrollValueChangedEventArgs(eScrollBarScrollChange scrollChange)
	{
		ScrollChange = scrollChange;
	}
}
