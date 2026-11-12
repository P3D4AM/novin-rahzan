using System.Data;
using System.Linq;

namespace Stimulsoft.Data.Extensions;

public static class EnumerableRowCollectionExt
{
	public static DataTable CopyToDataTableExt(this EnumerableRowCollection<DataRow> rows)
	{
		if (rows.Any())
		{
			return rows.CopyToDataTable();
		}
		return new DataTable();
	}
}
