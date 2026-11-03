using System;

namespace Stimulsoft.Report.Gauge.Events;

public class StiGetValueEventArgs : EventArgs
{
	public virtual object Value { get; set; }
}
