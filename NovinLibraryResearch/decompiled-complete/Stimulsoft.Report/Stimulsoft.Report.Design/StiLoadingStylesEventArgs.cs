using System;

namespace Stimulsoft.Report.Design;

public class StiLoadingStylesEventArgs : EventArgs
{
	public bool Processed { get; set; } = true;


	public StiStylesCollection Styles { get; }

	public StiLoadingStylesEventArgs(StiStylesCollection styles)
	{
		Styles = styles;
	}
}
