using System;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiDb2Connector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.Db2DataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.Db2DataSource;

	public override string Name => "DB2";

	public override Type SqlType => typeof(StiDbType.Db2);

	public override int DefaultSqlType => 13;

	public override string[] NuGetPackages => new string[1] { "IBM.Data.DB2" };

	public override string NuGetVersion => "10.0.5.5";

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
		return null;
	}

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 9;
		}
		if (type == typeof(TimeSpan))
		{
			return 11;
		}
		if (type == typeof(long))
		{
			return 3;
		}
		if (type == typeof(int))
		{
			return 2;
		}
		if (type == typeof(short))
		{
			return 1;
		}
		if (type == typeof(sbyte))
		{
			return 40;
		}
		if (type == typeof(ulong))
		{
			return 3;
		}
		if (type == typeof(uint))
		{
			return 2;
		}
		if (type == typeof(ushort))
		{
			return 1;
		}
		if (type == typeof(byte))
		{
			return 40;
		}
		if (type == typeof(float))
		{
			return 5;
		}
		if (type == typeof(double))
		{
			return 5;
		}
		if (type == typeof(decimal))
		{
			return 7;
		}
		if (type == typeof(string))
		{
			return 13;
		}
		if (type == typeof(bool))
		{
			return 40;
		}
		if (type == typeof(char))
		{
			return 13;
		}
		if (type == typeof(byte[]))
		{
			return 22;
		}
		return 13;
	}

	public override Type GetNetType(string dbType)
	{
		throw new NotSupportedException();
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Db2)dbType)
		{
		case StiDbType.Db2.BigInt:
			return typeof(long);
		case StiDbType.Db2.Integer:
			return typeof(int);
		case StiDbType.Db2.SmallInt:
			return typeof(short);
		case StiDbType.Db2.Int8:
		case StiDbType.Db2.Byte:
			return typeof(byte);
		case StiDbType.Db2.Decimal:
		case StiDbType.Db2.Numeric:
		case StiDbType.Db2.Money:
			return typeof(decimal);
		case StiDbType.Db2.Double:
			return typeof(double);
		case StiDbType.Db2.Float:
		case StiDbType.Db2.SmallFloat:
			return typeof(float);
		case StiDbType.Db2.Date:
		case StiDbType.Db2.Timestamp:
		case StiDbType.Db2.TimeStampWithTimeZone:
		case StiDbType.Db2.DateTime:
			return typeof(DateTime);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Server=myAddress:myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword;" + Environment.NewLine + "Max Pool Size=100;Min Pool Size=10;";
	}

	public static StiDb2Connector Get(string connectionString = null)
	{
		return new StiDb2Connector(connectionString);
	}

	public StiDb2Connector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "IBM.Data.DB2.dll";
		base.TypeConnection = "IBM.Data.DB2.DB2Connection";
		base.TypeDataAdapter = "IBM.Data.DB2.DB2DataAdapter";
		base.TypeCommand = "IBM.Data.DB2.DB2Command";
		base.TypeParameter = "IBM.Data.DB2.DB2Parameter";
		base.TypeDbType = "IBM.Data.DB2.DB2Type";
		base.TypeCommandBuilder = "IBM.Data.DB2.DB2CommandBuilder";
	}
}
