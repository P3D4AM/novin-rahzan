namespace Stimulsoft.Report.Dictionary;

public class StiUserGetDataEventArgs
{
	public StiUserSource UserSource { get; }

	public int Position { get; }

	public string ColumnName { get; }

	public object Data { get; set; }

	public StiUserGetDataEventArgs(StiUserSource userSource, int position, string columnName)
	{
		UserSource = userSource;
		Position = position;
		ColumnName = columnName;
	}
}
