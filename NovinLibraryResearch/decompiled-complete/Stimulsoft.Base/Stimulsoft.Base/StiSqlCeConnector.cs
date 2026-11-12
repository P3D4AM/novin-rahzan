using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiSqlCeConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.SqlCeDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.SqlCeDataSource;

	public override string Name => "SQL CE";

	public override Type SqlType => typeof(StiDbType.SqlCe);

	public override int DefaultSqlType => 22;

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

	public override void SetTimeout(DbCommand command, int timeOut)
	{
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
				List<StiDataTableSchema> list = new List<StiDataTableSchema>();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables");
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema))
					{
						StiDataTableSchema item = StiDataTableSchema.NewTable(item3.TABLE_NAME, this);
						list.Add(item);
						stiDataSchema.Tables.Add(item);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Columns");
					foreach (StiDataTableSchema item4 in list)
					{
						foreach (StiSchemaRow item5 in StiSchemaRow.All(schema2))
						{
							if (item5.TABLE_NAME != null && !(item5.TABLE_NAME != item4.Name))
							{
								StiDataColumnSchema item2 = new StiDataColumnSchema(item5.COLUMN_NAME, GetNetType(item5.DATA_TYPE));
								item4.Columns.Add(item2);
							}
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
			return 0;
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
			return 16;
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
			return 16;
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
			return 16;
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
		return 8;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.SqlCe)dbType)
		{
		case StiDbType.SqlCe.BigInt:
		case StiDbType.SqlCe.UniqueIdentifier:
		case StiDbType.SqlCe.Timestamp:
			return typeof(long);
		case StiDbType.SqlCe.Int:
			return typeof(int);
		case StiDbType.SqlCe.SmallInt:
			return typeof(short);
		case StiDbType.SqlCe.TinyInt:
			return typeof(byte);
		case StiDbType.SqlCe.Decimal:
		case StiDbType.SqlCe.Money:
		case StiDbType.SqlCe.SmallMoney:
			return typeof(decimal);
		case StiDbType.SqlCe.Float:
			return typeof(float);
		case StiDbType.SqlCe.Real:
			return typeof(double);
		case StiDbType.SqlCe.DateTime:
		case StiDbType.SqlCe.SmallDateTime:
		case StiDbType.SqlCe.Date:
		case StiDbType.SqlCe.Time:
		case StiDbType.SqlCe.DateTime2:
			return typeof(DateTime);
		case StiDbType.SqlCe.DateTimeOffset:
			return typeof(DateTimeOffset);
		case StiDbType.SqlCe.Bit:
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
		case "uniqueidentifier":
			return typeof(long);
		case "smallint":
			return typeof(sbyte);
		case "tinyint":
		case "int16":
			return typeof(short);
		case "int32":
		case "int":
			return typeof(int);
		case "int24":
		case "int64":
		case "byte":
			return typeof(byte);
		case "ubyte":
		case "uint16":
			return typeof(ushort);
		case "uint32":
		case "uint24":
			return typeof(uint);
		case "year":
			return typeof(long);
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
		case "timestamp":
		case "datetime":
		case "date":
		case "time":
			return typeof(DateTime);
		case "datetimeoffset":
			return typeof(DateTimeOffset);
		case "image":
			return typeof(byte[]);
		default:
			return typeof(string);
		}
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return $"[{name}]";
	}

	public override string GetSampleConnectionString()
	{
		return "Data Source=c:\\MyData.sdf; Persist Security Info=False;";
	}

	public static StiSqlCeConnector Get(string connectionString = null)
	{
		return new StiSqlCeConnector(connectionString);
	}

	public StiSqlCeConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.SqlServerCe.dll";
		base.TypeConnection = "System.Data.SqlServerCe.SqlCeConnection";
		base.TypeDataAdapter = "System.Data.SqlServerCe.SqlCeDataAdapter";
		base.TypeCommand = "System.Data.SqlServerCe.SqlCeCommand";
		base.TypeParameter = "System.Data.SqlServerCe.SqlCeParameter";
		base.TypeDbType = "System.Data.SqlServerCe.SqlCeType";
	}
}
