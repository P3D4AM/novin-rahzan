using System;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Gauge.Events;

public class StiGetTextEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public string Value { get; set; }
}
