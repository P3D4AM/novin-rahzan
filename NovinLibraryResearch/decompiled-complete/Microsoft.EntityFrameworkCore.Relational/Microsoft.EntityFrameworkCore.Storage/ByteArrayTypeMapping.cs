using System.Data;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class ByteArrayTypeMapping : RelationalTypeMapping
{
	public ByteArrayTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null, int? size = null)
		: base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(byte[])), storeType, StoreTypePostfix.None, dbType, unicode: false, size))
	{
	}

	protected ByteArrayTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new ByteArrayTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("X'");
		byte[] array = (byte[])value;
		foreach (byte b in array)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		stringBuilder.Append("'");
		return stringBuilder.ToString();
	}
}
