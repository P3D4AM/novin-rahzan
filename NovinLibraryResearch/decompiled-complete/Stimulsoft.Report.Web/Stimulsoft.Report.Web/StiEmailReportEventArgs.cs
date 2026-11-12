using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

public class StiEmailReportEventArgs : StiExportReportEventArgs
{
	public StiEmailOptions Options { get; private set; }

	public StiEmailReportEventArgs(StiRequestParams requestParams, StiReport report, StiExportSettings settings, StiEmailOptions options)
		: base(requestParams, report, settings)
	{
		Options = options;
	}
}
