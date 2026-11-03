using System;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report;

public class StiStyleConditionsCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiStyleCondition this[int index]
	{
		get
		{
			return (StiStyleCondition)base.List[index];
		}
		set
		{
			base.List[index] = value;
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
		foreach (StiStyleCondition item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiStyleCondition stiStyleCondition = new StiStyleCondition();
			stiStyleCondition.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiStyleCondition);
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiStyleConditionsCollection stiStyleConditionsCollection) || base.Count != stiStyleConditionsCollection.Count)
		{
			return false;
		}
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiStyleCondition stiStyleCondition = (StiStyleCondition)enumerator.Current;
				StiStyleCondition obj2 = stiStyleConditionsCollection[num++];
				if (!stiStyleCondition.Equals(obj2))
				{
					return false;
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
		return true;
	}

	public object Clone()
	{
		StiStyleConditionsCollection stiStyleConditionsCollection = new StiStyleConditionsCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiStyleCondition stiStyleCondition = (StiStyleCondition)enumerator.Current;
				stiStyleConditionsCollection.Add(stiStyleCondition.Clone() as StiStyleCondition);
			}
			return stiStyleConditionsCollection;
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

	public void Add(StiStyleCondition condition)
	{
		base.List.Add(condition);
	}

	public void Add(StiStyleConditionElement[] elements)
	{
		StiStyleCondition stiStyleCondition = new StiStyleCondition();
		stiStyleCondition.FromElements(elements);
		Add(stiStyleCondition);
	}

	public void AddRange(StiStyleConditionsCollection conditions)
	{
		foreach (StiStyleCondition condition in conditions)
		{
			Add(condition);
		}
	}

	public void AddRange(StiStyleCondition[] conditions)
	{
		foreach (StiStyleCondition condition in conditions)
		{
			Add(condition);
		}
	}

	public bool Contains(StiStyleCondition condition)
	{
		return base.List.Contains(condition);
	}

	public int IndexOf(StiStyleCondition condition)
	{
		return base.List.IndexOf(condition);
	}

	public void Insert(int index, StiStyleCondition condition)
	{
		base.List.Insert(index, condition);
	}

	public void Remove(StiStyleCondition condition)
	{
		base.List.Remove(condition);
	}
}
