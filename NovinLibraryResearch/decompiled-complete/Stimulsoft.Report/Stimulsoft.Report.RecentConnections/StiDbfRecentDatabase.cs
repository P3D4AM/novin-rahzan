namespace Stimulsoft.Report.RecentConnections;

public class StiDbfRecentDatabase : StiFileRecentDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiDBaseDatabase;

	public int CodePage { get; set; }
}
