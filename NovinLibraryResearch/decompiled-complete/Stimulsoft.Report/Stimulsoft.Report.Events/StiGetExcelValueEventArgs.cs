using System;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiGetExcelValueEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual bool StoreToPrinted { get; set; }

	public virtual string Value { get; set; }
}
