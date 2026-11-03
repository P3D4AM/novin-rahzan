using System;

namespace Stimulsoft.Report.Events;

public class StiTextChangedEventArgs : EventArgs
{
	public string OldValue { get; }

	public string NewValue { get; }

	public StiTextChangedEventArgs(string oldValue, string newValue)
	{
		OldValue = oldValue;
		NewValue = newValue;
	}
}
