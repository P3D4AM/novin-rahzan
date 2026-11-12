using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public class StiAppExpressionCollection : CollectionBase, ICloneable, IStiJsonReportObject
{
	public StiAppExpression this[int index]
	{
		get
		{
			return (StiAppExpression)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiAppExpression this[string name]
	{
		get
		{
			foreach (StiAppExpression item in base.List)
			{
				if (item.Name == name)
				{
					return item;
				}
			}
			return null;
		}
		set
		{
			for (int i = 0; i < base.List.Count; i++)
			{
				StiAppExpression stiAppExpression = base.List[i] as StiAppExpression;
				if (stiAppExpression.Name == name)
				{
					base.List[i] = value;
					return;
				}
			}
			base.List.Add(value);
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject jObject = new JObject();
		int num = 0;
		foreach (StiAppExpression item in base.List)
		{
			jObject.AddPropertyJObject(num.ToString(), item.SaveToJsonObject(mode));
			num++;
		}
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			StiAppExpression stiAppExpression = new StiAppExpression(null, null);
			stiAppExpression.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiAppExpression);
		}
	}

	public void Add(string name, string expression)
	{
		base.List.Add(new StiAppExpression(name, expression));
	}

	public void Add(StiAppExpression prop)
	{
		base.List.Add(prop);
	}

	public void AddRange(StiAppExpression[] props)
	{
		foreach (StiAppExpression prop in props)
		{
			Add(prop);
		}
	}

	public void AddRange(StiAppExpressionCollection props)
	{
		foreach (StiAppExpression prop in props)
		{
			Add(prop);
		}
	}

	public bool Contains(StiAppExpression prop)
	{
		return base.List.Contains(prop);
	}

	public bool Contains(string name)
	{
		return this[name] != null;
	}

	public int IndexOf(StiAppExpression prop)
	{
		return base.List.IndexOf(prop);
	}

	public void Insert(int index, StiAppExpression prop)
	{
		base.List.Insert(index, prop);
	}

	public void Remove(StiAppExpression prop)
	{
		base.List.Remove(prop);
	}

	public void Remove(string name)
	{
		StiAppExpression stiAppExpression = this[name];
		if (stiAppExpression != null)
		{
			Remove(stiAppExpression);
		}
	}

	public IEnumerable<StiAppExpression> ToList()
	{
		return base.List.Cast<StiAppExpression>();
	}

	public object Clone()
	{
		StiAppExpressionCollection stiAppExpressionCollection = new StiAppExpressionCollection();
		foreach (StiAppExpression item in base.List)
		{
			stiAppExpressionCollection.Add((StiAppExpression)item.Clone());
		}
		return stiAppExpressionCollection;
	}
}
