namespace Stimulsoft.Report.RecentConnections;

public class StiExcelRecentDatabase : StiFileRecentDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiExcelDatabase;

	public bool FirstRowIsHeader { get; set; }
}
