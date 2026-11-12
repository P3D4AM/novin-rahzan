using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public abstract class ValueConverter
{
	public abstract Func<object, object> ConvertToProvider { get; }

	public abstract Func<object, object> ConvertFromProvider { get; }

	public virtual LambdaExpression ConvertToProviderExpression { get; }

	public virtual LambdaExpression ConvertFromProviderExpression { get; }

	public abstract Type ModelClrType { get; }

	public abstract Type ProviderClrType { get; }

	public virtual ConverterMappingHints MappingHints { get; }

	protected ValueConverter([JetBrains.Annotations.NotNull] LambdaExpression convertToProviderExpression, [JetBrains.Annotations.NotNull] LambdaExpression convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(convertToProviderExpression, "convertToProviderExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(convertFromProviderExpression, "convertFromProviderExpression");
		ConvertToProviderExpression = convertToProviderExpression;
		ConvertFromProviderExpression = convertFromProviderExpression;
		MappingHints = mappingHints;
	}

	protected static Type CheckTypeSupported([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] Type converterType, [JetBrains.Annotations.NotNull] params Type[] supportedTypes)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(converterType, "converterType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(supportedTypes, "supportedTypes");
		if (!supportedTypes.Contains(type))
		{
			throw new InvalidOperationException(CoreStrings.ConverterBadType(converterType.ShortDisplayName(), type.ShortDisplayName(), string.Join(", ", supportedTypes.Select((Type t) => t.ShortDisplayName()))));
		}
		return type;
	}

	public virtual ValueConverter ComposeWith([JetBrains.Annotations.CanBeNull] ValueConverter secondConverter)
	{
		if (secondConverter == null)
		{
			return this;
		}
		if (ProviderClrType.UnwrapNullableType() != secondConverter.ModelClrType.UnwrapNullableType())
		{
			throw new ArgumentException(CoreStrings.ConvertersCannotBeComposed(ModelClrType.ShortDisplayName(), ProviderClrType.ShortDisplayName(), secondConverter.ModelClrType.ShortDisplayName(), secondConverter.ProviderClrType.ShortDisplayName()));
		}
		ValueConverter valueConverter = ((ProviderClrType.IsNullableType() && !secondConverter.ModelClrType.IsNullableType()) ? ComposeWith((ValueConverter)Activator.CreateInstance(typeof(CastingConverter<, >).MakeGenericType(ProviderClrType, secondConverter.ModelClrType), MappingHints)) : this);
		return (ValueConverter)Activator.CreateInstance(typeof(CompositeValueConverter<, , >).MakeGenericType(valueConverter.ModelClrType, valueConverter.ProviderClrType, secondConverter.ProviderClrType), valueConverter, secondConverter, (secondConverter.MappingHints == null) ? valueConverter.MappingHints : secondConverter.MappingHints.With(valueConverter.MappingHints));
	}
}
public class ValueConverter<TModel, TProvider> : ValueConverter
{
	private Func<object, object> _convertToProvider;

	private Func<object, object> _convertFromProvider;

	public override Func<object, object> ConvertToProvider => NonCapturingLazyInitializer.EnsureInitialized(ref _convertToProvider, this, (ValueConverter<TModel, TProvider> c) => SanitizeConverter(c.ConvertToProviderExpression));

	public override Func<object, object> ConvertFromProvider => NonCapturingLazyInitializer.EnsureInitialized(ref _convertFromProvider, this, (ValueConverter<TModel, TProvider> c) => SanitizeConverter(c.ConvertFromProviderExpression));

	public new virtual Expression<Func<TModel, TProvider>> ConvertToProviderExpression => (Expression<Func<TModel, TProvider>>)base.ConvertToProviderExpression;

	public new virtual Expression<Func<TProvider, TModel>> ConvertFromProviderExpression => (Expression<Func<TProvider, TModel>>)base.ConvertFromProviderExpression;

	public override Type ModelClrType => typeof(TModel);

	public override Type ProviderClrType => typeof(TProvider);

	public ValueConverter([JetBrains.Annotations.NotNull] Expression<Func<TModel, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TModel>> convertFromProviderExpression, [JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
	{
		_convertToProvider = SanitizeConverter(convertToProviderExpression);
		_convertFromProvider = SanitizeConverter(convertFromProviderExpression);
	}

	private static Func<object, object> SanitizeConverter<TIn, TOut>(Expression<Func<TIn, TOut>> convertExpression)
	{
		Func<TIn, TOut> compiled = convertExpression.Compile();
		return (object v) => (v != null) ? ((object)compiled(Sanitize<TIn>(v))) : null;
	}

	private static T Sanitize<T>(object value)
	{
		Type type = typeof(T).UnwrapNullableType();
		return (T)((!type.IsInstanceOfType(value)) ? Convert.ChangeType(value, type) : value);
	}
}
