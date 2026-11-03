using System;

namespace DevComponents.DotNetBar.Controls;

public class AlertClosedEventArgs : EventArgs
{
	public readonly eAlertClosureSource ClosureSource;

	public AlertClosedEventArgs(eAlertClosureSource source)
	{
		ClosureSource = source;
	}
}
