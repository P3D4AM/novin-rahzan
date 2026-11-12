using System;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerStringTypeMapping : StringTypeMapping
{
	private const int UnicodeMax = 4000;

	private const int AnsiMax = 8000;

	private readonly SqlDbType? _sqlDbType;

	private readonly int _maxSpecificSize;

	public SqlServerStringTypeMapping([JetBrains.Annotations.CanBeNull] string storeType = null, bool unicode = false, int? size = null, bool fixedLength = false, SqlDbType? sqlDbType = null, StoreTypePostfix? storeTypePostfix = null)
		: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(string)), storeType ?? GetStoreName(unicode, fixedLength), storeTypePostfix ?? StoreTypePostfix.Size, GetDbType(unicode, fixedLength), unicode, size, fixedLength), sqlDbType)
	{
	}

	private static string GetStoreName(bool unicode, bool fixedLength)
	{
		if (!unicode)
		{
			if (!fixedLength)
			{
				return "varchar";
			}
			return "char";
		}
		if (!fixedLength)
		{
			return "nvarchar";
		}
		return "nchar";
	}

	private static DbType? GetDbType(bool unicode, bool fixedLength)
	{
		if (!unicode)
		{
			return System.Data.DbType.AnsiString;
		}
		if (!fixedLength)
		{
			return null;
		}
		return System.Data.DbType.String;
	}

	protected SqlServerStringTypeMapping(RelationalTypeMappingParameters parameters, SqlDbType? sqlDbType)
		: base(parameters)
	{
		_maxSpecificSize = CalculateSize(parameters.Unicode, parameters.Size);
		_sqlDbType = sqlDbType;
	}

	private static int CalculateSize(bool unicode, int? size)
	{
		if (!unicode)
		{
			if (!size.HasValue || !(size <= 8000))
			{
				return 8000;
			}
			return size.Value;
		}
		if (!size.HasValue || !(size <= 4000))
		{
			return 4000;
		}
		return size.Value;
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerStringTypeMapping(parameters, _sqlDbType);
	}

	protected override void ConfigureParameter(DbParameter parameter)
	{
		object value = parameter.Value;
		int? num = (value as string)?.Length;
		if (_sqlDbType.HasValue && parameter is SqlParameter sqlParameter)
		{
			sqlParameter.SqlDbType = _sqlDbType.Value;
		}
		parameter.Size = ((value == null || value == DBNull.Value || (num.HasValue && num <= _maxSpecificSize)) ? _maxSpecificSize : (-1));
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		if (!IsUnicode)
		{
			return "'" + EscapeSqlLiteral((string)value) + "'";
		}
		return "N'" + EscapeSqlLiteral((string)value) + "'";
	}
}
