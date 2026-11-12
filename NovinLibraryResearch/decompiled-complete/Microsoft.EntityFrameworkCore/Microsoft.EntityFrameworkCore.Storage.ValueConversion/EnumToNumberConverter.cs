using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class EnumToNumberConverter<TEnum, TNumber> : ValueConverter<TEnum, TNumber> where TEnum : struct where TNumber : struct
{
	private static readonly ConverterMappingHints _defaultHints = CreateDefaultHints();

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TEnum), typeof(TNumber), (ValueConverterInfo i) => new EnumToNumberConverter<TEnum, TNumber>(i.MappingHints), _defaultHints);


	private static ConverterMappingHints CreateDefaultHints()
	{
		Type type = typeof(TEnum).UnwrapEnumType();
		if ((!(type == typeof(long)) && !(type == typeof(ulong))) || !(typeof(TNumber) == typeof(decimal)))
		{
			return null;
		}
		return new ConverterMappingHints(null, 20, 0);
	}

	public EnumToNumberConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(ToNumber(), ToEnum(), _defaultHints?.With(mappingHints) ?? mappingHints)
	{
	}

	private static Expression<Func<TEnum, TNumber>> ToNumber()
	{
		if (!typeof(TEnum).UnwrapNullableType().IsEnum)
		{
			throw new InvalidOperationException(CoreStrings.ConverterBadType(typeof(EnumToNumberConverter<TEnum, TNumber>).ShortDisplayName(), typeof(TEnum).ShortDisplayName(), "enum types"));
		}
		ValueConverter.CheckTypeSupported(typeof(TNumber).UnwrapNullableType(), typeof(EnumToNumberConverter<TEnum, TNumber>), typeof(int), typeof(long), typeof(short), typeof(byte), typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte), typeof(double), typeof(float), typeof(decimal));
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TEnum), "value");
		return Expression.Lambda<Func<TEnum, TNumber>>(Expression.Convert((typeof(TNumber) == typeof(decimal)) ? ((Expression)Expression.Convert(parameterExpression, typeof(long))) : ((Expression)parameterExpression), typeof(TNumber)), new ParameterExpression[1] { parameterExpression });
	}

	private static Expression<Func<TNumber, TEnum>> ToEnum()
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TNumber), "value");
		return Expression.Lambda<Func<TNumber, TEnum>>(Expression.Convert((typeof(TNumber) == typeof(decimal)) ? ((Expression)Expression.Convert(parameterExpression, typeof(long))) : ((Expression)parameterExpression), typeof(TEnum)), new ParameterExpression[1] { parameterExpression });
	}
}
