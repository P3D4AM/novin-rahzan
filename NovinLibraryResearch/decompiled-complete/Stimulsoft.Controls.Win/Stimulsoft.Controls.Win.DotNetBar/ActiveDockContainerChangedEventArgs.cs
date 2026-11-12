using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ActiveDockContainerChangedEventArgs : EventArgs
{
	public readonly DockContainerItem Item;

	public readonly bool IsActive;

	public ActiveDockContainerChangedEventArgs(DockContainerItem item, bool isActive)
	{
		Item = item;
		IsActive = isActive;
	}
}
