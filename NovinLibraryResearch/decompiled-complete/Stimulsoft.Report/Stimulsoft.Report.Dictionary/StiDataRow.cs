using System.Data;

namespace Stimulsoft.Report.Dictionary;

public class StiDataRow
{
	private StiDataSource dataSource;

	public DataRow Row { get; set; }

	protected StiDictionary Dictionary => dataSource.Dictionary;

	public object this[string columnName]
	{
		get
		{
			if (Row == null)
			{
				return null;
			}
			int columnIndex = dataSource.GetColumnIndex(columnName);
			if (columnIndex < 0)
			{
				return null;
			}
			return Row.ItemArray[columnIndex];
		}
	}

	public virtual StiDataRow GetParentData(string relation)
	{
		if (Row == null)
		{
			return null;
		}
		DataRow[] parentRows = Row.GetParentRows(relation);
		if (parentRows == null || parentRows.Length == 0)
		{
			return null;
		}
		return new StiDataRow(dataSource.GetParentDataSource(relation), parentRows[0]);
	}

	public StiDataRow(StiDataRow dataRow)
		: this(dataRow?.dataSource, dataRow?.Row)
	{
	}

	public StiDataRow(StiDataSource dataSource, DataRow dataRow)
	{
		this.dataSource = dataSource;
		Row = dataRow;
	}
}
