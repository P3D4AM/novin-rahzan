using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Stimulsoft.Base;

public class StiMsAccessConnector : StiOleDbConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.MsAccessDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.MsAccessDataSource;

	public override string Name => "MS Access";

	public override bool IsAvailable => true;

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		if (string.IsNullOrEmpty(base.ConnectionString))
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		try
		{
			using (DbConnection dbConnection = CreateConnection())
			{
				dbConnection.Open();
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables");
					foreach (StiSchemaRow item2 in StiSchemaRow.All(schema))
					{
						if (!(item2.TABLE_SCHEMA == "sys"))
						{
							StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTableOrView(item2.TABLE_NAME);
							hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
							if (item2.TABLE_TYPE == "VIEW")
							{
								stiDataSchema.Views.Add(stiDataTableSchema);
							}
							if (item2.TABLE_TYPE == "TABLE")
							{
								stiDataSchema.Tables.Add(stiDataTableSchema);
							}
						}
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Columns");
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema2))
					{
						if (!(item3.TABLE_SCHEMA == "sys"))
						{
							StiDataColumnSchema item = new StiDataColumnSchema(item3.COLUMN_NAME, GetNetType(item3.DATA_TYPE_INT));
							if (hashtable[item3.TABLE_NAME] is StiDataTableSchema stiDataTableSchema2)
							{
								stiDataTableSchema2.Columns.Add(item);
							}
						}
					}
				}
				catch
				{
				}
				Hashtable hashtable2 = new Hashtable();
				try
				{
					DataTable schema3 = dbConnection.GetSchema("Procedures");
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema3))
					{
						if (!(item4.PROCEDURE_SCHEMA == "sys"))
						{
							string text = item4.PROCEDURE_NAME;
							if (text.IndexOf(";", StringComparison.InvariantCulture) != -1)
							{
								text = text.Substring(0, text.IndexOf(";", StringComparison.InvariantCulture));
							}
							StiDataTableSchema stiDataTableSchema3 = StiDataTableSchema.NewProcedure(text);
							hashtable2[stiDataTableSchema3.Name] = stiDataTableSchema3;
							stiDataSchema.StoredProcedures.Add(stiDataTableSchema3);
						}
					}
				}
				catch
				{
				}
				dbConnection.Close();
			}
			return stiDataSchema.Sort();
		}
		catch (Exception)
		{
			if (allowException)
			{
				throw;
			}
			return null;
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Password=pass;" + Environment.NewLine + "Data Source=C:\\\\myAccessFile.accdb;";
	}

	public override Type GetDbType()
	{
		return typeof(OleDbType);
	}

	public new static StiMsAccessConnector Get(string connectionString = null)
	{
		return new StiMsAccessConnector(connectionString);
	}

	protected internal StiMsAccessConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.OleDb.dll";
		base.TypeConnection = typeof(OleDbConnection).ToString();
		base.TypeDataAdapter = typeof(OleDbDataAdapter).ToString();
		base.TypeCommand = typeof(OleDbCommand).ToString();
		base.TypeParameter = typeof(OleDbParameter).ToString();
		base.TypeCommandBuilder = typeof(OleDbCommandBuilder).ToString();
	}
}
