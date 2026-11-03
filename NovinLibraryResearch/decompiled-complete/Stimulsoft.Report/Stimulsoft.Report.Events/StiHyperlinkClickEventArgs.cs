using System;

namespace Stimulsoft.Report.Events;

public class StiHyperlinkClickEventArgs : EventArgs
{
	public bool Handled { get; set; }

	public string Hyperlink { get; set; }

	public StiHyperlinkClickEventArgs(string hyperlink)
	{
		Hyperlink = hyperlink;
	}
}
