using System;

namespace Stimulsoft.Report.Design;

public class StiSavingStylesEventArgs : EventArgs
{
	public bool Processed { get; set; } = true;


	public StiStylesCollection Styles { get; }

	public StiSavingStylesEventArgs(StiStylesCollection styles)
	{
		Styles = styles;
	}
}
