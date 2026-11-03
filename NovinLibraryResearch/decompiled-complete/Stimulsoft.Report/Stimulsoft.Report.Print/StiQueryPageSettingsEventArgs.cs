using System;
using System.Drawing.Printing;

namespace Stimulsoft.Report.Print;

public class StiQueryPageSettingsEventArgs : EventArgs
{
	public object PaperSize { get; set; }

	public PaperSource PaperSource { get; set; }

	public PageSettings PageSettings { get; set; }

	internal StiQueryPageSettingsEventArgs(object paperSize, PaperSource paperSource, PageSettings pageSettings)
	{
		PaperSize = paperSize;
		PaperSource = paperSource;
		PageSettings = pageSettings;
	}
}
