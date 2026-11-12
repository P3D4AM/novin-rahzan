using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Sdk.Sfc;

public abstract class SfcCollection<T, K, ParentT> : ICollection<T>, IEnumerable<T>, IEnumerable, ICollection, ISfcCollection, IListSource where T : SfcInstance where K : SfcKey where ParentT : SfcInstance
{
	private ParentT m_parent;

	private bool m_initialized;

	private bool m_initializing;

	public abstract int Count { get; }

	public abstract bool IsReadOnly { get; }

	public T this[K key] => GetObjectByKey(key);

	protected internal ParentT Parent
	{
		get
		{
			return m_parent;
		}
		set
		{
			m_parent = value;
		}
	}

	protected internal bool Initialized
	{
		get
		{
			return m_initialized;
		}
		set
		{
			m_initialized = value;
		}
	}

	SfcInstance ISfcCollection.Parent => Parent;

	bool ISfcCollection.Initialized
	{
		get
		{
			return Initialized;
		}
		set
		{
			if (value)
			{
				Initialized = value;
			}
		}
	}

	int ISfcCollection.Count => Count;

	public bool IsSynchronized
	{
		get
		{
			return false;
		}
		set
		{
			TraceHelper.Assert(condition: false, "Setting IsSynchronized property is not supported");
		}
	}

	public object SyncRoot
	{
		get
		{
			return this;
		}
		set
		{
			TraceHelper.Assert(condition: false, "Setting SyncRoot property is not supported");
		}
	}

	bool IListSource.ContainsListCollection => false;

	protected SfcCollection(ParentT parent)
	{
		Parent = parent;
		InitInnerCollection();
	}

	public virtual void Add(T obj)
	{
		PreprocessObjectForAdd(obj);
		AddImpl(obj);
		TraceHelper.Assert(obj.Parent == Parent);
	}

	public abstract void Clear();

	public abstract bool Contains(T obj);

	public abstract void CopyTo(T[] array, int arrayIndex);

	public abstract bool Remove(T obj);

	public abstract IEnumerator<T> GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public abstract bool Contains(K key);

	public void Refresh()
	{
		Refresh(refreshChildObjects: false);
	}

	public void Refresh(bool refreshChildObjects)
	{
		InitializeChildCollection(refresh: true);
		if (!refreshChildObjects)
		{
			return;
		}
		IEnumerator enumerator = GetEnumerator();
		if (enumerator != null)
		{
			while (enumerator.MoveNext())
			{
				((T)enumerator.Current).Refresh();
			}
		}
	}

	protected virtual string GetCollectionElementNameImpl()
	{
		return typeof(T).Name;
	}

	protected abstract SfcObjectFactory GetElementFactoryImpl();

	protected abstract void AddImpl(T obj);

	protected abstract bool RemoveImpl(T obj);

	protected abstract T GetObjectByKey(K key);

	protected abstract T GetExistingObjectByKey(K key);

	protected abstract void InitInnerCollection();

	protected abstract bool AddShadow(T obj);

	protected abstract void PrepareMerge();

	protected abstract void FinishMerge();

	protected void EnsureCollectionInitialized()
	{
		if ((!Parent.GetDomain().UseSfcStateManagement() || Parent.State == SfcObjectState.Existing || Parent.State == SfcObjectState.Recreate) && !m_initialized && !m_initializing && Parent.GetDomain().ConnectionContext.Mode != 0)
		{
			InitializeChildCollection();
		}
	}

	internal void AddExisting(T obj)
	{
		AddImpl(obj);
	}

	private void InitializeChildCollection()
	{
		InitializeChildCollection(refresh: false);
	}

	private void InitializeChildCollection(bool refresh)
	{
		m_initializing = true;
		if (Parent != null && Parent.KeyChain != null && Parent.KeyChain.IsConnected)
		{
			PrepareMerge();
			Parent.InitChildLevel(this);
			FinishMerge();
		}
		m_initializing = false;
		if (!m_initialized)
		{
			m_initialized = true;
		}
	}

	protected T CreateAndInitializeChildObject(K key)
	{
		if (Parent.GetDomain().ConnectionContext.Mode == SfcConnectionContextMode.Offline)
		{
			return null;
		}
		try
		{
			T val = GetElementFactory().Create(Parent, key, SfcObjectState.Existing) as T;
			val.Initialize();
			AddExisting(val);
			return val;
		}
		catch (SfcObjectInitializationException)
		{
			return null;
		}
	}

	protected internal bool RemoveInternal(T obj)
	{
		bool result = false;
		if (Contains(obj))
		{
			result = true;
			switch (obj.State)
			{
			case SfcObjectState.Existing:
			case SfcObjectState.Recreate:
				obj.State = SfcObjectState.ToBeDropped;
				break;
			default:
				obj.State = SfcObjectState.Dropped;
				result = RemoveImpl(obj);
				break;
			case SfcObjectState.ToBeDropped:
				break;
			}
		}
		return result;
	}

	internal void PreprocessObjectForAdd(T obj)
	{
		T existingObjectByKey = GetExistingObjectByKey(obj.AbstractIdentityKey as K);
		if (existingObjectByKey != null && existingObjectByKey.State == SfcObjectState.ToBeDropped)
		{
			RemoveImpl(existingObjectByKey);
			existingObjectByKey.State = SfcObjectState.Dropped;
			obj.State = SfcObjectState.Recreate;
		}
	}

	protected internal void Rename(T obj, K newKey)
	{
		EnsureCollectionInitialized();
		if (!Contains(obj))
		{
			throw new SfcInvalidRenameException(SfcStrings.KeyNotFound((obj.AbstractIdentityKey as K).ToString()));
		}
		if (Contains(newKey))
		{
			throw new SfcInvalidRenameException(SfcStrings.KeyExists((obj.AbstractIdentityKey as K).ToString()));
		}
		Urn urn = new SfcKeyChain(newKey, Parent.KeyChain.Parent).Urn;
		XPathExpressionBlock xPathExpressionBlock = urn.XPathExpression[urn.XPathExpression.Length - 1];
		string[] obj2 = new string[2] { "Schema", "Name" };
		int num = 0;
		string[] array = obj2;
		foreach (string propertyName in array)
		{
			SfcProperty sfcProperty = null;
			try
			{
				sfcProperty = obj.Properties[propertyName];
				TraceHelper.Assert(sfcProperty != null);
			}
			catch
			{
				continue;
			}
			num++;
			sfcProperty.Value = xPathExpressionBlock.GetAttributeFromFilter(sfcProperty.Name);
		}
		if (num == 0)
		{
			throw new SfcInvalidRenameException(SfcStrings.CannotRenameNoProperties(Parent));
		}
		RemoveImpl(obj);
		obj.KeyChain.LeafKey = newKey;
		obj.AbstractIdentityKey = obj.CreateIdentityKey();
		AddImpl(obj);
	}

	internal SfcObjectFactory GetElementFactory()
	{
		return GetElementFactoryImpl();
	}

	string ISfcCollection.GetCollectionElementNameImpl()
	{
		return GetCollectionElementNameImpl();
	}

	SfcObjectFactory ISfcCollection.GetElementFactory()
	{
		return GetElementFactory();
	}

	void ISfcCollection.EnsureInitialized()
	{
		EnsureCollectionInitialized();
	}

	void ISfcCollection.Add(SfcInstance sfcInstance)
	{
		Add(sfcInstance as T);
	}

	void ISfcCollection.Remove(SfcInstance sfcInstance)
	{
		Remove(sfcInstance as T);
	}

	void ISfcCollection.RemoveElement(SfcInstance sfcInstance)
	{
		RemoveImpl(sfcInstance as T);
	}

	void ISfcCollection.Rename(SfcInstance sfcInstance, SfcKey newKey)
	{
		Rename(sfcInstance as T, newKey as K);
	}

	bool ISfcCollection.GetExisting(SfcKey key, out SfcInstance sfcInstance)
	{
		sfcInstance = GetExistingObjectByKey(key as K);
		return sfcInstance != null;
	}

	SfcInstance ISfcCollection.GetObjectByKey(SfcKey key)
	{
		return GetObjectByKey(key as K);
	}

	void ISfcCollection.PrepareMerge()
	{
		PrepareMerge();
	}

	bool ISfcCollection.AddShadow(SfcInstance sfcInstance)
	{
		return AddShadow(sfcInstance as T);
	}

	void ISfcCollection.FinishMerge()
	{
		FinishMerge();
	}

	public void CopyTo(Array array, int index)
	{
		CopyTo(array, index);
	}

	IList IListSource.GetList()
	{
		T[] array = new T[Count];
		CopyTo(array, 0);
		return array;
	}
}
