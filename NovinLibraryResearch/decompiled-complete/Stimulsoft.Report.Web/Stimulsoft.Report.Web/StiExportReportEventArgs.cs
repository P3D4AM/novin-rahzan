using Stimulsoft.Report.Export;

namespace Stimulsoft.Report.Web;

public class StiExportReportEventArgs : StiReportDataEventArgs
{
	public StiExportSettings Settings { get; private set; }

	public StiExportFormat Format
	{
		get
		{
			if (Settings != null)
			{
				return Settings.GetExportFormat();
			}
			return StiExportFormat.None;
		}
	}

	public StiExportReportEventArgs(StiRequestParams requestParams, StiReport report, StiExportSettings settings)
		: base(requestParams, report)
	{
		Settings = settings;
	}
}
