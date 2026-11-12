using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiSqLiteConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.SqLiteDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.SqLiteDataSource;

	public override string Name => "SQLite";

	public override Type SqlType => typeof(StiDbType.SqLite);

	public override int DefaultSqlType => 3;

	public override string[] NuGetPackages => new string[1] { "System.Data.SQLite" };

	public override string NuGetVersion => "1.0.113.1";

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
				List<StiDataTableSchema> list = new List<StiDataTableSchema>();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables");
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema))
					{
						StiDataTableSchema item = StiDataTableSchema.NewTable(item4.TABLE_NAME, this);
						list.Add(item);
						stiDataSchema.Tables.Add(item);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Views");
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema2))
					{
						StiDataTableSchema item2 = StiDataTableSchema.NewView(item5.TABLE_NAME, this);
						list.Add(item2);
						stiDataSchema.Views.Add(item2);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema3 = dbConnection.GetSchema("Columns");
					foreach (StiDataTableSchema item6 in list)
					{
						foreach (StiSchemaRow item7 in StiSchemaRow.All(schema3))
						{
							if (item7.TABLE_NAME != null && !(item7.TABLE_NAME != item6.Name))
							{
								StiDataColumnSchema item3 = new StiDataColumnSchema(item7.COLUMN_NAME, GetNetType(item7.DATA_TYPE));
								item6.Columns.Add(item3);
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
			return 10;
		}
		if (type == typeof(TimeSpan))
		{
			return 1;
		}
		if (type == typeof(long))
		{
			return 1;
		}
		if (type == typeof(int))
		{
			return 1;
		}
		if (type == typeof(short))
		{
			return 1;
		}
		if (type == typeof(byte))
		{
			return 1;
		}
		if (type == typeof(ulong))
		{
			return 1;
		}
		if (type == typeof(uint))
		{
			return 1;
		}
		if (type == typeof(ushort))
		{
			return 1;
		}
		if (type == typeof(sbyte))
		{
			return 1;
		}
		if (type == typeof(float))
		{
			return 2;
		}
		if (type == typeof(double))
		{
			return 2;
		}
		if (type == typeof(decimal))
		{
			return 2;
		}
		if (type == typeof(string))
		{
			return 3;
		}
		if (type == typeof(bool))
		{
			return 1;
		}
		if (type == typeof(char))
		{
			return 3;
		}
		if (type == typeof(byte[]))
		{
			return 4;
		}
		if (type == typeof(Guid))
		{
			return 3;
		}
		return 1;
	}

	public override Type GetNetType(int dbType)
	{
		return (StiDbType.SqLite)dbType switch
		{
			StiDbType.SqLite.Int64 => typeof(long), 
			StiDbType.SqLite.Blob => typeof(byte[]), 
			StiDbType.SqLite.DateTime => typeof(DateTime), 
			StiDbType.SqLite.Double => typeof(double), 
			StiDbType.SqLite.Text => typeof(long), 
			_ => typeof(string), 
		};
	}

	public override Type GetNetType(string dbType)
	{
		return dbType.ToLowerInvariant() switch
		{
			"int64" => typeof(long), 
			"double" => typeof(double), 
			"datetime" => typeof(DateTime), 
			"blob" => typeof(byte[]), 
			_ => typeof(string), 
		};
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return $"`{name}`";
	}

	public override string GetSampleConnectionString()
	{
		return "Data Source=c:\\mydb.db; Version=3;";
	}

	public static StiSqLiteConnector Get(string connectionString = null)
	{
		return new StiSqLiteConnector(connectionString);
	}

	public StiSqLiteConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.SQLite.dll";
		base.TypeConnection = "System.Data.SQLite.SQLiteConnection";
		base.TypeDataAdapter = "System.Data.SQLite.SQLiteDataAdapter";
		base.TypeCommand = "System.Data.SQLite.SQLiteCommand";
		base.TypeParameter = "System.Data.SQLite.SQLiteParameter";
		base.TypeDbType = "System.Data.SQLite.SQLiteType";
	}
}
