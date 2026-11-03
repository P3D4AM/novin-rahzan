using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dictionary;

public class StiDataColumnsCollection : CollectionBase, IComparer, IStiJsonReportObject
{
	private int directionFactor = 1;

	internal Hashtable CachedDataColumns { get; set; } = new Hashtable();


	internal StiBusinessObject BusinessObject { get; set; }

	internal StiDataSource DataSource { get; set; }

	public StiDataColumn this[int index]
	{
		get
		{
			return (StiDataColumn)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiDataColumn this[string name]
	{
		get
		{
			name = StiNameValidator.CorrectName(name.ToLowerInvariant(), DataSource?.Dictionary?.Report);
			if (CachedDataColumns[name] is StiDataColumn result)
			{
				return result;
			}
			foreach (StiDataColumn item in base.List)
			{
				if (StiNameValidator.CorrectName(item.Name.ToLowerInvariant(), DataSource?.Dictionary?.Report) == name)
				{
					CachedDataColumns[name] = item;
					return item;
				}
			}
			return null;
		}
		set
		{
			name = StiNameValidator.CorrectName(name.ToLowerInvariant(), DataSource?.Dictionary?.Report);
			for (int i = 0; i < base.List.Count; i++)
			{
				StiDataColumn stiDataColumn = base.List[i] as StiDataColumn;
				if (StiNameValidator.CorrectName(stiDataColumn.Name.ToLowerInvariant(), DataSource?.Dictionary?.Report) == name)
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
		foreach (StiDataColumn item in base.List)
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
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiDataColumn stiDataColumn = null;
			JObject val = (JObject)item.Value;
			JProperty val2 = val.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident"));
			stiDataColumn = ((val2 != null && val2.Value.ToObject<string>() == "Calc") ? new StiCalcDataColumn() : ((val2 == null || !(val2.Value.ToObject<string>() == "Transform")) ? new StiDataColumn() : new StiDataTransformationColumn()));
			stiDataColumn.LoadFromJsonObject((JObject)item.Value);
			base.List.Add(stiDataColumn);
		}
	}

	int IComparer.Compare(object x, object y)
	{
		StiDataColumn stiDataColumn = x as StiDataColumn;
		StiDataColumn stiDataColumn2 = y as StiDataColumn;
		if (StiOptions.Designer.SortDictionaryByAliases)
		{
			return stiDataColumn.Alias.CompareTo(stiDataColumn2.Alias) * directionFactor;
		}
		return stiDataColumn.Name.CompareTo(stiDataColumn2.Name) * directionFactor;
	}

	public List<StiDataColumn> ToList()
	{
		return this.Cast<StiDataColumn>().ToList();
	}

	protected override void OnInsert(int index, object value)
	{
		if (value is StiDataColumn stiDataColumn)
		{
			if (stiDataColumn.DataSource == null)
			{
				stiDataColumn.DataSource = DataSource;
			}
			if (stiDataColumn.BusinessObject == null)
			{
				stiDataColumn.BusinessObject = BusinessObject;
			}
			if (stiDataColumn.DataColumnsCollection == null)
			{
				stiDataColumn.DataColumnsCollection = this;
			}
		}
	}

	public void Add(string name, Type type)
	{
		Add(name, name, type);
	}

	public void Add(string name, string alias, Type type)
	{
		Add(new StiDataColumn(name, alias, type));
	}

	public void Add(StiDataColumn column)
	{
		base.List.Add(column);
	}

	public void AddRange(StiDataColumnsCollection columns)
	{
		foreach (StiDataColumn column in columns)
		{
			Add(column);
		}
	}

	public void AddRange(StiDataColumn[] columns)
	{
		foreach (StiDataColumn column in columns)
		{
			Add(column);
		}
	}

	public bool Contains(StiDataColumn column)
	{
		return base.List.Contains(column);
	}

	public bool Contains(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		name = StiNameValidator.CorrectName(name.ToLowerInvariant(), DataSource?.Dictionary?.Report);
		foreach (StiDataColumn item in base.List)
		{
			if (StiNameValidator.CorrectName(item.Name.ToLowerInvariant(), DataSource?.Dictionary?.Report) == name)
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(StiDataColumn column)
	{
		return base.List.IndexOf(column);
	}

	public void Insert(int index, StiDataColumn column)
	{
		base.List.Insert(index, column);
	}

	public void Remove(StiDataColumn column)
	{
		base.List.Remove(column);
		string key = column.Name.ToLowerInvariant();
		if (CachedDataColumns.Contains(key))
		{
			CachedDataColumns.Remove(key);
		}
	}

	public void Sort(StiSortOrder order)
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
	}

	public StiDataColumnsCollection()
	{
	}

	public StiDataColumnsCollection(List<StiDataColumn> columns)
	{
		columns.ForEach(Add);
	}

	public StiDataColumnsCollection(List<DataColumn> columns)
	{
		columns.Select((DataColumn c) => new StiDataColumn(c.ColumnName, c.DataType)).ToList().ForEach(Add);
	}

	public StiDataColumnsCollection(StiDataSource dataSource)
	{
		DataSource = dataSource;
	}

	public StiDataColumnsCollection(StiBusinessObject source)
	{
		BusinessObject = source;
	}
}
