using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Events;

public class StiSaveLoadPageEventArgs : EventArgs
{
	public int PageIndex { get; }

	public string CachePath { get; }

	public StiPage Page { get; }

	public StiSaveLoadPageEventArgs(StiPage page, int pageIndex, string cachePath)
	{
		PageIndex = pageIndex;
		CachePath = cachePath;
		Page = page;
	}
}
