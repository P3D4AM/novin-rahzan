using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base;

public class StiSchemaRow
{
	private DataRow row;

	public string COLUMN_DATA_TYPE => row["COLUMN_DATA_TYPE"] as string;

	public string COLUMN_NAME => row["COLUMN_NAME"] as string;

	public string COLUMNNAME => row["COLUMNNAME"] as string;

	public string COLUMN_TYPE => row["COLUMN_TYPE"] as string;

	public string CONSTRAINT_NAME => row["CONSTRAINT_NAME"] as string;

	public string DATA_TYPE => row["DATA_TYPE"] as string;

	public int DATA_TYPE_INT
	{
		get
		{
			if (!(row["DATA_TYPE"] is int))
			{
				return 0;
			}
			return (int)row["DATA_TYPE"];
		}
	}

	public string DATATYPE => row["DATATYPE"] as string;

	public string FIELD_NAME => row["FIELD_NAME"] as string;

	public string FK_COLUMN_NAME => row["FK_COLUMN_NAME"] as string;

	public string FK_CONSTRAINT_NAME => row["FK_CONSTRAINT_NAME"] as string;

	public string FK_NAME => row["FK_NAME"] as string;

	public string FK_TABLE_NAME => row["FK_TABLE_NAME"] as string;

	public string FOREIGN_COLUMN_NAME => row["FOREIGN_COLUMN_NAME"] as string;

	public string FOREIGN_TABLE_NAME => row["FOREIGN_TABLE_NAME"] as string;

	public string NAME => row["NAME"] as string;

	public string OBJECT_NAME => row["OBJECT_NAME"] as string;

	public string OWNER => row["OWNER"] as string;

	public string PARAMETER_DIRECTION => row["PARAMETER_DIRECTION"] as string;

	public string PARAMETER_DATA_TYPE => row["PARAMETER_DATA_TYPE"] as string;

	public string PARAMETER_NAME => row["PARAMETER_NAME"] as string;

	public string PK_COLUMN_NAME => row["PK_COLUMN_NAME"] as string;

	public string PK_TABLE_NAME => row["PK_TABLE_NAME"] as string;

	public string PROCEDURE_CAT => row["PROCEDURE_CAT"] as string;

	public string PROCEDURE_NAME => row["PROCEDURE_NAME"] as string;

	public string PROCEDURE_SCHEMA => row["PROCEDURE_SCHEMA"] as string;

	public string PROCEDURE_SCHEM => row["PROCEDURE_SCHEM"] as string;

	public string R_OWNER => row["R_OWNER"] as string;

	public string R_PK => row["R_PK"] as string;

	public string R_TABLE_NAME => row["R_TABLE_NAME"] as string;

	public string REFERENCED_COLUMN_NAME => row["REFERENCED_COLUMN_NAME"] as string;

	public string REFERENCED_TABLE_NAME => row["REFERENCED_TABLE_NAME"] as string;

	public string REFERENCES_TABLE => row["REFERENCES_TABLE"] as string;

	public string REFERENCES_FIELD => row["REFERENCES_FIELD"] as string;

	public string ROUTINE_CATALOG => row["ROUTINE_CATALOG"] as string;

	public string ROUTINE_NAME => row["ROUTINE_NAME"] as string;

	public string ROUTINE_TYPE => row["ROUTINE_TYPE"] as string;

	public string ROUTINE_SCHEMA => row["ROUTINE_SCHEMA"] as string;

	public string SPECIFIC_NAME => row["SPECIFIC_NAME"] as string;

	public string SPECIFIC_SCHEMA => row["SPECIFIC_SCHEMA"] as string;

	public string TABLE_NAME => row["TABLE_NAME"] as string;

	public string TABLE_SCHEM => row["TABLE_SCHEM"] as string;

	public string TABLE_SCHEMA => row["TABLE_SCHEMA"] as string;

	public string TABLE_TYPE => row["TABLE_TYPE"] as string;

	public string TYPE => row["TYPE"] as string;

	public string TYPE_NAME => row["TYPE_NAME"] as string;

	public string VIEW_NAME => row["VIEW_NAME"] as string;

	public string UQ_COLUMN_NAME => row["UQ_COLUMN_NAME"] as string;

	public string UQ_TABLE_NAME => row["UQ_TABLE_NAME"] as string;

	public static IEnumerable<StiSchemaRow> All(DataTable table)
	{
		foreach (DataRow row in table.Rows)
		{
			yield return new StiSchemaRow(row);
		}
	}

	private StiSchemaRow(DataRow row)
	{
		this.row = row;
	}
}
