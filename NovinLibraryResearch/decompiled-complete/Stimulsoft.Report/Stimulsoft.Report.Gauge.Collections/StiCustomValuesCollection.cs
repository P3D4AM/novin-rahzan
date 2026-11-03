using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge;

namespace Stimulsoft.Report.Gauge.Collections;

public class StiCustomValuesCollection : CollectionBase, ICloneable
{
	public bool IsReadOnly => false;

	public StiCustomValueBase this[int index]
	{
		get
		{
			if (index < 0 || index >= base.List.Count)
			{
				throw new ArgumentOutOfRangeException("index", "InvalidArgument");
			}
			return base.InnerList[index] as StiCustomValueBase;
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
		foreach (StiCustomValueBase item in base.List)
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
			StiCustomValueBase stiCustomValueBase = StiOptions.Services.CustomValues.FirstOrDefault((StiCustomValueBase x) => x.GetType().Name == ident);
			if (stiCustomValueBase == null)
			{
				throw new Exception($"Type {ident} is not found!");
			}
			StiCustomValueBase stiCustomValueBase2 = stiCustomValueBase.CreateNew();
			Add(stiCustomValueBase2);
			stiCustomValueBase2.LoadFromJsonObject((JObject)item.Value);
		}
	}

	public object Clone()
	{
		StiCustomValuesCollection stiCustomValuesCollection = new StiCustomValuesCollection();
		lock (((ICollection)this).SyncRoot)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StiCustomValueBase stiCustomValueBase = (StiCustomValueBase)enumerator.Current;
					stiCustomValuesCollection.Add(stiCustomValueBase.Clone() as StiCustomValueBase);
				}
				return stiCustomValuesCollection;
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

	public void Add(StiCustomValueBase element)
	{
		base.InnerList.Add(element);
	}

	public void AddRange(StiCustomValueBase[] elements)
	{
		foreach (StiCustomValueBase value in elements)
		{
			base.InnerList.Add(value);
		}
	}

	public void Insert(int index, StiCustomValueBase element)
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

	public bool Remove(StiCustomValueBase element)
	{
		int num = base.InnerList.IndexOf(element);
		if (num != -1)
		{
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public bool Contains(StiCustomValueBase element)
	{
		return IndexOf(element) != -1;
	}

	public void CopyTo(StiCustomValueBase[] elements, int arrayIndex)
	{
		base.InnerList.CopyTo(elements, arrayIndex);
	}

	public int IndexOf(StiCustomValueBase element)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		return base.InnerList.IndexOf(element);
	}

	internal void SetItemInternal(int index, StiCustomValueBase element)
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

	public bool MoveUp(StiCustomValueBase element)
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

	public bool MoveDown(StiCustomValueBase element)
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
