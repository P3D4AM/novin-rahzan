using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringUriConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	public StringUriConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<Uri, string>> ToString()
	{
		return (Uri v) => (v != null) ? v.ToString() : null;
	}

	protected static Expression<Func<string, Uri>> ToUri()
	{
		return (string v) => ConvertToUri(v);
	}

	private static Uri ConvertToUri(string value)
	{
		if (!Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out Uri result))
		{
			return null;
		}
		return result;
	}
}
