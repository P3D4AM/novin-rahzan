using System;
using Stimulsoft.Report.CrossTab.Core;

namespace Stimulsoft.Report.CrossTab;

public class StiProcessCellEventArgs : EventArgs
{
	public StiCell Cell { get; set; }

	public int Column { get; set; }

	public int Row { get; set; }

	public decimal Value { get; set; }

	public string Text { get; set; }
}
