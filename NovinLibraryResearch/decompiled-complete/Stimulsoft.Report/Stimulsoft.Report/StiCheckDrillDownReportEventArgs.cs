namespace Stimulsoft.Report;

public class StiCheckDrillDownReportEventArgs
{
	public string DrillDownReport { get; set; }

	public StiCheckDrillDownReportEventArgs(string drillDownReport)
	{
		DrillDownReport = drillDownReport;
	}
}
