using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class ModelValidator : IModelValidator
{
	protected virtual ModelValidatorDependencies Dependencies { get; }

	public ModelValidator([JetBrains.Annotations.NotNull] ModelValidatorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		((Model)model).MakeReadonly();
		ValidateNoShadowEntities(model, logger);
		ValidateIgnoredMembers(model, logger);
		ValidatePropertyMapping(model, logger);
		ValidateRelationships(model, logger);
		ValidateDefiningNavigations(model, logger);
		ValidateOwnership(model, logger);
		ValidateNonNullPrimaryKeys(model, logger);
		ValidateNoShadowKeys(model, logger);
		ValidateNoMutableKeys(model, logger);
		ValidateNoCycles(model, logger);
		ValidateClrInheritance(model, logger);
		ValidateDiscriminatorValues(model, logger);
		ValidateChangeTrackingStrategy(model, logger);
		ValidateForeignKeys(model, logger);
		ValidateFieldMapping(model, logger);
		ValidateKeylessTypes(model, logger);
		ValidateQueryFilters(model, logger);
		ValidateDefiningQuery(model, logger);
		ValidateData(model, logger);
		ValidateTypeMappings(model, logger);
		LogShadowProperties(model, logger);
	}

	protected virtual void ValidateRelationships([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			foreach (IForeignKey declaredForeignKey in entityType.GetDeclaredForeignKeys())
			{
				if (declaredForeignKey.IsUnique && declaredForeignKey is IConventionForeignKey conventionForeignKey && !conventionForeignKey.GetPrincipalEndConfigurationSource().HasValue)
				{
					throw new InvalidOperationException(CoreStrings.AmbiguousOneToOneRelationship(declaredForeignKey.DeclaringEntityType.DisplayName() + ((declaredForeignKey.DependentToPrincipal == null) ? "" : ("." + declaredForeignKey.DependentToPrincipal.Name)), declaredForeignKey.PrincipalEntityType.DisplayName() + ((declaredForeignKey.PrincipalToDependent == null) ? "" : ("." + declaredForeignKey.PrincipalToDependent.Name))));
				}
			}
		}
	}

	protected virtual void ValidatePropertyMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		if (!(model is IConventionModel conventionModel))
		{
			return;
		}
		foreach (IConventionEntityType entityType in conventionModel.GetEntityTypes())
		{
			IConventionProperty conventionProperty = entityType.GetProperties().FirstOrDefault((IConventionProperty p) => (!ConfigurationSource.Convention.Overrides(p.GetConfigurationSource()) || !p.IsShadowProperty()) && p.FindTypeMapping() == null);
			if (conventionProperty != null)
			{
				throw new InvalidOperationException(CoreStrings.PropertyNotMapped(entityType.DisplayName(), conventionProperty.Name, conventionProperty.ClrType.ShortDisplayName()));
			}
			if (!entityType.HasClrType())
			{
				continue;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			IReadOnlyDictionary<string, PropertyInfo> runtimeProperties = entityType.AsEntityType("ValidatePropertyMapping").GetRuntimeProperties();
			hashSet.UnionWith(from pi in runtimeProperties.Values
				where pi.IsCandidateProperty()
				select pi.GetSimpleMemberName());
			hashSet.ExceptWith(from p in entityType.GetProperties()
				select p.Name);
			hashSet.ExceptWith(from p in entityType.GetNavigations()
				select p.Name);
			hashSet.ExceptWith(from p in entityType.GetServiceProperties()
				select p.Name);
			hashSet.RemoveWhere((string p) => entityType.FindIgnoredConfigurationSource(p).HasValue);
			if (hashSet.Count <= 0)
			{
				continue;
			}
			foreach (string item in hashSet)
			{
				PropertyInfo actualProperty = runtimeProperties[item];
				Type propertyType = actualProperty.PropertyType;
				Type type = propertyType.TryGetSequenceType();
				if (conventionModel.FindIgnoredConfigurationSource(propertyType.DisplayName()).HasValue || (type != null && conventionModel.FindIgnoredConfigurationSource(type.DisplayName()).HasValue))
				{
					continue;
				}
				Type type2 = FindCandidateNavigationPropertyType(actualProperty);
				bool flag = type2 != null && (conventionModel.HasEntityTypeWithDefiningNavigation(type2) || conventionModel.IsOwned(type2));
				if ((object)type2 != null && type2.IsValidEntityType() && (flag || conventionModel.FindEntityType(type2) != null || type2.GetRuntimeProperties().Any((PropertyInfo p) => p.IsCandidateProperty())))
				{
					if ((!entityType.IsKeyless || type == null) && entityType.GetDerivedTypes().All((IConventionEntityType dt) => dt.GetDeclaredNavigations().FirstOrDefault((IConventionNavigation n) => n.Name == actualProperty.GetSimpleMemberName()) == null) && (!flag || (!type2.Equals(entityType.ClrType) && (!entityType.IsInOwnershipPath(type2) || (entityType.FindOwnership().PrincipalEntityType.ClrType.Equals(type2) && type == null)) && (!entityType.IsInDefinitionPath(type2) || (entityType.DefiningEntityType.ClrType.Equals(type2) && type == null)))))
					{
						if (conventionModel.IsOwned(entityType.ClrType) && conventionModel.IsOwned(type2))
						{
							throw new InvalidOperationException(CoreStrings.AmbiguousOwnedNavigation(entityType.ClrType.ShortDisplayName(), type2.ShortDisplayName()));
						}
						throw new InvalidOperationException(CoreStrings.NavigationNotAdded(entityType.DisplayName(), actualProperty.Name, propertyType.ShortDisplayName()));
					}
					continue;
				}
				if ((type == null && propertyType.GetTypeInfo().IsInterface) || ((object)type != null && type.GetTypeInfo().IsInterface))
				{
					throw new InvalidOperationException(CoreStrings.InterfacePropertyNotAdded(entityType.DisplayName(), actualProperty.Name, propertyType.ShortDisplayName()));
				}
				throw new InvalidOperationException(CoreStrings.PropertyNotAdded(entityType.DisplayName(), actualProperty.Name, propertyType.ShortDisplayName()));
			}
		}
	}

	private Type FindCandidateNavigationPropertyType(PropertyInfo propertyInfo)
	{
		return Dependencies.MemberClassifier.FindCandidateNavigationPropertyType(propertyInfo);
	}

	protected virtual void ValidateIgnoredMembers([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		if (!(model is IConventionModel conventionModel))
		{
			return;
		}
		foreach (IConventionEntityType entityType in conventionModel.GetEntityTypes())
		{
			foreach (string ignoredMember in entityType.GetIgnoredMembers())
			{
				if (entityType.FindIgnoredConfigurationSource(ignoredMember) == ConfigurationSource.Explicit)
				{
					IConventionProperty conventionProperty = entityType.FindProperty(ignoredMember);
					if (conventionProperty != null && conventionProperty.DeclaringEntityType != entityType)
					{
						throw new InvalidOperationException(CoreStrings.InheritedPropertyCannotBeIgnored(ignoredMember, entityType.DisplayName(), conventionProperty.DeclaringEntityType.DisplayName()));
					}
					IConventionNavigation conventionNavigation = entityType.FindNavigation(ignoredMember);
					if (conventionNavigation != null && conventionNavigation.DeclaringEntityType != entityType)
					{
						throw new InvalidOperationException(CoreStrings.InheritedPropertyCannotBeIgnored(ignoredMember, entityType.DisplayName(), conventionNavigation.DeclaringEntityType.DisplayName()));
					}
				}
			}
		}
	}

	protected virtual void ValidateNoShadowEntities([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		IEntityType entityType2 = model.GetEntityTypes().FirstOrDefault((IEntityType entityType) => !entityType.HasClrType());
		if (entityType2 != null)
		{
			throw new InvalidOperationException(CoreStrings.ShadowEntity(entityType2.DisplayName()));
		}
	}

	protected virtual void ValidateNoShadowKeys([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType item in from t in model.GetEntityTypes()
			where t.ClrType != null
			select t)
		{
			foreach (IKey declaredKey in item.GetDeclaredKeys())
			{
				if (declaredKey.Properties.Any((IProperty p) => p.IsShadowProperty()) && declaredKey is Key key && ConfigurationSource.Convention.Overrides(key.GetConfigurationSource()) && !declaredKey.IsPrimaryKey())
				{
					IForeignKey foreignKey = declaredKey.GetReferencingForeignKeys().FirstOrDefault();
					if (foreignKey != null)
					{
						throw new InvalidOperationException(CoreStrings.ReferencedShadowKey(foreignKey.DeclaringEntityType.DisplayName() + ((foreignKey.DependentToPrincipal == null) ? "" : ("." + foreignKey.DependentToPrincipal.Name)), item.DisplayName() + ((foreignKey.PrincipalToDependent == null) ? "" : ("." + foreignKey.PrincipalToDependent.Name)), foreignKey.Properties.Format(includeTypes: true), item.FindPrimaryKey().Properties.Format(includeTypes: true)));
					}
				}
			}
		}
	}

	protected virtual void ValidateNoMutableKeys([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			foreach (IKey declaredKey in entityType.GetDeclaredKeys())
			{
				IProperty property = declaredKey.Properties.FirstOrDefault((IProperty p) => p.ValueGenerated.HasFlag(ValueGenerated.OnUpdate));
				if (property != null)
				{
					throw new InvalidOperationException(CoreStrings.MutableKeyProperty(property.Name));
				}
			}
		}
	}

	protected virtual void ValidateNoCycles([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		HashSet<IEntityType> hashSet = new HashSet<IEntityType>(model.GetEntityTypes());
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			IKey key = entityType.FindPrimaryKey();
			if (key != null)
			{
				Queue<IForeignKey> queue = new Queue<IForeignKey>(from fk in entityType.FindForeignKeys(key.Properties)
					where fk.PrincipalEntityType != entityType
					select fk);
				while (queue.Count > 0)
				{
					IForeignKey foreignKey = queue.Dequeue();
					if (!foreignKey.PrincipalKey.IsPrimaryKey() || !hashSet.Contains(foreignKey.PrincipalEntityType))
					{
						continue;
					}
					if (foreignKey.PrincipalEntityType == entityType)
					{
						throw new InvalidOperationException(CoreStrings.IdentifyingRelationshipCycle(entityType.DisplayName()));
					}
					foreach (IForeignKey item in foreignKey.PrincipalEntityType.FindForeignKeys(foreignKey.PrincipalKey.Properties))
					{
						if (item.PrincipalEntityType != item.DeclaringEntityType)
						{
							queue.Enqueue(item);
						}
					}
				}
			}
			hashSet.Remove(entityType);
		}
	}

	protected virtual void ValidateNonNullPrimaryKeys([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		IEntityType entityType = model.GetEntityTypes().FirstOrDefault((IEntityType et) => !((EntityType)et).IsKeyless && et.BaseType == null && et.FindPrimaryKey() == null);
		if (entityType != null)
		{
			throw new InvalidOperationException(CoreStrings.EntityRequiresKey(entityType.DisplayName()));
		}
	}

	protected virtual void ValidateClrInheritance([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		HashSet<IEntityType> validEntityTypes = new HashSet<IEntityType>();
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			ValidateClrInheritance(model, entityType, validEntityTypes);
		}
	}

	private void ValidateClrInheritance([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] HashSet<IEntityType> validEntityTypes)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(validEntityTypes, "validEntityTypes");
		if (validEntityTypes.Contains(entityType))
		{
			return;
		}
		if (!entityType.HasDefiningNavigation() && entityType.FindDeclaredOwnership() == null && entityType.BaseType != null)
		{
			Type type = entityType.ClrType?.GetTypeInfo().BaseType;
			while (type != null)
			{
				IEntityType entityType2 = model.FindEntityType(type);
				if (entityType2 != null)
				{
					if (entityType2.IsAssignableFrom(entityType))
					{
						break;
					}
					throw new InvalidOperationException(CoreStrings.InconsistentInheritance(entityType.DisplayName(), entityType2.DisplayName()));
				}
				type = type.GetTypeInfo().BaseType;
			}
		}
		Type clrType = entityType.ClrType;
		if ((object)clrType != null && !clrType.IsInstantiable() && !entityType.GetDerivedTypes().Any())
		{
			throw new InvalidOperationException(CoreStrings.AbstractLeafEntityType(entityType.DisplayName()));
		}
		validEntityTypes.Add(entityType);
	}

	protected virtual void ValidateDiscriminatorValues([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IEntityType rootEntityType in model.GetRootEntityTypes())
		{
			ValidateDiscriminatorValues(rootEntityType);
		}
	}

	private static void ValidateDiscriminatorValues(IEntityType rootEntityType)
	{
		Dictionary<object, IEntityType> dictionary = new Dictionary<object, IEntityType>();
		List<IEntityType> list = rootEntityType.GetDerivedTypesInclusive().ToList();
		if (list.Count == 1)
		{
			return;
		}
		if (rootEntityType.GetDiscriminatorProperty() == null)
		{
			throw new InvalidOperationException(CoreStrings.NoDiscriminatorProperty(rootEntityType.DisplayName()));
		}
		foreach (IEntityType item in list)
		{
			Type clrType = item.ClrType;
			if ((object)clrType != null && clrType.IsInstantiable())
			{
				object discriminatorValue = item.GetDiscriminatorValue();
				if (discriminatorValue == null)
				{
					throw new InvalidOperationException(CoreStrings.NoDiscriminatorValue(item.DisplayName()));
				}
				if (dictionary.TryGetValue(discriminatorValue, out var value))
				{
					throw new InvalidOperationException(CoreStrings.DuplicateDiscriminatorValue(item.DisplayName(), discriminatorValue, value.DisplayName()));
				}
				dictionary[discriminatorValue] = item;
			}
		}
	}

	protected virtual void ValidateChangeTrackingStrategy([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			string text = entityType.AsEntityType("ValidateChangeTrackingStrategy").CheckChangeTrackingStrategy(entityType.GetChangeTrackingStrategy());
			if (text != null)
			{
				throw new InvalidOperationException(text);
			}
		}
	}

	protected virtual void ValidateOwnership([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			List<IForeignKey> list = (from fk in entityType.GetForeignKeys()
				where fk.IsOwnership
				select fk).ToList();
			if (list.Count > 1)
			{
				throw new InvalidOperationException(CoreStrings.MultipleOwnerships(entityType.DisplayName()));
			}
			if (list.Count == 1)
			{
				IForeignKey foreignKey = list[0];
				if (entityType.BaseType != null && foreignKey.DeclaringEntityType == entityType)
				{
					throw new InvalidOperationException(CoreStrings.OwnedDerivedType(entityType.DisplayName()));
				}
				using (IEnumerator<IForeignKey> enumerator2 = (from fk in entityType.GetReferencingForeignKeys()
					where !fk.IsOwnership && !Contains(fk.DeclaringEntityType.FindOwnership(), fk)
					select fk).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						IForeignKey current2 = enumerator2.Current;
						throw new InvalidOperationException(CoreStrings.PrincipalOwnedType(current2.DeclaringEntityType.DisplayName() + ((current2.DependentToPrincipal == null) ? "" : ("." + current2.DependentToPrincipal.Name)), current2.PrincipalEntityType.DisplayName() + ((current2.PrincipalToDependent == null) ? "" : ("." + current2.PrincipalToDependent.Name)), entityType.DisplayName()));
					}
				}
				using IEnumerator<IForeignKey> enumerator2 = (from fk in entityType.GetDeclaredForeignKeys()
					where !fk.IsOwnership && fk.PrincipalToDependent != null && !Contains(fk.DeclaringEntityType.FindOwnership(), fk)
					select fk).GetEnumerator();
				if (enumerator2.MoveNext())
				{
					IForeignKey current3 = enumerator2.Current;
					throw new InvalidOperationException(CoreStrings.InverseToOwnedType(current3.PrincipalEntityType.DisplayName(), current3.PrincipalToDependent.Name, entityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
				}
			}
			else if (entityType.HasClrType() && ((IMutableModel)model).IsOwned(entityType.ClrType))
			{
				throw new InvalidOperationException(CoreStrings.OwnerlessOwnedType(entityType.DisplayName()));
			}
		}
	}

	private bool Contains(IForeignKey inheritedFk, IForeignKey derivedFk)
	{
		if (inheritedFk != null && inheritedFk.PrincipalEntityType.IsAssignableFrom(derivedFk.PrincipalEntityType))
		{
			return PropertyListComparer.Instance.Equals(inheritedFk.Properties, derivedFk.Properties);
		}
		return false;
	}

	protected virtual void ValidateForeignKeys([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			foreach (IForeignKey declaredForeignKey in entityType.GetDeclaredForeignKeys())
			{
				if (declaredForeignKey.PrincipalEntityType == declaredForeignKey.DeclaringEntityType && PropertyListComparer.Instance.Equals(declaredForeignKey.PrincipalKey.Properties, declaredForeignKey.Properties))
				{
					logger.RedundantForeignKeyWarning(declaredForeignKey);
				}
				if (entityType.BaseType == null)
				{
					continue;
				}
				IKey inheritedKey = declaredForeignKey.Properties.Where((IProperty p) => p.ValueGenerated != ValueGenerated.Never).SelectMany((IProperty p) => from k in p.GetContainingKeys()
					where k.DeclaringEntityType != entityType
					select k).FirstOrDefault();
				if (inheritedKey != null)
				{
					IProperty generatedProperty = declaredForeignKey.Properties.First((IProperty p) => p.ValueGenerated != 0 && inheritedKey.Properties.Contains(p));
					if (!entityType.BaseType.ClrType.IsAbstract || !entityType.BaseType.GetDerivedTypes().All((IEntityType d) => d.GetDeclaredForeignKeys().Any((IForeignKey fk) => fk.Properties.Contains(generatedProperty))))
					{
						throw new InvalidOperationException(CoreStrings.ForeignKeyPropertyInKey(generatedProperty.Name, entityType.DisplayName(), inheritedKey.Properties.Format(), inheritedKey.DeclaringEntityType.DisplayName()));
					}
				}
			}
		}
	}

	protected virtual void ValidateDefiningNavigations([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			if (entityType.DefiningEntityType == null)
			{
				continue;
			}
			if (entityType.FindDefiningNavigation() == null || (entityType.DefiningEntityType as EntityType)?.Builder == null)
			{
				throw new InvalidOperationException(CoreStrings.NoDefiningNavigation(entityType.DefiningNavigationName, entityType.DisplayName(), entityType.DefiningEntityType.DisplayName()));
			}
			IForeignKey foreignKey = entityType.GetForeignKeys().SingleOrDefault((IForeignKey fk) => fk.IsOwnership);
			if (foreignKey == null)
			{
				continue;
			}
			if (foreignKey.PrincipalToDependent?.Name != entityType.DefiningNavigationName)
			{
				string text = ((foreignKey.PrincipalToDependent == null) ? "" : ("." + foreignKey.PrincipalToDependent.Name));
				throw new InvalidOperationException(CoreStrings.NonDefiningOwnership(foreignKey.PrincipalEntityType.DisplayName() + text, entityType.DefiningNavigationName, entityType.DisplayName()));
			}
			foreach (IEntityType item in from et in model.GetEntityTypes()
				where et.ClrType == entityType.ClrType && et != entityType
				select et)
			{
				if (!item.GetForeignKeys().Any((IForeignKey fk) => fk.IsOwnership))
				{
					throw new InvalidOperationException(CoreStrings.InconsistentOwnership(entityType.DisplayName(), item.DisplayName()));
				}
			}
		}
	}

	protected virtual void ValidateFieldMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			HashSet<IPropertyBase> hashSet = new HashSet<IPropertyBase>(from p in entityType.GetDeclaredProperties().Cast<IPropertyBase>().Concat(entityType.GetDeclaredNavigations())
				where !p.IsShadowProperty()
				select p);
			InstantiationBinding instantiationBinding = (InstantiationBinding)entityType["ConstructorBinding"];
			if (instantiationBinding != null)
			{
				foreach (IPropertyBase item in instantiationBinding.ParameterBindings.SelectMany((ParameterBinding p) => p.ConsumedProperties))
				{
					hashSet.Remove(item);
				}
			}
			foreach (IPropertyBase item2 in hashSet)
			{
				if (!item2.TryGetMemberInfo(forConstruction: true, forSet: true, out var memberInfo, out var errorMessage))
				{
					throw new InvalidOperationException(errorMessage);
				}
				if (!item2.TryGetMemberInfo(forConstruction: false, forSet: true, out memberInfo, out errorMessage))
				{
					throw new InvalidOperationException(errorMessage);
				}
				if (!item2.TryGetMemberInfo(forConstruction: false, forSet: false, out memberInfo, out errorMessage))
				{
					throw new InvalidOperationException(errorMessage);
				}
			}
		}
	}

	protected virtual void ValidateTypeMappings([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(logger, "logger");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			foreach (IProperty declaredProperty in entityType.GetDeclaredProperties())
			{
				ValueConverter valueConverter = declaredProperty.GetValueConverter();
				if (valueConverter != null && declaredProperty.GetValueComparer() == null)
				{
					Type modelClrType = valueConverter.ModelClrType;
					if (modelClrType != typeof(string) && (!(modelClrType == typeof(byte[])) || !declaredProperty.IsKey()) && modelClrType.TryGetSequenceType() != null)
					{
						logger.CollectionWithoutComparer(declaredProperty);
					}
				}
			}
		}
	}

	protected virtual void ValidateKeylessTypes([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			if (entityType.GetDefiningQuery() != null)
			{
				if (entityType.BaseType != null)
				{
					throw new InvalidOperationException(CoreStrings.DerivedTypeDefiningQuery(entityType.DisplayName(), entityType.BaseType.DisplayName()));
				}
				if (entityType.FindPrimaryKey() != null)
				{
					throw new InvalidOperationException(CoreStrings.DefiningQueryWithKey(entityType.DisplayName()));
				}
			}
		}
	}

	protected virtual void ValidateQueryFilters([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			if (entityType.GetQueryFilter() != null && entityType.BaseType != null)
			{
				throw new InvalidOperationException(CoreStrings.BadFilterDerivedType(entityType.GetQueryFilter(), entityType.DisplayName()));
			}
		}
	}

	protected virtual void ValidateDefiningQuery([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			if (entityType.GetDefiningQuery() != null && entityType.FindPrimaryKey() != null)
			{
				throw new InvalidOperationException(CoreStrings.DefiningQueryWithKey(entityType.DisplayName()));
			}
		}
	}

	protected virtual void ValidateData([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Dictionary<IKey, IIdentityMap> dictionary = new Dictionary<IKey, IIdentityMap>();
		bool flag = logger.ShouldLogSensitiveData();
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			IKey key = entityType.FindPrimaryKey();
			if (key == null)
			{
				continue;
			}
			IIdentityMap value = null;
			foreach (IDictionary<string, object> seedDatum in entityType.GetSeedData())
			{
				foreach (IProperty property in entityType.GetProperties())
				{
					if (!seedDatum.TryGetValue(property.Name, out var value2) || value2 == null)
					{
						if (!property.IsNullable && ((!property.RequiresValueGenerator() && (property.ValueGenerated & ValueGenerated.OnAdd) == 0) || property.IsPrimaryKey()))
						{
							throw new InvalidOperationException(CoreStrings.SeedDatumMissingValue(entityType.DisplayName(), property.Name));
						}
						continue;
					}
					if (property.RequiresValueGenerator() && property.IsPrimaryKey() && property.ClrType.IsDefaultValue(value2))
					{
						if (property.ClrType.IsSignedInteger())
						{
							throw new InvalidOperationException(CoreStrings.SeedDatumSignedNumericValue(entityType.DisplayName(), property.Name));
						}
						throw new InvalidOperationException(CoreStrings.SeedDatumDefaultValue(entityType.DisplayName(), property.Name, property.ClrType.GetDefaultValue()));
					}
					if (!property.ClrType.GetTypeInfo().IsAssignableFrom(value2.GetType().GetTypeInfo()))
					{
						if (flag)
						{
							throw new InvalidOperationException(CoreStrings.SeedDatumIncompatibleValueSensitive(entityType.DisplayName(), value2, property.Name, property.ClrType.DisplayName()));
						}
						throw new InvalidOperationException(CoreStrings.SeedDatumIncompatibleValue(entityType.DisplayName(), property.Name, property.ClrType.DisplayName()));
					}
				}
				object[] keyValues = new object[key.Properties.Count];
				for (int j = 0; j < key.Properties.Count; j++)
				{
					keyValues[j] = seedDatum[key.Properties[j].Name];
				}
				foreach (INavigation navigation in entityType.GetNavigations())
				{
					if (!seedDatum.TryGetValue(navigation.Name, out var value3) || ((!navigation.IsCollection() || !(value3 is IEnumerable source) || !source.Any()) && (navigation.IsCollection() || value3 == null)))
					{
						continue;
					}
					if (flag)
					{
						throw new InvalidOperationException(CoreStrings.SeedDatumNavigationSensitive(entityType.DisplayName(), string.Join(", ", key.Properties.Select((IProperty p, int i) => p.Name + ":" + keyValues[i])), navigation.Name, navigation.GetTargetType().DisplayName(), navigation.ForeignKey.Properties.Format()));
					}
					throw new InvalidOperationException(CoreStrings.SeedDatumNavigation(entityType.DisplayName(), navigation.Name, navigation.GetTargetType().DisplayName(), navigation.ForeignKey.Properties.Format()));
				}
				if (value == null && !dictionary.TryGetValue(key, out value))
				{
					value = (dictionary[key] = key.GetIdentityMapFactory()(flag));
				}
				InternalEntityEntry internalEntityEntry = value.TryGetEntry(keyValues);
				if (internalEntityEntry != null)
				{
					if (flag)
					{
						throw new InvalidOperationException(CoreStrings.SeedDatumDuplicateSensitive(entityType.DisplayName(), string.Join(", ", key.Properties.Select((IProperty p, int i) => p.Name + ":" + keyValues[i]))));
					}
					throw new InvalidOperationException(CoreStrings.SeedDatumDuplicate(entityType.DisplayName(), key.Properties.Format()));
				}
				internalEntityEntry = new InternalShadowEntityEntry(null, entityType);
				value.Add(keyValues, internalEntityEntry);
			}
		}
	}

	protected virtual void LogShadowProperties([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IEntityType item in from t in model.GetEntityTypes()
			where t.ClrType != null
			select t)
		{
			foreach (IProperty declaredProperty in item.GetDeclaredProperties())
			{
				if (declaredProperty.IsShadowProperty())
				{
					logger.ShadowPropertyCreated(declaredProperty);
				}
			}
		}
	}
}
