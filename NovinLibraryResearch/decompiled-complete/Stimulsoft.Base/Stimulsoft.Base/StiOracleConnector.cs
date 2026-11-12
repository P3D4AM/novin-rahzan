using System;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiOracleConnector : StiSqlDataConnector
{
	private static object lockObject = new object();

	private static bool? isGeneric = null;

	private static bool? isManaged = null;

	private static bool? isDevart = null;

	private static bool? isClient = null;

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.OracleDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.OracleDataSource;

	public override string Name => "Oracle";

	public override Type SqlType => typeof(StiDbType.Oracle);

	public override int DefaultSqlType => 119;

	public override void ResetSettings()
	{
		isGeneric = null;
		isManaged = null;
		isDevart = null;
		isClient = null;
	}

	public override StiDataConnector[] GetFamilyConnectors()
	{
		return new StiDataConnector[4]
		{
			new StiOracleConnector(),
			new StiOracleManagedConnector(),
			new StiOracleDevartConnector(),
			new StiOracleClientConnector()
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
				DataTable schema = dbConnection.GetSchema("Tables");
				Hashtable hashtable = new Hashtable();
				try
				{
					foreach (StiSchemaRow item2 in StiSchemaRow.All(schema))
					{
						if (!(item2.TYPE == "System") && !IsSystemOwner(item2.OWNER))
						{
							string name = StiTableName.GetName(item2.OWNER, item2.TABLE_NAME);
							string selectQuery = StiTableQuery.Get(this).GetSelectQuery(item2.OWNER, item2.TABLE_NAME);
							StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTableOrView(name, this, selectQuery);
							hashtable[stiDataTableSchema.Name] = stiDataTableSchema;
							stiDataSchema.Tables.Add(stiDataTableSchema);
						}
					}
				}
				catch
				{
				}
				DataTable schema2 = dbConnection.GetSchema("Views");
				try
				{
					foreach (StiSchemaRow item3 in StiSchemaRow.All(schema2))
					{
						if (!IsSystemOwner(item3.OWNER))
						{
							string name2 = StiTableName.GetName(item3.OWNER, item3.VIEW_NAME);
							string selectQuery2 = StiTableQuery.Get(this).GetSelectQuery(item3.OWNER, item3.VIEW_NAME);
							StiDataTableSchema stiDataTableSchema2 = StiDataTableSchema.NewTableOrView(name2, this, selectQuery2);
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
						if (!IsSystemOwner(item4.OWNER))
						{
							string name3 = StiTableName.GetName(item4.OWNER, item4.TABLE_NAME);
							StiDataColumnSchema item = new StiDataColumnSchema(item4.COLUMN_NAME, GetNetType(item4.DATATYPE));
							if (hashtable[name3] is StiDataTableSchema stiDataTableSchema3)
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
				Hashtable hashtable2 = new Hashtable();
				try
				{
					foreach (StiSchemaRow item5 in StiSchemaRow.All(schema4))
					{
						if (!(item5.OWNER == "SYS") && !IsSystemOwner(item5.OWNER))
						{
							string name4 = StiTableName.GetName(item5.OWNER, item5.OBJECT_NAME);
							string procQuery = StiTableQuery.Get(this).GetProcQuery(item5.OWNER, item5.OBJECT_NAME);
							StiDataTableSchema stiDataTableSchema4 = StiDataTableSchema.NewProcedure(name4, this, procQuery);
							hashtable2[stiDataTableSchema4.Name] = stiDataTableSchema4;
							stiDataSchema.StoredProcedures.Add(stiDataTableSchema4);
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
			return 106;
		}
		if (type == typeof(TimeSpan))
		{
			return 123;
		}
		if (type == typeof(long))
		{
			return 113;
		}
		if (type == typeof(int))
		{
			return 112;
		}
		if (type == typeof(short))
		{
			return 111;
		}
		if (type == typeof(byte))
		{
			return 103;
		}
		if (type == typeof(ulong))
		{
			return 113;
		}
		if (type == typeof(uint))
		{
			return 112;
		}
		if (type == typeof(ushort))
		{
			return 111;
		}
		if (type == typeof(sbyte))
		{
			return 103;
		}
		if (type == typeof(float))
		{
			return 122;
		}
		if (type == typeof(double))
		{
			return 108;
		}
		if (type == typeof(decimal))
		{
			return 107;
		}
		if (type == typeof(string))
		{
			return 126;
		}
		if (type == typeof(bool))
		{
			return 103;
		}
		if (type == typeof(char))
		{
			return 104;
		}
		if (type == typeof(byte[]))
		{
			return 102;
		}
		if (type == typeof(Guid))
		{
			return 120;
		}
		return 112;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Oracle)dbType)
		{
		case StiDbType.Oracle.Date:
			return typeof(byte[]);
		case StiDbType.Oracle.BFile:
		case StiDbType.Oracle.Blob:
		case StiDbType.Oracle.LongRaw:
		case StiDbType.Oracle.Raw:
			return typeof(byte[]);
		case StiDbType.Oracle.Single:
			return typeof(float);
		case StiDbType.Oracle.Double:
			return typeof(double);
		case StiDbType.Oracle.Decimal:
			return typeof(decimal);
		case StiDbType.Oracle.Byte:
			return typeof(byte);
		case StiDbType.Oracle.Int16:
			return typeof(short);
		case StiDbType.Oracle.Int32:
			return typeof(int);
		case StiDbType.Oracle.Long:
		case StiDbType.Oracle.Int64:
		case StiDbType.Oracle.IntervalDS:
		case StiDbType.Oracle.IntervalYM:
			return typeof(long);
		default:
			return typeof(string);
		}
	}

	public override Type GetNetType(string dbType)
	{
		switch (dbType.Replace(" ", "").ToUpperInvariant())
		{
		case "BFILE":
		case "BLOB":
		case "LONGRAW":
		case "RAW":
			return typeof(byte[]);
		case "DATE":
		case "TIMESTAMP":
		case "TIMESTAMPWITHLOCALTIMEZONE":
		case "TIMESTAMPWITHTIMEZONE":
			return typeof(DateTime);
		case "INTERVALDAYTOSECOND":
			return typeof(TimeSpan);
		case "INTERVALYEARTOMONTH":
			return typeof(long);
		case "NUMBER":
		case "BINARY_DOUBLE":
		case "BINARY_FLOAT":
		case "BINARY_INTEGER":
			return typeof(decimal);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Data Source=TORCL;User Id=myUsername;Password=myPassword;";
	}

	public override string GetDatabaseSpecificName(string name)
	{
		return name;
	}

	private bool IsSystemOwner(string owner)
	{
		owner = owner.ToUpper();
		switch (owner)
		{
		case "SYS":
		case "SYSMAN":
		case "SYSTEM":
		case "WMSYS":
		case "EXFSYS":
		case "ORDSYS":
		case "MDSYS":
		case "XDB":
		case "OUTLN":
		case "CTXMGR":
		case "OEMGR":
			return true;
		default:
			return false;
		}
	}

	public static StiOracleConnector Get(string connectionString = null)
	{
		lock (lockObject)
		{
			if (isGeneric == true)
			{
				return new StiOracleConnector(connectionString);
			}
			if (isManaged == true)
			{
				return new StiOracleManagedConnector(connectionString);
			}
			if (isDevart == true)
			{
				return new StiOracleDevartConnector(connectionString);
			}
			if (isClient == true)
			{
				return new StiOracleClientConnector(connectionString);
			}
			if (connectionString == null)
			{
				return new StiOracleConnector();
			}
			if (isGeneric != true && isDevart != true && isClient != true && isManaged != true)
			{
				isGeneric = null;
				isManaged = null;
				isDevart = null;
				isClient = null;
			}
			if (!isGeneric.HasValue)
			{
				StiOracleConnector result = LoadGenericConnector(connectionString);
				if (isGeneric == true)
				{
					return result;
				}
			}
			if (!isManaged.HasValue)
			{
				StiOracleManagedConnector result2 = LoadManagedConnector(connectionString);
				if (isManaged == true)
				{
					return result2;
				}
			}
			if (!isDevart.HasValue)
			{
				StiOracleDevartConnector result3 = LoadDevartConnector(connectionString);
				if (isDevart == true)
				{
					return result3;
				}
			}
			if (!isClient.HasValue && StiDataOptions.AllowUseOracleClientConnector)
			{
				StiOracleClientConnector result4 = LoadClientConnector(connectionString);
				if (isClient == true)
				{
					return result4;
				}
			}
			isGeneric = true;
			return new StiOracleConnector(connectionString);
		}
	}

	public static StiOracleClientConnector LoadClientConnector(string connectionString = null)
	{
		StiOracleClientConnector stiOracleClientConnector = new StiOracleClientConnector(connectionString);
		isClient = stiOracleClientConnector.IsAvailable;
		return stiOracleClientConnector;
	}

	public static StiOracleDevartConnector LoadDevartConnector(string connectionString = null)
	{
		StiOracleDevartConnector stiOracleDevartConnector = new StiOracleDevartConnector(connectionString);
		isDevart = stiOracleDevartConnector.IsAvailable;
		return stiOracleDevartConnector;
	}

	public static StiOracleManagedConnector LoadManagedConnector(string connectionString = null)
	{
		StiOracleManagedConnector stiOracleManagedConnector = new StiOracleManagedConnector(connectionString);
		isManaged = stiOracleManagedConnector.IsAvailable;
		return stiOracleManagedConnector;
	}

	public static StiOracleConnector LoadGenericConnector(string connectionString = null)
	{
		StiOracleConnector stiOracleConnector = new StiOracleConnector(connectionString);
		isGeneric = stiOracleConnector.IsAvailable;
		return stiOracleConnector;
	}

	public StiOracleConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Oracle.DataAccess.dll";
		base.TypeConnection = "Oracle.DataAccess.Client.OracleConnection";
		base.TypeDataAdapter = "Oracle.DataAccess.Client.OracleDataAdapter";
		base.TypeCommand = "Oracle.DataAccess.Client.OracleCommand";
		base.TypeParameter = "Oracle.DataAccess.Client.OracleParameter";
		base.TypeDbType = "Oracle.DataAccess.Client.OracleDbType";
		base.TypeCommandBuilder = "Oracle.DataAccess.Client.OracleCommandBuilder";
		base.TypeConnectionStringBuilder = "Oracle.DataAccess.Client.OracleConnectionStringBuilder";
		base.TypeDataSourceEnumerator = "Oracle.DataAccess.Client.OracleDataSourceEnumerator";
	}
}
