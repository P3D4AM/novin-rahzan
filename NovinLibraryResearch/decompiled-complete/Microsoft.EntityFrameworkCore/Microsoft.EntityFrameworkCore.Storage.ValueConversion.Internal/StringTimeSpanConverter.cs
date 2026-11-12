using System;
using System.Globalization;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringTimeSpanConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	protected static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(48);

	public StringTimeSpanConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<TimeSpan, string>> ToString()
	{
		return (TimeSpan v) => v.ToString("c");
	}

	protected static Expression<Func<string, TimeSpan>> ToTimeSpan()
	{
		return (string v) => (v == null) ? default(TimeSpan) : TimeSpan.Parse(v, CultureInfo.InvariantCulture);
	}
}
