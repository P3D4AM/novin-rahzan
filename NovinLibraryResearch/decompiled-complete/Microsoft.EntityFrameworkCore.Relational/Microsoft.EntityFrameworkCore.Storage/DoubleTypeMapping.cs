using System;
using System.Data;
using System.Globalization;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class DoubleTypeMapping : RelationalTypeMapping
{
	public DoubleTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(double), dbType)
	{
	}

	protected DoubleTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new DoubleTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		double d = Convert.ToDouble(value);
		string text = d.ToString("G17", CultureInfo.InvariantCulture);
		if (text.Contains("E") || text.Contains("e") || text.Contains(".") || double.IsNaN(d) || double.IsInfinity(d))
		{
			return text;
		}
		return text + ".0";
	}
}
