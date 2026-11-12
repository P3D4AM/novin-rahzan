using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringEnumConverter<TModel, TProvider, TEnum> : ValueConverter<TModel, TProvider> where TEnum : struct
{
	public StringEnumConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected new static Expression<Func<TEnum, string>> ToString()
	{
		return (TEnum v) => v.ToString();
	}

	protected static Expression<Func<string, TEnum>> ToEnum()
	{
		if (!typeof(TEnum).UnwrapNullableType().IsEnum)
		{
			throw new InvalidOperationException(CoreStrings.ConverterBadType(typeof(StringEnumConverter<TModel, TProvider, TEnum>).ShortDisplayName(), typeof(TEnum).ShortDisplayName(), "enum types"));
		}
		return (string v) => ConvertToEnum(v);
	}

	private static TEnum ConvertToEnum(string value)
	{
		if (!Enum.TryParse<TEnum>(value, out var result))
		{
			if (!Enum.TryParse<TEnum>(value, ignoreCase: true, out result))
			{
				if (!ulong.TryParse(value, out var result2))
				{
					if (!long.TryParse(value, out var result3))
					{
						return default(TEnum);
					}
					return (TEnum)(object)result3;
				}
				return (TEnum)(object)result2;
			}
			return result;
		}
		return result;
	}
}
