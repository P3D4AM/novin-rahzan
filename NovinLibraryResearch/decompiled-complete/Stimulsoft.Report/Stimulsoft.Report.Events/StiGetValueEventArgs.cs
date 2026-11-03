using System;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiGetValueEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual string Value { get; set; }

	public virtual bool StoreToPrinted { get; set; }
}
