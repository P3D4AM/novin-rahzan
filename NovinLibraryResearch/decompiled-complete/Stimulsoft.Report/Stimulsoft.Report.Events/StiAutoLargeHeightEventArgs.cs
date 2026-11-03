using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Events;

public class StiAutoLargeHeightEventArgs : EventArgs
{
	public StiPage Page { get; }

	public StiReport Report { get; }

	public StiAutoLargeHeightEventArgs(StiReport report, StiPage page)
	{
		Page = page;
		Report = report;
	}
}
