using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDataSourcesCollection : CollectionBase, ICloneable, IComparer, IStiJsonReportObject
{
	private int directionFactor = 1;

	private Hashtable cachedDataSources;

	private StiDictionary dictionary;

	public StiDataSource this[int index]
	{
		get
		{
			return (StiDataSource)base.List[index];
		}
		set
		{
			if (dictionary != null)
			{
				value.Dictionary = dictionary;
			}
			base.List[index] = value;
		}
	}

	internal Hashtable CachedDataSources => cachedDataSources ?? (cachedDataSources = new Hashtable());

	public StiDataSource this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			if (CachedDataSources[name] is StiDataSource result)
			{
				return result;
			}
			foreach (StiDataSource item in base.List)
			{
				if (item.Name.ToLowerInvariant() == name)
				{
					CachedDataSources[name] = item;
					return item;
				}
				if (StiNameValidator.CorrectName(item.Name.ToLowerInvariant(), dictionary?.Report) == StiNameValidator.CorrectName(name, dictionary?.Report))
				{
					CachedDataSources[name] = item;
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
				StiDataSource stiDataSource = base.List[i] as StiDataSource;
				if (stiDataSource.Name.ToLowerInvariant() == name)
				{
					if (dictionary != null)
					{
						value.Dictionary = dictionary;
					}
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public StiDataSource[] Items => (StiDataSource[])base.InnerList.ToArray(typeof(StiDataSource));

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
		foreach (StiDataSource item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		List<StiDataSource> dataSource = StiOptions.Services.DataSource;
		foreach (JProperty item in jObject.Properties())
		{
			JObject val = (JObject)item.Value;
			JProperty val2 = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident"));
			string identName = val2.Value.ToObject<string>();
			StiDataSource stiDataSource = dataSource.FirstOrDefault((StiDataSource x) => x.GetType().Name == identName)?.CreateNew();
			if (stiDataSource != null)
			{
				stiDataSource.Dictionary = dictionary;
				stiDataSource.LoadFromJsonObject(val);
				base.List.Add(stiDataSource);
			}
		}
	}

	int IComparer.Compare(object x, object y)
	{
		StiDataSource stiDataSource = x as StiDataSource;
		StiDataSource stiDataSource2 = y as StiDataSource;
		if (StiOptions.Designer.SortDictionaryByAliases)
		{
			return string.Compare(stiDataSource.Alias, stiDataSource2.Alias, StringComparison.Ordinal) * directionFactor;
		}
		return string.Compare(stiDataSource.Name, stiDataSource2.Name, StringComparison.Ordinal) * directionFactor;
	}

	public object Clone()
	{
		StiDataSourcesCollection stiDataSourcesCollection = new StiDataSourcesCollection(dictionary);
		foreach (StiDataSource item in base.List)
		{
			stiDataSourcesCollection.Add((StiDataSource)item.Clone());
		}
		return stiDataSourcesCollection;
	}

	public void Sort()
	{
		Sort(StiSortOrder.Asc);
	}

	public void Sort(StiSortOrder order)
	{
		Sort(order, sortColumns: true);
	}

	public void Sort(StiSortOrder order, bool sortColumns)
	{
		if (order == StiSortOrder.Asc)
		{
			directionFactor = 1;
		}
		else
		{
			directionFactor = -1;
		}
		base.InnerList.Sort(this);
		if (!sortColumns)
		{
			return;
		}
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiDataSource stiDataSource = (StiDataSource)enumerator.Current;
				stiDataSource.Columns.Sort(order);
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
	}

	public List<StiDataTransformation> FetchAllDataTransformations()
	{
		return (from c in ToList()
			where c is StiDataTransformation
			select c).Cast<StiDataTransformation>().ToList();
	}

	public List<StiVirtualSource> FetchAllVirtualDataSources()
	{
		return (from c in ToList()
			where c is StiVirtualSource
			select c).Cast<StiVirtualSource>().ToList();
	}

	public List<StiCrossTabDataSource> FetchAllCrossTabDataSources()
	{
		return (from c in ToList()
			where c is StiCrossTabDataSource
			select c).Cast<StiCrossTabDataSource>().ToList();
	}

	public List<StiDataSource> ToList()
	{
		return this.Cast<StiDataSource>().ToList();
	}

	public void Add(StiDataSource dataSource)
	{
		if (dictionary != null)
		{
			dataSource.Dictionary = dictionary;
		}
		base.List.Add(dataSource);
	}

	public void AddRange(StiDataSource[] dataSources)
	{
		base.InnerList.AddRange(dataSources);
	}

	public void AddRange(StiDataSourcesCollection dataSources)
	{
		foreach (StiDataSource dataSource in dataSources)
		{
			Add(dataSource);
		}
	}

	public bool Contains(StiDataSource dataSource)
	{
		return base.List.Contains(dataSource);
	}

	public bool Contains(string name)
	{
		return this[name] != null;
	}

	public int IndexOf(StiDataSource dataSource)
	{
		return base.List.IndexOf(dataSource);
	}

	public void Insert(int index, StiDataSource dataSource)
	{
		base.List.Insert(index, dataSource);
	}

	protected override void OnClear()
	{
		base.OnClear();
		if (dictionary != null)
		{
			dictionary.Relations.Clear();
		}
		if (cachedDataSources != null)
		{
			cachedDataSources.Clear();
			cachedDataSources = null;
		}
	}

	public void Remove(StiDataSource dataSource)
	{
		base.List.Remove(dataSource);
		if (dictionary != null)
		{
			int num = 0;
			while (num < dictionary.Relations.Count)
			{
				if (dictionary.Relations[num].ParentSource == dataSource || dictionary.Relations[num].ChildSource == dataSource)
				{
					dictionary.Relations.Remove(dictionary.Relations[num]);
				}
				else
				{
					num++;
				}
			}
		}
		string key = dataSource.Name.ToLowerInvariant();
		if (CachedDataSources[key] != null)
		{
			CachedDataSources.Remove(key);
		}
	}

	public void ClearParametersExpression()
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiDataSource stiDataSource = (StiDataSource)enumerator.Current;
				if (!(stiDataSource is StiSqlSource stiSqlSource))
				{
					continue;
				}
				foreach (StiDataParameter parameter in stiSqlSource.Parameters)
				{
					parameter.Expression = string.Empty;
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
	}

	public void Connect(bool loadData)
	{
		Connect(null, loadData);
	}

	public void Connect(StiDataCollection datas, bool loadData)
	{
		StiDataSource[] array = new StiDataSource[base.Count];
		Items.CopyTo(array, 0);
		Array.Sort(array, new StiConnectionOrderComparer());
		StiDataSource[] array2 = array;
		foreach (StiDataSource stiDataSource in array2)
		{
			if (stiDataSource.ConnectionOrder != 0 && stiDataSource.ConnectOnStart && (loadData || stiDataSource is StiDataTableSource))
			{
				StiDataLeader.Connect(stiDataSource, datas, loadData);
			}
		}
	}

	public void Disconnect()
	{
		foreach (StiDataSource inner in base.InnerList)
		{
			StiDataLeader.Disconnect(inner);
		}
	}

	public StiDataSourcesCollection(StiDictionary dictionary)
	{
		this.dictionary = dictionary;
	}
}
