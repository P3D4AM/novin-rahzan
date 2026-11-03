using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;

namespace Stimulsoft.Report.Gauge.Collections;

public class StiScaleCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	private StiGauge parent;

	public bool IsReadOnly => false;

	public StiScaleBase this[int index]
	{
		get
		{
			if (index < 0 || index >= base.List.Count)
			{
				throw new ArgumentOutOfRangeException("index", "InvalidArgument");
			}
			return base.InnerList[index] as StiScaleBase;
		}
		set
		{
			SetItemInternal(index, value);
		}
	}

	public event EventHandler ElementsChanged;

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
		foreach (StiScaleBase item in base.List)
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
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			string ident = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
			StiScaleBase stiScaleBase = StiOptions.Services.GaugeScales.FirstOrDefault((StiScaleBase x) => x.GetType().Name == ident);
			if (stiScaleBase == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiScaleBase stiScaleBase2 = stiScaleBase.CreateNew();
			stiScaleBase2.Gauge = stiScaleBase.Gauge;
			Add(stiScaleBase2);
			stiScaleBase2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiScaleCollection stiScaleCollection = new StiScaleCollection(parent);
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiScaleBase stiScaleBase = (StiScaleBase)enumerator.Current;
					stiScaleCollection.Add(stiScaleBase.Clone() as StiScaleBase);
				}
				return stiScaleCollection;
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

	private void SetParent(StiScaleBase element)
	{
		element.Gauge = parent;
	}

	private void ClearParent(StiScaleBase element)
	{
		element.Gauge = null;
	}

	public void Add(StiScaleBase element)
	{
		AddInternal(element);
		InvokeElementsChanged();
	}

	public void AddRange(StiScaleBase[] elements)
	{
		try
		{
			AddRangeInternal(elements);
		}
		finally
		{
			InvokeElementsChanged();
		}
	}

	private void AddRangeInternal(IList elements)
	{
		if (elements == null)
		{
			throw new ArgumentNullException("items");
		}
		foreach (StiScaleBase element in elements)
		{
			AddInternal(element);
		}
	}

	public void Insert(int index, StiScaleBase element)
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
		InvokeElementsChanged();
	}

	public bool Remove(StiScaleBase element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1)
		{
			ClearParent(element);
			RemoveAt(num);
			InvokeElementsChanged();
			return true;
		}
		return false;
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		InvokeElementsChanged();
	}

	protected override void OnClearComplete()
	{
		base.OnClearComplete();
		InvokeElementsChanged();
	}

	public bool Contains(StiScaleBase element)
	{
		return IndexOf(element) != -1;
	}

	public void CopyTo(StiScaleBase[] elements, int arrayIndex)
	{
		base.InnerList.CopyTo(elements, arrayIndex);
	}

	public int IndexOf(StiScaleBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return base.InnerList.IndexOf(element);
	}

	internal void SetItemInternal(int index, StiScaleBase element)
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

	private void AddInternal(StiScaleBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		SetParent(element);
		base.InnerList.Add(element);
	}

	public bool MoveUp(StiScaleBase element)
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

	public bool MoveDown(StiScaleBase element)
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

	private void InvokeElementsChanged()
	{
		this.ElementsChanged?.Invoke(this, new EventArgs());
	}

	public StiScaleCollection(StiGauge parent)
	{
		this.parent = parent;
	}
}
