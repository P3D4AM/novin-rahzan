using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;

namespace Stimulsoft.Base;

public class StiOdbcConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.OdbcDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.OdbcDataSource;

	public override string Name => "ODBC";

	public override bool IsAvailable => true;

	public override Type ConnectionType => typeof(OdbcConnection);

	public override Type SqlType => typeof(StiDbType.Odbc);

	public override int DefaultSqlType => 18;

	public override DbConnection CreateConnection()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (DbConnection)new OdbcConnection(base.ConnectionString);
	}

	public override DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		OdbcDataAdapter val = new OdbcDataAdapter(query, (OdbcConnection)(object)((connection is OdbcConnection) ? connection : null));
		((DbCommand)(object)val.SelectCommand).CommandType = commandType;
		return (DbDataAdapter)(object)val;
	}

	public override DbCommand CreateCommand(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0014: Expected O, but got Unknown
		OdbcCommand val = new OdbcCommand(query, (OdbcConnection)(object)((connection is OdbcConnection) ? connection : null));
		((DbCommand)val).CommandType = commandType;
		return (DbCommand)val;
	}

	public override DbParameter CreateParameter(string parameterName, object value, int size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Expected O, but got Unknown
		OdbcParameter val = new OdbcParameter(parameterName, value);
		((DbParameter)val).Size = size;
		return (DbParameter)val;
	}

	public override DbParameter CreateParameter(string parameterName, int type, int size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Expected O, but got Unknown
		OdbcParameter val = new OdbcParameter(parameterName, (OdbcType)type);
		((DbParameter)val).Size = size;
		return (DbParameter)val;
	}

	public override void DeriveParameters(DbCommand command)
	{
		OdbcCommandBuilder.DeriveParameters((OdbcCommand)(object)((command is OdbcCommand) ? command : null));
	}

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
				bool getSchemaColumnsMode = StiDataConnector.GetSchemaColumnsMode;
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables");
					foreach (DataRow row in schema.Rows)
					{
						string text = (getSchemaColumnsMode ? row[1].ToString() : row["TABLE_SCHEM"].ToString());
						string text2 = (getSchemaColumnsMode ? row[2].ToString() : row["TABLE_NAME"].ToString());
						if (!(text == "sys"))
						{
							if (text2.Trim().Contains(" "))
							{
								text2 = $"[{text2}]";
							}
							text2 = StiTableName.GetName((text != "dbo") ? text : null, text2);
							string query = string.Format("select * from {0}{1}", (text == "dbo" || text == "") ? "" : $"{text}.", text2);
							StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTableOrView(text2, this, query);
							hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
							stiDataSchema.Tables.Add(stiDataTableSchema);
						}
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Views");
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema2))
					{
						if (!(item3.TABLE_SCHEM == "sys") && !(item3.TABLE_TYPE != "VIEW") && !(item3.TABLE_SCHEM == "INFORMATION_SCHEMA"))
						{
							string name = StiTableName.GetName((item3.TABLE_SCHEM != "dbo") ? item3.TABLE_SCHEM : null, item3.TABLE_NAME);
							string selectQuery = StiTableQuery.Get(this).GetSelectQuery((item3.TABLE_SCHEM != "dbo") ? item3.TABLE_SCHEM : null, item3.TABLE_NAME);
							StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewTableOrView(name, this, selectQuery);
							hashtable[stiDataTableSchema2.Name] = stiDataTableSchema2;
							stiDataSchema.Views.Add(stiDataTableSchema2);
						}
					}
				}
				catch
				{
				}
				try
				{
					if (StiDataConnector.AdvancedRetrievalModeOfDatabaseSchema)
					{
						DataTable schema3 = dbConnection.GetSchema("Tables");
						foreach (DataRow row2 in schema3.Rows)
						{
							string text3 = (getSchemaColumnsMode ? row2[2].ToString() : row2["TABLE_NAME"].ToString());
							string text4 = (getSchemaColumnsMode ? row2[1].ToString() : row2["TABLE_SCHEM"].ToString());
							string query2 = string.Format("select * from {0}", text3.Trim().Contains(" ") ? $"[{text3}]" : text3);
							using DbCommand dbCommand = CreateCommand(query2, dbConnection);
							using DbDataReader dbDataReader = dbCommand.ExecuteReader(CommandBehavior.SchemaOnly);
							using DataTable dataTable = dbDataReader.GetSchemaTable();
							foreach (DataRow rowTable in dataTable.Rows)
							{
								StiDataColumnSchema item = new StiDataColumnSchema(rowTable["ColumnName"].ToString(), Type.GetType(rowTable["DataType"].ToString()));
								text3 = StiTableName.GetName((text4 != "dbo") ? text4 : null, text3);
								if (hashtable[text3] is StiDataTableSchema stiDataTableSchema3 && !stiDataTableSchema3.Columns.Exists((StiDataColumnSchema t) => t.Name == rowTable["ColumnName"].ToString()))
								{
									stiDataTableSchema3.Columns.Add(item);
								}
							}
						}
					}
					else
					{
						DataTable schema4 = dbConnection.GetSchema("Columns");
						foreach (StiSchemaRow item4 in StiSchemaRow.All(schema4))
						{
							if (!(item4.TABLE_SCHEM == "sys"))
							{
								StiDataColumnSchema item2 = new StiDataColumnSchema(item4.COLUMN_NAME, GetNetType(item4.TYPE_NAME));
								string name2 = StiTableName.GetName((item4.TABLE_SCHEM != "dbo") ? item4.TABLE_SCHEM : null, item4.TABLE_NAME);
								if (hashtable[name2] is StiDataTableSchema stiDataTableSchema4)
								{
									stiDataTableSchema4.Columns.Add(item2);
								}
							}
						}
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema5 = dbConnection.GetSchema("Procedures");
					Hashtable hashtable2 = new Hashtable();
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema5))
					{
						if (!(item5.PROCEDURE_SCHEM == "sys") && item5.PROCEDURE_CAT != null && !(item5.PROCEDURE_CAT != dbConnection.Database))
						{
							string text5 = item5.PROCEDURE_NAME;
							if (text5.IndexOf(";", StringComparison.InvariantCulture) != -1)
							{
								text5 = text5.Substring(0, text5.IndexOf(";", StringComparison.InvariantCulture));
							}
							string name3 = StiTableName.GetName((item5.PROCEDURE_SCHEM != "dbo") ? item5.PROCEDURE_SCHEM : null, text5);
							string callQuery = StiTableQuery.Get(this).GetCallQuery((item5.PROCEDURE_SCHEM != "dbo") ? item5.PROCEDURE_SCHEM : null, text5);
							StiDataTableSchema stiDataTableSchema5 = StiDataTableSchema.NewProcedure(name3, this, callQuery);
							hashtable2[stiDataTableSchema5.Name] = stiDataTableSchema5;
							stiDataSchema.StoredProcedures.Add(stiDataTableSchema5);
						}
					}
				}
				catch
				{
				}
				foreach (StiDataTableSchema storedProcedure in stiDataSchema.StoredProcedures)
				{
					try
					{
						using DbCommand dbCommand2 = CreateCommand(storedProcedure.Name, dbConnection, CommandType.StoredProcedure);
						DeriveParameters(dbCommand2);
						if (dbCommand2.Parameters != null && dbCommand2.Parameters.Count > 0)
						{
							if (dbCommand2.Parameters[0].Direction == ParameterDirection.ReturnValue)
							{
								dbCommand2.Parameters.RemoveAt(0);
							}
							List<string> list = new List<string>();
							for (int num = dbCommand2.Parameters.Count; num > 0; num--)
							{
								list.Add("?");
							}
							storedProcedure.Query = string.Format("{{{0}({1})}}", storedProcedure.Query, string.Join(",", list));
						}
						else
						{
							storedProcedure.Query = $"{{{storedProcedure.Query}()}}";
						}
						dbCommand2.CommandText = storedProcedure.Query;
						using DbDataReader reader = dbCommand2.ExecuteReader(CommandBehavior.SchemaOnly);
						using DataTable dataTable2 = new DataTable(storedProcedure.Name);
						dataTable2.Load(reader);
						foreach (DataColumn column in dataTable2.Columns)
						{
							storedProcedure.Columns.Add(new StiDataColumnSchema
							{
								Name = column.ColumnName,
								Type = column.DataType
							});
						}
						foreach (DbParameter parameter in dbCommand2.Parameters)
						{
							if (parameter.Direction == ParameterDirection.Input)
							{
								storedProcedure.Parameters.Add(new StiDataParameterSchema
								{
									Name = parameter.ParameterName,
									Type = StiDbTypeConversion.GetNetType(parameter.DbType)
								});
							}
						}
					}
					catch
					{
					}
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

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 5;
		}
		if (type == typeof(TimeSpan))
		{
			return 19;
		}
		if (type == typeof(long))
		{
			return 1;
		}
		if (type == typeof(int))
		{
			return 10;
		}
		if (type == typeof(short))
		{
			return 17;
		}
		if (type == typeof(byte))
		{
			return 20;
		}
		if (type == typeof(ulong))
		{
			return 1;
		}
		if (type == typeof(uint))
		{
			return 10;
		}
		if (type == typeof(ushort))
		{
			return 17;
		}
		if (type == typeof(sbyte))
		{
			return 20;
		}
		if (type == typeof(float))
		{
			return 6;
		}
		if (type == typeof(double))
		{
			return 8;
		}
		if (type == typeof(decimal))
		{
			return 6;
		}
		if (type == typeof(string))
		{
			return 22;
		}
		if (type == typeof(bool))
		{
			return 4;
		}
		if (type == typeof(char))
		{
			return 4;
		}
		if (type == typeof(byte[]))
		{
			return 2;
		}
		if (type == typeof(Guid))
		{
			return 15;
		}
		return 10;
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "counter":
		case "bigint":
		case "longbinary":
		case "varbinary":
		case "int":
		case "uniqueidentifier":
			return typeof(long);
		case "smallint":
			return typeof(short);
		case "tinyint":
			return typeof(byte);
		case "decimal":
		case "money":
		case "smallmoney":
			return typeof(decimal);
		case "float":
		case "real":
			return typeof(float);
		case "double":
			return typeof(double);
		case "bit":
			return typeof(bool);
		case "datetime":
		case "smalldatetime":
		case "date":
		case "time":
		case "timestamp":
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Odbc)dbType)
		{
		case StiDbType.Odbc.BigInt:
		case StiDbType.Odbc.Int:
		case StiDbType.Odbc.UniqueIdentifier:
		case StiDbType.Odbc.VarBinary:
			return typeof(long);
		case StiDbType.Odbc.SmallInt:
			return typeof(short);
		case StiDbType.Odbc.TinyInt:
			return typeof(byte);
		case StiDbType.Odbc.Decimal:
			return typeof(decimal);
		case StiDbType.Odbc.Real:
			return typeof(float);
		case StiDbType.Odbc.Double:
			return typeof(double);
		case StiDbType.Odbc.Bit:
			return typeof(bool);
		case StiDbType.Odbc.DateTime:
		case StiDbType.Odbc.SmallDateTime:
		case StiDbType.Odbc.Timestamp:
		case StiDbType.Odbc.Date:
		case StiDbType.Odbc.Time:
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return $"`{name}`";
	}

	public override string GetSampleConnectionString()
	{
		return "Driver={SQL Server}; Server=myServerAddress;" + Environment.NewLine + "Database=myDataBase; Uid=myUsername; Pwd=myPassword;";
	}

	public override Type GetDbType()
	{
		return typeof(OdbcType);
	}

	public static StiOdbcConnector Get(string connectionString = null)
	{
		return new StiOdbcConnector(connectionString);
	}

	public StiOdbcConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.Odbc.OdbcConnection.dll";
		base.TypeConnection = typeof(OdbcConnection).ToString();
		base.TypeDataAdapter = typeof(OdbcDataAdapter).ToString();
		base.TypeCommand = typeof(OdbcCommand).ToString();
		base.TypeParameter = typeof(OdbcParameter).ToString();
		base.TypeCommandBuilder = typeof(OdbcCommandBuilder).ToString();
	}
}
