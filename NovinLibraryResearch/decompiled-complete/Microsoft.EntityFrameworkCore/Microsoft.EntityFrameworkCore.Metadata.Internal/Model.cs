using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Model : ConventionAnnotatable, IMutableModel, IModel, IAnnotatable, IMutableAnnotatable, IConventionModel, IConventionAnnotatable
{
	private readonly SortedDictionary<string, EntityType> _entityTypes = new SortedDictionary<string, EntityType>(StringComparer.Ordinal);

	private readonly ConcurrentDictionary<Type, string> _clrTypeNameMap = new ConcurrentDictionary<Type, string>();

	private readonly SortedDictionary<string, SortedSet<EntityType>> _entityTypesWithDefiningNavigation = new SortedDictionary<string, SortedSet<EntityType>>(StringComparer.Ordinal);

	private readonly SortedDictionary<string, List<(string, string)>> _detachedEntityTypesWithDefiningNavigation = new SortedDictionary<string, List<(string, string)>>(StringComparer.Ordinal);

	private readonly Dictionary<string, ConfigurationSource> _ignoredTypeNames = new Dictionary<string, ConfigurationSource>(StringComparer.Ordinal);

	public virtual ConventionDispatcher ConventionDispatcher
	{
		[DebuggerStepThrough]
		get;
		private set; }

	public virtual bool IsReadonly => ConventionDispatcher == null;

	public virtual InternalModelBuilder Builder
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual DebugView<Model> DebugView => new DebugView<Model>(this, (Model m) => m.ToDebugString());

	IConventionModelBuilder IConventionModel.Builder
	{
		[DebuggerStepThrough]
		get
		{
			return Builder;
		}
	}

	public Model()
		: this(new ConventionSet())
	{
	}

	public Model([JetBrains.Annotations.NotNull] ConventionSet conventions)
	{
		ConventionDispatcher conventionDispatcher = new ConventionDispatcher(conventions);
		InternalModelBuilder modelBuilder = new InternalModelBuilder(this);
		ConventionDispatcher = conventionDispatcher;
		Builder = modelBuilder;
		conventionDispatcher.OnModelInitialized(modelBuilder);
	}

	public virtual IEnumerable<EntityType> GetEntityTypes()
	{
		return _entityTypes.Values.Concat(_entityTypesWithDefiningNavigation.Values.SelectMany((SortedSet<EntityType> e) => e));
	}

	public virtual EntityType AddEntityType([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		EntityType entityType = new EntityType(name, this, configurationSource);
		return AddEntityType(entityType);
	}

	public virtual EntityType AddEntityType([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		EntityType entityType = new EntityType(type, this, configurationSource);
		return AddEntityType(entityType);
	}

	private EntityType AddEntityType(EntityType entityType)
	{
		string name = entityType.Name;
		if (entityType.HasDefiningNavigation())
		{
			if (_entityTypes.ContainsKey(name))
			{
				throw new InvalidOperationException(CoreStrings.ClashingNonWeakEntityType(entityType.DisplayName()));
			}
			if (_detachedEntityTypesWithDefiningNavigation.TryGetValue(name, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					var (text, text2) = value[i];
					if (text == entityType.DefiningNavigationName && text2 == entityType.DefiningEntityType.Name)
					{
						value.RemoveAt(i);
						break;
					}
				}
			}
			if (!_entityTypesWithDefiningNavigation.TryGetValue(name, out var value2))
			{
				value2 = new SortedSet<EntityType>(EntityTypePathComparer.Instance);
				_entityTypesWithDefiningNavigation[name] = value2;
			}
			value2.Add(entityType);
		}
		else
		{
			if (_entityTypesWithDefiningNavigation.ContainsKey(name))
			{
				throw new InvalidOperationException(CoreStrings.ClashingWeakEntityType(entityType.DisplayName()));
			}
			_detachedEntityTypesWithDefiningNavigation.Remove(name);
			if (_entityTypes.ContainsKey(name))
			{
				throw new InvalidOperationException(CoreStrings.DuplicateEntityType(entityType.DisplayName()));
			}
			_entityTypes.Add(name, entityType);
		}
		return (EntityType)(ConventionDispatcher.OnEntityTypeAdded(entityType.Builder)?.Metadata);
	}

	public virtual EntityType FindEntityType([JetBrains.Annotations.NotNull] Type type)
	{
		return FindEntityType(GetDisplayName(type));
	}

	public virtual EntityType FindEntityType([JetBrains.Annotations.NotNull] string name)
	{
		if (!_entityTypes.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual EntityType RemoveEntityType([JetBrains.Annotations.NotNull] Type type)
	{
		return RemoveEntityType(FindEntityType(type));
	}

	public virtual EntityType RemoveEntityType([JetBrains.Annotations.NotNull] string name)
	{
		return RemoveEntityType(FindEntityType(name));
	}

	private static void AssertCanRemove(EntityType entityType)
	{
		ForeignKey foreignKey = entityType.GetDeclaredForeignKeys().FirstOrDefault((ForeignKey fk) => fk.PrincipalEntityType != entityType);
		if (foreignKey != null)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeInUseByForeignKey(entityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName(), foreignKey.Properties.Format()));
		}
		ForeignKey foreignKey2 = entityType.GetDeclaredReferencingForeignKeys().FirstOrDefault();
		if (foreignKey2 != null)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeInUseByReferencingForeignKey(entityType.DisplayName(), foreignKey2.Properties.Format(), foreignKey2.DeclaringEntityType.DisplayName()));
		}
		EntityType entityType2 = entityType.GetDirectlyDerivedTypes().FirstOrDefault();
		if (entityType2 != null)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeInUseByDerived(entityType.DisplayName(), entityType2.DisplayName()));
		}
	}

	public virtual EntityType RemoveEntityType([JetBrains.Annotations.CanBeNull] EntityType entityType)
	{
		if (entityType?.Builder == null)
		{
			return null;
		}
		AssertCanRemove(entityType);
		string name = entityType.Name;
		if (entityType.HasDefiningNavigation())
		{
			if (!_entityTypesWithDefiningNavigation.TryGetValue(name, out var value))
			{
				return null;
			}
			value.Remove(entityType);
			if (value.Count == 0)
			{
				_entityTypesWithDefiningNavigation.Remove(name);
			}
		}
		else
		{
			_entityTypes.Remove(name);
		}
		entityType.OnTypeRemoved();
		return entityType;
	}

	public virtual EntityType AddEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		EntityType entityType = new EntityType(name, this, definingNavigationName, definingEntityType, configurationSource);
		return AddEntityType(entityType);
	}

	public virtual EntityType AddEntityType([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		EntityType entityType = new EntityType(type, this, definingNavigationName, definingEntityType, configurationSource);
		return AddEntityType(entityType);
	}

	public virtual void AddDetachedEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] string definingEntityTypeName)
	{
		if (!_detachedEntityTypesWithDefiningNavigation.TryGetValue(name, out var value))
		{
			value = new List<(string, string)>();
			_detachedEntityTypesWithDefiningNavigation[name] = value;
		}
		value.Add((definingNavigationName, definingEntityTypeName));
	}

	[DebuggerStepThrough]
	public virtual string GetDisplayName(Type type)
	{
		return _clrTypeNameMap.GetOrAdd(type, (Type t) => t.DisplayName());
	}

	public virtual bool HasEntityTypeWithDefiningNavigation([JetBrains.Annotations.NotNull] Type clrType)
	{
		return HasEntityTypeWithDefiningNavigation(GetDisplayName(clrType));
	}

	public virtual bool HasEntityTypeWithDefiningNavigation([JetBrains.Annotations.NotNull] string name)
	{
		return _entityTypesWithDefiningNavigation.ContainsKey(name);
	}

	public virtual bool HasOtherEntityTypesWithDefiningNavigation([JetBrains.Annotations.NotNull] EntityType entityType)
	{
		if (!entityType.HasDefiningNavigation())
		{
			return false;
		}
		if (_entityTypesWithDefiningNavigation.TryGetValue(entityType.Name, out var value) && value.Any((EntityType e) => EntityTypePathComparer.Instance.Compare(e, entityType) != 0))
		{
			return true;
		}
		if (_detachedEntityTypesWithDefiningNavigation.TryGetValue(entityType.Name, out var value2) && value2.Any(((string, string) e) => e.Item1 != entityType.DefiningNavigationName || e.Item2 != entityType.DefiningEntityType.Name))
		{
			return true;
		}
		return false;
	}

	public virtual bool EntityTypeShouldHaveDefiningNavigation([JetBrains.Annotations.NotNull] Type clrType)
	{
		TypeIdentity type = new TypeIdentity(clrType, this);
		return EntityTypeShouldHaveDefiningNavigation(in type);
	}

	public virtual bool EntityTypeShouldHaveDefiningNavigation([JetBrains.Annotations.NotNull] string name)
	{
		TypeIdentity type = new TypeIdentity(name);
		return EntityTypeShouldHaveDefiningNavigation(in type);
	}

	private bool EntityTypeShouldHaveDefiningNavigation(in TypeIdentity type)
	{
		if (!_entityTypesWithDefiningNavigation.ContainsKey(type.Name))
		{
			if (_detachedEntityTypesWithDefiningNavigation.TryGetValue(type.Name, out var value))
			{
				return value.Count > 1;
			}
			return false;
		}
		return true;
	}

	public virtual EntityType FindEntityType([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType)
	{
		return FindEntityType(GetDisplayName(type), definingNavigationName, definingEntityType);
	}

	public virtual EntityType FindEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType)
	{
		if (_entityTypesWithDefiningNavigation.TryGetValue(name, out var value))
		{
			return value.FirstOrDefault((EntityType e) => e.DefiningNavigationName == definingNavigationName && e.DefiningEntityType == definingEntityType);
		}
		return null;
	}

	public virtual EntityType FindActualEntityType([JetBrains.Annotations.NotNull] EntityType entityType)
	{
		if (entityType.Builder == null)
		{
			if (!entityType.HasDefiningNavigation())
			{
				return FindEntityType(entityType.Name);
			}
			return FindActualEntityType(entityType.DefiningEntityType)?.FindNavigation(entityType.DefiningNavigationName)?.GetTargetType();
		}
		return entityType;
	}

	public virtual Type FindClrType([JetBrains.Annotations.NotNull] string name)
	{
		if (!_entityTypes.TryGetValue(name, out var value))
		{
			if (!_entityTypesWithDefiningNavigation.TryGetValue(name, out var value2))
			{
				return null;
			}
			return value2.FirstOrDefault()?.ClrType;
		}
		return value.ClrType;
	}

	public virtual IReadOnlyCollection<EntityType> GetEntityTypes([JetBrains.Annotations.NotNull] Type type)
	{
		return GetEntityTypes(GetDisplayName(type));
	}

	public virtual IReadOnlyCollection<EntityType> GetEntityTypes([JetBrains.Annotations.NotNull] string name)
	{
		if (_entityTypesWithDefiningNavigation.TryGetValue(name, out var value))
		{
			return value;
		}
		EntityType entityType = FindEntityType(name);
		if (entityType != null)
		{
			return (IReadOnlyCollection<EntityType>)(object)new EntityType[1] { entityType };
		}
		return (IReadOnlyCollection<EntityType>)(object)Array.Empty<EntityType>();
	}

	public virtual IReadOnlyList<EntityType> FindLeastDerivedEntityTypes([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.CanBeNull] Func<EntityType, bool> condition = null)
	{
		Dictionary<TypeInfo, int> derivedLevels = new Dictionary<TypeInfo, int> { [type.GetTypeInfo()] = 0 };
		foreach (IGrouping<int, EntityType> item in from t in GetEntityTypes()
			group t by GetDerivedLevel(t.ClrType.GetTypeInfo(), derivedLevels) into g
			where g.Key != int.MaxValue
			orderby g.Key
			select g)
		{
			if (condition == null)
			{
				return item.ToList();
			}
			List<EntityType> list = item.Where(condition).ToList();
			if (list.Count > 0)
			{
				return list;
			}
		}
		return new List<EntityType>();
	}

	private static int GetDerivedLevel(TypeInfo derivedType, Dictionary<TypeInfo, int> derivedLevels)
	{
		if (derivedType?.BaseType == null)
		{
			return int.MaxValue;
		}
		if (derivedLevels.TryGetValue(derivedType, out var value))
		{
			return value;
		}
		value = GetDerivedLevel(derivedType.BaseType.GetTypeInfo(), derivedLevels);
		value += ((value != int.MaxValue) ? 1 : 0);
		derivedLevels.Add(derivedType, value);
		return value;
	}

	public virtual EntityType RemoveEntityType([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType)
	{
		return RemoveEntityType(FindEntityType(type, definingNavigationName, definingEntityType));
	}

	public virtual EntityType RemoveEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType)
	{
		return RemoveEntityType(FindEntityType(name, definingNavigationName, definingEntityType));
	}

	public virtual void AddIgnored([JetBrains.Annotations.NotNull] Type type, ConfigurationSource configurationSource)
	{
		AddIgnored(GetDisplayName(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type")), type, configurationSource);
	}

	public virtual void AddIgnored([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		AddIgnored(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name"), null, configurationSource);
	}

	private void AddIgnored([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] Type type, ConfigurationSource configurationSource)
	{
		if (_ignoredTypeNames.TryGetValue(name, out var value))
		{
			configurationSource = configurationSource.Max(value);
			_ignoredTypeNames[name] = configurationSource;
		}
		else
		{
			_ignoredTypeNames[name] = configurationSource;
			ConventionDispatcher.OnEntityTypeIgnored(Builder, name, type);
		}
	}

	public virtual ConfigurationSource? FindIgnoredConfigurationSource([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return FindIgnoredConfigurationSource(GetDisplayName(type));
	}

	public virtual ConfigurationSource? FindIgnoredConfigurationSource(string name)
	{
		if (!_ignoredTypeNames.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual bool IsIgnored(string name)
	{
		return FindIgnoredConfigurationSource(name).HasValue;
	}

	public virtual bool IsIgnored([JetBrains.Annotations.NotNull] Type type)
	{
		return FindIgnoredConfigurationSource(GetDisplayName(type)).HasValue;
	}

	public virtual void RemoveIgnored([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		RemoveIgnored(GetDisplayName(type));
	}

	public virtual void RemoveIgnored(string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		_ignoredTypeNames.Remove(name);
	}

	public virtual bool IsOwned([JetBrains.Annotations.NotNull] Type clrType)
	{
		return FindIsOwnedConfigurationSource(clrType).HasValue;
	}

	public virtual ConfigurationSource? FindIsOwnedConfigurationSource([JetBrains.Annotations.NotNull] Type clrType)
	{
		if (!(this["OwnedTypes"] is Dictionary<string, ConfigurationSource> dictionary))
		{
			return null;
		}
		while (clrType != null)
		{
			if (dictionary.TryGetValue(GetDisplayName(clrType), out var value))
			{
				return value;
			}
			clrType = clrType.BaseType;
		}
		return null;
	}

	public virtual void AddOwned([JetBrains.Annotations.NotNull] Type clrType, ConfigurationSource configurationSource)
	{
		string displayName = GetDisplayName(clrType);
		Dictionary<string, ConfigurationSource> dictionary = this["OwnedTypes"] as Dictionary<string, ConfigurationSource>;
		if (dictionary == null)
		{
			dictionary = (Dictionary<string, ConfigurationSource>)(this["OwnedTypes"] = new Dictionary<string, ConfigurationSource>(StringComparer.Ordinal));
		}
		if (dictionary.TryGetValue(displayName, out var value))
		{
			dictionary[displayName] = configurationSource.Max(value);
		}
		else
		{
			dictionary.Add(displayName, configurationSource);
		}
	}

	public virtual void RemoveOwned([JetBrains.Annotations.NotNull] Type clrType)
	{
		if (this["OwnedTypes"] is Dictionary<string, ConfigurationSource> dictionary)
		{
			dictionary.Remove(GetDisplayName(clrType));
		}
	}

	public virtual void SetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("PropertyAccessMode", propertyAccessMode, configurationSource);
	}

	public virtual void SetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("ChangeTrackingStrategy", changeTrackingStrategy, configurationSource);
	}

	protected override IConventionAnnotation OnAnnotationSet(string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
	{
		return ConventionDispatcher.OnModelAnnotationChanged(Builder, name, annotation, oldAnnotation);
	}

	public virtual IModel FinalizeModel()
	{
		return ((Model)(ConventionDispatcher.OnModelFinalized(Builder)?.Metadata))?.MakeReadonly() ?? this;
	}

	public virtual IModel MakeReadonly()
	{
		ConventionDispatcher = null;
		return this;
	}

	IEntityType IModel.FindEntityType(string name)
	{
		return FindEntityType(name);
	}

	IEnumerable<IEntityType> IModel.GetEntityTypes()
	{
		return GetEntityTypes();
	}

	IMutableEntityType IMutableModel.FindEntityType(string name)
	{
		return FindEntityType(name);
	}

	IMutableEntityType IMutableModel.AddEntityType(string name)
	{
		return AddEntityType(name, ConfigurationSource.Explicit);
	}

	IMutableEntityType IMutableModel.AddEntityType(Type type)
	{
		return AddEntityType(type, ConfigurationSource.Explicit);
	}

	void IMutableModel.RemoveEntityType(IMutableEntityType entityType)
	{
		RemoveEntityType((EntityType)entityType);
	}

	IEntityType IModel.FindEntityType(string name, string definingNavigationName, IEntityType definingEntityType)
	{
		return FindEntityType(name, definingNavigationName, (EntityType)definingEntityType);
	}

	IMutableEntityType IMutableModel.FindEntityType(string name, string definingNavigationName, IMutableEntityType definingEntityType)
	{
		return FindEntityType(name, definingNavigationName, (EntityType)definingEntityType);
	}

	IMutableEntityType IMutableModel.AddEntityType(string name, string definingNavigationName, IMutableEntityType definingEntityType)
	{
		return AddEntityType(name, definingNavigationName, (EntityType)definingEntityType, ConfigurationSource.Explicit);
	}

	IMutableEntityType IMutableModel.AddEntityType(Type type, string definingNavigationName, IMutableEntityType definingEntityType)
	{
		return AddEntityType(type, definingNavigationName, (EntityType)definingEntityType, ConfigurationSource.Explicit);
	}

	IEnumerable<IMutableEntityType> IMutableModel.GetEntityTypes()
	{
		return GetEntityTypes();
	}

	void IMutableModel.AddIgnored(string name)
	{
		AddIgnored(name, ConfigurationSource.Explicit);
	}

	IConventionEntityType IConventionModel.FindEntityType(string name)
	{
		return FindEntityType(name);
	}

	IConventionEntityType IConventionModel.FindEntityType(string name, string definingNavigationName, IConventionEntityType definingEntityType)
	{
		return FindEntityType(name, definingNavigationName, (EntityType)definingEntityType);
	}

	IConventionEntityType IConventionModel.AddEntityType(string name, bool fromDataAnnotation)
	{
		return AddEntityType(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityType IConventionModel.AddEntityType(Type clrType, bool fromDataAnnotation)
	{
		return AddEntityType(clrType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityType IConventionModel.AddEntityType(string name, string definingNavigationName, IConventionEntityType definingEntityType, bool fromDataAnnotation)
	{
		return AddEntityType(name, definingNavigationName, (EntityType)definingEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionEntityType IConventionModel.AddEntityType(Type clrType, string definingNavigationName, IConventionEntityType definingEntityType, bool fromDataAnnotation)
	{
		return AddEntityType(clrType, definingNavigationName, (EntityType)definingEntityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionModel.RemoveEntityType(IConventionEntityType entityType)
	{
		RemoveEntityType((EntityType)entityType);
	}

	IEnumerable<IConventionEntityType> IConventionModel.GetEntityTypes()
	{
		return GetEntityTypes();
	}

	void IConventionModel.AddIgnored(string name, bool fromDataAnnotation)
	{
		AddIgnored(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
