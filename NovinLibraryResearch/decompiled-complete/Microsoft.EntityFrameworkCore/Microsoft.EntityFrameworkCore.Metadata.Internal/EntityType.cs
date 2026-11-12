using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class EntityType : TypeBase, IMutableEntityType, IEntityType, ITypeBase, IAnnotatable, IMutableTypeBase, IMutableAnnotatable, IConventionEntityType, IConventionTypeBase, IConventionAnnotatable
{
	private class PropertyComparer : IComparer<string>
	{
		private readonly EntityType _entityType;

		public PropertyComparer(EntityType entityType)
		{
			_entityType = entityType;
		}

		public int Compare(string x, string y)
		{
			int num = -1;
			int num2 = -1;
			IReadOnlyList<Property> readOnlyList = _entityType.FindPrimaryKey()?.Properties;
			if (readOnlyList != null)
			{
				for (int i = 0; i < readOnlyList.Count; i++)
				{
					string name = readOnlyList[i].Name;
					if (name == x)
					{
						num = i;
					}
					if (name == y)
					{
						num2 = i;
					}
				}
			}
			if (num == -1 && num2 == -1)
			{
				return StringComparer.Ordinal.Compare(x, y);
			}
			if (num > -1 && num2 > -1)
			{
				return num - num2;
			}
			if (num <= num2)
			{
				return 1;
			}
			return -1;
		}
	}

	public class Snapshot
	{
		private EntityType EntityType
		{
			[DebuggerStepThrough]
			get;
		}

		private PropertiesSnapshot Properties
		{
			[DebuggerStepThrough]
			get;
		}

		public Snapshot([JetBrains.Annotations.NotNull] EntityType entityType, [JetBrains.Annotations.CanBeNull] PropertiesSnapshot properties, [JetBrains.Annotations.CanBeNull] List<InternalIndexBuilder> indexes, [JetBrains.Annotations.CanBeNull] List<(InternalKeyBuilder, ConfigurationSource?)> keys, [JetBrains.Annotations.CanBeNull] List<RelationshipSnapshot> relationships)
		{
			EntityType = entityType;
			Properties = properties ?? new PropertiesSnapshot(null, null, null, null);
			if (indexes != null)
			{
				Properties.Add(indexes);
			}
			if (keys != null)
			{
				Properties.Add(keys);
			}
			if (relationships != null)
			{
				Properties.Add(relationships);
			}
		}

		public virtual void Attach([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder)
		{
			entityTypeBuilder.MergeAnnotationsFrom(EntityType);
			foreach (string ignoredMember in EntityType.GetIgnoredMembers())
			{
				entityTypeBuilder.Ignore(ignoredMember, EntityType.FindDeclaredIgnoredConfigurationSource(ignoredMember).Value);
			}
			Properties.Attach(entityTypeBuilder);
			List<object> data = EntityType._data;
			if (data != null)
			{
				entityTypeBuilder.Metadata.AddData(Enumerable.ToArray(data));
			}
		}
	}

	private readonly SortedSet<ForeignKey> _foreignKeys = new SortedSet<ForeignKey>(ForeignKeyComparer.Instance);

	private readonly SortedDictionary<string, Navigation> _navigations = new SortedDictionary<string, Navigation>(StringComparer.Ordinal);

	private readonly SortedDictionary<IReadOnlyList<IProperty>, Index> _indexes = new SortedDictionary<IReadOnlyList<IProperty>, Index>(PropertyListComparer.Instance);

	private readonly SortedDictionary<string, Property> _properties;

	private readonly SortedDictionary<IReadOnlyList<IProperty>, Key> _keys = new SortedDictionary<IReadOnlyList<IProperty>, Key>(PropertyListComparer.Instance);

	private readonly SortedDictionary<string, ServiceProperty> _serviceProperties = new SortedDictionary<string, ServiceProperty>(StringComparer.Ordinal);

	private List<object> _data;

	private Key _primaryKey;

	private bool? _isKeyless;

	private EntityType _baseType;

	private ConfigurationSource? _primaryKeyConfigurationSource;

	private ConfigurationSource? _isKeylessConfigurationSource;

	private ConfigurationSource? _baseTypeConfigurationSource;

	private PropertyCounts _counts;

	private Func<InternalEntityEntry, ISnapshot> _relationshipSnapshotFactory;

	private Func<InternalEntityEntry, ISnapshot> _originalValuesFactory;

	private Func<InternalEntityEntry, ISnapshot> _temporaryValuesFactory;

	private Func<InternalEntityEntry, ISnapshot> _storeGeneratedValuesFactory;

	private Func<ValueBuffer, ISnapshot> _shadowValuesFactory;

	private Func<ISnapshot> _emptyShadowValuesFactory;

	private readonly SortedSet<EntityType> _directlyDerivedTypes = new SortedSet<EntityType>(EntityTypePathComparer.Instance);

	public virtual InternalEntityTypeBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual EntityType BaseType => _baseType;

	public virtual bool IsKeyless
	{
		get
		{
			return RootType()._isKeyless.GetValueOrDefault();
		}
		set
		{
			HasNoKey(value, ConfigurationSource.Explicit);
		}
	}

	public virtual string DefiningNavigationName { get; }

	public virtual EntityType DefiningEntityType { get; }

	private SortedSet<ForeignKey> DeclaredReferencingForeignKeys { get; set; }

	public virtual PropertyCounts Counts => NonCapturingLazyInitializer.EnsureInitialized(ref _counts, this, (EntityType entityType) => entityType.CalculateCounts());

	public virtual Func<InternalEntityEntry, ISnapshot> RelationshipSnapshotFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _relationshipSnapshotFactory, this, (EntityType entityType) => new RelationshipSnapshotFactoryFactory().Create(entityType));

	public virtual Func<InternalEntityEntry, ISnapshot> OriginalValuesFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _originalValuesFactory, this, (EntityType entityType) => new OriginalValuesFactoryFactory().Create(entityType));

	public virtual Func<InternalEntityEntry, ISnapshot> StoreGeneratedValuesFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _storeGeneratedValuesFactory, this, (EntityType entityType) => new SidecarValuesFactoryFactory().Create(entityType));

	public virtual Func<InternalEntityEntry, ISnapshot> TemporaryValuesFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _temporaryValuesFactory, this, (EntityType entityType) => new TemporaryValuesFactoryFactory().Create(entityType));

	public virtual Func<ValueBuffer, ISnapshot> ShadowValuesFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _shadowValuesFactory, this, (EntityType entityType) => new ShadowValuesFactoryFactory().Create(entityType));

	public virtual Func<ISnapshot> EmptyShadowValuesFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _emptyShadowValuesFactory, this, (EntityType entityType) => new EmptyShadowValuesFactoryFactory().CreateEmpty(entityType));

	IModel ITypeBase.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	IMutableModel IMutableTypeBase.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	IMutableModel IMutableEntityType.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	IEntityType IEntityType.BaseType
	{
		[DebuggerStepThrough]
		get
		{
			return _baseType;
		}
	}

	IMutableEntityType IMutableEntityType.BaseType
	{
		get
		{
			return _baseType;
		}
		set
		{
			HasBaseType((EntityType)value, ConfigurationSource.Explicit);
		}
	}

	IEntityType IEntityType.DefiningEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DefiningEntityType;
		}
	}

	IMutableEntityType IMutableEntityType.DefiningEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DefiningEntityType;
		}
	}

	IConventionEntityType IConventionEntityType.DefiningEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DefiningEntityType;
		}
	}

	IConventionEntityTypeBuilder IConventionEntityType.Builder
	{
		[DebuggerStepThrough]
		get
		{
			return Builder;
		}
	}

	IConventionModel IConventionEntityType.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	IConventionEntityType IConventionEntityType.BaseType
	{
		[DebuggerStepThrough]
		get
		{
			return BaseType;
		}
	}

	public virtual DebugView<EntityType> DebugView => new DebugView<EntityType>(this, (EntityType m) => m.ToDebugString(singleLine: false));

	public EntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Model model, ConfigurationSource configurationSource)
		: base(name, model, configurationSource)
	{
		_properties = new SortedDictionary<string, Property>(new PropertyComparer(this));
		Builder = new InternalEntityTypeBuilder(this, model.Builder);
	}

	public EntityType([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] Model model, ConfigurationSource configurationSource)
		: base(clrType, model, configurationSource)
	{
		if (!clrType.IsValidEntityType())
		{
			throw new ArgumentException(CoreStrings.InvalidEntityType(clrType));
		}
		_properties = new SortedDictionary<string, Property>(new PropertyComparer(this));
		Builder = new InternalEntityTypeBuilder(this, model.Builder);
	}

	public EntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Model model, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType, ConfigurationSource configurationSource)
		: this(name, model, configurationSource)
	{
		DefiningNavigationName = definingNavigationName;
		DefiningEntityType = definingEntityType;
	}

	public EntityType([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] Model model, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] EntityType definingEntityType, ConfigurationSource configurationSource)
		: this(clrType, model, configurationSource)
	{
		DefiningNavigationName = definingNavigationName;
		DefiningEntityType = definingEntityType;
	}

	public virtual void HasNoKey(bool? keyless, ConfigurationSource configurationSource)
	{
		if (_isKeyless == keyless)
		{
			UpdateIsKeylessConfigurationSource(configurationSource);
			return;
		}
		if (keyless == true)
		{
			if (_baseType != null)
			{
				throw new InvalidOperationException(CoreStrings.DerivedEntityTypeHasNoKey(this.DisplayName(), RootType().DisplayName()));
			}
			if (_keys.Any())
			{
				throw new InvalidOperationException(CoreStrings.KeylessTypeExistingKey(this.DisplayName()));
			}
		}
		_isKeyless = keyless;
		if (!keyless.HasValue)
		{
			_isKeylessConfigurationSource = null;
		}
		else
		{
			UpdateIsKeylessConfigurationSource(configurationSource);
		}
	}

	public virtual ConfigurationSource? GetIsKeylessConfigurationSource()
	{
		return _isKeylessConfigurationSource;
	}

	public virtual void UpdateIsKeylessConfigurationSource(ConfigurationSource configurationSource)
	{
		_isKeylessConfigurationSource = configurationSource.Max(_isKeylessConfigurationSource);
	}

	public virtual void HasBaseType([JetBrains.Annotations.CanBeNull] EntityType newBaseType, ConfigurationSource configurationSource)
	{
		if (_baseType == newBaseType)
		{
			UpdateBaseTypeConfigurationSource(configurationSource);
			newBaseType?.UpdateConfigurationSource(configurationSource);
			return;
		}
		if (this.HasDefiningNavigation())
		{
			throw new InvalidOperationException(CoreStrings.WeakDerivedType(this.DisplayName()));
		}
		EntityType baseType = _baseType;
		_baseType?._directlyDerivedTypes.Remove(this);
		_baseType = null;
		if (newBaseType != null)
		{
			if (this.HasClrType())
			{
				if (!newBaseType.HasClrType())
				{
					throw new InvalidOperationException(CoreStrings.NonClrBaseType(this.DisplayName(), newBaseType.DisplayName()));
				}
				if (!newBaseType.ClrType.GetTypeInfo().IsAssignableFrom(ClrType.GetTypeInfo()))
				{
					throw new InvalidOperationException(CoreStrings.NotAssignableClrBaseType(this.DisplayName(), newBaseType.DisplayName(), ClrType.ShortDisplayName(), newBaseType.ClrType.ShortDisplayName()));
				}
				if (newBaseType.HasDefiningNavigation())
				{
					throw new InvalidOperationException(CoreStrings.WeakBaseType(this.DisplayName(), newBaseType.DisplayName()));
				}
			}
			if (!this.HasClrType() && newBaseType.HasClrType())
			{
				throw new InvalidOperationException(CoreStrings.NonShadowBaseType(this.DisplayName(), newBaseType.DisplayName()));
			}
			if (newBaseType.InheritsFrom(this))
			{
				throw new InvalidOperationException(CoreStrings.CircularInheritance(this.DisplayName(), newBaseType.DisplayName()));
			}
			if (_keys.Count > 0)
			{
				throw new InvalidOperationException(CoreStrings.DerivedEntityCannotHaveKeys(this.DisplayName()));
			}
			if (IsKeyless)
			{
				throw new InvalidOperationException(CoreStrings.DerivedEntityCannotBeKeyless(this.DisplayName()));
			}
			List<Property> list = (from p in newBaseType.GetProperties()
				select p.Name).SelectMany(FindDerivedPropertiesInclusive).ToList();
			if (list.Count > 0)
			{
				Property property = list.First();
				Property property2 = newBaseType.FindProperty(property.Name);
				throw new InvalidOperationException(CoreStrings.DuplicatePropertiesOnBase(this.DisplayName(), newBaseType.DisplayName(), property.DeclaringEntityType.DisplayName(), property.Name, property2.DeclaringEntityType.DisplayName(), property2.Name));
			}
			List<Navigation> list2 = (from p in newBaseType.GetNavigations()
				select p.Name).SelectMany(FindNavigationsInHierarchy).ToList();
			if (list2.Count > 0)
			{
				throw new InvalidOperationException(CoreStrings.DuplicateNavigationsOnBase(this.DisplayName(), newBaseType.DisplayName(), string.Join(", ", list2.Select((Navigation p) => p.Name))));
			}
			_baseType = newBaseType;
			_baseType._directlyDerivedTypes.Add(this);
		}
		UpdateBaseTypeConfigurationSource(configurationSource);
		newBaseType?.UpdateConfigurationSource(configurationSource);
		Model.ConventionDispatcher.OnEntityTypeBaseTypeChanged(Builder, newBaseType, baseType);
	}

	public virtual void OnTypeRemoved()
	{
		Builder = null;
		_baseType?._directlyDerivedTypes.Remove(this);
		Model.ConventionDispatcher.OnEntityTypeRemoved(Model.Builder, this);
	}

	public virtual ConfigurationSource? GetBaseTypeConfigurationSource()
	{
		return _baseTypeConfigurationSource;
	}

	private void UpdateBaseTypeConfigurationSource(ConfigurationSource configurationSource)
	{
		_baseTypeConfigurationSource = configurationSource.Max(_baseTypeConfigurationSource);
	}

	public virtual ISet<EntityType> GetDirectlyDerivedTypes()
	{
		return _directlyDerivedTypes;
	}

	public virtual IEnumerable<EntityType> GetDerivedTypes()
	{
		List<EntityType> list = new List<EntityType>();
		EntityType entityType = this;
		int num = 0;
		while (entityType != null)
		{
			list.AddRange(entityType.GetDirectlyDerivedTypes());
			entityType = ((list.Count > num) ? list[num] : null);
			num++;
		}
		return list;
	}

	public virtual IEnumerable<EntityType> GetDerivedTypesInclusive()
	{
		return new EntityType[1] { this }.Concat(GetDerivedTypes());
	}

	public virtual IEnumerable<ForeignKey> GetForeignKeysInHierarchy()
	{
		return GetForeignKeys().Concat(GetDerivedForeignKeys());
	}

	private bool InheritsFrom(EntityType entityType)
	{
		EntityType entityType2 = this;
		do
		{
			if (entityType == entityType2)
			{
				return true;
			}
		}
		while ((entityType2 = entityType2._baseType) != null);
		return false;
	}

	[DebuggerStepThrough]
	public virtual EntityType RootType()
	{
		return (EntityType)Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetRootType(this);
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}

	protected override IConventionAnnotation OnAnnotationSet(string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
	{
		return Model.ConventionDispatcher.OnEntityTypeAnnotationChanged(Builder, name, annotation, oldAnnotation);
	}

	public virtual IEnumerable<PropertyBase> FindMembersInHierarchy([JetBrains.Annotations.NotNull] string name)
	{
		return FindPropertiesInHierarchy(name).Cast<PropertyBase>().Concat(FindServicePropertiesInHierarchy(name)).Concat(FindNavigationsInHierarchy(name));
	}

	public virtual Key SetPrimaryKey([JetBrains.Annotations.CanBeNull] Property property, ConfigurationSource configurationSource)
	{
		return SetPrimaryKey((property == null) ? null : new Property[1] { property }, configurationSource);
	}

	public virtual Key SetPrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		if (_baseType != null)
		{
			throw new InvalidOperationException(CoreStrings.DerivedEntityTypeKey(this.DisplayName(), RootType().DisplayName()));
		}
		Key primaryKey = _primaryKey;
		bool flag = (properties?.Count ?? 0) == 0;
		if (primaryKey == null && flag)
		{
			return null;
		}
		Key key = null;
		if (!flag)
		{
			key = FindKey(properties);
			if (primaryKey != null && primaryKey == key)
			{
				UpdatePrimaryKeyConfigurationSource(configurationSource);
				key.UpdateConfigurationSource(configurationSource);
				return key;
			}
			if (key == null)
			{
				key = AddKey(properties, configurationSource);
			}
		}
		if (primaryKey != null)
		{
			foreach (Property property in primaryKey.Properties)
			{
				_properties.Remove(property.Name);
				property.PrimaryKey = null;
			}
			_primaryKey = null;
			foreach (Property property2 in primaryKey.Properties)
			{
				_properties.Add(property2.Name, property2);
			}
		}
		if (!flag)
		{
			foreach (Property property3 in key.Properties)
			{
				_properties.Remove(property3.Name);
				property3.PrimaryKey = key;
			}
			_primaryKey = key;
			foreach (Property property4 in key.Properties)
			{
				_properties.Add(property4.Name, property4);
			}
			UpdatePrimaryKeyConfigurationSource(configurationSource);
		}
		else
		{
			SetPrimaryKeyConfigurationSource(null);
		}
		return (Key)Model.ConventionDispatcher.OnPrimaryKeyChanged(Builder, key, primaryKey);
	}

	public virtual Key FindPrimaryKey()
	{
		return _baseType?.FindPrimaryKey() ?? FindDeclaredPrimaryKey();
	}

	public virtual Key FindDeclaredPrimaryKey()
	{
		return _primaryKey;
	}

	public virtual Key FindPrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<Property> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		if (_baseType != null)
		{
			return _baseType.FindPrimaryKey(properties);
		}
		if (_primaryKey == null || PropertyListComparer.Instance.Compare(_primaryKey.Properties, properties) != 0)
		{
			return null;
		}
		return _primaryKey;
	}

	public virtual ConfigurationSource? GetPrimaryKeyConfigurationSource()
	{
		return _primaryKeyConfigurationSource;
	}

	private void SetPrimaryKeyConfigurationSource(ConfigurationSource? configurationSource)
	{
		_primaryKeyConfigurationSource = configurationSource;
	}

	private void UpdatePrimaryKeyConfigurationSource(ConfigurationSource configurationSource)
	{
		_primaryKeyConfigurationSource = configurationSource.Max(_primaryKeyConfigurationSource);
	}

	public virtual Key AddKey([JetBrains.Annotations.NotNull] Property property, ConfigurationSource configurationSource)
	{
		return AddKey(new Property[1] { property }, configurationSource);
	}

	public virtual Key AddKey([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		if (_baseType != null)
		{
			throw new InvalidOperationException(CoreStrings.DerivedEntityTypeKey(this.DisplayName(), _baseType.DisplayName()));
		}
		if (IsKeyless)
		{
			throw new InvalidOperationException(CoreStrings.KeylessTypeWithKey(properties.Format(), this.DisplayName()));
		}
		for (int i = 0; i < properties.Count; i++)
		{
			Property property = properties[i];
			for (int j = i + 1; j < properties.Count; j++)
			{
				if (property == properties[j])
				{
					throw new InvalidOperationException(CoreStrings.DuplicatePropertyInList(properties.Format(), property.Name));
				}
			}
			if (FindProperty(property.Name) != property || property.Builder == null)
			{
				throw new InvalidOperationException(CoreStrings.KeyPropertiesWrongEntity(properties.Format(), this.DisplayName()));
			}
			if (property.ValueGenerated != 0 && property.GetContainingForeignKeys().Any((ForeignKey k) => k.DeclaringEntityType != this))
			{
				throw new InvalidOperationException(CoreStrings.KeyPropertyInForeignKey(property.Name, this.DisplayName()));
			}
			if (property.IsNullable)
			{
				throw new InvalidOperationException(CoreStrings.NullableKey(this.DisplayName(), property.Name));
			}
		}
		Key key = FindKey(properties);
		if (key != null)
		{
			throw new InvalidOperationException(CoreStrings.DuplicateKey(properties.Format(), this.DisplayName(), key.DeclaringEntityType.DisplayName()));
		}
		key = new Key(properties, configurationSource);
		_keys.Add(properties, key);
		foreach (Property property2 in properties)
		{
			if (property2.Keys == null)
			{
				property2.Keys = new List<Key> { key };
			}
			else
			{
				property2.Keys.Add(key);
			}
		}
		return (Key)(Model.ConventionDispatcher.OnKeyAdded(key.Builder)?.Metadata);
	}

	public virtual Key FindKey([JetBrains.Annotations.NotNull] IProperty property)
	{
		return FindKey(new IProperty[1] { property });
	}

	public virtual Key FindKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Key key = FindDeclaredKey(properties);
		if (key == null)
		{
			EntityType baseType = _baseType;
			if (baseType == null)
			{
				return null;
			}
			key = baseType.FindKey(properties);
		}
		return key;
	}

	public virtual IEnumerable<Key> GetDeclaredKeys()
	{
		return _keys.Values;
	}

	public virtual Key FindDeclaredKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		if (!_keys.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual Key RemoveKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Key key = FindDeclaredKey(properties);
		if (key != null)
		{
			return RemoveKey(key);
		}
		return null;
	}

	public virtual Key RemoveKey([JetBrains.Annotations.NotNull] Key key)
	{
		if (key.DeclaringEntityType != this)
		{
			throw new InvalidOperationException(CoreStrings.KeyWrongType(key.Properties.Format(), key.DeclaringEntityType.DisplayName(), this.DisplayName()));
		}
		CheckKeyNotInUse(key);
		if (_primaryKey == key)
		{
			SetPrimaryKey((IReadOnlyList<Property>)null, ConfigurationSource.Explicit);
			_primaryKeyConfigurationSource = null;
		}
		_keys.Remove(key.Properties);
		key.Builder = null;
		foreach (Property property in key.Properties)
		{
			if (property.Keys != null)
			{
				property.Keys.Remove(key);
				if (property.Keys.Count == 0)
				{
					property.Keys = null;
				}
			}
		}
		return (Key)Model.ConventionDispatcher.OnKeyRemoved(Builder, key);
	}

	private void CheckKeyNotInUse(Key key)
	{
		ForeignKey foreignKey = key.GetReferencingForeignKeys().FirstOrDefault();
		if (foreignKey != null)
		{
			throw new InvalidOperationException(CoreStrings.KeyInUse(key.Properties.Format(), this.DisplayName(), foreignKey.DeclaringEntityType.DisplayName()));
		}
	}

	public virtual IEnumerable<Key> GetKeys()
	{
		return _baseType?.GetKeys().Concat(_keys.Values) ?? _keys.Values;
	}

	public virtual ForeignKey AddForeignKey([JetBrains.Annotations.NotNull] Property property, [JetBrains.Annotations.NotNull] Key principalKey, [JetBrains.Annotations.NotNull] EntityType principalEntityType, ConfigurationSource? componentConfigurationSource, ConfigurationSource configurationSource)
	{
		return AddForeignKey(new Property[1] { property }, principalKey, principalEntityType, componentConfigurationSource, configurationSource);
	}

	public virtual ForeignKey AddForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, [JetBrains.Annotations.NotNull] Key principalKey, [JetBrains.Annotations.NotNull] EntityType principalEntityType, ConfigurationSource? componentConfigurationSource, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalKey, "principalKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		ForeignKey foreignKey = new ForeignKey(properties, principalKey, this, principalEntityType, configurationSource);
		principalEntityType.UpdateConfigurationSource(configurationSource);
		if (componentConfigurationSource.HasValue)
		{
			foreignKey.UpdatePropertiesConfigurationSource(componentConfigurationSource.Value);
			foreignKey.UpdatePrincipalKeyConfigurationSource(componentConfigurationSource.Value);
			foreignKey.UpdatePrincipalEndConfigurationSource(componentConfigurationSource.Value);
		}
		OnForeignKeyUpdated(foreignKey);
		return (ForeignKey)(Model.ConventionDispatcher.OnForeignKeyAdded(foreignKey.Builder)?.Metadata);
	}

	public virtual void OnForeignKeyUpdating(ForeignKey foreignKey)
	{
		_foreignKeys.Remove(foreignKey);
		foreach (Property property in foreignKey.Properties)
		{
			if (property.ForeignKeys != null)
			{
				property.ForeignKeys.Remove(foreignKey);
				if (property.ForeignKeys.Count == 0)
				{
					property.ForeignKeys = null;
				}
			}
		}
		foreignKey.PrincipalKey.ReferencingForeignKeys.Remove(foreignKey);
		foreignKey.PrincipalEntityType.DeclaredReferencingForeignKeys.Remove(foreignKey);
	}

	public virtual void OnForeignKeyUpdated(ForeignKey foreignKey)
	{
		_foreignKeys.Add(foreignKey);
		foreach (Property property in foreignKey.Properties)
		{
			if (property.ForeignKeys == null)
			{
				property.ForeignKeys = new List<ForeignKey> { foreignKey };
			}
			else
			{
				property.ForeignKeys.Add(foreignKey);
			}
		}
		Key principalKey = foreignKey.PrincipalKey;
		if (principalKey.ReferencingForeignKeys == null)
		{
			principalKey.ReferencingForeignKeys = new SortedSet<ForeignKey>(ForeignKeyComparer.Instance) { foreignKey };
		}
		else
		{
			principalKey.ReferencingForeignKeys.Add(foreignKey);
		}
		EntityType principalEntityType = foreignKey.PrincipalEntityType;
		if (principalEntityType.DeclaredReferencingForeignKeys == null)
		{
			principalEntityType.DeclaredReferencingForeignKeys = new SortedSet<ForeignKey>(ForeignKeyComparer.Instance) { foreignKey };
		}
		else
		{
			principalEntityType.DeclaredReferencingForeignKeys.Add(foreignKey);
		}
	}

	public virtual IEnumerable<ForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] IProperty property)
	{
		return FindForeignKeys(new IProperty[1] { property });
	}

	public virtual IEnumerable<ForeignKey> FindForeignKeys([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		return _baseType?.FindForeignKeys(properties)?.Concat(FindDeclaredForeignKeys(properties)) ?? FindDeclaredForeignKeys(properties);
	}

	public virtual ForeignKey FindForeignKey([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		return FindForeignKey(new IProperty[1] { property }, principalKey, principalEntityType);
	}

	public virtual ForeignKey FindForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalKey, "principalKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		ForeignKey foreignKey = FindDeclaredForeignKey(properties, principalKey, principalEntityType);
		if (foreignKey == null)
		{
			EntityType baseType = _baseType;
			if (baseType == null)
			{
				return null;
			}
			foreignKey = baseType.FindForeignKey(properties, principalKey, principalEntityType);
		}
		return foreignKey;
	}

	public virtual ForeignKey FindOwnership()
	{
		return GetForeignKeys().FirstOrDefault((ForeignKey fk) => fk.IsOwnership);
	}

	public virtual ForeignKey FindDeclaredOwnership()
	{
		return GetDeclaredForeignKeys().FirstOrDefault((ForeignKey fk) => fk.IsOwnership);
	}

	public virtual IEnumerable<ForeignKey> GetDeclaredForeignKeys()
	{
		return _foreignKeys;
	}

	public virtual IEnumerable<ForeignKey> GetDerivedForeignKeys()
	{
		return GetDerivedTypes().SelectMany((EntityType et) => et.GetDeclaredForeignKeys());
	}

	public virtual IEnumerable<ForeignKey> GetForeignKeys()
	{
		return _baseType?.GetForeignKeys().Concat(_foreignKeys) ?? _foreignKeys;
	}

	public virtual IEnumerable<ForeignKey> FindDeclaredForeignKeys([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		return _foreignKeys.Where((ForeignKey fk) => PropertyListComparer.Instance.Equals(fk.Properties, properties));
	}

	public virtual ForeignKey FindDeclaredForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalKey, "principalKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(principalEntityType, "principalEntityType");
		foreach (ForeignKey item in FindDeclaredForeignKeys(properties))
		{
			if (PropertyListComparer.Instance.Equals(item.PrincipalKey.Properties, principalKey.Properties) && StringComparer.Ordinal.Equals(item.PrincipalEntityType.Name, principalEntityType.Name))
			{
				return item;
			}
		}
		return null;
	}

	public virtual IEnumerable<ForeignKey> FindDerivedForeignKeys([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return GetDerivedTypes().SelectMany((EntityType et) => et.FindDeclaredForeignKeys(properties));
	}

	public virtual IEnumerable<ForeignKey> FindDerivedForeignKeys([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		return from et in GetDerivedTypes()
			select et.FindDeclaredForeignKey(properties, principalKey, principalEntityType) into fk
			where fk != null
			select fk;
	}

	public virtual IEnumerable<ForeignKey> FindForeignKeysInHierarchy([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return FindForeignKeys(properties).Concat(FindDerivedForeignKeys(properties));
	}

	public virtual IEnumerable<ForeignKey> FindForeignKeysInHierarchy([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		return ToEnumerable(FindForeignKey(properties, principalKey, principalEntityType)).Concat(FindDerivedForeignKeys(properties, principalKey, principalEntityType));
	}

	public virtual ForeignKey RemoveForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		ForeignKey foreignKey = FindDeclaredForeignKey(properties, principalKey, principalEntityType);
		if (foreignKey != null)
		{
			return RemoveForeignKey(foreignKey);
		}
		return null;
	}

	public virtual ForeignKey RemoveForeignKey([JetBrains.Annotations.NotNull] ForeignKey foreignKey)
	{
		if (foreignKey.DeclaringEntityType != this)
		{
			throw new InvalidOperationException(CoreStrings.ForeignKeyWrongType(foreignKey.Properties.Format(), foreignKey.PrincipalKey.Properties.Format(), foreignKey.PrincipalEntityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), this.DisplayName()));
		}
		if (foreignKey.DependentToPrincipal != null)
		{
			foreignKey.DeclaringEntityType.RemoveNavigation(foreignKey.DependentToPrincipal.Name);
		}
		if (foreignKey.PrincipalToDependent != null)
		{
			foreignKey.PrincipalEntityType.RemoveNavigation(foreignKey.PrincipalToDependent.Name);
		}
		OnForeignKeyUpdating(foreignKey);
		foreignKey.Builder = null;
		if (foreignKey.DependentToPrincipal != null)
		{
			Model.ConventionDispatcher.OnNavigationRemoved(Builder, foreignKey.PrincipalEntityType.Builder, foreignKey.DependentToPrincipal.Name, foreignKey.DependentToPrincipal.GetIdentifyingMemberInfo());
		}
		if (foreignKey.PrincipalToDependent != null)
		{
			Model.ConventionDispatcher.OnNavigationRemoved(foreignKey.PrincipalEntityType.Builder, Builder, foreignKey.PrincipalToDependent.Name, foreignKey.PrincipalToDependent.GetIdentifyingMemberInfo());
		}
		Model.ConventionDispatcher.OnForeignKeyRemoved(Builder, foreignKey);
		return foreignKey;
	}

	public virtual IEnumerable<ForeignKey> GetReferencingForeignKeys()
	{
		return _baseType?.GetReferencingForeignKeys().Concat(GetDeclaredReferencingForeignKeys()) ?? GetDeclaredReferencingForeignKeys();
	}

	public virtual IEnumerable<ForeignKey> GetDeclaredReferencingForeignKeys()
	{
		IEnumerable<ForeignKey> declaredReferencingForeignKeys = DeclaredReferencingForeignKeys;
		return declaredReferencingForeignKeys ?? Enumerable.Empty<ForeignKey>();
	}

	public virtual IEnumerable<ForeignKey> GetDerivedReferencingForeignKeys()
	{
		return GetDerivedTypes().SelectMany((EntityType et) => et.GetDeclaredReferencingForeignKeys());
	}

	public virtual Navigation AddNavigation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] ForeignKey foreignKey, bool pointsToPrincipal)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		return AddNavigation(new MemberIdentity(name), foreignKey, pointsToPrincipal);
	}

	public virtual Navigation AddNavigation([JetBrains.Annotations.NotNull] MemberInfo navigationProperty, [JetBrains.Annotations.NotNull] ForeignKey foreignKey, bool pointsToPrincipal)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationProperty, "navigationProperty");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		return AddNavigation(new MemberIdentity(navigationProperty), foreignKey, pointsToPrincipal);
	}

	private Navigation AddNavigation(MemberIdentity propertyIdentity, ForeignKey foreignKey, bool pointsToPrincipal)
	{
		string name = propertyIdentity.Name;
		Navigation navigation = FindNavigationsInHierarchy(name).FirstOrDefault();
		if (navigation != null)
		{
			if (navigation.ForeignKey != foreignKey)
			{
				throw new InvalidOperationException(CoreStrings.NavigationForWrongForeignKey(navigation.Name, navigation.DeclaringEntityType.DisplayName(), foreignKey.Properties.Format(), navigation.ForeignKey.Properties.Format()));
			}
			throw new InvalidOperationException(CoreStrings.ConflictingPropertyOrNavigation(name, this.DisplayName(), navigation.DeclaringEntityType.DisplayName()));
		}
		PropertyBase propertyBase = FindPropertiesInHierarchy(name).Cast<PropertyBase>().Concat(FindServicePropertiesInHierarchy(name)).FirstOrDefault();
		if (propertyBase != null)
		{
			throw new InvalidOperationException(CoreStrings.ConflictingPropertyOrNavigation(name, this.DisplayName(), propertyBase.DeclaringType.DisplayName()));
		}
		MemberInfo memberInfo = propertyIdentity.MemberInfo ?? ClrType?.GetMembersInHierarchy(name).FirstOrDefault();
		if (ClrType != null)
		{
			Navigation.IsCompatible(propertyIdentity.Name, memberInfo, this, pointsToPrincipal ? foreignKey.PrincipalEntityType : foreignKey.DeclaringEntityType, !pointsToPrincipal && !foreignKey.IsUnique, shouldThrow: true);
		}
		Navigation navigation2 = new Navigation(name, memberInfo as PropertyInfo, memberInfo as FieldInfo, foreignKey);
		_navigations.Add(name, navigation2);
		return navigation2;
	}

	public virtual Navigation FindNavigation([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Navigation navigation = FindDeclaredNavigation(name);
		if (navigation == null)
		{
			EntityType baseType = _baseType;
			if (baseType == null)
			{
				return null;
			}
			navigation = baseType.FindNavigation(name);
		}
		return navigation;
	}

	public virtual Navigation FindNavigation([JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return FindNavigation(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo").GetSimpleMemberName());
	}

	public virtual Navigation FindDeclaredNavigation([JetBrains.Annotations.NotNull] string name)
	{
		if (!_navigations.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual IEnumerable<Navigation> GetDeclaredNavigations()
	{
		return _navigations.Values;
	}

	public virtual IEnumerable<Navigation> GetDerivedNavigations()
	{
		return GetDerivedTypes().SelectMany((EntityType et) => et.GetDeclaredNavigations());
	}

	public virtual IEnumerable<Navigation> GetDerivedNavigationsInclusive()
	{
		return GetDerivedTypesInclusive().SelectMany((EntityType et) => et.GetDeclaredNavigations());
	}

	public virtual IEnumerable<Navigation> FindDerivedNavigations([JetBrains.Annotations.NotNull] string navigationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(navigationName, "navigationName");
		return from et in GetDerivedTypes()
			select et.FindDeclaredNavigation(navigationName) into n
			where n != null
			select n;
	}

	public virtual IEnumerable<Navigation> FindNavigationsInHierarchy([JetBrains.Annotations.NotNull] string navigationName)
	{
		return ToEnumerable(FindNavigation(navigationName)).Concat(FindDerivedNavigations(navigationName));
	}

	public virtual Navigation RemoveNavigation([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Navigation navigation = FindDeclaredNavigation(name);
		if (navigation == null)
		{
			return null;
		}
		_navigations.Remove(name);
		return navigation;
	}

	public virtual IEnumerable<Navigation> GetNavigations()
	{
		return _baseType?.GetNavigations().Concat(_navigations.Values) ?? _navigations.Values;
	}

	public virtual Index AddIndex([JetBrains.Annotations.NotNull] Property property, ConfigurationSource configurationSource)
	{
		return AddIndex(new Property[1] { property }, configurationSource);
	}

	public virtual Index AddIndex([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		for (int i = 0; i < properties.Count; i++)
		{
			Property property = properties[i];
			for (int j = i + 1; j < properties.Count; j++)
			{
				if (property == properties[j])
				{
					throw new InvalidOperationException(CoreStrings.DuplicatePropertyInList(properties.Format(), property.Name));
				}
			}
			if (FindProperty(property.Name) != property || property.Builder == null)
			{
				throw new InvalidOperationException(CoreStrings.IndexPropertiesWrongEntity(properties.Format(), this.DisplayName()));
			}
		}
		Index index = FindIndexesInHierarchy(properties).FirstOrDefault();
		if (index != null)
		{
			throw new InvalidOperationException(CoreStrings.DuplicateIndex(properties.Format(), this.DisplayName(), index.DeclaringEntityType.DisplayName()));
		}
		Index index2 = new Index(properties, this, configurationSource);
		_indexes.Add(properties, index2);
		foreach (Property property2 in properties)
		{
			if (property2.Indexes == null)
			{
				property2.Indexes = new List<Index> { index2 };
			}
			else
			{
				property2.Indexes.Add(index2);
			}
		}
		return (Index)(Model.ConventionDispatcher.OnIndexAdded(index2.Builder)?.Metadata);
	}

	public virtual Index FindIndex([JetBrains.Annotations.NotNull] IProperty property)
	{
		return FindIndex(new IProperty[1] { property });
	}

	public virtual Index FindIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Index index = FindDeclaredIndex(properties);
		if (index == null)
		{
			EntityType baseType = _baseType;
			if (baseType == null)
			{
				return null;
			}
			index = baseType.FindIndex(properties);
		}
		return index;
	}

	public virtual IEnumerable<Index> GetDeclaredIndexes()
	{
		return _indexes.Values;
	}

	public virtual IEnumerable<Index> GetDerivedIndexes()
	{
		return GetDerivedTypes().SelectMany((EntityType et) => et.GetDeclaredIndexes());
	}

	public virtual Index FindDeclaredIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		if (!_indexes.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual IEnumerable<Index> FindDerivedIndexes([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return from et in GetDerivedTypes()
			select et.FindDeclaredIndex(properties) into i
			where i != null
			select i;
	}

	public virtual IEnumerable<Index> FindIndexesInHierarchy([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		return ToEnumerable(FindIndex(properties)).Concat(FindDerivedIndexes(properties));
	}

	public virtual Index RemoveIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Index index = FindDeclaredIndex(properties);
		if (index != null)
		{
			return RemoveIndex(index);
		}
		return null;
	}

	public virtual Index RemoveIndex(Index index)
	{
		if (!_indexes.Remove(index.Properties))
		{
			throw new InvalidOperationException(CoreStrings.IndexWrongType(index.Properties.Format(), index.DeclaringEntityType.DisplayName(), this.DisplayName()));
		}
		index.Builder = null;
		foreach (Property property in index.Properties)
		{
			if (property.Indexes != null)
			{
				property.Indexes.Remove(index);
				if (property.Indexes.Count == 0)
				{
					property.Indexes = null;
				}
			}
		}
		Model.ConventionDispatcher.OnIndexRemoved(Builder, index);
		return index;
	}

	public virtual IEnumerable<Index> GetIndexes()
	{
		return _baseType?.GetIndexes().Concat(_indexes.Values) ?? _indexes.Values;
	}

	public virtual Property AddProperty([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type propertyType, ConfigurationSource? typeConfigurationSource, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyType, "propertyType");
		return AddProperty(name, propertyType, null, typeConfigurationSource, configurationSource);
	}

	public virtual Property AddProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo, ConfigurationSource configurationSource)
	{
		return AddProperty(memberInfo.GetSimpleMemberName(), memberInfo.GetMemberType(), memberInfo, configurationSource, configurationSource);
	}

	public virtual Property AddProperty([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		MemberInfo memberInfo = ClrType?.GetMembersInHierarchy(name).FirstOrDefault();
		if (memberInfo == null)
		{
			throw new InvalidOperationException(CoreStrings.NoPropertyType(name, this.DisplayName()));
		}
		return AddProperty(memberInfo, configurationSource);
	}

	public virtual Property AddProperty(string name, Type propertyType, MemberInfo memberInfo, ConfigurationSource? typeConfigurationSource, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyType, "propertyType");
		PropertyBase propertyBase = FindMembersInHierarchy(name).FirstOrDefault();
		if (propertyBase != null)
		{
			throw new InvalidOperationException(CoreStrings.ConflictingPropertyOrNavigation(name, this.DisplayName(), propertyBase.DeclaringType.DisplayName()));
		}
		if (memberInfo != null)
		{
			if (ClrType == null)
			{
				throw new InvalidOperationException(CoreStrings.ClrPropertyOnShadowEntity(memberInfo.Name, this.DisplayName()));
			}
			Type? declaringType = memberInfo.DeclaringType;
			if ((object)declaringType == null || !declaringType.GetTypeInfo().IsAssignableFrom(ClrType.GetTypeInfo()))
			{
				throw new ArgumentException(CoreStrings.PropertyWrongEntityClrType(memberInfo.Name, this.DisplayName(), memberInfo.DeclaringType?.ShortDisplayName()));
			}
			if (name != memberInfo.GetSimpleMemberName())
			{
				PropertyInfo obj = memberInfo as PropertyInfo;
				if ((object)obj == null || !obj.IsEFIndexerProperty())
				{
					if (typeConfigurationSource.HasValue)
					{
						throw new InvalidOperationException(CoreStrings.PropertyWrongName(name, this.DisplayName(), memberInfo.GetSimpleMemberName()));
					}
					propertyType = memberInfo.GetMemberType();
				}
				else if (ClrType?.GetMembersInHierarchy(name).FirstOrDefault() != null)
				{
					throw new InvalidOperationException(CoreStrings.PropertyClashingNonIndexer(name, this.DisplayName()));
				}
			}
		}
		else
		{
			memberInfo = ClrType?.GetMembersInHierarchy(name).FirstOrDefault();
		}
		if (memberInfo != null && propertyType != memberInfo.GetMemberType())
		{
			PropertyInfo obj2 = memberInfo as PropertyInfo;
			if ((object)obj2 == null || !obj2.IsEFIndexerProperty())
			{
				if (typeConfigurationSource.HasValue)
				{
					throw new InvalidOperationException(CoreStrings.PropertyWrongClrType(name, this.DisplayName(), memberInfo.GetMemberType().ShortDisplayName(), propertyType.ShortDisplayName()));
				}
				propertyType = memberInfo.GetMemberType();
			}
		}
		Property property = new Property(name, propertyType, memberInfo as PropertyInfo, memberInfo as FieldInfo, this, configurationSource, typeConfigurationSource);
		_properties.Add(property.Name, property);
		return (Property)(Model.ConventionDispatcher.OnPropertyAdded(property.Builder)?.Metadata);
	}

	public virtual Property FindProperty([JetBrains.Annotations.NotNull] string name)
	{
		Property property = FindDeclaredProperty(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"));
		if (property == null)
		{
			EntityType baseType = _baseType;
			if (baseType == null)
			{
				return null;
			}
			property = baseType.FindProperty(name);
		}
		return property;
	}

	public virtual Property FindDeclaredProperty([JetBrains.Annotations.NotNull] string name)
	{
		if (!_properties.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual IEnumerable<Property> GetDeclaredProperties()
	{
		return _properties.Values;
	}

	public virtual IEnumerable<Property> FindDerivedProperties([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyName, "propertyName");
		return from et in GetDerivedTypes()
			select et.FindDeclaredProperty(propertyName) into p
			where p != null
			select p;
	}

	public virtual IEnumerable<Property> FindDerivedPropertiesInclusive([JetBrains.Annotations.NotNull] string propertyName)
	{
		return ToEnumerable(FindDeclaredProperty(propertyName)).Concat(FindDerivedProperties(propertyName));
	}

	public virtual IEnumerable<Property> FindPropertiesInHierarchy([JetBrains.Annotations.NotNull] string propertyName)
	{
		return ToEnumerable(FindProperty(propertyName)).Concat(FindDerivedProperties(propertyName));
	}

	public virtual IReadOnlyList<Property> FindProperties([JetBrains.Annotations.NotNull] IReadOnlyList<string> propertyNames)
	{
		List<Property> list = new List<Property>(propertyNames.Count);
		foreach (string propertyName in propertyNames)
		{
			Property property = FindProperty(propertyName);
			if (property == null)
			{
				return null;
			}
			list.Add(property);
		}
		return list;
	}

	public virtual Property RemoveProperty([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Property property = FindDeclaredProperty(name);
		if (property != null)
		{
			return RemoveProperty(property);
		}
		return null;
	}

	public virtual Property RemoveProperty(Property property)
	{
		if (property.DeclaringEntityType != this)
		{
			throw new InvalidOperationException(CoreStrings.PropertyWrongType(property.Name, property.DeclaringEntityType.DisplayName(), this.DisplayName()));
		}
		CheckPropertyNotInUse(property);
		_properties.Remove(property.Name);
		property.Builder = null;
		return property;
	}

	private void CheckPropertyNotInUse(Property property)
	{
		Key key = property.Keys?.FirstOrDefault();
		if (key != null)
		{
			throw new InvalidOperationException(CoreStrings.PropertyInUseKey(property.Name, this.DisplayName(), key.Properties.Format()));
		}
		ForeignKey foreignKey = property.ForeignKeys?.FirstOrDefault();
		if (foreignKey != null)
		{
			throw new InvalidOperationException(CoreStrings.PropertyInUseForeignKey(property.Name, this.DisplayName(), foreignKey.Properties.Format(), foreignKey.DeclaringEntityType.DisplayName()));
		}
		Index index = property.Indexes?.FirstOrDefault();
		if (index != null)
		{
			throw new InvalidOperationException(CoreStrings.PropertyInUseIndex(property.Name, this.DisplayName(), index.Properties.Format(), index.DeclaringEntityType.DisplayName()));
		}
	}

	public virtual IEnumerable<Property> GetProperties()
	{
		return _baseType?.GetProperties().Concat(_properties.Values) ?? _properties.Values;
	}

	public virtual ServiceProperty AddServiceProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memberInfo, "memberInfo");
		string simpleMemberName = memberInfo.GetSimpleMemberName();
		PropertyBase propertyBase = FindMembersInHierarchy(simpleMemberName).FirstOrDefault();
		if (propertyBase != null)
		{
			throw new InvalidOperationException(CoreStrings.ConflictingPropertyOrNavigation(simpleMemberName, this.DisplayName(), propertyBase.DeclaringType.DisplayName()));
		}
		ServiceProperty serviceProperty = new ServiceProperty(simpleMemberName, memberInfo as PropertyInfo, memberInfo as FieldInfo, this, configurationSource);
		ServiceProperty serviceProperty2 = GetServiceProperties().FirstOrDefault((ServiceProperty p) => p.ClrType == serviceProperty.ClrType);
		if (serviceProperty2 != null)
		{
			throw new InvalidOperationException(CoreStrings.DuplicateServicePropertyType(simpleMemberName, serviceProperty.ClrType.ShortDisplayName(), this.DisplayName(), serviceProperty2.Name, serviceProperty2.DeclaringEntityType.DisplayName()));
		}
		_serviceProperties[serviceProperty.Name] = serviceProperty;
		return serviceProperty;
	}

	public virtual ServiceProperty FindServiceProperty([JetBrains.Annotations.NotNull] string name)
	{
		ServiceProperty serviceProperty = FindDeclaredServiceProperty(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"));
		if (serviceProperty == null)
		{
			EntityType baseType = _baseType;
			if (baseType == null)
			{
				return null;
			}
			serviceProperty = baseType.FindServiceProperty(name);
		}
		return serviceProperty;
	}

	public virtual Property FindServiceProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo)
	{
		return FindProperty(memberInfo.GetSimpleMemberName());
	}

	public virtual ServiceProperty FindDeclaredServiceProperty([JetBrains.Annotations.NotNull] string name)
	{
		if (!_serviceProperties.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual IEnumerable<ServiceProperty> FindDerivedServiceProperties([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyName, "propertyName");
		return from et in GetDerivedTypes()
			select et.FindDeclaredServiceProperty(propertyName) into p
			where p != null
			select p;
	}

	public virtual IEnumerable<ServiceProperty> FindDerivedServicePropertiesInclusive([JetBrains.Annotations.NotNull] string propertyName)
	{
		return ToEnumerable(FindDeclaredServiceProperty(propertyName)).Concat(FindDerivedServiceProperties(propertyName));
	}

	public virtual IEnumerable<ServiceProperty> FindServicePropertiesInHierarchy([JetBrains.Annotations.NotNull] string propertyName)
	{
		return ToEnumerable(FindServiceProperty(propertyName)).Concat(FindDerivedServiceProperties(propertyName));
	}

	public virtual ServiceProperty RemoveServiceProperty([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		ServiceProperty serviceProperty = FindServiceProperty(name);
		if (serviceProperty != null)
		{
			return RemoveServiceProperty(serviceProperty);
		}
		return null;
	}

	private ServiceProperty RemoveServiceProperty(ServiceProperty property)
	{
		_serviceProperties.Remove(property.Name);
		property.Builder = null;
		return property;
	}

	public virtual IEnumerable<ServiceProperty> GetServiceProperties()
	{
		return _baseType?.GetServiceProperties().Concat(_serviceProperties.Values) ?? _serviceProperties.Values;
	}

	public virtual IEnumerable<ServiceProperty> GetDeclaredServiceProperties()
	{
		return _serviceProperties.Values;
	}

	public override ConfigurationSource? FindIgnoredConfigurationSource(string name)
	{
		ConfigurationSource? configurationSource = FindDeclaredIgnoredConfigurationSource(name);
		if (BaseType != null)
		{
			return BaseType.FindIgnoredConfigurationSource(name).Max(configurationSource);
		}
		return configurationSource;
	}

	public override void OnTypeMemberIgnored(string name)
	{
		Model.ConventionDispatcher.OnEntityTypeMemberIgnored(Builder, name);
	}

	public virtual IEnumerable<IDictionary<string, object>> GetSeedData(bool providerValues = false)
	{
		if (_data == null || _data.Count == 0)
		{
			return Enumerable.Empty<IDictionary<string, object>>();
		}
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		Dictionary<string, ValueConverter> dictionary = new Dictionary<string, ValueConverter>(StringComparer.Ordinal);
		Dictionary<string, IPropertyBase> dictionary2 = this.GetPropertiesAndNavigations().ToDictionary((IPropertyBase p) => p.Name);
		foreach (object datum in _data)
		{
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>(StringComparer.Ordinal);
			list.Add(dictionary3);
			foreach (MemberInfo item in datum.GetType().GetMembersInHierarchy())
			{
				if (!dictionary2.TryGetValue(item.GetSimpleMemberName(), out var value))
				{
					continue;
				}
				ValueConverter value2 = null;
				if (providerValues && !dictionary.TryGetValue(item.Name, out value2))
				{
					if (value is IProperty property)
					{
						value2 = property.FindTypeMapping()?.Converter ?? property.GetValueConverter();
					}
					dictionary[item.Name] = value2;
				}
				object obj = null;
				if (!(item is PropertyInfo propertyInfo))
				{
					if (item is FieldInfo fieldInfo)
					{
						obj = fieldInfo.GetValue(datum);
					}
				}
				else
				{
					obj = propertyInfo.GetValue(datum);
				}
				dictionary3[item.Name] = ((value2 == null) ? obj : value2.ConvertToProvider(obj));
			}
		}
		return list;
	}

	public virtual void AddData([JetBrains.Annotations.NotNull] IEnumerable<object> data)
	{
		if (_data == null)
		{
			_data = new List<object>();
		}
		foreach (object datum in data)
		{
			if (ClrType != null && ClrType != datum.GetType() && ClrType.GetTypeInfo().IsAssignableFrom(datum.GetType().GetTypeInfo()))
			{
				throw new InvalidOperationException(CoreStrings.SeedDatumDerivedType(this.DisplayName(), datum.GetType().ShortDisplayName()));
			}
			_data.Add(datum);
		}
	}

	public virtual void SetChangeTrackingStrategy(ChangeTrackingStrategy? changeTrackingStrategy, ConfigurationSource configurationSource)
	{
		if (changeTrackingStrategy.HasValue)
		{
			string text = CheckChangeTrackingStrategy(changeTrackingStrategy.Value);
			if (text != null)
			{
				throw new InvalidOperationException(text);
			}
		}
		this.SetOrRemoveAnnotation("ChangeTrackingStrategy", changeTrackingStrategy, configurationSource);
	}

	public virtual string CheckChangeTrackingStrategy(ChangeTrackingStrategy value)
	{
		if (ClrType != null)
		{
			if (value != 0 && !typeof(INotifyPropertyChanged).GetTypeInfo().IsAssignableFrom(ClrType.GetTypeInfo()))
			{
				return CoreStrings.ChangeTrackingInterfaceMissing(this.DisplayName(), value, "INotifyPropertyChanged");
			}
			if ((value == ChangeTrackingStrategy.ChangingAndChangedNotifications || value == ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues) && !typeof(INotifyPropertyChanging).GetTypeInfo().IsAssignableFrom(ClrType.GetTypeInfo()))
			{
				return CoreStrings.ChangeTrackingInterfaceMissing(this.DisplayName(), value, "INotifyPropertyChanging");
			}
		}
		return null;
	}

	public virtual void SetQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression queryFilter, ConfigurationSource configurationSource)
	{
		string text = CheckQueryFilter(queryFilter);
		if (text != null)
		{
			throw new InvalidOperationException(text);
		}
		this.SetOrRemoveAnnotation("QueryFilter", queryFilter, configurationSource);
	}

	public virtual string CheckQueryFilter([JetBrains.Annotations.CanBeNull] LambdaExpression queryFilter)
	{
		if (queryFilter != null && (queryFilter.Parameters.Count != 1 || queryFilter.Parameters[0].Type != ClrType || queryFilter.ReturnType != typeof(bool)))
		{
			return CoreStrings.BadFilterExpression(queryFilter, this.DisplayName(), ClrType);
		}
		return null;
	}

	public virtual void SetDefiningQuery([JetBrains.Annotations.CanBeNull] LambdaExpression definingQuery, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("DefiningQuery", definingQuery, configurationSource);
	}

	public virtual void SetDiscriminatorProperty([JetBrains.Annotations.CanBeNull] IProperty property, ConfigurationSource configurationSource)
	{
		CheckDiscriminatorProperty(property);
		if ((property == null && BaseType == null) || (property != null && !property.ClrType.IsInstanceOfType(this.GetDiscriminatorValue())))
		{
			Microsoft.EntityFrameworkCore.MutableEntityTypeExtensions.RemoveDiscriminatorValue(this);
			if (BaseType == null)
			{
				foreach (EntityType derivedType in GetDerivedTypes())
				{
					Microsoft.EntityFrameworkCore.MutableEntityTypeExtensions.RemoveDiscriminatorValue(derivedType);
				}
			}
		}
		SetAnnotation("DiscriminatorProperty", property?.Name, configurationSource);
	}

	private void CheckDiscriminatorProperty(IProperty property)
	{
		if (property != null)
		{
			if (BaseType != null)
			{
				throw new InvalidOperationException(CoreStrings.DiscriminatorPropertyMustBeOnRoot(this.DisplayName()));
			}
			if (property.DeclaringEntityType != this)
			{
				throw new InvalidOperationException(CoreStrings.DiscriminatorPropertyNotFound(property.Name, this.DisplayName()));
			}
		}
	}

	public virtual void CheckDiscriminatorValue(IEntityType entityType, object value)
	{
		if (value != null && entityType.GetDiscriminatorProperty() == null)
		{
			throw new InvalidOperationException(CoreStrings.NoDiscriminatorForValue(entityType.DisplayName(), entityType.GetRootType().DisplayName()));
		}
		if (value != null && !entityType.GetDiscriminatorProperty().ClrType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
		{
			throw new InvalidOperationException(CoreStrings.DiscriminatorValueIncompatible(value, entityType.GetDiscriminatorProperty().Name, entityType.GetDiscriminatorProperty().ClrType));
		}
	}

	IMutableKey IMutableEntityType.SetPrimaryKey(IReadOnlyList<IMutableProperty> properties)
	{
		return SetPrimaryKey(properties?.Cast<Property>().ToList(), ConfigurationSource.Explicit);
	}

	[DebuggerStepThrough]
	IKey IEntityType.FindPrimaryKey()
	{
		return FindPrimaryKey();
	}

	[DebuggerStepThrough]
	IMutableKey IMutableEntityType.FindPrimaryKey()
	{
		return FindPrimaryKey();
	}

	IMutableKey IMutableEntityType.AddKey(IReadOnlyList<IMutableProperty> properties)
	{
		return AddKey(properties.Cast<Property>().ToList(), ConfigurationSource.Explicit);
	}

	[DebuggerStepThrough]
	IKey IEntityType.FindKey(IReadOnlyList<IProperty> properties)
	{
		return FindKey(properties);
	}

	[DebuggerStepThrough]
	IMutableKey IMutableEntityType.FindKey(IReadOnlyList<IProperty> properties)
	{
		return FindKey(properties);
	}

	IEnumerable<IKey> IEntityType.GetKeys()
	{
		return GetKeys();
	}

	IEnumerable<IMutableKey> IMutableEntityType.GetKeys()
	{
		return GetKeys();
	}

	void IMutableEntityType.RemoveKey(IMutableKey key)
	{
		RemoveKey((Key)key);
	}

	IMutableForeignKey IMutableEntityType.AddForeignKey(IReadOnlyList<IMutableProperty> properties, IMutableKey principalKey, IMutableEntityType principalEntityType)
	{
		return AddForeignKey(properties.Cast<Property>().ToList(), (Key)principalKey, (EntityType)principalEntityType, ConfigurationSource.Explicit, ConfigurationSource.Explicit);
	}

	[DebuggerStepThrough]
	IMutableForeignKey IMutableEntityType.FindForeignKey(IReadOnlyList<IProperty> properties, IKey principalKey, IEntityType principalEntityType)
	{
		return FindForeignKey(properties, principalKey, principalEntityType);
	}

	[DebuggerStepThrough]
	IForeignKey IEntityType.FindForeignKey(IReadOnlyList<IProperty> properties, IKey principalKey, IEntityType principalEntityType)
	{
		return FindForeignKey(properties, principalKey, principalEntityType);
	}

	IEnumerable<IForeignKey> IEntityType.GetForeignKeys()
	{
		return GetForeignKeys();
	}

	IEnumerable<IMutableForeignKey> IMutableEntityType.GetForeignKeys()
	{
		return GetForeignKeys();
	}

	void IMutableEntityType.RemoveForeignKey(IMutableForeignKey foreignKey)
	{
		RemoveForeignKey((ForeignKey)foreignKey);
	}

	IMutableIndex IMutableEntityType.AddIndex(IReadOnlyList<IMutableProperty> properties)
	{
		return AddIndex(properties.Cast<Property>().ToList(), ConfigurationSource.Explicit);
	}

	[DebuggerStepThrough]
	IIndex IEntityType.FindIndex(IReadOnlyList<IProperty> properties)
	{
		return FindIndex(properties);
	}

	[DebuggerStepThrough]
	IMutableIndex IMutableEntityType.FindIndex(IReadOnlyList<IProperty> properties)
	{
		return FindIndex(properties);
	}

	IEnumerable<IIndex> IEntityType.GetIndexes()
	{
		return GetIndexes();
	}

	IEnumerable<IMutableIndex> IMutableEntityType.GetIndexes()
	{
		return GetIndexes();
	}

	void IMutableEntityType.RemoveIndex(IMutableIndex index)
	{
		RemoveIndex((Index)index);
	}

	IMutableProperty IMutableEntityType.AddProperty(string name, Type propertyType, MemberInfo memberInfo)
	{
		return AddProperty(name, propertyType, memberInfo, ConfigurationSource.Explicit, ConfigurationSource.Explicit);
	}

	[DebuggerStepThrough]
	IProperty IEntityType.FindProperty(string name)
	{
		return FindProperty(name);
	}

	[DebuggerStepThrough]
	IMutableProperty IMutableEntityType.FindProperty(string name)
	{
		return FindProperty(name);
	}

	[DebuggerStepThrough]
	IEnumerable<IProperty> IEntityType.GetProperties()
	{
		return GetProperties();
	}

	[DebuggerStepThrough]
	IEnumerable<IMutableProperty> IMutableEntityType.GetProperties()
	{
		return GetProperties();
	}

	void IMutableEntityType.RemoveProperty(IMutableProperty property)
	{
		RemoveProperty((Property)property);
	}

	IMutableServiceProperty IMutableEntityType.AddServiceProperty(MemberInfo memberInfo)
	{
		return AddServiceProperty(memberInfo, ConfigurationSource.Explicit);
	}

	[DebuggerStepThrough]
	IServiceProperty IEntityType.FindServiceProperty(string name)
	{
		return FindServiceProperty(name);
	}

	[DebuggerStepThrough]
	IMutableServiceProperty IMutableEntityType.FindServiceProperty(string name)
	{
		return FindServiceProperty(name);
	}

	IEnumerable<IServiceProperty> IEntityType.GetServiceProperties()
	{
		return GetServiceProperties();
	}

	IEnumerable<IMutableServiceProperty> IMutableEntityType.GetServiceProperties()
	{
		return GetServiceProperties();
	}

	IMutableServiceProperty IMutableEntityType.RemoveServiceProperty(string name)
	{
		return RemoveServiceProperty(name);
	}

	void IConventionEntityType.HasBaseType(IConventionEntityType entityType, bool fromDataAnnotation)
	{
		HasBaseType((EntityType)entityType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionEntityType.HasNoKey(bool? keyless, bool fromDataAnnotation)
	{
		HasNoKey(keyless, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionKey IConventionEntityType.SetPrimaryKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return SetPrimaryKey(properties?.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionKey IConventionEntityType.FindPrimaryKey()
	{
		return FindPrimaryKey();
	}

	IConventionKey IConventionEntityType.FindKey(IReadOnlyList<IProperty> properties)
	{
		return FindKey(properties);
	}

	IEnumerable<IConventionKey> IConventionEntityType.GetKeys()
	{
		return GetKeys();
	}

	IConventionForeignKey IConventionEntityType.FindForeignKey(IReadOnlyList<IProperty> properties, IKey principalKey, IEntityType principalEntityType)
	{
		return FindForeignKey(properties, principalKey, principalEntityType);
	}

	IEnumerable<IConventionForeignKey> IConventionEntityType.GetForeignKeys()
	{
		return GetForeignKeys();
	}

	IConventionIndex IConventionEntityType.FindIndex(IReadOnlyList<IProperty> properties)
	{
		return FindIndex(properties);
	}

	IEnumerable<IConventionIndex> IConventionEntityType.GetIndexes()
	{
		return GetIndexes();
	}

	IConventionProperty IConventionEntityType.FindProperty(string name)
	{
		return FindProperty(name);
	}

	IEnumerable<IConventionProperty> IConventionEntityType.GetProperties()
	{
		return GetProperties();
	}

	IConventionServiceProperty IConventionEntityType.FindServiceProperty(string name)
	{
		return FindServiceProperty(name);
	}

	IEnumerable<IConventionServiceProperty> IConventionEntityType.GetServiceProperties()
	{
		return GetServiceProperties();
	}

	IConventionServiceProperty IConventionEntityType.RemoveServiceProperty(string name)
	{
		return RemoveServiceProperty(name);
	}

	void IConventionEntityType.RemoveProperty(IConventionProperty property)
	{
		RemoveProperty((Property)property);
	}

	IConventionServiceProperty IConventionEntityType.AddServiceProperty(MemberInfo memberInfo, bool fromDataAnnotation)
	{
		return AddServiceProperty(memberInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionEntityType.RemoveIndex(IConventionIndex index)
	{
		RemoveIndex((Index)index);
	}

	IConventionProperty IConventionEntityType.AddProperty(string name, Type propertyType, MemberInfo memberInfo, bool setTypeConfigurationSource, bool fromDataAnnotation)
	{
		return AddProperty(name, propertyType, memberInfo, setTypeConfigurationSource ? new ConfigurationSource?(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention) : null, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionEntityType.RemoveForeignKey(IConventionForeignKey foreignKey)
	{
		RemoveForeignKey((ForeignKey)foreignKey);
	}

	IConventionIndex IConventionEntityType.AddIndex(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return AddIndex(properties.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionEntityType.RemoveKey(IConventionKey key)
	{
		RemoveKey((Key)key);
	}

	IConventionForeignKey IConventionEntityType.AddForeignKey(IReadOnlyList<IConventionProperty> properties, IConventionKey principalKey, IConventionEntityType principalEntityType, bool setComponentConfigurationSource, bool fromDataAnnotation)
	{
		return AddForeignKey(properties.Cast<Property>().ToList(), (Key)principalKey, (EntityType)principalEntityType, setComponentConfigurationSource ? new ConfigurationSource?(fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention) : null, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionKey IConventionEntityType.AddKey(IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation)
	{
		return AddKey(properties.Cast<Property>().ToList(), fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	private static IEnumerable<T> ToEnumerable<T>(T element) where T : class
	{
		if (element != null)
		{
			return new T[1] { element };
		}
		return Enumerable.Empty<T>();
	}
}
