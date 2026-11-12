using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToBoolConverter : ValueConverter<string, bool>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(bool), (ValueConverterInfo i) => new StringToBoolConverter(i.MappingHints));


	public StringToBoolConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<string, bool>>)((string v) => Convert.ToBoolean(v)), (Expression<Func<bool, string>>)((bool v) => Convert.ToString(v)), mappingHints)
	{
	}
}
