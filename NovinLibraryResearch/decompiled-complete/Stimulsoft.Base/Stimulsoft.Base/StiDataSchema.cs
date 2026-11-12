using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Stimulsoft.Base;

public class StiDataSchema : StiObjectSchema
{
	public List<StiDataTableSchema> Tables { get; set; }

	public List<StiDataTableSchema> Views { get; set; }

	public List<StiDataTableSchema> StoredProcedures { get; set; }

	public List<StiDataTableSchema> Queries { get; set; }

	public List<StiDataRelationSchema> Relations { get; set; }

	public StiConnectionIdent ConnectionIdent { get; set; }

	public bool IsEmpty()
	{
		if (Tables.Count <= 0 && Views.Count <= 0 && StoredProcedures.Count <= 0 && Relations.Count <= 0)
		{
			return Queries.Count <= 0;
		}
		return false;
	}

	public DataSet GetDataSet()
	{
		DataSet dataSet = new DataSet();
		if (Tables != null)
		{
			Tables.ForEach(delegate(StiDataTableSchema table)
			{
				DataTable dataTable6 = new DataTable(table.Name);
				table.Columns.ForEach(delegate(StiDataColumnSchema column)
				{
					dataTable6.Columns.Add(column.Name, column.Type);
				});
				dataSet.Tables.Add(dataTable6);
			});
		}
		if (Views != null)
		{
			Views.ForEach(delegate(StiDataTableSchema view)
			{
				DataTable dataTable5 = new DataTable(view.Name);
				view.Columns.ForEach(delegate(StiDataColumnSchema column)
				{
					dataTable5.Columns.Add(column.Name, column.Type);
				});
				dataSet.Tables.Add(dataTable5);
			});
		}
		if (StoredProcedures != null)
		{
			StoredProcedures.ForEach(delegate(StiDataTableSchema proc)
			{
				DataTable dataTable4 = new DataTable(proc.Name);
				proc.Columns.ForEach(delegate(StiDataColumnSchema column)
				{
					dataTable4.Columns.Add(column.Name, column.Type);
				});
				dataSet.Tables.Add(dataTable4);
			});
		}
		if (Queries != null)
		{
			Queries.ForEach(delegate(StiDataTableSchema query)
			{
				DataTable dataTable3 = new DataTable(query.Name);
				query.Columns.ForEach(delegate(StiDataColumnSchema column)
				{
					dataTable3.Columns.Add(column.Name, column.Type);
				});
				dataSet.Tables.Add(dataTable3);
			});
		}
		if (Relations != null)
		{
			Relations.ForEach(delegate(StiDataRelationSchema relation)
			{
				try
				{
					DataTable dataTable = dataSet.Tables[relation.ParentSourceName];
					DataTable dataTable2 = dataSet.Tables[relation.ChildSourceName];
					if (dataTable != null && dataTable2 != null)
					{
						List<DataColumn> list = new List<DataColumn>();
						List<DataColumn> list2 = new List<DataColumn>();
						foreach (string parentColumn in relation.ParentColumns)
						{
							DataColumn dataColumn = dataTable.Columns[parentColumn];
							if (dataColumn == null)
							{
								return;
							}
							list.Add(dataColumn);
						}
						foreach (string childColumn in relation.ChildColumns)
						{
							DataColumn dataColumn2 = dataTable2.Columns[childColumn];
							if (dataColumn2 == null)
							{
								return;
							}
							list2.Add(dataColumn2);
						}
						dataSet.Relations.Add(new DataRelation(relation.Name, list.ToArray(), list2.ToArray(), createConstraints: false));
					}
				}
				catch
				{
				}
			});
		}
		return dataSet;
	}

	public StiDataSchema Sort()
	{
		Tables = Tables.OrderBy((StiDataTableSchema t) => t.Name).ToList();
		Views = Views.OrderBy((StiDataTableSchema v) => v.Name).ToList();
		Queries = Queries.OrderBy((StiDataTableSchema q) => q.Name).ToList();
		StoredProcedures = StoredProcedures.OrderBy((StiDataTableSchema s) => s.Name).ToList();
		Relations = Relations.OrderBy((StiDataRelationSchema r) => r.Name).ToList();
		return this;
	}

	public StiDataSchema()
		: this(StiConnectionIdent.Unspecified)
	{
	}

	public StiDataSchema(StiConnectionIdent ident)
	{
		ConnectionIdent = ident;
		Tables = new List<StiDataTableSchema>();
		Views = new List<StiDataTableSchema>();
		Queries = new List<StiDataTableSchema>();
		StoredProcedures = new List<StiDataTableSchema>();
		Relations = new List<StiDataRelationSchema>();
	}
}
