using System;

namespace Stimulsoft.Report.Design;

public class StiCreatingObjectEventArgs : EventArgs
{
	public bool Processed { get; set; } = true;

}
