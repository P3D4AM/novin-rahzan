using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

public class StiPrintReportEventArgs : StiReportDataEventArgs
{
	public StiPrintAction PrintAction { get; private set; }

	public StiExportSettings Settings { get; private set; }

	public StiPrintReportEventArgs(StiRequestParams requestParams, StiReport report, StiExportSettings settings)
		: base(requestParams, report)
	{
		Settings = settings;
		PrintAction = requestParams.Viewer.PrintAction;
	}
}
