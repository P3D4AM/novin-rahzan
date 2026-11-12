using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Abstractions;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Routing;

public class RouteValueDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, IReadOnlyDictionary<string, object>, IReadOnlyCollection<KeyValuePair<string, object>>
{
	public struct Enumerator : IEnumerator<KeyValuePair<string, object>>, IEnumerator, IDisposable
	{
		private readonly Storage _storage;

		private int _index;

		public KeyValuePair<string, object> Current { get; private set; }

		object IEnumerator.Current => Current;

		public Enumerator(RouteValueDictionary dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException();
			}
			_storage = dictionary._storage;
			Current = default(KeyValuePair<string, object>);
			_index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (++_index < _storage.Count)
			{
				Current = _storage[_index];
				return true;
			}
			Current = default(KeyValuePair<string, object>);
			return false;
		}

		public void Reset()
		{
			Current = default(KeyValuePair<string, object>);
			_index = -1;
		}
	}

	internal abstract class Storage
	{
		public abstract int Count { get; }

		public abstract KeyValuePair<string, object> this[int index] { get; set; }

		public abstract void Upgrade(ref Storage storage);

		public abstract bool TryGetValue(string key, out object value);

		public abstract bool ContainsKey(string key);

		public abstract bool TrySetValue(string key, object value);
	}

	internal class ListStorage : Storage
	{
		private KeyValuePair<string, object>[] _items;

		private int _count;

		private static readonly KeyValuePair<string, object>[] _emptyArray = new KeyValuePair<string, object>[0];

		public int Capacity => _items.Length;

		public override int Count => _count;

		public override KeyValuePair<string, object> this[int index]
		{
			get
			{
				if (index < 0 || index >= _count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return _items[index];
			}
			set
			{
				if (index < 0 || index >= _count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				_items[index] = value;
			}
		}

		public ListStorage()
		{
			_items = _emptyArray;
		}

		public ListStorage(int capacity)
		{
			if (capacity == 0)
			{
				_items = _emptyArray;
			}
			else
			{
				_items = new KeyValuePair<string, object>[capacity];
			}
		}

		public ListStorage(ListStorage other)
		{
			if (other.Count == 0)
			{
				_items = _emptyArray;
				return;
			}
			_items = new KeyValuePair<string, object>[other.Count];
			for (int i = 0; i < other.Count; i++)
			{
				Add(other[i]);
			}
		}

		public void Add(KeyValuePair<string, object> item)
		{
			if (_count == _items.Length)
			{
				EnsureCapacity(_count + 1);
			}
			_items[_count++] = item;
		}

		public void RemoveAt(int index)
		{
			_count--;
			for (int i = index; i < _count; i++)
			{
				_items[i] = _items[i + 1];
			}
			_items[_count] = default(KeyValuePair<string, object>);
		}

		public void Clear()
		{
			for (int i = 0; i < _count; i++)
			{
				_items[i] = default(KeyValuePair<string, object>);
			}
			_count = 0;
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			for (int i = 0; i < _count; i++)
			{
				array[arrayIndex++] = _items[i];
			}
		}

		public override bool ContainsKey(string key)
		{
			for (int i = 0; i < Count; i++)
			{
				KeyValuePair<string, object> keyValuePair = _items[i];
				if (string.Equals(key, keyValuePair.Key, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public override bool TrySetValue(string key, object value)
		{
			for (int i = 0; i < Count; i++)
			{
				KeyValuePair<string, object> keyValuePair = _items[i];
				if (string.Equals(key, keyValuePair.Key, StringComparison.OrdinalIgnoreCase))
				{
					_items[i] = new KeyValuePair<string, object>(key, value);
					return true;
				}
			}
			Add(new KeyValuePair<string, object>(key, value));
			return true;
		}

		public override bool TryGetValue(string key, out object value)
		{
			for (int i = 0; i < Count; i++)
			{
				KeyValuePair<string, object> keyValuePair = _items[i];
				if (string.Equals(key, keyValuePair.Key, StringComparison.OrdinalIgnoreCase))
				{
					value = keyValuePair.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public override void Upgrade(ref Storage storage)
		{
		}

		private void EnsureCapacity(int min)
		{
			KeyValuePair<string, object>[] array = new KeyValuePair<string, object>[(_items.Length == 0) ? 4 : (_items.Length * 2)];
			for (int i = 0; i < _count; i++)
			{
				array[i] = _items[i];
			}
			_items = array;
		}
	}

	internal class PropertyStorage : Storage
	{
		private static readonly PropertyCache _propertyCache = new PropertyCache();

		internal readonly object _value;

		internal readonly PropertyHelper[] _properties;

		public override int Count => _properties.Length;

		public override KeyValuePair<string, object> this[int index]
		{
			get
			{
				PropertyHelper propertyHelper = _properties[index];
				return new KeyValuePair<string, object>(propertyHelper.Name, propertyHelper.GetValue(_value));
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public PropertyStorage(object value)
		{
			_value = value;
			Type type = _value.GetType();
			if (!_propertyCache.TryGetValue(type, out _properties))
			{
				_properties = PropertyHelper.GetVisibleProperties(type);
				ValidatePropertyNames(type, _properties);
				_propertyCache.TryAdd(type, _properties);
			}
		}

		public PropertyStorage(PropertyStorage propertyStorage)
		{
			_value = propertyStorage._value;
			_properties = propertyStorage._properties;
		}

		public override bool TryGetValue(string key, out object value)
		{
			for (int i = 0; i < _properties.Length; i++)
			{
				PropertyHelper propertyHelper = _properties[i];
				if (string.Equals(key, propertyHelper.Name, StringComparison.OrdinalIgnoreCase))
				{
					value = propertyHelper.GetValue(_value);
					return true;
				}
			}
			value = null;
			return false;
		}

		public override bool ContainsKey(string key)
		{
			for (int i = 0; i < _properties.Length; i++)
			{
				PropertyHelper propertyHelper = _properties[i];
				if (string.Equals(key, propertyHelper.Name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public override bool TrySetValue(string key, object value)
		{
			return false;
		}

		public override void Upgrade(ref Storage storage)
		{
			storage = new ListStorage(Count);
			for (int i = 0; i < _properties.Length; i++)
			{
				PropertyHelper propertyHelper = _properties[i];
				storage.TrySetValue(propertyHelper.Name, propertyHelper.GetValue(_value));
			}
		}

		private static void ValidatePropertyNames(Type type, PropertyHelper[] properties)
		{
			Dictionary<string, PropertyHelper> dictionary = new Dictionary<string, PropertyHelper>(StringComparer.OrdinalIgnoreCase);
			foreach (PropertyHelper propertyHelper in properties)
			{
				if (dictionary.TryGetValue(propertyHelper.Name, out var value))
				{
					throw new InvalidOperationException(Resources.FormatRouteValueDictionary_DuplicatePropertyName(type.FullName, propertyHelper.Name, value.Name, "RouteValueDictionary"));
				}
				dictionary.Add(propertyHelper.Name, propertyHelper);
			}
		}
	}

	internal class EmptyStorage : Storage
	{
		public static readonly EmptyStorage Instance = new EmptyStorage();

		public override int Count => 0;

		public override KeyValuePair<string, object> this[int index]
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		private EmptyStorage()
		{
		}

		public override bool ContainsKey(string key)
		{
			return false;
		}

		public override bool TryGetValue(string key, out object value)
		{
			value = null;
			return false;
		}

		public override bool TrySetValue(string key, object value)
		{
			return false;
		}

		public override void Upgrade(ref Storage storage)
		{
			storage = new ListStorage();
		}
	}

	private class PropertyCache : ConcurrentDictionary<Type, PropertyHelper[]>
	{
	}

	internal Storage _storage;

	public object this[string key]
	{
		get
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			TryGetValue(key, out var value);
			return value;
		}
		set
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (!_storage.TrySetValue(key, value))
			{
				Upgrade();
				_storage.TrySetValue(key, value);
			}
		}
	}

	public IEqualityComparer<string> Comparer => StringComparer.OrdinalIgnoreCase;

	public int Count => _storage.Count;

	bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;

	public ICollection<string> Keys
	{
		get
		{
			Upgrade();
			ListStorage listStorage = (ListStorage)_storage;
			string[] array = new string[listStorage.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = listStorage[i].Key;
			}
			return array;
		}
	}

	IEnumerable<string> IReadOnlyDictionary<string, object>.Keys => Keys;

	public ICollection<object> Values
	{
		get
		{
			Upgrade();
			ListStorage listStorage = (ListStorage)_storage;
			object[] array = new object[listStorage.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = listStorage[i].Value;
			}
			return array;
		}
	}

	IEnumerable<object> IReadOnlyDictionary<string, object>.Values => Values;

	public RouteValueDictionary()
	{
		_storage = EmptyStorage.Instance;
	}

	public RouteValueDictionary(object values)
	{
		if (values is RouteValueDictionary routeValueDictionary)
		{
			if (routeValueDictionary._storage is ListStorage other)
			{
				_storage = new ListStorage(other);
			}
			else if (routeValueDictionary._storage is PropertyStorage)
			{
				_storage = routeValueDictionary._storage;
			}
			else
			{
				_storage = EmptyStorage.Instance;
			}
			return;
		}
		if (values is IEnumerable<KeyValuePair<string, object>> enumerable)
		{
			ListStorage listStorage = (ListStorage)(_storage = new ListStorage());
			{
				foreach (KeyValuePair<string, object> item in enumerable)
				{
					if (listStorage.ContainsKey(item.Key))
					{
						throw new ArgumentException(Resources.FormatRouteValueDictionary_DuplicateKey(item.Key, "RouteValueDictionary"), "values");
					}
					listStorage.Add(item);
				}
				return;
			}
		}
		if (values is IEnumerable<KeyValuePair<string, string>> enumerable2)
		{
			ListStorage listStorage2 = (ListStorage)(_storage = new ListStorage());
			{
				foreach (KeyValuePair<string, string> item2 in enumerable2)
				{
					if (listStorage2.ContainsKey(item2.Key))
					{
						throw new ArgumentException(Resources.FormatRouteValueDictionary_DuplicateKey(item2.Key, "RouteValueDictionary"), "values");
					}
					listStorage2.Add(new KeyValuePair<string, object>(item2.Key, item2.Value));
				}
				return;
			}
		}
		if (values != null)
		{
			_storage = new PropertyStorage(values);
		}
		else
		{
			_storage = EmptyStorage.Instance;
		}
	}

	void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
	{
		Add(item.Key, item.Value);
	}

	public void Add(string key, object value)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		Upgrade();
		ListStorage listStorage = (ListStorage)_storage;
		for (int i = 0; i < listStorage.Count; i++)
		{
			if (string.Equals(listStorage[i].Key, key, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(Resources.FormatRouteValueDictionary_DuplicateKey(key, "RouteValueDictionary"), "key");
			}
		}
		listStorage.Add(new KeyValuePair<string, object>(key, value));
	}

	public void Clear()
	{
		if (_storage.Count != 0)
		{
			Upgrade();
			((ListStorage)_storage).Clear();
		}
	}

	bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
	{
		if (_storage.Count == 0)
		{
			return false;
		}
		Upgrade();
		ListStorage listStorage = (ListStorage)_storage;
		for (int i = 0; i < listStorage.Count; i++)
		{
			if (string.Equals(listStorage[i].Key, item.Key, StringComparison.OrdinalIgnoreCase))
			{
				return EqualityComparer<object>.Default.Equals(listStorage[i].Value, item.Value);
			}
		}
		return false;
	}

	public bool ContainsKey(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		return _storage.ContainsKey(key);
	}

	void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (arrayIndex < 0 || arrayIndex > array.Length || array.Length - arrayIndex < Count)
		{
			throw new ArgumentOutOfRangeException("arrayIndex");
		}
		if (_storage.Count != 0)
		{
			Upgrade();
			((ListStorage)_storage).CopyTo(array, arrayIndex);
		}
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
	{
		if (_storage.Count == 0)
		{
			return false;
		}
		Upgrade();
		ListStorage listStorage = (ListStorage)_storage;
		for (int i = 0; i < listStorage.Count; i++)
		{
			if (string.Equals(listStorage[i].Key, item.Key, StringComparison.OrdinalIgnoreCase) && EqualityComparer<object>.Default.Equals(listStorage[i].Value, item.Value))
			{
				listStorage.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public bool Remove(string key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (_storage.Count == 0)
		{
			return false;
		}
		Upgrade();
		ListStorage listStorage = (ListStorage)_storage;
		for (int i = 0; i < listStorage.Count; i++)
		{
			if (string.Equals(listStorage[i].Key, key, StringComparison.OrdinalIgnoreCase))
			{
				listStorage.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public bool TryGetValue(string key, out object value)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		return _storage.TryGetValue(key, out value);
	}

	private void Upgrade()
	{
		_storage.Upgrade(ref _storage);
	}
}
