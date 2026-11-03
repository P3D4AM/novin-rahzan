using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDatabaseInformation
{
	public List<DataRelation> Relations { get; set; } = new List<DataRelation>();


	public List<DataTable> Tables { get; } = new List<DataTable>();


	public List<DataTable> Views { get; } = new List<DataTable>();


	public List<DataTable> StoredProcedures { get; } = new List<DataTable>();


	public StiDatabaseInformation()
	{
	}

	public StiDatabaseInformation(List<DataTable> tables)
	{
		tables.ForEach(Tables.Add);
	}

	public StiDatabaseInformation(DataTableCollection tables)
		: this(tables.Cast<DataTable>().ToList())
	{
	}
}
