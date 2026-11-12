using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class RelationalValueGeneratorSelector : ValueGeneratorSelector
{
	private readonly TemporaryNumberValueGeneratorFactory _numberFactory = new TemporaryNumberValueGeneratorFactory();

	public RelationalValueGeneratorSelector([JetBrains.Annotations.NotNull] ValueGeneratorSelectorDependencies dependencies)
		: base(dependencies)
	{
	}

	public override ValueGenerator Create(IProperty property, IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		if (property.ValueGenerated != 0)
		{
			Type type = property.ClrType.UnwrapNullableType().UnwrapEnumType();
			if (type.IsInteger() || type == typeof(decimal) || type == typeof(float) || type == typeof(double))
			{
				return _numberFactory.Create(property);
			}
			if (type == typeof(DateTime))
			{
				return new TemporaryDateTimeValueGenerator();
			}
			if (type == typeof(DateTimeOffset))
			{
				return new TemporaryDateTimeOffsetValueGenerator();
			}
			if (property.GetDefaultValueSql() != null)
			{
				if (type == typeof(Guid))
				{
					return new TemporaryGuidValueGenerator();
				}
				if (type == typeof(string))
				{
					return new StringValueGenerator(generateTemporaryValues: true);
				}
				if (type == typeof(byte[]))
				{
					return new BinaryValueGenerator(generateTemporaryValues: true);
				}
			}
		}
		return base.Create(property, entityType);
	}
}
