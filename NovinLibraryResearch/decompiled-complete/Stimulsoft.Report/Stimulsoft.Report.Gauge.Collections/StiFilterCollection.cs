using System;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge;

namespace Stimulsoft.Report.Gauge.Collections;

public class StiFilterCollection : CollectionBase, ICloneable
{
	public bool IsReadOnly => false;

	public virtual StiStateIndicatorFilter this[int index]
	{
		get
		{
			if (index < 0 || index >= base.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", "InvalidArgument");
			}
			return base.InnerList[index] as StiStateIndicatorFilter;
		}
		set
		{
			SetItemInternal(index, value);
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiStateIndicatorFilter item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiStateIndicatorFilter stiStateIndicatorFilter = new StiStateIndicatorFilter();
			Add(stiStateIndicatorFilter);
			stiStateIndicatorFilter.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiFilterCollection stiFilterCollection = new StiFilterCollection();
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiStateIndicatorFilter stiStateIndicatorFilter = (StiStateIndicatorFilter)enumerator.Current;
					stiFilterCollection.Add(stiStateIndicatorFilter.Clone() as StiStateIndicatorFilter);
				}
				return stiFilterCollection;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	public void Add(StiStateIndicatorFilter element)
	{
		base.InnerList.Add(element);
	}

	public void AddRange(StiStateIndicatorFilter[] elements)
	{
		foreach (StiStateIndicatorFilter value in elements)
		{
			base.InnerList.Add(value);
		}
	}

	public void Insert(int index, StiStateIndicatorFilter element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("item");
		}
		if (index < 0 || index > base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		base.InnerList.Insert(index, element);
	}

	public bool Remove(StiStateIndicatorFilter element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1)
		{
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public bool Contains(StiStateIndicatorFilter element)
	{
		return IndexOf(element) != -1;
	}

	public void CopyTo(StiStateIndicatorFilter[] elements, int arrayIndex)
	{
		base.InnerList.CopyTo(elements, arrayIndex);
	}

	public int IndexOf(StiStateIndicatorFilter element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return base.InnerList.IndexOf(element);
	}

	internal void SetItemInternal(int index, StiStateIndicatorFilter element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("value");
		}
		if (index < 0 || index >= base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		base.InnerList[index] = element;
	}

	public bool MoveUp(StiStateIndicatorFilter element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num > 0)
		{
			base.InnerList.RemoveAt(num);
			num--;
			base.InnerList.Insert(num, element);
			return true;
		}
		return false;
	}

	public bool MoveDown(StiStateIndicatorFilter element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1 && base.InnerList.Count > 1 && num < base.InnerList.Count - 1)
		{
			base.InnerList.RemoveAt(num);
			num++;
			base.InnerList.Insert(num, element);
			return true;
		}
		return false;
	}
}
