using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;

namespace Stimulsoft.Report.Gauge.Collections;

public class StiRangeCollection : CollectionBase, ICloneable
{
	private StiScaleRangeList parent;

	public bool IsReadOnly => false;

	public virtual StiRangeBase this[int index]
	{
		get
		{
			if (index < 0 || index >= base.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", "InvalidArgument");
			}
			return base.InnerList[index] as StiRangeBase;
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
		foreach (StiRangeBase item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			string ident = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
			StiRangeBase stiRangeBase = StiOptions.Services.Ranges.FirstOrDefault((StiRangeBase x) => x.GetType().Name == ident);
			if (stiRangeBase == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiRangeBase stiRangeBase2 = stiRangeBase.CreateNew();
			Add(stiRangeBase2);
			stiRangeBase2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiRangeCollection stiRangeCollection = new StiRangeCollection(parent);
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiRangeBase stiRangeBase = (StiRangeBase)enumerator.Current;
					stiRangeCollection.Add(stiRangeBase.Clone() as StiRangeBase);
				}
				return stiRangeCollection;
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

	protected virtual void SetParent(StiRangeBase element)
	{
		element.rangeList = parent;
	}

	protected virtual void ClearParent(StiRangeBase element)
	{
		element.rangeList = null;
	}

	public void Add(StiRangeBase element)
	{
		AddInternal(element);
	}

	public void AddRange(StiRangeBase[] elements)
	{
		foreach (StiRangeBase element in elements)
		{
			AddInternal(element);
		}
	}

	public void Insert(int index, StiRangeBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("item");
		}
		if (index < 0 || index > base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		SetParent(element);
		base.InnerList.Insert(index, element);
	}

	public bool Remove(StiRangeBase element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1)
		{
			ClearParent(element);
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public bool Contains(StiRangeBase element)
	{
		return IndexOf(element) != -1;
	}

	public void CopyTo(StiRangeBase[] elements, int arrayIndex)
	{
		base.InnerList.CopyTo(elements, arrayIndex);
	}

	public int IndexOf(StiRangeBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return base.InnerList.IndexOf(element);
	}

	internal void SetItemInternal(int index, StiRangeBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("value");
		}
		if (index < 0 || index >= base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		SetParent(element);
		base.InnerList[index] = element;
	}

	private void AddInternal(StiRangeBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		SetParent(element);
		base.InnerList.Add(element);
	}

	public bool MoveUp(StiRangeBase element)
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

	public bool MoveDown(StiRangeBase element)
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

	public StiRangeCollection(StiScaleRangeList parent)
	{
		this.parent = parent;
	}
}
