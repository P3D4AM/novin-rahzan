using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Storage;

public abstract class CoreTypeMapping
{
	protected readonly struct CoreTypeMappingParameters
	{
		public Type ClrType { get; }

		public ValueConverter Converter { get; }

		public ValueComparer Comparer { get; }

		public ValueComparer KeyComparer { get; }

		public ValueComparer StructuralComparer { get; }

		public Func<IProperty, IEntityType, ValueGenerator> ValueGeneratorFactory { get; }

		public CoreTypeMappingParameters([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.CanBeNull] ValueConverter converter = null, [JetBrains.Annotations.CanBeNull] ValueComparer comparer = null, [JetBrains.Annotations.CanBeNull] ValueComparer keyComparer = null, [JetBrains.Annotations.CanBeNull] ValueComparer structuralComparer = null, [JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> valueGeneratorFactory = null)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
			ClrType = clrType;
			Converter = converter;
			Comparer = comparer;
			KeyComparer = keyComparer;
			StructuralComparer = structuralComparer;
			ValueGeneratorFactory = valueGeneratorFactory;
		}

		public CoreTypeMappingParameters WithComposedConverter([JetBrains.Annotations.CanBeNull] ValueConverter converter)
		{
			return new CoreTypeMappingParameters(ClrType, (converter == null) ? Converter : converter.ComposeWith(Converter), Comparer, KeyComparer, StructuralComparer, ValueGeneratorFactory);
		}
	}

	private ValueComparer _comparer;

	private ValueComparer _keyComparer;

	private readonly ValueComparer _structuralComparer;

	protected virtual CoreTypeMappingParameters Parameters { get; }

	public virtual Type ClrType { get; }

	public virtual ValueConverter Converter => Parameters.Converter;

	public virtual Func<IProperty, IEntityType, ValueGenerator> ValueGeneratorFactory { get; }

	public virtual ValueComparer Comparer => NonCapturingLazyInitializer.EnsureInitialized(ref _comparer, this, (CoreTypeMapping c) => CreateComparer(c.ClrType, favorStructuralComparisons: false));

	public virtual ValueComparer KeyComparer => NonCapturingLazyInitializer.EnsureInitialized(ref _keyComparer, this, (CoreTypeMapping c) => CreateComparer(c.ClrType, favorStructuralComparisons: true));

	public virtual ValueComparer StructuralComparer => _structuralComparer ?? KeyComparer;

	protected CoreTypeMapping(CoreTypeMappingParameters parameters)
	{
		Parameters = parameters;
		ValueConverter converter = parameters.Converter;
		Type type = (ClrType = converter?.ModelClrType ?? parameters.ClrType);
		if (parameters.Comparer?.Type == type)
		{
			_comparer = parameters.Comparer;
		}
		if (parameters.KeyComparer?.Type == type)
		{
			_keyComparer = parameters.KeyComparer;
		}
		if (parameters.StructuralComparer?.Type == type)
		{
			_structuralComparer = parameters.StructuralComparer;
		}
		ValueGeneratorFactory = parameters.ValueGeneratorFactory ?? converter?.MappingHints?.ValueGeneratorFactory;
	}

	private static ValueComparer CreateComparer(Type clrType, bool favorStructuralComparisons)
	{
		return (ValueComparer)Activator.CreateInstance(typeof(ValueComparer<>).MakeGenericType(clrType), new object[1] { favorStructuralComparisons });
	}

	public abstract CoreTypeMapping Clone([JetBrains.Annotations.CanBeNull] ValueConverter converter);

	public virtual Expression GenerateCodeLiteral([JetBrains.Annotations.NotNull] object value)
	{
		throw new NotSupportedException(CoreStrings.LiteralGenerationNotSupported(ClrType.ShortDisplayName()));
	}
}
