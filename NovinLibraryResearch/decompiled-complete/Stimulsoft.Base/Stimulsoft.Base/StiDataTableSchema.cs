using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base;

public class StiDataTableSchema : StiObjectSchema
{
	public List<StiDataColumnSchema> Columns { get; set; }

	public List<StiDataParameterSchema> Parameters { get; set; }

	public string Query { get; set; }

	public static StiDataTableSchema NewTableOrView(string name, StiSqlDataConnector connector = null, string query = null)
	{
		return NewTable(name, connector, query);
	}

	public static StiDataTableSchema NewTable(string name, StiSqlDataConnector connector = null, string query = null)
	{
		return new StiDataTableSchema(name, query ?? StiTableQuery.Get(connector).GetSelectQuery(name));
	}

	public static StiDataTableSchema NewView(string name, StiSqlDataConnector connector = null, string query = null)
	{
		return new StiDataTableSchema(name, query ?? StiTableQuery.Get(connector).GetSelectQuery(name));
	}

	public static StiDataTableSchema NewProcedure(string name, StiSqlDataConnector connector = null, string query = null)
	{
		return new StiDataTableSchema(name, query ?? StiTableQuery.Get(connector).GetExecuteQuery(name));
	}

	public StiDataTableSchema()
	{
		Columns = new List<StiDataColumnSchema>();
		Parameters = new List<StiDataParameterSchema>();
	}

	public StiDataTableSchema(string name, string query = null)
		: this()
	{
		base.Name = name;
		Query = query;
	}

	public StiDataTableSchema(DataTable table)
	{
		base.Name = table.TableName;
		Columns = new List<StiDataColumnSchema>();
		foreach (DataColumn column in table.Columns)
		{
			Columns.Add(new StiDataColumnSchema
			{
				Name = column.ColumnName,
				Type = column.DataType
			});
		}
		Parameters = new List<StiDataParameterSchema>();
	}
}
