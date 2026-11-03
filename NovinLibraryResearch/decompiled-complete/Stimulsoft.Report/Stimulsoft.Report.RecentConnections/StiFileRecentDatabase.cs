namespace Stimulsoft.Report.RecentConnections;

public class StiFileRecentDatabase : StiRecentDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiFileDatabase;

	public string PathData { get; set; }
}
