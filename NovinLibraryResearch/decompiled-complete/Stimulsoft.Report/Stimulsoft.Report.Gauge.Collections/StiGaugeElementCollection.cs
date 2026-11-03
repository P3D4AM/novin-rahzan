using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;

namespace Stimulsoft.Report.Gauge.Collections;

public class StiGaugeElementCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	private readonly StiScaleBase scale;

	private readonly StiGaugeElemenType scaleType;

	public bool IsReadOnly => false;

	public StiGaugeElement this[int index]
	{
		get
		{
			if (index < 0 || index >= base.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", "InvalidArgument");
			}
			return base.InnerList[index] as StiGaugeElement;
		}
		set
		{
			value.Scale = scale;
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
		foreach (StiGaugeElement item in base.List)
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
			StiGaugeElement stiGaugeElement = StiOptions.Services.GaugeElements.FirstOrDefault((StiGaugeElement x) => x.GetType().Name == ident);
			if (stiGaugeElement == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiGaugeElement stiGaugeElement2 = stiGaugeElement.CreateNew();
			stiGaugeElement2.Scale = stiGaugeElement.Scale;
			Add(stiGaugeElement2);
			stiGaugeElement2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiGaugeElementCollection stiGaugeElementCollection = new StiGaugeElementCollection(scale);
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiGaugeElement stiGaugeElement = (StiGaugeElement)enumerator.Current;
					stiGaugeElementCollection.Add(stiGaugeElement.Clone() as StiGaugeElement);
				}
				return stiGaugeElementCollection;
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

	public List<StiGaugeElement> ToArray()
	{
		List<StiGaugeElement> list = new List<StiGaugeElement>();
		foreach (StiGaugeElement inner in base.InnerList)
		{
			list.Add(inner);
		}
		return list;
	}

	private void AddCore(StiGaugeElement element)
	{
		if (element.ElementType == scaleType || element.ElementType == StiGaugeElemenType.All)
		{
			element.Scale = scale;
			base.InnerList.Add(element);
		}
	}

	public void Add(StiGaugeElement element)
	{
		AddCore(element);
	}

	public void AddRange(StiGaugeElement[] elements)
	{
		foreach (StiGaugeElement element in elements)
		{
			AddCore(element);
		}
	}

	public void Insert(int index, StiGaugeElement element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("item");
		}
		if (index < 0 || index > base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		AddCore(element);
	}

	public bool Remove(StiGaugeElement element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1)
		{
			element.Scale = null;
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public bool Contains(StiGaugeElement element)
	{
		return IndexOf(element) != -1;
	}

	public void CopyTo(StiGaugeElement[] elements, int arrayIndex)
	{
		base.InnerList.CopyTo(elements, arrayIndex);
	}

	public int IndexOf(StiGaugeElement element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return base.InnerList.IndexOf(element);
	}

	internal void SetItemInternal(int index, StiGaugeElement element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("value");
		}
		if (index < 0 || index >= base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		if (element.ElementType == scaleType || element.ElementType == StiGaugeElemenType.All)
		{
			element.Scale = scale;
			base.InnerList[index] = element;
		}
	}

	public bool MoveUp(StiGaugeElement element)
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

	public bool MoveDown(StiGaugeElement element)
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

	public StiGaugeElementCollection(StiScaleBase scale)
	{
		scaleType = scale.ScaleType;
		this.scale = scale;
	}
}
