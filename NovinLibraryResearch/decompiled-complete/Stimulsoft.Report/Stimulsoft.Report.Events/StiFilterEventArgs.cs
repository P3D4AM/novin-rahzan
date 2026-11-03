using System;

namespace Stimulsoft.Report.Events;

public class StiFilterEventArgs : EventArgs
{
	public virtual bool Value { get; set; }

	public StiFilterEventArgs()
		: this(value: false)
	{
	}

	public StiFilterEventArgs(bool value)
	{
		Value = value;
	}
}
