namespace Stimulsoft.Report.Export;

public abstract class StiPageRangeExportSettings : StiExportSettings
{
	public StiPagesRange PageRange { get; set; } = new StiPagesRange();

}
