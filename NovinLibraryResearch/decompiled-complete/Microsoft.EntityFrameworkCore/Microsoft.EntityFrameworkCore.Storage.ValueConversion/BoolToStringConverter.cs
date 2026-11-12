using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class BoolToStringConverter : BoolToTwoValuesConverter<string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(bool), typeof(string), (ValueConverterInfo i) => new BoolToStringConverter("0", "1", i.MappingHints), new ConverterMappingHints(1));


	public BoolToStringConverter([JetBrains.Annotations.NotNull] string falseValue, [JetBrains.Annotations.NotNull] string trueValue, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(falseValue, "falseValue"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(trueValue, "trueValue"), FromProvider(trueValue), new ConverterMappingHints(Math.Max(falseValue.Length, trueValue.Length)).With(mappingHints))
	{
	}

	private static Expression<Func<string, bool>> FromProvider(string trueValue)
	{
		char testChar = trueValue.ToUpperInvariant()[0];
		return (string v) => !string.IsNullOrEmpty(v) && v.ToUpperInvariant()[0] == testChar;
	}
}
