namespace Stimulsoft.Report.RecentConnections;

public abstract class StiRecentDatabase
{
	public abstract StiComponentId ComponentId { get; }

	public string Name { get; set; }

	public string Alias { get; set; }
}
