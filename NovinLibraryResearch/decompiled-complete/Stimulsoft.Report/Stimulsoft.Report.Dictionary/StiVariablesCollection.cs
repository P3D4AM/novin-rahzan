using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary;

public class StiVariablesCollection : CollectionBase, ICloneable, IComparer
{
	private int directionFactor = 1;

	public StiVariable this[int index]
	{
		get
		{
			return (StiVariable)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public StiVariable this[string name]
	{
		get
		{
			name = name.ToLowerInvariant();
			foreach (StiVariable item in base.List)
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
				StiVariable stiVariable = base.List[i] as StiVariable;
				if (stiVariable.Name.ToLowerInvariant() == name)
				{
					base.List[i] = value;
					return;
				}
			}
			Add(value);
		}
	}

	public StiVariable[] Items => (StiVariable[])base.InnerList.ToArray(typeof(StiVariable));

	public JObject SaveToJsonObjectEx(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (base.List.Count == 0)
		{
			return null;
		}
		JObject val = new JObject();
		int num = 0;
		foreach (StiVariable item in base.List)
		{
			JsonExtensions.AddPropertyJObject(val, num.ToString(), item.SaveToJsonObjectEx());
			num++;
		}
		return val;
	}

	public void LoadFromJsonObjectEx(JObject jObject, StiReport report)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			StiVariable stiVariable = new StiVariable();
			stiVariable.LoadFromJsonObjectEx((JObject)item.Value, report);
			base.List.Add(stiVariable);
		}
	}

	int IComparer.Compare(object x, object y)
	{
		StiVariable stiVariable = x as StiVariable;
		StiVariable stiVariable2 = y as StiVariable;
		int num = stiVariable.Category.CompareTo(stiVariable2.Category) * directionFactor;
		if (num != 0)
		{
			return num;
		}
		if (!StiOptions.Designer.SortDictionaryByAliases)
		{
			return stiVariable.Name.CompareTo(stiVariable2.Name) * directionFactor;
		}
		return stiVariable.Alias.CompareTo(stiVariable2.Alias) * directionFactor;
	}

	public void Sort()
	{
		Sort(StiSortOrder.Asc);
	}

	public void Sort(StiSortOrder order)
	{
		directionFactor = ((order == StiSortOrder.Asc) ? 1 : (-1));
		base.InnerList.Sort(this);
	}

	public List<StiVariable> ToList()
	{
		return this.Cast<StiVariable>().ToList();
	}

	public StiVariable Add(string name, object value)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(name, value));
	}

	public StiVariable Add(string category, string name, object value)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(category, name, value));
	}

	public StiVariable Add(string category, string name, string alias, object value)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(category, name, alias, value));
	}

	public StiVariable Add(string category, string name, string alias, object value, bool readOnly)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(category, name, alias, value, readOnly));
	}

	public StiVariable Add(string category, string name, string alias, Type type, string value, bool readOnly)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(category, name, alias, type, value, readOnly));
	}

	public StiVariable Add(string category)
	{
		return AddInternal(new StiVariable(category));
	}

	public StiVariable Add(string category, string name, Type type, string value, bool readOnly)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(category, name, type, value, readOnly));
	}

	public StiVariable Add(string category, string name, Type type)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(category, name, type));
	}

	public StiVariable Add(string name, Type type)
	{
		if (name.Length > 0 && Contains(name))
		{
			Remove(name);
		}
		return AddInternal(new StiVariable(name, type));
	}

	public StiVariable Add(StiVariable variable)
	{
		if (variable.Name.Length > 0 && Contains(variable.Name))
		{
			Remove(variable.Name);
		}
		return AddInternal(variable);
	}

	public void AddRange(StiVariable[] variables)
	{
		foreach (StiVariable stiVariable in variables)
		{
			if (stiVariable.Name.Length > 0 && Contains(stiVariable.Name))
			{
				Remove(stiVariable.Name);
			}
			AddInternal(stiVariable);
		}
	}

	public void AddRange(StiVariablesCollection variables)
	{
		foreach (StiVariable variable in variables)
		{
			Add(variable);
		}
	}

	private StiVariable AddInternal(StiVariable variable)
	{
		if (variable.IsCategory)
		{
			foreach (StiVariable item in base.List)
			{
				if (item.IsCategory && item.Category == variable.Category)
				{
					throw new Exception("Category '" + variable.Category + "' already exists!");
				}
			}
		}
		base.List.Add(variable);
		return variable;
	}

	public bool Contains(StiVariable variable)
	{
		return base.List.Contains(variable);
	}

	public bool Contains(string name)
	{
		name = name.ToLowerInvariant();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiVariable stiVariable = (StiVariable)enumerator.Current;
				if (stiVariable.Name.ToLowerInvariant() == name)
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

	public bool ContainsCategory(string name)
	{
		name = name.ToLowerInvariant();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiVariable stiVariable = (StiVariable)enumerator.Current;
				if (stiVariable.Category.ToLowerInvariant() == name)
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

	public int IndexOf(StiVariable variable)
	{
		return base.List.IndexOf(variable);
	}

	public int IndexOf(string name)
	{
		name = name.ToLowerInvariant();
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiVariable stiVariable = (StiVariable)enumerator.Current;
				if (stiVariable.Name.ToLowerInvariant() == name)
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

	public void Insert(int index, StiVariable variable)
	{
		base.List.Insert(index, variable);
	}

	public void Remove(StiVariable variable)
	{
		if (variable.Category.Length > 0 && GetVariablesCount(variable.Category) == 1)
		{
			variable.Name = "";
		}
		else
		{
			base.List.Remove(variable);
		}
	}

	public void Remove(string name)
	{
		name = name.ToLowerInvariant();
		int num = 0;
		while (num < base.List.Count)
		{
			StiVariable stiVariable = base.List[num] as StiVariable;
			if (stiVariable.Name.ToLowerInvariant() == name)
			{
				base.List.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	public object Clone()
	{
		StiVariablesCollection stiVariablesCollection = new StiVariablesCollection();
		foreach (StiVariable item in base.List)
		{
			stiVariablesCollection.Add((StiVariable)item.Clone());
		}
		return stiVariablesCollection;
	}

	public void MoveCategoryTo(string fromCategory, string toCategory)
	{
		if (fromCategory == toCategory)
		{
			return;
		}
		int firstCategoryIndex = GetFirstCategoryIndex(fromCategory);
		int firstCategoryIndex2 = GetFirstCategoryIndex(toCategory);
		if (firstCategoryIndex == firstCategoryIndex2)
		{
			return;
		}
		List<StiVariable> list = new List<StiVariable>();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiVariable stiVariable = (StiVariable)enumerator.Current;
				if (stiVariable.Category == fromCategory)
				{
					list.Add(stiVariable);
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
		RemoveCategory(fromCategory);
		int num = ((firstCategoryIndex > firstCategoryIndex2) ? GetFirstCategoryIndex(toCategory) : (GetLastCategoryIndex(toCategory) + 1));
		foreach (StiVariable item in list)
		{
			Insert(num, item);
			num++;
		}
	}

	public int GetFirstCategoryIndex(string category)
	{
		int num = 0;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiVariable stiVariable = (StiVariable)enumerator.Current;
				if (category == stiVariable.Category)
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

	public int GetLastCategoryIndex(string category)
	{
		int result = -1;
		int num = 0;
		bool flag = false;
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				StiVariable stiVariable = (StiVariable)enumerator.Current;
				if (category == stiVariable.Category)
				{
					result = num;
					flag = true;
				}
				else if (flag)
				{
					break;
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
		return result;
	}

	public void RenameCategory(string oldName, string newName)
	{
		oldName = oldName.ToLower(CultureInfo.InvariantCulture);
		foreach (StiVariable item in base.List)
		{
			if (item.Category.ToLowerInvariant() == oldName)
			{
				item.Category = newName;
			}
		}
	}

	public void RemoveCategory(string category)
	{
		int num = 0;
		while (num < Items.Length)
		{
			if (Items[num].Category == category)
			{
				RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	public int GetVariablesCount(string category)
	{
		int num = 0;
		foreach (StiVariable item in base.List)
		{
			if (item.Category == category)
			{
				num++;
			}
		}
		return num;
	}

	public string GenerateEmptyCategoryName()
	{
		string text = StiLocalization.Get("PropertyMain", "Category");
		int num = 1;
		string text2;
		while (true)
		{
			text2 = text + num;
			if (!ContainsCategory(text2))
			{
				break;
			}
			num++;
		}
		return text2;
	}
}
