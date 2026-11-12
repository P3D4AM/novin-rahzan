using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerTypeMappingSource : RelationalTypeMappingSource
{
	private readonly RelationalTypeMapping _sqlVariant = new SqlServerSqlVariantTypeMapping("sql_variant");

	private readonly FloatTypeMapping _real = new SqlServerFloatTypeMapping("real");

	private readonly ByteTypeMapping _byte = new SqlServerByteTypeMapping("tinyint", DbType.Byte);

	private readonly ShortTypeMapping _short = new SqlServerShortTypeMapping("smallint", DbType.Int16);

	private readonly LongTypeMapping _long = new SqlServerLongTypeMapping("bigint", DbType.Int64);

	private readonly SqlServerByteArrayTypeMapping _rowversion = new SqlServerByteArrayTypeMapping("rowversion", 8, fixedLength: false, new ValueComparer<byte[]>((byte[] v1, byte[] v2) => StructuralComparisons.StructuralEqualityComparer.Equals(v1, v2), (byte[] v) => StructuralComparisons.StructuralEqualityComparer.GetHashCode(v), (byte[] v) => (v == null) ? null : v.ToArray()), null, StoreTypePostfix.None);

	private readonly IntTypeMapping _int = new IntTypeMapping("int", DbType.Int32);

	private readonly BoolTypeMapping _bool = new SqlServerBoolTypeMapping("bit");

	private readonly SqlServerStringTypeMapping _fixedLengthUnicodeString = new SqlServerStringTypeMapping(null, unicode: true, null, fixedLength: true);

	private readonly SqlServerStringTypeMapping _textUnicodeString = new SqlServerStringTypeMapping("ntext", unicode: true, null, fixedLength: false, SqlDbType.NText);

	private readonly SqlServerStringTypeMapping _variableLengthUnicodeString = new SqlServerStringTypeMapping(null, unicode: true);

	private readonly SqlServerStringTypeMapping _variableLengthMaxUnicodeString = new SqlServerStringTypeMapping("nvarchar(max)", unicode: true, null, fixedLength: false, null, StoreTypePostfix.None);

	private readonly SqlServerStringTypeMapping _fixedLengthAnsiString = new SqlServerStringTypeMapping(null, unicode: false, null, fixedLength: true);

	private readonly SqlServerStringTypeMapping _textAnsiString = new SqlServerStringTypeMapping("text", unicode: false, null, fixedLength: false, SqlDbType.Text);

	private readonly SqlServerStringTypeMapping _variableLengthAnsiString = new SqlServerStringTypeMapping();

	private readonly SqlServerStringTypeMapping _variableLengthMaxAnsiString = new SqlServerStringTypeMapping("varchar(max)", unicode: false, null, fixedLength: false, null, StoreTypePostfix.None);

	private readonly SqlServerByteArrayTypeMapping _variableLengthBinary = new SqlServerByteArrayTypeMapping();

	private readonly SqlServerByteArrayTypeMapping _imageBinary = new SqlServerByteArrayTypeMapping("image", null, fixedLength: false, null, SqlDbType.Image);

	private readonly SqlServerByteArrayTypeMapping _variableLengthMaxBinary = new SqlServerByteArrayTypeMapping("varbinary(max)", null, fixedLength: false, null, null, StoreTypePostfix.None);

	private readonly SqlServerByteArrayTypeMapping _fixedLengthBinary = new SqlServerByteArrayTypeMapping(null, null, fixedLength: true);

	private readonly SqlServerDateTimeTypeMapping _date = new SqlServerDateTimeTypeMapping("date", DbType.Date);

	private readonly SqlServerDateTimeTypeMapping _datetime = new SqlServerDateTimeTypeMapping("datetime", DbType.DateTime);

	private readonly SqlServerDateTimeTypeMapping _datetime2 = new SqlServerDateTimeTypeMapping("datetime2", DbType.DateTime2);

	private readonly DoubleTypeMapping _double = new SqlServerDoubleTypeMapping("float");

	private readonly SqlServerDateTimeOffsetTypeMapping _datetimeoffset = new SqlServerDateTimeOffsetTypeMapping("datetimeoffset", DbType.DateTimeOffset);

	private readonly GuidTypeMapping _uniqueidentifier = new GuidTypeMapping("uniqueidentifier", DbType.Guid);

	private readonly DecimalTypeMapping _decimal = new SqlServerDecimalTypeMapping("decimal(18, 2)", null, 18, 2, StoreTypePostfix.PrecisionAndScale);

	private readonly DecimalTypeMapping _money = new SqlServerDecimalTypeMapping("money");

	private readonly TimeSpanTypeMapping _time = new SqlServerTimeSpanTypeMapping("time");

	private readonly SqlServerStringTypeMapping _xml = new SqlServerStringTypeMapping("xml", unicode: true, null, fixedLength: false, null, StoreTypePostfix.None);

	private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;

	private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;

	private readonly HashSet<string> _disallowedMappings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"binary", "binary varying", "varbinary", "char", "character", "char varying", "character varying", "varchar", "national char", "national character",
		"nchar", "national char varying", "national character varying", "nvarchar"
	};

	private readonly IReadOnlyDictionary<string, Func<Type, RelationalTypeMapping>> _namedClrMappings = new Dictionary<string, Func<Type, RelationalTypeMapping>>(StringComparer.Ordinal)
	{
		{
			"Microsoft.SqlServer.Types.SqlHierarchyId",
			(Type t) => SqlServerUdtTypeMapping.CreateSqlHierarchyIdMapping(t)
		},
		{
			"Microsoft.SqlServer.Types.SqlGeography",
			(Type t) => SqlServerUdtTypeMapping.CreateSqlSpatialMapping(t, "geography")
		},
		{
			"Microsoft.SqlServer.Types.SqlGeometry",
			(Type t) => SqlServerUdtTypeMapping.CreateSqlSpatialMapping(t, "geometry")
		}
	};

	public SqlServerTypeMappingSource([JetBrains.Annotations.NotNull] TypeMappingSourceDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalTypeMappingSourceDependencies relationalDependencies)
		: base(dependencies, relationalDependencies)
	{
		_clrTypeMappings = new Dictionary<Type, RelationalTypeMapping>
		{
			{
				typeof(int),
				_int
			},
			{
				typeof(long),
				_long
			},
			{
				typeof(DateTime),
				_datetime2
			},
			{
				typeof(Guid),
				_uniqueidentifier
			},
			{
				typeof(bool),
				_bool
			},
			{
				typeof(byte),
				_byte
			},
			{
				typeof(double),
				_double
			},
			{
				typeof(DateTimeOffset),
				_datetimeoffset
			},
			{
				typeof(short),
				_short
			},
			{
				typeof(float),
				_real
			},
			{
				typeof(decimal),
				_decimal
			},
			{
				typeof(TimeSpan),
				_time
			}
		};
		_storeTypeMappings = new Dictionary<string, RelationalTypeMapping>(StringComparer.OrdinalIgnoreCase)
		{
			{ "bigint", _long },
			{ "binary varying", _variableLengthBinary },
			{ "binary", _fixedLengthBinary },
			{ "bit", _bool },
			{ "char varying", _variableLengthAnsiString },
			{ "char", _fixedLengthAnsiString },
			{ "character varying", _variableLengthAnsiString },
			{ "character", _fixedLengthAnsiString },
			{ "date", _date },
			{ "datetime", _datetime },
			{ "datetime2", _datetime2 },
			{ "datetimeoffset", _datetimeoffset },
			{ "dec", _decimal },
			{ "decimal", _decimal },
			{ "double precision", _double },
			{ "float", _double },
			{ "image", _imageBinary },
			{ "int", _int },
			{ "money", _money },
			{ "national char varying", _variableLengthUnicodeString },
			{ "national character varying", _variableLengthUnicodeString },
			{ "national character", _fixedLengthUnicodeString },
			{ "nchar", _fixedLengthUnicodeString },
			{ "ntext", _textUnicodeString },
			{ "numeric", _decimal },
			{ "nvarchar", _variableLengthUnicodeString },
			{ "nvarchar(max)", _variableLengthMaxUnicodeString },
			{ "real", _real },
			{ "rowversion", _rowversion },
			{ "smalldatetime", _datetime },
			{ "smallint", _short },
			{ "smallmoney", _money },
			{ "sql_variant", _sqlVariant },
			{ "text", _textAnsiString },
			{ "time", _time },
			{ "timestamp", _rowversion },
			{ "tinyint", _byte },
			{ "uniqueidentifier", _uniqueidentifier },
			{ "varbinary", _variableLengthBinary },
			{ "varbinary(max)", _variableLengthMaxBinary },
			{ "varchar", _variableLengthAnsiString },
			{ "varchar(max)", _variableLengthMaxAnsiString },
			{ "xml", _xml }
		};
	}

	protected override void ValidateMapping(CoreTypeMapping mapping, IProperty property)
	{
		RelationalTypeMapping relationalTypeMapping = mapping as RelationalTypeMapping;
		if (_disallowedMappings.Contains(relationalTypeMapping?.StoreType))
		{
			if (property == null)
			{
				throw new ArgumentException(SqlServerStrings.UnqualifiedDataType(relationalTypeMapping.StoreType));
			}
			throw new ArgumentException(SqlServerStrings.UnqualifiedDataTypeOnProperty(relationalTypeMapping.StoreType, property.Name));
		}
	}

	protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
	{
		return FindRawMapping(mappingInfo)?.Clone(in mappingInfo) ?? base.FindMapping(in mappingInfo);
	}

	private RelationalTypeMapping FindRawMapping(RelationalTypeMappingInfo mappingInfo)
	{
		Type clrType = mappingInfo.ClrType;
		string storeTypeName = mappingInfo.StoreTypeName;
		string storeTypeNameBase = mappingInfo.StoreTypeNameBase;
		if (storeTypeName != null)
		{
			if (clrType == typeof(float) && mappingInfo.Size.HasValue && mappingInfo.Size <= 24 && (storeTypeNameBase.Equals("float", StringComparison.OrdinalIgnoreCase) || storeTypeNameBase.Equals("double precision", StringComparison.OrdinalIgnoreCase)))
			{
				return _real;
			}
			if (_storeTypeMappings.TryGetValue(storeTypeName, out var value) || _storeTypeMappings.TryGetValue(storeTypeNameBase, out value))
			{
				if (!(clrType == null) && !(value.ClrType == clrType))
				{
					return null;
				}
				return value;
			}
		}
		if (clrType != null)
		{
			if (_clrTypeMappings.TryGetValue(clrType, out var value2))
			{
				return value2;
			}
			if (_namedClrMappings.TryGetValue(clrType.FullName, out var value3))
			{
				return value3(clrType);
			}
			if (clrType == typeof(string))
			{
				bool flag = mappingInfo.IsUnicode == false;
				bool flag2 = mappingInfo.IsFixedLength == true;
				int num = (flag ? 8000 : 4000);
				int? num2 = mappingInfo.Size ?? (mappingInfo.IsKeyOrIndex ? new int?(flag ? 900 : 450) : null);
				if (num2 > num)
				{
					num2 = (flag2 ? new int?(num) : null);
				}
				if (num2.HasValue)
				{
					return new SqlServerStringTypeMapping(null, !flag, num2, flag2);
				}
				if (!flag)
				{
					return _variableLengthMaxUnicodeString;
				}
				return _variableLengthMaxAnsiString;
			}
			if (clrType == typeof(byte[]))
			{
				if (mappingInfo.IsRowVersion == true)
				{
					return _rowversion;
				}
				bool flag3 = mappingInfo.IsFixedLength == true;
				int? num3 = mappingInfo.Size ?? (mappingInfo.IsKeyOrIndex ? new int?(900) : null);
				if (num3 > 8000)
				{
					num3 = (flag3 ? new int?(8000) : null);
				}
				if (num3.HasValue)
				{
					return new SqlServerByteArrayTypeMapping(null, num3, flag3);
				}
				return _variableLengthMaxBinary;
			}
		}
		return null;
	}
}
