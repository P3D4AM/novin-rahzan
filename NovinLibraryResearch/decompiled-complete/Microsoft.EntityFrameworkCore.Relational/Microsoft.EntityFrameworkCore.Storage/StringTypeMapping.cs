using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class StringTypeMapping : RelationalTypeMapping
{
	public StringTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null, bool unicode = false, int? size = null)
		: base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(string)), storeType, StoreTypePostfix.None, dbType, unicode, size))
	{
	}

	protected StringTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new StringTypeMapping(parameters);
	}

	protected virtual string EscapeSqlLiteral([JetBrains.Annotations.NotNull] string literal)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(literal, "literal").Replace("'", "''");
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		return "'" + EscapeSqlLiteral((string)value) + "'";
	}
}
