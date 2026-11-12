using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;

namespace Stimulsoft.Base;

public class StiOleDbConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.OleDbDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.OleDbDataSource;

	public override string Name => "OLE DB";

	public override bool IsAvailable => true;

	public override Type ConnectionType => typeof(OleDbConnection);

	public override Type SqlType => typeof(StiDbType.OleDb);

	public override int DefaultSqlType => 12;

	public override DbConnection CreateConnection()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (DbConnection)new OleDbConnection(base.ConnectionString);
	}

	public override DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		OleDbDataAdapter val = new OleDbDataAdapter(query, (OleDbConnection)(object)((connection is OleDbConnection) ? connection : null));
		((DbCommand)(object)val.SelectCommand).CommandType = commandType;
		return (DbDataAdapter)(object)val;
	}

	public override DbCommand CreateCommand(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0014: Expected O, but got Unknown
		OleDbCommand val = new OleDbCommand(query, (OleDbConnection)(object)((connection is OleDbConnection) ? connection : null));
		((DbCommand)val).CommandType = commandType;
		return (DbCommand)val;
	}

	public override DbParameter CreateParameter(string parameterName, object value, int size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Expected O, but got Unknown
		OleDbParameter val = new OleDbParameter(parameterName, value);
		((DbParameter)val).Size = size;
		return (DbParameter)val;
	}

	public override DbParameter CreateParameter(string parameterName, int type, int size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Expected O, but got Unknown
		OleDbParameter val = new OleDbParameter(parameterName, (OleDbType)type);
		((DbParameter)val).Size = size;
		return (DbParameter)val;
	}

	public override void DeriveParameters(DbCommand command)
	{
		OleDbCommandBuilder.DeriveParameters((OleDbCommand)(object)((command is OleDbCommand) ? command : null));
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
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables");
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema))
					{
						if (!(item3.TABLE_SCHEMA == "sys") && !(item3.TABLE_SCHEMA == "INFORMATION_SCHEMA"))
						{
							string name = StiTableName.GetName((item3.TABLE_SCHEMA != "dbo") ? item3.TABLE_SCHEMA : null, item3.TABLE_NAME);
							string selectQuery = StiTableQuery.Get(this).GetSelectQuery((item3.TABLE_SCHEMA != "dbo") ? item3.TABLE_SCHEMA : null, item3.TABLE_NAME);
							StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTableOrView(name, this, selectQuery);
							hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
							if (item3.TABLE_TYPE == "VIEW")
							{
								stiDataSchema.Views.Add(stiDataTableSchema);
							}
							if (item3.TABLE_TYPE == "TABLE")
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
					if (StiDataConnector.AdvancedRetrievalModeOfDatabaseSchema)
					{
						DataTable schema3 = dbConnection.GetSchema("Tables");
						foreach (StiSchemaRow item4 in StiSchemaRow.All(schema3))
						{
							if (item4.TABLE_SCHEMA == "sys" || item4.TABLE_TYPE == null || (item4.TABLE_TYPE != "TABLE" && item4.TABLE_TYPE != "VIEW"))
							{
								continue;
							}
							string selectQuery2 = StiTableQuery.Get(this).GetSelectQuery((item4.TABLE_SCHEMA != "dbo") ? item4.TABLE_SCHEMA : null, item4.TABLE_NAME);
							using DbCommand dbCommand = CreateCommand(selectQuery2, dbConnection);
							using DbDataReader dbDataReader = dbCommand.ExecuteReader(CommandBehavior.SchemaOnly);
							using DataTable dataTable = dbDataReader.GetSchemaTable();
							foreach (DataRow row2 in dataTable.Rows)
							{
								StiDataColumnSchema item = new StiDataColumnSchema(row2["COLUMNNAME"].ToString(), Type.GetType(row2["DATATYPE"].ToString()));
								StiDataTableSchema stiDataTableSchema2 = hashtable[item4.TABLE_NAME] as StiDataTableSchema;
								if (dataTable != null && !dataTable.Columns.Contains(row2["ColumnName"].ToString()))
								{
									stiDataTableSchema2.Columns.Add(item);
								}
							}
						}
					}
					else
					{
						foreach (StiSchemaRow row in StiSchemaRow.All(schema2))
						{
							if (!(row.TABLE_SCHEMA == "sys"))
							{
								Type netType = GetNetType(row.DATA_TYPE_INT);
								string name2 = StiTableName.GetName((row.TABLE_SCHEMA != "dbo") ? row.TABLE_SCHEMA : null, row.TABLE_NAME);
								StiDataColumnSchema item2 = new StiDataColumnSchema(row.COLUMN_NAME, netType);
								if (hashtable[name2] is StiDataTableSchema stiDataTableSchema3 && stiDataTableSchema3.Columns.All((StiDataColumnSchema c) => c.Name != row.COLUMN_NAME))
								{
									stiDataTableSchema3.Columns.Add(item2);
								}
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
					DataTable schema4 = dbConnection.GetSchema("Procedures");
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema4))
					{
						if (!(item5.PROCEDURE_SCHEMA == "sys"))
						{
							string text = item5.PROCEDURE_NAME;
							if (text.IndexOf(";", StringComparison.InvariantCulture) != -1)
							{
								text = text.Substring(0, text.IndexOf(";", StringComparison.InvariantCulture));
							}
							string name3 = StiTableName.GetName((item5.PROCEDURE_SCHEMA != "dbo") ? item5.PROCEDURE_SCHEMA : null, text);
							string procQuery = StiTableQuery.Get(this).GetProcQuery((item5.PROCEDURE_SCHEMA != "dbo") ? item5.PROCEDURE_SCHEMA : null, text);
							StiDataTableSchema stiDataTableSchema4 = StiDataTableSchema.NewProcedure(name3, this, procQuery);
							hashtable2[stiDataTableSchema4.Name] = stiDataTableSchema4;
							stiDataSchema.StoredProcedures.Add(stiDataTableSchema4);
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
						using DbCommand dbCommand2 = CreateCommand(storedProcedure.Query, dbConnection, CommandType.StoredProcedure);
						DeriveParameters(dbCommand2);
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
				try
				{
					foreach (StiDataTableSchema table in stiDataSchema.Tables)
					{
						DataTable relationsTable = GetRelationsTable(dbConnection, table.Name);
						if (relationsTable == null)
						{
							continue;
						}
						foreach (StiSchemaRow item6 in StiSchemaRow.All(relationsTable))
						{
							stiDataSchema.Relations.Add(new StiDataRelationSchema
							{
								Name = item6.FK_NAME,
								ParentSourceName = item6.PK_TABLE_NAME,
								ChildSourceName = item6.FK_TABLE_NAME,
								ParentColumns = new List<string> { item6.PK_COLUMN_NAME },
								ChildColumns = new List<string> { item6.FK_COLUMN_NAME }
							});
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

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 133;
		}
		if (type == typeof(TimeSpan))
		{
			return 135;
		}
		if (type == typeof(long))
		{
			return 20;
		}
		if (type == typeof(int))
		{
			return 3;
		}
		if (type == typeof(short))
		{
			return 2;
		}
		if (type == typeof(byte))
		{
			return 16;
		}
		if (type == typeof(ulong))
		{
			return 21;
		}
		if (type == typeof(uint))
		{
			return 19;
		}
		if (type == typeof(ushort))
		{
			return 18;
		}
		if (type == typeof(sbyte))
		{
			return 17;
		}
		if (type == typeof(float))
		{
			return 4;
		}
		if (type == typeof(double))
		{
			return 5;
		}
		if (type == typeof(decimal))
		{
			return 14;
		}
		if (type == typeof(string))
		{
			return 8;
		}
		if (type == typeof(bool))
		{
			return 11;
		}
		if (type == typeof(char))
		{
			return 129;
		}
		if (type == typeof(byte[]))
		{
			return 128;
		}
		if (type == typeof(Guid))
		{
			return 72;
		}
		return 3;
	}

	public override Type GetNetType(string dbType)
	{
		throw new NotSupportedException();
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.OleDb)dbType)
		{
		case StiDbType.OleDb.Date:
		case StiDbType.OleDb.DBDate:
		case StiDbType.OleDb.DBTimeStamp:
			return typeof(DateTime);
		case StiDbType.OleDb.DBTime:
			return typeof(TimeSpan);
		case StiDbType.OleDb.BigInt:
			return typeof(long);
		case StiDbType.OleDb.Integer:
			return typeof(int);
		case StiDbType.OleDb.SmallInt:
			return typeof(short);
		case StiDbType.OleDb.TinyInt:
			return typeof(byte);
		case StiDbType.OleDb.UnsignedBigInt:
			return typeof(ulong);
		case StiDbType.OleDb.UnsignedInt:
			return typeof(uint);
		case StiDbType.OleDb.UnsignedSmallInt:
			return typeof(ushort);
		case StiDbType.OleDb.UnsignedTinyInt:
			return typeof(sbyte);
		case StiDbType.OleDb.Single:
			return typeof(float);
		case StiDbType.OleDb.Double:
			return typeof(double);
		case StiDbType.OleDb.Currency:
		case StiDbType.OleDb.Decimal:
		case StiDbType.OleDb.Numeric:
			return typeof(decimal);
		case StiDbType.OleDb.BSTR:
		case StiDbType.OleDb.WChar:
		case StiDbType.OleDb.VarChar:
		case StiDbType.OleDb.VarWChar:
			return typeof(string);
		case StiDbType.OleDb.Boolean:
			return typeof(bool);
		case StiDbType.OleDb.Char:
			return typeof(char);
		case StiDbType.OleDb.Binary:
			return typeof(byte[]);
		case StiDbType.OleDb.Guid:
			return typeof(Guid);
		default:
			return typeof(string);
		}
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return $"[{name}]";
	}

	protected override DataTable GetRelationsTable(DbConnection connection, string tableName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[3] { null, null, tableName });
	}

	public override string GetSampleConnectionString()
	{
		return "Provider=SQLOLEDB.1; Integrated Security=SSPI;" + Environment.NewLine + "Persist Security Info=False; Initial Catalog=myDataBase;" + Environment.NewLine + "Data Source=myServerAddress";
	}

	public override Type GetDbType()
	{
		return typeof(OleDbType);
	}

	public static StiOleDbConnector Get(string connectionString = null)
	{
		return new StiOleDbConnector(connectionString);
	}

	public StiOleDbConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.OleDb.OleDbConnection.dll";
		base.TypeConnection = typeof(OleDbConnection).ToString();
		base.TypeDataAdapter = typeof(OleDbDataAdapter).ToString();
		base.TypeCommand = typeof(OleDbCommand).ToString();
		base.TypeParameter = typeof(OleDbParameter).ToString();
		base.TypeCommandBuilder = typeof(OleDbCommandBuilder).ToString();
	}
}
