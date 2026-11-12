using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class CastingConverter<TModel, TProvider> : ValueConverter<TModel, TProvider>
{
	private static readonly ConverterMappingHints _defaultHints = CreateDefaultHints();

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TModel), typeof(TProvider), (ValueConverterInfo i) => new CastingConverter<TModel, TProvider>(i.MappingHints), _defaultHints);


	private static ConverterMappingHints CreateDefaultHints()
	{
		if (typeof(TProvider).UnwrapNullableType() == typeof(decimal))
		{
			Type type = typeof(TModel).UnwrapNullableType().UnwrapEnumType();
			if (type == typeof(long) || type == typeof(ulong))
			{
				return new ConverterMappingHints(null, 20, 0);
			}
			if (type == typeof(float) || type == typeof(double))
			{
				return new ConverterMappingHints(null, 38, 17);
			}
		}
		return null;
	}

	public CastingConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(Convert<TModel, TProvider>(), Convert<TProvider, TModel>(), _defaultHints?.With(mappingHints) ?? mappingHints)
	{
	}

	private static Expression<Func<TIn, TOut>> Convert<TIn, TOut>()
	{
		return (TIn v) => (TOut)v;
	}
}
