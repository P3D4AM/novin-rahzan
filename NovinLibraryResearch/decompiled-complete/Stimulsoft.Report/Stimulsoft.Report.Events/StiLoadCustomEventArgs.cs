using System;

namespace Stimulsoft.Report.Events;

public sealed class StiLoadCustomEventArgs : EventArgs
{
	public string Ident { get; }

	public StiBaseStyle Style { get; set; }

	public StiLoadCustomEventArgs(string ident)
	{
		Ident = ident;
	}
}
