using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal static class ScopedMetadataCollection<T, MT> where T : class, IDatabaseObject where MT : class, T
{
	private sealed class EmptyCollection : IAlterableScopedMetadataCollection<T, MT>, IScopedMetadataCollection<T, MT>
	{
		public T this[string name, LexicalScope lexicalScope] => null;

		public IMetadataCollection<T> GlobalMetadata => Collection<T>.Empty;

		public bool Contains(string name, LexicalScope lexicalScope)
		{
			return false;
		}

		public IMetadataCollection<IMetadataObject> GetMetadataCollection(LexicalScope lexicalScope)
		{
			return Collection<IMetadataObject>.Empty;
		}

		public void Create(MT createdObject, IBindFinalizer finalizer, StatementBindMode bindMode)
		{
		}

		public void Drop(T droppedObject, LexicalScope lexicalScope)
		{
		}

		public void Alter(MT alteredObject, LexicalScope lexicalScope)
		{
		}

		public MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode)
		{
			return null;
		}

		public MT AsMutableObject(T immutableObject)
		{
			return null;
		}

		public void ReplaceWithMutableObject(T immutableObject, MT mutableObject)
		{
		}
	}

	private sealed class MergeOfTwoMetadataCollectionEx : IAlterableScopedMetadataCollection<T, MT>, IScopedMetadataCollection<T, MT>
	{
		private readonly IMetadataCollection<T> m_proxyCollection;

		private readonly IScopedMetadataCollection<T, MT> m_primaryCollection;

		private readonly IScopedMetadataCollection<T, MT> m_secondaryCollection;

		public T this[string name, LexicalScope lexicalScope] => m_primaryCollection[name, lexicalScope] ?? m_secondaryCollection[name, lexicalScope];

		public IMetadataCollection<T> GlobalMetadata => m_proxyCollection;

		public MergeOfTwoMetadataCollectionEx(IScopedMetadataCollection<T, MT> primaryCollection, IScopedMetadataCollection<T, MT> secondaryCollection)
		{
			m_proxyCollection = Collection<T>.Merge(primaryCollection.GlobalMetadata, secondaryCollection.GlobalMetadata);
			m_primaryCollection = primaryCollection;
			m_secondaryCollection = secondaryCollection;
		}

		public bool Contains(string name, LexicalScope lexicalScope)
		{
			if (!m_primaryCollection.Contains(name, lexicalScope))
			{
				return m_secondaryCollection.Contains(name, lexicalScope);
			}
			return true;
		}

		public IMetadataCollection<IMetadataObject> GetMetadataCollection(LexicalScope lexicalScope)
		{
			return Collection<IMetadataObject>.Merge(m_primaryCollection.GetMetadataCollection(lexicalScope), m_secondaryCollection.GetMetadataCollection(lexicalScope));
		}

		public void Create(MT createdObject, IBindFinalizer finalizer, StatementBindMode bindMode)
		{
		}

		public void Drop(T droppedObject, LexicalScope lexicalScope)
		{
		}

		public void Alter(MT alteredObject, LexicalScope lexicalScope)
		{
		}

		public MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode)
		{
			return null;
		}

		public MT AsMutableObject(T immutableObject)
		{
			return null;
		}

		public void ReplaceWithMutableObject(T immutableObject, MT mutableObject)
		{
		}
	}

	private sealed class MergeOfThreeCollection : IAlterableScopedMetadataCollection<T, MT>, IScopedMetadataCollection<T, MT>
	{
		private readonly IMetadataCollection<T> m_proxyCollection;

		private readonly IScopedMetadataCollection<T, MT> m_primaryCollection;

		private readonly IScopedMetadataCollection<T, MT> m_secondaryCollection1;

		private readonly IScopedMetadataCollection<T, MT> m_secondaryCollection2;

		public T this[string name, LexicalScope lexicalScope] => m_primaryCollection[name, lexicalScope] ?? m_secondaryCollection1[name, lexicalScope] ?? m_secondaryCollection2[name, lexicalScope];

		public IMetadataCollection<T> GlobalMetadata => m_proxyCollection;

		public MergeOfThreeCollection(IScopedMetadataCollection<T, MT> primaryCollection, IScopedMetadataCollection<T, MT> secondaryCollection1, IScopedMetadataCollection<T, MT> secondaryCollection2)
		{
			m_proxyCollection = Collection<T>.Merge(primaryCollection.GlobalMetadata, secondaryCollection1.GlobalMetadata, secondaryCollection2.GlobalMetadata);
			m_primaryCollection = primaryCollection;
			m_secondaryCollection1 = secondaryCollection1;
			m_secondaryCollection2 = secondaryCollection2;
		}

		public bool Contains(string name, LexicalScope lexicalScope)
		{
			if (!m_primaryCollection.Contains(name, lexicalScope) && !m_secondaryCollection1.Contains(name, lexicalScope))
			{
				return m_secondaryCollection2.Contains(name, lexicalScope);
			}
			return true;
		}

		public IMetadataCollection<IMetadataObject> GetMetadataCollection(LexicalScope lexicalScope)
		{
			return Collection<IMetadataObject>.Merge(m_primaryCollection.GetMetadataCollection(lexicalScope), m_secondaryCollection1.GetMetadataCollection(lexicalScope), m_secondaryCollection2.GetMetadataCollection(lexicalScope));
		}

		public void Create(MT createdObject, IBindFinalizer finalizer, StatementBindMode bindMode)
		{
		}

		public void Drop(T droppedObject, LexicalScope lexicalScope)
		{
		}

		public void Alter(MT alteredObject, LexicalScope lexicalScope)
		{
		}

		public MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode)
		{
			return null;
		}

		public MT AsMutableObject(T immutableObject)
		{
			return null;
		}

		public void ReplaceWithMutableObject(T immutableObject, MT mutableObject)
		{
		}
	}

	private abstract class CollectionBase<C> : IAlterableScopedMetadataCollection<T, MT>, IScopedMetadataCollection<T, MT> where C : class, IMetadataCollection<T>
	{
		protected readonly C m_permCollection;

		private readonly CollationInfo m_collationInfo;

		private SortedList<string, ExecutionSimulator<T>> m_transientCollection;

		private readonly IDuplicator<T, MT> m_duplicator;

		protected IDuplicator<T, MT> Duplicator => m_duplicator;

		public virtual T this[string name, LexicalScope lexicalScope]
		{
			get
			{
				if (m_transientCollection != null && lexicalScope != null && m_transientCollection.TryGetValue(name, out var value))
				{
					return value[lexicalScope];
				}
				return m_permCollection[name];
			}
		}

		public IMetadataCollection<T> GlobalMetadata => m_permCollection;

		public SortedList<string, ExecutionSimulator<T>> TransientCollection => m_transientCollection;

		protected CollectionBase(C permCollection, CollationInfo collationInfo, IDuplicator<T, MT> duplicator)
		{
			m_permCollection = permCollection;
			m_collationInfo = collationInfo;
			m_duplicator = duplicator;
		}

		public bool Contains(string name, LexicalScope lexicalScope)
		{
			return this[name, lexicalScope] != null;
		}

		public IMetadataCollection<IMetadataObject> GetMetadataCollection(LexicalScope lexicalScope)
		{
			if (m_transientCollection == null || lexicalScope == null)
			{
				return m_permCollection.AsMetadataObjectCollection;
			}
			return new SnapshotCollection<C>(this, lexicalScope);
		}

		public void Create(MT createObject, IBindFinalizer finalizer, StatementBindMode bindMode)
		{
			switch (bindMode)
			{
			case StatementBindMode.Sequential:
				Create(createObject, finalizer.LexicalScope);
				break;
			case StatementBindMode.OutOfOrder:
				Create(createObject, finalizer);
				break;
			}
		}

		private void Create(MT createObject, LexicalScope lexicalScope)
		{
			GetOrCreateExecutionSimulator((T)createObject, lexicalScope).Create((T)createObject, lexicalScope);
		}

		protected abstract void Create(MT createdObject, IBindFinalizer finalizer);

		public void Alter(MT alterObject, LexicalScope lexicalScope)
		{
			GetOrCreateExecutionSimulator((T)alterObject, lexicalScope).Alter((T)alterObject, lexicalScope);
		}

		public void Drop(T dropObject, LexicalScope lexicalScope)
		{
			GetOrCreateExecutionSimulator(dropObject, lexicalScope).Drop(dropObject, lexicalScope);
		}

		public abstract MT AsMutableObject(T immutableObject);

		public abstract void ReplaceWithMutableObject(T immutableObject, MT mutableObject);

		public abstract MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode);

		private ExecutionSimulator<T> GetOrCreateExecutionSimulator(T dbObject, LexicalScope lexicalScope)
		{
			string name = dbObject.Name;
			ExecutionSimulator<T> value;
			if (m_transientCollection == null)
			{
				m_transientCollection = new SortedList<string, ExecutionSimulator<T>>(8, m_collationInfo.Comparer);
				value = null;
			}
			else
			{
				m_transientCollection.TryGetValue(name, out value);
			}
			if (value == null)
			{
				value = ExecutionSimulator<T>.New(m_permCollection[dbObject.Name], lexicalScope);
				m_transientCollection[name] = value;
			}
			return value;
		}
	}

	private sealed class ImmutableMetadataCollection : CollectionBase<IMetadataCollection<T>>
	{
		public ImmutableMetadataCollection(IMetadataCollection<T> proxyCollection, CollationInfo collationInfo, IDuplicator<T, MT> duplicator)
			: base(proxyCollection, collationInfo, duplicator)
		{
		}

		public override MT AsMutableObject(T immutableObject)
		{
			return null;
		}

		public override void ReplaceWithMutableObject(T immutableObject, MT mutableObject)
		{
		}

		public override MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode)
		{
			MT val = base.Duplicator.Duplicate(immutableObject);
			Alter(val, lexicalScope);
			return val;
		}

		protected override void Create(MT createdObject, IBindFinalizer finalizer)
		{
		}
	}

	private sealed class MutableMetadataCollection : CollectionBase<IMutableMetadataCollection<T>>
	{
		private readonly Dictionary<T, IBindFinalizer> m_bindFinalizers;

		private readonly Dictionary<T, MT> m_mutableOjects;

		public override T this[string name, LexicalScope lexicalScope]
		{
			get
			{
				T val = base[name, lexicalScope];
				if (val != null && m_bindFinalizers.TryGetValue(val, out var value))
				{
					if (value.IsFinalizing)
					{
						if (lexicalScope == value.LexicalScope)
						{
							val = null;
						}
					}
					else
					{
						if (!value.IsFinalized)
						{
							value.FinalizeBind();
						}
						m_bindFinalizers.Remove(val);
					}
				}
				return val;
			}
		}

		public MutableMetadataCollection(IMutableMetadataCollection<T> proxyCollection, CollationInfo collationInfo, IDuplicator<T, MT> duplicator)
			: base(proxyCollection, collationInfo, duplicator)
		{
			m_bindFinalizers = new Dictionary<T, IBindFinalizer>();
			m_mutableOjects = new Dictionary<T, MT>();
		}

		public override MT AsMutableObject(T immutableObject)
		{
			if (!m_mutableOjects.TryGetValue(immutableObject, out var value))
			{
				return null;
			}
			return value;
		}

		public override void ReplaceWithMutableObject(T immutableObject, MT mutableObject)
		{
			m_permCollection[mutableObject.Name] = (T)mutableObject;
			m_mutableOjects[(T)mutableObject] = mutableObject;
		}

		public override MT GetAlterableObject(T immutableObject, LexicalScope lexicalScope, StatementBindMode bindMode)
		{
			MT val;
			if (bindMode != StatementBindMode.OutOfOrder || (val = AsMutableObject(immutableObject)) == null)
			{
				val = base.Duplicator.Duplicate(immutableObject);
				if (bindMode == StatementBindMode.OutOfOrder && m_permCollection.Contains(immutableObject))
				{
					ReplaceWithMutableObject(immutableObject, val);
				}
				else
				{
					Alter(val, lexicalScope);
				}
			}
			return val;
		}

		protected override void Create(MT createdObject, IBindFinalizer finalizer)
		{
			m_permCollection.Add((T)createdObject);
			m_bindFinalizers[(T)createdObject] = finalizer;
			m_mutableOjects[(T)createdObject] = createdObject;
		}
	}

	private class SnapshotCollection<C> : IMetadataCollection<IMetadataObject>, IEnumerable<IMetadataObject>, IEnumerable where C : class, IMetadataCollection<T>
	{
		private readonly CollectionBase<C> m_scopedCollection;

		private readonly LexicalScope m_lexicalScope;

		public int Count => m_scopedCollection.GlobalMetadata.Count + m_scopedCollection.TransientCollection.Count;

		public IMetadataObject this[string name] => m_scopedCollection[name, m_lexicalScope];

		public IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => this;

		public SnapshotCollection(CollectionBase<C> scopedCollection, LexicalScope lexicalScope)
		{
			m_scopedCollection = scopedCollection;
			m_lexicalScope = lexicalScope;
		}

		public bool Contains(string name)
		{
			return m_scopedCollection.Contains(name, m_lexicalScope);
		}

		public bool Contains(IMetadataObject item)
		{
			return this[item.Name] == item;
		}

		public IEnumerable<IMetadataObject> FindAll(Predicate<IMetadataObject> match)
		{
			using IEnumerator<IMetadataObject> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				IMetadataObject current = enumerator.Current;
				if (match(current))
				{
					yield return current;
				}
			}
		}

		public IEnumerable<IMetadataObject> FindAll(string name)
		{
			IMetadataObject metadataObject = this[name];
			if (metadataObject != null)
			{
				yield return metadataObject;
			}
		}

		public IEnumerator<IMetadataObject> GetEnumerator()
		{
			foreach (T globalMetadatum in m_scopedCollection.GlobalMetadata)
			{
				if (!m_scopedCollection.TransientCollection.ContainsKey(globalMetadatum.Name))
				{
					yield return globalMetadatum;
				}
			}
			foreach (ExecutionSimulator<T> value in m_scopedCollection.TransientCollection.Values)
			{
				IMetadataObject metadataObject = value[m_lexicalScope];
				if (metadataObject != null)
				{
					yield return metadataObject;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private static readonly EmptyCollection EmptyInstance;

	public static IAlterableScopedMetadataCollection<T, MT> Empty => EmptyInstance;

	static ScopedMetadataCollection()
	{
		EmptyInstance = new EmptyCollection();
	}

	public static IScopedMetadataCollection<T, MT> Merge(IScopedMetadataCollection<T, MT> primaryCollection, IScopedMetadataCollection<T, MT> secondaryCollection)
	{
		if (primaryCollection == secondaryCollection)
		{
			return primaryCollection;
		}
		return new MergeOfTwoMetadataCollectionEx(primaryCollection, secondaryCollection);
	}

	public static IAlterableScopedMetadataCollection<T, MT> Merge(IAlterableScopedMetadataCollection<T, MT> primaryCollection, IAlterableScopedMetadataCollection<T, MT> secondaryCollection)
	{
		if (primaryCollection == secondaryCollection)
		{
			return primaryCollection;
		}
		return new MergeOfTwoMetadataCollectionEx(primaryCollection, secondaryCollection);
	}

	public static IScopedMetadataCollection<T, MT> Merge(IScopedMetadataCollection<T, MT> primaryCollection, IScopedMetadataCollection<T, MT> secondaryCollection1, IScopedMetadataCollection<T, MT> secondaryCollection2)
	{
		if (primaryCollection == secondaryCollection1)
		{
			return Merge(primaryCollection, secondaryCollection2);
		}
		if (primaryCollection == secondaryCollection2)
		{
			return Merge(primaryCollection, secondaryCollection1);
		}
		if (secondaryCollection1 == secondaryCollection2)
		{
			return Merge(primaryCollection, secondaryCollection1);
		}
		return new MergeOfThreeCollection(primaryCollection, secondaryCollection1, secondaryCollection2);
	}

	public static IAlterableScopedMetadataCollection<T, MT> Merge(IAlterableScopedMetadataCollection<T, MT> primaryCollection, IAlterableScopedMetadataCollection<T, MT> secondaryCollection1, IAlterableScopedMetadataCollection<T, MT> secondaryCollection2)
	{
		if (primaryCollection == secondaryCollection1)
		{
			return Merge(primaryCollection, secondaryCollection2);
		}
		if (primaryCollection == secondaryCollection2)
		{
			return Merge(primaryCollection, secondaryCollection1);
		}
		if (secondaryCollection1 == secondaryCollection2)
		{
			return Merge(primaryCollection, secondaryCollection1);
		}
		return new MergeOfThreeCollection(primaryCollection, secondaryCollection1, secondaryCollection2);
	}

	public static IScopedMetadataCollection<T, MT> CreateImmutableCollection(IMetadataCollection<T> proxyCollection, CollationInfo collationInfo)
	{
		return new ImmutableMetadataCollection(proxyCollection, collationInfo, null);
	}

	public static IAlterableScopedMetadataCollection<T, MT> CreateImmutableCollection(IMetadataCollection<T> proxyCollection, CollationInfo collationInfo, IDuplicator<T, MT> duplicator)
	{
		return new ImmutableMetadataCollection(proxyCollection, collationInfo, duplicator);
	}

	public static IScopedMetadataCollection<T, MT> CreateMutableCollection(IMutableMetadataCollection<T> proxyCollection, CollationInfo collationInfo)
	{
		return new MutableMetadataCollection(proxyCollection, collationInfo, null);
	}

	public static IAlterableScopedMetadataCollection<T, MT> CreateMutableCollection(IMutableMetadataCollection<T> proxyCollection, CollationInfo collationInfo, IDuplicator<T, MT> duplicator)
	{
		return new MutableMetadataCollection(proxyCollection, collationInfo, duplicator);
	}
}
