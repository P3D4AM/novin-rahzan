using System;

namespace DevComponents.DotNetBar;

public class BarStateChangedEventArgs : EventArgs
{
	public readonly Bar Bar;

	public readonly eBarStateChange AffectedState;

	public object ContextInformation;

	public BarStateChangedEventArgs(Bar bar, eBarStateChange affectedState)
	{
		Bar = bar;
		AffectedState = affectedState;
	}

	public BarStateChangedEventArgs(Bar bar, eBarStateChange affectedState, object contextInformation)
	{
		Bar = bar;
		AffectedState = affectedState;
		ContextInformation = contextInformation;
	}
}
