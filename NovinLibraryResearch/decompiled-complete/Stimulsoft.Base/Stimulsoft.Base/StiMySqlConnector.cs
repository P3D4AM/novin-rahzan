using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Stimulsoft.Base;

public class StiMySqlConnector : StiSqlDataConnector
{
	private static object lockObject = new object();

	private static bool? isGeneric = null;

	private static bool? isDevart = null;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.MySqlDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.MySqlDataSource;

	public override string Name => "MySQL";

	public override Type SqlType => typeof(StiDbType.MySql);

	public override int DefaultSqlType => 752;

	public override string[] NuGetPackages => new string[1] { "MySql.Data" };

	public override string NuGetVersion => "6.9.12";

	public override void ResetSettings()
	{
		isGeneric = null;
		isDevart = null;
	}

	public override StiDataConnector[] GetFamilyConnectors()
	{
		return new StiDataConnector[2]
		{
			new StiMySqlConnector(),
			new StiMySqlDevartConnector()
		};
	}

	public override DbParameter CreateParameter(string parameterName, object value, int size)
	{
		DbParameter dbParameter = base.AssemblyHelper.CreateParameterWithValue(base.TypeParameter, parameterName, value);
		dbParameter.Size = size;
		return dbParameter;
	}

	public override DbParameter CreateParameter(string parameterName, int type, int size)
	{
		Type dbType = GetDbType();
		DbParameter dbParameter = base.AssemblyHelper.CreateParameterWithType(base.TypeParameter, parameterName, type, dbType ?? typeof(int));
		dbParameter.Size = size;
		return dbParameter;
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
				string text = ((dbConnection.Database != null) ? dbConnection.Database.ToLowerInvariant() : null);
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables");
					foreach (StiSchemaRow item2 in StiSchemaRow.All(schema))
					{
						if (item2.TABLE_SCHEMA != null && !(item2.TABLE_SCHEMA.ToLowerInvariant() != text) && !(item2.TABLE_TYPE != "BASE TABLE"))
						{
							StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(item2.TABLE_NAME, this);
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
						if (item3.TABLE_SCHEMA != null && !(item3.TABLE_SCHEMA.ToLowerInvariant() != text))
						{
							StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewView(item3.TABLE_NAME, this);
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
					DataTable schema3 = dbConnection.GetSchema("Columns");
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema3))
					{
						if (item4.TABLE_SCHEMA != null && !(item4.TABLE_SCHEMA.ToLowerInvariant() != text))
						{
							StiDataColumnSchema item = new StiDataColumnSchema(item4.COLUMN_NAME, GetNetType(item4.DATA_TYPE));
							if (hashtable[item4.TABLE_NAME] is StiDataTableSchema stiDataTableSchema3)
							{
								stiDataTableSchema3.Columns.Add(item);
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
						string text2 = item5.ROUTINE_NAME;
						if (item5.ROUTINE_SCHEMA != null && !(item5.ROUTINE_SCHEMA.ToLowerInvariant() != text) && text2 != null)
						{
							if (text2.IndexOfInvariant(";") != -1)
							{
								text2 = text2.Substring(0, text2.IndexOfInvariant(";"));
							}
							StiDataTableSchema stiDataTableSchema4 = StiDataTableSchema.NewProcedure(text2, this);
							stiDataTableSchema4.Query = StiTableQuery.Get(this).GetProcQuery(text2);
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
						using DbCommand dbCommand = CreateCommand(storedProcedure.Name, dbConnection, CommandType.StoredProcedure);
						DeriveParameters(dbCommand);
						using DbDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SchemaOnly);
						using DataTable dataTable = new DataTable(storedProcedure.Name);
						dataTable.Load(reader);
						foreach (DataColumn column in dataTable.Columns)
						{
							storedProcedure.Columns.Add(new StiDataColumnSchema
							{
								Name = column.ColumnName,
								Type = column.DataType
							});
						}
						if (dbCommand.Parameters.Count <= 0)
						{
							continue;
						}
						StringBuilder stringBuilder = new StringBuilder();
						foreach (DbParameter parameter in dbCommand.Parameters)
						{
							if (parameter.Direction == ParameterDirection.Input)
							{
								storedProcedure.Parameters.Add(new StiDataParameterSchema
								{
									Name = parameter.ParameterName,
									Type = StiDbTypeConversion.GetNetType(parameter.DbType)
								});
								stringBuilder = ((stringBuilder.Length == 0) ? stringBuilder.Append(parameter.ParameterName) : stringBuilder.AppendFormat("{0},", parameter.ParameterName));
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
			return 12;
		}
		if (type == typeof(TimeSpan))
		{
			return 7;
		}
		if (type == typeof(long))
		{
			return 8;
		}
		if (type == typeof(int))
		{
			return 3;
		}
		if (type == typeof(short))
		{
			return 2;
		}
		if (type == typeof(sbyte))
		{
			return 1;
		}
		if (type == typeof(ulong))
		{
			return 508;
		}
		if (type == typeof(uint))
		{
			return 503;
		}
		if (type == typeof(ushort))
		{
			return 502;
		}
		if (type == typeof(byte))
		{
			return 501;
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
			return 0;
		}
		if (type == typeof(string))
		{
			return 253;
		}
		if (type == typeof(bool))
		{
			return 16;
		}
		if (type == typeof(char))
		{
			return 253;
		}
		if (type == typeof(byte[]))
		{
			return 600;
		}
		if (type == typeof(Guid))
		{
			return 800;
		}
		return 3;
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "uniqueidentifier":
		case "bigint":
		case "int64":
		case "year":
			return typeof(long);
		case "int32":
		case "int24":
		case "int":
			return typeof(int);
		case "int16":
		case "smallint":
			return typeof(short);
		case "byte":
		case "ubyte":
			return typeof(byte);
		case "uint32":
		case "uint24":
			return typeof(uint);
		case "uint16":
			return typeof(ushort);
		case "tinyint":
			return typeof(sbyte);
		case "uint64":
			return typeof(ulong);
		case "decimal":
		case "newdecimal":
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
		case "newdatetime":
		case "smalldatetime":
		case "datetime":
		case "date":
		case "timestamp":
			return typeof(DateTime);
		case "time":
			return typeof(TimeSpan);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.MySql)dbType)
		{
		case StiDbType.MySql.Int64:
		case StiDbType.MySql.Year:
			return typeof(long);
		case StiDbType.MySql.Int32:
			return typeof(int);
		case StiDbType.MySql.Int16:
			return typeof(short);
		case StiDbType.MySql.Byte:
			return typeof(sbyte);
		case StiDbType.MySql.UInt64:
			return typeof(ulong);
		case StiDbType.MySql.UInt32:
			return typeof(uint);
		case StiDbType.MySql.UInt16:
			return typeof(ushort);
		case StiDbType.MySql.UByte:
			return typeof(byte);
		case StiDbType.MySql.Decimal:
			return typeof(decimal);
		case StiDbType.MySql.Float:
			return typeof(float);
		case StiDbType.MySql.Double:
			return typeof(double);
		case StiDbType.MySql.Bit:
			return typeof(bool);
		case StiDbType.MySql.Timestamp:
		case StiDbType.MySql.Date:
		case StiDbType.MySql.DateTime:
			return typeof(DateTime);
		case StiDbType.MySql.Time:
			return typeof(TimeSpan);
		default:
			return typeof(string);
		}
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return $"`{name}`";
	}

	public static StiMySqlConnector Get(string connectionString = null)
	{
		lock (lockObject)
		{
			if (isGeneric == true)
			{
				return new StiMySqlConnector(connectionString);
			}
			if (isDevart == true)
			{
				return new StiMySqlDevartConnector(connectionString);
			}
			if (connectionString == null)
			{
				return new StiMySqlConnector();
			}
			if (isGeneric != true && isDevart != true)
			{
				isGeneric = null;
				isDevart = null;
			}
			if (!isGeneric.HasValue)
			{
				StiMySqlConnector result = LoadGenericMySqlConnector(connectionString);
				if (isGeneric == true)
				{
					return result;
				}
			}
			if (!isDevart.HasValue)
			{
				StiMySqlDevartConnector result2 = LoadDevartConnector(connectionString);
				if (isDevart == true)
				{
					return result2;
				}
			}
			isGeneric = true;
			return new StiMySqlConnector(connectionString);
		}
	}

	private static StiMySqlDevartConnector LoadDevartConnector(string connectionString = null)
	{
		StiMySqlDevartConnector stiMySqlDevartConnector = new StiMySqlDevartConnector(connectionString);
		isDevart = stiMySqlDevartConnector.IsAvailable;
		return stiMySqlDevartConnector;
	}

	private static StiMySqlConnector LoadGenericMySqlConnector(string connectionString = null)
	{
		StiMySqlConnector stiMySqlConnector = new StiMySqlConnector(connectionString);
		isGeneric = stiMySqlConnector.IsAvailable;
		return stiMySqlConnector;
	}

	public static void SetDevart()
	{
		isDevart = true;
	}

	public override string GetSampleConnectionString()
	{
		return "Server=myServerAddress; Database=myDataBase;" + Environment.NewLine + "UserId=myUsername; Pwd=myPassword;";
	}

	public StiMySqlConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "MySql.Data.dll";
		base.TypeConnection = "MySql.Data.MySqlClient.MySqlConnection";
		base.TypeDataAdapter = "MySql.Data.MySqlClient.MySqlDataAdapter";
		base.TypeCommand = "MySql.Data.MySqlClient.MySqlCommand";
		base.TypeParameter = "MySql.Data.MySqlClient.MySqlParameter";
		base.TypeDbType = "MySql.Data.MySqlClient.MySqlDbType";
		base.TypeCommandBuilder = "MySql.Data.MySqlClient.MySqlCommandBuilder";
	}
}
