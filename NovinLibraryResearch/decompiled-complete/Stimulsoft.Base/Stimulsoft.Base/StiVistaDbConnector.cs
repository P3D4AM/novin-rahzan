using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Drawing;

namespace Stimulsoft.Base;

public class StiVistaDbConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.VistaDbDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.VistaDbDataSource;

	public override string Name => "VistaDB";

	public override Type SqlType => typeof(StiDbType.VistaDb);

	public override int DefaultSqlType => 3;

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
				DataTable schema = dbConnection.GetSchema("Tables");
				Hashtable hashtable = new Hashtable();
				try
				{
					foreach (StiSchemaRow item2 in StiSchemaRow.All(schema))
					{
						if (item2.TABLE_TYPE != null && !(item2.TABLE_TYPE == "System"))
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
				DataTable schema2 = dbConnection.GetSchema("Views");
				Hashtable hashtable2 = new Hashtable();
				try
				{
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema2))
					{
						StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewView(item3.TABLE_NAME, this);
						hashtable2[stiDataTableSchema2.Name] = stiDataTableSchema2;
						stiDataSchema.Views.Add(stiDataTableSchema2);
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
						if (hashtable[item4.TABLE_NAME] != null)
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
				DataTable schema4 = dbConnection.GetSchema("Procedures");
				Hashtable hashtable3 = new Hashtable();
				try
				{
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema4))
					{
						StiDataTableSchema stiDataTableSchema4 = StiDataTableSchema.NewProcedure(item5.SPECIFIC_NAME, this);
						hashtable3[stiDataTableSchema4.Name] = stiDataTableSchema4;
						stiDataSchema.StoredProcedures.Add(stiDataTableSchema4);
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
			return 28;
		}
		if (type == typeof(TimeSpan))
		{
			return 24;
		}
		if (type == typeof(long))
		{
			return 11;
		}
		if (type == typeof(int))
		{
			return 10;
		}
		if (type == typeof(short))
		{
			return 9;
		}
		if (type == typeof(sbyte))
		{
			return 8;
		}
		if (type == typeof(ulong))
		{
			return 11;
		}
		if (type == typeof(uint))
		{
			return 10;
		}
		if (type == typeof(ushort))
		{
			return 9;
		}
		if (type == typeof(byte))
		{
			return 8;
		}
		if (type == typeof(float))
		{
			return 13;
		}
		if (type == typeof(double))
		{
			return 12;
		}
		if (type == typeof(decimal))
		{
			return 14;
		}
		if (type == typeof(string))
		{
			return 3;
		}
		if (type == typeof(bool))
		{
			return 17;
		}
		if (type == typeof(char))
		{
			return 3;
		}
		if (type == typeof(byte[]))
		{
			return 25;
		}
		return 3;
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType)
		{
		case "BigInt":
		case "UniqueIdentifier":
		case "Timestamp":
			return typeof(long);
		case "Int":
			return typeof(int);
		case "TinyInt":
			return typeof(short);
		case "SmallInt":
			return typeof(sbyte);
		case "Decimal":
		case "Money":
		case "SmallMoney":
			return typeof(decimal);
		case "Float":
			return typeof(float);
		case "Real":
			return typeof(double);
		case "DateTime":
		case "DateTime2":
		case "SmallDateTime":
		case "Date":
			return typeof(DateTime);
		case "DateTimeOffset":
			return typeof(DateTimeOffset);
		case "Timespan":
			return typeof(TimeSpan);
		case "Char":
			return typeof(char);
		case "Image":
			return typeof(Image);
		case "Bit":
			return typeof(bool);
		case "Binary":
			return typeof(byte[]);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.VistaDb)dbType)
		{
		case StiDbType.VistaDb.BigInt:
		case StiDbType.VistaDb.UniqueIdentifier:
			return typeof(long);
		case StiDbType.VistaDb.Int:
			return typeof(int);
		case StiDbType.VistaDb.TinyInt:
			return typeof(short);
		case StiDbType.VistaDb.SmallInt:
			return typeof(sbyte);
		case StiDbType.VistaDb.Decimal:
		case StiDbType.VistaDb.Money:
		case StiDbType.VistaDb.SmallMoney:
			return typeof(decimal);
		case StiDbType.VistaDb.Float:
			return typeof(float);
		case StiDbType.VistaDb.Real:
			return typeof(double);
		case StiDbType.VistaDb.DateTime:
		case StiDbType.VistaDb.SmallDateTime:
		case StiDbType.VistaDb.Date:
		case StiDbType.VistaDb.DateTime2:
			return typeof(DateTime);
		case StiDbType.VistaDb.DateTimeOffset:
			return typeof(DateTime);
		case StiDbType.VistaDb.Timestamp:
			return typeof(TimeSpan);
		case StiDbType.VistaDb.Char:
			return typeof(char);
		case StiDbType.VistaDb.Image:
			return typeof(Image);
		case StiDbType.VistaDb.Bit:
			return typeof(bool);
		case StiDbType.VistaDb.Binary:
			return typeof(byte[]);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Data Source=D:\\folder\\myVistaDatabaseFile.vdb4;Open Mode=ExclusiveReadWrite;";
	}

	public static StiVistaDbConnector Get(string connectionString = null)
	{
		return new StiVistaDbConnector(connectionString);
	}

	public StiVistaDbConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "VistaDB.5.NET40.dll";
		base.TypeConnection = "VistaDB.Provider.VistaDBConnection";
		base.TypeDataAdapter = "VistaDB.Provider.VistaDBDataAdapter";
		base.TypeCommand = "VistaDB.Provider.VistaDBCommand";
		base.TypeParameter = "VistaDB.Provider.VistaDBParameter";
		base.TypeDbType = "VistaDB.VistaDBType";
		base.TypeCommandBuilder = "VistaDB.Provider.VistaDBCommandBuilder";
	}
}
