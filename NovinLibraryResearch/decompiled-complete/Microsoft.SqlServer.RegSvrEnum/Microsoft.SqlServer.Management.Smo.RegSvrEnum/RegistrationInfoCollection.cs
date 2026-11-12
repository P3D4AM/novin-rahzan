using System;
using System.Collections;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public class RegistrationInfoCollection : IEnumerable, ICollection
{
	private class RegistrationInfoCollectionEnumerator : IEnumerator
	{
		private RegistrationInfoCollection current;

		private int index;

		public object Current => current.entries[index];

		public RegistrationInfoCollectionEnumerator(RegistrationInfoCollection aic)
		{
			current = aic;
			index = -1;
		}

		public bool MoveNext()
		{
			index++;
			if (index >= current.entries.Count)
			{
				return false;
			}
			return true;
		}

		public void Reset()
		{
			index = -1;
		}
	}

	protected object syncObject;

	protected ArrayList entries;

	public RegistrationInfo this[int index] => (RegistrationInfo)entries[index];

	public RegistrationInfo this[string index]
	{
		get
		{
			if (index == null)
			{
				return null;
			}
			int count = Count;
			for (int i = 0; i < count; i++)
			{
				RegistrationInfo registrationInfo = this[i];
				if (string.Compare(registrationInfo.FriendlyName, index, ignoreCase: false, CultureInfo.CurrentCulture) == 0)
				{
					return registrationInfo;
				}
			}
			return null;
		}
	}

	public virtual int Count => entries.Count;

	public virtual object SyncRoot => syncObject;

	public virtual bool IsSynchronized => false;

	public RegistrationInfoCollection()
	{
		entries = new ArrayList();
		syncObject = new object();
	}

	public RegistrationInfoCollection(RegistrationInfo[] entries)
		: this()
	{
		this.entries.AddRange(entries);
	}

	internal void Clear()
	{
		entries.Clear();
	}

	internal int Add(RegistrationInfo reg)
	{
		return entries.Add(reg);
	}

	internal void Remove(RegistrationInfo reg)
	{
		entries.Remove(reg);
	}

	public void Sort()
	{
		entries.Sort();
	}

	public int IndexOf(string index)
	{
		if (index == null)
		{
			return -1;
		}
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			RegistrationInfo registrationInfo = this[i];
			if (string.Compare(registrationInfo.FriendlyName, index, StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				return i;
			}
		}
		return -1;
	}

	public bool Contains(string childName)
	{
		return this[childName] != null;
	}

	public void CopyTo(RegistrationInfo[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}

	public virtual void CopyTo(Array array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (array.Rank != 1)
		{
			throw new ArgumentException(SRError.ArrayDimensionInvalid, "array");
		}
		if (entries.Count >= array.Length - index)
		{
			throw new ArgumentException(SRError.ArraySizeError, "array");
		}
		entries.CopyTo(array, index);
	}

	public IEnumerator GetEnumerator()
	{
		return new RegistrationInfoCollectionEnumerator(this);
	}
}
