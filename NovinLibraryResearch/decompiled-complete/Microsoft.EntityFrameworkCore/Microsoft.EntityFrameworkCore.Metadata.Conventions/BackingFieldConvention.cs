using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class BackingFieldConvention : IPropertyAddedConvention, IConvention, INavigationAddedConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public BackingFieldConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
	{
		FieldInfo fieldToSet = GetFieldToSet(propertyBuilder.Metadata);
		if (fieldToSet != null)
		{
			propertyBuilder.HasField(fieldToSet);
		}
	}

	public virtual void ProcessNavigationAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionNavigation navigation, IConventionContext<IConventionNavigation> context)
	{
		FieldInfo fieldToSet = GetFieldToSet(navigation);
		if (fieldToSet != null)
		{
			relationshipBuilder.HasField(fieldToSet, navigation.IsDependentToPrincipal());
		}
	}

	private FieldInfo GetFieldToSet(IConventionPropertyBase propertyBase)
	{
		if (propertyBase == null || !ConfigurationSource.Convention.Overrides(propertyBase.GetFieldInfoConfigurationSource()))
		{
			return null;
		}
		Type type = propertyBase.DeclaringType.ClrType;
		while (type != null)
		{
			FieldInfo fieldInfo = TryMatchFieldName(propertyBase, type);
			if (fieldInfo != null && (propertyBase.PropertyInfo != null || propertyBase.Name == fieldInfo.GetSimpleMemberName()))
			{
				return fieldInfo;
			}
			type = type.GetTypeInfo().BaseType;
		}
		return null;
	}

	private static FieldInfo TryMatchFieldName(IConventionPropertyBase propertyBase, Type entityClrType)
	{
		IConventionModel model = propertyBase.DeclaringType.Model;
		string name = propertyBase.Name;
		IConventionEntityType conventionEntityType = model.FindEntityType(entityClrType);
		IReadOnlyDictionary<string, FieldInfo> source;
		if (conventionEntityType == null)
		{
			Dictionary<string, FieldInfo> dictionary = new Dictionary<string, FieldInfo>(StringComparer.Ordinal);
			foreach (FieldInfo runtimeField in entityClrType.GetRuntimeFields())
			{
				if (!runtimeField.IsStatic && !dictionary.ContainsKey(runtimeField.Name))
				{
					dictionary[runtimeField.Name] = runtimeField;
				}
			}
			source = dictionary;
		}
		else
		{
			source = conventionEntityType.GetRuntimeFields();
		}
		KeyValuePair<string, FieldInfo>[] array = source.OrderBy<KeyValuePair<string, FieldInfo>, string>((KeyValuePair<string, FieldInfo> p) => p.Key, StringComparer.Ordinal).ToArray();
		FieldInfo fieldInfo = TryMatch(array, "<", name, ">k__BackingField", null, null, entityClrType, name);
		if (fieldInfo == null)
		{
			fieldInfo = TryMatch(array, name, "", "", propertyBase, null, entityClrType, name);
			string text = char.ToLowerInvariant(name[0]).ToString();
			string text2 = name.Substring(1);
			fieldInfo = TryMatch(array, text, text2, "", propertyBase, fieldInfo, entityClrType, name);
			fieldInfo = TryMatch(array, "_", text, text2, propertyBase, fieldInfo, entityClrType, name);
			fieldInfo = TryMatch(array, "_", "", name, propertyBase, fieldInfo, entityClrType, name);
			fieldInfo = TryMatch(array, "m_", text, text2, propertyBase, fieldInfo, entityClrType, name);
			fieldInfo = TryMatch(array, "m_", "", name, propertyBase, fieldInfo, entityClrType, name);
		}
		return fieldInfo;
	}

	private static FieldInfo TryMatch(KeyValuePair<string, FieldInfo>[] array, string prefix, string middle, string suffix, IConventionPropertyBase propertyBase, FieldInfo existingMatch, Type entityClrType, string propertyName)
	{
		int num = PrefixBinarySearch(array, prefix, 0, array.Length - 1);
		if (num == -1)
		{
			return existingMatch;
		}
		TypeInfo typeInfo = propertyBase?.ClrType.GetTypeInfo();
		int num2 = prefix.Length + middle.Length + suffix.Length;
		KeyValuePair<string, FieldInfo> keyValuePair = array[num];
		do
		{
			if (keyValuePair.Key.Length == num2 && keyValuePair.Key.EndsWith(suffix, StringComparison.Ordinal) && keyValuePair.Key.IndexOf(middle, prefix.Length, StringComparison.Ordinal) == prefix.Length)
			{
				FieldInfo fieldInfo = ((typeInfo == null) ? keyValuePair.Value : (IsConvertible(typeInfo, keyValuePair.Value) ? keyValuePair.Value : null));
				if (fieldInfo != null)
				{
					if (existingMatch != null && fieldInfo != existingMatch)
					{
						propertyBase.SetOrRemoveAnnotation("BackingFieldConvention:AmbiguousField", CoreStrings.ConflictingBackingFields(propertyName, entityClrType.ShortDisplayName(), existingMatch.Name, fieldInfo.Name));
						return null;
					}
					return fieldInfo;
				}
				return existingMatch;
			}
			if (++num == array.Length)
			{
				return existingMatch;
			}
			keyValuePair = array[num];
		}
		while (keyValuePair.Key.StartsWith(prefix, StringComparison.Ordinal));
		return existingMatch;
	}

	private static int PrefixBinarySearch<T>(KeyValuePair<string, T>[] array, string prefix, int left, int right)
	{
		int result = -1;
		while (right >= left)
		{
			int num = left + right >> 1;
			string key = array[num].Key;
			if (key.StartsWith(prefix, StringComparison.Ordinal))
			{
				result = num;
			}
			else if (StringComparer.Ordinal.Compare(key, prefix) < 0)
			{
				left = num + 1;
				continue;
			}
			right = num - 1;
		}
		return result;
	}

	private static bool IsConvertible(TypeInfo typeInfo, FieldInfo fieldInfo)
	{
		TypeInfo typeInfo2 = fieldInfo.FieldType.GetTypeInfo();
		if (!typeInfo.IsAssignableFrom(typeInfo2))
		{
			return typeInfo2.IsAssignableFrom(typeInfo);
		}
		return true;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
			{
				IConventionAnnotation conventionAnnotation = declaredProperty.FindAnnotation("BackingFieldConvention:AmbiguousField");
				if (conventionAnnotation != null)
				{
					if (declaredProperty.GetFieldName() == null)
					{
						throw new InvalidOperationException((string)conventionAnnotation.Value);
					}
					declaredProperty.RemoveAnnotation("BackingFieldConvention:AmbiguousField");
				}
			}
		}
	}
}
