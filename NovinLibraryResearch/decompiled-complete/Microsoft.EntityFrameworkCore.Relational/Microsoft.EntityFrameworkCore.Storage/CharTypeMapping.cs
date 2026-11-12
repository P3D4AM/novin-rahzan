using System;
using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class CharTypeMapping : RelationalTypeMapping
{
	public CharTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(char), dbType)
	{
	}

	protected CharTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new CharTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		char c = Convert.ToChar(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value"));
		if (c == '\'')
		{
			return "''''";
		}
		return "'" + c + "'";
	}
}
