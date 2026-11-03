using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Components;

public class StiConditionsCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiBaseCondition this[int index]
	{
		get
		{
			return (StiBaseCondition)base.List[index];
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
		foreach (StiBaseCondition item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiBaseCondition stiBaseCondition = null;
			JObject val = (JObject)item.Value;
			switch (val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>())
			{
			case "StiBaseCondition":
				stiBaseCondition = new StiBaseCondition();
				break;
			case "StiCondition":
				stiBaseCondition = new StiCondition();
				break;
			case "StiIconSetCondition":
				stiBaseCondition = new StiIconSetCondition();
				break;
			case "StiColorScaleCondition":
				stiBaseCondition = new StiColorScaleCondition();
				break;
			case "StiDataBarCondition":
				stiBaseCondition = new StiDataBarCondition();
				break;
			case "StiMultiCondition":
				stiBaseCondition = new StiMultiCondition();
				break;
			}
			stiBaseCondition.LoadFromJsonObject(val);
			base.List.Add(stiBaseCondition);
		}
	}

	public object Clone()
	{
		StiConditionsCollection stiConditionsCollection = new StiConditionsCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiBaseCondition stiBaseCondition = (StiBaseCondition)enumerator.Current;
				stiConditionsCollection.Add(stiBaseCondition.Clone() as StiBaseCondition);
			}
			return stiConditionsCollection;
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

	public void Add(StiBaseCondition condition)
	{
		base.List.Add(condition);
	}

	public void AddRange(StiConditionsCollection conditions)
	{
		foreach (StiBaseCondition condition in conditions)
		{
			Add(condition);
		}
	}

	public void AddRange(StiConditionsCollection conditions, bool addOnlyNotEqual)
	{
		foreach (StiBaseCondition condition in conditions)
		{
			if (base.Count == 0)
			{
				Add(condition);
				continue;
			}
			{
				IEnumerator enumerator2 = GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						StiBaseCondition stiBaseCondition2 = (StiBaseCondition)enumerator2.Current;
						if (!stiBaseCondition2.Equals(condition))
						{
							Add(condition);
							break;
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator2 as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}

	public void AddRange(StiBaseCondition[] conditions)
	{
		foreach (StiBaseCondition condition in conditions)
		{
			Add(condition);
		}
	}

	public bool Contains(StiBaseCondition condition)
	{
		return base.List.Contains(condition);
	}

	public int IndexOf(StiBaseCondition condition)
	{
		return base.List.IndexOf(condition);
	}

	public void Insert(int index, StiBaseCondition condition)
	{
		base.List.Insert(index, condition);
	}

	public void Remove(StiBaseCondition condition)
	{
		base.List.Remove(condition);
	}
}
