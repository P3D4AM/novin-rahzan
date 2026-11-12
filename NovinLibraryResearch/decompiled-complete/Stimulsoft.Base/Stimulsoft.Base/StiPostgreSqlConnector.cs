using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiPostgreSqlConnector : StiSqlDataConnector
{
	private static object lockObject = new object();

	private static bool? isGeneric = null;

	private static bool? isDevart = null;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.PostgreSqlDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.PostgreSqlDataSource;

	public override string Name => "PostgreSQL";

	public override Type SqlType => typeof(StiDbType.PostgreSql);

	public override int DefaultSqlType => 22;

	public override string[] NuGetPackages => new string[1] { "Npgsql" };

	public override string NuGetVersion => "3.2.7";

	public override void ResetSettings()
	{
		isGeneric = null;
		isDevart = null;
	}

	public override StiDataConnector[] GetFamilyConnectors()
	{
		return new StiDataConnector[2]
		{
			new StiPostgreSqlConnector(),
			new StiPostgreSqlDevartConnector()
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
				string text = ExtractDataBaseNameFromConnectionString();
				string retrieveSchemaNamePostgreSql = StiBaseOptions.RetrieveSchemaNamePostgreSql;
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables", new string[4] { text, retrieveSchemaNamePostgreSql, null, null });
					foreach (StiSchemaRow item2 in StiSchemaRow.All(schema))
					{
						string name = ((retrieveSchemaNamePostgreSql != "public") ? $"{item2.TABLE_SCHEMA}.{item2.TABLE_NAME}" : item2.TABLE_NAME);
						StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTableOrView(name);
						hashtable[item2.TABLE_NAME] = stiDataTableSchema;
						if (item2.TABLE_TYPE == "BASE TABLE")
						{
							stiDataSchema.Tables.Add(stiDataTableSchema);
						}
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Views", new string[4] { text, retrieveSchemaNamePostgreSql, null, null });
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema2))
					{
						string name2 = ((retrieveSchemaNamePostgreSql != "public") ? $"{item3.TABLE_SCHEMA}.{item3.TABLE_NAME}" : item3.TABLE_NAME);
						StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewTableOrView(name2);
						hashtable[item3.TABLE_NAME] = stiDataTableSchema2;
						stiDataSchema.Views.Add(stiDataTableSchema2);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema3 = dbConnection.GetSchema("Columns", new string[3] { text, retrieveSchemaNamePostgreSql, null });
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema3))
					{
						StiDataColumnSchema item = new StiDataColumnSchema(item4.COLUMN_NAME, GetNetType(item4.DATA_TYPE));
						if (hashtable[item4.TABLE_NAME] is StiDataTableSchema stiDataTableSchema3)
						{
							stiDataTableSchema3.Columns.Add(item);
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
						using DbCommand dbCommand = CreateCommand(storedProcedure.Query, dbConnection, CommandType.StoredProcedure);
						DeriveParameters(dbCommand);
						using DbDataReader reader = dbCommand.ExecuteReader(CommandBehavior.SchemaOnly);
						using DataTable dataTable = new DataTable(storedProcedure.Name);
						dataTable.Load(reader);
						foreach (DataColumn column in dataTable.Columns)
						{
							storedProcedure.Columns.Add(new StiDataColumnSchema(column.ColumnName, column.DataType));
						}
						foreach (DbParameter parameter in dbCommand.Parameters)
						{
							if (parameter.Direction == ParameterDirection.Input)
							{
								storedProcedure.Parameters.Add(new StiDataParameterSchema(parameter.ParameterName, StiDbTypeConversion.GetNetType(parameter.DbType)));
							}
						}
					}
					catch
					{
					}
				}
				try
				{
					string query = "\r\n                            SELECT\r\n                                tc.constraint_name, tc.table_name, kcu.column_name, \r\n                                ccu.table_name AS foreign_table_name,\r\n                                ccu.column_name AS foreign_column_name \r\n                            FROM \r\n                                information_schema.table_constraints AS tc \r\n                                JOIN information_schema.key_column_usage AS kcu\r\n                                  ON tc.constraint_name = kcu.constraint_name\r\n                                JOIN information_schema.constraint_column_usage AS ccu\r\n                                  ON ccu.constraint_name = tc.constraint_name\r\n                            WHERE constraint_type = 'FOREIGN KEY'";
					using DataSet dataSet = new DataSet();
					using DbDataAdapter dbDataAdapter = CreateAdapter(query, dbConnection);
					dbDataAdapter.Fill(dataSet);
					DataTable dataTable2 = ((dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null);
					if (dataTable2 != null)
					{
						foreach (StiSchemaRow item5 in StiSchemaRow.All(dataTable2))
						{
							stiDataSchema.Relations.Add(new StiDataRelationSchema
							{
								Name = item5.CONSTRAINT_NAME,
								ParentSourceName = item5.TABLE_NAME,
								ChildSourceName = item5.FOREIGN_TABLE_NAME,
								ParentColumns = new List<string> { item5.COLUMN_NAME },
								ChildColumns = new List<string> { item5.FOREIGN_COLUMN_NAME }
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
			return 7;
		}
		if (type == typeof(TimeSpan))
		{
			return 30;
		}
		if (type == typeof(long))
		{
			return 1;
		}
		if (type == typeof(int))
		{
			return 9;
		}
		if (type == typeof(short))
		{
			return 18;
		}
		if (type == typeof(byte))
		{
			return 18;
		}
		if (type == typeof(ulong))
		{
			return 1;
		}
		if (type == typeof(uint))
		{
			return 9;
		}
		if (type == typeof(ushort))
		{
			return 18;
		}
		if (type == typeof(sbyte))
		{
			return 18;
		}
		if (type == typeof(float))
		{
			return 17;
		}
		if (type == typeof(double))
		{
			return 8;
		}
		if (type == typeof(decimal))
		{
			return 13;
		}
		if (type == typeof(string))
		{
			return 19;
		}
		if (type == typeof(bool))
		{
			return 2;
		}
		if (type == typeof(char))
		{
			return 6;
		}
		if (type == typeof(byte[]))
		{
			return 4;
		}
		if (type == typeof(Guid))
		{
			return 27;
		}
		return 9;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.PostgreSql)dbType)
		{
		case StiDbType.PostgreSql.Bigint:
		case StiDbType.PostgreSql.Integer:
		case StiDbType.PostgreSql.Numeric:
			return typeof(long);
		case StiDbType.PostgreSql.Smallint:
			return typeof(short);
		case StiDbType.PostgreSql.Money:
			return typeof(decimal);
		case StiDbType.PostgreSql.Real:
			return typeof(float);
		case StiDbType.PostgreSql.Double:
			return typeof(double);
		case StiDbType.PostgreSql.Date:
		case StiDbType.PostgreSql.Timestamp:
		case StiDbType.PostgreSql.TimestampTZ:
		case StiDbType.PostgreSql.Abstime:
			return typeof(DateTime);
		case StiDbType.PostgreSql.Time:
			return typeof(TimeSpan);
		case StiDbType.PostgreSql.TimeTZ:
			return typeof(DateTimeOffset);
		case StiDbType.PostgreSql.Boolean:
			return typeof(bool);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "bigint":
		case "int":
		case "int4":
		case "int8":
		case "integer":
		case "numeric":
		case "uniqueidentifier":
			return typeof(long);
		case "smallint":
			return typeof(short);
		case "tinyint":
			return typeof(sbyte);
		case "decimal":
		case "money":
		case "smallmoney":
			return typeof(decimal);
		case "float":
		case "real":
			return typeof(float);
		case "double":
			return typeof(double);
		case "abstime":
		case "date":
		case "datetime":
		case "smalldatetime":
		case "timestamp":
		case "timestamptz":
			return typeof(DateTime);
		case "time":
			return typeof(TimeSpan);
		case "timetz":
			return typeof(DateTimeOffset);
		case "boolean":
			return typeof(bool);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Server=myServerAddress; Port=5432; Database=myDataBase;" + Environment.NewLine + "User Id=myUsername; Password=myPassword;";
	}

	private string ExtractDataBaseNameFromConnectionString()
	{
		string result = string.Empty;
		string[] array = base.ConnectionString.Split(new char[1] { ';' });
		string[] array2 = array;
		int num = 0;
		if (num < array2.Length)
		{
			string text = array2[num];
			string[] array3 = text.Split(new char[1] { '=' });
			if (array3.Length == 2 && array3[0] != null && array3[1] != null && array3[0].ToLower().Contains("database"))
			{
				result = array3[1].Trim(new char[1] { ' ' });
			}
		}
		return result;
	}

	public static StiPostgreSqlConnector Get(string connectionString = null)
	{
		lock (lockObject)
		{
			if (isGeneric == true)
			{
				return new StiPostgreSqlConnector(connectionString);
			}
			if (isDevart == true)
			{
				return new StiPostgreSqlDevartConnector(connectionString);
			}
			if (connectionString == null)
			{
				return new StiPostgreSqlConnector();
			}
			if (isGeneric != true && isDevart != true)
			{
				isGeneric = null;
				isDevart = null;
			}
			if (!isGeneric.HasValue)
			{
				StiPostgreSqlConnector result = LoadGenericConnector(connectionString);
				if (isGeneric == true)
				{
					return result;
				}
			}
			if (!isDevart.HasValue)
			{
				StiPostgreSqlDevartConnector result2 = LoadDevartConnector(connectionString);
				if (isDevart == true)
				{
					return result2;
				}
			}
			isGeneric = true;
			return new StiPostgreSqlConnector(connectionString);
		}
	}

	public static StiPostgreSqlDevartConnector LoadDevartConnector(string connectionString = null)
	{
		StiPostgreSqlDevartConnector stiPostgreSqlDevartConnector = new StiPostgreSqlDevartConnector(connectionString);
		isDevart = stiPostgreSqlDevartConnector.IsAvailable;
		return stiPostgreSqlDevartConnector;
	}

	public static StiPostgreSqlConnector LoadGenericConnector(string connectionString = null)
	{
		StiPostgreSqlConnector stiPostgreSqlConnector = new StiPostgreSqlConnector(connectionString);
		isGeneric = stiPostgreSqlConnector.IsAvailable;
		return stiPostgreSqlConnector;
	}

	public StiPostgreSqlConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Npgsql.dll";
		base.TypeConnection = "Npgsql.NpgsqlConnection";
		base.TypeDataAdapter = "Npgsql.NpgsqlDataAdapter";
		base.TypeCommand = "Npgsql.NpgsqlCommand";
		base.TypeParameter = "Npgsql.NpgsqlParameter";
		base.TypeDbType = "NpgsqlTypes.NpgsqlDbType";
		base.TypeCommandBuilder = "Npgsql.NpgsqlCommandBuilder";
	}
}
