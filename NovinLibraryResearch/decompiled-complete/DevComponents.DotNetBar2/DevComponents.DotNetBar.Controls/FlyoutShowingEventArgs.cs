using System;

namespace DevComponents.DotNetBar.Controls;

public class FlyoutShowingEventArgs : EventArgs
{
	public FlyoutForm Flyout;

	public object Target;

	public bool Cancel;

	public FlyoutShowingEventArgs(FlyoutForm flyout, object target)
	{
		Flyout = flyout;
		Target = target;
	}
}
