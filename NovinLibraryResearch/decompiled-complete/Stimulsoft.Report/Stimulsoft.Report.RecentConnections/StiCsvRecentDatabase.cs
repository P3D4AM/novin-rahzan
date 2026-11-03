namespace Stimulsoft.Report.RecentConnections;

public class StiCsvRecentDatabase : StiFileRecentDatabase
{
	public override StiComponentId ComponentId => StiComponentId.StiCsvDatabase;

	public string Separator { get; set; }

	public int CodePage { get; set; }
}
