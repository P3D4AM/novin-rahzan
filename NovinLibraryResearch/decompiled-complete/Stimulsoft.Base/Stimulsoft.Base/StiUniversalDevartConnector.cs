using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiUniversalDevartConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.UniversalDevartDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.UniversalDevartDataSource;

	public override string Name => "Universal (dotConnect)";

	public override Type SqlType => typeof(StiDbType.Universal);

	public override int DefaultSqlType => 28;

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
				string[] restrictionValues = null;
				switch (dbConnection.GetType().GetProperty("Provider").GetValue(dbConnection, null) as string)
				{
				case "MySQL":
					restrictionValues = new string[1] { dbConnection.Database };
					break;
				case "Oracle":
				case "OracleClient":
					if (dbConnection.GetType().GetProperty("UserId").GetValue(dbConnection, null) is string text)
					{
						restrictionValues = new string[1] { text.ToUpper() };
					}
					break;
				default:
					restrictionValues = new string[0];
					break;
				}
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables", restrictionValues);
					string columnName = "TABLE_NAME";
					foreach (DataRow row in schema.Rows)
					{
						StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(row[columnName] as string, this);
						if (row["TABLE_SCHEMA"] != DBNull.Value && (string)row["TABLE_SCHEMA"] != "dbo")
						{
							stiDataTableSchema = StiDataTableSchema.NewTable(string.Format("{0} ({1})", row["TABLE_NAME"] as string, row["TABLE_SCHEMA"] as string), this);
						}
						hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
						stiDataSchema.Tables.Add(stiDataTableSchema);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Columns");
					foreach (DataRow row2 in schema2.Rows)
					{
						string name = row2["COLUMN_NAME"] as string;
						string key = ((row2["TABLE_SCHEMA"] == DBNull.Value || !((string)row2["TABLE_SCHEMA"] != "sys") || !((string)row2["TABLE_SCHEMA"] != "dbo")) ? (row2["TABLE_NAME"] as string) : string.Format("{0} ({1})", row2["TABLE_NAME"] as string, row2["TABLE_SCHEMA"] as string));
						Type netType = GetNetType(row2["DATA_TYPE"] as string);
						StiDataColumnSchema item = new StiDataColumnSchema(name, netType);
						if (hashtable[key] is StiDataTableSchema stiDataTableSchema2)
						{
							stiDataTableSchema2.Columns.Add(item);
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
			return 11;
		}
		if (type == typeof(TimeSpan))
		{
			return 27;
		}
		if (type == typeof(long))
		{
			return 1;
		}
		if (type == typeof(int))
		{
			return 16;
		}
		if (type == typeof(short))
		{
			return 24;
		}
		if (type == typeof(sbyte))
		{
			return 6;
		}
		if (type == typeof(ulong))
		{
			return 1;
		}
		if (type == typeof(uint))
		{
			return 16;
		}
		if (type == typeof(ushort))
		{
			return 24;
		}
		if (type == typeof(byte))
		{
			return 6;
		}
		if (type == typeof(float))
		{
			return 14;
		}
		if (type == typeof(double))
		{
			return 14;
		}
		if (type == typeof(decimal))
		{
			return 13;
		}
		if (type == typeof(string))
		{
			return 28;
		}
		if (type == typeof(bool))
		{
			return 6;
		}
		if (type == typeof(char))
		{
			return 28;
		}
		if (type == typeof(byte[]))
		{
			return 4;
		}
		return 390;
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "bigint":
		case "uniqueidentifier":
			return typeof(long);
		case "int":
			return typeof(int);
		case "tinyint":
		case "smallint":
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
		case "smalldatetime":
		case "timestamp":
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Universal)dbType)
		{
		case StiDbType.Universal.BigInt:
			return typeof(long);
		case StiDbType.Universal.Int:
			return typeof(int);
		case StiDbType.Universal.SmallInt:
			return typeof(short);
		case StiDbType.Universal.Byte:
		case StiDbType.Universal.TinyInt:
			return typeof(byte);
		case StiDbType.Universal.Currency:
		case StiDbType.Universal.Decimal:
			return typeof(decimal);
		case StiDbType.Universal.Double:
			return typeof(double);
		case StiDbType.Universal.Single:
			return typeof(float);
		case StiDbType.Universal.Date:
		case StiDbType.Universal.DateTime:
		case StiDbType.Universal.TimeStamp:
		case StiDbType.Universal.TimeStampTZ:
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Provider=Oracle;direct=true;data source=192.168.0.1;port=1521;sid=sid;user=user;password=pass";
	}

	public static StiUniversalDevartConnector Get(string connectionString = null)
	{
		return new StiUniversalDevartConnector(connectionString);
	}

	public StiUniversalDevartConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Devart.Data.Universal.dll";
		base.TypeConnection = "Devart.Data.Universal.UniConnection";
		base.TypeDataAdapter = "Devart.Data.Universal.UniDataAdapter";
		base.TypeCommand = "Devart.Data.Universal.UniCommand";
		base.TypeParameter = "Devart.Data.Universal.UniParameter";
		base.TypeDbType = "Devart.Data.Universal.UniDbType";
		base.TypeCommandBuilder = "Devart.Data.Universal.UniCommandBuilder";
		base.TypeConnectionStringBuilder = "Devart.Data.Universal.UniConnectionStringBuilder";
		base.TypeDataSourceEnumerator = "Devart.Data.Universal.UniDataSourceEnumerator";
	}
}
