using System;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiSybaseAdsConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.SybaseAdsDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.SybaseAdsDataSource;

	public override string Name => "Sybase ADS";

	public override Type SqlType => null;

	public override int DefaultSqlType => 0;

	public override string[] NuGetPackages => null;

	public override string NuGetVersion => null;

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
				DataTable dataTable = new DataTable("Tables");
				try
				{
					using DbDataAdapter dbDataAdapter = CreateAdapter("SELECT id, name FROM sysobjects where type = 'U'", dbConnection);
					dbDataAdapter.Fill(dataTable);
					foreach (DataRow row in dataTable.Rows)
					{
						StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(row["name"] as string, this);
						DataTable dataTable2 = new DataTable("Columns");
						using (DbDataAdapter dbDataAdapter2 = CreateAdapter("SELECT name, type FROM syscolumns where id = " + row["id"], dbConnection))
						{
							dbDataAdapter2.Fill(dataTable2);
							foreach (DataRow row2 in dataTable2.Rows)
							{
								Type netType = GetNetType((byte)row2["type"]);
								stiDataTableSchema.Columns.Add(new StiDataColumnSchema(row2["name"].ToString(), netType));
							}
						}
						stiDataSchema.Tables.Add(stiDataTableSchema);
					}
				}
				catch
				{
				}
				DataTable dataTable3 = new DataTable("Views");
				try
				{
					using DbDataAdapter dbDataAdapter3 = CreateAdapter("SELECT name FROM sysobjects where type = 'V'", dbConnection);
					dbDataAdapter3.Fill(dataTable3);
					foreach (DataRow row3 in dataTable3.Rows)
					{
						StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewView(row3[0] as string, this);
						DataTable dataTable4 = new DataTable("Columns");
						using (DbDataAdapter dbDataAdapter4 = CreateAdapter("SELECT name, type FROM syscolumns where id = " + row3["id"], dbConnection))
						{
							dbDataAdapter4.Fill(dataTable4);
							foreach (DataRow row4 in dataTable4.Rows)
							{
								Type netType2 = GetNetType((byte)row4["type"]);
								stiDataTableSchema2.Columns.Add(new StiDataColumnSchema(row4["name"].ToString(), netType2));
							}
						}
						stiDataSchema.Views.Add(stiDataTableSchema2);
					}
				}
				catch
				{
				}
				DataTable dataTable5 = new DataTable("Procedures");
				try
				{
					using DbDataAdapter dbDataAdapter5 = CreateAdapter("SELECT name FROM sysobjects where type = 'P'", dbConnection);
					dbDataAdapter5.Fill(dataTable5);
					foreach (DataRow row5 in dataTable5.Rows)
					{
						StiDataTableSchema item = StiDataTableSchema.NewProcedure(row5[0] as string, this);
						stiDataSchema.StoredProcedures.Add(item);
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
			return 91;
		}
		if (type == typeof(TimeSpan))
		{
			return -203;
		}
		if (type == typeof(long))
		{
			return -5;
		}
		if (type == typeof(int))
		{
			return 4;
		}
		if (type == typeof(short))
		{
			return 5;
		}
		if (type == typeof(sbyte))
		{
			return 5;
		}
		if (type == typeof(ulong))
		{
			return -5;
		}
		if (type == typeof(uint))
		{
			return 4;
		}
		if (type == typeof(ushort))
		{
			return 5;
		}
		if (type == typeof(byte))
		{
			return -6;
		}
		if (type == typeof(float))
		{
			return 7;
		}
		if (type == typeof(double))
		{
			return 8;
		}
		if (type == typeof(decimal))
		{
			return 3;
		}
		if (type == typeof(string))
		{
			return 12;
		}
		if (type == typeof(bool))
		{
			return -7;
		}
		if (type == typeof(char))
		{
			return 12;
		}
		if (type == typeof(byte[]))
		{
			return -2;
		}
		return 12;
	}

	public override Type GetNetType(string dbType)
	{
		throw new NotSupportedException();
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Sybase)dbType)
		{
		case StiDbType.Sybase.BigInt:
			return typeof(long);
		case StiDbType.Sybase.Integer:
			return typeof(int);
		case StiDbType.Sybase.SmallInt:
			return typeof(short);
		case StiDbType.Sybase.TinyInt:
			return typeof(short);
		case StiDbType.Sybase.UnsignedBigInt:
			return typeof(ulong);
		case StiDbType.Sybase.UnsignedInt:
			return typeof(uint);
		case StiDbType.Sybase.UnsignedSmallInt:
			return typeof(ushort);
		case StiDbType.Sybase.Bit:
			return typeof(bool);
		case StiDbType.Sybase.SmallMoney:
		case StiDbType.Sybase.Money:
		case StiDbType.Sybase.Decimal:
			return typeof(decimal);
		case StiDbType.Sybase.Double:
			return typeof(double);
		case StiDbType.Sybase.Numeric:
		case StiDbType.Sybase.Real:
			return typeof(float);
		case StiDbType.Sybase.SmallDateTime:
		case StiDbType.Sybase.Date:
		case StiDbType.Sybase.BigDateTime:
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return "`" + name + "`";
	}

	public override string GetSampleConnectionString()
	{
		return "Data Source=\\\\myserver\\myvolume\\mypat\\mydd.add;User ID=myUsername;Password=myPassword;ServerType=REMOTE;";
	}

	public static StiSybaseAdsConnector Get(string connectionString = null)
	{
		return new StiSybaseAdsConnector(connectionString);
	}

	public StiSybaseAdsConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Advantage.Data.Provider.dll";
		base.TypeConnection = "Advantage.Data.Provider.AdsConnection";
		base.TypeDataAdapter = "Advantage.Data.Provider.AdsDataAdapter";
		base.TypeCommand = "Advantage.Data.Provider.AdsCommand";
		base.TypeParameter = "Advantage.Data.Provider.AdsParameter";
		base.TypeDbType = typeof(DbType).ToString();
		base.TypeCommandBuilder = "Advantage.Data.Provider.AdsCommandBuilder";
	}
}
