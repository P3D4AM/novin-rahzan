using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiTeradataConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.TeradataDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.TeradataDataSource;

	public override string Name => "Teradata";

	public override Type SqlType => typeof(StiDbType.Teradata);

	public override int DefaultSqlType => 390;

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
				string[] restrictionValues = new string[1] { dbConnection.Database };
				DataTable schema = dbConnection.GetSchema("Tables", restrictionValues);
				Hashtable hashtable = new Hashtable();
				try
				{
					foreach (StiSchemaRow item2 in StiSchemaRow.All(schema))
					{
						string text = $"{item2.TABLE_SCHEMA}.{item2.TABLE_NAME}";
						StiDataTableSchema item = (StiDataTableSchema)(hashtable[text] = StiDataTableSchema.NewTableOrView(text));
						if (item2.TABLE_TYPE == "TABLE")
						{
							stiDataSchema.Tables.Add(item);
						}
						if (item2.TABLE_TYPE == "VIEW")
						{
							stiDataSchema.Views.Add(item);
						}
					}
				}
				catch
				{
				}
				try
				{
					string[] restrictionValues2 = new string[1] { dbConnection.Database };
					DataTable schema2 = dbConnection.GetSchema("Columns", restrictionValues2);
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema2))
					{
						string key = $"{item3.TABLE_SCHEMA}.{item3.TABLE_NAME}";
						if (hashtable[key] != null)
						{
							DataColumn column = new DataColumn(item3.COLUMN_NAME, GetNetType(item3.COLUMN_TYPE));
							if (hashtable[key] is DataTable dataTable)
							{
								dataTable.Columns.Add(column);
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
			return 150;
		}
		if (type == typeof(TimeSpan))
		{
			return 360;
		}
		if (type == typeof(long))
		{
			return 90;
		}
		if (type == typeof(int))
		{
			return 190;
		}
		if (type == typeof(short))
		{
			return 330;
		}
		if (type == typeof(sbyte))
		{
			return 110;
		}
		if (type == typeof(ulong))
		{
			return 90;
		}
		if (type == typeof(uint))
		{
			return 190;
		}
		if (type == typeof(ushort))
		{
			return 330;
		}
		if (type == typeof(byte))
		{
			return 110;
		}
		if (type == typeof(float))
		{
			return 170;
		}
		if (type == typeof(double))
		{
			return 170;
		}
		if (type == typeof(decimal))
		{
			return 160;
		}
		if (type == typeof(string))
		{
			return 390;
		}
		if (type == typeof(bool))
		{
			return 110;
		}
		if (type == typeof(char))
		{
			return 390;
		}
		if (type == typeof(byte[]))
		{
			return 100;
		}
		return 390;
	}

	public override Type GetNetType(string dbType)
	{
		dbType = dbType.Trim().ToUpper();
		if (dbType.StartsWith("PERIOD"))
		{
			return typeof(string);
		}
		switch (dbType)
		{
		case "SMALLINT":
		case "BYTEINT":
			return typeof(short);
		case "INTEGER":
			return typeof(int);
		case "BIGINT":
			return typeof(long);
		case "BLOB":
		case "BYTE":
		case "VARBYTE":
			return typeof(byte[]);
		case "DECIMAL":
			return typeof(decimal);
		case "NUMBER":
		case "DOUBLE":
			return typeof(double);
		case "INTERVALDAY":
		case "INTERVALDAYTOHOUR":
		case "INTERVALDAYTOMINUTE":
		case "INTERVALDAYTOSECOND":
		case "INTERVALHOUR":
		case "INTERVALHOURTOMINUTE":
		case "INTERVALHOURTOSECOND":
		case "INTERVALMINUTE":
		case "INTERVALMINUTETOSECOND":
		case "INTERVALSECOND":
		case "TIME":
			return typeof(TimeSpan);
		case "DATE":
		case "TIMESTAMP":
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Teradata)dbType)
		{
		case StiDbType.Teradata.AnyType:
			return typeof(object);
		case StiDbType.Teradata.BigInt:
			return typeof(long);
		case StiDbType.Teradata.Integer:
			return typeof(int);
		case StiDbType.Teradata.SmallInt:
			return typeof(short);
		case StiDbType.Teradata.ByteInt:
			return typeof(sbyte);
		case StiDbType.Teradata.Blob:
		case StiDbType.Teradata.Byte:
		case StiDbType.Teradata.VarByte:
			return typeof(byte[]);
		case StiDbType.Teradata.Char:
		case StiDbType.Teradata.Clob:
		case StiDbType.Teradata.Graphic:
		case StiDbType.Teradata.IntervalYear:
		case StiDbType.Teradata.IntervalYearToMonth:
		case StiDbType.Teradata.IntervalMonth:
		case StiDbType.Teradata.TimeWithZone:
		case StiDbType.Teradata.TimestampWithZone:
		case StiDbType.Teradata.VarChar:
		case StiDbType.Teradata.VarGraphic:
		case StiDbType.Teradata.PeriodDate:
		case StiDbType.Teradata.PeriodTime:
		case StiDbType.Teradata.PeriodTimeWithTimeZone:
		case StiDbType.Teradata.PeriodTimestamp:
		case StiDbType.Teradata.PeriodTimestampWithTimeZone:
			return typeof(string);
		case StiDbType.Teradata.Date:
		case StiDbType.Teradata.Timestamp:
			return typeof(DateTime);
		case StiDbType.Teradata.Decimal:
			return typeof(decimal);
		case StiDbType.Teradata.Double:
		case StiDbType.Teradata.Number:
			return typeof(double);
		case StiDbType.Teradata.IntervalDay:
		case StiDbType.Teradata.IntervalDayToHour:
		case StiDbType.Teradata.IntervalDayToMinute:
		case StiDbType.Teradata.IntervalDayToSecond:
		case StiDbType.Teradata.IntervalHour:
		case StiDbType.Teradata.IntervalHourToMinute:
		case StiDbType.Teradata.IntervalHourToSecond:
		case StiDbType.Teradata.IntervalMinute:
		case StiDbType.Teradata.IntervalMinuteToSecond:
		case StiDbType.Teradata.IntervalSecond:
		case StiDbType.Teradata.Time:
			return typeof(TimeSpan);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Data Source=myServerAddress;User ID=myUsername;Password=myPassword;";
	}

	public static StiTeradataConnector Get(string connectionString = null)
	{
		return new StiTeradataConnector(connectionString);
	}

	public StiTeradataConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Teradata.Client.Provider.dll";
		base.TypeConnection = "Teradata.Client.Provider.TdConnection";
		base.TypeDataAdapter = "Teradata.Client.Provider.TdDataAdapter";
		base.TypeCommand = "Teradata.Client.Provider.TdCommand";
		base.TypeParameter = "Teradata.Client.Provider.TdParameter";
		base.TypeDbType = "Teradata.Client.Provider.TdType";
		base.TypeCommandBuilder = "Teradata.Client.Provider.TdCommandBuilder";
	}
}
