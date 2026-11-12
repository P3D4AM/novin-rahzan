using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Web;

[Obsolete("This delegate is obsolete. It will be removed in next versions. Please use the StiReportDataEventArgs delegate instead.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public class StiExitEventArgs : StiReportDataEventArgs
{
	public StiExitEventArgs(StiRequestParams reqestParams, StiReport report)
		: base(reqestParams, report)
	{
		base.Report = report;
	}
}
