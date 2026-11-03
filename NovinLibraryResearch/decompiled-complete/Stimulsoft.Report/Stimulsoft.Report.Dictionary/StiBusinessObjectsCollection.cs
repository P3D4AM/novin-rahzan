using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiBusinessObjectsCollection : CollectionBase, ICloneable, IComparer, IStiJsonReportObject
{
	private int directionFactor = 1;

	internal Hashtable cachedBusinessObjects;

	internal StiDictionary dictionary;

	internal StiBusinessObject parentBusinessObject;

	public StiBusinessObject this[int index]
	{
		get
		{
			return (StiBusinessObject)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal Hashtable CachedBusinessObjects
	{
		get
		{
			if (cachedBusinessObjects == null)
			{
				cachedBusinessObjects = new Hashtable();
			}
			return cachedBusinessObjects;
		}
	}

	public StiBusinessObject this[string name]
	{
		get
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			if (CachedBusinessObjects[name] is StiBusinessObject result)
			{
				return result;
			}
			foreach (StiBusinessObject item in base.List)
			{
				if (item.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					CachedBusinessObjects[name] = item;
					return item;
				}
				if (StiNameValidator.CorrectName(item.Name.ToLower(CultureInfo.InvariantCulture), dictionary?.Report) == StiNameValidator.CorrectName(name, dictionary?.Report))
				{
					cachedBusinessObjects[name] = item;
					return item;
				}
			}
			return null;
		}
		set
		{
			name = name.ToLower(CultureInfo.InvariantCulture);
			for (int i = 0; i < base.List.Count; i++)
			{
				StiBusinessObject stiBusinessObject = base.List[i] as StiBusinessObject;
				if (stiBusinessObject.Name.ToLower(CultureInfo.InvariantCulture) == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
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
		foreach (StiBusinessObject item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiBusinessObject stiBusinessObject = new StiBusinessObject();
			stiBusinessObject.Dictionary = dictionary;
			stiBusinessObject.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiBusinessObject);
		}
	}

	int IComparer.Compare(object x, object y)
	{
		StiBusinessObject stiBusinessObject = x as StiBusinessObject;
		StiBusinessObject stiBusinessObject2 = y as StiBusinessObject;
		if (StiOptions.Designer.SortDictionaryByAliases)
		{
			return stiBusinessObject.Alias.CompareTo(stiBusinessObject2.Alias) * directionFactor;
		}
		return stiBusinessObject.Name.CompareTo(stiBusinessObject2.Name) * directionFactor;
	}

	public List<StiBusinessObject> ToList()
	{
		return this.Cast<StiBusinessObject>().ToList();
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		base.OnSet(index, oldValue, newValue);
		StiBusinessObject stiBusinessObject = newValue as StiBusinessObject;
		if (dictionary != null)
		{
			stiBusinessObject.Dictionary = dictionary;
		}
		if (parentBusinessObject != null)
		{
			stiBusinessObject.ParentBusinessObject = parentBusinessObject;
		}
	}

	protected override void OnInsert(int index, object value)
	{
		base.OnInsert(index, value);
		StiBusinessObject stiBusinessObject = value as StiBusinessObject;
		if (dictionary != null)
		{
			stiBusinessObject.Dictionary = dictionary;
		}
		if (parentBusinessObject != null)
		{
			stiBusinessObject.ParentBusinessObject = parentBusinessObject;
		}
	}

	public void Add(StiBusinessObject source)
	{
		base.List.Add(source);
	}

	public void AddRange(StiBusinessObject[] sources)
	{
		base.InnerList.AddRange(sources);
	}

	public void AddRange(StiBusinessObjectsCollection sources)
	{
		foreach (StiBusinessObject source in sources)
		{
			Add(source);
		}
	}

	public bool Contains(StiBusinessObject source)
	{
		return base.List.Contains(source);
	}

	public bool Contains(string name)
	{
		name = name.ToLowerInvariant();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiBusinessObject stiBusinessObject = (StiBusinessObject)enumerator.Current;
				if (stiBusinessObject.Name.ToLowerInvariant() == name)
				{
					return true;
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
		return false;
	}

	public int IndexOf(StiBusinessObject source)
	{
		return base.List.IndexOf(source);
	}

	public void Insert(int index, StiBusinessObject source)
	{
		base.List.Insert(index, source);
	}

	protected override void OnClear()
	{
		base.OnClear();
		if (cachedBusinessObjects != null)
		{
			cachedBusinessObjects.Clear();
			cachedBusinessObjects = null;
		}
	}

	public void Remove(StiBusinessObject source)
	{
		base.List.Remove(source);
		string key = source.Name.ToLower(CultureInfo.InvariantCulture);
		if (CachedBusinessObjects[key] != null)
		{
			CachedBusinessObjects.Remove(key);
		}
	}

	public object Clone()
	{
		StiBusinessObjectsCollection stiBusinessObjectsCollection = new StiBusinessObjectsCollection(dictionary, parentBusinessObject);
		foreach (StiBusinessObject item in base.List)
		{
			stiBusinessObjectsCollection.Add((StiBusinessObject)item.Clone());
		}
		return stiBusinessObjectsCollection;
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
				StiBusinessObject stiBusinessObject = (StiBusinessObject)enumerator.Current;
				stiBusinessObject.Columns.Sort(order);
				StiBusinessObjectsCollection businessObjects = stiBusinessObject.BusinessObjects;
				if (businessObjects != null && businessObjects.Count > 0)
				{
					businessObjects.Sort(order);
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

	public void Connect()
	{
		foreach (StiBusinessObject item in base.List)
		{
			item.Connect();
		}
	}

	public void Disconnect()
	{
		foreach (StiBusinessObject item in base.List)
		{
			item.Disconnect();
		}
	}

	public StiBusinessObjectsCollection(StiDictionary dictionary, StiBusinessObject parentBusinessObject)
	{
		this.dictionary = dictionary;
		this.parentBusinessObject = parentBusinessObject;
	}
}
