namespace Stimulsoft.Report.Web;

internal class StiReportContainer
{
	public StiReport report;

	public bool resourcesIncluded;

	public StiDesignerCommand command;

	public StiReportContainer(StiReport report, bool resourcesIncluded, StiDesignerCommand command)
	{
		this.report = report;
		this.resourcesIncluded = resourcesIncluded;
		this.command = command;
	}
}
