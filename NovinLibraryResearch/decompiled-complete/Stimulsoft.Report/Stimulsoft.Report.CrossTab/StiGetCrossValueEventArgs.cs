using System;

namespace Stimulsoft.Report.CrossTab;

public class StiGetCrossValueEventArgs : EventArgs
{
	public virtual object Value { get; set; }
}
