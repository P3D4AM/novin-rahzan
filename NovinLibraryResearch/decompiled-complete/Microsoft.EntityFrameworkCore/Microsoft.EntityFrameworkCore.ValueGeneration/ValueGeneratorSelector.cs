using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class ValueGeneratorSelector : IValueGeneratorSelector
{
	public virtual IValueGeneratorCache Cache => Dependencies.Cache;

	protected virtual ValueGeneratorSelectorDependencies Dependencies { get; }

	public ValueGeneratorSelector([JetBrains.Annotations.NotNull] ValueGeneratorSelectorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual ValueGenerator Select(IProperty property, IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return Cache.GetOrAdd(property, entityType, (IProperty p, IEntityType t) => CreateFromFactory(p, t) ?? Create(p, t));
	}

	private static ValueGenerator CreateFromFactory(IProperty property, IEntityType entityType)
	{
		Func<IProperty, IEntityType, ValueGenerator> func = property.GetValueGeneratorFactory();
		if (func == null)
		{
			CoreTypeMapping coreTypeMapping = property.FindTypeMapping();
			func = coreTypeMapping?.ValueGeneratorFactory;
			if (func == null)
			{
				ValueConverter valueConverter = coreTypeMapping?.Converter ?? property.GetValueConverter();
				if (valueConverter != null)
				{
					throw new NotSupportedException(CoreStrings.ValueGenWithConversion(property.DeclaringEntityType.DisplayName(), property.Name, valueConverter.GetType().ShortDisplayName()));
				}
			}
		}
		return func?.Invoke(property, entityType);
	}

	public virtual ValueGenerator Create([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Type type = property.ClrType.UnwrapNullableType().UnwrapEnumType();
		if (type == typeof(Guid))
		{
			return new GuidValueGenerator();
		}
		if (type == typeof(string))
		{
			return new StringValueGenerator(generateTemporaryValues: false);
		}
		if (type == typeof(byte[]))
		{
			return new BinaryValueGenerator(generateTemporaryValues: false);
		}
		throw new NotSupportedException(CoreStrings.NoValueGenerator(property.Name, property.DeclaringEntityType.DisplayName(), type.ShortDisplayName()));
	}
}
