using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public static class Collection<T> where T : class, IMetadataObject
{
	private abstract class MergerOf2Base : IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		protected readonly IMetadataCollection<T> m_collection1;

		protected readonly IMetadataCollection<T> m_collection2;

		public int Count => m_collection1.Count + m_collection2.Count;

		public T this[string name]
		{
			get
			{
				T val = m_collection1[name];
				if (val == null)
				{
					return m_collection2[name];
				}
				return val;
			}
		}

		public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

		protected MergerOf2Base(IMetadataCollection<T> collection1, IMetadataCollection<T> collection2)
		{
			m_collection1 = collection1;
			m_collection2 = collection2;
		}

		public bool Contains(string name)
		{
			if (!m_collection1.Contains(name))
			{
				return m_collection2.Contains(name);
			}
			return true;
		}

		public bool Contains(T item)
		{
			if (!m_collection1.Contains(item))
			{
				return m_collection2.Contains(item);
			}
			return true;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			foreach (T item in m_collection1.FindAll(match))
			{
				yield return item;
			}
			foreach (T item2 in m_collection2.FindAll(match))
			{
				yield return item2;
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			foreach (T item in m_collection1.FindAll(name))
			{
				yield return item;
			}
			foreach (T item2 in m_collection2.FindAll(name))
			{
				yield return item2;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (T item in m_collection1)
			{
				yield return item;
			}
			foreach (T item2 in m_collection2)
			{
				yield return item2;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class MergerOf2 : MergerOf2Base, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable
	{
		public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		public new IMetadataObject this[string name] => base[name];

		public MergerOf2(IMetadataCollection<T> collection1, IMetadataCollection<T> collection2)
			: base(collection1, collection2)
		{
		}

		public bool Contains(IMetadataObject item)
		{
			if (!(item is T item2))
			{
				return false;
			}
			return base.Contains(item2);
		}

		public IEnumerable<IMetadataObject> FindAll(Predicate<IMetadataObject> match)
		{
			using IEnumerator<IMetadataObject> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T val = (T)enumerator.Current;
				if (match(val))
				{
					yield return val;
				}
			}
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
		{
			foreach (T item in FindAll(name))
			{
				yield return item;
			}
		}

		public new IEnumerator<IMetadataObject> GetEnumerator()
		{
			foreach (T item in m_collection1)
			{
				yield return item;
			}
			foreach (T item2 in m_collection2)
			{
				yield return item2;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return base.GetEnumerator();
		}
	}

	private abstract class MergerOfNBase : IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		protected readonly IMetadataCollection<T>[] m_collections;

		public int Count
		{
			get
			{
				int num = 0;
				for (int i = 0; i < m_collections.Length; i++)
				{
					num += m_collections[i].Count;
				}
				return num;
			}
		}

		public T this[string name]
		{
			get
			{
				for (int i = 0; i < m_collections.Length; i++)
				{
					T val = m_collections[i][name];
					if (val != null)
					{
						return val;
					}
				}
				return null;
			}
		}

		public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

		protected MergerOfNBase(IMetadataCollection<T>[] collections)
		{
			m_collections = collections;
		}

		public bool Contains(string name)
		{
			for (int i = 0; i < m_collections.Length; i++)
			{
				if (m_collections[i].Contains(name))
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(T item)
		{
			for (int i = 0; i < m_collections.Length; i++)
			{
				if (m_collections[i].Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			for (int i = 0; i < m_collections.Length; i++)
			{
				foreach (T item in m_collections[i].FindAll(match))
				{
					yield return item;
				}
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			for (int i = 0; i < m_collections.Length; i++)
			{
				foreach (T item in m_collections[i].FindAll(name))
				{
					yield return item;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < m_collections.Length; i++)
			{
				foreach (T item in m_collections[i])
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class MergerOfN : MergerOfNBase, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable
	{
		public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		public new IMetadataObject this[string name] => base[name];

		public MergerOfN(params IMetadataCollection<T>[] collections)
			: base(collections)
		{
		}

		public bool Contains(IMetadataObject item)
		{
			if (!(item is T item2))
			{
				return false;
			}
			return base.Contains(item2);
		}

		public IEnumerable<IMetadataObject> FindAll(Predicate<IMetadataObject> match)
		{
			using IEnumerator<IMetadataObject> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T val = (T)enumerator.Current;
				if (match(val))
				{
					yield return val;
				}
			}
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
		{
			foreach (T item in FindAll(name))
			{
				yield return item;
			}
		}

		public new IEnumerator<IMetadataObject> GetEnumerator()
		{
			for (int i = 0; i < m_collections.Length; i++)
			{
				foreach (T item in m_collections[i])
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return base.GetEnumerator();
		}
	}

	private sealed class WrapperCollection<TBase, TDerived> : IMetadataCollection<TBase>, IEnumerable<TBase>, IEnumerable where TBase : class, IMetadataObject where TDerived : class, TBase
	{
		private readonly IMetadataCollection<TDerived> m_collection;

		public int Count => m_collection.Count;

		public TBase this[string name] => (TBase)m_collection[name];

		public IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => Collection<IMetadataObject>.Convert(m_collection);

		public WrapperCollection(IMetadataCollection<TDerived> collection)
		{
			m_collection = collection;
		}

		public bool Contains(string name)
		{
			return m_collection.Contains(name);
		}

		public bool Contains(TBase item)
		{
			if (!(item is TDerived item2))
			{
				return false;
			}
			return m_collection.Contains(item2);
		}

		public IEnumerable<TBase> FindAll(Predicate<TBase> match)
		{
			foreach (TDerived item in m_collection)
			{
				if (match((TBase)item))
				{
					yield return (TBase)item;
				}
			}
		}

		public IEnumerable<TBase> FindAll(string name)
		{
			foreach (TDerived item in m_collection.FindAll(name))
			{
				yield return (TBase)item;
			}
		}

		public IEnumerator<TBase> GetEnumerator()
		{
			foreach (TDerived item in m_collection)
			{
				yield return (TBase)item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class FilteredCollection : IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		private readonly IMetadataCollection<T> m_collection;

		private readonly Predicate<T> m_match;

		public int Count
		{
			get
			{
				int num = 0;
				using IEnumerator<T> enumerator = GetEnumerator();
				while (enumerator.MoveNext())
				{
					_ = enumerator.Current;
					num++;
				}
				return num;
			}
		}

		public T this[string name]
		{
			get
			{
				using (IEnumerator<T> enumerator = FindAll(name).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}
		}

		public IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => Collection<IMetadataObject>.Convert((IMetadataCollection<T>)this);

		public FilteredCollection(IMetadataCollection<T> collection, Predicate<T> match)
		{
			m_collection = collection;
			m_match = match;
		}

		public bool Contains(string name)
		{
			return this[name] != null;
		}

		public bool Contains(T item)
		{
			if (m_collection.Contains(item))
			{
				return m_match(item);
			}
			return false;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			using IEnumerator<T> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				if (match(current))
				{
					yield return current;
				}
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			foreach (T item in m_collection.FindAll(name))
			{
				if (m_match(item))
				{
					yield return item;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (T item in m_collection)
			{
				if (m_match(item))
				{
					yield return item;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class OnFirstAccessCollection<C> : IMetadataCollection<T>, IEnumerable<T>, IEnumerable where C : class, T
	{
		private readonly Action<C> m_action;

		private readonly Predicate<C> m_actionCompleted;

		private readonly IMetadataCollection<C> m_collection;

		private readonly IMutableMetadataCollection<C> m_accessedCollection;

		public int Count => m_collection.Count;

		public T this[string name]
		{
			get
			{
				C val = m_accessedCollection[name];
				if (val == null)
				{
					val = m_collection[name];
					if (val != null)
					{
						try
						{
							m_accessedCollection.Add(val);
							m_action(val);
						}
						finally
						{
							if (!m_actionCompleted(val))
							{
								m_accessedCollection.Remove(val);
							}
						}
					}
				}
				return (T)val;
			}
		}

		public IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => Collection<IMetadataObject>.Convert((IMetadataCollection<T>)this);

		public OnFirstAccessCollection(IMutableMetadataCollection<C> collection, Action<C> action, Predicate<C> actionCompleted)
		{
			m_action = action;
			m_actionCompleted = actionCompleted;
			m_collection = collection;
			m_accessedCollection = collection.Clone();
			foreach (C item in collection)
			{
				if (actionCompleted(item))
				{
					m_accessedCollection.Add(item);
				}
			}
		}

		public bool Contains(string name)
		{
			return m_collection.Contains(name);
		}

		public bool Contains(T item)
		{
			if (!(item is C item2))
			{
				return false;
			}
			return m_collection.Contains(item2);
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			foreach (C item in m_collection)
			{
				if (match((T)item))
				{
					yield return (T)item;
				}
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			foreach (C item in m_collection.FindAll(name))
			{
				yield return (T)item;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (C item in m_collection)
			{
				yield return (T)item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private abstract class FixedOrderedCollectionOfOneItemBase : IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		protected readonly T m_item;

		protected readonly IEqualityComparer<string> m_equalityComparer;

		public T this[int index]
		{
			get
			{
				if (index == 0)
				{
					return m_item;
				}
				throw new IndexOutOfRangeException();
			}
		}

		public int Count => 1;

		public T this[string name]
		{
			get
			{
				if (!m_equalityComparer.Equals(m_item.Name, name))
				{
					return null;
				}
				return m_item;
			}
		}

		public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

		protected FixedOrderedCollectionOfOneItemBase(T item, CollationInfo collationInfo)
		{
			m_item = item;
			m_equalityComparer = collationInfo.EqualityComparer;
		}

		public bool Contains(string name)
		{
			return m_equalityComparer.Equals(m_item.Name, name);
		}

		public bool Contains(T item)
		{
			return m_item == item;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			if (match(m_item))
			{
				yield return m_item;
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			if (this[name] != null)
			{
				yield return m_item;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return m_item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class FixedOrderedCollectionOfOneItem : FixedOrderedCollectionOfOneItemBase, IMetadataOrderedCollection<IMetadataObject>, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable
	{
		public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		public new IMetadataObject this[int index] => base[index];

		IMetadataObject IMetadataCollection<IMetadataObject>.this[string name] => base[name];

		public FixedOrderedCollectionOfOneItem(T item, CollationInfo collationInfo)
			: base(item, collationInfo)
		{
		}

		public bool Contains(IMetadataObject item)
		{
			return m_item == item;
		}

		public IEnumerable<IMetadataObject> FindAll(Predicate<IMetadataObject> match)
		{
			if (match(m_item))
			{
				yield return m_item;
			}
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
		{
			if (base[name] != null)
			{
				yield return m_item;
			}
		}

		public new IEnumerator<IMetadataObject> GetEnumerator()
		{
			yield return m_item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return base.GetEnumerator();
		}
	}

	private abstract class FixedOrderedCollectionOfTwoItemsBase : IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		protected readonly T m_item1;

		protected readonly T m_item2;

		protected readonly IEqualityComparer<string> m_equalityComparer;

		public T this[int index] => index switch
		{
			0 => m_item1, 
			1 => m_item2, 
			_ => throw new IndexOutOfRangeException(), 
		};

		public int Count => 2;

		public T this[string name]
		{
			get
			{
				if (m_equalityComparer.Equals(m_item1.Name, name))
				{
					return m_item1;
				}
				if (m_equalityComparer.Equals(m_item2.Name, name))
				{
					return m_item2;
				}
				return null;
			}
		}

		public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

		protected FixedOrderedCollectionOfTwoItemsBase(T item1, T item2, CollationInfo collationInfo)
		{
			m_item1 = item1;
			m_item2 = item2;
			m_equalityComparer = collationInfo.EqualityComparer;
		}

		public bool Contains(string name)
		{
			if (!m_equalityComparer.Equals(m_item1.Name, name))
			{
				return m_equalityComparer.Equals(m_item2.Name, name);
			}
			return true;
		}

		public bool Contains(T item)
		{
			if (m_item1 != item)
			{
				return m_item2 == item;
			}
			return true;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			if (match(m_item1))
			{
				yield return m_item1;
			}
			if (match(m_item2))
			{
				yield return m_item2;
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			if (m_equalityComparer.Equals(m_item1.Name, name))
			{
				yield return m_item1;
			}
			if (m_equalityComparer.Equals(m_item2.Name, name))
			{
				yield return m_item2;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return m_item1;
			yield return m_item2;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class FixedOrderedCollectionOfTwoItems : FixedOrderedCollectionOfTwoItemsBase, IMetadataOrderedCollection<IMetadataObject>, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable
	{
		public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		public new IMetadataObject this[int index] => base[index];

		IMetadataObject IMetadataCollection<IMetadataObject>.this[string name] => base[name];

		public FixedOrderedCollectionOfTwoItems(T item1, T item2, CollationInfo collationInfo)
			: base(item1, item2, collationInfo)
		{
		}

		public bool Contains(IMetadataObject item)
		{
			if (m_item1 != item)
			{
				return m_item2 == item;
			}
			return true;
		}

		public IEnumerable<IMetadataObject> FindAll(Predicate<IMetadataObject> match)
		{
			if (match(m_item1))
			{
				yield return m_item1;
			}
			if (match(m_item2))
			{
				yield return m_item2;
			}
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
		{
			if (m_equalityComparer.Equals(m_item1.Name, name))
			{
				yield return m_item1;
			}
			if (m_equalityComparer.Equals(m_item2.Name, name))
			{
				yield return m_item2;
			}
		}

		public new IEnumerator<IMetadataObject> GetEnumerator()
		{
			yield return m_item1;
			yield return m_item2;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return base.GetEnumerator();
		}
	}

	private abstract class FixedOrderedCollectionBase : IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		protected readonly T[] m_data;

		protected readonly IEqualityComparer<string> m_equalityComparer;

		public int Count => m_data.Length;

		public T this[string name]
		{
			get
			{
				for (int i = 0; i < m_data.Length; i++)
				{
					T val = m_data[i];
					if (m_equalityComparer.Equals(name, val.Name))
					{
						return val;
					}
				}
				return null;
			}
		}

		public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

		public T this[int index] => m_data[index];

		protected FixedOrderedCollectionBase(T[] data, CollationInfo collationInfo)
		{
			m_data = data;
			m_equalityComparer = collationInfo.EqualityComparer;
		}

		public bool Contains(string name)
		{
			return this[name] != null;
		}

		public bool Contains(T item)
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				if (m_data[i] == item)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				if (match(m_data[i]))
				{
					yield return m_data[i];
				}
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				T val = m_data[i];
				if (m_equalityComparer.Equals(name, val.Name))
				{
					yield return val;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				yield return m_data[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class FixedOrderedCollection : FixedOrderedCollectionBase, IMetadataOrderedCollection<IMetadataObject>, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable
	{
		public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		IMetadataObject IMetadataOrderedCollection<IMetadataObject>.this[int index] => base[index];

		int IMetadataCollection<IMetadataObject>.Count => base.Count;

		IMetadataObject IMetadataCollection<IMetadataObject>.this[string name] => base[name];

		IMetadataCollection<IMetadataObject> IMetadataCollection<IMetadataObject>.AsMetadataObjectCollection => this;

		public FixedOrderedCollection(T[] data, CollationInfo collationInfo)
			: base(data, collationInfo)
		{
		}

		bool IMetadataCollection<IMetadataObject>.Contains(string name)
		{
			return Contains(name);
		}

		bool IMetadataCollection<IMetadataObject>.Contains(IMetadataObject item)
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				if (m_data[i] == item)
				{
					return true;
				}
			}
			return false;
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(Predicate<IMetadataObject> match)
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				if (match(m_data[i]))
				{
					yield return m_data[i];
				}
			}
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
		{
			foreach (T item in FindAll(name))
			{
				yield return item;
			}
		}

		IEnumerator<IMetadataObject> IEnumerable<IMetadataObject>.GetEnumerator()
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				yield return m_data[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private abstract class EmptyCollectionBase : IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable
	{
		public T this[int index]
		{
			get
			{
				throw new IndexOutOfRangeException();
			}
		}

		public int Count => 0;

		public T this[string name] => null;

		public abstract IMetadataCollection<IMetadataObject> AsMetadataObjectCollection { get; }

		public bool Contains(string name)
		{
			return false;
		}

		public bool Contains(T item)
		{
			return false;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			yield break;
		}

		public IEnumerable<T> FindAll(string name)
		{
			yield break;
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			yield break;
		}
	}

	private sealed class EmptyCollection : EmptyCollectionBase, IMetadataOrderedCollection<IMetadataObject>, IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable
	{
		private static class Singleton
		{
			public static readonly EmptyCollection Instance;

			static Singleton()
			{
				Instance = new EmptyCollection();
			}
		}

		public static EmptyCollection Instance => Singleton.Instance;

		public override IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		public new IMetadataObject this[int index] => base[index];

		IMetadataObject IMetadataCollection<IMetadataObject>.this[string name] => null;

		private EmptyCollection()
		{
		}

		public bool Contains(IMetadataObject item)
		{
			return false;
		}

		public IEnumerable<IMetadataObject> FindAll(Predicate<IMetadataObject> match)
		{
			yield break;
		}

		IEnumerable<IMetadataObject> IMetadataCollection<IMetadataObject>.FindAll(string name)
		{
			yield break;
		}

		public new IEnumerator<IMetadataObject> GetEnumerator()
		{
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			yield break;
		}
	}

	private static readonly IMetadataOrderedCollection<T> emptyCollection = EmptyCollection.Instance;

	public static IMetadataCollection<T> Empty => emptyCollection;

	public static IMetadataOrderedCollection<T> EmptyOrdered => emptyCollection;

	public static IMetadataCollection<T> Merge(IMetadataCollection<T> collection1, IMetadataCollection<T> collection2)
	{
		return Merge(allowDuplicates: false, collection1, collection2);
	}

	public static IMetadataCollection<T> Merge(bool allowDuplicates, IMetadataCollection<T> collection1, IMetadataCollection<T> collection2)
	{
		IMetadataCollection<T> empty = Empty;
		if (collection1 == null || collection1 == empty)
		{
			return collection2 ?? empty;
		}
		if (collection2 == null || collection2 == empty)
		{
			return collection1;
		}
		if (!allowDuplicates && collection1 == collection2)
		{
			return collection1;
		}
		return new MergerOf2(collection1, collection2);
	}

	public static IMetadataCollection<T> Merge(params IMetadataCollection<T>[] collections)
	{
		return Merge(allowDuplicates: false, collections);
	}

	public static IMetadataCollection<T> Merge(bool allowDuplicates, params IMetadataCollection<T>[] collections)
	{
		IMetadataCollection<T> empty = Empty;
		int num = 0;
		for (int i = 0; i < collections.Length; i++)
		{
			if (collections[i] == null || collections[i] == empty)
			{
				continue;
			}
			bool flag = false;
			if (!allowDuplicates)
			{
				for (int j = 0; j < num; j++)
				{
					if (collections[i] == collections[j])
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				collections[num++] = collections[i];
			}
		}
		switch (num)
		{
		case 0:
			return empty;
		case 1:
			return collections[0];
		case 2:
			return new MergerOf2(collections[0], collections[1]);
		default:
			if (num < collections.Length)
			{
				IMetadataCollection<T>[] array = new IMetadataCollection<T>[num];
				Array.Copy(collections, array, num);
				return new MergerOfN(array);
			}
			return new MergerOfN(collections);
		}
	}

	public static IMetadataCollection<T> Convert<TDerived>(IMetadataCollection<TDerived> collection) where TDerived : class, T
	{
		if (collection == null)
		{
			return null;
		}
		if (collection == Collection<TDerived>.Empty)
		{
			return Empty;
		}
		IMetadataCollection<T> metadataCollection = collection as IMetadataCollection<T>;
		if (metadataCollection == null)
		{
			metadataCollection = new WrapperCollection<T, TDerived>(collection);
		}
		return metadataCollection;
	}

	public static IMetadataCollection<T> Filter(IMetadataCollection<T> collection, Predicate<T> match)
	{
		return new FilteredCollection(collection, match);
	}

	public static IMetadataCollection<T> ApplyOnFirstAccess<C>(IMutableMetadataCollection<C> collection, Action<C> action, Predicate<C> actionCompleted) where C : class, T
	{
		return new OnFirstAccessCollection<C>(collection, action, actionCompleted);
	}

	public static IMetadataOrderedCollection<T> CreateOrderedCollection(CollationInfo collationInfo, T item)
	{
		return new FixedOrderedCollectionOfOneItem(item, collationInfo);
	}

	public static IMetadataOrderedCollection<T> CreateOrderedCollection(CollationInfo collationInfo, T item1, T item2)
	{
		return new FixedOrderedCollectionOfTwoItems(item1, item2, collationInfo);
	}

	public static IMetadataOrderedCollection<T> CreateOrderedCollection(CollationInfo collationInfo, params T[] items)
	{
		return CreateOrderedCollection(collationInfo, items, items.Length);
	}

	public static IMetadataOrderedCollection<T> CreateOrderedCollection(CollationInfo collationInfo, T[] items, int count)
	{
		switch (count)
		{
		case 0:
			return EmptyOrdered;
		case 1:
			return new FixedOrderedCollectionOfOneItem(items[0], collationInfo);
		case 2:
			return new FixedOrderedCollectionOfTwoItems(items[0], items[1], collationInfo);
		default:
		{
			T[] array = new T[count];
			Array.Copy(items, array, count);
			return new FixedOrderedCollection(array, collationInfo);
		}
		}
	}

	public static T[] CopyToArray(IMetadataCollection<T> collection)
	{
		if (collection == null)
		{
			return null;
		}
		T[] array = new T[collection.Count];
		int num = 0;
		foreach (T item in collection)
		{
			array[num++] = item;
		}
		return array;
	}

	internal static IMetadataOrderedCollection<T> ConvertArray(T[] data, CollationInfo collationInfo)
	{
		return data.Length switch
		{
			0 => EmptyOrdered, 
			1 => new FixedOrderedCollectionOfOneItem(data[0], collationInfo), 
			2 => new FixedOrderedCollectionOfTwoItems(data[0], data[1], collationInfo), 
			_ => new FixedOrderedCollection(data, collationInfo), 
		};
	}
}
