using System;
using System.Globalization;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringCharConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	public StringCharConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<char, string>> ToString()
	{
		return (char v) => string.Format(CultureInfo.InvariantCulture, "{0}", v);
	}

	protected static Expression<Func<string, char>> ToChar()
	{
		return (string v) => (v != null && v.Length >= 1) ? v[0] : '\0';
	}
}
