using System;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiGetExcelSheetEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual string Value { get; set; }
}
