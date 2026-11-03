using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDataRelationsCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	private Hashtable cachedDataRelations = new Hashtable();

	private StiDictionary dictionary;

	public StiDataRelation this[int index]
	{
		get
		{
			return (StiDataRelation)base.List[index];
		}
		set
		{
			base.List[index] = value;
			cachedDataRelations.Clear();
		}
	}

	public StiDataRelation this[string relationName]
	{
		get
		{
			if (cachedDataRelations[relationName] is StiDataRelation result)
			{
				return result;
			}
			StiDataRelation[] items = Items;
			foreach (StiDataRelation stiDataRelation in items)
			{
				if (stiDataRelation.NameInSource == relationName)
				{
					cachedDataRelations[relationName] = stiDataRelation;
					return stiDataRelation;
				}
				if (StiNameValidator.CorrectName(stiDataRelation.NameInSource, dictionary?.Report) == relationName)
				{
					cachedDataRelations[relationName] = stiDataRelation;
					return stiDataRelation;
				}
			}
			return null;
		}
		set
		{
			for (int i = 0; i < base.List.Count; i++)
			{
				StiDataRelation stiDataRelation = base.List[i] as StiDataRelation;
				if (stiDataRelation.Name == relationName)
				{
					base.List[i] = value;
					cachedDataRelations.Clear();
					return;
				}
			}
			Add(value);
		}
	}

	public StiDataRelation[] Items => (StiDataRelation[])base.InnerList.ToArray(typeof(StiDataRelation));

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
		foreach (StiDataRelation item in base.List)
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
			StiDataRelation stiDataRelation = new StiDataRelation
			{
				Dictionary = dictionary
			};
			stiDataRelation.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiDataRelation);
		}
	}

	public List<StiDataRelation> ToList()
	{
		return this.Cast<StiDataRelation>().ToList();
	}

	public void Add(StiDataRelation relation)
	{
		if (dictionary != null)
		{
			relation.Dictionary = dictionary;
		}
		base.List.Add(relation);
	}

	public void AddRange(StiDataRelation[] relations)
	{
		foreach (StiDataRelation relation in relations)
		{
			Add(relation);
		}
	}

	public void AddRange(StiDataRelationsCollection relations)
	{
		foreach (StiDataRelation relation in relations)
		{
			Add(relation);
		}
	}

	public bool Contains(StiDataRelation relation)
	{
		return base.List.Contains(relation);
	}

	public bool Contains(string name)
	{
		return ToList().FirstOrDefault((StiDataRelation x) => x.Name == name) != null;
	}

	public int IndexOf(StiDataRelation relation)
	{
		return base.List.IndexOf(relation);
	}

	public void Insert(int index, StiDataRelation relation)
	{
		base.List.Insert(index, relation);
	}

	public void Remove(StiDataRelation relation)
	{
		base.List.Remove(relation);
		cachedDataRelations.Clear();
	}

	public object Clone()
	{
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(dictionary);
		foreach (StiDataRelation item in base.List)
		{
			stiDataRelationsCollection.Add((StiDataRelation)item.Clone());
		}
		return stiDataRelationsCollection;
	}

	protected override void OnClear()
	{
		base.OnClear();
		cachedDataRelations?.Clear();
	}

	public StiDataRelationsCollection(StiDictionary dictionary)
	{
		this.dictionary = dictionary;
	}
}
