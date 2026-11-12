using System;
using System.Collections;
using System.Diagnostics;

namespace Microsoft.SqlServer.Management.Sdk.Sfc.Diagnostics;

internal class SQLToolsTraceListenerCollection : IList, ICollection, IEnumerable
{
	private ArrayList list;

	public TraceListener this[int i]
	{
		get
		{
			return (TraceListener)list[i];
		}
		set
		{
			list[i] = value;
		}
	}

	public TraceListener this[string name]
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TraceListener traceListener = (TraceListener)enumerator.Current;
					if (traceListener.Name == name)
					{
						return traceListener;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return null;
		}
	}

	public int Count => list.Count;

	object IList.this[int index]
	{
		get
		{
			return list[index];
		}
		set
		{
			list[index] = value;
		}
	}

	bool IList.IsReadOnly => false;

	bool IList.IsFixedSize => false;

	object ICollection.SyncRoot => this;

	bool ICollection.IsSynchronized => true;

	public SQLToolsTraceListenerCollection()
	{
		list = new ArrayList(1);
	}

	public int Add(TraceListener listener)
	{
		return ((IList)this).Add((object?)listener);
	}

	public void AddRange(TraceListener[] value)
	{
		for (int i = 0; i < value.Length; i++)
		{
			Add(value[i]);
		}
	}

	public void AddRange(SQLToolsTraceListenerCollection value)
	{
		for (int i = 0; i < value.Count; i++)
		{
			Add(value[i]);
		}
	}

	public void Clear()
	{
		list = new ArrayList();
	}

	public bool Contains(TraceListener listener)
	{
		return ((IList)this).Contains((object?)listener);
	}

	public void CopyTo(TraceListener[] listeners, int index)
	{
		((ICollection)this).CopyTo((Array)listeners, index);
	}

	public IEnumerator GetEnumerator()
	{
		return list.GetEnumerator();
	}

	public int IndexOf(TraceListener listener)
	{
		return ((IList)this).IndexOf((object?)listener);
	}

	public void Insert(int index, TraceListener listener)
	{
		((IList)this).Insert(index, (object?)listener);
	}

	public void Remove(TraceListener listener)
	{
		((IList)this).Remove((object?)listener);
	}

	public void Remove(string name)
	{
		TraceListener traceListener = this[name];
		if (traceListener != null)
		{
			((IList)this).Remove((object?)traceListener);
		}
	}

	public void RemoveAt(int index)
	{
		ArrayList arrayList = new ArrayList(list.Count);
		lock (this)
		{
			arrayList.AddRange(list);
			arrayList.RemoveAt(index);
			list = arrayList;
		}
	}

	int IList.Add(object value)
	{
		ArrayList arrayList = new ArrayList(list.Count + 1);
		lock (this)
		{
			arrayList.AddRange(list);
			int result = arrayList.Add(value);
			list = arrayList;
			return result;
		}
	}

	bool IList.Contains(object value)
	{
		return list.Contains(value);
	}

	int IList.IndexOf(object value)
	{
		return list.IndexOf(value);
	}

	void IList.Insert(int index, object value)
	{
		ArrayList arrayList = new ArrayList(list.Count + 1);
		lock (this)
		{
			arrayList.AddRange(list);
			arrayList.Insert(index, value);
			list = arrayList;
		}
	}

	void IList.Remove(object value)
	{
		ArrayList arrayList = new ArrayList(list.Count);
		lock (this)
		{
			arrayList.AddRange(list);
			arrayList.Remove(value);
			list = arrayList;
		}
	}

	void ICollection.CopyTo(Array array, int index)
	{
		ArrayList arrayList = new ArrayList(list.Count + array.Length);
		lock (this)
		{
			arrayList.AddRange(list);
			arrayList.CopyTo(array, index);
			list = arrayList;
		}
	}
}
