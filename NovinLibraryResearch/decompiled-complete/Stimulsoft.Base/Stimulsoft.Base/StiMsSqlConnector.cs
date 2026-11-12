using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Stimulsoft.Base;

public class StiMsSqlConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.MsSqlDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.MsSqlDataSource;

	public override string Name => "MS SQL";

	public override bool IsAvailable => true;

	public override Type ConnectionType => typeof(SqlConnection);

	public override Type SqlType => typeof(StiDbType.MsSql);

	public override int DefaultSqlType => 18;

	public override DbConnection CreateConnection()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		return (DbConnection)new SqlConnection(base.ConnectionString);
	}

	public override DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		SqlDataAdapter val = new SqlDataAdapter(query, (SqlConnection)(object)((connection is SqlConnection) ? connection : null));
		((DbCommand)(object)val.SelectCommand).CommandType = commandType;
		return (DbDataAdapter)(object)val;
	}

	public override DbCommand CreateCommand(string query, DbConnection connection, CommandType commandType = CommandType.Text)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0014: Expected O, but got Unknown
		SqlCommand val = new SqlCommand(query, (SqlConnection)(object)((connection is SqlConnection) ? connection : null));
		((DbCommand)val).CommandType = commandType;
		return (DbCommand)val;
	}

	public override DbParameter CreateParameter(string parameterName, object value, int size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Expected O, but got Unknown
		SqlParameter val = new SqlParameter(parameterName, value);
		((DbParameter)val).Size = size;
		return (DbParameter)val;
	}

	public override DbParameter CreateParameter(string parameterName, int type, int size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000f: Expected O, but got Unknown
		SqlParameter val = new SqlParameter(parameterName, (SqlDbType)type);
		((DbParameter)val).Size = size;
		return (DbParameter)val;
	}

	public override void DeriveParameters(DbCommand command)
	{
		SqlCommandBuilder.DeriveParameters((SqlCommand)(object)((command is SqlCommand) ? command : null));
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
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema))
					{
						if (!(item4.TABLE_SCHEMA == "sys"))
						{
							string name = StiTableName.GetName((item4.TABLE_SCHEMA != "dbo") ? item4.TABLE_SCHEMA : null, item4.TABLE_NAME);
							string selectQuery = StiTableQuery.Get(this).GetSelectQuery((item4.TABLE_SCHEMA != "dbo") ? item4.TABLE_SCHEMA : null, item4.TABLE_NAME);
							StiDataTableSchema item = (StiDataTableSchema)(hashtable[name] = StiDataTableSchema.NewTableOrView(name, this, selectQuery));
							if (item4.TABLE_TYPE == "BASE TABLE")
							{
								stiDataSchema.Tables.Add(item);
							}
							if (item4.TABLE_TYPE == "VIEW")
							{
								stiDataSchema.Views.Add(item);
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
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema2))
					{
						if (!(item5.TABLE_SCHEMA == "sys"))
						{
							string name2 = StiTableName.GetName((item5.TABLE_SCHEMA != "dbo") ? item5.TABLE_SCHEMA : null, item5.TABLE_NAME);
							StiDataColumnSchema item2 = new StiDataColumnSchema(item5.COLUMN_NAME, GetNetType(item5.DATA_TYPE));
							if (hashtable[name2] is StiDataTableSchema stiDataTableSchema2)
							{
								stiDataTableSchema2.Columns.Add(item2);
							}
						}
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema3 = dbConnection.GetSchema("Procedures");
					string text = ((dbConnection.Database != null) ? dbConnection.Database.ToUpperInvariant() : null);
					foreach (StiSchemaRow item6 in StiSchemaRow.All(schema3))
					{
						string text2 = ((item6.ROUTINE_CATALOG != null) ? item6.ROUTINE_CATALOG.ToUpperInvariant() : null);
						if (!(item6.SPECIFIC_SCHEMA == "sys") && !(item6.ROUTINE_TYPE != "PROCEDURE") && !(text2 != text))
						{
							string name3 = StiTableName.GetName((item6.SPECIFIC_SCHEMA != "dbo") ? item6.SPECIFIC_SCHEMA : null, item6.SPECIFIC_NAME);
							string procQuery = StiTableQuery.Get(this).GetProcQuery((item6.SPECIFIC_SCHEMA != "dbo") ? item6.SPECIFIC_SCHEMA : null, item6.SPECIFIC_NAME);
							StiDataTableSchema item3 = StiDataTableSchema.NewProcedure(name3, this, procQuery);
							stiDataSchema.StoredProcedures.Add(item3);
						}
					}
				}
				catch
				{
				}
				try
				{
					string query = "select obj.name as procName, params.*, type_name(system_type_id) as type_name from sys.parameters params, sys.objects obj where params.object_id = obj.object_id";
					using (DbCommand dbCommand = CreateCommand(query, dbConnection))
					{
						using DbDataReader reader = dbCommand.ExecuteReader();
						using DataTable dataTable = new DataTable("Parameters");
						dataTable.Load(reader);
						foreach (StiDataTableSchema storedProcedure in stiDataSchema.StoredProcedures)
						{
							foreach (DataRow row in dataTable.Rows)
							{
								if (storedProcedure.Name == row["procName"].ToString())
								{
									storedProcedure.Parameters.Add(new StiDataParameterSchema
									{
										Name = row["name"].ToString(),
										Type = GetNetType(row["type_name"].ToString())
									});
								}
							}
						}
					}
					if (StiDataOptions.WizardStoredProcRetrieveMode == StiWizardStoredProcRetrieveMode.All)
					{
						try
						{
							foreach (StiDataTableSchema storedProcedure2 in stiDataSchema.StoredProcedures)
							{
								using DbCommand dbCommand2 = CreateCommand(storedProcedure2.Query, dbConnection, CommandType.StoredProcedure);
								DeriveParameters(dbCommand2);
								using DbDataReader reader2 = dbCommand2.ExecuteReader(CommandBehavior.SchemaOnly);
								using DataTable dataTable2 = new DataTable(storedProcedure2.Name);
								dataTable2.Load(reader2);
								foreach (DataColumn column in dataTable2.Columns)
								{
									storedProcedure2.Columns.Add(new StiDataColumnSchema
									{
										Name = column.ColumnName,
										Type = column.DataType
									});
								}
							}
						}
						catch
						{
						}
					}
				}
				catch
				{
				}
				try
				{
					using DataSet dataSet = new DataSet();
					using DbDataAdapter dbDataAdapter = CreateAdapter("\r\n                            SELECT KCU1.CONSTRAINT_NAME AS 'FK_CONSTRAINT_NAME' \r\n                                , KCU1.TABLE_NAME AS 'FK_TABLE_NAME'\r\n                                , KCU1.COLUMN_NAME AS 'FK_COLUMN_NAME'\r\n                                , KCU1.ORDINAL_POSITION AS 'FK_ORDINAL_POSITION'\r\n                                , KCU2.CONSTRAINT_NAME AS 'UQ_CONSTRAINT_NAME'\r\n                                , KCU2.TABLE_NAME AS 'UQ_TABLE_NAME'\r\n                                , KCU2.COLUMN_NAME AS 'UQ_COLUMN_NAME'\r\n                                , KCU2.ORDINAL_POSITION AS 'UQ_ORDINAL_POSITION'\r\n                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC\r\n                            JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1\r\n                            ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG \r\n                                AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA\r\n                                AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME\r\n                            JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2\r\n                            ON KCU2.CONSTRAINT_CATALOG = \r\n                            RC.UNIQUE_CONSTRAINT_CATALOG \r\n                                AND KCU2.CONSTRAINT_SCHEMA = \r\n                            RC.UNIQUE_CONSTRAINT_SCHEMA\r\n                                AND KCU2.CONSTRAINT_NAME = \r\n                            RC.UNIQUE_CONSTRAINT_NAME\r\n                                AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION", dbConnection);
					dbDataAdapter.Fill(dataSet);
					DataTable dataTable3 = ((dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null);
					if (dataTable3 != null)
					{
						foreach (StiSchemaRow item7 in StiSchemaRow.All(dataTable3))
						{
							stiDataSchema.Relations.Add(new StiDataRelationSchema
							{
								Name = item7.FK_CONSTRAINT_NAME,
								ParentSourceName = item7.UQ_TABLE_NAME,
								ChildSourceName = item7.FK_TABLE_NAME,
								ParentColumns = new List<string> { item7.UQ_COLUMN_NAME },
								ChildColumns = new List<string> { item7.FK_COLUMN_NAME }
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
			return 4;
		}
		if (type == typeof(DateTimeOffset))
		{
			return 34;
		}
		if (type == typeof(TimeSpan))
		{
			return 19;
		}
		if (type == typeof(long))
		{
			return 0;
		}
		if (type == typeof(int))
		{
			return 8;
		}
		if (type == typeof(short))
		{
			return 16;
		}
		if (type == typeof(byte))
		{
			return 20;
		}
		if (type == typeof(ulong))
		{
			return 0;
		}
		if (type == typeof(uint))
		{
			return 8;
		}
		if (type == typeof(ushort))
		{
			return 16;
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
			return 13;
		}
		if (type == typeof(decimal))
		{
			return 5;
		}
		if (type == typeof(string))
		{
			return 22;
		}
		if (type == typeof(bool))
		{
			return 2;
		}
		if (type == typeof(char))
		{
			return 3;
		}
		if (type == typeof(byte[]))
		{
			return 1;
		}
		if (type == typeof(Guid))
		{
			return 14;
		}
		return 22;
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "uniqueidentifier":
		case "bigint":
		case "timestamp":
			return typeof(long);
		case "int":
			return typeof(int);
		case "smallint":
			return typeof(short);
		case "tinyint":
			return typeof(byte);
		case "decimal":
		case "money":
		case "smallmoney":
			return typeof(decimal);
		case "float":
			return typeof(float);
		case "real":
			return typeof(double);
		case "datetime":
		case "date":
		case "datetime2":
		case "smalldatetime":
			return typeof(DateTime);
		case "time":
			return typeof(TimeSpan);
		case "datetimeoffset":
			return typeof(DateTimeOffset);
		case "bit":
			return typeof(bool);
		case "binary":
		case "image":
			return typeof(byte[]);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.MsSql)dbType)
		{
		case StiDbType.MsSql.UniqueIdentifier:
			return typeof(Guid);
		case StiDbType.MsSql.BigInt:
		case StiDbType.MsSql.Timestamp:
			return typeof(long);
		case StiDbType.MsSql.Int:
			return typeof(int);
		case StiDbType.MsSql.SmallInt:
			return typeof(short);
		case StiDbType.MsSql.TinyInt:
			return typeof(byte);
		case StiDbType.MsSql.Decimal:
		case StiDbType.MsSql.Money:
		case StiDbType.MsSql.SmallMoney:
			return typeof(decimal);
		case StiDbType.MsSql.Float:
			return typeof(float);
		case StiDbType.MsSql.Real:
			return typeof(double);
		case StiDbType.MsSql.DateTime:
		case StiDbType.MsSql.SmallDateTime:
		case StiDbType.MsSql.Date:
		case StiDbType.MsSql.DateTime2:
			return typeof(DateTime);
		case StiDbType.MsSql.Time:
			return typeof(TimeSpan);
		case StiDbType.MsSql.DateTimeOffset:
			return typeof(DateTimeOffset);
		case StiDbType.MsSql.Bit:
			return typeof(bool);
		case StiDbType.MsSql.Binary:
			return typeof(byte[]);
		default:
			return typeof(string);
		}
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return "[" + name + "]";
	}

	public override string GetSampleConnectionString()
	{
		return "Integrated Security=False; Data Source=myServerAddress;" + Environment.NewLine + "Initial Catalog=myDataBase; User ID=myUsername; Password=myPassword;";
	}

	public override Type GetDbType()
	{
		return typeof(SqlDbType);
	}

	public static StiMsSqlConnector Get(string connectionString = null)
	{
		return new StiMsSqlConnector(connectionString);
	}

	public StiMsSqlConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.SqlClient.SqlConnection.dll";
		base.TypeConnection = typeof(SqlConnection).ToString();
		base.TypeDataAdapter = typeof(SqlDataAdapter).ToString();
		base.TypeCommand = typeof(SqlCommand).ToString();
		base.TypeParameter = typeof(SqlParameter).ToString();
		base.TypeCommandBuilder = typeof(SqlCommandBuilder).ToString();
	}
}
