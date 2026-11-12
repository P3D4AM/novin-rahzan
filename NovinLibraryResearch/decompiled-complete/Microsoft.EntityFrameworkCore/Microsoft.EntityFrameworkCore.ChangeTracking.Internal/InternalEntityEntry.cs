using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public abstract class InternalEntityEntry : IUpdateEntry
{
	private enum CurrentValueType
	{
		Normal,
		StoreGenerated,
		Temporary
	}

	private readonly struct OriginalValues
	{
		private readonly ISnapshot _values;

		public bool IsEmpty => _values == null;

		public OriginalValues(InternalEntityEntry entry)
		{
			_values = ((EntityType)entry.EntityType).OriginalValuesFactory(entry);
		}

		public object GetValue(InternalEntityEntry entry, IProperty property)
		{
			int originalValueIndex = property.GetOriginalValueIndex();
			if (originalValueIndex == -1)
			{
				throw new InvalidOperationException(CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringEntityType.DisplayName()));
			}
			if (!IsEmpty)
			{
				return _values[originalValueIndex];
			}
			return entry[property];
		}

		public T GetValue<T>(InternalEntityEntry entry, IProperty property, int index)
		{
			if (index == -1)
			{
				throw new InvalidOperationException(CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringEntityType.DisplayName()));
			}
			if (!IsEmpty)
			{
				return _values.GetValue<T>(index);
			}
			return entry.GetCurrentValue<T>(property);
		}

		public void SetValue(IProperty property, object value, int index)
		{
			if (index == -1)
			{
				index = property.GetOriginalValueIndex();
				if (index == -1)
				{
					throw new InvalidOperationException(CoreStrings.OriginalValueNotTracked(property.Name, property.DeclaringEntityType.DisplayName()));
				}
			}
			if (value == null && !property.ClrType.IsNullableType())
			{
				throw new InvalidOperationException(CoreStrings.ValueCannotBeNull(property.Name, property.DeclaringEntityType.DisplayName(), property.ClrType.DisplayName()));
			}
			_values[index] = SnapshotValue(property, value);
		}

		public void RejectChanges(InternalEntityEntry entry)
		{
			if (IsEmpty)
			{
				return;
			}
			foreach (IProperty property in entry.EntityType.GetProperties())
			{
				int originalValueIndex = property.GetOriginalValueIndex();
				if (originalValueIndex >= 0)
				{
					entry[property] = SnapshotValue(property, _values[originalValueIndex]);
				}
			}
		}

		public void AcceptChanges(InternalEntityEntry entry)
		{
			if (IsEmpty)
			{
				return;
			}
			foreach (IProperty property in entry.EntityType.GetProperties())
			{
				int originalValueIndex = property.GetOriginalValueIndex();
				if (originalValueIndex >= 0)
				{
					_values[originalValueIndex] = SnapshotValue(property, entry[property]);
				}
			}
		}

		private static object SnapshotValue(IProperty property, object value)
		{
			ValueComparer valueComparer = property.GetValueComparer() ?? property.FindTypeMapping()?.Comparer;
			if (valueComparer != null)
			{
				return valueComparer.Snapshot(value);
			}
			return value;
		}
	}

	private readonly struct RelationshipsSnapshot
	{
		private readonly ISnapshot _values;

		public bool IsEmpty => _values == null;

		public RelationshipsSnapshot(InternalEntityEntry entry)
		{
			_values = ((EntityType)entry.EntityType).RelationshipSnapshotFactory(entry);
		}

		public object GetValue(InternalEntityEntry entry, IPropertyBase propertyBase)
		{
			if (!IsEmpty)
			{
				return _values[propertyBase.GetRelationshipIndex()];
			}
			return entry[propertyBase];
		}

		public T GetValue<T>(InternalEntityEntry entry, IPropertyBase propertyBase, int index)
		{
			if (!IsEmpty)
			{
				return _values.GetValue<T>(index);
			}
			return entry.GetCurrentValue<T>(propertyBase);
		}

		public void SetValue(IPropertyBase propertyBase, object value)
		{
			if (value != null || !(propertyBase is IProperty { IsNullable: false }))
			{
				_values[propertyBase.GetRelationshipIndex()] = SnapshotValue(propertyBase, value);
			}
		}

		private static object SnapshotValue(IPropertyBase propertyBase, object value)
		{
			if (propertyBase is IProperty property)
			{
				ValueComparer valueComparer = property.GetKeyValueComparer() ?? property.GetTypeMapping().KeyComparer;
				if (valueComparer != null)
				{
					return valueComparer.Snapshot(value);
				}
			}
			return value;
		}

		public void RemoveFromCollection(IPropertyBase propertyBase, object removedEntity)
		{
			int relationshipIndex = propertyBase.GetRelationshipIndex();
			if (relationshipIndex != -1)
			{
				((HashSet<object>)_values[relationshipIndex])?.Remove(removedEntity);
			}
		}

		public void AddToCollection(IPropertyBase propertyBase, object addedEntity)
		{
			int relationshipIndex = propertyBase.GetRelationshipIndex();
			if (relationshipIndex != -1)
			{
				GetOrCreateCollection(relationshipIndex).Add(addedEntity);
			}
		}

		public void AddRangeToCollection(IPropertyBase propertyBase, IEnumerable<object> addedEntities)
		{
			int relationshipIndex = propertyBase.GetRelationshipIndex();
			if (relationshipIndex == -1)
			{
				return;
			}
			HashSet<object> orCreateCollection = GetOrCreateCollection(relationshipIndex);
			foreach (object addedEntity in addedEntities)
			{
				orCreateCollection.Add(addedEntity);
			}
		}

		private HashSet<object> GetOrCreateCollection(int index)
		{
			HashSet<object> hashSet = (HashSet<object>)_values[index];
			if (hashSet == null)
			{
				hashSet = new HashSet<object>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
				_values[index] = hashSet;
			}
			return hashSet;
		}
	}

	private readonly struct SidecarValues
	{
		private readonly ISnapshot _values;

		public bool IsEmpty => _values == null;

		public SidecarValues(ISnapshot valuesFactory)
		{
			_values = valuesFactory;
		}

		public bool TryGetValue(int index, out object value)
		{
			if (IsEmpty)
			{
				value = null;
				return false;
			}
			value = _values[index];
			return true;
		}

		public T GetValue<T>(int index)
		{
			if (!IsEmpty)
			{
				return _values.GetValue<T>(index);
			}
			return default(T);
		}

		public void SetValue(IProperty property, object value, int index)
		{
			if (value == null && !property.ClrType.IsNullableType())
			{
				throw new InvalidOperationException(CoreStrings.ValueCannotBeNull(property.Name, property.DeclaringEntityType.DisplayName(), property.ClrType.DisplayName()));
			}
			_values[index] = SnapshotValue(property, value);
		}

		private static object SnapshotValue(IProperty property, object value)
		{
			ValueComparer valueComparer = property.GetValueComparer() ?? property.FindTypeMapping()?.Comparer;
			if (valueComparer != null)
			{
				return valueComparer.Snapshot(value);
			}
			return value;
		}
	}

	internal enum PropertyFlag
	{
		Modified,
		Null,
		Unknown,
		IsLoaded
	}

	internal readonly struct StateData
	{
		private const int BitsPerInt = 32;

		private const int BitsForEntityState = 3;

		private const int BitsForEntityFlags = 1;

		private const int BitsForPropertyFlags = 4;

		private const int BitsForAdditionalState = 4;

		private const int EntityStateMask = 7;

		private const int UnusedStateMask = 8;

		private const int AdditionalStateMask = 15;

		private const int PropertyFlagMask = 286331153;

		private readonly int[] _bits;

		public EntityState EntityState
		{
			get
			{
				return (EntityState)(_bits[0] & 7);
			}
			set
			{
				_bits[0] = (_bits[0] & -8) | (int)value;
			}
		}

		public StateData(int propertyCount, int navigationCount)
		{
			int num = Math.Max(propertyCount, navigationCount) * 4 + 4 - 1;
			_bits = new int[num / 32 + 1];
		}

		public void FlagAllProperties(int propertyCount, PropertyFlag propertyFlag, bool flagged)
		{
			for (int i = 0; i < _bits.Length; i++)
			{
				if (flagged)
				{
					_bits[i] |= CreateMaskForWrite(i, propertyCount, propertyFlag);
				}
				else
				{
					_bits[i] &= ~CreateMaskForWrite(i, propertyCount, propertyFlag);
				}
			}
		}

		public bool IsPropertyFlagged(int propertyIndex, PropertyFlag propertyFlag)
		{
			propertyIndex = (int)(propertyIndex * 4 + propertyFlag + 4);
			return (_bits[propertyIndex / 32] & (1 << propertyIndex % 32)) != 0;
		}

		public void FlagProperty(int propertyIndex, PropertyFlag propertyFlag, bool isFlagged)
		{
			propertyIndex = (int)(propertyIndex * 4 + propertyFlag + 4);
			if (isFlagged)
			{
				_bits[propertyIndex / 32] |= 1 << propertyIndex % 32;
			}
			else
			{
				_bits[propertyIndex / 32] &= ~(1 << propertyIndex % 32);
			}
		}

		public bool AnyPropertiesFlagged(PropertyFlag propertyFlag)
		{
			for (int i = 0; i < _bits.Length; i++)
			{
				if ((_bits[i] & CreateMaskForRead(i, propertyFlag)) != 0)
				{
					return true;
				}
			}
			return false;
		}

		private static int CreateMaskForRead(int i, PropertyFlag propertyFlag)
		{
			int num = 286331153 << (int)propertyFlag;
			if (i == 0)
			{
				num &= -16;
			}
			return num;
		}

		private int CreateMaskForWrite(int i, int propertyCount, PropertyFlag propertyFlag)
		{
			int num = CreateMaskForRead(i, propertyFlag);
			if (i == _bits.Length - 1)
			{
				int num2 = 286331153 << (int)propertyFlag;
				int num3 = (propertyCount * 4 + 4) % 32;
				num2 = ((num3 != 0) ? (num2 << num3) : 0);
				num &= ~num2;
			}
			return num;
		}
	}

	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<object, object, bool> _003C_003E9__86_1;

		public static Func<IProperty, bool> _003C_003E9__89_1;

		public static Func<IForeignKey, bool> _003C_003E9__89_0;

		public static Func<IServiceProperty, bool> _003C_003E9__101_0;

		internal bool _003CValuesEqualFunc_003Eb__86_1(object l, object r)
		{
			return object.Equals(l, r);
		}

		internal bool _003CHandleConceptualNulls_003Eb__89_1(IProperty p)
		{
			return p.IsNullable;
		}

		internal bool _003CHandleConceptualNulls_003Eb__89_0(IForeignKey fk)
		{
			if (fk.DeleteBehavior != DeleteBehavior.Cascade)
			{
				return fk.DeleteBehavior == DeleteBehavior.ClientCascade;
			}
			return true;
		}

		internal bool _003CSetIsLoaded_003Eb__101_0(IServiceProperty p)
		{
			return p.ClrType == typeof(ILazyLoader);
		}

		internal bool _003C_002Ecctor_003Eb__112_0(MethodInfo m)
		{
			return m.IsGenericMethod;
		}
	}

	private readonly StateData _stateData;

	private OriginalValues _originalValues;

	private RelationshipsSnapshot _relationshipsSnapshot;

	private SidecarValues _temporaryValues;

	private SidecarValues _storeGeneratedValues;

	internal static readonly MethodInfo ReadShadowValueMethod = typeof(InternalEntityEntry).GetTypeInfo().GetDeclaredMethod("ReadShadowValue");

	internal static readonly MethodInfo ReadOriginalValueMethod = typeof(InternalEntityEntry).GetTypeInfo().GetDeclaredMethod("ReadOriginalValue");

	internal static readonly MethodInfo ReadRelationshipSnapshotValueMethod = typeof(InternalEntityEntry).GetTypeInfo().GetDeclaredMethod("ReadRelationshipSnapshotValue");

	internal static readonly MethodInfo ReadStoreGeneratedValueMethod = typeof(InternalEntityEntry).GetTypeInfo().GetDeclaredMethod("ReadStoreGeneratedValue");

	internal static readonly MethodInfo ReadTemporaryValueMethod = typeof(InternalEntityEntry).GetTypeInfo().GetDeclaredMethod("ReadTemporaryValue");

	internal static readonly MethodInfo GetCurrentValueMethod = typeof(InternalEntityEntry).GetTypeInfo().GetDeclaredMethods("GetCurrentValue").Single((MethodInfo m) => m.IsGenericMethod);

	public abstract object Entity { get; }

	public virtual IEntityType EntityType
	{
		[DebuggerStepThrough]
		get;
	}

	EntityState IUpdateEntry.EntityState
	{
		get
		{
			return EntityState;
		}
		set
		{
			SetEntityState(value, acceptChanges: false, modifyProperties: false);
		}
	}

	public virtual IStateManager StateManager
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual InternalEntityEntry SharedIdentityEntry
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual EntityState EntityState => _stateData.EntityState;

	public virtual bool HasConceptualNull => _stateData.AnyPropertiesFlagged(PropertyFlag.Null);

	public virtual bool HasOriginalValuesSnapshot => !_originalValues.IsEmpty;

	public virtual bool HasRelationshipSnapshot => !_relationshipsSnapshot.IsEmpty;

	public object this[[JetBrains.Annotations.NotNull] IPropertyBase propertyBase]
	{
		get
		{
			object obj = ReadPropertyValue(propertyBase);
			int storeGeneratedIndex = propertyBase.GetStoreGeneratedIndex();
			if (storeGeneratedIndex != -1)
			{
				object defaultValue = propertyBase.ClrType.GetDefaultValue();
				Func<object, object, bool> func = ValuesEqualFunc((IProperty)propertyBase);
				if (func(obj, defaultValue))
				{
					if (_storeGeneratedValues.TryGetValue(storeGeneratedIndex, out var value) && !func(value, defaultValue))
					{
						return value;
					}
					if (_temporaryValues.TryGetValue(storeGeneratedIndex, out value) && !func(value, defaultValue))
					{
						return value;
					}
				}
			}
			return obj;
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			SetProperty(propertyBase, value, isMaterialization: false);
		}
	}

	public virtual (bool IsGenerated, bool IsSet) IsKeySet
	{
		get
		{
			bool item = false;
			IReadOnlyList<Property> properties = ((EntityType)EntityType).FindPrimaryKey().Properties;
			for (int i = 0; i < properties.Count; i++)
			{
				Property property = properties[i];
				bool flag = property.ValueGenerated == ValueGenerated.OnAdd;
				if ((HasTemporaryValue(property) || HasDefaultValue(property)) && (flag || property.IsForeignKey()))
				{
					return (IsGenerated: true, IsSet: false);
				}
				if (flag)
				{
					item = true;
				}
			}
			return (IsGenerated: item, IsSet: true);
		}
	}

	public virtual bool IsKeyUnknown
	{
		get
		{
			IReadOnlyList<Property> properties = ((EntityType)EntityType).FindPrimaryKey().Properties;
			for (int i = 0; i < properties.Count; i++)
			{
				Property property = properties[i];
				if (_stateData.IsPropertyFlagged(property.GetIndex(), PropertyFlag.Unknown))
				{
					return true;
				}
			}
			return false;
		}
	}

	IUpdateEntry IUpdateEntry.SharedIdentityEntry => SharedIdentityEntry;

	protected InternalEntityEntry([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		StateManager = stateManager;
		EntityType = entityType;
		_stateData = new StateData(entityType.PropertyCount(), entityType.NavigationCount());
	}

	void IUpdateEntry.SetOriginalValue(IProperty property, object value)
	{
		SetOriginalValue(property, value);
	}

	void IUpdateEntry.SetPropertyModified(IProperty property)
	{
		SetPropertyModified(property);
	}

	public virtual void SetEntityState(EntityState entityState, bool acceptChanges = false, bool modifyProperties = true, EntityState? forceStateWhenUnknownKey = null)
	{
		EntityState entityState2 = _stateData.EntityState;
		bool flag = PrepareForAdd(entityState);
		entityState = PropagateToUnknownKey(entityState2, entityState, flag, forceStateWhenUnknownKey);
		if (flag)
		{
			StateManager.ValueGenerationManager.Generate(this);
		}
		SetEntityState(entityState2, entityState, acceptChanges, modifyProperties);
	}

	public virtual async Task SetEntityStateAsync(EntityState entityState, bool acceptChanges = false, bool modifyProperties = true, EntityState? forceStateWhenUnknownKey = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		EntityState oldState = _stateData.EntityState;
		bool flag = PrepareForAdd(entityState);
		entityState = PropagateToUnknownKey(oldState, entityState, flag, forceStateWhenUnknownKey);
		if (flag)
		{
			await StateManager.ValueGenerationManager.GenerateAsync(this, cancellationToken);
		}
		SetEntityState(oldState, entityState, acceptChanges, modifyProperties);
	}

	private EntityState PropagateToUnknownKey(EntityState oldState, EntityState entityState, bool adding, EntityState? forceStateWhenUnknownKey)
	{
		bool isKeyUnknown = IsKeyUnknown;
		if (adding || (oldState == EntityState.Detached && isKeyUnknown))
		{
			InternalEntityEntry internalEntityEntry = StateManager.ValueGenerationManager.Propagate(this);
			if (forceStateWhenUnknownKey.HasValue && isKeyUnknown && internalEntityEntry != null && internalEntityEntry.EntityState != 0 && internalEntityEntry.EntityState != EntityState.Deleted)
			{
				entityState = ((internalEntityEntry.EntityState == EntityState.Added) ? EntityState.Added : forceStateWhenUnknownKey.Value);
			}
		}
		return entityState;
	}

	private bool PrepareForAdd(EntityState newState)
	{
		if (newState != EntityState.Added || EntityState == EntityState.Added)
		{
			return false;
		}
		if (EntityState == EntityState.Modified)
		{
			_stateData.FlagAllProperties(EntityType.PropertyCount(), PropertyFlag.Modified, flagged: false);
		}
		_stateData.EntityState = EntityState.Detached;
		return true;
	}

	private void SetEntityState(EntityState oldState, EntityState newState, bool acceptChanges, bool modifyProperties)
	{
		EntityType entityType = (EntityType)EntityType;
		if (oldState == EntityState.Added && newState != EntityState.Added && newState != 0)
		{
			foreach (Property property in entityType.GetProperties())
			{
				if (HasTemporaryValue(property))
				{
					throw new InvalidOperationException(CoreStrings.TempValuePersists(property.Name, EntityType.DisplayName(), newState));
				}
			}
		}
		if (newState == EntityState.Modified && modifyProperties)
		{
			_stateData.FlagAllProperties(entityType.PropertyCount(), PropertyFlag.Modified, flagged: true);
			foreach (Property property2 in entityType.GetProperties())
			{
				if (property2.GetAfterSaveBehavior() != 0)
				{
					_stateData.FlagProperty(property2.GetIndex(), PropertyFlag.Modified, isFlagged: false);
				}
			}
		}
		if (oldState == newState)
		{
			return;
		}
		if (newState == EntityState.Unchanged)
		{
			_stateData.FlagAllProperties(EntityType.PropertyCount(), PropertyFlag.Modified, flagged: false);
		}
		if (_stateData.EntityState != oldState)
		{
			_stateData.EntityState = oldState;
		}
		StateManager.StateChanging(this, newState);
		if (newState == EntityState.Unchanged && oldState == EntityState.Modified)
		{
			if (acceptChanges)
			{
				_originalValues.AcceptChanges(this);
			}
			else
			{
				_originalValues.RejectChanges(this);
			}
		}
		SetServiceProperties(oldState, newState);
		_stateData.EntityState = newState;
		if (oldState == EntityState.Detached)
		{
			StateManager.StartTracking(this);
		}
		else if (newState == EntityState.Detached)
		{
			StateManager.StopTracking(this, oldState);
		}
		if ((newState == EntityState.Deleted || newState == EntityState.Detached) && HasConceptualNull)
		{
			_stateData.FlagAllProperties(EntityType.PropertyCount(), PropertyFlag.Null, flagged: false);
		}
		if (oldState == EntityState.Detached || oldState == EntityState.Unchanged)
		{
			if (newState == EntityState.Added || newState == EntityState.Deleted || newState == EntityState.Modified)
			{
				StateManager.ChangedCount++;
			}
		}
		else if (newState == EntityState.Detached || newState == EntityState.Unchanged)
		{
			StateManager.ChangedCount--;
		}
		FireStateChanged(oldState);
		if ((newState == EntityState.Deleted || newState == EntityState.Detached) && StateManager.CascadeDeleteTiming == CascadeTiming.Immediate)
		{
			StateManager.CascadeDelete(this, force: false);
		}
	}

	private void FireStateChanged(EntityState oldState)
	{
		StateManager.InternalEntityEntryNotifier.StateChanged(this, oldState, fromQuery: false);
		if (oldState != 0)
		{
			StateManager.OnStateChanged(this, oldState);
		}
		else
		{
			StateManager.OnTracked(this, fromQuery: false);
		}
	}

	private void SetServiceProperties(EntityState oldState, EntityState newState)
	{
		if (oldState == EntityState.Detached)
		{
			foreach (ServiceProperty serviceProperty in ((EntityType)EntityType).GetServiceProperties())
			{
				this[serviceProperty] = serviceProperty.GetParameterBinding().ServiceDelegate(new MaterializationContext(in ValueBuffer.Empty, StateManager.Context), EntityType, Entity);
			}
			return;
		}
		if (newState != 0)
		{
			return;
		}
		foreach (ServiceProperty serviceProperty2 in ((EntityType)EntityType).GetServiceProperties())
		{
			this[serviceProperty2] = null;
		}
	}

	public virtual void MarkUnchangedFromQuery()
	{
		StateManager.InternalEntityEntryNotifier.StateChanging(this, EntityState.Unchanged);
		_stateData.EntityState = EntityState.Unchanged;
		StateManager.InternalEntityEntryNotifier.StateChanged(this, EntityState.Detached, fromQuery: true);
		StateManager.OnTracked(this, fromQuery: true);
		StateManager.InternalEntityEntryNotifier.TrackedFromQuery(this);
	}

	public virtual bool IsModified(IProperty property)
	{
		int index = property.GetIndex();
		if (_stateData.EntityState == EntityState.Modified && _stateData.IsPropertyFlagged(index, PropertyFlag.Modified))
		{
			return !_stateData.IsPropertyFlagged(index, PropertyFlag.Unknown);
		}
		return false;
	}

	public virtual void SetPropertyModified([JetBrains.Annotations.NotNull] IProperty property, bool changeState = true, bool isModified = true, bool isConceptualNull = false, bool acceptChanges = false)
	{
		int index = property.GetIndex();
		_stateData.FlagProperty(index, PropertyFlag.Unknown, isFlagged: false);
		EntityState entityState = _stateData.EntityState;
		if (entityState == EntityState.Added || entityState == EntityState.Detached || !changeState)
		{
			int originalValueIndex = property.GetOriginalValueIndex();
			if (originalValueIndex != -1 && !IsConceptualNull(property))
			{
				SetOriginalValue(property, this[property], originalValueIndex);
			}
		}
		if (entityState == EntityState.Added)
		{
			return;
		}
		if (changeState && !isConceptualNull && isModified && !StateManager.SavingChanges && property.IsKey() && property.GetAfterSaveBehavior() == PropertySaveBehavior.Throw)
		{
			throw new InvalidOperationException(CoreStrings.KeyReadOnly(property.Name, EntityType.DisplayName()));
		}
		if (entityState == EntityState.Deleted)
		{
			return;
		}
		if (changeState)
		{
			if (!isModified && entityState != 0 && property.GetOriginalValueIndex() != -1)
			{
				if (acceptChanges)
				{
					SetOriginalValue(property, GetCurrentValue(property));
				}
				SetProperty(property, GetOriginalValue(property), isMaterialization: false, setModified: false);
			}
			_stateData.FlagProperty(index, PropertyFlag.Modified, isModified);
		}
		if (isModified && (entityState == EntityState.Unchanged || entityState == EntityState.Detached))
		{
			if (changeState)
			{
				StateManager.StateChanging(this, EntityState.Modified);
				SetServiceProperties(entityState, EntityState.Modified);
				_stateData.EntityState = EntityState.Modified;
				if (entityState == EntityState.Detached)
				{
					StateManager.StartTracking(this);
				}
			}
			if (changeState)
			{
				StateManager.ChangedCount++;
				FireStateChanged(entityState);
			}
		}
		else if (entityState == EntityState.Modified && changeState && !isModified && !_stateData.AnyPropertiesFlagged(PropertyFlag.Modified))
		{
			StateManager.StateChanging(this, EntityState.Unchanged);
			_stateData.EntityState = EntityState.Unchanged;
			StateManager.ChangedCount--;
			FireStateChanged(entityState);
		}
	}

	public virtual bool IsConceptualNull([JetBrains.Annotations.NotNull] IProperty property)
	{
		return _stateData.IsPropertyFlagged(property.GetIndex(), PropertyFlag.Null);
	}

	public virtual bool HasTemporaryValue(IProperty property)
	{
		return GetValueType(property) == CurrentValueType.Temporary;
	}

	private CurrentValueType GetValueType(IProperty property, Func<object, object, bool> equals = null)
	{
		int storeGeneratedIndex = property.GetStoreGeneratedIndex();
		if (storeGeneratedIndex == -1)
		{
			return CurrentValueType.Normal;
		}
		if (equals == null)
		{
			equals = ValuesEqualFunc(property);
		}
		object defaultValue = property.ClrType.GetDefaultValue();
		object value = ReadPropertyValue(property);
		if (!equals(value, defaultValue))
		{
			return CurrentValueType.Normal;
		}
		if (_storeGeneratedValues.TryGetValue(storeGeneratedIndex, out value) && !equals(value, defaultValue))
		{
			return CurrentValueType.StoreGenerated;
		}
		if (_temporaryValues.TryGetValue(storeGeneratedIndex, out value) && !equals(value, defaultValue))
		{
			return CurrentValueType.Temporary;
		}
		return CurrentValueType.Normal;
	}

	public virtual void SetTemporaryValue([JetBrains.Annotations.NotNull] IProperty property, object value, bool setModified = true)
	{
		if (property.GetStoreGeneratedIndex() == -1)
		{
			throw new InvalidOperationException(CoreStrings.TempValue(property.Name, EntityType.DisplayName()));
		}
		SetProperty(property, value, isMaterialization: false, setModified, isCascadeDelete: false, CurrentValueType.Temporary);
	}

	public virtual void SetStoreGeneratedValue(IProperty property, object value)
	{
		if (property.GetStoreGeneratedIndex() == -1)
		{
			throw new InvalidOperationException(CoreStrings.StoreGenValue(property.Name, EntityType.DisplayName()));
		}
		SetProperty(property, value, isMaterialization: false, setModified: true, isCascadeDelete: false, CurrentValueType.StoreGenerated);
	}

	protected virtual void MarkShadowPropertiesNotSet([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		foreach (IProperty property in entityType.GetProperties())
		{
			if (property.IsShadowProperty())
			{
				_stateData.FlagProperty(property.GetIndex(), PropertyFlag.Unknown, isFlagged: true);
			}
		}
	}

	[JetBrains.Annotations.UsedImplicitly]
	protected virtual T ReadShadowValue<T>(int shadowIndex)
	{
		return default(T);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private T ReadOriginalValue<T>(IProperty property, int originalValueIndex)
	{
		return _originalValues.GetValue<T>(this, property, originalValueIndex);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private T ReadRelationshipSnapshotValue<T>(IPropertyBase propertyBase, int relationshipSnapshotIndex)
	{
		return _relationshipsSnapshot.GetValue<T>(this, propertyBase, relationshipSnapshotIndex);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private T ReadStoreGeneratedValue<T>(int storeGeneratedIndex)
	{
		return _storeGeneratedValues.GetValue<T>(storeGeneratedIndex);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private T ReadTemporaryValue<T>(int storeGeneratedIndex)
	{
		return _temporaryValues.GetValue<T>(storeGeneratedIndex);
	}

	public virtual TProperty GetCurrentValue<TProperty>(IPropertyBase propertyBase)
	{
		return ((Func<InternalEntityEntry, TProperty>)propertyBase.GetPropertyAccessors().CurrentValueGetter)(this);
	}

	public virtual TProperty GetOriginalValue<TProperty>(IProperty property)
	{
		return ((Func<InternalEntityEntry, TProperty>)property.GetPropertyAccessors().OriginalValueGetter)(this);
	}

	public virtual TProperty GetRelationshipSnapshotValue<TProperty>([JetBrains.Annotations.NotNull] IPropertyBase propertyBase)
	{
		return ((Func<InternalEntityEntry, TProperty>)propertyBase.GetPropertyAccessors().RelationshipSnapshotGetter)(this);
	}

	protected virtual object ReadPropertyValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase)
	{
		return ((PropertyBase)propertyBase).Getter.GetClrValue(Entity);
	}

	protected virtual bool PropertyHasDefaultValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase)
	{
		return ((PropertyBase)propertyBase).Getter.HasDefaultValue(Entity);
	}

	protected virtual void WritePropertyValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.CanBeNull] object value, bool forMaterialization)
	{
		PropertyBase propertyBase2 = (PropertyBase)propertyBase;
		(forMaterialization ? propertyBase2.MaterializationSetter : propertyBase2.Setter).SetClrValue(Entity, value);
	}

	public virtual object GetOrCreateCollection([JetBrains.Annotations.NotNull] INavigation navigation, bool forMaterialization)
	{
		return ((Navigation)navigation).CollectionAccessor.GetOrCreate(Entity, forMaterialization);
	}

	public virtual bool CollectionContains([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry value)
	{
		return ((Navigation)navigation).CollectionAccessor.Contains(Entity, value.Entity);
	}

	public virtual bool AddToCollection([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry value, bool forMaterialization)
	{
		return ((Navigation)navigation).CollectionAccessor.Add(Entity, value.Entity, forMaterialization);
	}

	public virtual bool RemoveFromCollection([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry value)
	{
		return ((Navigation)navigation).CollectionAccessor.Remove(Entity, value.Entity);
	}

	public virtual object GetCurrentValue(IPropertyBase propertyBase)
	{
		if (propertyBase is IProperty property && IsConceptualNull(property))
		{
			return null;
		}
		return this[propertyBase];
	}

	public virtual object GetPreStoreGeneratedCurrentValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase)
	{
		if (propertyBase is IProperty property && IsConceptualNull(property))
		{
			return null;
		}
		return ReadPropertyValue(propertyBase);
	}

	public virtual object GetOriginalValue(IPropertyBase propertyBase)
	{
		return _originalValues.GetValue(this, (IProperty)propertyBase);
	}

	public virtual object GetRelationshipSnapshotValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase)
	{
		return _relationshipsSnapshot.GetValue(this, propertyBase);
	}

	public virtual void SetOriginalValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.CanBeNull] object value, int index = -1)
	{
		EnsureOriginalValues();
		Property property = (Property)propertyBase;
		_originalValues.SetValue(property, value, index);
		if (EntityState == EntityState.Unchanged || (EntityState == EntityState.Modified && !IsModified(property)))
		{
			object arg = this[propertyBase];
			int index2 = property.GetIndex();
			if (!ValuesEqualFunc(property)(arg, value) && !_stateData.IsPropertyFlagged(index2, PropertyFlag.Unknown))
			{
				SetPropertyModified(property);
			}
		}
	}

	public virtual void SetRelationshipSnapshotValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.CanBeNull] object value)
	{
		EnsureRelationshipSnapshot();
		_relationshipsSnapshot.SetValue(propertyBase, value);
	}

	public virtual void EnsureOriginalValues()
	{
		if (_originalValues.IsEmpty)
		{
			_originalValues = new OriginalValues(this);
		}
	}

	public virtual void EnsureTemporaryValues()
	{
		if (_temporaryValues.IsEmpty)
		{
			_temporaryValues = new SidecarValues(((EntityType)EntityType).TemporaryValuesFactory(this));
		}
	}

	public virtual void EnsureStoreGeneratedValues()
	{
		if (_storeGeneratedValues.IsEmpty)
		{
			_storeGeneratedValues = new SidecarValues(((EntityType)EntityType).StoreGeneratedValuesFactory(this));
		}
	}

	public virtual void EnsureRelationshipSnapshot()
	{
		if (_relationshipsSnapshot.IsEmpty)
		{
			_relationshipsSnapshot = new RelationshipsSnapshot(this);
		}
	}

	public virtual void RemoveFromCollectionSnapshot([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.NotNull] object removedEntity)
	{
		EnsureRelationshipSnapshot();
		_relationshipsSnapshot.RemoveFromCollection(propertyBase, removedEntity);
	}

	public virtual void AddToCollectionSnapshot([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.NotNull] object addedEntity)
	{
		EnsureRelationshipSnapshot();
		_relationshipsSnapshot.AddToCollection(propertyBase, addedEntity);
	}

	public virtual void AddRangeToCollectionSnapshot([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.NotNull] IEnumerable<object> addedEntities)
	{
		EnsureRelationshipSnapshot();
		_relationshipsSnapshot.AddRangeToCollection(propertyBase, addedEntities);
	}

	public virtual void SetProperty([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.CanBeNull] object value, bool isMaterialization, bool setModified = true, bool isCascadeDelete = false)
	{
		SetProperty(propertyBase, value, isMaterialization, setModified, isCascadeDelete, CurrentValueType.Normal);
	}

	private void SetProperty([JetBrains.Annotations.NotNull] IPropertyBase propertyBase, [JetBrains.Annotations.CanBeNull] object value, bool isMaterialization, bool setModified, bool isCascadeDelete, CurrentValueType valueType)
	{
		object arg = this[propertyBase];
		Property property = propertyBase as Property;
		int num;
		Func<object, object, bool> func;
		CurrentValueType currentValueType;
		if (property != null)
		{
			num = property.GetIndex();
			func = ValuesEqualFunc(property);
			currentValueType = GetValueType(property, func);
		}
		else
		{
			num = -1;
			func = object.ReferenceEquals;
			currentValueType = CurrentValueType.Normal;
		}
		if (func(arg, value) && (num == -1 || (!_stateData.IsPropertyFlagged(num, PropertyFlag.Unknown) && !_stateData.IsPropertyFlagged(num, PropertyFlag.Null) && valueType == currentValueType)))
		{
			return;
		}
		bool flag = true;
		if (property != null && valueType == CurrentValueType.Normal && (!property.ClrType.IsNullableType() || property.GetContainingForeignKeys().Any((ForeignKey fk) => (fk.DeleteBehavior == DeleteBehavior.Cascade || fk.DeleteBehavior == DeleteBehavior.ClientCascade) && fk.DeclaringEntityType.IsAssignableFrom(EntityType))))
		{
			if (value == null)
			{
				if (EntityState != EntityState.Deleted && EntityState != 0)
				{
					_stateData.FlagProperty(num, PropertyFlag.Null, isFlagged: true);
					if (setModified)
					{
						SetPropertyModified(property, changeState: true, isModified: true, isConceptualNull: true);
					}
					if (!isCascadeDelete && StateManager.DeleteOrphansTiming == CascadeTiming.Immediate)
					{
						HandleConceptualNulls(StateManager.SensitiveLoggingEnabled, force: false, isCascadeDelete: false);
					}
				}
				flag = false;
			}
			else
			{
				_stateData.FlagProperty(num, PropertyFlag.Null, isFlagged: false);
			}
		}
		if (!flag)
		{
			return;
		}
		StateManager.InternalEntityEntryNotifier.PropertyChanging(this, propertyBase);
		if (valueType == CurrentValueType.Normal)
		{
			WritePropertyValue(propertyBase, value, isMaterialization);
			if (!AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue19137", out var isEnabled) || !isEnabled)
			{
				if (currentValueType != 0 && !_temporaryValues.IsEmpty)
				{
					object defaultValue = property.ClrType.GetDefaultValue();
					int storeGeneratedIndex = property.GetStoreGeneratedIndex();
					_temporaryValues.SetValue(property, defaultValue, storeGeneratedIndex);
				}
			}
			else if (currentValueType != 0 && !_temporaryValues.IsEmpty && func(value, property.ClrType.GetDefaultValue()))
			{
				int storeGeneratedIndex2 = property.GetStoreGeneratedIndex();
				_temporaryValues.SetValue(property, value, storeGeneratedIndex2);
			}
		}
		else
		{
			int storeGeneratedIndex3 = property.GetStoreGeneratedIndex();
			if (valueType == CurrentValueType.StoreGenerated)
			{
				EnsureStoreGeneratedValues();
				_storeGeneratedValues.SetValue(property, value, storeGeneratedIndex3);
			}
			else
			{
				object defaultValue2 = property.ClrType.GetDefaultValue();
				if (!func(arg, defaultValue2))
				{
					WritePropertyValue(property, defaultValue2, isMaterialization);
				}
				if (_storeGeneratedValues.TryGetValue(storeGeneratedIndex3, out var value2) && !func(value2, defaultValue2))
				{
					_storeGeneratedValues.SetValue(property, defaultValue2, storeGeneratedIndex3);
				}
				EnsureTemporaryValues();
				_temporaryValues.SetValue(property, value, storeGeneratedIndex3);
			}
		}
		if (num != -1)
		{
			_stateData.FlagProperty(num, PropertyFlag.Unknown, isFlagged: false);
		}
		if (propertyBase is INavigation navigation && !navigation.IsCollection())
		{
			SetIsLoaded(navigation, value != null);
		}
		StateManager.InternalEntityEntryNotifier.PropertyChanged(this, propertyBase, setModified);
	}

	private static Func<object, object, bool> ValuesEqualFunc(IProperty property)
	{
		ValueComparer comparer = property.GetValueComparer() ?? property.FindTypeMapping()?.Comparer;
		object obj;
		if (comparer == null)
		{
			obj = _003C_003Ec._003C_003E9__86_1;
			if (obj == null)
			{
				return _003C_003Ec._003C_003E9__86_1 = (object l, object r) => object.Equals(l, r);
			}
		}
		else
		{
			obj = (Func<object, object, bool>)((object l, object r) => comparer.Equals(l, r));
		}
		return (Func<object, object, bool>)obj;
	}

	public virtual void AcceptChanges()
	{
		if (!_storeGeneratedValues.IsEmpty)
		{
			foreach (IProperty property in EntityType.GetProperties())
			{
				int storeGeneratedIndex = property.GetStoreGeneratedIndex();
				if (storeGeneratedIndex != -1 && _storeGeneratedValues.TryGetValue(storeGeneratedIndex, out var value))
				{
					this[property] = value;
				}
			}
			_storeGeneratedValues = default(SidecarValues);
			_temporaryValues = default(SidecarValues);
		}
		switch (EntityState)
		{
		case EntityState.Modified:
		case EntityState.Added:
			_originalValues.AcceptChanges(this);
			SharedIdentityEntry?.AcceptChanges();
			SetEntityState(EntityState.Unchanged, acceptChanges: true);
			break;
		case EntityState.Deleted:
			SetEntityState(EntityState.Detached);
			break;
		}
	}

	public virtual InternalEntityEntry PrepareToSave()
	{
		EntityType entityType = (EntityType)EntityType;
		if (EntityState == EntityState.Added)
		{
			foreach (Property property in entityType.GetProperties())
			{
				if (property.GetBeforeSaveBehavior() == PropertySaveBehavior.Throw && !HasTemporaryValue(property) && !HasDefaultValue(property))
				{
					throw new InvalidOperationException(CoreStrings.PropertyReadOnlyBeforeSave(property.Name, EntityType.DisplayName()));
				}
			}
		}
		else if (EntityState == EntityState.Modified)
		{
			foreach (Property property2 in entityType.GetProperties())
			{
				if (property2.GetAfterSaveBehavior() == PropertySaveBehavior.Throw && IsModified(property2))
				{
					throw new InvalidOperationException(CoreStrings.PropertyReadOnlyAfterSave(property2.Name, EntityType.DisplayName()));
				}
			}
		}
		DiscardStoreGeneratedValues();
		return this;
	}

	public virtual void HandleConceptualNulls(bool sensitiveLoggingEnabled, bool force, bool isCascadeDelete)
	{
		List<IForeignKey> list = new List<IForeignKey>();
		foreach (IForeignKey foreignKey3 in EntityType.GetForeignKeys())
		{
			IReadOnlyList<IProperty> properties = foreignKey3.Properties;
			foreach (IProperty item in properties)
			{
				if (!_stateData.IsPropertyFlagged(item.GetIndex(), PropertyFlag.Null))
				{
					continue;
				}
				if (properties.Any((IProperty p) => p.IsNullable) && foreignKey3.DeleteBehavior != DeleteBehavior.Cascade && foreignKey3.DeleteBehavior != DeleteBehavior.ClientCascade)
				{
					foreach (IProperty item2 in properties)
					{
						if (item2.IsNullable)
						{
							this[item2] = null;
						}
						else
						{
							_stateData.FlagProperty(item2.GetIndex(), PropertyFlag.Null, isFlagged: false);
						}
					}
				}
				else if (EntityState != EntityState.Modified || IsModified(item))
				{
					list.Add(foreignKey3);
				}
				break;
			}
		}
		IForeignKey foreignKey = list.FirstOrDefault((IForeignKey fk) => fk.DeleteBehavior == DeleteBehavior.Cascade || fk.DeleteBehavior == DeleteBehavior.ClientCascade);
		if (foreignKey != null && (force || (!isCascadeDelete && StateManager.DeleteOrphansTiming != CascadeTiming.Never)))
		{
			EntityState entityState = ((EntityState != EntityState.Added) ? EntityState.Deleted : EntityState.Detached);
			if (StateManager.SensitiveLoggingEnabled)
			{
				StateManager.UpdateLogger.CascadeDeleteOrphanSensitive(this, foreignKey.PrincipalEntityType, entityState);
			}
			else
			{
				StateManager.UpdateLogger.CascadeDeleteOrphan(this, foreignKey.PrincipalEntityType, entityState);
			}
			SetEntityState(entityState);
			return;
		}
		if (list.Count > 0)
		{
			IForeignKey foreignKey2 = list.First();
			if (sensitiveLoggingEnabled)
			{
				throw new InvalidOperationException(CoreStrings.RelationshipConceptualNullSensitive(foreignKey2.PrincipalEntityType.DisplayName(), EntityType.DisplayName(), this.BuildOriginalValuesString(foreignKey2.Properties)));
			}
			throw new InvalidOperationException(CoreStrings.RelationshipConceptualNull(foreignKey2.PrincipalEntityType.DisplayName(), EntityType.DisplayName()));
		}
		IProperty property = EntityType.GetProperties().FirstOrDefault((IProperty p) => (EntityState != EntityState.Modified || IsModified(p)) && _stateData.IsPropertyFlagged(p.GetIndex(), PropertyFlag.Null));
		if (property != null)
		{
			if (sensitiveLoggingEnabled)
			{
				throw new InvalidOperationException(CoreStrings.PropertyConceptualNullSensitive(property.Name, EntityType.DisplayName(), this.BuildOriginalValuesString(new IProperty[1] { property })));
			}
			throw new InvalidOperationException(CoreStrings.PropertyConceptualNull(property.Name, EntityType.DisplayName()));
		}
	}

	public virtual void DiscardStoreGeneratedValues()
	{
		if (!_storeGeneratedValues.IsEmpty)
		{
			_storeGeneratedValues = default(SidecarValues);
		}
	}

	public virtual bool IsStoreGenerated(IProperty property)
	{
		if (!property.ValueGenerated.ForAdd() || EntityState != EntityState.Added || (property.GetBeforeSaveBehavior() != PropertySaveBehavior.Ignore && !HasTemporaryValue(property) && !HasDefaultValue(property)))
		{
			if (property.ValueGenerated.ForUpdate() && EntityState == EntityState.Modified)
			{
				if (property.GetAfterSaveBehavior() != PropertySaveBehavior.Ignore)
				{
					return !IsModified(property);
				}
				return true;
			}
			return false;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasDefaultValue(IProperty property)
	{
		if (!PropertyHasDefaultValue(property))
		{
			return false;
		}
		int storeGeneratedIndex = property.GetStoreGeneratedIndex();
		if (storeGeneratedIndex == -1)
		{
			return true;
		}
		object defaultValue = property.ClrType.GetDefaultValue();
		Func<object, object, bool> func = ValuesEqualFunc(property);
		if (_storeGeneratedValues.TryGetValue(storeGeneratedIndex, out var value) && !func(defaultValue, value))
		{
			return false;
		}
		if (_temporaryValues.TryGetValue(storeGeneratedIndex, out value) && !func(defaultValue, value))
		{
			return false;
		}
		return true;
	}

	public virtual EntityEntry ToEntityEntry()
	{
		return new EntityEntry(this);
	}

	public virtual void HandleINotifyPropertyChanging([JetBrains.Annotations.NotNull] object sender, [JetBrains.Annotations.NotNull] PropertyChangingEventArgs eventArgs)
	{
		foreach (IPropertyBase notificationProperty in EntityType.GetNotificationProperties(eventArgs.PropertyName))
		{
			StateManager.InternalEntityEntryNotifier.PropertyChanging(this, notificationProperty);
		}
	}

	public virtual void HandleINotifyPropertyChanged([JetBrains.Annotations.NotNull] object sender, [JetBrains.Annotations.NotNull] PropertyChangedEventArgs eventArgs)
	{
		foreach (IPropertyBase notificationProperty in EntityType.GetNotificationProperties(eventArgs.PropertyName))
		{
			StateManager.InternalEntityEntryNotifier.PropertyChanged(this, notificationProperty, setModified: true);
		}
	}

	public virtual void HandleINotifyCollectionChanged([JetBrains.Annotations.NotNull] object sender, [JetBrains.Annotations.NotNull] NotifyCollectionChangedEventArgs eventArgs)
	{
		INavigation navigation = EntityType.GetNavigations().FirstOrDefault((INavigation n) => n.IsCollection() && this[n] == sender);
		if (navigation != null)
		{
			switch (eventArgs.Action)
			{
			case NotifyCollectionChangedAction.Add:
				StateManager.InternalEntityEntryNotifier.NavigationCollectionChanged(this, navigation, eventArgs.NewItems.OfType<object>(), Enumerable.Empty<object>());
				break;
			case NotifyCollectionChangedAction.Remove:
				StateManager.InternalEntityEntryNotifier.NavigationCollectionChanged(this, navigation, Enumerable.Empty<object>(), eventArgs.OldItems.OfType<object>());
				break;
			case NotifyCollectionChangedAction.Replace:
				StateManager.InternalEntityEntryNotifier.NavigationCollectionChanged(this, navigation, eventArgs.NewItems.OfType<object>(), eventArgs.OldItems.OfType<object>());
				break;
			case NotifyCollectionChangedAction.Reset:
				throw new InvalidOperationException(CoreStrings.ResetNotSupported);
			case NotifyCollectionChangedAction.Move:
				break;
			}
		}
	}

	public virtual void SetIsLoaded([JetBrains.Annotations.NotNull] INavigation navigation, bool loaded = true)
	{
		if (!loaded && !navigation.IsCollection() && this[navigation] != null)
		{
			throw new InvalidOperationException(CoreStrings.ReferenceMustBeLoaded(navigation.Name, navigation.DeclaringEntityType.DisplayName()));
		}
		_stateData.FlagProperty(navigation.GetIndex(), PropertyFlag.IsLoaded, loaded);
		IServiceProperty serviceProperty = EntityType.GetServiceProperties().FirstOrDefault((IServiceProperty p) => p.ClrType == typeof(ILazyLoader));
		if (serviceProperty != null)
		{
			((ILazyLoader)this[serviceProperty])?.SetLoaded(Entity, navigation.Name, loaded);
		}
	}

	public virtual bool IsLoaded([JetBrains.Annotations.NotNull] INavigation navigation)
	{
		return _stateData.IsPropertyFlagged(navigation.GetIndex(), PropertyFlag.IsLoaded);
	}

	public override string ToString()
	{
		return $"{this.BuildCurrentValuesString(EntityType.FindPrimaryKey().Properties)} {EntityState}" + string.Format("{0} {1}", (((IUpdateEntry)this).SharedIdentityEntry == null) ? "" : " Shared", EntityType);
	}
}
