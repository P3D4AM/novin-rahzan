using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class PropertyExtensions
{
	public static bool ForAdd(this ValueGenerated valueGenerated)
	{
		return (valueGenerated & ValueGenerated.OnAdd) != 0;
	}

	public static bool ForUpdate(this ValueGenerated valueGenerated)
	{
		return (valueGenerated & ValueGenerated.OnUpdate) != 0;
	}

	public static IEnumerable<IEntityType> GetContainingEntityTypes([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return property.DeclaringEntityType.GetDerivedTypesInclusive();
	}

	public static IEnumerable<IForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return property.GetContainingKeys().SelectMany((IKey k) => k.GetReferencingForeignKeys());
	}

	public static IProperty GetGenerationProperty([JetBrains.Annotations.NotNull] this IProperty property)
	{
		List<IProperty> list = new List<IProperty> { property };
		for (int i = 0; i < list.Count; i++)
		{
			IProperty property2 = list[i];
			if (property2.RequiresValueGenerator())
			{
				return property2;
			}
			foreach (IForeignKey containingForeignKey in property2.GetContainingForeignKeys())
			{
				for (int j = 0; j < containingForeignKey.Properties.Count; j++)
				{
					if (property2 == containingForeignKey.Properties[j])
					{
						IProperty item = containingForeignKey.PrincipalKey.Properties[j];
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
			}
		}
		return null;
	}

	public static bool RequiresValueGenerator([JetBrains.Annotations.NotNull] this IProperty property)
	{
		if ((property.ValueGenerated & ValueGenerated.OnAdd) != ValueGenerated.OnAdd || property.IsForeignKey() || !property.IsKey())
		{
			return property.GetValueGeneratorFactory() != null;
		}
		return true;
	}

	public static bool MayBeStoreGenerated([JetBrains.Annotations.NotNull] this IProperty property)
	{
		if (property.ValueGenerated != 0)
		{
			return true;
		}
		if (property.IsKeyOrForeignKey())
		{
			IProperty generationProperty = property.GetGenerationProperty();
			if (generationProperty != null)
			{
				return generationProperty.ValueGenerated != ValueGenerated.Never;
			}
			return false;
		}
		return false;
	}

	public static bool RequiresOriginalValue([JetBrains.Annotations.NotNull] this IProperty property)
	{
		if (property.DeclaringEntityType.GetChangeTrackingStrategy() == ChangeTrackingStrategy.ChangingAndChangedNotifications && !property.IsConcurrencyToken && !property.IsKey())
		{
			return property.IsForeignKey();
		}
		return true;
	}

	public static bool IsKeyOrForeignKey([JetBrains.Annotations.NotNull] this IProperty property)
	{
		if (!property.IsKey())
		{
			return property.IsForeignKey();
		}
		return true;
	}

	public static IReadOnlyList<IProperty> FindPrincipals([JetBrains.Annotations.NotNull] this IProperty property)
	{
		List<IProperty> list = new List<IProperty> { property };
		AddPrincipals(property, list);
		return list;
	}

	private static void AddPrincipals(IProperty property, List<IProperty> visited)
	{
		Property property2 = property.AsProperty("AddPrincipals");
		if (property2.ForeignKeys == null)
		{
			return;
		}
		foreach (ForeignKey foreignKey in property2.ForeignKeys)
		{
			for (int i = 0; i < foreignKey.Properties.Count; i++)
			{
				if (property == foreignKey.Properties[i])
				{
					Property property3 = foreignKey.PrincipalKey.Properties[i];
					if (!visited.Contains(property3))
					{
						visited.Add(property3);
						AddPrincipals(property3, visited);
					}
				}
			}
		}
	}

	public static string ToDebugString([JetBrains.Annotations.NotNull] this IProperty property, bool singleLine = true, bool includeIndexes = false, [JetBrains.Annotations.NotNull] string indent = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(indent);
		if (singleLine)
		{
			stringBuilder.Append("Property: " + property.DeclaringEntityType.DisplayName() + ".");
		}
		stringBuilder.Append(property.Name).Append(" (");
		string fieldName = property.GetFieldName();
		if (fieldName == null)
		{
			stringBuilder.Append("no field, ");
		}
		else if (!fieldName.EndsWith(">k__BackingField", StringComparison.Ordinal))
		{
			stringBuilder.Append(fieldName).Append(", ");
		}
		stringBuilder.Append(property.ClrType.ShortDisplayName()).Append(")");
		if (property.IsShadowProperty())
		{
			stringBuilder.Append(" Shadow");
		}
		if (!property.IsNullable)
		{
			stringBuilder.Append(" Required");
		}
		if (property.IsPrimaryKey())
		{
			stringBuilder.Append(" PK");
		}
		if (property.IsForeignKey())
		{
			stringBuilder.Append(" FK");
		}
		if (property.IsKey() && !property.IsPrimaryKey())
		{
			stringBuilder.Append(" AlternateKey");
		}
		if (property.IsIndex())
		{
			stringBuilder.Append(" Index");
		}
		if (property.IsConcurrencyToken)
		{
			stringBuilder.Append(" Concurrency");
		}
		if (property.GetBeforeSaveBehavior() != 0)
		{
			stringBuilder.Append(" BeforeSave:").Append(property.GetBeforeSaveBehavior());
		}
		if (property.GetAfterSaveBehavior() != 0)
		{
			stringBuilder.Append(" AfterSave:").Append(property.GetAfterSaveBehavior());
		}
		if (property.ValueGenerated != 0)
		{
			stringBuilder.Append(" ValueGenerated.").Append(property.ValueGenerated);
		}
		if (property.GetMaxLength().HasValue)
		{
			stringBuilder.Append(" MaxLength").Append(property.GetMaxLength());
		}
		if (property.IsUnicode() == false)
		{
			stringBuilder.Append(" Ansi");
		}
		if (property.GetPropertyAccessMode() != PropertyAccessMode.PreferField)
		{
			stringBuilder.Append(" PropertyAccessMode.").Append(property.GetPropertyAccessMode());
		}
		if (includeIndexes)
		{
			PropertyIndexes propertyIndexes = property.GetPropertyIndexes();
			if (propertyIndexes != null)
			{
				stringBuilder.Append(" ").Append(propertyIndexes.Index);
				stringBuilder.Append(" ").Append(propertyIndexes.OriginalValueIndex);
				stringBuilder.Append(" ").Append(propertyIndexes.RelationshipIndex);
				stringBuilder.Append(" ").Append(propertyIndexes.ShadowIndex);
				stringBuilder.Append(" ").Append(propertyIndexes.StoreGenerationIndex);
			}
		}
		if (!singleLine)
		{
			stringBuilder.Append(property.AnnotationsToDebugString(indent + "  "));
		}
		return stringBuilder.ToString();
	}

	public static Property AsProperty([JetBrains.Annotations.NotNull] this IProperty property, [JetBrains.Annotations.NotNull][CallerMemberName] string methodName = "")
	{
		return MetadataExtensions.AsConcreteMetadataType<IProperty, Property>(property, methodName);
	}
}
