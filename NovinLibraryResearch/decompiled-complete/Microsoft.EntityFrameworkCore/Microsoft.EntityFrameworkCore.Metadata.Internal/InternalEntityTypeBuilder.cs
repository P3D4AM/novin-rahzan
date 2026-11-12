using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalEntityTypeBuilder : InternalModelItemBuilder<EntityType>, IConventionEntityTypeBuilder, IConventionAnnotatableBuilder
{
	private static readonly string _defaultDiscriminatorName = "Discriminator";

	private static readonly Type _defaultDiscriminatorType = typeof(string);

	IConventionEntityType IConventionEntityTypeBuilder.Metadata
	{
		[DebuggerStepThrough]
		get
		{
			return Metadata;
		}
	}

	public InternalEntityTypeBuilder([JetBrains.Annotations.NotNull] EntityType metadata, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(metadata, modelBuilder)
	{
	}

	public virtual InternalKeyBuilder PrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, ConfigurationSource configurationSource)
	{
		return PrimaryKey(GetOrCreateProperties(propertyNames, configurationSource, null, required: true), configurationSource);
	}

	public virtual InternalKeyBuilder PrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<MemberInfo> clrMembers, ConfigurationSource configurationSource)
	{
		return PrimaryKey(GetOrCreateProperties(clrMembers, configurationSource), configurationSource);
	}

	public virtual InternalKeyBuilder PrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		if (!CanSetPrimaryKey(properties, configurationSource))
		{
			return null;
		}
		InternalKeyBuilder internalKeyBuilder = null;
		if (properties == null)
		{
			Metadata.SetPrimaryKey(properties, configurationSource);
		}
		else
		{
			Key key = Metadata.FindPrimaryKey();
			if (key != null && PropertyListComparer.Instance.Compare(key.Properties, properties) == 0)
			{
				key.UpdateConfigurationSource(configurationSource);
				return Metadata.SetPrimaryKey(properties, configurationSource).Builder;
			}
			using (ModelBuilder.Metadata.ConventionDispatcher.DelayConventions())
			{
				internalKeyBuilder = HasKeyInternal(properties, configurationSource);
				if (internalKeyBuilder == null)
				{
					return null;
				}
				Metadata.SetPrimaryKey(internalKeyBuilder.Metadata.Properties, configurationSource);
				foreach (Key item in Metadata.GetDeclaredKeys().ToList())
				{
					if (item == internalKeyBuilder.Metadata)
					{
						continue;
					}
					foreach (ForeignKey item2 in (from fk in item.GetReferencingForeignKeys()
						where !fk.GetPrincipalKeyConfigurationSource().HasValue
						select fk).ToList())
					{
						DetachRelationship(item2).Attach();
					}
				}
				if (key?.Builder != null)
				{
					RemoveKeyIfUnused(key, configurationSource);
				}
			}
		}
		if (internalKeyBuilder?.Metadata.Builder == null)
		{
			properties = GetActualProperties(properties, null);
			if (properties != null)
			{
				return Metadata.FindPrimaryKey(properties).Builder;
			}
			return null;
		}
		return internalKeyBuilder;
	}

	public virtual bool CanSetPrimaryKey(IReadOnlyList<IConventionProperty> properties, ConfigurationSource configurationSource)
	{
		Key key = Metadata.FindPrimaryKey();
		if (properties == null)
		{
			if (key == null)
			{
				return true;
			}
		}
		else if (key != null && PropertyListComparer.Instance.Compare(key.Properties, properties) == 0)
		{
			return true;
		}
		return configurationSource.Overrides(Metadata.GetPrimaryKeyConfigurationSource());
	}

	public virtual InternalKeyBuilder HasKey([JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, ConfigurationSource configurationSource)
	{
		return HasKeyInternal(GetOrCreateProperties(propertyNames, configurationSource, null, required: true), configurationSource);
	}

	public virtual InternalKeyBuilder HasKey([JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> clrProperties, ConfigurationSource configurationSource)
	{
		return HasKeyInternal(GetOrCreateProperties(clrProperties, configurationSource), configurationSource);
	}

	public virtual InternalKeyBuilder HasKey([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, ConfigurationSource? configurationSource)
	{
		return HasKeyInternal(properties, configurationSource);
	}

	private InternalKeyBuilder HasKeyInternal(IReadOnlyList<Property> properties, ConfigurationSource? configurationSource)
	{
		if (properties == null)
		{
			return null;
		}
		IReadOnlyList<Property> actualProperties = GetActualProperties(properties, configurationSource);
		Key key = Metadata.FindDeclaredKey(actualProperties);
		if (key == null)
		{
			if (!configurationSource.HasValue)
			{
				return null;
			}
			if (Metadata.IsKeyless && !configurationSource.Overrides(Metadata.GetIsKeylessConfigurationSource()))
			{
				return null;
			}
			if (Metadata.GetIsKeylessConfigurationSource() != ConfigurationSource.Explicit)
			{
				Metadata.HasNoKey(false, configurationSource.Value);
			}
			List<ForeignKey> list = actualProperties.SelectMany((Property p) => from k in p.GetContainingForeignKeys()
				where k.DeclaringEntityType != Metadata
				select k).ToList();
			if (list.Any((ForeignKey fk) => !configurationSource.Overrides(fk.GetPropertiesConfigurationSource())))
			{
				return null;
			}
			if (configurationSource != ConfigurationSource.Explicit && actualProperties.Any((Property p) => !p.Builder.CanSetIsRequired(true, configurationSource)))
			{
				return null;
			}
			using (Metadata.Model.ConventionDispatcher.DelayConventions())
			{
				foreach (ForeignKey item in list)
				{
					if (item.GetPropertiesConfigurationSource() != ConfigurationSource.Explicit)
					{
						item.Builder.HasForeignKey((IReadOnlyList<Property>)null, configurationSource.Value);
					}
				}
				foreach (Property item2 in actualProperties)
				{
					item2.Builder.IsRequired(true, configurationSource.Value);
				}
				key = Metadata.AddKey(actualProperties, configurationSource.Value);
			}
			if (key.Builder == null)
			{
				key = Metadata.FindDeclaredKey(actualProperties);
			}
		}
		else if (configurationSource.HasValue)
		{
			key.UpdateConfigurationSource(configurationSource.Value);
			Metadata.HasNoKey(false, configurationSource.Value);
		}
		return key?.Builder;
	}

	public virtual InternalEntityTypeBuilder HasNoKey([JetBrains.Annotations.NotNull] Key key, ConfigurationSource configurationSource)
	{
		ConfigurationSource configurationSource2 = key.GetConfigurationSource();
		if (!configurationSource.Overrides(configurationSource2))
		{
			return null;
		}
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			List<RelationshipSnapshot> list = key.GetReferencingForeignKeys().ToList().Select(DetachRelationship)
				.ToList();
			Metadata.RemoveKey(key);
			foreach (RelationshipSnapshot item in list)
			{
				item.Attach();
			}
			RemoveUnusedShadowProperties(key.Properties);
			foreach (Property property in key.Properties)
			{
				if (!property.IsKey() && property.ClrType.IsNullableType() && !property.GetContainingForeignKeys().Any((ForeignKey fk) => fk.IsRequired))
				{
					property.Builder?.IsRequired(null, configurationSource);
				}
			}
			return this;
		}
	}

	public static List<(InternalKeyBuilder, ConfigurationSource?)> DetachKeys(IEnumerable<Key> keysToDetach)
	{
		List<Key> list = (keysToDetach as List<Key>) ?? keysToDetach.ToList();
		if (list.Count == 0)
		{
			return null;
		}
		List<(InternalKeyBuilder, ConfigurationSource?)> list2 = new List<(InternalKeyBuilder, ConfigurationSource?)>();
		foreach (Key item2 in list)
		{
			(InternalKeyBuilder, ConfigurationSource?) item = DetachKey(item2);
			list2.Add(item);
		}
		return list2;
	}

	private static (InternalKeyBuilder, ConfigurationSource?) DetachKey(Key keyToDetach)
	{
		InternalEntityTypeBuilder builder = keyToDetach.DeclaringEntityType.Builder;
		InternalKeyBuilder builder2 = keyToDetach.Builder;
		ConfigurationSource? item = (keyToDetach.IsPrimaryKey() ? keyToDetach.DeclaringEntityType.GetPrimaryKeyConfigurationSource() : null);
		if (builder == null)
		{
			keyToDetach.DeclaringEntityType.RemoveKey(keyToDetach.Properties);
		}
		else
		{
			builder.HasNoKey(keyToDetach, keyToDetach.GetConfigurationSource());
		}
		return (builder2, item);
	}

	public virtual InternalEntityTypeBuilder HasNoKey(ConfigurationSource configurationSource)
	{
		if (Metadata.IsKeyless)
		{
			Metadata.HasNoKey(true, configurationSource);
			return this;
		}
		if (!configurationSource.Overrides(Metadata.GetIsKeylessConfigurationSource()) || Metadata.GetReferencingForeignKeys().Any((ForeignKey fk) => !configurationSource.Overrides(fk.GetConfigurationSource())) || Metadata.GetForeignKeys().Any((ForeignKey fk) => !configurationSource.Overrides(fk.GetPrincipalToDependentConfigurationSource())))
		{
			return null;
		}
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			foreach (ForeignKey item in Metadata.GetReferencingForeignKeys().ToList())
			{
				item.DeclaringEntityType.Builder.HasNoRelationship(item, configurationSource);
			}
			foreach (ForeignKey foreignKey in Metadata.GetForeignKeys())
			{
				foreignKey.HasPrincipalToDependent((string)null, configurationSource);
			}
			foreach (Key item2 in Metadata.GetKeys().ToList())
			{
				if (item2.GetConfigurationSource() != 0)
				{
					HasNoKey(item2, configurationSource);
				}
			}
			Metadata.HasNoKey(true, configurationSource);
			return this;
		}
	}

	public virtual InternalPropertyBuilder Property([JetBrains.Annotations.CanBeNull] Type propertyType, [JetBrains.Annotations.NotNull] string propertyName, ConfigurationSource? configurationSource)
	{
		return Property(propertyType, propertyName, configurationSource, configurationSource);
	}

	public virtual InternalPropertyBuilder Property([JetBrains.Annotations.CanBeNull] Type propertyType, [JetBrains.Annotations.NotNull] string propertyName, ConfigurationSource? typeConfigurationSource, ConfigurationSource? configurationSource)
	{
		return Property(propertyType, propertyName, null, typeConfigurationSource, configurationSource);
	}

	public virtual InternalPropertyBuilder Property([JetBrains.Annotations.NotNull] string propertyName, ConfigurationSource? configurationSource)
	{
		return Property(null, propertyName, null, null, configurationSource);
	}

	public virtual InternalPropertyBuilder Property([JetBrains.Annotations.NotNull] MemberInfo memberInfo, ConfigurationSource? configurationSource)
	{
		return Property(memberInfo.GetMemberType(), memberInfo.GetSimpleMemberName(), memberInfo, configurationSource, configurationSource);
	}

	private InternalPropertyBuilder Property([JetBrains.Annotations.CanBeNull] Type propertyType, [JetBrains.Annotations.NotNull] string propertyName, [JetBrains.Annotations.CanBeNull] MemberInfo memberInfo, ConfigurationSource? typeConfigurationSource, ConfigurationSource? configurationSource)
	{
		List<Property> list = null;
		Property property = Metadata.FindProperty(propertyName);
		if (property != null)
		{
			if (property.DeclaringEntityType != Metadata)
			{
				if (!(memberInfo != null) || !(property.GetIdentifyingMemberInfo() == null))
				{
					if (!IsIgnored(propertyName, configurationSource))
					{
						Metadata.RemoveIgnored(propertyName);
					}
					return property.DeclaringEntityType.Builder.Property(property, propertyName, propertyType, memberInfo, typeConfigurationSource, configurationSource);
				}
				if (!configurationSource.Overrides(property.GetConfigurationSource()))
				{
					return null;
				}
				list = new List<Property> { property };
			}
		}
		else
		{
			if (IsIgnored(propertyName, configurationSource))
			{
				return null;
			}
			foreach (ServiceProperty item in Metadata.FindServicePropertiesInHierarchy(propertyName))
			{
				if (!configurationSource.Overrides(item.GetConfigurationSource()))
				{
					return null;
				}
			}
			foreach (Navigation item2 in Metadata.FindNavigationsInHierarchy(propertyName))
			{
				ForeignKey foreignKey = item2.ForeignKey;
				ConfigurationSource? configurationSource2 = (item2.IsDependentToPrincipal() ? foreignKey.GetDependentToPrincipalConfigurationSource() : foreignKey.GetPrincipalToDependentConfigurationSource());
				if (!configurationSource.Overrides(configurationSource2))
				{
					return null;
				}
				if (configurationSource2 == ConfigurationSource.Explicit)
				{
					throw new InvalidOperationException(CoreStrings.PropertyCalledOnNavigation(propertyName, Metadata.DisplayName()));
				}
			}
			if (propertyType == null)
			{
				MemberInfo? obj = Metadata.ClrType?.GetMembersInHierarchy(propertyName).FirstOrDefault();
				if (obj == null)
				{
					throw new InvalidOperationException(CoreStrings.NoPropertyType(propertyName, Metadata.DisplayName()));
				}
				propertyType = obj.GetMemberType();
				typeConfigurationSource = ConfigurationSource.Explicit;
			}
			Metadata.RemoveIgnored(propertyName);
			foreach (EntityType derivedType in Metadata.GetDerivedTypes())
			{
				Property property2 = derivedType.FindDeclaredProperty(propertyName);
				if (property2 != null)
				{
					if (list == null)
					{
						list = new List<Property>();
					}
					list.Add(property2);
				}
			}
		}
		InternalPropertyBuilder internalPropertyBuilder;
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			PropertiesSnapshot obj2 = ((list == null) ? null : DetachProperties(list));
			internalPropertyBuilder = Property(property, propertyName, propertyType, memberInfo, typeConfigurationSource, configurationSource);
			obj2?.Attach(this);
		}
		if (internalPropertyBuilder == null || internalPropertyBuilder.Metadata.Builder != null)
		{
			return internalPropertyBuilder;
		}
		return Metadata.FindProperty(propertyName)?.Builder;
	}

	private InternalPropertyBuilder Property([JetBrains.Annotations.CanBeNull] Property existingProperty, [JetBrains.Annotations.NotNull] string propertyName, [JetBrains.Annotations.CanBeNull] Type propertyType, [JetBrains.Annotations.CanBeNull] MemberInfo clrProperty, ConfigurationSource? typeConfigurationSource, ConfigurationSource? configurationSource)
	{
		Property property;
		if (existingProperty == null)
		{
			if (!configurationSource.HasValue)
			{
				return null;
			}
			using (ModelBuilder.Metadata.ConventionDispatcher.DelayConventions())
			{
				foreach (ServiceProperty item in Metadata.FindServicePropertiesInHierarchy(propertyName))
				{
					Metadata.RemoveServiceProperty(item.Name);
				}
				foreach (Navigation item2 in Metadata.FindNavigationsInHierarchy(propertyName))
				{
					ForeignKey foreignKey = item2.ForeignKey;
					if (foreignKey.GetConfigurationSource() == ConfigurationSource.Convention)
					{
						foreignKey.DeclaringEntityType.Builder.HasNoRelationship(foreignKey, ConfigurationSource.Convention);
					}
					else if (foreignKey.Builder.HasNavigation((string)null, item2.IsDependentToPrincipal(), configurationSource.Value) == null)
					{
						return null;
					}
				}
				property = ((clrProperty != null) ? Metadata.AddProperty(clrProperty, configurationSource.Value) : Metadata.AddProperty(propertyName, propertyType, typeConfigurationSource, configurationSource.Value));
			}
		}
		else
		{
			if ((!(propertyType != null) || !(propertyType != existingProperty.ClrType)) && ((object)clrProperty == null || clrProperty.IsSameAs(existingProperty.GetIdentifyingMemberInfo())))
			{
				if (configurationSource.HasValue)
				{
					existingProperty.UpdateConfigurationSource(configurationSource.Value);
				}
				if (propertyType != null && typeConfigurationSource.HasValue)
				{
					existingProperty.UpdateTypeConfigurationSource(typeConfigurationSource.Value);
				}
				return existingProperty.Builder;
			}
			if (!configurationSource.HasValue || !configurationSource.Value.Overrides(existingProperty.GetConfigurationSource()))
			{
				return null;
			}
			using (Metadata.Model.ConventionDispatcher.DelayConventions())
			{
				PropertiesSnapshot propertiesSnapshot = DetachProperties(new Property[1] { existingProperty });
				property = ((clrProperty != null) ? Metadata.AddProperty(clrProperty, configurationSource.Value) : Metadata.AddProperty(propertyName, propertyType, typeConfigurationSource, configurationSource.Value));
				propertiesSnapshot.Attach(this);
			}
		}
		return property.Builder;
	}

	private bool CanRemoveProperty([JetBrains.Annotations.NotNull] Property property, ConfigurationSource configurationSource, bool canOverrideSameSource = true)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		ConfigurationSource configurationSource2 = property.GetConfigurationSource();
		if (configurationSource.Overrides(configurationSource2))
		{
			if (!canOverrideSameSource)
			{
				return configurationSource != configurationSource2;
			}
			return true;
		}
		return false;
	}

	public virtual bool CanAddProperty(Type propertyType, string name, ConfigurationSource? typeConfigurationSource)
	{
		Property property = Metadata.FindPropertiesInHierarchy(name).FirstOrDefault();
		if (property != null && property.IsShadowProperty() && property.ClrType != propertyType && typeConfigurationSource.HasValue && !typeConfigurationSource.Overrides(property.GetTypeConfigurationSource()))
		{
			return false;
		}
		if (Metadata.ClrType == null)
		{
			return true;
		}
		MemberInfo memberInfo = Metadata.ClrType.GetMembersInHierarchy(name).FirstOrDefault();
		if (memberInfo != null && propertyType != memberInfo.GetMemberType())
		{
			PropertyInfo obj = memberInfo as PropertyInfo;
			if (((object)obj == null || !obj.IsEFIndexerProperty()) && typeConfigurationSource.HasValue)
			{
				return false;
			}
		}
		return true;
	}

	public virtual InternalServicePropertyBuilder ServiceProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo, ConfigurationSource configurationSource)
	{
		string simpleMemberName = memberInfo.GetSimpleMemberName();
		if (IsIgnored(simpleMemberName, configurationSource))
		{
			return null;
		}
		Metadata.RemoveIgnored(simpleMemberName);
		ServiceProperty serviceProperty = Metadata.FindServiceProperty(simpleMemberName);
		if (serviceProperty != null)
		{
			if (serviceProperty.DeclaringEntityType != Metadata)
			{
				if (!configurationSource.Overrides(serviceProperty.GetConfigurationSource()))
				{
					return null;
				}
			}
			else
			{
				if (serviceProperty.GetIdentifyingMemberInfo() == memberInfo)
				{
					serviceProperty.UpdateConfigurationSource(configurationSource);
					return serviceProperty.Builder;
				}
				if (!configurationSource.Overrides(serviceProperty.GetConfigurationSource()))
				{
					return null;
				}
			}
		}
		else
		{
			foreach (ServiceProperty item in Metadata.FindDerivedServiceProperties(simpleMemberName))
			{
				if (!configurationSource.Overrides(item.GetConfigurationSource()))
				{
					return null;
				}
			}
			foreach (Property item2 in Metadata.FindPropertiesInHierarchy(simpleMemberName))
			{
				if (!configurationSource.Overrides(item2.GetConfigurationSource()))
				{
					return null;
				}
			}
			foreach (Navigation item3 in Metadata.FindNavigationsInHierarchy(simpleMemberName))
			{
				ForeignKey foreignKey = item3.ForeignKey;
				if (!configurationSource.Overrides(item3.IsDependentToPrincipal() ? foreignKey.GetDependentToPrincipalConfigurationSource() : foreignKey.GetPrincipalToDependentConfigurationSource()))
				{
					return null;
				}
			}
		}
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			using (ModelBuilder.Metadata.ConventionDispatcher.DelayConventions())
			{
				foreach (ServiceProperty item4 in Metadata.FindServicePropertiesInHierarchy(simpleMemberName))
				{
					Metadata.RemoveServiceProperty(item4.Name);
				}
				if (serviceProperty == null)
				{
					foreach (Property item5 in Metadata.FindPropertiesInHierarchy(simpleMemberName))
					{
						Metadata.RemoveProperty(item5.Name);
					}
					foreach (Navigation item6 in Metadata.FindNavigationsInHierarchy(simpleMemberName))
					{
						ForeignKey foreignKey2 = item6.ForeignKey;
						if (foreignKey2.GetConfigurationSource() == ConfigurationSource.Convention)
						{
							foreignKey2.DeclaringEntityType.Builder.HasNoRelationship(foreignKey2, ConfigurationSource.Convention);
						}
						else if (foreignKey2.Builder.HasNavigation((string)null, item6.IsDependentToPrincipal(), configurationSource) == null)
						{
							return null;
						}
					}
				}
				return Metadata.AddServiceProperty(memberInfo, configurationSource).Builder;
			}
		}
	}

	public virtual bool CanAddNavigation([JetBrains.Annotations.NotNull] string navigationName, ConfigurationSource configurationSource)
	{
		if (!IsIgnored(navigationName, configurationSource))
		{
			return Metadata.FindNavigationsInHierarchy(navigationName).All((Navigation n) => n.ForeignKey.Builder.CanSetNavigation((string)null, n.IsDependentToPrincipal(), (ConfigurationSource?)configurationSource));
		}
		return false;
	}

	public virtual bool IsIgnored([JetBrains.Annotations.NotNull] string name, ConfigurationSource? configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		ConfigurationSource? oldConfigurationSource = Metadata.FindIgnoredConfigurationSource(name);
		if (configurationSource.HasValue)
		{
			return !configurationSource.Value.Overrides(oldConfigurationSource);
		}
		return true;
	}

	public virtual bool CanRemoveForeignKey([JetBrains.Annotations.NotNull] ForeignKey foreignKey, ConfigurationSource configurationSource)
	{
		ConfigurationSource configurationSource2 = foreignKey.GetConfigurationSource();
		return configurationSource.Overrides(configurationSource2);
	}

	public virtual InternalEntityTypeBuilder Ignore([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		ConfigurationSource? configurationSource2 = Metadata.FindIgnoredConfigurationSource(name);
		if (configurationSource2.HasValue)
		{
			if (configurationSource2.Value.Overrides(configurationSource))
			{
				return this;
			}
		}
		else if (!CanIgnore(name, configurationSource, shouldThrow: true))
		{
			return null;
		}
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			Metadata.AddIgnored(name, configurationSource);
			Navigation navigation = Metadata.FindNavigation(name);
			if (navigation != null)
			{
				ForeignKey foreignKey = navigation.ForeignKey;
				bool flag = navigation.IsDependentToPrincipal();
				ConfigurationSource? obj = (flag ? foreignKey.GetDependentToPrincipalConfigurationSource() : foreignKey.GetPrincipalToDependentConfigurationSource());
				if (foreignKey.GetConfigurationSource() != obj)
				{
					foreignKey.Builder.HasNavigation((MemberInfo)null, flag, configurationSource);
				}
				else
				{
					foreignKey.DeclaringEntityType.Builder.HasNoRelationship(foreignKey, configurationSource);
				}
			}
			else
			{
				Property property = Metadata.FindProperty(name);
				if (property != null)
				{
					property.DeclaringEntityType.Builder.RemoveProperty(property, configurationSource);
				}
				else
				{
					Metadata.FindServiceProperty(name)?.DeclaringEntityType.RemoveServiceProperty(name);
				}
			}
			foreach (EntityType derivedType in Metadata.GetDerivedTypes())
			{
				ConfigurationSource? oldConfigurationSource = derivedType.FindDeclaredIgnoredConfigurationSource(name);
				if (oldConfigurationSource.HasValue)
				{
					if (configurationSource.Overrides(oldConfigurationSource))
					{
						derivedType.RemoveIgnored(name);
					}
					continue;
				}
				Navigation navigation2 = derivedType.FindDeclaredNavigation(name);
				if (navigation2 != null)
				{
					ForeignKey foreignKey2 = navigation2.ForeignKey;
					if (configurationSource != foreignKey2.GetConfigurationSource())
					{
						foreignKey2.DeclaringEntityType.Builder.HasNoRelationship(foreignKey2, configurationSource);
					}
					continue;
				}
				Property property2 = derivedType.FindDeclaredProperty(name);
				if (property2 != null)
				{
					if (configurationSource != property2.GetConfigurationSource())
					{
						derivedType.Builder.RemoveProperty(property2, configurationSource);
					}
					continue;
				}
				ServiceProperty serviceProperty = derivedType.FindServiceProperty(name);
				if (serviceProperty != null && configurationSource.OverridesStrictly(serviceProperty.GetConfigurationSource()))
				{
					serviceProperty.DeclaringEntityType.RemoveServiceProperty(name);
				}
			}
			return this;
		}
	}

	public virtual bool CanIgnore(string name, ConfigurationSource configurationSource)
	{
		return CanIgnore(name, configurationSource, shouldThrow: false);
	}

	private bool CanIgnore(string name, ConfigurationSource configurationSource, bool shouldThrow)
	{
		if (Metadata.FindIgnoredConfigurationSource(name).HasValue)
		{
			return true;
		}
		Navigation navigation = Metadata.FindNavigation(name);
		if (navigation != null)
		{
			ForeignKey foreignKey = navigation.ForeignKey;
			if (navigation.DeclaringEntityType != Metadata)
			{
				if (shouldThrow)
				{
					throw new InvalidOperationException(CoreStrings.InheritedPropertyCannotBeIgnored(name, Metadata.DisplayName(), navigation.DeclaringEntityType.DisplayName()));
				}
				return false;
			}
			ConfigurationSource? configurationSource2 = (navigation.IsDependentToPrincipal() ? foreignKey.GetDependentToPrincipalConfigurationSource() : foreignKey.GetPrincipalToDependentConfigurationSource());
			if (foreignKey.GetConfigurationSource() != configurationSource2)
			{
				if (!configurationSource.Overrides(configurationSource2))
				{
					return false;
				}
			}
			else if (configurationSource != 0 && !configurationSource.OverridesStrictly(foreignKey.GetConfigurationSource()))
			{
				return false;
			}
		}
		else
		{
			Property property = Metadata.FindProperty(name);
			if (property != null)
			{
				if (property.DeclaringEntityType != Metadata)
				{
					if (shouldThrow)
					{
						throw new InvalidOperationException(CoreStrings.InheritedPropertyCannotBeIgnored(name, Metadata.DisplayName(), property.DeclaringEntityType.DisplayName()));
					}
					return false;
				}
				if (!property.DeclaringEntityType.Builder.CanRemoveProperty(property, configurationSource, configurationSource == ConfigurationSource.Explicit))
				{
					return false;
				}
			}
			else
			{
				ServiceProperty serviceProperty = Metadata.FindServiceProperty(name);
				if (serviceProperty != null)
				{
					if (serviceProperty.DeclaringEntityType != Metadata)
					{
						if (shouldThrow)
						{
							throw new InvalidOperationException(CoreStrings.InheritedPropertyCannotBeIgnored(name, Metadata.DisplayName(), serviceProperty.DeclaringEntityType.DisplayName()));
						}
						return false;
					}
					if (configurationSource != 0 && !configurationSource.OverridesStrictly(serviceProperty.GetConfigurationSource()))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public virtual InternalEntityTypeBuilder HasQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression filter, ConfigurationSource configurationSource)
	{
		if (CanSetQueryFilter(filter, configurationSource))
		{
			Metadata.SetQueryFilter(filter, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression filter, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetQueryFilterConfigurationSource()))
		{
			return Metadata.GetQueryFilter() == filter;
		}
		return true;
	}

	public virtual InternalEntityTypeBuilder HasDefiningQuery([JetBrains.Annotations.CanBeNull] LambdaExpression query, ConfigurationSource configurationSource)
	{
		if (CanSetDefiningQuery(query, configurationSource))
		{
			Metadata.SetDefiningQuery(query, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetDefiningQuery(LambdaExpression query, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetDefiningQueryConfigurationSource()))
		{
			return Metadata.GetDefiningQuery() == query;
		}
		return true;
	}

	public virtual InternalEntityTypeBuilder HasBaseType([JetBrains.Annotations.CanBeNull] Type baseEntityType, ConfigurationSource configurationSource)
	{
		if (baseEntityType == null)
		{
			return HasBaseType((EntityType)null, configurationSource);
		}
		InternalEntityTypeBuilder internalEntityTypeBuilder = ModelBuilder.Entity(baseEntityType, configurationSource, false);
		if (internalEntityTypeBuilder != null)
		{
			return HasBaseType(internalEntityTypeBuilder.Metadata, configurationSource);
		}
		return null;
	}

	public virtual InternalEntityTypeBuilder HasBaseType([JetBrains.Annotations.CanBeNull] string baseEntityTypeName, ConfigurationSource configurationSource)
	{
		if (baseEntityTypeName == null)
		{
			return HasBaseType((EntityType)null, configurationSource);
		}
		InternalEntityTypeBuilder internalEntityTypeBuilder = ModelBuilder.Entity(baseEntityTypeName, configurationSource, false);
		if (internalEntityTypeBuilder != null)
		{
			return HasBaseType(internalEntityTypeBuilder.Metadata, configurationSource);
		}
		return null;
	}

	public virtual InternalEntityTypeBuilder HasBaseType([JetBrains.Annotations.CanBeNull] EntityType baseEntityType, ConfigurationSource configurationSource)
	{
		if (Metadata.BaseType == baseEntityType)
		{
			Metadata.HasBaseType(baseEntityType, configurationSource);
			return this;
		}
		if (!configurationSource.Overrides(Metadata.GetBaseTypeConfigurationSource()))
		{
			return null;
		}
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			List<RelationshipSnapshot> list = null;
			PropertiesSnapshot propertiesSnapshot = null;
			IReadOnlyList<(InternalKeyBuilder, ConfigurationSource?)> readOnlyList = null;
			ConfigurationSource configurationSourceForRemoval = ConfigurationSource.DataAnnotation.Max(configurationSource);
			if (baseEntityType != null)
			{
				if (Metadata.GetDeclaredKeys().Any((Key k) => !configurationSourceForRemoval.Overrides(k.GetConfigurationSource())) || (Metadata.IsKeyless && !configurationSource.Overrides(Metadata.GetIsKeylessConfigurationSource())))
				{
					return null;
				}
				IEnumerable<ForeignKey> enumerable = FindConflictingRelationships(baseEntityType, configurationSourceForRemoval);
				if (enumerable == null)
				{
					return null;
				}
				List<ForeignKey> list2 = (from fk in Metadata.GetDerivedTypesInclusive().SelectMany((EntityType t) => t.GetDeclaredForeignKeys())
					where fk.Properties.Any((Property p) => baseEntityType.FindProperty(p.Name)?.IsKey() ?? false)
					select fk).ToList();
				if (list2.Any((ForeignKey fk) => !configurationSourceForRemoval.Overrides(fk.GetPropertiesConfigurationSource())))
				{
					return null;
				}
				foreach (ForeignKey item in list2)
				{
					item.Builder.HasForeignKey((IReadOnlyList<Property>)null, configurationSourceForRemoval);
				}
				foreach (ForeignKey item2 in enumerable)
				{
					if (list == null)
					{
						list = new List<RelationshipSnapshot>();
					}
					list.Add(DetachRelationship(item2));
				}
				foreach (Key item3 in Metadata.GetDeclaredKeys().ToList())
				{
					foreach (ForeignKey item4 in item3.GetReferencingForeignKeys().ToList())
					{
						if (list == null)
						{
							list = new List<RelationshipSnapshot>();
						}
						list.Add(DetachRelationship(item4));
					}
				}
				readOnlyList = DetachKeys(Metadata.GetDeclaredKeys());
				propertiesSnapshot = DetachProperties(from p in baseEntityType.GetProperties().SelectMany((Property p) => Metadata.FindDerivedPropertiesInclusive(p.Name))
					where p != null
					select p);
				foreach (Property item5 in (from p in Metadata.GetDerivedTypesInclusive().SelectMany((EntityType et) => et.GetDeclaredProperties())
					where !p.GetConfigurationSource().Overrides(baseEntityType.FindIgnoredConfigurationSource(p.Name))
					select p).ToList())
				{
					item5.DeclaringEntityType.Builder.RemoveProperty(item5, ConfigurationSource.Explicit);
				}
				foreach (ServiceProperty item6 in (from p in baseEntityType.GetServiceProperties().SelectMany((ServiceProperty p) => Metadata.FindDerivedServicePropertiesInclusive(p.Name))
					where p != null
					select p).ToList())
				{
					item6.DeclaringEntityType.RemoveServiceProperty(item6.Name);
				}
				foreach (string ignoredMember in Metadata.GetIgnoredMembers())
				{
					ConfigurationSource? configurationSource2 = baseEntityType.FindIgnoredConfigurationSource(ignoredMember);
					if (configurationSource2.HasValue && configurationSource2.Value.Overrides(Metadata.FindDeclaredIgnoredConfigurationSource(ignoredMember)))
					{
						Metadata.RemoveIgnored(ignoredMember);
					}
				}
				Metadata.HasNoKey(false, configurationSource);
				baseEntityType.UpdateConfigurationSource(configurationSource);
			}
			List<InternalIndexBuilder> list3 = null;
			HashSet<Property> hashSet = null;
			List<(string, ConfigurationSource)> list4 = null;
			if (Metadata.BaseType != null)
			{
				HashSet<Property> removedInheritedProperties = new HashSet<Property>(from p in Metadata.BaseType.GetProperties()
					where baseEntityType == null || baseEntityType.FindProperty(p.Name) != p
					select p);
				if (removedInheritedProperties.Count != 0)
				{
					hashSet = new HashSet<Property>();
					foreach (ForeignKey item7 in (from fk in Metadata.GetDerivedTypesInclusive().SelectMany((EntityType t) => t.GetDeclaredForeignKeys())
						where fk.Properties.Any((Property p) => removedInheritedProperties.Contains(p))
						select fk).ToList())
					{
						foreach (Property property in item7.Properties)
						{
							if (removedInheritedProperties.Contains(property))
							{
								hashSet.Add(property);
							}
						}
						if (list == null)
						{
							list = new List<RelationshipSnapshot>();
						}
						list.Add(DetachRelationship(item7));
					}
					foreach (Index item8 in (from i in Metadata.GetDerivedTypesInclusive().SelectMany((EntityType e) => e.GetDeclaredIndexes())
						where i.Properties.Any((Property p) => removedInheritedProperties.Contains(p))
						select i).ToList())
					{
						foreach (Property property2 in item8.Properties)
						{
							if (removedInheritedProperties.Contains(property2))
							{
								hashSet.Add(property2);
							}
						}
						if (list3 == null)
						{
							list3 = new List<InternalIndexBuilder>();
						}
						list3.Add(DetachIndex(item8));
					}
					foreach (Key item9 in (from k in Metadata.GetKeys()
						where k.ReferencingForeignKeys != null && k.Properties.Any((Property p) => removedInheritedProperties.Contains(p))
						select k).ToList())
					{
						foreach (ForeignKey item10 in item9.ReferencingForeignKeys.ToList())
						{
							if (Metadata.IsAssignableFrom(item10.PrincipalEntityType))
							{
								if (list == null)
								{
									list = new List<RelationshipSnapshot>();
								}
								list.Add(DetachRelationship(item10));
							}
						}
					}
				}
				foreach (string ignoredMember2 in Metadata.BaseType.GetIgnoredMembers())
				{
					if (baseEntityType == null || (baseEntityType.FindProperty(ignoredMember2) == null && baseEntityType.FindNavigation(ignoredMember2) == null))
					{
						if (list4 == null)
						{
							list4 = new List<(string, ConfigurationSource)>();
						}
						list4.Add((ignoredMember2, Metadata.BaseType.FindDeclaredIgnoredConfigurationSource(ignoredMember2).Value));
					}
				}
			}
			Metadata.HasBaseType(baseEntityType, configurationSource);
			if (list4 != null)
			{
				foreach (var item11 in list4)
				{
					Ignore(item11.Item1, item11.Item2);
					Metadata.RemoveIgnored(item11.Item1);
				}
			}
			if (hashSet != null)
			{
				foreach (Property item12 in hashSet)
				{
					item12.Builder?.Attach(this);
				}
			}
			propertiesSnapshot?.Attach(this);
			if (readOnlyList != null)
			{
				foreach (var item13 in readOnlyList)
				{
					item13.Item1.Attach(Metadata.RootType().Builder, item13.Item2);
				}
			}
			if (list3 != null)
			{
				foreach (InternalIndexBuilder item14 in list3)
				{
					item14.Attach(item14.Metadata.DeclaringEntityType.Builder);
				}
			}
			if (list != null)
			{
				foreach (RelationshipSnapshot item15 in list)
				{
					item15.Attach();
				}
			}
		}
		return this;
	}

	public virtual bool CanSetBaseType(EntityType baseEntityType, ConfigurationSource configurationSource)
	{
		if (Metadata.BaseType != baseEntityType)
		{
			return configurationSource.Overrides(Metadata.GetBaseTypeConfigurationSource());
		}
		return true;
	}

	internal static PropertiesSnapshot DetachProperties(IEnumerable<Property> propertiesToDetach)
	{
		List<Property> list = propertiesToDetach.ToList();
		if (list.Count == 0)
		{
			return null;
		}
		List<RelationshipSnapshot> list2 = null;
		foreach (Property item in list)
		{
			foreach (ForeignKey item2 in item.GetContainingForeignKeys().ToList())
			{
				if (list2 == null)
				{
					list2 = new List<RelationshipSnapshot>();
				}
				list2.Add(DetachRelationship(item2));
			}
		}
		List<InternalIndexBuilder> indexes = DetachIndexes(list.SelectMany((Property p) => p.GetContainingIndexes()).Distinct());
		List<Key> list3 = list.SelectMany((Property p) => p.GetContainingKeys()).Distinct().ToList();
		foreach (Key item3 in list3)
		{
			foreach (ForeignKey item4 in item3.GetReferencingForeignKeys().ToList())
			{
				if (list2 == null)
				{
					list2 = new List<RelationshipSnapshot>();
				}
				list2.Add(DetachRelationship(item4));
			}
		}
		List<(InternalKeyBuilder, ConfigurationSource?)> keys = DetachKeys(list3);
		List<InternalPropertyBuilder> list4 = new List<InternalPropertyBuilder>();
		foreach (Property item5 in list)
		{
			Property property = item5.DeclaringEntityType.FindDeclaredProperty(item5.Name);
			if (property != null)
			{
				InternalEntityTypeBuilder builder = property.DeclaringEntityType.Builder;
				InternalPropertyBuilder builder2 = property.Builder;
				builder2.ValueGenerated(null, ConfigurationSource.Convention);
				builder2.AfterSave(null, ConfigurationSource.Convention);
				builder2.BeforeSave(null, ConfigurationSource.Convention);
				if (builder != null)
				{
					builder.RemoveProperty(property, property.GetConfigurationSource());
				}
				else
				{
					property.GetConfigurationSource();
					property.DeclaringEntityType.RemoveProperty(property.Name);
				}
				list4.Add(builder2);
			}
		}
		return new PropertiesSnapshot(list4, indexes, keys, list2);
	}

	private IEnumerable<ForeignKey> FindConflictingRelationships(EntityType baseEntityType, ConfigurationSource configurationSource)
	{
		HashSet<ForeignKey> hashSet = new HashSet<ForeignKey>();
		foreach (Navigation item in Metadata.GetDerivedNavigationsInclusive())
		{
			if (!item.ForeignKey.GetConfigurationSource().Overrides(baseEntityType.FindIgnoredConfigurationSource(item.Name)))
			{
				hashSet.Add(item.ForeignKey);
				continue;
			}
			Navigation navigation = baseEntityType.FindNavigation(item.Name);
			if (navigation == null)
			{
				continue;
			}
			if (item.ForeignKey.DeclaringEntityType.Builder.CanRemoveForeignKey(item.ForeignKey, configurationSource))
			{
				hashSet.Add(navigation.ForeignKey);
				continue;
			}
			if (navigation.ForeignKey.DeclaringEntityType.Builder.CanRemoveForeignKey(navigation.ForeignKey, configurationSource))
			{
				hashSet.Add(item.ForeignKey);
				continue;
			}
			return null;
		}
		return hashSet;
	}

	private ConfigurationSource? RemoveProperty(Property property, ConfigurationSource configurationSource, bool canOverrideSameSource = true)
	{
		ConfigurationSource configurationSource2 = property.GetConfigurationSource();
		if (!configurationSource.Overrides(configurationSource2) || (!canOverrideSameSource && configurationSource == configurationSource2))
		{
			return null;
		}
		using (Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			List<RelationshipSnapshot> list = property.GetContainingForeignKeys().ToList().Select(DetachRelationship)
				.ToList();
			foreach (Key item in property.GetContainingKeys().ToList())
			{
				list.AddRange(item.GetReferencingForeignKeys().ToList().Select(DetachRelationship));
				item.DeclaringEntityType.Builder.HasNoKey(item, configurationSource);
			}
			foreach (Index item2 in property.GetContainingIndexes().ToList())
			{
				item2.DeclaringEntityType.Builder.HasNoIndex(item2, configurationSource);
			}
			if (property.Builder != null)
			{
				Metadata.RemoveProperty(property.Name);
			}
			foreach (RelationshipSnapshot item3 in list)
			{
				item3.Attach();
			}
		}
		return configurationSource2;
	}

	public static RelationshipSnapshot DetachRelationship([JetBrains.Annotations.NotNull] ForeignKey foreignKey)
	{
		return DetachRelationship(foreignKey, includeDefinedType: false);
	}

	public static RelationshipSnapshot DetachRelationship([JetBrains.Annotations.NotNull] ForeignKey foreignKey, bool includeDefinedType)
	{
		InternalRelationshipBuilder builder = foreignKey.Builder;
		foreignKey.DeclaringEntityType.Builder.HasNoRelationship(foreignKey, foreignKey.GetConfigurationSource());
		EntityType.Snapshot definedEntityTypeSnapshot = null;
		if (includeDefinedType)
		{
			EntityType declaringEntityType = foreignKey.DeclaringEntityType;
			if (declaringEntityType.DefiningEntityType == foreignKey.PrincipalEntityType && declaringEntityType.DefiningNavigationName == foreignKey.PrincipalToDependent?.Name)
			{
				definedEntityTypeSnapshot = DetachAllMembers(declaringEntityType);
				declaringEntityType.Model.Builder.HasNoEntityType(declaringEntityType, ConfigurationSource.Explicit);
			}
		}
		return new RelationshipSnapshot(builder, definedEntityTypeSnapshot);
	}

	public virtual InternalEntityTypeBuilder HasNoRelationship([JetBrains.Annotations.NotNull] ForeignKey foreignKey, ConfigurationSource configurationSource)
	{
		ConfigurationSource configurationSource2 = foreignKey.GetConfigurationSource();
		if (!configurationSource.Overrides(configurationSource2))
		{
			return null;
		}
		Metadata.RemoveForeignKey(foreignKey);
		RemoveUnusedShadowProperties(foreignKey.Properties);
		foreignKey.PrincipalKey.DeclaringEntityType.Builder?.RemoveKeyIfUnused(foreignKey.PrincipalKey);
		return this;
	}

	public static EntityType.Snapshot DetachAllMembers([JetBrains.Annotations.NotNull] EntityType entityType)
	{
		if (entityType.Builder == null)
		{
			return null;
		}
		if (entityType.HasDefiningNavigation())
		{
			entityType.Model.AddDetachedEntityType(entityType.Name, entityType.DefiningNavigationName, entityType.DefiningEntityType.Name);
		}
		List<RelationshipSnapshot> list = null;
		foreach (ForeignKey item2 in entityType.GetDeclaredForeignKeys().ToList())
		{
			if (list == null)
			{
				list = new List<RelationshipSnapshot>();
			}
			RelationshipSnapshot relationshipSnapshot = DetachRelationship(item2);
			if (relationshipSnapshot.Relationship.Metadata.GetConfigurationSource().Overrides(ConfigurationSource.DataAnnotation) || item2.IsOwnership)
			{
				list.Add(relationshipSnapshot);
			}
		}
		List<(InternalKeyBuilder, ConfigurationSource?)> list2 = null;
		foreach (Key item3 in entityType.GetDeclaredKeys().ToList())
		{
			foreach (ForeignKey item4 in item3.GetReferencingForeignKeys().ToList())
			{
				if (list == null)
				{
					list = new List<RelationshipSnapshot>();
				}
				RelationshipSnapshot relationshipSnapshot2 = DetachRelationship(item4, includeDefinedType: true);
				if (relationshipSnapshot2.Relationship.Metadata.GetConfigurationSource().Overrides(ConfigurationSource.DataAnnotation) || item4.IsOwnership)
				{
					list.Add(relationshipSnapshot2);
				}
			}
			if (item3.Builder != null)
			{
				if (list2 == null)
				{
					list2 = new List<(InternalKeyBuilder, ConfigurationSource?)>();
				}
				(InternalKeyBuilder, ConfigurationSource?) item = DetachKey(item3);
				if (item.Item1.Metadata.GetConfigurationSource().Overrides(ConfigurationSource.Explicit))
				{
					list2.Add(item);
				}
			}
		}
		List<InternalIndexBuilder> list3 = null;
		foreach (Index item5 in entityType.GetDeclaredIndexes().ToList())
		{
			if (list3 == null)
			{
				list3 = new List<InternalIndexBuilder>();
			}
			InternalIndexBuilder internalIndexBuilder = DetachIndex(item5);
			if (internalIndexBuilder.Metadata.GetConfigurationSource().Overrides(ConfigurationSource.Explicit))
			{
				list3.Add(internalIndexBuilder);
			}
		}
		PropertiesSnapshot properties = DetachProperties(entityType.GetDeclaredProperties());
		return new EntityType.Snapshot(entityType, properties, list3, list2, list);
	}

	private void RemoveKeyIfUnused(Key key, ConfigurationSource configurationSource = ConfigurationSource.Convention)
	{
		if (Metadata.FindPrimaryKey() != key && !key.GetReferencingForeignKeys().Any())
		{
			HasNoKey(key, configurationSource);
		}
	}

	public virtual InternalEntityTypeBuilder RemoveUnusedShadowProperties<T>([JetBrains.Annotations.NotNull] IReadOnlyList<T> properties, ConfigurationSource configurationSource = ConfigurationSource.Convention) where T : class, IProperty
	{
		foreach (T property in properties)
		{
			if (property != null && property.IsShadowProperty())
			{
				RemovePropertyIfUnused((Property)(object)property, configurationSource);
			}
		}
		return this;
	}

	private static void RemovePropertyIfUnused(Property property, ConfigurationSource configurationSource)
	{
		if (property.Builder != null && property.DeclaringEntityType.Builder.CanRemoveProperty(property, configurationSource) && !property.GetContainingIndexes().Any() && !property.GetContainingForeignKeys().Any() && !property.GetContainingKeys().Any())
		{
			property.DeclaringEntityType.RemoveProperty(property.Name);
		}
	}

	public virtual InternalIndexBuilder HasIndex([JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, ConfigurationSource configurationSource)
	{
		return HasIndex(GetOrCreateProperties(propertyNames, configurationSource), configurationSource);
	}

	public virtual InternalIndexBuilder HasIndex([JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> clrMembers, ConfigurationSource configurationSource)
	{
		return HasIndex(GetOrCreateProperties(clrMembers, configurationSource), configurationSource);
	}

	public virtual InternalIndexBuilder HasIndex([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		if (properties == null)
		{
			return null;
		}
		List<InternalIndexBuilder> list = null;
		Index index = Metadata.FindIndex(properties);
		if (index == null)
		{
			list = Metadata.FindDerivedIndexes(properties).ToList().Select(DetachIndex)
				.ToList();
		}
		else if (index.DeclaringEntityType != Metadata)
		{
			return index.DeclaringEntityType.Builder.HasIndex(index, properties, configurationSource);
		}
		InternalIndexBuilder result = HasIndex(index, properties, configurationSource);
		if (list != null)
		{
			foreach (InternalIndexBuilder item in list)
			{
				item.Attach(item.Metadata.DeclaringEntityType.Builder);
			}
		}
		return result;
	}

	private InternalIndexBuilder HasIndex(Index index, IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		if (index == null)
		{
			index = Metadata.AddIndex(properties, configurationSource);
		}
		else
		{
			index.UpdateConfigurationSource(configurationSource);
		}
		return index?.Builder;
	}

	public virtual InternalEntityTypeBuilder HasNoIndex([JetBrains.Annotations.NotNull] Index index, ConfigurationSource configurationSource)
	{
		ConfigurationSource configurationSource2 = index.GetConfigurationSource();
		if (!configurationSource.Overrides(configurationSource2))
		{
			return null;
		}
		Metadata.RemoveIndex(index.Properties);
		RemoveUnusedShadowProperties(index.Properties);
		return this;
	}

	public static List<InternalIndexBuilder> DetachIndexes(IEnumerable<Index> indexesToDetach)
	{
		List<Index> list = (indexesToDetach as List<Index>) ?? indexesToDetach.ToList();
		if (list.Count == 0)
		{
			return null;
		}
		List<InternalIndexBuilder> list2 = new List<InternalIndexBuilder>();
		foreach (Index item2 in list)
		{
			InternalIndexBuilder item = DetachIndex(item2);
			list2.Add(item);
		}
		return list2;
	}

	private static InternalIndexBuilder DetachIndex(Index indexToDetach)
	{
		InternalEntityTypeBuilder builder = indexToDetach.DeclaringEntityType.Builder;
		InternalIndexBuilder builder2 = indexToDetach.Builder;
		builder.HasNoIndex(indexToDetach, indexToDetach.GetConfigurationSource());
		return builder2;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] string principalEntityTypeName, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(principalEntityTypeName, "principalEntityTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames");
		InternalEntityTypeBuilder internalEntityTypeBuilder = ModelBuilder.Entity(principalEntityTypeName, configurationSource, false);
		Key key = internalEntityTypeBuilder?.Metadata.FindPrimaryKey();
		if (internalEntityTypeBuilder != null)
		{
			return HasForeignKey(internalEntityTypeBuilder.Metadata, GetOrCreateProperties(propertyNames, configurationSource, key?.Properties, required: false, key == null), null, configurationSource);
		}
		return null;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] string principalEntityTypeName, [JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames, [JetBrains.Annotations.NotNull] Key principalKey, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(principalEntityTypeName, "principalEntityTypeName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyNames, "propertyNames");
		InternalEntityTypeBuilder internalEntityTypeBuilder = ModelBuilder.Entity(principalEntityTypeName, configurationSource, false);
		if (internalEntityTypeBuilder != null)
		{
			return HasForeignKey(internalEntityTypeBuilder.Metadata, GetOrCreateProperties(propertyNames, configurationSource, principalKey.Properties), principalKey, configurationSource);
		}
		return null;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] Type principalClrType, [JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> clrMembers, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalClrType, "principalClrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(clrMembers, "clrMembers");
		InternalEntityTypeBuilder internalEntityTypeBuilder = ModelBuilder.Entity(principalClrType, configurationSource, false);
		if (internalEntityTypeBuilder != null)
		{
			return HasForeignKey(internalEntityTypeBuilder.Metadata, GetOrCreateProperties(clrMembers, configurationSource), null, configurationSource);
		}
		return null;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] Type principalClrType, [JetBrains.Annotations.NotNull] IReadOnlyList<MemberInfo> clrMembers, [JetBrains.Annotations.NotNull] Key principalKey, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalClrType, "principalClrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(clrMembers, "clrMembers");
		InternalEntityTypeBuilder internalEntityTypeBuilder = ModelBuilder.Entity(principalClrType, configurationSource, false);
		if (internalEntityTypeBuilder != null)
		{
			return HasForeignKey(internalEntityTypeBuilder.Metadata, GetOrCreateProperties(clrMembers, configurationSource), principalKey, configurationSource);
		}
		return null;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] IReadOnlyList<Property> dependentProperties, ConfigurationSource configurationSource)
	{
		return HasForeignKey(principalEntityType, GetActualProperties(dependentProperties, configurationSource), null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] Key principalKey, ConfigurationSource configurationSource)
	{
		return HasForeignKey(principalEntityType, GetActualProperties(dependentProperties, configurationSource), principalKey, configurationSource);
	}

	private InternalRelationshipBuilder HasForeignKey(EntityType principalEntityType, IReadOnlyList<Property> dependentProperties, Key principalKey, ConfigurationSource configurationSource)
	{
		if (dependentProperties == null)
		{
			return null;
		}
		InternalRelationshipBuilder internalRelationshipBuilder = HasRelationshipInternal(principalEntityType, principalKey, configurationSource);
		InternalRelationshipBuilder internalRelationshipBuilder2 = internalRelationshipBuilder.HasForeignKey(dependentProperties, configurationSource);
		if (internalRelationshipBuilder2 == null && internalRelationshipBuilder.Metadata.Builder != null)
		{
			HasNoRelationship(internalRelationshipBuilder.Metadata, configurationSource);
		}
		return internalRelationshipBuilder2;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType targetEntityType, [JetBrains.Annotations.CanBeNull] string navigationToTargetName, [JetBrains.Annotations.CanBeNull] string inverseNavigationName, ConfigurationSource configurationSource, bool setTargetAsPrincipal = false)
	{
		return HasRelationship(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(targetEntityType, "targetEntityType"), MemberIdentity.Create(navigationToTargetName), MemberIdentity.Create(inverseNavigationName), setTargetAsPrincipal, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType targetEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationToTarget, [JetBrains.Annotations.CanBeNull] MemberInfo inverseNavigation, ConfigurationSource configurationSource, bool setTargetAsPrincipal = false)
	{
		return HasRelationship(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(targetEntityType, "targetEntityType"), MemberIdentity.Create(navigationToTarget), MemberIdentity.Create(inverseNavigation), setTargetAsPrincipal, configurationSource);
	}

	private InternalRelationshipBuilder HasRelationship(EntityType targetEntityType, MemberIdentity? navigationToTarget, MemberIdentity? inverseNavigation, bool setTargetAsPrincipal, ConfigurationSource configurationSource, bool? required = null)
	{
		MemberInfo memberInfo = navigationToTarget?.MemberInfo;
		if (!inverseNavigation.HasValue && (object)memberInfo != null && !memberInfo.GetMemberType().GetTypeInfo().IsAssignableFrom(targetEntityType.ClrType.GetTypeInfo()))
		{
			return targetEntityType.Builder.HasRelationship(Metadata, null, navigationToTarget, setTargetAsPrincipal, configurationSource, required);
		}
		InternalRelationshipBuilder internalRelationshipBuilder = InternalRelationshipBuilder.FindCurrentRelationshipBuilder(targetEntityType, Metadata, navigationToTarget, inverseNavigation, null, null);
		if (internalRelationshipBuilder != null)
		{
			bool flag = false;
			if (navigationToTarget.HasValue)
			{
				if (navigationToTarget.Value.Name == internalRelationshipBuilder.Metadata.DependentToPrincipal?.Name)
				{
					internalRelationshipBuilder.Metadata.UpdateDependentToPrincipalConfigurationSource(configurationSource);
				}
				else if (setTargetAsPrincipal)
				{
					flag = true;
				}
				else
				{
					internalRelationshipBuilder.Metadata.UpdatePrincipalToDependentConfigurationSource(configurationSource);
				}
				if (navigationToTarget.Value.Name != null)
				{
					Metadata.RemoveIgnored(navigationToTarget.Value.Name);
				}
			}
			if (inverseNavigation.HasValue)
			{
				if (inverseNavigation.Value.Name == internalRelationshipBuilder.Metadata.PrincipalToDependent?.Name)
				{
					internalRelationshipBuilder.Metadata.UpdatePrincipalToDependentConfigurationSource(configurationSource);
				}
				else if (setTargetAsPrincipal)
				{
					flag = true;
				}
				else
				{
					internalRelationshipBuilder.Metadata.UpdateDependentToPrincipalConfigurationSource(configurationSource);
				}
				if (inverseNavigation.Value.Name != null)
				{
					targetEntityType.RemoveIgnored(inverseNavigation.Value.Name);
				}
			}
			internalRelationshipBuilder.Metadata.UpdateConfigurationSource(configurationSource);
			if (!flag)
			{
				if (setTargetAsPrincipal)
				{
					internalRelationshipBuilder = internalRelationshipBuilder.HasEntityTypes(internalRelationshipBuilder.Metadata.PrincipalEntityType, internalRelationshipBuilder.Metadata.DeclaringEntityType, configurationSource);
					if (required.HasValue)
					{
						internalRelationshipBuilder = internalRelationshipBuilder.IsRequired(required.Value, configurationSource);
					}
				}
				return internalRelationshipBuilder;
			}
		}
		internalRelationshipBuilder = InternalRelationshipBuilder.FindCurrentRelationshipBuilder(Metadata, targetEntityType, inverseNavigation, navigationToTarget, null, null);
		if (internalRelationshipBuilder != null)
		{
			if (navigationToTarget.HasValue)
			{
				internalRelationshipBuilder.Metadata.UpdatePrincipalToDependentConfigurationSource(configurationSource);
				if (navigationToTarget.Value.Name != null)
				{
					Metadata.RemoveIgnored(navigationToTarget.Value.Name);
				}
			}
			if (inverseNavigation.HasValue)
			{
				internalRelationshipBuilder.Metadata.UpdateDependentToPrincipalConfigurationSource(configurationSource);
				if (inverseNavigation.Value.Name != null)
				{
					targetEntityType.RemoveIgnored(inverseNavigation.Value.Name);
				}
			}
			internalRelationshipBuilder.Metadata.UpdateConfigurationSource(configurationSource);
			if (!setTargetAsPrincipal)
			{
				return internalRelationshipBuilder;
			}
		}
		InternalRelationshipBuilder internalRelationshipBuilder2 = null;
		InternalRelationshipBuilder internalRelationshipBuilder3;
		using (IConventionBatch batch = Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			if (internalRelationshipBuilder != null)
			{
				internalRelationshipBuilder3 = internalRelationshipBuilder;
			}
			else
			{
				if (setTargetAsPrincipal || targetEntityType.DefiningEntityType != Metadata)
				{
					internalRelationshipBuilder2 = CreateForeignKey(targetEntityType.Builder, null, null, memberInfo?.GetSimpleMemberName(), required, configurationSource);
				}
				else
				{
					MemberIdentity? memberIdentity = navigationToTarget;
					navigationToTarget = inverseNavigation;
					inverseNavigation = memberIdentity;
					memberInfo = navigationToTarget?.MemberInfo;
					internalRelationshipBuilder2 = targetEntityType.Builder.CreateForeignKey(this, null, null, memberInfo?.GetSimpleMemberName(), null, configurationSource);
				}
				internalRelationshipBuilder3 = internalRelationshipBuilder2;
			}
			if (setTargetAsPrincipal)
			{
				internalRelationshipBuilder3 = internalRelationshipBuilder3.HasEntityTypes(targetEntityType.Builder.Metadata, Metadata, configurationSource);
				if (required.HasValue)
				{
					internalRelationshipBuilder3 = internalRelationshipBuilder3.IsRequired(required.Value, configurationSource);
				}
			}
			MemberInfo memberInfo2 = inverseNavigation?.MemberInfo;
			internalRelationshipBuilder3 = ((!inverseNavigation.HasValue) ? ((memberInfo != null) ? internalRelationshipBuilder3.HasNavigation(memberInfo, pointsToPrincipal: true, configurationSource) : internalRelationshipBuilder3.HasNavigation(navigationToTarget.Value.Name, pointsToPrincipal: true, configurationSource)) : (navigationToTarget.HasValue ? ((memberInfo != null || memberInfo2 != null) ? internalRelationshipBuilder3.HasNavigations(memberInfo, memberInfo2, configurationSource) : internalRelationshipBuilder3.HasNavigations(navigationToTarget.Value.Name, inverseNavigation.Value.Name, configurationSource)) : ((memberInfo2 != null) ? internalRelationshipBuilder3.HasNavigation(memberInfo2, pointsToPrincipal: false, configurationSource) : internalRelationshipBuilder3.HasNavigation(inverseNavigation.Value.Name, pointsToPrincipal: false, configurationSource))));
			if (internalRelationshipBuilder3 != null)
			{
				internalRelationshipBuilder3 = batch.Run(internalRelationshipBuilder3);
			}
		}
		if (internalRelationshipBuilder3 != null && ((navigationToTarget.HasValue && internalRelationshipBuilder3.Metadata.DependentToPrincipal?.Name != navigationToTarget.Value.Name) || (inverseNavigation.HasValue && internalRelationshipBuilder3.Metadata.PrincipalToDependent?.Name != inverseNavigation.Value.Name)) && ((inverseNavigation.HasValue && internalRelationshipBuilder3.Metadata.DependentToPrincipal?.Name != inverseNavigation.Value.Name) || (navigationToTarget.HasValue && internalRelationshipBuilder3.Metadata.PrincipalToDependent?.Name != navigationToTarget.Value.Name)))
		{
			internalRelationshipBuilder3 = null;
		}
		if (internalRelationshipBuilder3 == null)
		{
			if (internalRelationshipBuilder2?.Metadata.Builder != null)
			{
				internalRelationshipBuilder2.Metadata.DeclaringEntityType.Builder.HasNoRelationship(internalRelationshipBuilder2.Metadata, configurationSource);
			}
			return null;
		}
		return internalRelationshipBuilder3;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType principalEntityType, ConfigurationSource configurationSource)
	{
		return HasRelationshipInternal(principalEntityType, null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType principalEntityType, [JetBrains.Annotations.NotNull] Key principalKey, ConfigurationSource configurationSource)
	{
		return HasRelationshipInternal(principalEntityType, principalKey, configurationSource);
	}

	private InternalRelationshipBuilder HasRelationshipInternal(EntityType targetEntityType, Key principalKey, ConfigurationSource configurationSource)
	{
		InternalRelationshipBuilder internalRelationshipBuilder;
		InternalRelationshipBuilder internalRelationshipBuilder2;
		using (IConventionBatch batch = Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			internalRelationshipBuilder = CreateForeignKey(targetEntityType.Builder, null, principalKey, null, null, configurationSource);
			internalRelationshipBuilder2 = internalRelationshipBuilder;
			if (principalKey != null)
			{
				internalRelationshipBuilder2 = internalRelationshipBuilder2.HasEntityTypes(targetEntityType, Metadata, configurationSource)?.HasPrincipalKey(principalKey.Properties, configurationSource);
			}
			internalRelationshipBuilder2 = ((internalRelationshipBuilder2 == null) ? null : batch.Run(internalRelationshipBuilder2));
		}
		if (internalRelationshipBuilder2 == null)
		{
			if (internalRelationshipBuilder?.Metadata.Builder != null)
			{
				internalRelationshipBuilder.Metadata.DeclaringEntityType.Builder.HasNoRelationship(internalRelationshipBuilder.Metadata, configurationSource);
			}
			return null;
		}
		return internalRelationshipBuilder2;
	}

	public virtual InternalRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] string targetEntityTypeName, [JetBrains.Annotations.NotNull] string navigationName, ConfigurationSource configurationSource)
	{
		TypeIdentity targetEntityType = new TypeIdentity(targetEntityTypeName);
		return HasOwnership(in targetEntityType, MemberIdentity.Create(navigationName), null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] string navigationName, ConfigurationSource configurationSource)
	{
		TypeIdentity targetEntityType2 = new TypeIdentity(targetEntityType, Metadata.Model);
		return HasOwnership(in targetEntityType2, MemberIdentity.Create(navigationName), null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] MemberInfo navigationProperty, ConfigurationSource configurationSource)
	{
		TypeIdentity targetEntityType2 = new TypeIdentity(targetEntityType, Metadata.Model);
		return HasOwnership(in targetEntityType2, MemberIdentity.Create(navigationProperty), null, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] string navigationPropertyName, [JetBrains.Annotations.CanBeNull] string inversePropertyName, ConfigurationSource configurationSource)
	{
		TypeIdentity targetEntityType2 = new TypeIdentity(targetEntityType, Metadata.Model);
		return HasOwnership(in targetEntityType2, MemberIdentity.Create(navigationPropertyName), MemberIdentity.Create(inversePropertyName), configurationSource);
	}

	public virtual InternalRelationshipBuilder HasOwnership([JetBrains.Annotations.NotNull] Type targetEntityType, [JetBrains.Annotations.NotNull] MemberInfo navigationProperty, [JetBrains.Annotations.CanBeNull] MemberInfo inverseProperty, ConfigurationSource configurationSource)
	{
		TypeIdentity targetEntityType2 = new TypeIdentity(targetEntityType, Metadata.Model);
		return HasOwnership(in targetEntityType2, MemberIdentity.Create(navigationProperty), MemberIdentity.Create(inverseProperty), configurationSource);
	}

	private InternalRelationshipBuilder HasOwnership(in TypeIdentity targetEntityType, MemberIdentity navigation, MemberIdentity? inverse, ConfigurationSource configurationSource)
	{
		InternalEntityTypeBuilder internalEntityTypeBuilder2;
		InternalRelationshipBuilder internalRelationshipBuilder2;
		using (IConventionBatch batch = Metadata.Model.ConventionDispatcher.DelayConventions())
		{
			Navigation navigation2 = Metadata.FindNavigation(navigation.Name);
			if (navigation2 != null)
			{
				if (navigation2.GetTargetType().Name == targetEntityType.Name)
				{
					EntityType declaringEntityType = navigation2.ForeignKey.DeclaringEntityType;
					if (declaringEntityType.HasDefiningNavigation())
					{
						if (targetEntityType.Type != null)
						{
							ModelBuilder.Entity(targetEntityType.Type, declaringEntityType.DefiningNavigationName, declaringEntityType.DefiningEntityType, configurationSource);
						}
						else
						{
							ModelBuilder.Entity(targetEntityType.Name, declaringEntityType.DefiningNavigationName, declaringEntityType.DefiningEntityType, configurationSource);
						}
					}
					else if (targetEntityType.Type != null)
					{
						ModelBuilder.Entity(targetEntityType.Type, configurationSource, true);
					}
					else
					{
						ModelBuilder.Entity(targetEntityType.Name, configurationSource, true);
					}
					InternalRelationshipBuilder builder = navigation2.ForeignKey.Builder;
					builder = builder.IsRequired(true, configurationSource)?.HasEntityTypes(Metadata, builder.Metadata.FindNavigationsFromInHierarchy(Metadata).Single().GetTargetType(), configurationSource)?.HasNavigations(inverse, navigation, configurationSource)?.IsOwnership(true, configurationSource);
					return (builder == null) ? null : batch.Run(builder);
				}
				if (navigation2.ForeignKey.DeclaringEntityType.Builder.HasNoRelationship(navigation2.ForeignKey, configurationSource) == null)
				{
					return null;
				}
			}
			InternalEntityTypeBuilder internalEntityTypeBuilder = this;
			string name = targetEntityType.Name;
			Type type = targetEntityType.Type;
			if (type == null)
			{
				Type type2 = navigation2?.GetIdentifyingMemberInfo()?.GetMemberType();
				if (type2 != null)
				{
					type = type2.TryGetSequenceType() ?? type2;
				}
			}
			internalEntityTypeBuilder2 = ((!(type == null)) ? ModelBuilder.Metadata.FindEntityType(type)?.Builder : ModelBuilder.Metadata.FindEntityType(name)?.Builder);
			if (internalEntityTypeBuilder2 == null)
			{
				if (Metadata.Model.EntityTypeShouldHaveDefiningNavigation(name))
				{
					if (!configurationSource.Overrides(ConfigurationSource.Explicit) && ((type == null) ? Metadata.IsInDefinitionPath(name) : Metadata.IsInDefinitionPath(type)))
					{
						return null;
					}
					internalEntityTypeBuilder2 = ((type == null) ? ModelBuilder.Entity(name, navigation.Name, Metadata, configurationSource) : ModelBuilder.Entity(type, navigation.Name, Metadata, configurationSource));
				}
				else
				{
					if (ModelBuilder.IsIgnored(name, configurationSource))
					{
						return null;
					}
					ModelBuilder.Metadata.RemoveIgnored(name);
					internalEntityTypeBuilder2 = ((type == null) ? ModelBuilder.Entity(name, configurationSource, true) : ModelBuilder.Entity(type, configurationSource, true));
				}
				if (internalEntityTypeBuilder2 == null)
				{
					return null;
				}
			}
			else
			{
				ForeignKey foreignKey = internalEntityTypeBuilder2.Metadata.FindDeclaredOwnership();
				if (foreignKey != null)
				{
					if (!configurationSource.Overrides(ConfigurationSource.Explicit) && ((type == null) ? Metadata.IsInDefinitionPath(name) : Metadata.IsInDefinitionPath(type)))
					{
						return null;
					}
					InternalRelationshipBuilder internalRelationshipBuilder = foreignKey.Builder.IsWeakTypeDefinition(configurationSource);
					if (internalRelationshipBuilder == null)
					{
						return null;
					}
					if (foreignKey.DeclaringEntityType == Metadata)
					{
						internalEntityTypeBuilder = internalRelationshipBuilder.Metadata.DeclaringEntityType.Builder;
					}
					internalEntityTypeBuilder2 = ((type == null) ? ModelBuilder.Entity(name, navigation.Name, internalEntityTypeBuilder.Metadata, configurationSource) : ModelBuilder.Entity(type, navigation.Name, internalEntityTypeBuilder.Metadata, configurationSource));
				}
			}
			internalRelationshipBuilder2 = internalEntityTypeBuilder2.HasRelationship(internalEntityTypeBuilder.Metadata, inverse, navigation, setTargetAsPrincipal: true, configurationSource, true);
			internalRelationshipBuilder2 = batch.Run(internalRelationshipBuilder2.IsOwnership(true, configurationSource));
		}
		if (internalRelationshipBuilder2?.Metadata.Builder == null)
		{
			if (internalEntityTypeBuilder2.Metadata.Builder != null && internalEntityTypeBuilder2.Metadata.HasDefiningNavigation())
			{
				ModelBuilder.HasNoEntityType(internalEntityTypeBuilder2.Metadata, configurationSource);
			}
			return null;
		}
		return internalRelationshipBuilder2;
	}

	public virtual bool RemoveNonOwnershipRelationships(ForeignKey ownership, ConfigurationSource configurationSource)
	{
		List<ForeignKey> list = (from fk in Metadata.GetDerivedTypesInclusive().SelectMany((EntityType t) => t.GetDeclaredForeignKeys())
			where !fk.IsOwnership && fk.PrincipalToDependent != null && !Contains(ownership, fk)
			select fk).Concat(from fk in Metadata.GetDerivedTypesInclusive().SelectMany((EntityType t) => t.GetDeclaredReferencingForeignKeys())
			where !fk.IsOwnership && !Contains(fk.DeclaringEntityType.FindOwnership(), fk)
			select fk).Distinct().ToList();
		if (list.Any((ForeignKey fk) => !configurationSource.Overrides(fk.GetConfigurationSource())))
		{
			return false;
		}
		foreach (ForeignKey item in list)
		{
			item.DeclaringEntityType.Builder.HasNoRelationship(item, configurationSource);
		}
		return true;
	}

	private bool Contains(IForeignKey inheritedFk, IForeignKey derivedFk)
	{
		if (inheritedFk != null && inheritedFk.PrincipalEntityType.IsAssignableFrom(derivedFk.PrincipalEntityType))
		{
			return PropertyListComparer.Instance.Equals(inheritedFk.Properties, derivedFk.Properties);
		}
		return false;
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType targetEntityType, [JetBrains.Annotations.CanBeNull] string navigationName, ConfigurationSource configurationSource, bool setTargetAsPrincipal = false)
	{
		return HasRelationship(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(targetEntityType, "targetEntityType"), MemberIdentity.Create(navigationName), null, setTargetAsPrincipal, configurationSource);
	}

	public virtual InternalRelationshipBuilder HasRelationship([JetBrains.Annotations.NotNull] EntityType targetEntityType, [JetBrains.Annotations.CanBeNull] MemberInfo navigationProperty, ConfigurationSource configurationSource, bool setTargetAsPrincipal = false)
	{
		return HasRelationship(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(targetEntityType, "targetEntityType"), MemberIdentity.Create(navigationProperty), null, setTargetAsPrincipal, configurationSource);
	}

	public virtual InternalEntityTypeBuilder GetTargetEntityTypeBuilder(Type targetClrType, MemberInfo navigationInfo, ConfigurationSource? configurationSource)
	{
		ForeignKey foreignKey = Metadata.FindOwnership();
		if (foreignKey != null)
		{
			if (targetClrType.Equals(Metadata.ClrType))
			{
				return null;
			}
			if (targetClrType.IsAssignableFrom(foreignKey.PrincipalEntityType.ClrType))
			{
				if (configurationSource.HasValue)
				{
					foreignKey.PrincipalEntityType.UpdateConfigurationSource(configurationSource.Value);
				}
				return foreignKey.PrincipalEntityType.Builder;
			}
		}
		EntityType metadata = Metadata;
		InternalEntityTypeBuilder internalEntityTypeBuilder = null;
		if (!ModelBuilder.Metadata.EntityTypeShouldHaveDefiningNavigation(targetClrType))
		{
			EntityType entityType = ModelBuilder.Metadata.FindEntityType(targetClrType);
			ForeignKey foreignKey2 = entityType?.FindOwnership();
			if (foreignKey2 != null && metadata.Model.IsOwned(targetClrType) && (foreignKey2.PrincipalEntityType != metadata || foreignKey2.PrincipalToDependent?.Name != navigationInfo.GetSimpleMemberName()))
			{
				if (!configurationSource.HasValue || targetClrType.Equals(Metadata.ClrType))
				{
					return null;
				}
				return ModelBuilder.Entity(targetClrType, navigationInfo.GetSimpleMemberName(), metadata, configurationSource.Value);
			}
			bool value = foreignKey2 != null || metadata.Model.IsOwned(targetClrType);
			internalEntityTypeBuilder = (configurationSource.HasValue ? ModelBuilder.Entity(targetClrType, configurationSource.Value, value) : entityType?.Builder);
		}
		else if (!targetClrType.Equals(Metadata.ClrType))
		{
			EntityType definingEntityType = metadata.DefiningEntityType;
			if (definingEntityType != null && definingEntityType.ClrType.Equals(targetClrType))
			{
				if (configurationSource.HasValue)
				{
					metadata.DefiningEntityType.UpdateConfigurationSource(configurationSource.Value);
				}
				return metadata.DefiningEntityType.Builder;
			}
			internalEntityTypeBuilder = metadata.FindNavigation(navigationInfo.GetSimpleMemberName())?.GetTargetType().Builder ?? metadata.Model.FindEntityType(targetClrType, navigationInfo.GetSimpleMemberName(), metadata)?.Builder;
			if (internalEntityTypeBuilder == null && configurationSource.HasValue && !metadata.IsInDefinitionPath(targetClrType) && !metadata.IsInOwnershipPath(targetClrType))
			{
				return ModelBuilder.Entity(targetClrType, navigationInfo.GetSimpleMemberName(), metadata, configurationSource.Value);
			}
			if (configurationSource.HasValue)
			{
				internalEntityTypeBuilder?.Metadata.UpdateConfigurationSource(configurationSource.Value);
			}
		}
		return internalEntityTypeBuilder;
	}

	public virtual InternalRelationshipBuilder CreateForeignKey([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder principalEntityTypeBuilder, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] Key principalKey, [JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, bool? isRequired, ConfigurationSource configurationSource)
	{
		using IConventionBatch conventionBatch = ModelBuilder.Metadata.ConventionDispatcher.DelayConventions();
		ForeignKey foreignKey = SetOrAddForeignKey(null, principalEntityTypeBuilder, dependentProperties, principalKey, navigationToPrincipalName, isRequired, configurationSource);
		if (isRequired.HasValue && foreignKey.IsRequired == isRequired.Value)
		{
			foreignKey = foreignKey.SetIsRequired(isRequired.Value, configurationSource);
		}
		return (InternalRelationshipBuilder)(conventionBatch.Run(foreignKey)?.Builder);
	}

	public virtual InternalRelationshipBuilder UpdateForeignKey([JetBrains.Annotations.NotNull] ForeignKey foreignKey, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> dependentProperties, [JetBrains.Annotations.CanBeNull] Key principalKey, [JetBrains.Annotations.CanBeNull] string navigationToPrincipalName, bool? isRequired, ConfigurationSource? configurationSource)
	{
		using IConventionBatch conventionBatch = ModelBuilder.Metadata.ConventionDispatcher.DelayConventions();
		foreignKey = SetOrAddForeignKey(foreignKey, foreignKey.PrincipalEntityType.Builder, dependentProperties, principalKey, navigationToPrincipalName, isRequired, configurationSource);
		return (InternalRelationshipBuilder)(conventionBatch.Run(foreignKey)?.Builder);
	}

	private ForeignKey SetOrAddForeignKey(ForeignKey foreignKey, InternalEntityTypeBuilder principalEntityTypeBuilder, IReadOnlyList<Property> dependentProperties, Key principalKey, string navigationToPrincipalName, bool? isRequired, ConfigurationSource? configurationSource)
	{
		EntityType metadata = principalEntityTypeBuilder.Metadata;
		InternalEntityTypeBuilder builder = metadata.RootType().Builder;
		if (principalKey == null)
		{
			principalKey = metadata.FindPrimaryKey();
			if (principalKey != null && dependentProperties != null && (!ForeignKey.AreCompatible(principalKey.Properties, dependentProperties, metadata, Metadata, shouldThrow: false) || (foreignKey == null && Metadata.FindForeignKeysInHierarchy(dependentProperties, principalKey, metadata).Any())))
			{
				principalKey = null;
			}
			if (principalKey == null && foreignKey != null && (dependentProperties == null || ForeignKey.AreCompatible(foreignKey.PrincipalKey.Properties, dependentProperties, metadata, Metadata, shouldThrow: false)))
			{
				principalKey = foreignKey.PrincipalKey;
			}
		}
		if (dependentProperties != null)
		{
			dependentProperties = GetActualProperties(dependentProperties, ConfigurationSource.Convention);
			if (principalKey == null)
			{
				IReadOnlyList<Property> item = builder.TryCreateUniqueProperties(dependentProperties.Count, null, Enumerable.Repeat("", dependentProperties.Count), dependentProperties.Select((Property p) => p.ClrType), isRequired: true, "TempId").Item2;
				principalKey = builder.HasKeyInternal(item, ConfigurationSource.Convention).Metadata;
			}
		}
		else
		{
			if (principalKey == null)
			{
				IReadOnlyList<Property> item2 = builder.TryCreateUniqueProperties(1, null, new string[1] { "TempId" }, new Type[1] { typeof(int) }, isRequired: true, "").Item2;
				principalKey = builder.HasKeyInternal(item2, ConfigurationSource.Convention).Metadata;
			}
			if (foreignKey != null)
			{
				IReadOnlyList<Property> properties = foreignKey.Properties;
				Key principalKey2 = foreignKey.PrincipalKey;
				IReadOnlyList<Property> properties2 = CreateUniqueProperties(null, principalKey.Properties, isRequired.GetValueOrDefault(), "TempFk");
				foreignKey.SetProperties(properties2, principalKey, configurationSource);
				foreignKey.DeclaringEntityType.Builder.RemoveUnusedShadowProperties(properties);
				if (principalKey2 != principalKey)
				{
					principalKey2.DeclaringEntityType.Builder.RemoveKeyIfUnused(principalKey2);
				}
			}
			string baseName = (string.IsNullOrEmpty(navigationToPrincipalName) ? metadata.ShortName() : navigationToPrincipalName);
			dependentProperties = CreateUniqueProperties(null, principalKey.Properties, isRequired.GetValueOrDefault(), baseName);
		}
		if (foreignKey == null)
		{
			return Metadata.AddForeignKey(dependentProperties, principalKey, metadata, null, configurationSource.Value);
		}
		IReadOnlyList<Property> properties3 = foreignKey.Properties;
		Key principalKey3 = foreignKey.PrincipalKey;
		foreignKey.SetProperties(dependentProperties, principalKey, configurationSource);
		if (properties3 != dependentProperties)
		{
			foreignKey.DeclaringEntityType.Builder.RemoveUnusedShadowProperties(properties3);
		}
		if (principalKey3 != principalKey)
		{
			principalKey3.DeclaringEntityType.Builder.RemoveKeyIfUnused(principalKey3);
		}
		return foreignKey;
	}

	public virtual bool ShouldReuniquifyTemporaryProperties([JetBrains.Annotations.NotNull] ForeignKey foreignKey)
	{
		return TryCreateUniqueProperties(foreignKey.PrincipalKey.Properties.Count, foreignKey.Properties, foreignKey.PrincipalKey.Properties.Select((Property p) => p.Name), foreignKey.PrincipalKey.Properties.Select((Property p) => p.ClrType), foreignKey.IsRequired && foreignKey.GetIsRequiredConfigurationSource().Overrides(ConfigurationSource.Convention), foreignKey.DependentToPrincipal?.Name ?? foreignKey.PrincipalEntityType.ShortName()).Item1;
	}

	public virtual Property CreateUniqueProperty(string propertyName, Type propertyType, bool isRequired)
	{
		return CreateUniqueProperties(new string[1] { propertyName }, new Type[1] { propertyType }, isRequired).First();
	}

	public virtual IReadOnlyList<Property> CreateUniqueProperties(IReadOnlyList<string> propertyNames, IReadOnlyList<Type> propertyTypes, bool isRequired)
	{
		return TryCreateUniqueProperties(propertyNames.Count, null, propertyNames, propertyTypes, isRequired, "").Item2;
	}

	private IReadOnlyList<Property> CreateUniqueProperties(IReadOnlyList<Property> currentProperties, IReadOnlyList<Property> principalProperties, bool isRequired, string baseName)
	{
		return TryCreateUniqueProperties(principalProperties.Count, currentProperties, principalProperties.Select((Property p) => p.Name), principalProperties.Select((Property p) => p.ClrType), isRequired, baseName).Item2;
	}

	private (bool, IReadOnlyList<Property>) TryCreateUniqueProperties(int propertyCount, IReadOnlyList<Property> currentProperties, IEnumerable<string> principalPropertyNames, IEnumerable<Type> principalPropertyTypes, bool isRequired, string baseName)
	{
		Property[] array = ((currentProperties == null) ? new Property[propertyCount] : null);
		IReadOnlyDictionary<string, PropertyInfo> runtimeProperties = Metadata.GetRuntimeProperties();
		IReadOnlyDictionary<string, FieldInfo> runtimeFields = Metadata.GetRuntimeFields();
		bool item = false;
		using (IEnumerator<string> enumerator = principalPropertyNames.GetEnumerator())
		{
			using IEnumerator<Type> enumerator2 = principalPropertyTypes.GetEnumerator();
			for (int i = 0; i < propertyCount; i++)
			{
				if (!enumerator.MoveNext())
				{
					break;
				}
				if (!enumerator2.MoveNext())
				{
					break;
				}
				string current = enumerator.Current;
				Type current2 = enumerator2.Current;
				string text = (current.StartsWith(baseName, StringComparison.OrdinalIgnoreCase) ? current : (baseName + current));
				Type type = current2.MakeNullable(!isRequired);
				int num = -1;
				string propertyName;
				while (true)
				{
					propertyName = text + ((++num > 0) ? num.ToString(CultureInfo.InvariantCulture) : "");
					if (!Metadata.FindPropertiesInHierarchy(propertyName).Any() && (runtimeProperties == null || !runtimeProperties.ContainsKey(propertyName)) && (runtimeFields == null || !runtimeFields.ContainsKey(propertyName)) && !IsIgnored(propertyName, ConfigurationSource.Convention))
					{
						if (currentProperties == null)
						{
							InternalPropertyBuilder internalPropertyBuilder = Property(type, propertyName, null, ConfigurationSource.Convention);
							if (type.IsNullableType())
							{
								internalPropertyBuilder.IsRequired(isRequired, ConfigurationSource.Convention);
							}
							array[i] = internalPropertyBuilder.Metadata;
						}
						else
						{
							item = true;
						}
						break;
					}
					Property property = currentProperties?.SingleOrDefault((Property p) => p.Name == propertyName);
					if (property != null)
					{
						if (property.IsShadowProperty() && property.ClrType != type && isRequired)
						{
							item = true;
						}
						break;
					}
				}
			}
		}
		return (item, array);
	}

	public virtual IReadOnlyList<Property> GetOrCreateProperties([JetBrains.Annotations.CanBeNull] IReadOnlyList<string> propertyNames, ConfigurationSource? configurationSource, [JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> referencedProperties = null, bool required = false, bool useDefaultType = false)
	{
		if (propertyNames == null)
		{
			return null;
		}
		if (referencedProperties != null && referencedProperties.Count != propertyNames.Count)
		{
			referencedProperties = null;
		}
		List<Property> list = new List<Property>();
		for (int i = 0; i < propertyNames.Count; i++)
		{
			string text = propertyNames[i];
			Property property = Metadata.FindProperty(text);
			if (property == null)
			{
				Type type = ((referencedProperties != null) ? referencedProperties[i].ClrType : (useDefaultType ? typeof(int) : null));
				if (!configurationSource.HasValue)
				{
					return null;
				}
				InternalPropertyBuilder internalPropertyBuilder = Property(required ? type : type?.MakeNullable(), text, null, configurationSource.Value);
				if (internalPropertyBuilder == null)
				{
					return null;
				}
				property = internalPropertyBuilder.Metadata;
			}
			else if (configurationSource.HasValue)
			{
				property = ((!ConfigurationSource.Convention.Overrides(property.GetTypeConfigurationSource()) || !property.IsShadowProperty() || (property.IsNullable && (!required || property.GetIsNullableConfigurationSource().HasValue)) || !property.ClrType.IsNullableType()) ? property.DeclaringEntityType.Builder.Property(property.Name, configurationSource.Value).Metadata : property.DeclaringEntityType.Builder.Property(property.ClrType.MakeNullable(nullable: false), property.Name, configurationSource.Value).Metadata);
			}
			list.Add(property);
		}
		return list;
	}

	public virtual IReadOnlyList<Property> GetOrCreateProperties([JetBrains.Annotations.CanBeNull] IEnumerable<MemberInfo> clrMembers, ConfigurationSource? configurationSource)
	{
		if (clrMembers == null)
		{
			return null;
		}
		List<Property> list = new List<Property>();
		foreach (MemberInfo clrMember in clrMembers)
		{
			InternalPropertyBuilder internalPropertyBuilder = Property(clrMember, configurationSource);
			if (internalPropertyBuilder == null)
			{
				return null;
			}
			list.Add(internalPropertyBuilder.Metadata);
		}
		return list;
	}

	public virtual IReadOnlyList<Property> GetActualProperties([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource? configurationSource)
	{
		if (properties == null)
		{
			return null;
		}
		Property[] array = new Property[properties.Count];
		for (int i = 0; i < array.Length; i++)
		{
			Property property = properties[i];
			ConfigurationSource? typeConfigurationSource = property.GetTypeConfigurationSource();
			InternalPropertyBuilder internalPropertyBuilder = ((property.Builder != null && property.DeclaringEntityType.IsAssignableFrom(Metadata)) ? property.Builder : Property(typeConfigurationSource.Overrides(ConfigurationSource.DataAnnotation) ? property.ClrType : null, property.Name, property.GetIdentifyingMemberInfo(), typeConfigurationSource.Overrides(ConfigurationSource.DataAnnotation) ? typeConfigurationSource : null, configurationSource));
			if (internalPropertyBuilder == null)
			{
				return null;
			}
			array[i] = internalPropertyBuilder.Metadata;
		}
		return array;
	}

	public virtual InternalEntityTypeBuilder HasChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, ConfigurationSource configurationSource)
	{
		if (CanSetChangeTrackingStrategy(changeTrackingStrategy, configurationSource))
		{
			Metadata.SetChangeTrackingStrategy(changeTrackingStrategy, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetChangeTrackingStrategyConfigurationSource()))
		{
			return Metadata.GetChangeTrackingStrategy() == changeTrackingStrategy;
		}
		return true;
	}

	public virtual InternalEntityTypeBuilder UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		if (CanSetPropertyAccessMode(propertyAccessMode, configurationSource))
		{
			Metadata.SetPropertyAccessMode(propertyAccessMode, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetPropertyAccessModeConfigurationSource()))
		{
			return Metadata.GetPropertyAccessMode() == propertyAccessMode;
		}
		return true;
	}

	public virtual InternalPropertyBuilder GetOrCreateDiscriminatorProperty(Type type, string name, bool fromDataAnnotation)
	{
		IProperty property = Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDiscriminatorProperty(Metadata);
		if ((name != null && property?.Name != name) || (type != null && property?.ClrType != type))
		{
			property = null;
		}
		ConfigurationSource value = (fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
		return Metadata.RootType().Builder.Property(type ?? property?.ClrType ?? _defaultDiscriminatorType, name ?? property?.Name ?? _defaultDiscriminatorName, (type != null) ? new ConfigurationSource?(value) : null, value);
	}

	public virtual DiscriminatorBuilder DiscriminatorBuilder(InternalPropertyBuilder discriminatorPropertyBuilder, ConfigurationSource configurationSource)
	{
		if (discriminatorPropertyBuilder == null)
		{
			return null;
		}
		InternalEntityTypeBuilder builder = Metadata.RootType().Builder;
		Property metadata = discriminatorPropertyBuilder.Metadata;
		discriminatorPropertyBuilder = builder.Property(metadata.ClrType, metadata.Name, null, ConfigurationSource.Convention);
		RemoveUnusedDiscriminatorProperty(metadata, configurationSource);
		builder.Metadata.SetDiscriminatorProperty(metadata, configurationSource);
		discriminatorPropertyBuilder.IsRequired(true, configurationSource);
		discriminatorPropertyBuilder.HasValueGenerator(DiscriminatorValueGenerator.Factory, configurationSource);
		return new DiscriminatorBuilder(Metadata);
	}

	public virtual InternalEntityTypeBuilder HasNoDeclaredDiscriminator(ConfigurationSource configurationSource)
	{
		if (Metadata["DiscriminatorProperty"] != null && !configurationSource.Overrides(Metadata.GetDiscriminatorPropertyConfigurationSource()))
		{
			return null;
		}
		if (Metadata.BaseType == null)
		{
			RemoveUnusedDiscriminatorProperty(null, configurationSource);
		}
		Metadata.SetDiscriminatorProperty(null, configurationSource);
		return this;
	}

	private void RemoveUnusedDiscriminatorProperty(Property newDiscriminatorProperty, ConfigurationSource configurationSource)
	{
		Property property = Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDiscriminatorProperty(Metadata) as Property;
		if (property?.Builder != null && property != newDiscriminatorProperty)
		{
			property.DeclaringEntityType.Builder.RemoveUnusedShadowProperties(new Property[1] { property });
			if (property.Builder != null)
			{
				property.Builder.IsRequired(null, configurationSource);
				property.Builder.HasValueGenerator((Type)null, configurationSource);
			}
		}
	}

	private bool CanSetDiscriminator(IProperty discriminatorProperty, string name, Type discriminatorType, bool fromDataAnnotation)
	{
		if ((name == null && discriminatorType == null) || ((name == null || discriminatorProperty?.Name == name) && (discriminatorType == null || discriminatorProperty?.ClrType == discriminatorType)) || (fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention).Overrides(Metadata.GetDiscriminatorPropertyConfigurationSource()))
		{
			if (discriminatorProperty == null)
			{
				return Metadata.RootType().Builder.CanAddProperty(discriminatorType ?? _defaultDiscriminatorType, name ?? _defaultDiscriminatorName, (discriminatorType != null) ? new ConfigurationSource?(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention) : null);
			}
			return true;
		}
		return false;
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasBaseType(IConventionEntityType baseEntityType, bool fromDataAnnotation)
	{
		return HasBaseType((EntityType)baseEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetBaseType(IConventionEntityType baseEntityType, bool fromDataAnnotation)
	{
		return CanSetBaseType((EntityType)baseEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionEntityTypeBuilder.Property(Type propertyType, string propertyName, bool setTypeConfigurationSource, bool fromDataAnnotation)
	{
		return Property(propertyType, propertyName, setTypeConfigurationSource ? new ConfigurationSource?(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention) : null, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionEntityTypeBuilder.Property(MemberInfo memberInfo, bool fromDataAnnotation)
	{
		return Property(memberInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IReadOnlyList<IConventionProperty> IConventionEntityTypeBuilder.GetOrCreateProperties(IReadOnlyList<string> propertyNames, bool fromDataAnnotation)
	{
		return GetOrCreateProperties(propertyNames, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IReadOnlyList<IConventionProperty> IConventionEntityTypeBuilder.GetOrCreateProperties(IEnumerable<MemberInfo> memberInfos, bool fromDataAnnotation)
	{
		return GetOrCreateProperties(memberInfos, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.RemoveUnusedShadowProperties(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return RemoveUnusedShadowProperties(properties, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionServicePropertyBuilder IConventionEntityTypeBuilder.ServiceProperty(MemberInfo memberInfo, bool fromDataAnnotation)
	{
		return ServiceProperty(memberInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.IsIgnored(string name, bool fromDataAnnotation)
	{
		return IsIgnored(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.Ignore(string name, bool fromDataAnnotation)
	{
		return Ignore(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanIgnore(string name, bool fromDataAnnotation)
	{
		return CanIgnore(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionKeyBuilder IConventionEntityTypeBuilder.PrimaryKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return PrimaryKey((properties as IReadOnlyList<Property>) ?? properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetPrimaryKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return CanSetPrimaryKey((properties as IReadOnlyList<Property>) ?? properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionKeyBuilder IConventionEntityTypeBuilder.HasKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return HasKey((properties as IReadOnlyList<Property>) ?? properties.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoKey(bool fromDataAnnotation)
	{
		return HasNoKey(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Key key = Metadata.FindDeclaredKey(properties);
		if (key == null)
		{
			return this;
		}
		return HasNoKey(key, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoKey(IConventionKey key, bool fromDataAnnotation)
	{
		return HasNoKey((Key)key, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionIndexBuilder IConventionEntityTypeBuilder.HasIndex(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return HasIndex((properties as IReadOnlyList<Property>) ?? properties.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoIndex(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Index index = Metadata.FindDeclaredIndex(properties);
		if (index == null)
		{
			return this;
		}
		return HasNoIndex(index, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoIndex(IConventionIndex index, bool fromDataAnnotation)
	{
		return HasNoIndex((Index)index, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType targetEntityType, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)targetEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType principalEntityType, IReadOnlyList<IConventionProperty> dependentProperties, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)principalEntityType, (dependentProperties as IReadOnlyList<Property>) ?? dependentProperties.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType principalEntityType, IConventionKey principalKey, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)principalEntityType, (Key)principalKey, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType principalEntityType, IReadOnlyList<IConventionProperty> dependentProperties, IConventionKey principalKey, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)principalEntityType, (dependentProperties as IReadOnlyList<Property>) ?? dependentProperties.Cast<Property>().ToList(), (Key)principalKey, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType targetEntityType, string navigationToTargetName, bool setTargetAsPrincipal, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)targetEntityType, navigationToTargetName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention, setTargetAsPrincipal);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType targetEntityType, MemberInfo navigationToTarget, bool setTargetAsPrincipal, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)targetEntityType, navigationToTarget, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention, setTargetAsPrincipal);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType targetEntityType, string navigationToTargetName, string inverseNavigationName, bool setTargetAsPrincipal, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)targetEntityType, navigationToTargetName, inverseNavigationName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention, setTargetAsPrincipal);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasRelationship(IConventionEntityType targetEntityType, MemberInfo navigationToTarget, MemberInfo inverseNavigation, bool setTargetAsPrincipal, bool fromDataAnnotation)
	{
		return HasRelationship((EntityType)targetEntityType, navigationToTarget, inverseNavigation, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention, setTargetAsPrincipal);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasOwnership(Type targetEntityType, string navigationToTargetName, bool fromDataAnnotation)
	{
		return HasOwnership(targetEntityType, navigationToTargetName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasOwnership(Type targetEntityType, MemberInfo navigationToTarget, bool fromDataAnnotation)
	{
		return HasOwnership(targetEntityType, navigationToTarget, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasOwnership(Type targetEntityType, string navigationToTargetName, string inversePropertyName, bool fromDataAnnotation)
	{
		return HasOwnership(targetEntityType, navigationToTargetName, inversePropertyName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionRelationshipBuilder IConventionEntityTypeBuilder.HasOwnership(Type targetEntityType, MemberInfo navigationToTarget, MemberInfo inverseProperty, bool fromDataAnnotation)
	{
		return HasOwnership(targetEntityType, navigationToTarget, inverseProperty, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoRelationship(IReadOnlyList<IConventionProperty> properties, IConventionKey principalKey, IConventionEntityType principalEntityType, bool fromDataAnnotation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalKey, "principalKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		ForeignKey foreignKey = Metadata.FindDeclaredForeignKey(properties, principalKey, principalEntityType);
		if (foreignKey == null)
		{
			return this;
		}
		return HasNoRelationship(foreignKey, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoRelationship(IConventionForeignKey foreignKey, bool fromDataAnnotation)
	{
		return HasNoRelationship((ForeignKey)foreignKey, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanAddNavigation(string navigationName, bool fromDataAnnotation)
	{
		return CanAddNavigation(navigationName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasQueryFilter(LambdaExpression filter, bool fromDataAnnotation)
	{
		return HasQueryFilter(filter, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetQueryFilter(LambdaExpression filter, bool fromDataAnnotation)
	{
		return CanSetQueryFilter(filter, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasDefiningQuery(LambdaExpression query, bool fromDataAnnotation)
	{
		return HasDefiningQuery(query, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetDefiningQuery(LambdaExpression query, bool fromDataAnnotation)
	{
		return CanSetDefiningQuery(query, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation)
	{
		return HasChangeTrackingStrategy(changeTrackingStrategy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, bool fromDataAnnotation)
	{
		return CanSetChangeTrackingStrategy(changeTrackingStrategy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation)
	{
		return UsePropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation)
	{
		return CanSetPropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionDiscriminatorBuilder IConventionEntityTypeBuilder.HasDiscriminator(bool fromDataAnnotation)
	{
		return DiscriminatorBuilder(GetOrCreateDiscriminatorProperty(null, null, fromDataAnnotation: false), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionDiscriminatorBuilder IConventionEntityTypeBuilder.HasDiscriminator(Type type, bool fromDataAnnotation)
	{
		if (!((IConventionEntityTypeBuilder)this).CanSetDiscriminator(type, fromDataAnnotation))
		{
			return null;
		}
		return DiscriminatorBuilder(GetOrCreateDiscriminatorProperty(type, null, fromDataAnnotation), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionDiscriminatorBuilder IConventionEntityTypeBuilder.HasDiscriminator(string name, bool fromDataAnnotation)
	{
		if (!((IConventionEntityTypeBuilder)this).CanSetDiscriminator(name, fromDataAnnotation))
		{
			return null;
		}
		return DiscriminatorBuilder(GetOrCreateDiscriminatorProperty(null, name, fromDataAnnotation), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionDiscriminatorBuilder IConventionEntityTypeBuilder.HasDiscriminator(string name, Type type, bool fromDataAnnotation)
	{
		if (!((IConventionEntityTypeBuilder)this).CanSetDiscriminator(type, name, fromDataAnnotation))
		{
			return null;
		}
		return DiscriminatorBuilder(Metadata.RootType().Builder.Property(type, name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionDiscriminatorBuilder IConventionEntityTypeBuilder.HasDiscriminator(MemberInfo memberInfo, bool fromDataAnnotation)
	{
		if (!((IConventionEntityTypeBuilder)this).CanSetDiscriminator(memberInfo.GetMemberType(), memberInfo.GetSimpleMemberName(), fromDataAnnotation))
		{
			return null;
		}
		return DiscriminatorBuilder(Metadata.RootType().Builder.Property(memberInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityTypeBuilder IConventionEntityTypeBuilder.HasNoDeclaredDiscriminator(bool fromDataAnnotation)
	{
		return HasNoDeclaredDiscriminator(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionEntityTypeBuilder.CanSetDiscriminator(string name, bool fromDataAnnotation)
	{
		return CanSetDiscriminator(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDiscriminatorProperty(Metadata), name, null, fromDataAnnotation);
	}

	bool IConventionEntityTypeBuilder.CanSetDiscriminator(Type type, bool fromDataAnnotation)
	{
		return CanSetDiscriminator(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDiscriminatorProperty(Metadata), null, type, fromDataAnnotation);
	}

	bool IConventionEntityTypeBuilder.CanSetDiscriminator(Type type, string name, bool fromDataAnnotation)
	{
		return CanSetDiscriminator(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDiscriminatorProperty(Metadata), name, type, fromDataAnnotation);
	}
}
