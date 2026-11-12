using System.Data;

namespace Stimulsoft.Base.BusinessObjects;

public sealed class StiFixedColumnInfo
{
	public DataTable Table { get; }

	public DataColumn FixedColumn { get; }

	public StiFixedColumnInfo(DataTable table, DataColumn fixedColumn)
	{
		Table = table;
		FixedColumn = fixedColumn;
	}
}
