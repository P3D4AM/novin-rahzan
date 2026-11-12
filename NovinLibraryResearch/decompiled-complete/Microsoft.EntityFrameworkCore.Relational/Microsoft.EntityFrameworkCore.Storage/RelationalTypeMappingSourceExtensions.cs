using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public static class RelationalTypeMappingSourceExtensions
{
	public static RelationalTypeMapping GetMappingForValue([JetBrains.Annotations.CanBeNull] this IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.CanBeNull] object value)
	{
		if (value != null && value != DBNull.Value && typeMappingSource != null)
		{
			return typeMappingSource.GetMapping(value.GetType());
		}
		return RelationalTypeMapping.NullMapping;
	}

	public static RelationalTypeMapping GetMapping([JetBrains.Annotations.NotNull] this IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		RelationalTypeMapping relationalTypeMapping = typeMappingSource.FindMapping(property);
		if (relationalTypeMapping != null)
		{
			return relationalTypeMapping;
		}
		throw new InvalidOperationException(RelationalStrings.UnsupportedPropertyType(property.DeclaringEntityType.DisplayName(), property.Name, property.ClrType.ShortDisplayName()));
	}

	public static RelationalTypeMapping GetMapping([JetBrains.Annotations.NotNull] this IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] Type clrType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
		RelationalTypeMapping relationalTypeMapping = typeMappingSource.FindMapping(clrType);
		if (relationalTypeMapping != null)
		{
			return relationalTypeMapping;
		}
		throw new InvalidOperationException(RelationalStrings.UnsupportedType(clrType.ShortDisplayName()));
	}

	public static RelationalTypeMapping GetMapping([JetBrains.Annotations.NotNull] this IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] string typeName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeName, "typeName");
		RelationalTypeMapping relationalTypeMapping = typeMappingSource.FindMapping(typeName);
		if (relationalTypeMapping != null)
		{
			return relationalTypeMapping;
		}
		throw new InvalidOperationException(RelationalStrings.UnsupportedType(typeName));
	}
}
