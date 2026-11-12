using System;
using System.Globalization;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringDateTimeOffsetConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	protected static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(48);

	public StringDateTimeOffsetConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<DateTimeOffset, string>> ToString()
	{
		return (DateTimeOffset v) => v.ToString("yyyy\\-MM\\-dd HH\\:mm\\:ss.FFFFFFFzzz");
	}

	protected static Expression<Func<string, DateTimeOffset>> ToDateTimeOffset()
	{
		return (string v) => (v == null) ? default(DateTimeOffset) : DateTimeOffset.Parse(v, CultureInfo.InvariantCulture);
	}
}
