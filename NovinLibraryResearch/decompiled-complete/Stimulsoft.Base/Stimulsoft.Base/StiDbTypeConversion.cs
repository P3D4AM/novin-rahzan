using System;
using System.Data;

namespace Stimulsoft.Base;

public class StiDbTypeConversion
{
	public static Type GetNetType(DbType type)
	{
		switch (type)
		{
		case DbType.SByte:
			return typeof(sbyte);
		case DbType.Int16:
			return typeof(short);
		case DbType.Int32:
			return typeof(int);
		case DbType.Int64:
			return typeof(long);
		case DbType.Byte:
			return typeof(byte);
		case DbType.UInt16:
			return typeof(ushort);
		case DbType.UInt32:
			return typeof(uint);
		case DbType.UInt64:
			return typeof(ulong);
		case DbType.Single:
			return typeof(float);
		case DbType.Double:
			return typeof(double);
		case DbType.Currency:
		case DbType.Decimal:
			return typeof(decimal);
		case DbType.Guid:
			return typeof(Guid);
		case DbType.Date:
		case DbType.DateTime:
		case DbType.Time:
		case DbType.DateTime2:
			return typeof(DateTime);
		case DbType.DateTimeOffset:
			return typeof(DateTimeOffset);
		case DbType.Boolean:
			return typeof(bool);
		case DbType.Binary:
			return typeof(byte[]);
		case DbType.String:
		case DbType.StringFixedLength:
			return typeof(string);
		default:
			return typeof(string);
		}
	}
}
