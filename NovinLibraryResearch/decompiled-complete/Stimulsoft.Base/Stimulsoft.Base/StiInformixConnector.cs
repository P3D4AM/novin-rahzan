using System;
using System.Data.Common;

namespace Stimulsoft.Base;

public class StiInformixConnector : StiSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.InformixDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.InformixDataSource;

	public override string Name => "Informix";

	public override Type SqlType => typeof(StiDbType.Informix);

	public override int DefaultSqlType => 12;

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
			return 7;
		}
		if (type == typeof(long))
		{
			return 203;
		}
		if (type == typeof(int))
		{
			return 2;
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
			return 203;
		}
		if (type == typeof(uint))
		{
			return 2;
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
			return 3;
		}
		if (type == typeof(double))
		{
			return 205;
		}
		if (type == typeof(decimal))
		{
			return 5;
		}
		if (type == typeof(string))
		{
			return 12;
		}
		if (type == typeof(bool))
		{
			return 1;
		}
		if (type == typeof(char))
		{
			return 0;
		}
		if (type == typeof(byte[]))
		{
			return 215;
		}
		_ = type == typeof(Guid);
		return 12;
	}

	public override Type GetNetType(string dbType)
	{
		return dbType.ToLowerInvariant() switch
		{
			"BigInt" => typeof(long), 
			"Integer" => typeof(int), 
			"SmallInt" => typeof(short), 
			"Decimal" => typeof(decimal), 
			"Float" => typeof(float), 
			"Double" => typeof(double), 
			"Date" => typeof(DateTime), 
			_ => typeof(string), 
		};
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Informix)dbType)
		{
		case StiDbType.Informix.BigInt:
		case StiDbType.Informix.Numeric:
			return typeof(long);
		case StiDbType.Informix.Integer:
			return typeof(int);
		case StiDbType.Informix.SmallInt:
			return typeof(short);
		case StiDbType.Informix.Decimal:
			return typeof(decimal);
		case StiDbType.Informix.Float:
			return typeof(float);
		case StiDbType.Informix.Double:
			return typeof(double);
		case StiDbType.Informix.Date:
		case StiDbType.Informix.Time:
			return typeof(DateTime);
		case StiDbType.Informix.Boolean:
			return typeof(bool);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "Database=myDataBase;Host=192.168.10.10;Server=db_engine_tcp;Service=1492;" + Environment.NewLine + "Protocol=onsoctcp;UID=myUsername;Password=myPassword;";
	}

	public static StiInformixConnector Get(string connectionString = null)
	{
		return new StiInformixConnector(connectionString);
	}

	protected internal StiInformixConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "IBM.Data.Informix.dll";
		base.TypeConnection = "IBM.Data.Informix.IfxConnection";
		base.TypeDataAdapter = "IBM.Data.Informix.IfxDataAdapter";
		base.TypeCommand = "IBM.Data.Informix.IfxCommand";
		base.TypeParameter = "IBM.Data.Informix.IfxParameter";
		base.TypeDbType = "IBM.Data.Informix.IfxType";
		base.TypeCommandBuilder = "IBM.Data.Informix.IfxCommandBuilder";
	}
}
