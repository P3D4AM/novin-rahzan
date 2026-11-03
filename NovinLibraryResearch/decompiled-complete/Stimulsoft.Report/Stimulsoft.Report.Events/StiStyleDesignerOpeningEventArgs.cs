using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Events;

public sealed class StiStyleDesignerOpeningEventArgs : EventArgs
{
	public List<StiCustomStyleButtonInfo> CustomAddButtons { get; set; } = new List<StiCustomStyleButtonInfo>();

}
