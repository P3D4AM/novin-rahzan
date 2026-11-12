using System;
using System.Globalization;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringDateTimeConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	protected static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(48);

	public StringDateTimeConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<DateTime, string>> ToString()
	{
		return (DateTime v) => v.ToString("yyyy\\-MM\\-dd HH\\:mm\\:ss.FFFFFFF");
	}

	protected static Expression<Func<string, DateTime>> ToDateTime()
	{
		return (string v) => (v == null) ? default(DateTime) : DateTime.Parse(v, CultureInfo.InvariantCulture);
	}
}
