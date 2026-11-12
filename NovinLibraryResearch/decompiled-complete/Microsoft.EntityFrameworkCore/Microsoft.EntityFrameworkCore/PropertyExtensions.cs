using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore;

public static class PropertyExtensions
{
	public static CoreTypeMapping GetTypeMapping([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return ((CoreTypeMapping)property["TypeMapping"]) ?? throw new InvalidOperationException(CoreStrings.ModelNotFinalized("GetTypeMapping"));
	}

	public static CoreTypeMapping FindTypeMapping([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (CoreTypeMapping)property["TypeMapping"];
	}

	[Obsolete("Use FindTypeMapping instead")]
	public static CoreTypeMapping FindMapping([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return property.FindTypeMapping();
	}

	public static IProperty FindFirstPrincipal([JetBrains.Annotations.NotNull] this IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Property property2 = property.AsProperty("FindFirstPrincipal");
		if (property2.ForeignKeys != null)
		{
			foreach (ForeignKey foreignKey in property2.ForeignKeys)
			{
				for (int i = 0; i < foreignKey.Properties.Count; i++)
				{
					if (property == foreignKey.Properties[i])
					{
						return foreignKey.PrincipalKey.Properties[i];
					}
				}
			}
		}
		return null;
	}

	public static bool IsForeignKey([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("IsForeignKey").ForeignKeys != null;
	}

	public static bool IsIndex([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("IsIndex").Indexes != null;
	}

	public static bool IsPrimaryKey([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return property.FindContainingPrimaryKey() != null;
	}

	public static bool IsKey([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("IsKey").Keys != null;
	}

	public static IEnumerable<IForeignKey> GetContainingForeignKeys([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("GetContainingForeignKeys").GetContainingForeignKeys();
	}

	public static IEnumerable<IIndex> GetContainingIndexes([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("GetContainingIndexes").GetContainingIndexes();
	}

	[Obsolete("Use FindContainingPrimaryKey()")]
	public static IKey GetContainingPrimaryKey([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return property.FindContainingPrimaryKey();
	}

	public static IKey FindContainingPrimaryKey([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("FindContainingPrimaryKey").PrimaryKey;
	}

	public static IEnumerable<IKey> GetContainingKeys([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property").AsProperty("GetContainingKeys").GetContainingKeys();
	}

	public static int? GetMaxLength([JetBrains.Annotations.NotNull] this IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		return (int?)property["MaxLength"];
	}

	public static bool? IsUnicode([JetBrains.Annotations.NotNull] this IProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		return (bool?)property["Unicode"];
	}

	public static PropertySaveBehavior GetBeforeSaveBehavior([JetBrains.Annotations.NotNull] this IProperty property)
	{
		PropertySaveBehavior? propertySaveBehavior = (PropertySaveBehavior?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["BeforeSaveBehavior"];
		if (!propertySaveBehavior.HasValue)
		{
			if (property.ValueGenerated != ValueGenerated.OnAddOrUpdate)
			{
				return PropertySaveBehavior.Save;
			}
			return PropertySaveBehavior.Ignore;
		}
		return propertySaveBehavior.GetValueOrDefault();
	}

	public static PropertySaveBehavior GetAfterSaveBehavior([JetBrains.Annotations.NotNull] this IProperty property)
	{
		PropertySaveBehavior? propertySaveBehavior = (PropertySaveBehavior?)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["AfterSaveBehavior"];
		if (!propertySaveBehavior.HasValue)
		{
			if (!property.IsKey())
			{
				if (!property.ValueGenerated.ForUpdate())
				{
					return PropertySaveBehavior.Save;
				}
				return PropertySaveBehavior.Ignore;
			}
			return PropertySaveBehavior.Throw;
		}
		return propertySaveBehavior.GetValueOrDefault();
	}

	public static Func<IProperty, IEntityType, ValueGenerator> GetValueGeneratorFactory([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (Func<IProperty, IEntityType, ValueGenerator>)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["ValueGeneratorFactory"];
	}

	public static ValueConverter GetValueConverter([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (ValueConverter)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["ValueConverter"];
	}

	public static Type GetProviderClrType([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (Type)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["ProviderClrType"];
	}

	public static ValueComparer GetValueComparer([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (ValueComparer)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["ValueComparer"];
	}

	public static ValueComparer GetKeyValueComparer([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (ValueComparer)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["KeyValueComparer"];
	}

	public static ValueComparer GetStructuralValueComparer([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (ValueComparer)Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property")["KeyValueComparer"];
	}

	public static string Format([JetBrains.Annotations.NotNull] this IEnumerable<IPropertyBase> properties, bool includeTypes = false)
	{
		return "{" + string.Join(", ", properties.Select((IPropertyBase p) => "'" + p.Name + "'" + (includeTypes ? (" : " + p.ClrType.DisplayName(fullName: false)) : ""))) + "}";
	}
}
