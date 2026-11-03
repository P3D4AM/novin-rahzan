using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDatabaseCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiDatabase this[int index]
	{
		get
		{
			return (StiDatabase)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiDatabase this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			foreach (StiDatabase item in base.List)
			{
				if (item.Name.ToLowerInvariant() == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLowerInvariant();
			for (int i = 0; i < base.List.Count; i++)
			{
				StiDatabase stiDatabase = base.List[i] as StiDatabase;
				if (stiDatabase.Name.ToLowerInvariant() == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public StiDatabase[] Items => (StiDatabase[])base.InnerList.ToArray(typeof(StiDatabase));

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiDatabase stiDatabase = (StiDatabase)enumerator.Current;
				JsonExtensions.AddPropertyJObject(val, num.ToString(), stiDatabase.SaveToJsonObject(mode));
				num++;
			}
			return val;
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

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			string ident = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
			StiDatabase stiDatabase = StiOptions.Services.Databases.FirstOrDefault((StiDatabase x) => ((object)x).GetType().Name == ident);
			if (stiDatabase != null)
			{
				StiDatabase stiDatabase2 = stiDatabase.CreateNew();
				stiDatabase2.LoadFromJsonObject((JObject)item.Value);
				base.List.Add(stiDatabase2);
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public List<StiDatabase> ToList()
	{
		return this.Cast<StiDatabase>().ToList();
	}

	public void Add(StiDatabase data)
	{
		base.List.Add(data);
	}

	public void AddRange(StiDatabase[] datas)
	{
		base.InnerList.AddRange(datas);
	}

	public void AddRange(StiDatabaseCollection datas)
	{
		foreach (StiDatabase data in datas)
		{
			Add(data);
		}
	}

	public bool Contains(StiDatabase data)
	{
		return base.List.Contains(data);
	}

	public bool Contains(string name)
	{
		return ToList().FirstOrDefault((StiDatabase x) => x.Name == name) != null;
	}

	public int IndexOf(StiDatabase data)
	{
		return base.List.IndexOf(data);
	}

	public int IndexOf(string name)
	{
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiDatabase stiDatabase = (StiDatabase)enumerator.Current;
				if (name == stiDatabase.Name)
				{
					return num;
				}
				num++;
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
		return -1;
	}

	public void Insert(int index, StiDatabase data)
	{
		base.List.Insert(index, data);
	}

	public void Remove(StiDatabase data)
	{
		base.List.Remove(data);
	}

	public void Remove(string name)
	{
		int num = IndexOf(name);
		if (num != -1)
		{
			base.List.Remove(num);
			return;
		}
		throw new Exception("Database '" + name + "' is not found");
	}
}
