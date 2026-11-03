namespace Stimulsoft.Report.Dictionary;

public class StiRow
{
	private StiDataSource dataSource;

	private int rowIndex;

	public object this[string columnName] => dataSource.GetData(columnName, rowIndex);

	public StiRow(StiDataSource dataSource, int rowIndex)
	{
		this.dataSource = dataSource;
		this.rowIndex = rowIndex;
	}
}
