using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge;

namespace Stimulsoft.Report.Gauge.Collections;

public class StiBarRangeListCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	private StiBarRangeListType barType;

	public bool IsReadOnly => false;

	public virtual StiIndicatorRangeInfo this[int index]
	{
		get
		{
			if (index < 0 || index >= base.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", "InvalidArgument");
			}
			return base.InnerList[index] as StiIndicatorRangeInfo;
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
		foreach (StiIndicatorRangeInfo item in base.List)
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
			StiIndicatorRangeInfo stiIndicatorRangeInfo = StiOptions.Services.IndicatorRanges.FirstOrDefault((StiIndicatorRangeInfo x) => x.GetType().Name == ident);
			if (stiIndicatorRangeInfo == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiIndicatorRangeInfo stiIndicatorRangeInfo2 = stiIndicatorRangeInfo.CreateNew();
			Add(stiIndicatorRangeInfo2);
			stiIndicatorRangeInfo2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiBarRangeListCollection stiBarRangeListCollection = new StiBarRangeListCollection(barType);
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiIndicatorRangeInfo stiIndicatorRangeInfo = (StiIndicatorRangeInfo)enumerator.Current;
					stiBarRangeListCollection.Add(stiIndicatorRangeInfo.Clone() as StiIndicatorRangeInfo);
				}
				return stiBarRangeListCollection;
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

	public void Add(StiIndicatorRangeInfo element)
	{
		AddCore(element);
	}

	public void AddRange(StiIndicatorRangeInfo[] elements)
	{
		foreach (StiIndicatorRangeInfo element in elements)
		{
			AddCore(element);
		}
	}

	private void AddCore(StiIndicatorRangeInfo element)
	{
		if (element.RangeListType == barType)
		{
			base.InnerList.Add(element);
		}
	}

	public void Insert(int index, StiIndicatorRangeInfo element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("item");
		}
		if (index < 0 || index > base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		if (element.RangeListType == barType)
		{
			base.InnerList.Insert(index, element);
		}
	}

	public bool Remove(StiIndicatorRangeInfo element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1)
		{
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public bool Contains(StiIndicatorRangeInfo element)
	{
		return IndexOf(element) != -1;
	}

	public void CopyTo(StiIndicatorRangeInfo[] elements, int arrayIndex)
	{
		base.InnerList.CopyTo(elements, arrayIndex);
	}

	public int IndexOf(StiIndicatorRangeInfo element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return base.InnerList.IndexOf(element);
	}

	internal void SetItemInternal(int index, StiIndicatorRangeInfo element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("value");
		}
		if (index < 0 || index >= base.InnerList.Count)
		{
			throw new ArgumentOutOfRangeException("index", "InvalidArgument");
		}
		if (element.RangeListType == barType)
		{
			base.InnerList[index] = element;
		}
	}

	public bool MoveUp(StiIndicatorRangeInfo element)
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

	public bool MoveDown(StiIndicatorRangeInfo element)
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

	public StiBarRangeListCollection(StiBarRangeListType barType)
	{
		this.barType = barType;
	}
}
