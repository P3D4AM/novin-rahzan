using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class BoolToTwoValuesConverter<TProvider> : ValueConverter<bool, TProvider>
{
	public BoolToTwoValuesConverter([JetBrains.Annotations.CanBeNull] TProvider falseValue, [JetBrains.Annotations.CanBeNull] TProvider trueValue, [JetBrains.Annotations.CanBeNull] Expression<Func<TProvider, bool>> fromProvider = null, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(ToProvider(falseValue, trueValue), fromProvider ?? ToBool(trueValue), mappingHints)
	{
	}

	private static Expression<Func<bool, TProvider>> ToProvider(TProvider falseValue, TProvider trueValue)
	{
		return (bool v) => v ? trueValue : falseValue;
	}

	private static Expression<Func<TProvider, bool>> ToBool(TProvider trueValue)
	{
		return (TProvider v) => (object)v == (object)trueValue;
	}
}
