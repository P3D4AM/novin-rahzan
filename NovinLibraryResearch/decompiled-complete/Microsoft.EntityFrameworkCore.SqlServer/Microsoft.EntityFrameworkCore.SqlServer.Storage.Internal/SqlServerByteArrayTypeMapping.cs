using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerByteArrayTypeMapping : ByteArrayTypeMapping
{
	private const int MaxSize = 8000;

	private readonly SqlDbType? _sqlDbType;

	public SqlServerByteArrayTypeMapping([JetBrains.Annotations.CanBeNull] string storeType = null, int? size = null, bool fixedLength = false, ValueComparer comparer = null, SqlDbType? sqlDbType = null, StoreTypePostfix? storeTypePostfix = null)
		: this(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(byte[]), null, comparer), storeType ?? (fixedLength ? "binary" : "varbinary"), storeTypePostfix ?? StoreTypePostfix.Size, System.Data.DbType.Binary, unicode: false, size, fixedLength), sqlDbType)
	{
	}

	protected SqlServerByteArrayTypeMapping(RelationalTypeMappingParameters parameters, SqlDbType? sqlDbType)
		: base(parameters)
	{
		_sqlDbType = sqlDbType;
	}

	private static int CalculateSize(int? size)
	{
		if (!size.HasValue || !(size < 8000))
		{
			return 8000;
		}
		return size.Value;
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerByteArrayTypeMapping(parameters, _sqlDbType);
	}

	protected override void ConfigureParameter(DbParameter parameter)
	{
		object value = parameter.Value;
		int? num = (value as byte[])?.Length;
		int num2 = CalculateSize(Size);
		if (_sqlDbType.HasValue && parameter is SqlParameter sqlParameter)
		{
			sqlParameter.SqlDbType = _sqlDbType.Value;
		}
		parameter.Size = ((value == null || value == DBNull.Value || (num.HasValue && num <= num2)) ? num2 : (-1));
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("0x");
		byte[] array = (byte[])value;
		foreach (byte b in array)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}
}
