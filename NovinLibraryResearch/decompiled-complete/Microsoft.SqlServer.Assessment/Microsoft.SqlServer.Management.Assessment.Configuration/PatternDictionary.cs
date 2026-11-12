using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public class PatternDictionary<TKey, TValue> : IList<TValue>, ICollection<TValue>, IEnumerable<TValue>, IEnumerable
{
	private readonly List<Tuple<IPattern<TKey>, TValue>> rep = new List<Tuple<IPattern<TKey>, TValue>>();

	public int Count => rep.Count;

	public bool IsReadOnly => ((ICollection<Tuple<IPattern<TKey>, TValue>>)rep).IsReadOnly;

	private Func<TValue, IPattern<TKey>> GetPattern { get; }

	public TValue this[TKey key]
	{
		get
		{
			Tuple<IPattern<TKey>, TValue> tuple = rep.FirstOrDefault((Tuple<IPattern<TKey>, TValue> p) => p.Item1?.IsMatch(key) ?? true);
			if (tuple == null)
			{
				throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorKeyNotFound, key));
			}
			return tuple.Item2;
		}
	}

	public TValue this[int index]
	{
		get
		{
			return rep[index].Item2;
		}
		set
		{
			rep[index] = CreateRecord(value);
		}
	}

	public PatternDictionary(Func<TValue, IPattern<TKey>> getPattern)
	{
		GetPattern = getPattern ?? throw new ArgumentNullException("getPattern");
	}

	public IEnumerator<TValue> GetEnumerator()
	{
		return rep.Select((Tuple<IPattern<TKey>, TValue> i) => i.Item2).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(TValue item)
	{
		rep.Add(CreateRecord(item));
	}

	public void AddRange(IEnumerable<TValue> collection)
	{
		rep.AddRange(collection.Select(CreateRecord));
	}

	public void InsertRange(int index, IEnumerable<TValue> collection)
	{
		rep.InsertRange(index, collection.Select(CreateRecord));
	}

	public void Clear()
	{
		rep.Clear();
	}

	public bool Contains(TValue item)
	{
		return rep.Any((Tuple<IPattern<TKey>, TValue> rec) => rec.Item2.Equals(item));
	}

	public void CopyTo(TValue[] array, int arrayIndex)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (arrayIndex < 0)
		{
			throw new ArgumentOutOfRangeException(Resources.ErrorIndexOutOfRange);
		}
		if (Count + arrayIndex > array.LongLength)
		{
			throw new ArgumentException(Resources.ErrorIndexOutOfRange);
		}
		foreach (Tuple<IPattern<TKey>, TValue> item in rep)
		{
			array[arrayIndex] = item.Item2;
		}
	}

	public bool Remove(TValue item)
	{
		return rep.RemoveAll((Tuple<IPattern<TKey>, TValue> rec) => rec.Item2.Equals(item)) > 0;
	}

	public int IndexOf(TValue item)
	{
		return rep.FindIndex((Tuple<IPattern<TKey>, TValue> rec) => rec.Item2.Equals(item));
	}

	public void Insert(int index, TValue item)
	{
		rep.Insert(index, CreateRecord(item));
	}

	public void RemoveAt(int index)
	{
		rep.RemoveAt(index);
	}

	protected internal virtual void TagItem(TValue value)
	{
	}

	private Tuple<IPattern<TKey>, TValue> CreateRecord(TValue value)
	{
		TagItem(value);
		return Tuple.Create(GetPattern(value), value);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		Tuple<IPattern<TKey>, TValue> tuple = rep.FirstOrDefault((Tuple<IPattern<TKey>, TValue> p) => p.Item1?.IsMatch(key) ?? true);
		if (tuple == null)
		{
			value = default(TValue);
			return false;
		}
		value = tuple.Item2;
		return true;
	}
}
