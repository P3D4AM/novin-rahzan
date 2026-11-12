using System;

namespace Stimulsoft.Base.Wizards;

public class StiCreatedReportEventArgs : EventArgs
{
	public bool IsCreatedReportFromDataWizard { get; set; }

	public bool IsBlankReport { get; set; }
}
