using System.Data;
using System.Data.Common;

namespace Stimulsoft.Report.Dictionary;

internal class StiDataAdapterHelper
{
	public static void Fill(StiDictionary dictionary, DbDataAdapter dataAdapter, DataTable dataTable, bool schemaOnly)
	{
		if (schemaOnly)
		{
			dataAdapter.FillSchema(dataTable, SchemaType.Source);
		}
		else
		{
			dataAdapter.Fill(dataTable);
		}
	}

	public static DataTable Fill(StiDictionary dictionary, DataTable dataTable)
	{
		return dataTable;
	}

	public static DataSet Fill(StiDictionary dictionary, DataSet dataSet, StiDatabase database)
	{
		return dataSet;
	}
}
