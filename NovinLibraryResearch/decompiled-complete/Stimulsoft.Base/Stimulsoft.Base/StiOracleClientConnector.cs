using System;

namespace Stimulsoft.Base;

public class StiOracleClientConnector : StiOracleConnector
{
	public override Type SqlType => typeof(StiDbType.OracleClient);

	public override int DefaultSqlType => 14;

	public override string[] NuGetPackages => null;

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 6;
		}
		if (type == typeof(TimeSpan))
		{
			return 18;
		}
		if (type == typeof(long))
		{
			return 13;
		}
		if (type == typeof(int))
		{
			return 28;
		}
		if (type == typeof(short))
		{
			return 27;
		}
		if (type == typeof(sbyte))
		{
			return 26;
		}
		if (type == typeof(ulong))
		{
			return 13;
		}
		if (type == typeof(uint))
		{
			return 28;
		}
		if (type == typeof(ushort))
		{
			return 27;
		}
		if (type == typeof(byte))
		{
			return 23;
		}
		if (type == typeof(float))
		{
			return 29;
		}
		if (type == typeof(double))
		{
			return 30;
		}
		if (type == typeof(decimal))
		{
			return 13;
		}
		if (type == typeof(string))
		{
			return 22;
		}
		if (type == typeof(bool))
		{
			return 23;
		}
		if (type == typeof(char))
		{
			return 3;
		}
		if (type == typeof(byte[]))
		{
			return 2;
		}
		if (type == typeof(Guid))
		{
			return 15;
		}
		return 28;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.OracleClient)dbType)
		{
		case StiDbType.OracleClient.DateTime:
			return typeof(byte[]);
		case StiDbType.OracleClient.BFile:
		case StiDbType.OracleClient.Blob:
		case StiDbType.OracleClient.LongRaw:
		case StiDbType.OracleClient.Raw:
			return typeof(byte[]);
		case StiDbType.OracleClient.Float:
			return typeof(float);
		case StiDbType.OracleClient.Double:
			return typeof(double);
		case StiDbType.OracleClient.Number:
			return typeof(decimal);
		case StiDbType.OracleClient.SByte:
			return typeof(sbyte);
		case StiDbType.OracleClient.Int16:
			return typeof(short);
		case StiDbType.OracleClient.Int32:
			return typeof(int);
		case StiDbType.OracleClient.Byte:
			return typeof(byte);
		case StiDbType.OracleClient.UInt16:
			return typeof(ushort);
		case StiDbType.OracleClient.UInt32:
			return typeof(uint);
		default:
			return typeof(string);
		}
	}

	public StiOracleClientConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "System.Data.OracleClient.dll";
		base.TypeConnection = "System.Data.OracleClient.OracleConnection";
		base.TypeDataAdapter = "System.Data.OracleClient.OracleDataAdapter";
		base.TypeCommand = "System.Data.OracleClient.OracleCommand";
		base.TypeParameter = "System.Data.OracleClient.OracleParameter";
		base.TypeDbType = "System.Data.OracleClient.OracleDbType";
		base.TypeCommandBuilder = "System.Data.OracleClient.OracleCommandBuilder";
		base.TypeConnectionStringBuilder = "Oracle.DataAccess.Client.OracleConnectionStringBuilder";
	}
}
