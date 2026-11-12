using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiFirebirdConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.FirebirdDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.FirebirdDataSource;

	public override string Name => "Firebird";

	public override Type SqlType => typeof(StiDbType.Firebird);

	public override int DefaultSqlType => 13;

	public override string[] NuGetPackages => new string[1] { "FirebirdSql.Data.FirebirdClient" };

	public override string NuGetVersion => "5.9.0.1";

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
				Hashtable hashtable = new Hashtable();
				try
				{
					DataTable schema = dbConnection.GetSchema("Tables", new string[4] { null, null, null, "TABLE" });
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema))
					{
						StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(item3.TABLE_NAME, this);
						hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
						stiDataSchema.Tables.Add(stiDataTableSchema);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema2 = dbConnection.GetSchema("Views");
					foreach (StiSchemaRow item4 in StiSchemaRow.All(schema2))
					{
						StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewView(item4.VIEW_NAME, this);
						hashtable[stiDataTableSchema2.Name] = stiDataTableSchema2;
						stiDataSchema.Views.Add(stiDataTableSchema2);
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema3 = dbConnection.GetSchema("Columns");
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema3))
					{
						StiDataColumnSchema item = new StiDataColumnSchema(item5.COLUMN_NAME, GetNetType(item5.COLUMN_DATA_TYPE));
						if (hashtable[item5.TABLE_NAME] is StiDataTableSchema stiDataTableSchema3)
						{
							stiDataTableSchema3.Columns.Add(item);
						}
					}
				}
				catch
				{
				}
				try
				{
					DataTable schema4 = dbConnection.GetSchema("Procedures");
					foreach (StiSchemaRow item6 in StiSchemaRow.All(schema4))
					{
						StiDataTableSchema item2 = StiDataTableSchema.NewProcedure(item6.PROCEDURE_NAME, this);
						stiDataSchema.StoredProcedures.Add(item2);
					}
				}
				catch
				{
				}
				foreach (StiDataTableSchema storedProcedure in stiDataSchema.StoredProcedures)
				{
					try
					{
						DataTable schema5 = dbConnection.GetSchema("procedureparameters", new string[3] { null, null, storedProcedure.Name });
						foreach (StiSchemaRow item7 in StiSchemaRow.All(schema5))
						{
							if (!(item7.PARAMETER_DIRECTION != "1"))
							{
								storedProcedure.Parameters.Add(new StiDataParameterSchema(item7.PARAMETER_NAME, GetNetType(item7.PARAMETER_DATA_TYPE)));
							}
						}
					}
					catch
					{
					}
				}
				try
				{
					using DataSet dataSet = new DataSet();
					using DbDataAdapter dbDataAdapter = CreateAdapter("\r\n                            SELECT rc.RDB$CONSTRAINT_NAME AS constraint_name,\r\n                            i.RDB$RELATION_NAME AS table_name,\r\n                            s.RDB$FIELD_NAME AS field_name,\r\n                            i2.RDB$RELATION_NAME AS references_table,\r\n                            s2.RDB$FIELD_NAME AS references_field,\r\n                            (s.RDB$FIELD_POSITION + 1) AS field_position\r\n                            FROM RDB$INDEX_SEGMENTS s\r\n                            LEFT JOIN RDB$INDICES i ON i.RDB$INDEX_NAME = s.RDB$INDEX_NAME\r\n                            LEFT JOIN RDB$RELATION_CONSTRAINTS rc ON rc.RDB$INDEX_NAME = s.RDB$INDEX_NAME\r\n                            LEFT JOIN RDB$REF_CONSTRAINTS refc ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME\r\n                            LEFT JOIN RDB$RELATION_CONSTRAINTS rc2 ON rc2.RDB$CONSTRAINT_NAME = refc.RDB$CONST_NAME_UQ\r\n                            LEFT JOIN RDB$INDICES i2 ON i2.RDB$INDEX_NAME = rc2.RDB$INDEX_NAME\r\n                            LEFT JOIN RDB$INDEX_SEGMENTS s2 ON i2.RDB$INDEX_NAME = s2.RDB$INDEX_NAME\r\n                            WHERE rc.RDB$CONSTRAINT_TYPE = 'FOREIGN KEY'\r\n                            ORDER BY i.RDB$RELATION_NAME", dbConnection);
					dbDataAdapter.Fill(dataSet);
					DataTable dataTable = ((dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null);
					if (dataTable != null)
					{
						foreach (StiSchemaRow item8 in StiSchemaRow.All(dataTable))
						{
							stiDataSchema.Relations.Add(new StiDataRelationSchema
							{
								Name = item8.CONSTRAINT_NAME.Trim(),
								ParentSourceName = item8.TABLE_NAME.Trim(),
								ChildSourceName = item8.REFERENCES_TABLE.Trim(),
								ParentColumns = new List<string> { item8.FIELD_NAME.Trim() },
								ChildColumns = new List<string> { item8.REFERENCES_FIELD.Trim() }
							});
						}
					}
				}
				catch
				{
				}
				dbConnection.Close();
			}
			return stiDataSchema;
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
			return 15;
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
			return 12;
		}
		if (type == typeof(byte))
		{
			return 12;
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
			return 12;
		}
		if (type == typeof(sbyte))
		{
			return 12;
		}
		if (type == typeof(float))
		{
			return 8;
		}
		if (type == typeof(double))
		{
			return 7;
		}
		if (type == typeof(decimal))
		{
			return 6;
		}
		if (type == typeof(string))
		{
			return 16;
		}
		if (type == typeof(bool))
		{
			return 12;
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
			return 9;
		}
		return 10;
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.ToLowerInvariant())
		{
		case "bigint":
		case "numeric":
		case "uniqueidentifier":
			return typeof(long);
		case "int":
		case "integer":
			return typeof(int);
		case "smallint":
			return typeof(short);
		case "decimal":
		case "money":
		case "smallmoney":
			return typeof(decimal);
		case "float":
		case "real":
			return typeof(float);
		case "double":
			return typeof(double);
		case "datetime":
		case "date":
		case "smalldatetime":
		case "timestamp":
			return typeof(DateTime);
		case "time":
			return typeof(TimeSpan);
		case "boolean":
			return typeof(bool);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Firebird)dbType)
		{
		case StiDbType.Firebird.BigInt:
		case StiDbType.Firebird.Numeric:
			return typeof(long);
		case StiDbType.Firebird.Integer:
			return typeof(int);
		case StiDbType.Firebird.SmallInt:
			return typeof(short);
		case StiDbType.Firebird.Decimal:
			return typeof(decimal);
		case StiDbType.Firebird.Float:
			return typeof(float);
		case StiDbType.Firebird.Double:
			return typeof(double);
		case StiDbType.Firebird.Date:
		case StiDbType.Firebird.TimeStamp:
			return typeof(DateTime);
		case StiDbType.Firebird.Time:
			return typeof(TimeSpan);
		case StiDbType.Firebird.Boolean:
			return typeof(bool);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "User=SYSDBA; Password=masterkey; Database=SampleDatabase.fdb;" + Environment.NewLine + "DataSource=myServerAddress; Port=3050; Dialect=3; Charset=NONE;" + Environment.NewLine + "Role=; Connection lifetime=15; Pooling=true; MinPoolSize=0;" + Environment.NewLine + "MaxPoolSize=50; Packet Size=8192; ServerType=0;";
	}

	public static StiFirebirdConnector Get(string connectionString = null)
	{
		return new StiFirebirdConnector(connectionString);
	}

	public StiFirebirdConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "FirebirdSql.Data.FirebirdClient.dll";
		base.TypeConnection = "FirebirdSql.Data.FirebirdClient.FbConnection";
		base.TypeDataAdapter = "FirebirdSql.Data.FirebirdClient.FbDataAdapter";
		base.TypeCommand = "FirebirdSql.Data.FirebirdClient.FbCommand";
		base.TypeParameter = "FirebirdSql.Data.FirebirdClient.FbParameter";
		base.TypeDbType = "FirebirdSql.Data.FirebirdClient.FbDbType";
		base.TypeCommandBuilder = "FirebirdSql.Data.FirebirdClient.FbCommandBuilder";
	}
}
