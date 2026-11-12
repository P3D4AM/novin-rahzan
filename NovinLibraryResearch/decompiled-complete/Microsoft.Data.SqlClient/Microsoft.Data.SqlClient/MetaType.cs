using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient.Server;
using Microsoft.SqlServer.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class MetaType
{
	private static class MetaTypeName
	{
		public const string BIGINT = "bigint";

		public const string BINARY = "binary";

		public const string BIT = "bit";

		public const string CHAR = "char";

		public const string DATETIME = "datetime";

		public const string DECIMAL = "decimal";

		public const string FLOAT = "float";

		public const string IMAGE = "image";

		public const string INT = "int";

		public const string MONEY = "money";

		public const string NCHAR = "nchar";

		public const string NTEXT = "ntext";

		public const string NVARCHAR = "nvarchar";

		public const string REAL = "real";

		public const string ROWGUID = "uniqueidentifier";

		public const string SMALLDATETIME = "smalldatetime";

		public const string SMALLINT = "smallint";

		public const string SMALLMONEY = "smallmoney";

		public const string TEXT = "text";

		public const string TIMESTAMP = "timestamp";

		public const string TINYINT = "tinyint";

		public const string UDT = "udt";

		public const string VARBINARY = "varbinary";

		public const string VARCHAR = "varchar";

		public const string VARIANT = "sql_variant";

		public const string XML = "xml";

		public const string TABLE = "table";

		public const string DATE = "date";

		public const string TIME = "time";

		public const string DATETIME2 = "datetime2";

		public const string DATETIMEOFFSET = "datetimeoffset";
	}

	internal readonly Type ClassType;

	internal readonly Type SqlType;

	internal readonly int FixedLength;

	internal readonly bool IsFixed;

	internal readonly bool IsLong;

	internal readonly bool IsPlp;

	internal readonly byte Precision;

	internal readonly byte Scale;

	internal readonly byte TDSType;

	internal readonly byte NullableType;

	internal readonly string TypeName;

	internal readonly SqlDbType SqlDbType;

	internal readonly DbType DbType;

	internal readonly byte PropBytes;

	internal readonly bool IsAnsiType;

	internal readonly bool IsBinType;

	internal readonly bool IsCharType;

	internal readonly bool IsNCharType;

	internal readonly bool IsSizeInCharacters;

	internal readonly bool IsNewKatmaiType;

	internal readonly bool IsVarTime;

	internal readonly bool Is70Supported;

	internal readonly bool Is80Supported;

	internal readonly bool Is90Supported;

	internal readonly bool Is100Supported;

	private static readonly MetaType MetaBigInt = new MetaType(19, byte.MaxValue, 8, isFixed: true, isLong: false, isPlp: false, 127, 38, "bigint", typeof(long), typeof(SqlInt64), SqlDbType.BigInt, DbType.Int64, 0);

	private static readonly MetaType MetaFloat = new MetaType(15, byte.MaxValue, 8, isFixed: true, isLong: false, isPlp: false, 62, 109, "float", typeof(double), typeof(SqlDouble), SqlDbType.Float, DbType.Double, 0);

	private static readonly MetaType MetaReal = new MetaType(7, byte.MaxValue, 4, isFixed: true, isLong: false, isPlp: false, 59, 109, "real", typeof(float), typeof(SqlSingle), SqlDbType.Real, DbType.Single, 0);

	private static readonly MetaType MetaBinary = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 173, 173, "binary", typeof(byte[]), typeof(SqlBinary), SqlDbType.Binary, DbType.Binary, 2);

	private static readonly MetaType MetaTimestamp = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 173, 173, "timestamp", typeof(byte[]), typeof(SqlBinary), SqlDbType.Timestamp, DbType.Binary, 2);

	internal static readonly MetaType MetaVarBinary = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 165, 165, "varbinary", typeof(byte[]), typeof(SqlBinary), SqlDbType.VarBinary, DbType.Binary, 2);

	internal static readonly MetaType MetaMaxVarBinary = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: true, 165, 165, "varbinary", typeof(byte[]), typeof(SqlBinary), SqlDbType.VarBinary, DbType.Binary, 2);

	private static readonly MetaType MetaSmallVarBinary = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 37, 173, ADP.StrEmpty, typeof(byte[]), typeof(SqlBinary), (SqlDbType)24, DbType.Binary, 2);

	internal static readonly MetaType MetaImage = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: false, 34, 34, "image", typeof(byte[]), typeof(SqlBinary), SqlDbType.Image, DbType.Binary, 0);

	private static readonly MetaType MetaBit = new MetaType(byte.MaxValue, byte.MaxValue, 1, isFixed: true, isLong: false, isPlp: false, 50, 104, "bit", typeof(bool), typeof(SqlBoolean), SqlDbType.Bit, DbType.Boolean, 0);

	private static readonly MetaType MetaTinyInt = new MetaType(3, byte.MaxValue, 1, isFixed: true, isLong: false, isPlp: false, 48, 38, "tinyint", typeof(byte), typeof(SqlByte), SqlDbType.TinyInt, DbType.Byte, 0);

	private static readonly MetaType MetaSmallInt = new MetaType(5, byte.MaxValue, 2, isFixed: true, isLong: false, isPlp: false, 52, 38, "smallint", typeof(short), typeof(SqlInt16), SqlDbType.SmallInt, DbType.Int16, 0);

	private static readonly MetaType MetaInt = new MetaType(10, byte.MaxValue, 4, isFixed: true, isLong: false, isPlp: false, 56, 38, "int", typeof(int), typeof(SqlInt32), SqlDbType.Int, DbType.Int32, 0);

	private static readonly MetaType MetaChar = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 175, 175, "char", typeof(string), typeof(SqlString), SqlDbType.Char, DbType.AnsiStringFixedLength, 7);

	private static readonly MetaType MetaVarChar = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 167, 167, "varchar", typeof(string), typeof(SqlString), SqlDbType.VarChar, DbType.AnsiString, 7);

	internal static readonly MetaType MetaMaxVarChar = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: true, 167, 167, "varchar", typeof(string), typeof(SqlString), SqlDbType.VarChar, DbType.AnsiString, 7);

	internal static readonly MetaType MetaText = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: false, 35, 35, "text", typeof(string), typeof(SqlString), SqlDbType.Text, DbType.AnsiString, 0);

	private static readonly MetaType MetaNChar = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 239, 239, "nchar", typeof(string), typeof(SqlString), SqlDbType.NChar, DbType.StringFixedLength, 7);

	internal static readonly MetaType MetaNVarChar = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 231, 231, "nvarchar", typeof(string), typeof(SqlString), SqlDbType.NVarChar, DbType.String, 7);

	internal static readonly MetaType MetaMaxNVarChar = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: true, 231, 231, "nvarchar", typeof(string), typeof(SqlString), SqlDbType.NVarChar, DbType.String, 7);

	internal static readonly MetaType MetaNText = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: false, 99, 99, "ntext", typeof(string), typeof(SqlString), SqlDbType.NText, DbType.String, 7);

	internal static readonly MetaType MetaDecimal = new MetaType(38, 4, 17, isFixed: true, isLong: false, isPlp: false, 108, 108, "decimal", typeof(decimal), typeof(SqlDecimal), SqlDbType.Decimal, DbType.Decimal, 2);

	internal static readonly MetaType MetaXml = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: true, 241, 241, "xml", typeof(string), typeof(SqlXml), SqlDbType.Xml, DbType.Xml, 0);

	private static readonly MetaType MetaDateTime = new MetaType(23, 3, 8, isFixed: true, isLong: false, isPlp: false, 61, 111, "datetime", typeof(DateTime), typeof(SqlDateTime), SqlDbType.DateTime, DbType.DateTime, 0);

	private static readonly MetaType MetaSmallDateTime = new MetaType(16, 0, 4, isFixed: true, isLong: false, isPlp: false, 58, 111, "smalldatetime", typeof(DateTime), typeof(SqlDateTime), SqlDbType.SmallDateTime, DbType.DateTime, 0);

	private static readonly MetaType MetaMoney = new MetaType(19, byte.MaxValue, 8, isFixed: true, isLong: false, isPlp: false, 60, 110, "money", typeof(decimal), typeof(SqlMoney), SqlDbType.Money, DbType.Currency, 0);

	private static readonly MetaType MetaSmallMoney = new MetaType(10, byte.MaxValue, 4, isFixed: true, isLong: false, isPlp: false, 122, 110, "smallmoney", typeof(decimal), typeof(SqlMoney), SqlDbType.SmallMoney, DbType.Currency, 0);

	private static readonly MetaType MetaUniqueId = new MetaType(byte.MaxValue, byte.MaxValue, 16, isFixed: true, isLong: false, isPlp: false, 36, 36, "uniqueidentifier", typeof(Guid), typeof(SqlGuid), SqlDbType.UniqueIdentifier, DbType.Guid, 0);

	private static readonly MetaType MetaVariant = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: true, isLong: false, isPlp: false, 98, 98, "sql_variant", typeof(object), typeof(object), SqlDbType.Variant, DbType.Object, 0);

	internal static readonly MetaType MetaUdt = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: true, 240, 240, "udt", typeof(object), typeof(object), SqlDbType.Udt, DbType.Object, 0);

	private static readonly MetaType MetaMaxUdt = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: true, isPlp: true, 240, 240, "udt", typeof(object), typeof(object), SqlDbType.Udt, DbType.Object, 0);

	private static readonly MetaType MetaTable = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 243, 243, "table", typeof(IEnumerable<DbDataRecord>), typeof(IEnumerable<DbDataRecord>), SqlDbType.Structured, DbType.Object, 0);

	private static readonly MetaType MetaSUDT = new MetaType(byte.MaxValue, byte.MaxValue, -1, isFixed: false, isLong: false, isPlp: false, 31, 31, "", typeof(SqlDataRecord), typeof(SqlDataRecord), SqlDbType.Structured, DbType.Object, 0);

	private static readonly MetaType MetaDate = new MetaType(byte.MaxValue, byte.MaxValue, 3, isFixed: true, isLong: false, isPlp: false, 40, 40, "date", typeof(DateTime), typeof(DateTime), SqlDbType.Date, DbType.Date, 0);

	internal static readonly MetaType MetaTime = new MetaType(byte.MaxValue, 7, -1, isFixed: false, isLong: false, isPlp: false, 41, 41, "time", typeof(TimeSpan), typeof(TimeSpan), SqlDbType.Time, DbType.Time, 1);

	private static readonly MetaType MetaDateTime2 = new MetaType(byte.MaxValue, 7, -1, isFixed: false, isLong: false, isPlp: false, 42, 42, "datetime2", typeof(DateTime), typeof(DateTime), SqlDbType.DateTime2, DbType.DateTime2, 1);

	internal static readonly MetaType MetaDateTimeOffset = new MetaType(byte.MaxValue, 7, -1, isFixed: false, isLong: false, isPlp: false, 43, 43, "datetimeoffset", typeof(DateTimeOffset), typeof(DateTimeOffset), SqlDbType.DateTimeOffset, DbType.DateTimeOffset, 1);

	public int TypeId => 0;

	public MetaType(byte precision, byte scale, int fixedLength, bool isFixed, bool isLong, bool isPlp, byte tdsType, byte nullableTdsType, string typeName, Type classType, Type sqlType, SqlDbType sqldbType, DbType dbType, byte propBytes)
	{
		Precision = precision;
		Scale = scale;
		FixedLength = fixedLength;
		IsFixed = isFixed;
		IsLong = isLong;
		IsPlp = isPlp;
		TDSType = tdsType;
		NullableType = nullableTdsType;
		TypeName = typeName;
		SqlDbType = sqldbType;
		DbType = dbType;
		ClassType = classType;
		SqlType = sqlType;
		PropBytes = propBytes;
		IsAnsiType = _IsAnsiType(sqldbType);
		IsBinType = _IsBinType(sqldbType);
		IsCharType = _IsCharType(sqldbType);
		IsNCharType = _IsNCharType(sqldbType);
		IsSizeInCharacters = _IsSizeInCharacters(sqldbType);
		IsNewKatmaiType = _IsNewKatmaiType(sqldbType);
		IsVarTime = _IsVarTime(sqldbType);
		Is70Supported = _Is70Supported(SqlDbType);
		Is80Supported = _Is80Supported(SqlDbType);
		Is90Supported = _Is90Supported(SqlDbType);
		Is100Supported = _Is100Supported(SqlDbType);
	}

	private static bool _IsAnsiType(SqlDbType type)
	{
		if (type != SqlDbType.Char && type != SqlDbType.VarChar)
		{
			return type == SqlDbType.Text;
		}
		return true;
	}

	private static bool _IsSizeInCharacters(SqlDbType type)
	{
		if (type != SqlDbType.NChar && type != SqlDbType.NVarChar && type != SqlDbType.Xml)
		{
			return type == SqlDbType.NText;
		}
		return true;
	}

	private static bool _IsCharType(SqlDbType type)
	{
		if (type != SqlDbType.NChar && type != SqlDbType.NVarChar && type != SqlDbType.NText && type != SqlDbType.Char && type != SqlDbType.VarChar && type != SqlDbType.Text)
		{
			return type == SqlDbType.Xml;
		}
		return true;
	}

	private static bool _IsNCharType(SqlDbType type)
	{
		if (type != SqlDbType.NChar && type != SqlDbType.NVarChar && type != SqlDbType.NText)
		{
			return type == SqlDbType.Xml;
		}
		return true;
	}

	private static bool _IsBinType(SqlDbType type)
	{
		if (type != SqlDbType.Image && type != SqlDbType.Binary && type != SqlDbType.VarBinary && type != SqlDbType.Timestamp && type != SqlDbType.Udt)
		{
			return type == (SqlDbType)24;
		}
		return true;
	}

	private static bool _Is70Supported(SqlDbType type)
	{
		if (type != 0 && type > SqlDbType.BigInt)
		{
			return type <= SqlDbType.VarChar;
		}
		return false;
	}

	private static bool _Is80Supported(SqlDbType type)
	{
		if (type >= SqlDbType.BigInt)
		{
			return type <= SqlDbType.Variant;
		}
		return false;
	}

	private static bool _Is90Supported(SqlDbType type)
	{
		if (!_Is80Supported(type) && SqlDbType.Xml != type)
		{
			return SqlDbType.Udt == type;
		}
		return true;
	}

	private static bool _Is100Supported(SqlDbType type)
	{
		if (!_Is90Supported(type) && SqlDbType.Date != type && SqlDbType.Time != type && SqlDbType.DateTime2 != type)
		{
			return SqlDbType.DateTimeOffset == type;
		}
		return true;
	}

	private static bool _IsNewKatmaiType(SqlDbType type)
	{
		return SqlDbType.Structured == type;
	}

	internal static bool _IsVarTime(SqlDbType type)
	{
		if (type != SqlDbType.Time && type != SqlDbType.DateTime2)
		{
			return type == SqlDbType.DateTimeOffset;
		}
		return true;
	}

	internal static MetaType GetMetaTypeFromSqlDbType(SqlDbType target, bool isMultiValued)
	{
		switch (target)
		{
		case SqlDbType.BigInt:
			return MetaBigInt;
		case SqlDbType.Binary:
			return MetaBinary;
		case SqlDbType.Bit:
			return MetaBit;
		case SqlDbType.Char:
			return MetaChar;
		case SqlDbType.DateTime:
			return MetaDateTime;
		case SqlDbType.Decimal:
			return MetaDecimal;
		case SqlDbType.Float:
			return MetaFloat;
		case SqlDbType.Image:
			return MetaImage;
		case SqlDbType.Int:
			return MetaInt;
		case SqlDbType.Money:
			return MetaMoney;
		case SqlDbType.NChar:
			return MetaNChar;
		case SqlDbType.NText:
			return MetaNText;
		case SqlDbType.NVarChar:
			return MetaNVarChar;
		case SqlDbType.Real:
			return MetaReal;
		case SqlDbType.UniqueIdentifier:
			return MetaUniqueId;
		case SqlDbType.SmallDateTime:
			return MetaSmallDateTime;
		case SqlDbType.SmallInt:
			return MetaSmallInt;
		case SqlDbType.SmallMoney:
			return MetaSmallMoney;
		case SqlDbType.Text:
			return MetaText;
		case SqlDbType.Timestamp:
			return MetaTimestamp;
		case SqlDbType.TinyInt:
			return MetaTinyInt;
		case SqlDbType.VarBinary:
			return MetaVarBinary;
		case SqlDbType.VarChar:
			return MetaVarChar;
		case SqlDbType.Variant:
			return MetaVariant;
		case (SqlDbType)24:
			return MetaSmallVarBinary;
		case SqlDbType.Xml:
			return MetaXml;
		case SqlDbType.Udt:
			return MetaUdt;
		case SqlDbType.Structured:
			if (isMultiValued)
			{
				return MetaTable;
			}
			return MetaSUDT;
		case SqlDbType.Date:
			return MetaDate;
		case SqlDbType.Time:
			return MetaTime;
		case SqlDbType.DateTime2:
			return MetaDateTime2;
		case SqlDbType.DateTimeOffset:
			return MetaDateTimeOffset;
		default:
			throw SQL.InvalidSqlDbType(target);
		}
	}

	internal static MetaType GetMetaTypeFromDbType(DbType target)
	{
		return target switch
		{
			DbType.AnsiString => MetaVarChar, 
			DbType.AnsiStringFixedLength => MetaChar, 
			DbType.Binary => MetaVarBinary, 
			DbType.Byte => MetaTinyInt, 
			DbType.Boolean => MetaBit, 
			DbType.Currency => MetaMoney, 
			DbType.Date => MetaDate, 
			DbType.DateTime => MetaDateTime, 
			DbType.Decimal => MetaDecimal, 
			DbType.Double => MetaFloat, 
			DbType.Guid => MetaUniqueId, 
			DbType.Int16 => MetaSmallInt, 
			DbType.Int32 => MetaInt, 
			DbType.Int64 => MetaBigInt, 
			DbType.Object => MetaVariant, 
			DbType.Single => MetaReal, 
			DbType.String => MetaNVarChar, 
			DbType.StringFixedLength => MetaNChar, 
			DbType.Time => MetaTime, 
			DbType.Xml => MetaXml, 
			DbType.DateTime2 => MetaDateTime2, 
			DbType.DateTimeOffset => MetaDateTimeOffset, 
			_ => throw ADP.DbTypeNotSupported(target, typeof(SqlDbType)), 
		};
	}

	internal static MetaType GetMaxMetaTypeFromMetaType(MetaType mt)
	{
		switch (mt.SqlDbType)
		{
		case SqlDbType.Binary:
		case SqlDbType.VarBinary:
			return MetaMaxVarBinary;
		case SqlDbType.Char:
		case SqlDbType.VarChar:
			return MetaMaxVarChar;
		case SqlDbType.NChar:
		case SqlDbType.NVarChar:
			return MetaMaxNVarChar;
		case SqlDbType.Udt:
			return MetaMaxUdt;
		default:
			return mt;
		}
	}

	internal static MetaType GetMetaTypeFromType(Type dataType)
	{
		return GetMetaTypeFromValue(dataType, null, inferLen: false, streamAllowed: true);
	}

	internal static MetaType GetMetaTypeFromValue(object value, bool streamAllowed = true)
	{
		return GetMetaTypeFromValue(value.GetType(), value, inferLen: true, streamAllowed);
	}

	private static MetaType GetMetaTypeFromValue(Type dataType, object value, bool inferLen, bool streamAllowed)
	{
		switch (Type.GetTypeCode(dataType))
		{
		case TypeCode.Empty:
			throw ADP.InvalidDataType(TypeCode.Empty);
		case TypeCode.Object:
		{
			if (dataType == typeof(byte[]))
			{
				if (!inferLen || ((byte[])value).Length <= 8000)
				{
					return MetaVarBinary;
				}
				return MetaImage;
			}
			if (dataType == typeof(Guid))
			{
				return MetaUniqueId;
			}
			if (dataType == typeof(object))
			{
				return MetaVariant;
			}
			if (dataType == typeof(SqlBinary))
			{
				return MetaVarBinary;
			}
			if (dataType == typeof(SqlBoolean))
			{
				return MetaBit;
			}
			if (dataType == typeof(SqlByte))
			{
				return MetaTinyInt;
			}
			if (dataType == typeof(SqlBytes))
			{
				return MetaVarBinary;
			}
			if (dataType == typeof(SqlChars))
			{
				return MetaNVarChar;
			}
			if (dataType == typeof(SqlDateTime))
			{
				return MetaDateTime;
			}
			if (dataType == typeof(SqlDouble))
			{
				return MetaFloat;
			}
			if (dataType == typeof(SqlGuid))
			{
				return MetaUniqueId;
			}
			if (dataType == typeof(SqlInt16))
			{
				return MetaSmallInt;
			}
			if (dataType == typeof(SqlInt32))
			{
				return MetaInt;
			}
			if (dataType == typeof(SqlInt64))
			{
				return MetaBigInt;
			}
			if (dataType == typeof(SqlMoney))
			{
				return MetaMoney;
			}
			if (dataType == typeof(SqlDecimal))
			{
				return MetaDecimal;
			}
			if (dataType == typeof(SqlSingle))
			{
				return MetaReal;
			}
			if (dataType == typeof(SqlXml))
			{
				return MetaXml;
			}
			if (dataType == typeof(SqlString))
			{
				if (!inferLen || ((SqlString)value).IsNull)
				{
					return MetaNVarChar;
				}
				return PromoteStringType(((SqlString)value).Value);
			}
			if (dataType == typeof(IEnumerable<DbDataRecord>) || dataType == typeof(DataTable))
			{
				return MetaTable;
			}
			if (dataType == typeof(TimeSpan))
			{
				return MetaTime;
			}
			if (dataType == typeof(DateTimeOffset))
			{
				return MetaDateTimeOffset;
			}
			SqlUdtInfo sqlUdtInfo = SqlUdtInfo.TryGetFromType(dataType);
			if (sqlUdtInfo != null)
			{
				return MetaUdt;
			}
			if (streamAllowed)
			{
				if (typeof(Stream).IsAssignableFrom(dataType))
				{
					return MetaVarBinary;
				}
				if (typeof(TextReader).IsAssignableFrom(dataType))
				{
					return MetaNVarChar;
				}
				if (typeof(XmlReader).IsAssignableFrom(dataType))
				{
					return MetaXml;
				}
			}
			throw ADP.UnknownDataType(dataType);
		}
		case TypeCode.DBNull:
			throw ADP.InvalidDataType(TypeCode.DBNull);
		case TypeCode.Boolean:
			return MetaBit;
		case TypeCode.Char:
			throw ADP.InvalidDataType(TypeCode.Char);
		case TypeCode.SByte:
			throw ADP.InvalidDataType(TypeCode.SByte);
		case TypeCode.Byte:
			return MetaTinyInt;
		case TypeCode.Int16:
			return MetaSmallInt;
		case TypeCode.UInt16:
			throw ADP.InvalidDataType(TypeCode.UInt16);
		case TypeCode.Int32:
			return MetaInt;
		case TypeCode.UInt32:
			throw ADP.InvalidDataType(TypeCode.UInt32);
		case TypeCode.Int64:
			return MetaBigInt;
		case TypeCode.UInt64:
			throw ADP.InvalidDataType(TypeCode.UInt64);
		case TypeCode.Single:
			return MetaReal;
		case TypeCode.Double:
			return MetaFloat;
		case TypeCode.Decimal:
			return MetaDecimal;
		case TypeCode.DateTime:
			return MetaDateTime;
		case TypeCode.String:
			if (!inferLen)
			{
				return MetaNVarChar;
			}
			return PromoteStringType((string)value);
		default:
			throw ADP.UnknownDataTypeCode(dataType, Type.GetTypeCode(dataType));
		}
	}

	internal static object GetNullSqlValue(Type sqlType)
	{
		if (sqlType == typeof(SqlSingle))
		{
			return SqlSingle.Null;
		}
		if (sqlType == typeof(SqlString))
		{
			return SqlString.Null;
		}
		if (sqlType == typeof(SqlDouble))
		{
			return SqlDouble.Null;
		}
		if (sqlType == typeof(SqlBinary))
		{
			return SqlBinary.Null;
		}
		if (sqlType == typeof(SqlGuid))
		{
			return SqlGuid.Null;
		}
		if (sqlType == typeof(SqlBoolean))
		{
			return SqlBoolean.Null;
		}
		if (sqlType == typeof(SqlByte))
		{
			return SqlByte.Null;
		}
		if (sqlType == typeof(SqlInt16))
		{
			return SqlInt16.Null;
		}
		if (sqlType == typeof(SqlInt32))
		{
			return SqlInt32.Null;
		}
		if (sqlType == typeof(SqlInt64))
		{
			return SqlInt64.Null;
		}
		if (sqlType == typeof(SqlDecimal))
		{
			return SqlDecimal.Null;
		}
		if (sqlType == typeof(SqlDateTime))
		{
			return SqlDateTime.Null;
		}
		if (sqlType == typeof(SqlMoney))
		{
			return SqlMoney.Null;
		}
		if (sqlType == typeof(SqlXml))
		{
			return SqlXml.Null;
		}
		if (sqlType == typeof(object))
		{
			return DBNull.Value;
		}
		if (sqlType == typeof(IEnumerable<DbDataRecord>))
		{
			return DBNull.Value;
		}
		if (sqlType == typeof(DataTable))
		{
			return DBNull.Value;
		}
		if (sqlType == typeof(DateTime))
		{
			return DBNull.Value;
		}
		if (sqlType == typeof(TimeSpan))
		{
			return DBNull.Value;
		}
		_ = sqlType == typeof(DateTimeOffset);
		return DBNull.Value;
	}

	internal static MetaType PromoteStringType(string s)
	{
		int length = s.Length;
		if (length << 1 > 8000)
		{
			return MetaVarChar;
		}
		return MetaNVarChar;
	}

	internal static object GetComValueFromSqlVariant(object sqlVal)
	{
		object result = null;
		if (ADP.IsNull(sqlVal))
		{
			return result;
		}
		if (sqlVal is SqlSingle sqlSingle)
		{
			result = sqlSingle.Value;
		}
		else if (sqlVal is SqlString sqlString)
		{
			result = sqlString.Value;
		}
		else if (sqlVal is SqlDouble sqlDouble)
		{
			result = sqlDouble.Value;
		}
		else if (sqlVal is SqlBinary sqlBinary)
		{
			result = sqlBinary.Value;
		}
		else if (sqlVal is SqlGuid sqlGuid)
		{
			result = sqlGuid.Value;
		}
		else if (sqlVal is SqlBoolean sqlBoolean)
		{
			result = sqlBoolean.Value;
		}
		else if (sqlVal is SqlByte sqlByte)
		{
			result = sqlByte.Value;
		}
		else if (sqlVal is SqlInt16 sqlInt)
		{
			result = sqlInt.Value;
		}
		else if (sqlVal is SqlInt32 sqlInt2)
		{
			result = sqlInt2.Value;
		}
		else if (sqlVal is SqlInt64 sqlInt3)
		{
			result = sqlInt3.Value;
		}
		else if (sqlVal is SqlDecimal sqlDecimal)
		{
			result = sqlDecimal.Value;
		}
		else if (sqlVal is SqlDateTime sqlDateTime)
		{
			result = sqlDateTime.Value;
		}
		else if (sqlVal is SqlMoney sqlMoney)
		{
			result = sqlMoney.Value;
		}
		else if (sqlVal is SqlXml)
		{
			result = ((SqlXml)sqlVal).Value;
		}
		return result;
	}

	[Conditional("DEBUG")]
	private static void AssertIsUserDefinedTypeInstance(object sqlValue, string failedAssertMessage)
	{
		Type type = sqlValue.GetType();
		SqlUserDefinedTypeAttribute[] array = (SqlUserDefinedTypeAttribute[])type.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), inherit: true);
	}

	internal static object GetSqlValueFromComVariant(object comVal)
	{
		object result = null;
		if (comVal != null && DBNull.Value != comVal)
		{
			if (comVal is float)
			{
				result = new SqlSingle((float)comVal);
			}
			else if (comVal is string)
			{
				result = new SqlString((string)comVal);
			}
			else if (comVal is double)
			{
				result = new SqlDouble((double)comVal);
			}
			else if (comVal is byte[])
			{
				result = new SqlBinary((byte[])comVal);
			}
			else if (comVal is char c)
			{
				result = new SqlString(c.ToString());
			}
			else if (comVal is char[])
			{
				result = new SqlChars((char[])comVal);
			}
			else if (comVal is Guid)
			{
				result = new SqlGuid((Guid)comVal);
			}
			else if (comVal is bool)
			{
				result = new SqlBoolean((bool)comVal);
			}
			else if (comVal is byte)
			{
				result = new SqlByte((byte)comVal);
			}
			else if (comVal is short)
			{
				result = new SqlInt16((short)comVal);
			}
			else if (comVal is int)
			{
				result = new SqlInt32((int)comVal);
			}
			else if (comVal is long)
			{
				result = new SqlInt64((long)comVal);
			}
			else if (comVal is decimal)
			{
				result = new SqlDecimal((decimal)comVal);
			}
			else if (comVal is DateTime)
			{
				result = new SqlDateTime((DateTime)comVal);
			}
			else if (comVal is XmlReader)
			{
				result = new SqlXml((XmlReader)comVal);
			}
			else if (comVal is TimeSpan || comVal is DateTimeOffset)
			{
				result = comVal;
			}
		}
		return result;
	}

	internal static SqlDbType GetSqlDbTypeFromOleDbType(short dbType, string typeName)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected I4, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected I4, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected I4, but got Unknown
		SqlDbType result = SqlDbType.Variant;
		OleDbType val = (OleDbType)dbType;
		if ((int)val <= 64)
		{
			switch (val - 2)
			{
			default:
				if ((int)val == 64)
				{
					break;
				}
				goto IL_01d2;
			case 18:
				result = SqlDbType.BigInt;
				goto IL_01d2;
			case 9:
				result = SqlDbType.Bit;
				goto IL_01d2;
			case 4:
				result = ((typeName == "smallmoney") ? SqlDbType.SmallMoney : SqlDbType.Money);
				goto IL_01d2;
			case 5:
				break;
			case 12:
				goto IL_015d;
			case 3:
				result = SqlDbType.Float;
				goto IL_01d2;
			case 1:
				result = SqlDbType.Int;
				goto IL_01d2;
			case 2:
				result = SqlDbType.Real;
				goto IL_01d2;
			case 0:
			case 16:
				result = SqlDbType.SmallInt;
				goto IL_01d2;
			case 14:
			case 15:
				result = SqlDbType.TinyInt;
				goto IL_01d2;
			case 10:
				result = SqlDbType.Variant;
				goto IL_01d2;
			case 6:
				goto IL_01a5;
			case 7:
			case 8:
			case 11:
			case 13:
			case 17:
				goto IL_01d2;
			}
			goto IL_0133;
		}
		if ((int)val != 72)
		{
			switch (val - 128)
			{
			case 1:
				goto IL_0102;
			case 7:
				goto IL_0133;
			case 3:
				goto IL_015d;
			case 0:
				goto IL_018b;
			case 2:
				goto IL_01a5;
			case 5:
				result = SqlDbType.Date;
				goto IL_01d2;
			case 4:
				result = SqlDbType.Udt;
				goto IL_01d2;
			case 13:
				result = SqlDbType.Xml;
				goto IL_01d2;
			case 17:
				result = SqlDbType.Time;
				goto IL_01d2;
			case 18:
				result = SqlDbType.DateTimeOffset;
				goto IL_01d2;
			case 6:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 14:
			case 15:
			case 16:
				goto IL_01d2;
			}
			switch (val - 200)
			{
			case 0:
				break;
			case 5:
				result = SqlDbType.Image;
				goto IL_01d2;
			case 1:
				result = SqlDbType.Text;
				goto IL_01d2;
			case 3:
				result = SqlDbType.NText;
				goto IL_01d2;
			case 4:
				goto IL_018b;
			case 2:
				goto IL_01a5;
			default:
				goto IL_01d2;
			}
			goto IL_0102;
		}
		result = SqlDbType.UniqueIdentifier;
		goto IL_01d2;
		IL_01d2:
		return result;
		IL_0102:
		result = ((typeName == "char") ? SqlDbType.Char : SqlDbType.VarChar);
		goto IL_01d2;
		IL_015d:
		result = SqlDbType.Decimal;
		goto IL_01d2;
		IL_018b:
		result = ((typeName == "binary") ? SqlDbType.Binary : SqlDbType.VarBinary);
		goto IL_01d2;
		IL_01a5:
		result = ((typeName == "nchar") ? SqlDbType.NChar : SqlDbType.NVarChar);
		goto IL_01d2;
		IL_0133:
		result = ((typeName == "smalldatetime") ? SqlDbType.SmallDateTime : ((!(typeName == "datetime2")) ? SqlDbType.DateTime : SqlDbType.DateTime2));
		goto IL_01d2;
	}

	internal static MetaType GetSqlDataType(int tdsType, uint userType, int length)
	{
		switch (tdsType)
		{
		case 110:
			if (4 != length)
			{
				return MetaMoney;
			}
			return MetaSmallMoney;
		case 111:
			if (4 != length)
			{
				return MetaDateTime;
			}
			return MetaSmallDateTime;
		case 38:
			if (4 > length)
			{
				if (2 != length)
				{
					return MetaTinyInt;
				}
				return MetaSmallInt;
			}
			if (4 != length)
			{
				return MetaBigInt;
			}
			return MetaInt;
		case 109:
			if (4 != length)
			{
				return MetaFloat;
			}
			return MetaReal;
		case 35:
			return MetaText;
		case 37:
			return MetaSmallVarBinary;
		case 165:
			return MetaVarBinary;
		case 39:
		case 167:
			return MetaVarChar;
		case 45:
		case 173:
			if (80 != userType)
			{
				return MetaBinary;
			}
			return MetaTimestamp;
		case 34:
			return MetaImage;
		case 47:
		case 175:
			return MetaChar;
		case 48:
			return MetaTinyInt;
		case 50:
		case 104:
			return MetaBit;
		case 52:
			return MetaSmallInt;
		case 56:
			return MetaInt;
		case 127:
			return MetaBigInt;
		case 60:
			return MetaMoney;
		case 61:
			return MetaDateTime;
		case 62:
			return MetaFloat;
		case 59:
			return MetaReal;
		case 122:
			return MetaSmallMoney;
		case 58:
			return MetaSmallDateTime;
		case 106:
		case 108:
			return MetaDecimal;
		case 36:
			return MetaUniqueId;
		case 239:
			return MetaNChar;
		case 231:
			return MetaNVarChar;
		case 99:
			return MetaNText;
		case 98:
			return MetaVariant;
		case 240:
			return MetaUdt;
		case 241:
			return MetaXml;
		case 243:
			return MetaTable;
		case 40:
			return MetaDate;
		case 41:
			return MetaTime;
		case 42:
			return MetaDateTime2;
		case 43:
			return MetaDateTimeOffset;
		default:
			throw SQL.InvalidSqlDbType((SqlDbType)tdsType);
		}
	}

	internal static MetaType GetDefaultMetaType()
	{
		return MetaNVarChar;
	}

	internal static string GetStringFromXml(XmlReader xmlreader)
	{
		SqlXml sqlXml = new SqlXml(xmlreader);
		return sqlXml.Value;
	}

	public static TdsDateTime FromDateTime(DateTime dateTime, byte cb)
	{
		TdsDateTime result = default(TdsDateTime);
		SqlDateTime sqlDateTime;
		if (cb == 8)
		{
			sqlDateTime = new SqlDateTime(dateTime);
			result.time = sqlDateTime.TimeTicks;
		}
		else
		{
			sqlDateTime = new SqlDateTime(dateTime.AddSeconds(30.0));
			result.time = sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute;
		}
		result.days = sqlDateTime.DayTicks;
		return result;
	}

	public static DateTime ToDateTime(int sqlDays, int sqlTime, int length)
	{
		if (length == 4)
		{
			return new SqlDateTime(sqlDays, sqlTime * SqlDateTime.SQLTicksPerMinute).Value;
		}
		return new SqlDateTime(sqlDays, sqlTime).Value;
	}

	internal static int GetTimeSizeFromScale(byte scale)
	{
		if (scale <= 2)
		{
			return 3;
		}
		if (scale <= 4)
		{
			return 4;
		}
		return 5;
	}
}
