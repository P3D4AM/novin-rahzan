using System;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiGetDataUrlEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual string Value { get; set; }
}
