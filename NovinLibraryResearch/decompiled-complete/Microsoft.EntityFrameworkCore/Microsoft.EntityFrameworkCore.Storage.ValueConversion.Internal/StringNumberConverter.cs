using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public class StringNumberConverter<TModel, TProvider, TNumber> : ValueConverter<TModel, TProvider>
{
	protected static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(64);

	public StringNumberConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
	}

	protected static Expression<Func<string, TNumber>> ToNumber()
	{
		Type type = typeof(TNumber).UnwrapNullableType();
		ValueConverter.CheckTypeSupported(type, typeof(StringNumberConverter<TModel, TProvider, TNumber>), typeof(int), typeof(long), typeof(short), typeof(byte), typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte), typeof(decimal), typeof(float), typeof(double));
		MethodInfo method = type.GetMethod("TryParse", new Type[4]
		{
			typeof(string),
			typeof(NumberStyles),
			typeof(IFormatProvider),
			type.MakeByRefType()
		});
		ParameterExpression parameterExpression = Expression.Variable(type, "parsed");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(string), "v");
		return Expression.Lambda<Func<string, TNumber>>(Expression.Block(typeof(TNumber), new ParameterExpression[1] { parameterExpression }, Expression.Condition(Expression.Call(method, parameterExpression2, Expression.Constant(NumberStyles.Any), Expression.Constant(CultureInfo.InvariantCulture, typeof(IFormatProvider)), parameterExpression), typeof(TNumber).IsNullableType() ? ((Expression)Expression.Convert(parameterExpression, typeof(TNumber))) : ((Expression)parameterExpression), Expression.Constant(default(TNumber), typeof(TNumber)))), new ParameterExpression[1] { parameterExpression2 });
	}

	protected new static Expression<Func<TNumber, string>> ToString()
	{
		Type type = typeof(TNumber).UnwrapNullableType();
		ValueConverter.CheckTypeSupported(type, typeof(StringNumberConverter<TModel, TProvider, TNumber>), typeof(int), typeof(long), typeof(short), typeof(byte), typeof(uint), typeof(ulong), typeof(ushort), typeof(sbyte), typeof(decimal), typeof(float), typeof(double));
		return (TNumber v) => ((object)v == null) ? null : string.Format(CultureInfo.InvariantCulture, (type == typeof(float) || type == typeof(double)) ? "{0:R}" : "{0}", v);
	}
}
