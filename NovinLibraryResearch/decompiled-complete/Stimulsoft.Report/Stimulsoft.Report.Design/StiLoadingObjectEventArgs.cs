using System;

namespace Stimulsoft.Report.Design;

public class StiLoadingObjectEventArgs : EventArgs
{
	public bool Processed { get; set; } = true;

}
